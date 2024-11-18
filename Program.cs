using GraflowBackend.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<GraflowContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Database") ?? throw new Exception("Cannot find database connection string"));
});

builder.Services.AddIdentityCore<User>(options =>
{
    options.Password.RequiredLength = 8;
}).AddUserStore<UserStore>().AddSignInManager<SignInManager<User>>();

builder.Services.AddAuthentication().AddCookie("Identity.Application");


builder.Services.AddCors(options =>
{
    options.AddPolicy("Dev", opt =>
    {
        opt.WithOrigins("http://localhost:4200");
        opt.AllowAnyMethod();
        opt.AllowAnyHeader();
        opt.AllowCredentials();
    });

    options.AddPolicy("Prod", opt =>
    {
        opt.WithOrigins("");
        opt.AllowAnyMethod();
        opt.AllowAnyHeader();
        opt.AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors("Dev");
} 
else
{
    app.UseCors("Prod");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
