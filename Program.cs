using System.Text;
using MangaApp.Data;
using MangaApp.Interfaces;
using MangaApp.Services;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using CloudinaryDotNet;

var builder = WebApplication.CreateBuilder(args);
var _config = builder.Configuration;

// Configure Cloudinary account
var cloudinaryAccount = new Account(
    _config["Cloudinary:CloudName"],
    _config["Cloudinary:ApiKey"],
    _config["Cloudinary:ApiSecret"]
);
var cloudinary = new CloudinaryDotNet.Cloudinary(cloudinaryAccount);
builder.Services.AddSingleton(cloudinary);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
    })
    .AddGoogle(options =>
    {
        options.ClientId = _config["GOOGLE_CLIENT_ID"];
        options.ClientSecret = _config["GOOGLE_CLIENT_SECRET"];
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenKey"])),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MangaAppDbcontext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ITokenService, TokenService>();

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Add CORS policy
app.UseCors(policy =>
    policy.WithOrigins("http://localhost:5173", "http://hung11062003-001-site1.btempurl.com/", "https://manga-app-steel.vercel.app") // replace with your frontend URL
        .AllowAnyMethod()
        .AllowAnyHeader());

// Configure port binding
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Urls.Add($"http://*:{port}");

app.MapControllers();

app.Run();
