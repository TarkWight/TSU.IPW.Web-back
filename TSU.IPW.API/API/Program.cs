using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TSU.IPW.API.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Регистрация сервисов
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
    c.EnableAnnotations();
});

// Настройка CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()  // Позволяет любые источники
                   .AllowAnyMethod()  // Позволяет любые методы (GET, POST, PUT и т.д.)
                   .AllowAnyHeader(); // Позволяет любые заголовки
        });
});

var app = builder.Build();

// Применение миграций
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dbContext = services.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.Migrate();
        if (!dbContext.Database.GetPendingMigrations().Any())
        {
            dbContext.Database.EnsureCreated();
            Console.WriteLine("Automatically applied migration.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error occurred while migrating: {ex.Message}");
    }
}

// Включение CORS
app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
