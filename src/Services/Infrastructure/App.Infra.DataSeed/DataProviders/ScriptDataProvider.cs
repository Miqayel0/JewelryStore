using App.Infra.DataSeed.Seeders;
using Common.Extensions;

namespace App.Infra.DataSeed.DataProviders;

public class ScriptDataProvider : IScriptDataProvider
{
    private string _fileName;

    public ScriptDataProvider(string fileName)
    {
        _fileName = fileName;
    }

    public string GetScriptSql()
    {
        const string extension = ".sql";
        if (!_fileName.Contains(extension))
            _fileName += extension;
        return _fileName.ReadEmbeddedResource<DataSeeder>();
    }
}
