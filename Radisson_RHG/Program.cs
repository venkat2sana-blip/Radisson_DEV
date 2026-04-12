using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Options;
using Radisson_RHG.Controllers;
using Radisson_RHG.Repositories;
using Radisson_RHG.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Radisson_RHG;
using FluentValidation.AspNetCore;
using FluentValidation;
using Radisson_RHG.Validators;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//for patch method implementation adding addnewtonsoftjson

builder.Services.AddControllers().AddNewtonsoftJson();

// adding this below line
builder.Services.AddFluentValidationAutoValidation()
                .AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// adding this below lines
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// adding this also
builder.Services.AddScoped<IRegistrationRepository,RegistrationRepo>();
builder.Services.AddScoped<IRegistrationInterface,Registractionservices>();

//adding this also
builder.Services.AddScoped<IRepositoryUserInterface, UserRepository>();
builder.Services.AddScoped<IUserAuthServices, UserAuthServices>();
var jwt = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwt["key"]);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
  .AddJwtBearer(Options =>
  {
      Options.RequireHttpsMetadata = true;
      Options.SaveToken = true;
      Options.TokenValidationParameters = new TokenValidationParameters
      {
          ValidateIssuer=true,
          ValidateAudience=true,
          ValidateIssuerSigningKey=true,
          ValidIssuer = jwt["Issuer"],
          ValidAudience = jwt["Audience"],
          IssuerSigningKey=new SymmetricSecurityKey(key)
      };
  });
builder.Services.AddAuthorization();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();
//this is writing i am
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userRepo = services.GetRequiredService<IRepositoryUserInterface>();

    const string adminUserName = "admin";
    if (userRepo!.GetByUserName(adminUserName) == null)
    {
        var adminPassword = "Admin@123";
        var hash = BCrypt.Net.BCrypt.HashPassword(adminPassword);
        var admin = new User
        {
            UserName = adminUserName,
            Email = "admin@example.com",
            PasswordHash = hash,
            CreatedOn = DateTime.UtcNow
        };
        userRepo.Create(admin);

    }

}




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.UseSwagger();
        app.UseSwaggerUI();
    }

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();
app.UseRouting();

app.MapControllers();

app.Run();
