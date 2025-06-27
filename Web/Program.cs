using Microsoft.Extensions.Options;
using System.Net;
using Web.DependencyInjection;
using Web.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

builder.Services.AddHttpClient("ApiClient", (serviceProvider, client) =>
{
    var settings = serviceProvider.GetRequiredService<IOptions<ApiSettings>>().Value;
    //client.BaseAddress = new Uri(settings.BaseUrl);
    client.BaseAddress = new Uri("https://localhost:7155/api");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});
//.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
//{
//    AllowAutoRedirect = true,
//    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
//    UseCookies = true
//});

builder.Services.AddWebService();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Index}");

app.Run();
