using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using HGD_SSO_API.Data;
using HGD_SSO_API.Services.AuthManagement;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- Database configuration ---
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- Custom services ---
builder.Services.AddScoped<PasswordService>();
builder.Services.AddScoped<JwtService>();

// --- Authentication (JWT Bearer) ---
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
            )
        };
    });


// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalAndNgrok",
        policy =>
        {
            policy
                .WithOrigins(
                    "http://localhost:3000",          // Your frontend local dev
                    "https://786cdd3b306c.ngrok-free.app" // Your ngrok domain
                )
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
});

var app = builder.Build();

// Use CORS policy
app.UseCors("AllowLocalAndNgrok");

// --- DataBase Seeder ---
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DatabaseSeeder.SeedUsers(dbContext);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// --- Enable Authentication & Authorization ---
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();