using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using WeavyChat;
using WeavyChat.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//NJB-ini
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthorization();
builder.Services.Configure<MySettings>(builder.Configuration.GetSection("MySettings"));
MySettings options = new();
builder.Configuration.GetSection(nameof(MySettings))
    .Bind(options);
//builder.Services.AddSession(o =>
//{
//    o.IdleTimeout = TimeSpan.FromHours(100); //TODO: add value to config file
//    o.Cookie.HttpOnly = true;
//    o.Cookie.SameSite = SameSiteMode.Strict;
//    // Change for production enviroment of HTTPS
//    //o.Cookie.SecurePolicy = CookieSecurePolicy.Always;
//    o.Cookie.SecurePolicy = CookieSecurePolicy.None;
//    o.Cookie.IsEssential = true;
//});

builder.Services.AddCors(options => { options.AddPolicy("PoliticaCORS", app => 
    { app.AllowAnyOrigin() 
         .AllowAnyHeader()
         .AllowAnyMethod();
    }); 
});

//NJB-end

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("PoliticaCORS");

app.UseAuthorization();

//NJB
//app.UseSession();

app.MapControllers();

app.Run();
