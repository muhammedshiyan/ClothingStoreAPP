using ClothingStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Database connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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
    pattern: "{controller=Home}/{action=Index}/{id?}");

//using (var scope = app.Services.CreateScope())
//{
//    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//    if (!context.Products.Any())
//    {
//        context.Products.AddRange(
//            new ClothingStore.Core.Entities.Product { Name = "T-Shirt", Category = "Men", Price = 499, StockQuantity = 50 },
//            new ClothingStore.Core.Entities.Product { Name = "Dress", Category = "Women", Price = 899, StockQuantity = 30 }
//        );
//        context.SaveChanges();
//    }
//}

app.Run();
