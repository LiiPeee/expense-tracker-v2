using ExpenseTrackerV2.Application;
using ExpenseTrackerV2.Infrastructure;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var appSettings = builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

builder.Services.ConfigureApplicationServicesWebApi(builder.Configuration);
builder.Services.AddInfrastructureWebApi(builder.Configuration);
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi


// Configure the HTTP request pipeline.
Env.Load();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = appSettings["Jwt:Issuer"],
        ValidAudience = appSettings["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings["Jwt:Token"]!)),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendDev", policy =>
    {
        policy
            .WithOrigins("http://localhost:8081")
            .AllowAnyHeader()
            .AllowAnyMethod();
        // Se você usa cookies/sessão (credentials), descomente:
        // .AllowCredentials();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
};

app.UseCors("FrontendDev");
app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.Run();