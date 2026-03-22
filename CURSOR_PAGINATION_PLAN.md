# Cursor-Based Stats Pagination Plan

## Context

The player stats table currently loads all data at once (`limit=500`) from the backend and paginates entirely client-side. The backend has a keyset cursor (`SortKey:PlayerId`) that is bypassed by the frontend.

After researching how sports analytics APIs (BallDontLie NFL API, Redis pagination patterns) handle this, the industry-standard approach is a **Redis snapshot model**: on first request for a given filter combination, fetch and sort the full result set, store it in Redis as an ordered snapshot, then return pages using a simple Base64-encoded offset as the cursor. This cleanly decouples dynamic column sorting from the cursor format and is the correct approach for a stats leaderboard where:
- Data doesn't change mid-session
- Arbitrary column sorting is needed
- Per-position result sets are manageable (64–500 records)

The existing `SortKey` field and keyset cursor in `PlayerQueryRepository` will be **replaced** by this snapshot model.

---

## Architecture: Redis Snapshot Pagination

```
First request (position=QB, season=2025, seasonType=REG, sortBy=passYds, sortDir=desc):
  1. Check Redis for snapshot key  →  MISS
  2. Fetch ALL matching players from DB (no limit)
  3. Sort in-memory by sortBy/sortDir
  4. Serialize sorted list → store in Redis (TTL: 30min current / 24h past)
  5. Slice [0..24], encode cursor = Base64(25), return page + nextCursor + total

Subsequent request with cursor:
  1. Check Redis for snapshot key  →  HIT
  2. Decode cursor → offset integer
  3. Slice [offset..offset+24]
  4. Encode next cursor = Base64(offset + 25) if more remain
  5. Return page + nextCursor + total
```

Cursor is simply `Base64(offset)` — opaque to the client, stateless on the server.

---

## Implementation Plan

### Step 1 — Backend: Extend `ICacheService` with `GetAsync` / `SetAsync`

**File:** `PatriotIndex.Domain/Interfaces/ICacheService.cs`

Add two new methods alongside the existing `GetOrSetAsync`:
```csharp
Task<T?> GetAsync<T>(string key, CancellationToken ct = default);
Task SetAsync<T>(string key, T value, TimeSpan ttl, CancellationToken ct = default);
```

Implement both in the existing Redis `CacheService` using the same JSON serialization pattern already in `GetOrSetAsync`.

---

### Step 2 — Backend: Replace Keyset Cursor with Snapshot Pattern in `PlayerQueryRepository`

**File:** `PatriotIndex.Domain/Queries/PlayerQueryRepository.cs`

Replace `GetAllPlayersStatsAsync` with a snapshot-aware implementation:

```csharp
public async Task<PagedResultDto<PlayerSeasonStats>> GetAllPlayersStatsAsync(
    string? positionGroup, int season, string seasonType,
    string? cursor, int limit, string? sortBy, string? sortDir,
    CancellationToken ct = default)
```

**Logic:**
1. Compute snapshot cache key:
   `$"stats:snapshot:players:{positionGroup ?? "all"}:{season}:{seasonType}:{sortBy ?? "default"}:{sortDir ?? "desc"}"`
2. Try `cache.GetAsync<PlayerSeasonStats[]>(snapshotKey)`
3. On **miss**: query DB with **no `Take()` limit**, sort in-memory, store in Redis
4. On **hit**: deserialize and use directly
5. Decode cursor → offset; slice `[offset..offset+limit-1]`; encode next cursor if more remain

**Sort mapping** (switch on `sortBy` → `Func<PlayerSeasonStats, IComparable>`):

| `sortBy` value | Sort expression |
|---|---|
| `"passYds"` | `s.Stats.Passing?.Yards ?? 0` |
| `"passTd"` | `s.Stats.Passing?.Touchdowns ?? 0` |
| `"passRating"` | `s.Stats.Passing?.Rating ?? 0` |
| `"rushYds"` | `s.Stats.Rushing?.Yards ?? 0` |
| `"rushTd"` | `s.Stats.Rushing?.Touchdowns ?? 0` |
| `"recYds"` | `s.Stats.Receiving?.Yards ?? 0` |
| `"recTd"` | `s.Stats.Receiving?.Touchdowns ?? 0` |
| `"tackles"` | `s.Stats.Defense?.Tackles ?? 0` |
| `"sacks"` | `s.Stats.Defense?.Sacks ?? 0` |
| `null` / default | `s.SortKey ?? 0` |

**Cursor helpers** (new `internal static class CursorHelper` in the file):
```csharp
internal static string Encode(int offset) =>
    Convert.ToBase64String(BitConverter.GetBytes(offset));

internal static int Decode(string? cursor) =>
    cursor is null ? 0 : BitConverter.ToInt32(Convert.FromBase64String(cursor));
```

