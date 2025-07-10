using Formio.Areas.Admin.Connection;
using Formio.Areas.Admin.Repository;
using Formio.Areas.Admin.Services;
using Formio.Areas.Identity.Data;
using Formio.Areas.Identity.RoleInitilizar;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FormConnection")));



builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();

builder.Services.AddTransient<IFromRepository, FormRepository>();
builder.Services.AddTransient<IFormService, FormService>();

builder.Services.AddTransient<ISubmissionService, SubmissionService>();
builder.Services.AddTransient<ISubmissionRepository, SubmissionRepository>();
builder.Services.AddSingleton<IConnectionFactory, ConnectionFactory>();



var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await AssignRole.SeedRolesAndUsersAsync(services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.MapStaticAssets();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
     name: "Areas",
     pattern: "{area:exists}/{controller}/{action}/{id?}");


    endpoints.MapControllerRoute(
          name: "default",
          pattern: "{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "Admin",
        pattern: "Admin/{controller=Form}/{action=ViewAllForms}/{formGroupId?}");

    

});

app.MapRazorPages()
   .WithStaticAssets();

app.Run();
