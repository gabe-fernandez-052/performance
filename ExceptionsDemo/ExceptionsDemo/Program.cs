using ExceptionsDemo.Database;
using ExceptionsDemo.Exceptions;
using ExceptionsDemo.Repository;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseInMemoryDatabase("Database");
});

builder.Services.AddScoped(service => new ProductRepository(service.GetRequiredService<ApplicationDbContext>()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(options =>
{
    options.Run(async context =>
    {
        context.Response.ContentType = "application/json";

        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

        if (contextFeature != null)
        {
            var baseException = contextFeature.Error?.GetBaseException();

            var problemDetails = baseException switch
            {
                ProductNotValidException nve => new ProblemDetails
                {
                    Title = "BadRequest",
                    Detail = nve.Message,
                    Status = StatusCodes.Status400BadRequest
                },
                ProductNotFoundException nfe => new ProblemDetails
                {
                    Title = "NotFound",
                    Detail = nfe.Message,
                    Status = StatusCodes.Status404NotFound
                },
                Exception e => new ProblemDetails
                {
                    Title = "Server Error",
                    Detail = e.Message,
                    Status = StatusCodes.Status500InternalServerError
                },
                _ => new ProblemDetails()
            };

            context.Response.StatusCode = (int)problemDetails.Status;

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    });
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();