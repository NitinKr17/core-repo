using BusBooking.Application.Interfaces;
using BusBooking.Application.Services;
using BusBooking.Domain.Entities;
using BusBooking.Infrastructure.Data;
using BusBooking.Infrastructure.Repositories;
using BusBooking.Infrastructure.Repositories.Interfaces;
using BusBooking.Shared.Constants;
using BusBooking.Shared.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var jwtSettings = builder.Configuration.GetSection(APIConstants.JwtSection);
var key = jwtSettings[APIConstants.JwtKey];
var issuer = jwtSettings[APIConstants.JwtIssuer];
var audience = jwtSettings[APIConstants.JwtAudience];

builder.Services.AddDbContext<BusBookingDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString(APIConstants.DBConnectionKey)));

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IBusService, BusService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IBusRepository, BusRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddControllers();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(APIConstants.V1, new OpenApiInfo { Title = APIConstants.APITitle, Version = APIConstants.V1 });
    options.AddSecurityDefinition(APIConstants.Bearer, new OpenApiSecurityScheme
    {
        Name = APIConstants.Authorization,
        Type = SecuritySchemeType.ApiKey,
        Scheme = APIConstants.Bearer,
        BearerFormat = APIConstants.JWT,
        In = ParameterLocation.Header,
        Description = APIConstants.BearerMessage
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = APIConstants.Bearer
                }
            },
            Array.Empty<string>()
        }
    });
});

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
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!))
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(APIConstants.AllowFrontend, policy =>
    {
        policy.WithOrigins(APIConstants.APIEndpoint)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(APIConstants.AllowFrontend);
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.MapControllers();
app.Run();
