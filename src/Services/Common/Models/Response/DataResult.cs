using System.Net;

namespace Common.Models.Response;

public class DataResult<TData> : UseCaseResult
{
    public DataResult(TData data, int statusCode = (int)HttpStatusCode.OK) : base(statusCode)
    {
        Data = data;
    }

    public TData Data { get; }
}
