namespace App.Infra.DataSeed.DataProviders;

public interface IJsonDataProvider<out TEntity>
{
    TEntity[] GetData();
}
