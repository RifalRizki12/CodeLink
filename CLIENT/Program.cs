using CLIENT.Contract;
using CLIENT.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllersWithViews();

builder.Services.AddScoped(typeof(IRepository<,>), typeof(GeneralRepository<,>));
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<ISkillRepository, SkillRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IInterviewRepository, InterviewRepository>();

builder.Services.AddScoped<IHomeClientRepository, HomeClientRepository>();

//builder JWT yang berfungsi sebagai authenticaion pada saat pengguna ingin mengakses method method yang ada pada program
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JWTService:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWTService:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTService:SecretKey"])),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

/*app.UseStatusCodePagesWithReExecute("/Home/Error", "?code={0}");*/

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseRouting();

// Custome Error page
app.UseStatusCodePages(async context =>
{
    var response = context.HttpContext.Response;

    if (response.StatusCode.Equals((int)HttpStatusCode.Unauthorized))
    {
        response.Redirect("/unauthorized");
    }
    else if (response.StatusCode.Equals((int)HttpStatusCode.NotFound))
    {
        response.Redirect("/notfound");
    }
    else if (response.StatusCode.Equals((int)HttpStatusCode.Forbidden))
    {
        response.Redirect("/forbidden");
    }
});

app.UseSession();

//Add JWToken to all incoming HTTP Request Header
app.Use(async (context, next) =>
{
    var JWToken = context.Session.GetString("JWToken");
    if (!string.IsNullOrEmpty(JWToken))
    {
        context.Request.Headers.Add("Authorization", "Bearer " + JWToken);
    }
    await next();
});

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=HomeClient}/{action=HomeClient}/{id?}");

app.Run();