**Remove** dead methods: `FetchAllPlayersFromDb`, `Paginate<T>` (currently unused / commented out).

---

### Step 3 — Backend: Update `IPlayerRepository` Interface

**File:** `PatriotIndex.Domain/Interfaces/IPlayerRepository.cs`

```csharp
Task<PagedResultDto<PlayerSeasonStats>> GetAllPlayersStatsAsync(
    string? positionGroup, int season, string seasonType,
    string? cursor, int limit,
    string? sortBy = null, string? sortDir = "desc",
    CancellationToken ct = default);
```

---

### Step 4 — Backend: Thread Sort Params Through Service and Controller

**File:** `PatriotIndex.Domain/Queries/StatisticsQueryService.cs`

Add `sortBy` / `sortDir` to `GetPlayerSeasonStats(...)` and pass to repository.

**File:** `PatriotIndex.Api/Controllers/StatsController.cs`

```csharp
[FromQuery] string? sortBy = null,
[FromQuery] string? sortDir = "desc"
```

**Remove** the Redis `GetOrSetAsync` call in the controller for the players endpoint — the repository now owns the snapshot cache. The controller becomes a thin pass-through.

> **Why**: The snapshot key covers the entire sorted dataset. A controller-level cache keyed by cursor would duplicate per-page slices and produce unnecessary Redis entries.

Team stats endpoint: no changes (32 teams, single-page, existing cache fine).

---

### Step 5 — Frontend: `StatsService` — Page-Keyed Cache

**File:** `PatriotIndex.Frontend/src/app/core/services/stats.service.ts`

Replace the flat `Map<string, PlayerStatsSummary[]>` cache with a **page-level cache**:

```typescript
private readonly pageCache = new Map<string, PagedResult<PlayerStatsSummary>>();

getPlayerStatsPage(p: {
  position?: string;
  season: number;
  seasonType?: string;
  cursor?: string | null;
  limit?: number;          // default 25
  sortBy?: string | null;
  sortDir?: 'asc' | 'desc';
}): Observable<PagedResult<PlayerStatsSummary>> {
  const key = [
    p.position ?? 'all', p.season,
    p.seasonType ?? 'REG',
    p.sortBy ?? '', p.sortDir ?? 'desc',
    p.cursor ?? 'start',
  ].join(':');

  if (this.pageCache.has(key)) return of(this.pageCache.get(key)!);

  let params = new HttpParams()
    .set('season', p.season)
    .set('seasonType', p.seasonType ?? 'REG')
    .set('limit', p.limit ?? 25);
  if (p.position) params = params.set('position', p.position);
  if (p.cursor)   params = params.set('cursor', p.cursor);
  if (p.sortBy)   params = params.set('sortBy', p.sortBy);
  if (p.sortDir)  params = params.set('sortDir', p.sortDir);

  return this.http
    .get<PagedResult<PlayerStatsSummary>>('/stats/players', { params })
    .pipe(tap(r => this.pageCache.set(key, r)));
}
```

Keep `getAllTeamsStats` unchanged.

---

### Step 6 — Frontend: `PlayerStatsComponent` — Cursor Navigation State

**File:** `PatriotIndex.Frontend/src/app/features/stats/player-stats/player-stats.ts`

#### New signals
```typescript
readonly cursor      = signal<string | null>(null);
readonly cursorStack = signal<(string | null)[]>([]);
readonly sortByCol   = signal<string | null>(null);
readonly sortDirVal  = signal<'asc' | 'desc'>('desc');
```

#### Updated `params` computed
```typescript
private params = computed(() => ({
  position:   this.selectedPosition(),
  season:     this.year(),
  seasonType: this.seasonType(),
  cursor:     this.cursor(),
  sortBy:     this.sortByCol(),
  sortDir:    this.sortDirVal(),
  limit:      25,
}));
```

#### Updated `rxResource`
```typescript
statsResource = rxResource({
  params: this.params,
  stream: ({ params }) => this.statsService.getPlayerStatsPage(params),
});
```

#### Reset cursor on filter change (in constructor)
```typescript
effect(() => {
  this.selectedPosition(); this.year(); this.seasonType();
  untracked(() => {
    this.cursor.set(null);
    this.cursorStack.set([]);
  });
});
```

#### Navigation methods
```typescript
nextPage() {
  const next = this.statsResource.value()?.nextCursor;
  if (!next) return;
  this.cursorStack.update(s => [...s, this.cursor()]);
  this.cursor.set(next);
}

prevPage() {
  const stack = this.cursorStack();
  if (!stack.length) return;
  this.cursorStack.set(stack.slice(0, -1));
  this.cursor.set(stack[stack.length - 1]);
}
```

