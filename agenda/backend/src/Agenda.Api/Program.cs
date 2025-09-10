// Agenda.Api/Program.cs
using System.Text;
using System.Text.Json.Serialization;
using Agenda.Application.Contacts.Handlers;
using Agenda.Application.Contacts.Validators;
using Agenda.Infrastructure.Messaging;
using Agenda.Infrastructure.Persistence;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var cfg = builder.Configuration;

// EF
builder.Services.AddDbContext<AgendaDbContext>(o =>
    o.UseNpgsql(cfg.GetConnectionString("DefaultConnection")));

// DI
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddSingleton<RabbitPublisher>();
builder.Services.AddScoped<ContactReadRepository>();

// MediatR (varre Assembly da Application)
builder.Services.AddMediatR(typeof(CreateContactHandler).Assembly);

// FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateContactValidator>();

// AutoMapper (se/quando precisar)
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Auth JWT simples
var key = Encoding.ASCII.GetBytes(cfg["Jwt:Key"]!);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Agenda API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Ex: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        { new OpenApiSecurityScheme{ Reference = new OpenApiReference{ Type = ReferenceType.SecurityScheme, Id = "Bearer"}}, new string[]{} }
    });
});

var app = builder.Build();

// Migrate auto (dev)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AgendaDbContext>();
    db.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
