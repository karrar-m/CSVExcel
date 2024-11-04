using CSVExcel.Interface;
using CSVExcel.Model.Employee;
using CSVExcel.Service;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<EmployeeValidator, EmployeeValidator>(); // This remains the same
builder.Services.AddScoped<IEmployeeService, EmployeeService>(); // This remains the same

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
