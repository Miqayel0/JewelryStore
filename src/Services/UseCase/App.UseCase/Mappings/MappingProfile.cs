using App.UseCase.Common;
using AutoMapper;
using System.Reflection;

namespace App.UseCase.Mappings;

/// <summary>
/// Creates Mappings for Data Transfer Objects.
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        AllowNullCollections = true;
        ApplyMappingsFromAssembly(Assembly.GetAssembly(typeof(IAutoMap<,>)));
    }

    private void ApplyMappingsFromAssembly(Assembly assembly)
    {
        var types = assembly.GetExportedTypes()
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IAutoMap<,>)))
            .ToList();

        foreach (var type in types)
        {
            var instance = Activator.CreateInstance(type);
            var methodName = "CreateMap";
            var methodInfo = type.GetMethod(methodName) ?? type.GetInterface("IAutoMap`2").GetMethod(methodName);

            methodInfo?.Invoke(instance, new object[] { this });
        }
    }
}
