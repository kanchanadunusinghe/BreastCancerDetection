using BCD.Api;
using BCD.Infrastructure.Persistence;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Clear default providers
builder.Logging.ClearProviders();

// Add console logging
builder.Logging.AddConsole();

// Optional: Debug logging
builder.Logging.AddDebug();

// Add services to the container.
builder.Services.AddBCDApplicationeServices();
builder.Services.AddBCDInfrastructureServices(builder.Configuration);
builder.Services.AddBCDApiServices(builder.Configuration);


builder.Services.AddControllers();
builder.Services.AddHttpClient();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        // IMPORTANT: Remove default swagger.json path
        options.SwaggerEndpoint("/openapi/v1.json", "BCD.API v1");


    });
}

using (var scope = app.Services.CreateScope())
{
    var initialiser = scope.ServiceProvider.GetRequiredService<BCDContextSeeder>();
    await initialiser.InitializeAsync();
    await initialiser.SeedAsync();
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(@"C:\BCDMammographyImages"),
    RequestPath = "/mammography-images"
});

app.UseCors("CorsPolicy");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
