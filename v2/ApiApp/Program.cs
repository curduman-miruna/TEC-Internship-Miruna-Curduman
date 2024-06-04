using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Web;
using System;
using System.Text;


var nLogPath = "D:\\Documents\\GitHub\\TEC-Internship-Miruna-Curduman\\v2\\ApiApp\\nlog.config";
var logger = NLogBuilder.ConfigureNLog(nLogPath).GetCurrentClassLogger();


try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.NLog.NLogConfigurationException: 'Exception when loading configuration D:\Documents\GitHub\TEC-Internship-Miruna-Curduman\v2\ApiApp\nlog.config'
    builder.Services.AddControllers();

    var jwtSettings = builder.Configuration.GetSection("JWT");

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = jwtSettings["ValidIssuer"],
                            ValidAudience = jwtSettings["ValidAudience"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]))
                        };
                    });

    // Add authorization services
    builder.Services.AddAuthorization();

    // Configure Swagger
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v2", new OpenApiInfo
        {
            Version = "v2",
            Title = "ApiApp",
        });

        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
    });

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v2/swagger.json", "ApiApp v2"));
    }
    else
    {
        app.UseExceptionHandler("/Home/");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();  

    app.MapControllers(); 

    app.Run();
}

catch (Exception exception)
{
    logger.Error(exception, "Stopped program because of exception");
throw;
}

finally
{
    NLog.LogManager.Shutdown();
}