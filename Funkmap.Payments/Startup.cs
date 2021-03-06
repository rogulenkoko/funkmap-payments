﻿using System;
using System.IO;
using System.Reflection;
using Autofac;
using Funkmap.Payments.Core;
using Funkmap.Payments.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Funkmap.Payments.Data.Module;
using Funkmap.Payments.Tools;
using PayPal;
using Swashbuckle.AspNetCore.Swagger;

namespace Funkmap.Payments
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            FunkmapConfigurationProvider.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var authOptions = new FunkmapJwtOptions(Configuration);
            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ValidIssuer = authOptions.Issuer,
                        ValidAudience = authOptions.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(authOptions.Key))
                    };
                });

            services.AddDataServices(Configuration);
            services.AddCors();
            services.AddMvc();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Title = "Funkmap Payments API", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);

                options.AddSecurityDefinition("Bearer", new OAuth2Scheme
                {
                    Flow = "password",
                    TokenUrl = authOptions.TokenUrl,
                });

                options.DocumentFilter<OAuthDocumentFilter>();
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterPayPalServices(Configuration);
            builder.RegisterDataServices();
            builder.RegisterDomainServices();
            builder.RegisterRedisServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            app.UseCors(builder =>
            {
                builder.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin();
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Funkmap API");
                c.RoutePrefix = string.Empty;
            });

            app.Use(next =>
            {
                return async context =>
                {
                    LocaleMiddleware.Invoke(context);
                    await next(context);
                };
            });


            app.UseMvc();
        }
    }
}
