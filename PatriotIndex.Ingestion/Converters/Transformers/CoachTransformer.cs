using PatriotIndex.Domain.Entities;

namespace PatriotIndex.Ingestion.Converters.Transformers;

public class CoachTransformer : IJsonTransformer<Coach>
{
    public Coach Transform(JsonNavigator nav)
    {
        var coach = new Coach
        {
            Id = nav["id"].GetGuid(),
            FirstName = nav["first_name"].GetStringOrNull(),
            LastName = nav["last_name"].GetStringOrNull(),
            FullName = nav["full_name"].GetStringOrNull(),
            Position = nav["position"].GetStringOrNull(),
        };

        return coach;
    }
}