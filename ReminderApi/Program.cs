using ApplicationDatabase.Context;
using Infrastructure.AppEnvironment;
using ReminderApi.Interfaces;
using ReminderApi.Interfaces.Pills;
using ReminderApi.Repositories.Pills;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<ApplicationContext>();

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddScoped<IPillRepository, PillRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();