using SistemaVenta.IOC;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using SistemadeVenta.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor(); 

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Hemos creado una extension nuestra y le estamos pasando a nuestro sistema nuestra configuracion
//le vamos a pasa la confuiguracion de nuestra aplicacion. porque el solo de maneja en el entorno de appsettings
builder.Services.InyectarDependencias(builder.Configuration);
builder.Services.AddControllersWithViews();

builder.Services.AddCors(p =>
{
   p.AddPolicy("NuevaPolitica", app =>
    {
        app.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("NuevaPolitica");

app.UseAuthorization();

app.MapControllers();

app.Run();
/*
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
   
}
app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseCors("NuevaPolitica");

app.UseAuthorization();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});
//rEDIRECT REQUEST TO THE ROOT URL TO THE SWAGGER UI

app.Use(async (Context, next) =>
{
    if (Context.Request.Path == "/")
    {
        Context.Response.Redirect("/swagger/index.html");
        return;
    }
});
app.Run();*/
