using App.Infra.Dependency;
using App.SignalRHub.Extensions;
using AppAPI.Extensions;
using AppAPI.Infrastructure.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCustomSwagger();
builder.Services.AddCustomConfiguration(builder.Configuration, builder.Environment);
builder.Services.AddCustomAuth(builder.Configuration, builder.Environment);
builder.Services.AddCustomDependencies(builder.Configuration, builder.Environment);
builder.Services.AddCustomMvc(builder.Configuration);
builder.Services.AddHttpServices(builder.Environment);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandling();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHubs();
    endpoints.MapControllers().RequireAuthorization();
});

await app.MigrateDatabaseAsync();

app.Run();
