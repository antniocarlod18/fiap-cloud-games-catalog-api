using FiapCloudGamesCatalog.Api.Authorize;
using FiapCloudGamesCatalog.Api.Endpoints;
using FiapCloudGamesCatalog.Api.Extensions;
using FiapCloudGamesCatalog.Application.EventHandler;
using FiapCloudGamesCatalog.Application.Middlewares;
using FiapCloudGamesCatalog.Application.Services;
using FiapCloudGamesCatalog.Application.Services.Interfaces;
using FiapCloudGamesCatalog.Application.Validators;
using FiapCloudGamesCatalog.Domain.Abstractions;
using FiapCloudGamesCatalog.Domain.Repositories;
using FiapCloudGamesCatalog.Infra.Data.Context;
using FiapCloudGamesCatalog.Infra.Data.Messaging;
using FiapCloudGamesCatalog.Infra.Data.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.AddElasticConfiguration();
builder.AddMassTransitConfiguration();

var serverVersion = new MySqlServerVersion(new Version(8, 0));
builder.Services.AddDbContext<ContextDb>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("MySQL"), serverVersion);
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IAuditService, AuditService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<ILibraryService, LibraryService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPromotionService, PromotionService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddAuthentication(opt => {
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Authentication:Issuer"],
        ValidAudience = builder.Configuration["Authentication:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Authentication:Key"])),
        RoleClaimType = ClaimTypes.Role
    };
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SameUserOrAdmin", policy =>
        policy.Requirements.Add(new SameUserRequirement()));
});
builder.Services.AddScoped<IAuthorizationHandler, SameUserHandler>();
builder.Services.AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();
builder.Services.AddValidatorsFromAssemblyContaining<GameRequestDtoValidator>();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(OrderCanceledEventHandler).Assembly);
});

var app = builder.Build();
app.UseMiddleware<ExceptionHandlerMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHsts();
app.UseHttpsRedirection();

app.MapAuditEndpoints();
app.MapCartEndpoints();
app.MapGameEndpoints();
app.MapLibraryEndpoints();
app.MapOrderEndpoints();
app.MapPromotionEndpoints();

app.Run();