#### Column sort handler (wire to `nzSortOrderChange` or `(nzSortOrderChange)` on `nz-table-sort-header`)
```typescript
onColumnSort(colKey: string, dir: NzTableSortOrder) {
  this.sortByCol.set(dir ? colKey : null);
  this.sortDirVal.set(dir === 'ascend' ? 'asc' : 'desc');
  this.cursor.set(null);
  this.cursorStack.set([]);
}
```

#### Derived signals
```typescript
readonly players   = computed(() => this.statsResource.value()?.items ?? []);
readonly total     = computed(() => this.statsResource.value()?.totalCount ?? 0);
readonly hasPrev   = computed(() => this.cursorStack().length > 0);
readonly hasNext   = computed(() => !!this.statsResource.value()?.nextCursor);
readonly pageIndex = computed(() => this.cursorStack().length + 1);
```

#### Replace ng-zorro pagination with manual nav buttons
```html
<!-- Remove nzPageSizeOptions / nzShowPagination from nz-table -->
<nz-table [nzData]="players()" [nzShowPagination]="false" ...>
  ...
</nz-table>

<div class="flex items-center justify-between mt-3 text-sm text-gray-400">
  <button nz-button nzSize="small" [disabled]="!hasPrev()" (click)="prevPage()">
    ← Prev
  </button>
  <span>Page {{ pageIndex() }} · {{ total() | number }} players</span>
  <button nz-button nzSize="small" [disabled]="!hasNext()" (click)="nextPage()">
    Next →
  </button>
</div>
```

---

### Step 7 — `SortKey` Field (No Action Required Now)

`SortKey` on `PlayerSeasonStats` remains as-is — it becomes the fallback default sort when no `sortBy` is specified. No migration changes needed.

---

## Cache Strategy Summary

| Layer | Mechanism | Key Pattern | TTL |
|---|---|---|---|
| Redis (snapshot) | `ICacheService.GetAsync` / `SetAsync` | `stats:snapshot:players:{pos}:{yr}:{type}:{sort}:{dir}` | 30 min (current season) / 24 h (past) |
| Angular (page) | In-memory `Map` in `StatsService` | `{pos}:{yr}:{type}:{sort}:{dir}:{cursor\|start}` | Session |

---

## Critical Files

| File | Change |
|---|---|
| `PatriotIndex.Domain/Interfaces/ICacheService.cs` | Add `GetAsync<T>` and `SetAsync<T>` |
| `PatriotIndex.Domain/Interfaces/IPlayerRepository.cs` | Add `sortBy`, `sortDir` params |
| `PatriotIndex.Domain/Queries/PlayerQueryRepository.cs` | Snapshot logic, `CursorHelper`, sort map; remove dead methods |
| `PatriotIndex.Domain/Queries/StatisticsQueryService.cs` | Thread `sortBy`/`sortDir` |
| `PatriotIndex.Api/Controllers/StatsController.cs` | Add sort params; remove per-page controller cache |
| `PatriotIndex.Frontend/src/app/core/services/stats.service.ts` | Page-keyed cache; `getPlayerStatsPage` |
| `PatriotIndex.Frontend/src/app/features/stats/player-stats/player-stats.ts` | Cursor signals, nav methods, sort handler, pagination UI |

---

## Verification Checklist

- [ ] `GET /api/stats/players?position=QB&season=2025&limit=25` → 25 records + opaque `nextCursor`, total count
- [ ] Same request a second time → Redis hit, no DB query
- [ ] `GET ...&cursor={nextCursor}` → next 25 QBs from same snapshot
- [ ] `GET ...&sortBy=passTd&sortDir=desc` → top 25 by TDs; new Redis snapshot key created
- [ ] Frontend: switch QB→RB tab → cursor + stack reset, new request fires
- [ ] Frontend: click "Next →" → correct next page; "← Prev" → returns correctly
- [ ] Frontend: click column header → sortBy sent, cursor resets, correct ranking across pages
- [ ] Year change (2025→2024) → cursor resets, past-season snapshot created with 24h TTL

---

## Confirmed Decisions

| Decision | Value |
|---|---|
| Default page size | 25 per page |
| Column sort | Server-side (new snapshot key per sort; Redis serves instantly after first hit) |
| ICacheService | Add `GetAsync<T>` / `SetAsync<T>` — currently only `GetOrSetAsync` exists |

## Open Question

Default `sortBy` per position group when no user sort is active — which stat leads?
Suggested defaults: QB→`passYds`, RB→`rushYds`, WR/TE→`recYds`, DEF→`tackles`, K→`fgMade`, P→`puntAvg`
