using API.Contracts;
using API.Data;
using API.Repositories;
using API.Utilities.Handler;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<CodeLinkDbContext>(option => option.UseSqlServer(connectionString));

// Add email service to the container
builder.Services.AddTransient<IEmailHandler, EmailHandler>(_ => new EmailHandler(
                                                            builder.Configuration["SmtpService:Host"],
                                                            int.Parse(builder.Configuration["SmtpService:Port"]),
                                                            builder.Configuration["SmtpService:FromEmailAddress"]));


builder.Services.AddScoped<ITokenHandlers, API.Utilities.Handler.TokenHandler>();

// Add repositories to the container.
// Mendaftarkan AccountRepository sebagai implementasi IAccountRepository.
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IExperienceRepository, ExperienceRepository>();
builder.Services.AddScoped<ICurriculumVitaeRepository, CurriculumVitaeRepository>();
builder.Services.AddScoped<IRatingRepository, RatingRepository>();
builder.Services.AddScoped<IRatingRepository, RatingRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<ISkillRepository, SkillRepository>();
builder.Services.AddScoped<IInterviewRepository, InterviewRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x => {
    x.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Code Link",
        Description = "ASP.NET Core API 6.0"
    });
    x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    x.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

// Mengkonfigurasi layanan untuk pengendalian API (controllers)
builder.Services.AddControllers()
       .ConfigureApiBehaviorOptions(options =>
       {
           // Konfigurasi respons kustom untuk validasi yang gagal
           options.InvalidModelStateResponseFactory = context =>
           {
               // Mengambil semua pesan kesalahan validasi dari ModelState
               var errors = context.ModelState.Values
                                   .SelectMany(v => v.Errors)
                                   .Select(v => v.ErrorMessage);

               // Mengembalikan respons HTTP 400 Bad Request dengan pesan kesalahan validasi
               return new BadRequestObjectResult(new ResponseValidatorHandler(errors));
           };
       });

//Add FluentValidation Service
builder.Services.AddFluentValidationAutoValidation()
    .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

// JWT Authentication Configuration
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
       .AddJwtBearer(options =>
       {
           options.RequireHttpsMetadata = false; // for development only
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

// Add CORS
//Add CORS, bisa diterapkan pada projek yang sudah dideploy atau jika local harus menggunakan sln yg berbeda
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin(); // allow all request dari semua asal (domain) yang berbeda
        policy.AllowAnyHeader();  // allow penggunaan header apa pun dalam request
        policy.AllowAnyMethod(); // allow penggunaan metode HTTP GET, POST, DELETE, dan PUT
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
