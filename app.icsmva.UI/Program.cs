using app.icsmva.DAO.dao.app.Role;
using app.icsmva.DAO.dao.app.user;
using app.icsmva.DAO.dao.Application;
using app.icsmva.DAO.dao.Privilege;
using app.icsmva.DAO.dao.RolesAndPrivilegeMap;
using app.icsmva.DAO.dao.userRole;
using app.icsmva.DAO.dao.users;
using app.icsmva.DAO.Handlers;
using app.icsmva.Models;
using app.icsmva.UI.CurrentUser;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration()
     .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(evt => evt.Level == LogEventLevel.Error)
   .WriteTo.File("C:\\logsFile\\" + DateTime.Now.Year.ToString() + "\\" + DateTime.UtcNow.ToString("MM-yyyy") + "\\" + DateTime.UtcNow.ToString("dd-MM-yyyy") + "\\error.txt",
            rollingInterval: RollingInterval.Minute))
    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(evt => evt.Level == LogEventLevel.Warning)
   .WriteTo.File("C:\\logsFile\\" + DateTime.Now.Year.ToString() + "\\" + DateTime.UtcNow.ToString("MM-yyyy") + "\\" + DateTime.UtcNow.ToString("dd-MM-yyyy") + "\\warning.txt",
            rollingInterval: RollingInterval.Day))
    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(evt => evt.Level == LogEventLevel.Information)
        .WriteTo.File("C:\\logsFile\\" + DateTime.Now.Year.ToString() + "\\" + DateTime.UtcNow.ToString("MM-yyyy") + "\\" + DateTime.UtcNow.ToString("dd-MM-yyyy") + "\\info.txt",
            rollingInterval: RollingInterval.Day))
    .CreateLogger();
// Add services to the container.
builder.Services.AddControllersWithViews();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<icsmvaDBContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = new PathString("/login/Index");
    options.AccessDeniedPath = new PathString("/login/AccessDenied");
    options.LogoutPath = new PathString("/login/Logout");
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddRazorPages();
builder.Services.AddMemoryCache();
builder.Services.AddTransient<IPrivilege, PrivilegeService>();
builder.Services.AddTransient<IApplicationName, ApplicationService>();
builder.Services.AddTransient<IUsersRoles, UsersRolesService>();
builder.Services.AddTransient<IUsers, UserService>();
builder.Services.AddSingleton<Iappusers, appuserService>();
builder.Services.AddSingleton<IAppRole, appRoleService>();
builder.Services.AddTransient<IRolePrivilegemap, RolePrivilegemapService>();
builder.Services.AddTransient<ICurentUserGet, CurentUserService>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();    
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "areaRoute",
        pattern: "{area:exists}/{controller}/{action}",
        defaults: new { action = "Index" });

    endpoints.MapControllerRoute(
        name: "default",
    pattern: "{controller=login}/{action=Index}/{id?}");


    endpoints.MapRazorPages();
});
app.Run();
