using AutoMapper;
using CourseApiDtoCrud.Api.Manage;
using CourseApiDtoCrud.Api.Manage.Dtos.CategoriesDtos;
using CourseApiDtoCrud.Data;
using CourseApiDtoCrud.Data.Entities;
using CourseApiDtoCrud.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseApiDtoCrud
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins("http://localhost:3000")
                                                                                 .AllowAnyOrigin()
                                                                                .AllowAnyHeader()
                                                                                .AllowAnyMethod();
                                  });
            });
            services.AddControllers()
                                    .AddFluentValidation(option => option.RegisterValidatorsFromAssemblyContaining<CategoryCreateDtoValidator>());

            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("Default"));
            }).AddIdentity<AppUser, IdentityRole>(opt =>
            {
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
            }).AddDefaultTokenProviders().AddEntityFrameworkStores<AppDbContext>();

            services.AddScoped(provider => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ManageMapper());
            }).CreateMapper());

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
             .AddJwtBearer(options =>
             {
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidIssuer = Configuration.GetSection("JWT:issuer").Value,
                     ValidAudience = Configuration.GetSection("JWT:audience").Value,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("JWT:secret").Value))
                 };
             });

            services.AddScoped<IJwtService, JwtService>();

            services.AddSwaggerGen(c =>
            {
              
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
               {
                 new OpenApiSecurityScheme
                 {
                   Reference = new OpenApiReference
                   {
                     Type = ReferenceType.SecurityScheme,
                     Id = "Bearer"
                   }
                  },
                  new string[] { }
                }
              });

              

            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors(MyAllowSpecificOrigins);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
