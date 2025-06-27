using API.Configurations;
using API.DependencyInjection;
using API.Handler;
using Application.DependencyInjection;
using DataTransferObjects.Response.Common;
using Infrastructure.DependencyInjection;
using Infrastructure.SeriLog;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAPIService();
builder.Services.AddApplicationService();
builder.Services.AddInfrastructureService(builder.Configuration);
builder.Services.AddCustomModelValidationResponse(builder.Environment);
builder.Services.AddRateLimiterPolicy(builder.Environment);
builder.Services.AddExceptionHandler<ExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddHttpContextAccessor();

builder.Services.AddCorsPolicy(builder.Environment);

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
app.UseCors(CorsConfig.GetPolicyName());
app.UseRateLimiter();
app.UseExceptionHandler();

app.UseAuthorization();

app.MapControllers();

app.Run();
