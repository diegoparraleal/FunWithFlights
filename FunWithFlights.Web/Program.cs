using FunWithFlights.Web;
using FunWithFlights.Infrastructure.Contracts.Loggers;

var builder = WebApplication.CreateBuilder(args);

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Configure services
builder.RegisterServices();
builder.Services.AddControllers().AddNewtonsoftJson();

var app = builder.Build();
Logger.Configure(app.Services.GetRequiredService<ILoggerFactory>());

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

if (app.Environment.IsDevelopment())
{
    app.MapGet("/api/debug/routes", (IEnumerable<EndpointDataSource> endpointSources) =>
        string.Join("\n", endpointSources.SelectMany(
                source => source.Endpoints.Select(x => $"{(x as RouteEndpoint)?.RoutePattern.RawText} => {x.DisplayName}")
            )
        )
    );
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();