using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using user_auth.models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors((option) =>
{
    option.AddPolicy("DevCors", (corsBuilder) =>
    {
        corsBuilder.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
    });
});




JWtSettings? jwtSettings = builder.Configuration.GetSection("Jwt").Get<JWtSettings>();
SymmetricSecurityKey tokenKey = new(Encoding.UTF8.GetBytes(jwtSettings != null ? jwtSettings.TokenKey : ""));

TokenValidationParameters tokenValidationParameters = new()
{
    IssuerSigningKey = tokenKey,
    ValidateIssuer = false,
    ValidateIssuerSigningKey = false,
    ValidateAudience = false
};

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = tokenValidationParameters;
});

var app = builder.Build();
app.UseCors("DevCors");


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}
app.MapControllers();
app.Run();