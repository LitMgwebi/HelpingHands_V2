using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using HelpingHands_V2.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var emailer = builder.Configuration.GetSection("Emailer").Get<Emailer>();
QuestPDF.Settings.License = LicenseType.Community;

builder.Services.AddDbContext<Grp0444HelpingHandsContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddSingleton(emailer);
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IEndUser, EndUserService>();
builder.Services.AddScoped<IReport, ReportService>();
builder.Services.AddScoped<INurse, NurseService>();
builder.Services.AddScoped<ICity, CityService>();
builder.Services.AddScoped<ISuburb, SuburbService>();
builder.Services.AddScoped<IVisit, VisitService>();
builder.Services.AddScoped<IWound, WoundService>();
builder.Services.AddScoped<IBusiness, BusinessService>();
builder.Services.AddScoped<IOperation, OperationService>();
builder.Services.AddScoped<ICondition, ConditionService>();
builder.Services.AddScoped<IContract, ContractService>();
builder.Services.AddScoped<IPatient, PatientService>();
builder.Services.AddScoped<IPrefferedSuburb, PrefferedSuburbService>();
builder.Services.AddScoped<IPatientCondition, PatientConditionService>();
builder.Services.AddScoped<IEmailSender, EmailSenderService>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromHours(2);
    options.SlidingExpiration = true;
    options.AccessDeniedPath = "/Pending";
    options.LoginPath = "/EndUser/Login";
});

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

app.UseAuthentication();
app.UseAuthorization();

var cookiePolicyOptions = new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
};
app.UseCookiePolicy(cookiePolicyOptions);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
