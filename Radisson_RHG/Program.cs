using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Options;
using Radisson_RHG.Controllers;
using Radisson_RHG.Repositories;
using Radisson_RHG.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//for patch method implementation adding addnewtonsoftjson

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// adding this below lines
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// adding this also
builder.Services.AddScoped<IRegistrationRepository,RegistrationRepo>();

// adding this also
builder.Services.AddScoped<IRegistrationInterface,Registractionservices>();


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseRouting();

app.MapControllers();

app.Run();
