using Ambev.DeveloperEvaluation.Application;
using Ambev.DeveloperEvaluation.Common.HealthChecks;
using Ambev.DeveloperEvaluation.Common.Logging;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.IoC;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.WebApi.Middleware;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;

namespace Ambev.DeveloperEvaluation.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            Log.Information("Starting web application");

            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            builder.AddDefaultLogging();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.AddBasicHealthChecks();

            builder.Services.AddSwaggerGen(c =>
            {
                // ensure unique schema ids
                c.CustomSchemaIds(type => type.FullName);

                // Add JWT bearer auth to Swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Enter 'Bearer {your JWT token}'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                //c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                //{
                //    Name = "Authorization",
                //    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                //    Scheme = "Bearer",
                //    BearerFormat = "JWT",
                //    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                //    Description = "Enter 'Bearer' [space] and then your valid JWT token."
                //});

                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });

            builder.Services.AddDbContext<DefaultContext>(options =>
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("Ambev.DeveloperEvaluation.ORM")
                )
            );

            builder.Services.AddJwtAuthentication(builder.Configuration);

            builder.RegisterDependencies();

            builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(ApplicationLayer).Assembly);

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(
                    typeof(ApplicationLayer).Assembly,
                    typeof(Program).Assembly
                );
            });

            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            var app = builder.Build();
            app.UseMiddleware<ValidationExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseBasicHealthChecks();

            app.MapControllers();

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}


//// File: src/Ambev.DeveloperEvaluation.WebApi/Program.cs
//using Ambev.DeveloperEvaluation.Application;
//using Ambev.DeveloperEvaluation.Common.HealthChecks;
//using Ambev.DeveloperEvaluation.Common.Logging;
//using Ambev.DeveloperEvaluation.Common.Security;
//using Ambev.DeveloperEvaluation.Common.Validation;
//using Ambev.DeveloperEvaluation.IoC;
//using Ambev.DeveloperEvaluation.ORM;
//using Ambev.DeveloperEvaluation.WebApi.Middleware;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using Serilog;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Http;
//using System.Text.Json;

//namespace Ambev.DeveloperEvaluation.WebApi;

//public class Program
//{
//    public static void Main(string[] args)
//    {
//        try
//        {
//            Log.Information("Starting web application");

//            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
//            builder.AddDefaultLogging();

//            builder.Services.AddControllers();
//            builder.Services.AddEndpointsApiExplorer();

//            builder.AddBasicHealthChecks();
//            builder.Services.AddSwaggerGen(c =>
//            {
//                // Ensure unique schema IDs if needed
//                c.CustomSchemaIds(type => type.FullName);
//            });

//            builder.Services.AddDbContext<DefaultContext>(options =>
//                options.UseNpgsql(
//                    builder.Configuration.GetConnectionString("DefaultConnection"),
//                    b => b.MigrationsAssembly("Ambev.DeveloperEvaluation.ORM")
//                )
//            );

//            // JWT Authentication with custom responses
//            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//                .AddJwtBearer(options =>
//                {
//                    // existing configuration
//                    options.Authority = builder.Configuration["Jwt:Authority"];
//                    options.Audience = builder.Configuration["Jwt:Audience"];

//                    // custom Challenge and Forbidden responses
//                    options.Events = new JwtBearerEvents
//                    {
//                        OnChallenge = async context =>
//                        {
//                            context.HandleResponse();
//                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
//                            context.Response.ContentType = "application/json";
//                            var payload = JsonSerializer.Serialize(new { success = false, message = "You are not authenticated to access this resource." });
//                            await context.Response.WriteAsync(payload);
//                        },
//                        OnForbidden = async context =>
//                        {
//                            context.Response.StatusCode = StatusCodes.Status403Forbidden;
//                            context.Response.ContentType = "application/json";
//                            var payload = JsonSerializer.Serialize(new { success = false, message = "You do not have permission (role) to access this resource." });
//                            await context.Response.WriteAsync(payload);
//                        }
//                    };
//                });

//            builder.RegisterDependencies();

//            builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(ApplicationLayer).Assembly);

//            builder.Services.AddMediatR(cfg =>
//            {
//                cfg.RegisterServicesFromAssemblies(
//                    typeof(ApplicationLayer).Assembly,
//                    typeof(Program).Assembly
//                );
//            });

//            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

//            var app = builder.Build();
//            app.UseMiddleware<ValidationExceptionMiddleware>();

//            if (app.Environment.IsDevelopment())
//            {
//                app.UseSwagger();
//                app.UseSwaggerUI();
//            }

//            app.UseHttpsRedirection();

//            app.UseAuthentication();
//            app.UseAuthorization();

//            app.UseBasicHealthChecks();

