using Application.Extensions;
using ClinicManagement.API.Application.Models;
using ClinicManagement.API.Domain.Entities;
using ClinicManagement.API.Extensions;
using ClinicManagement.API.Infrastructure.Data;
using ClinicManagement.API.Middleware;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerExtension();

// Add JWT Bearer Configuration
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JWT"));

// Õﬁ‰ Œœ„«  ÿ»ﬁ… «· Infrastructure
builder.Services.AddInfrastructureServices(builder.Configuration);

// Õﬁ‰ Œœ„«  ÿ»ﬁ… «·  Application
builder.Services.AddApplicationServices();
builder.Services.AddIdentityServices();

// Õﬁ‰ «·Œœ„«  «·Œ«’… »  API Ê«·Ê”Ìÿ
builder.Services.AddAuthService(builder.Configuration);

builder.Services.AddTransient<ExceptionMiddleware>();

var app = builder.Build();

using(var scope = app.Services.CreateScope())
{
    var roleManamger = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    await ContextSeed.SeedRolesAsync(roleManamger);
    await ContextSeed.CreateAdmin(userManager);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
