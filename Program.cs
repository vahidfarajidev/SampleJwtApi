
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Load JWT configuration from appsettings.json
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var issuer = jwtSettings.GetValue<string>("Issuer");
var audience = jwtSettings.GetValue<string>("Audience");
var secretKey = jwtSettings.GetValue<string>("SecretKey");

// Register services for controllers and Swagger
builder.Services.AddControllers();

// Swagger:begin
// Configure Swagger for API documentation and JWT support
builder.Services.AddEndpointsApiExplorer(); // Enables minimal API metadata for Swagger
builder.Services.AddSwaggerGen(options =>
{
    // Define JWT Bearer scheme so Swagger UI can send tokens
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by space and your JWT token."
    });

    // Apply JWT security to all endpoints
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
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
            new string[] {}
        }
    });
});
//Swagger:end

// Enable JWT Authentication & Authorization
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

builder.Services.AddAuthorization(); // Adds support for role-based or policy-based access control

var app = builder.Build();

//Swagger:begin
// Configure the HTTP request pipeline
app.UseSwagger();       // Enable Swagger middleware
app.UseSwaggerUI();     // Enable Swagger UI
//Swagger:end

app.UseHttpsRedirection();   // Enforce HTTPS
app.UseAuthentication();     // Activate JWT authentication middleware
app.UseAuthorization();      // Enforce authorization policies
app.MapControllers();        // Map controller endpoints

app.Run();
