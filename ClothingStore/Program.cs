using Microsoft.EntityFrameworkCore;
using ClothingStore.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
string dbConnStr = builder.Configuration.GetConnectionString("ClothingShop");
builder.Services.AddDbContext<ClothingShopContext>(options => options.UseSqlServer(dbConnStr));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
