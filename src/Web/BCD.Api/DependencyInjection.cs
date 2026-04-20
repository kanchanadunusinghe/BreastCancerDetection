using Microsoft.AspNetCore.Http.Features;
using Microsoft.OpenApi;

namespace BCD.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBCDApiServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddHttpContextAccessor();

            var allowedOrigins = new List<string>();

            services.Configure<FormOptions>(o =>
            {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = long.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
                o.ValueCountLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });

            services.AddMvc(options =>
            {
                options.MaxModelBindingCollectionSize = int.MaxValue;
            });

            var allowOrigins = configuration.GetValue<string>("AllowedOrigins")!
                .Split(",");

            services.AddCors(options =>
            {
                options.AddPolicy(name: "CorsPolicy",
                          builder => builder.WithOrigins(allowOrigins)
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .WithExposedHeaders("Content-Disposition")
                          .SetIsOriginAllowed((x) => true)
                          .AllowCredentials()
                         );
            });


            services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer((document, context, cancellationToken) =>
                {
                    var scheme = new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "Enter your valid token in the text input below."
                    };

                    document.Components ??= new OpenApiComponents();

                    if (document.Components.SecuritySchemes == null)
                    {

                        document.Components.SecuritySchemes = new Dictionary<string, IOpenApiSecurityScheme>();
                    }
                    document.Components.SecuritySchemes["Bearer"] = scheme;

                    var requirement = new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecuritySchemeReference("Bearer", document),
                            new List<string>()
                        }
                    };

                    document.Security ??= new List<OpenApiSecurityRequirement>();
                    document.Security.Add(requirement);

                    return Task.CompletedTask;
                });
            });


            return services;
        }
    }
}
