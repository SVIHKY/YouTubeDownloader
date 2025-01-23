var builder = WebApplication.CreateBuilder(args);

// Pøidej podporu pro Controllers + Views
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Nastavení middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Výchozí trasa pro MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); // Domovská stránka = HomeController, metoda Index

app.Run();
