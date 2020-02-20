using AuditService.Infrastructure;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MediatR;
using AutoMapper;
using System.Reflection;
using AuditService.Data;
using Steeltoe.CloudFoundry.Connector.MySql.EFCore;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using AuditService.Infrastructure.Idempotency;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace AuditService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var jwtSettings = JwtSettings.FromConfiguration(Configuration);
            services.AddSingleton(jwtSettings);

            services.AddDbContext<AuditContext>(options => options.UseMySql(Configuration));

            services.AddAutoMapper(typeof(Startup).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);
            services.AddControllers();
            
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
                    {
                        options.Authority = "https://cognito-idp.eu-west-2.amazonaws.com/eu-west-2_QtgogH91v";
                        options.Audience = "3inpv3ubfmag4k97cu5iqsesg8";
                        options.TokenValidationParameters = new TokenValidationParameters() { ValidateAudience = false };
                    });

            //services.AddCors(options =>
            //{
            //    // The CORS policy is open for testing purposes. In a production application, you should restrict it to known origins.
            //    options.AddPolicy(
            //        "AllowAll",
            //        builder => builder.AllowAnyOrigin()
            //                          .AllowAnyMethod()
            //                          .AllowAnyHeader());
            //});

            services.AddMvc(opt =>
                {
                    opt.Filters.Add(typeof(ValidatorActionFilter));
                }).AddFluentValidation(cfg => { cfg.RegisterValidatorsFromAssemblyContaining<Startup>(); });
            
             // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Audit Service", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
            }); 

            services.AddSingleton<IUriService>(provider =>
            {
                var accessor = provider.GetRequiredService<IHttpContextAccessor>();
                var request = accessor.HttpContext.Request;
                var absoluteUri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent(), "/");
                return new UriService(absoluteUri);
            });

            services.AddScoped<IRequestManager, RequestManager>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {          
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
             
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AuditContext>();
                context.Database.Migrate();
                
                try{
                    DbInitializer.Initialize(context);
                }
                catch (Exception ex)
                {
                    var logger = serviceScope.ServiceProvider.GetService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }
                                   

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Audit Service V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();
            
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
