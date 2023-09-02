using Microsoft.EntityFrameworkCore;

using ToDoList.Data;
using ToDoList.Entities;
using ToDoList.Repository;
using ToDoList.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var config  = builder.Configuration;
var sqlConnection = config.GetConnectionString("DefaultConnection");

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IService<TodoItem>, ItemService>();
builder.Services.AddScoped<IRepository<TodoItem>, ItemRepository>();
builder.Services.AddDbContext<ItemDbContext>(options => options.UseSqlServer(sqlConnection));

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
