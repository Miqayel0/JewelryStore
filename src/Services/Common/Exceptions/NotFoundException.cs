using System.Net;
using System.Reflection;

namespace Common.Exceptions;

public class NotFoundException : HttpRequestException
{
    public NotFoundException() : base(HttpStatusCode.NotFound)
    {
    }

    public NotFoundException(string message)
        : base(message, HttpStatusCode.NotFound)
    {
    }

    public NotFoundException(MemberInfo type)
        : base(type.Name + " not found", HttpStatusCode.NotFound)
    {
    }
}
