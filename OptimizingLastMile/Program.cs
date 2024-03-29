﻿using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OptimizingLastMile.Configs;
using OptimizingLastMile.Hubs;
using OptimizingLastMile.Utils;
using SendGrid.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add DB Context
string connectionString = builder.Configuration.GetConnectionString("db");
builder.Services.AddDbContext<OlmDbContext>(option => option.UseSqlServer(connectionString));

// Add services to the container.

builder.Services.AddControllers().ConfigureApiBehaviorOptions(option =>
{
    option.InvalidModelStateResponseFactory = ModelStateValidator.ValidateModelState;
});

// Add SignalR
builder.Services.AddSignalR(o =>
{
    o.EnableDetailedErrors = true;
});

// Add Dependency injection
builder.Services.RegisterDIService(builder.Configuration);

// Add AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add SendGrid
builder.Services.AddSendGrid(option =>
{
    option.ApiKey = builder.Configuration["EmailConfig:apiKey"];
});

// Add Cors
builder.Services.AddCors(opt => opt.AddDefaultPolicy(builder => builder.AllowAnyOrigin()
                                                               .AllowAnyMethod()
                                                               .AllowAnyHeader()));

// Config authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
{
    option.SaveToken = true;
    option.RequireHttpsMetadata = false;
    option.TokenValidationParameters = new()
    {
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        RequireExpirationTime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };

    option.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];

            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) &&
                path.StartsWithSegments("/notificationconnect"))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.SwaggerDoc("optimizing", new OpenApiInfo { Title = "Optimizing Last Mile API", Version = "v1" }));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(option =>
{
    option.SwaggerEndpoint("/swagger/optimizing/swagger.json", "Optimizing Last Mile API");
    option.RoutePrefix = "";
});


//app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<NotificationHub>("/notificationconnect");

app.Run();

