using BCD.Api;
using BCD.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddBCDApplicationeServices();
builder.Services.AddBCDInfrastructureServices(builder.Configuration);
builder.Services.AddBCDApiServices(builder.Configuration);


builder.Services.AddControllers();
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

app.UseCors("CorsPolicy");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
