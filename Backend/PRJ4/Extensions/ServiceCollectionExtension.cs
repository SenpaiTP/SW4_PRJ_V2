using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace PRJ4.ServiceCollectionExtension
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddSwaggerGenWithAuth(this IServiceCollection services)
        {
            services.AddSwaggerGen(c=>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In=ParameterLocation.Header,
                    Description = "Please enter 'Bearer' followed by a space and your JWT token.",
                    Name="Authorization",
                    Type=SecuritySchemeType.ApiKey,
                    BearerFormat="JWT",
                    Scheme="Bearer"
                });

                var securityRequirement = new OpenApiSecurityRequirement
                {
                {
                        new OpenApiSecurityScheme
                        {
                            Reference=new OpenApiReference
                            {
                                    Id="Bearer",
                                    Type=ReferenceType.SecurityScheme
                            }
                        },
                        Array.Empty<string>()
                }
                };

                c.AddSecurityRequirement(securityRequirement);
            });
            
            return services;
        }
    };
}