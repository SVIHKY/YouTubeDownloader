var builder = WebApplication.CreateBuilder(args);

// P�idej podporu pro Controllers + Views
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Nastaven� middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// V�choz� trasa pro MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); // Domovsk� str�nka = HomeController, metoda Index

app.Run();
