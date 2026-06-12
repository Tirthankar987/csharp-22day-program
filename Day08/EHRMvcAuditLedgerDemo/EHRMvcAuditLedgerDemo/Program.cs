// Brings in EHRDbContext — our single gateway to the SQL Server database
using EHRMvcAuditLedgerDemo.Data;

// Entity Framework Core — the ORM that translates C# code into SQL queries
// Without this, you'd write raw SQL strings everywhere (fragile, injection-prone)
using Microsoft.EntityFrameworkCore;

// Creates the app builder — reads appsettings.json, environment variables,
// command-line args, and wires up the dependency injection container
var builder = WebApplication.CreateBuilder(args);

// Registers MVC + Razor Views into the DI container
// This single line enables controllers, model binding, validation, and tag helpers
// In a real hospital system, you'd also add: AddAuthorization(), AddAuthentication()
// because every PHI screen must verify WHO is asking before showing anything
builder.Services.AddControllersWithViews();

// Registers EHRDbContext as a scoped service (one instance per HTTP request)
// "Scoped" is critical — if it were Singleton, two users could share the same
// DB connection and see each other's uncommitted data (a HIPAA problem)
// Real hospital: connection string comes from Azure Key Vault, NOT appsettings.json
builder.Services.AddDbContext<EHRDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("EHRConnection"))
);

// Locks in all registrations and produces the runnable app object
// Nothing above this line executes at runtime — it's all configuration
var app = builder.Build();

// HIPAA-critical: in production, never show stack traces or error details
// A raw exception page reveals table names, column names, server paths —
// exactly what an attacker needs to plan a breach
// Real hospital: this redirects to a custom error page with only a reference ID,
// while the full error is silently written to a HIPAA-compliant audit log
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

// Redirects all HTTP → HTTPS automatically
// In healthcare, transmitting PHI over plain HTTP is a HIPAA violation
// Real hospital: TLS 1.2 minimum enforced at the load balancer too — double protection
app.UseHttpsRedirection();

// Serves static files (CSS, JS, images) from wwwroot/
// Real hospital: Content-Security-Policy headers added here to prevent
// malicious scripts from injecting into PHI-displaying pages (XSS attacks)
app.UseStaticFiles();

// Matches the incoming URL to the right controller + action
// Must come BEFORE UseAuthorization — order matters in the middleware pipeline
// Wrong order = authorization checks never run = open access to patient data
app.UseRouting();

// Real hospital: app.UseAuthentication() and app.UseAuthorization() go HERE
// Authentication = "who are you?" (login, JWT token, SSO)
// Authorization = "are you allowed to see this patient's record?"
// This demo skips both — a real EHR would never skip either

// Defines the default URL pattern: /Controller/Action/OptionalId
// {id?} — the ? means optional, so /Transaction/Create works without an ID
// Real hospital: role-based routes added here so /Admin/* requires admin role,
// /Patient/* requires at minimum a nurse or doctor role
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Transaction}/{action=Create}/{id?}");

// Starts the Kestrel web server and begins listening for requests
// Everything above was setup — this is the line that actually opens the door
app.Run();
