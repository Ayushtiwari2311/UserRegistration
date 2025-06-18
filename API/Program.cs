using API.Middlewares;
using Application.DependencyInjection;
using Domain.DTOs;
using Infrastructure.DependencyInjection;
using Infrastructure.SeriLog;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationService();
builder.Services.AddInfrastructureService(builder.Configuration);


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("https://localhost:7059")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(x => x.Value?.Errors.Count > 0)
            .SelectMany(kvp => kvp.Value!.Errors.Select(e => $"<li><strong>{kvp.Key}</strong>: {e.ErrorMessage}</li>"));

        var htmlList = $"<ul style='text-align: left; margin-left: 1.5em;'>{string.Join("", errors)}</ul>";

        return new OkObjectResult(ResponseDTO.Fail(htmlList));
    };
});

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .MinimumLevel.Error() // Only log errors and above
        .WriteTo.Console()
        .WriteTo.Sink(new EFSerilogSink(services));
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors(MyAllowSpecificOrigins);
app.UseMiddleware<ErrorLoggingMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
