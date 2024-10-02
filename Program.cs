using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using todoListApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(); // Swagger

// Registrar tus servicios personalizados
builder.Services.AddScoped<TaskService>(); // TaskService
builder.Services.AddScoped<PersonService>();

builder.Services.AddDbContext<TaskManagerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

// Configurar la tubería de solicitud HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers(); // Asegúrate de mapear los controladores


#region Conf CORS
app.UseCors(x =>
{
    if (builder.Environment.IsDevelopment())
    {
        x.AllowAnyHeader()
        .AllowAnyMethod()
        .SetIsOriginAllowed(origin => true) // allow any origin
        .WithMethods("GET", "POST", "PUT")
        .AllowCredentials();
    }
    else
    {
        x.AllowAnyHeader()
        .WithOrigins("*") //Specify url
        .AllowAnyMethod() // Permitir cualquier método
        .AllowCredentials();
    }
});
#endregion

app.Run();