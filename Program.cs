using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShopNest.Services.User.Model;
using ShopNest.Services.Login.Model; // Add the namespace for LoginDbContext
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// CORS configuration (adjust localhost port if needed)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policyBuilder =>
    {
        policyBuilder.WithOrigins("http://localhost:4200")  // Angular app URL
                     .AllowAnyMethod()
                     .AllowAnyHeader()
                     .AllowCredentials();  // Allow credentials if necessary
    });
});

// Add services.
builder.Services.AddControllers();

// Configure DbContext with the connection string from appsettings.json
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnStr")));

// Add LoginDbContext
builder.Services.AddDbContext<LoginDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnStr"))); // Using the same connection string, change if needed

// JWT Authentication configuration
var key = builder.Configuration["Jwt:SecretKey"];  // JWT Secret Key from appsettings.json
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;  // Disable if using HTTP (only for development)
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),  // Convert secret key to byte array
        ValidateIssuer = false,  // Optional: set to true and configure Issuer if needed
        ValidateAudience = false,  // Optional: set to true and configure Audience if needed
        ValidateLifetime = true,  // Ensure token hasn't expired
        ClockSkew = TimeSpan.Zero  // Optional: set to zero to ensure exact expiration time
    };
});

// Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Use CORS policy
app.UseCors("AllowSpecificOrigin");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use authentication and authorization
app.UseHttpsRedirection();
app.UseAuthentication();  // Ensure JWT middleware is used before authorization
app.UseAuthorization();

app.MapControllers();

app.Run();
