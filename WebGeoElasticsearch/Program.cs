using WebGeoElasticsearch.ElasticsearchApi;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddElasticsearchClient("elasticsearch");

builder.Services.AddScoped<SearchProvider>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.MapDefaultEndpoints();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
