namespace PatriotIndex.Ingestion.Converters;

public interface IJsonTransformer<out TEntity>
{
    TEntity Transform(JsonNavigator nav);
}
