var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseDefaultFiles();   // index.html loads at /
app.UseStaticFiles();    // Serves .html .css .js from wwwroot

app.Run();
