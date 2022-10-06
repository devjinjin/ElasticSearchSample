using ElasticSearchSample.Data;
using ElasticSearchSample.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddTransient<WeatherForecastService>();

builder.Services.AddControllers();
builder.Services.ConfigureSwagger();
builder.Services.AddHttpClient();

builder.Services.AddElasticsearch(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{

    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else {
    app.UseSwagger();
    //app.UseSwaggerUI();
    app.UseSwaggerUI();

}


app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();


app.UseEndpoints(endpoints =>
{
    endpoints.MapFallbackToPage("/_Host");
    endpoints.MapBlazorHub();
    endpoints.MapControllers();

});

app.Run();
