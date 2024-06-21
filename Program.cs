using HotelBillingMVC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<Hotel_Billing_SystemContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("Dbconnection")).EnableSensitiveDataLogging());
builder.Services.AddSession(options =>
{
    // Set session timeout to 30 minutes
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
//builder.Services.AddMvc().AddJsonOptions(
//                options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore

//    //option=>option.=Newtonsoft.Json.ReferenceLoopHandling.Ignore
//    );


//builder.Services.AddDefaultIdentity<Usermaster>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<Hotel_Billing_SystemContext>();



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
app.UseSession();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
