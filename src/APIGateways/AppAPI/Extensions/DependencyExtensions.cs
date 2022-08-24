using App.Domain.Config;
using AppAPI.Infrastructure.Filters;
using AppAPI.Infrastructure.JsonOptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Text.Json;

namespace AppAPI.Extensions;

public static class DependencyExtensions
{
    public static IServiceCollection AddCustomConfiguration(this IServiceCollection services, IConfiguration config, IWebHostEnvironment env)
    {
        services.Configure<JwtOptions>(config.GetSection(nameof(JwtOptions)));
        services.AddSingleton<IConfigureOptions<JwtBearerOptions>, CustomJwtBearerOptionsSetup>(); ;
        //services.Replace(ServiceDescriptor.Singleton<IConfigureOptions<JwtBearerOptions>, CustomJwtBearerOptionsSetup>());
        services.AddSingleton<CustomJwtBearerEvents>();

        return services;
    }

    public static IServiceCollection AddCustomMvc(this IServiceCollection services, IConfiguration config)
    {
        services.AddControllers(options =>
            {
                options.Filters.Add(new ProducesAttribute("application/json"));
                options.Filters.Add(typeof(ValidateModelActionFilter));
                options.Filters.Add(typeof(ResponseResultFilter));
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });
        //.AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()))

        services.AddAuthorization(o =>
            {
                o.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .Build();
                o.AddPolicy("MyPolicy1", b =>
                {
                    b.RequireAuthenticatedUser();
                });
                //o.AddPolicy("DomainRestricted", b =>
                //{
                //    b.Requirements.Add(new DomainRestrictedRequirement());
                //});
            });

        // https://docs.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-3.1#set-the-preflight-expiration-time
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder => builder
                .WithExposedHeaders(HeaderNames.WWWAuthenticate)
                .WithHeaders(
                    HeaderNames.Authorization,
                    HeaderNames.ContentType,
                    HeaderNames.Accept,
                    HeaderNames.Origin,
                    "x-requested-with")
                .WithMethods(
                    HttpMethods.Get,
                    HttpMethods.Post,
                    HttpMethods.Delete,
                    HttpMethods.Options)
                .SetIsOriginAllowed((host) => true)
                .DisallowCredentials()
                .SetPreflightMaxAge(TimeSpan.FromSeconds(2400))
            );
        });

        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
        });

        return services;
    }

    public static IServiceCollection AddCustomAuth(this IServiceCollection services, IConfiguration config, IHostEnvironment env)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(ops =>
            {
                using var serviceProvider = services.BuildServiceProvider();
                var configOps = serviceProvider.GetService<IOptions<JwtBearerOptions>>().Value;
                foreach (var p in ops.GetType().GetProperties())
                {
                    if (p.CanWrite && p.CanRead)
                    {
                        p.SetValue(ops, p.GetValue(configOps));
                    }
                }
            });

        return services;
    }

    public static IServiceCollection AddHttpServices(this IServiceCollection services, IWebHostEnvironment env)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        return services;
    }
}
