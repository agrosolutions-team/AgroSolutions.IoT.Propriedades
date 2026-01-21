using AgroSolutions.IoT.Propriedades.Application.Services;
using AgroSolutions.IoT.Propriedades.Domain.Contracts;
using AgroSolutions.IoT.Propriedades.Infrastructure.Data;
using AgroSolutions.IoT.Propriedades.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING") 
                    ?? builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<PropriedadesDbContext>(options =>
    options.UseNpgsql(connectionString, npgsqlOptions =>
    {
        npgsqlOptions.MigrationsAssembly("AgroSolutions.IoT.Propriedades.Infrastructure");
    })
    .EnableSensitiveDataLogging(builder.Environment.IsDevelopment())
);

builder.Services.AddScoped<IPropriedadeRepository, PropriedadeRepository>();
builder.Services.AddScoped<ITalhaoRepository, TalhaoRepository>();
builder.Services.AddScoped<PropriedadeService>();
builder.Services.AddScoped<TalhaoService>();

var jwtSecretKey = builder.Configuration["JwtSettings:SecretKey"] 
                ?? throw new InvalidOperationException("JWT SecretKey não configurada");

var key = System.Text.Encoding.UTF8.GetBytes(jwtSecretKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = builder.Configuration.GetValue<bool>("JwtSettings:ValidateIssuer"),
            ValidateAudience = builder.Configuration.GetValue<bool>("JwtSettings:ValidateAudience"),
            ValidateLifetime = builder.Configuration.GetValue<bool>("JwtSettings:ValidateLifetime"),
            ValidateIssuerSigningKey = builder.Configuration.GetValue<bool>("JwtSettings:ValidateIssuerSigningKey"),
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers();

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "AgroSolutions - API de Propriedades", 
        Version = "v1",
        Description = "API para gerenciamento de propriedades rurais e talhões"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando Bearer scheme. Exemplo: 'Bearer {token}'",
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
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PropriedadesDbContext>();
    dbContext.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AgroSolutions API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
