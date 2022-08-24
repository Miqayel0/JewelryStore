using Common.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;
using System.Runtime.Serialization;

namespace Common.Exceptions;

[DataContract]
public class ValidationException : HttpRequestException
{
    [DataMember]
    public IDictionary<string, string[]> Failures { get; }
    public ValidationException(
        string message = "One or more validation failures have occurred.",
        HttpStatusCode statusCode = HttpStatusCode.UnprocessableEntity) : base(message, statusCode)
    {
        Failures = new Dictionary<string, string[]>();
    }

    public ValidationException(IEnumerable<ValidationFailure> failures = null, string message = "One or more validation failures have occurred.") : this(message, HttpStatusCode.UnprocessableEntity)
    {
        if (failures == null) return;
        var failureGroups = failures.GroupBy(e => e.PropertyName, e => e.ErrorMessage);

        foreach (var failureGroup in failureGroups)
        {
            var propertyName = failureGroup.Key;
            var propertyFailures = failureGroup.ToArray();

            Failures.Add(propertyName, propertyFailures);
        }
    }

    public ValidationException(ModelStateDictionary modelState, string message = "One or more validation failures have occurred.") : this(message, HttpStatusCode.UnprocessableEntity)
    {
        foreach (var state in modelState)
        {
            var propertyName = state.Key;
            var propertyFailures = state.Value.Errors.Select(err => err.ErrorMessage).ToArray();

            Failures.Add(propertyName, propertyFailures);
        }
    }
}