//            app.MapControllers();

//            app.Run();
//        }
//        catch (Exception ex)
//        {
//            Log.Fatal(ex, "Application terminated unexpectedly");
//        }
//        finally
//        {
//            Log.CloseAndFlush();
//        }
//    }
//}

//// File: src/Ambev.DeveloperEvaluation.WebApi/Program.cs
//using Ambev.DeveloperEvaluation.Application;
//using Ambev.DeveloperEvaluation.Common.HealthChecks;
//using Ambev.DeveloperEvaluation.Common.Logging;
//using Ambev.DeveloperEvaluation.Common.Security;
//using Ambev.DeveloperEvaluation.Common.Validation;
//using Ambev.DeveloperEvaluation.IoC;
//using Ambev.DeveloperEvaluation.ORM;
//using Ambev.DeveloperEvaluation.WebApi.Middleware;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using Serilog;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Http;
//using System.Text.Json;

//namespace Ambev.DeveloperEvaluation.WebApi;

//public class Program
//{
//    public static void Main(string[] args)
//    {
//        try
//        {
//            Log.Information("Starting web application");

//            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
//            builder.AddDefaultLogging();

//            builder.Services.AddControllers();
//            builder.Services.AddEndpointsApiExplorer();

//            builder.AddBasicHealthChecks();
//            builder.Services.AddSwaggerGen(c =>
//            {
//                // ensure unique schema ids
//                c.CustomSchemaIds(type => type.FullName);

//                // Add JWT bearer auth to Swagger
//                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
//                {
//                    Name = "Authorization",
//                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
//                    Scheme = "Bearer",
//                    BearerFormat = "JWT",
//                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
//                    Description = "Enter 'Bearer' [space] and then your valid JWT token."
//                });

//                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
//                {
//                    {
//                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
//                        {
//                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
//                            {
//                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
//                                Id = "Bearer"
//                            }
//                        },
//                        new string[] { }
//                    }
//                });
//            });

//            //c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
//            //    {
//            //        {
//            //            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
//            //            {
//            //                Reference = new Microsoft.OpenApi.Models.OpenApiReference
//            //                {
//            //                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
//            //                    Id = "Bearer"
//            //                }
//            //            },
//            //            new string[] { }
//            //        }
//            //    });
//            //{
//            //    // Ensure unique schema IDs if needed
//            //    c.CustomSchemaIds(type => type.FullName);
//            //});

//            builder.Services.AddDbContext<DefaultContext>(options =>
//                options.UseNpgsql(
//                    builder.Configuration.GetConnectionString("DefaultConnection"),
//                    b => b.MigrationsAssembly("Ambev.DeveloperEvaluation.ORM")
//                )
//            );

//            // JWT Authentication with custom responses
//            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//                .AddJwtBearer(options =>
//                {
//                    // existing configuration
//                    options.Authority = builder.Configuration["Jwt:Authority"];
//                    options.Audience = builder.Configuration["Jwt:Audience"];

//                    // custom Challenge and Forbidden responses
//                    options.Events = new JwtBearerEvents
//                    {
//                        OnChallenge = async context =>
//                        {
//                            context.HandleResponse();
//                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
//                            context.Response.ContentType = "application/json";
//                            var payload = JsonSerializer.Serialize(new { success = false, message = "You are not authenticated to access this resource." });
//                            await context.Response.WriteAsync(payload);
//                        },
//                        OnForbidden = async context =>
//                        {
//                            context.Response.StatusCode = StatusCodes.Status403Forbidden;
//                            context.Response.ContentType = "application/json";
//                            var payload = JsonSerializer.Serialize(new { success = false, message = "You do not have permission (role) to access this resource." });
//                            await context.Response.WriteAsync(payload);
//                        }
//                    };
//                });

//            builder.RegisterDependencies();

//            builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(ApplicationLayer).Assembly);

//            builder.Services.AddMediatR(cfg =>
//            {
//                cfg.RegisterServicesFromAssemblies(
//                    typeof(ApplicationLayer).Assembly,
//                    typeof(Program).Assembly
//                );
//            });

//            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

//            var app = builder.Build();
//            app.UseMiddleware<ValidationExceptionMiddleware>();

//            if (app.Environment.IsDevelopment())
//            {
//                app.UseSwagger();
//                app.UseSwaggerUI();
//            }

//            app.UseHttpsRedirection();

//            app.UseAuthentication();
//            app.UseAuthorization();

//            app.UseBasicHealthChecks();

//            app.MapControllers();

//            app.Run();
//        }
//        catch (Exception ex)
//        {
//            Log.Fatal(ex, "Application terminated unexpectedly");
//        }
//        finally
//        {
//            Log.CloseAndFlush();
//        }
//    }
//}
