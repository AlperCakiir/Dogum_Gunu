using Hafi_Abi_Doğum_Günü.Data;
using Hafi_Abi_Doğum_Günü.Hubs;
using Hafi_Abi_Doğum_Günü.Repositories.EfCore;
using Hafi_Abi_Doğum_Günü.Repositories.Interface;
using Hafi_Abi_Doğum_Günü.Services.Implementations;
using Hafi_Abi_Doğum_Günü.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. MVC ve SignalR
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

// 2. PostgreSQL DbContext Bağlantısı
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. Repository Katmanı DI Kayıtları
builder.Services.AddScoped<IRsvpRepository, RsvpRepository>();
builder.Services.AddScoped<IQuizRepository, QuizRepository>();

// 4. Service Katmanı DI Kayıtları
builder.Services.AddScoped<IRsvpService, RsvpService>();
builder.Services.AddScoped<IQuizService, QuizService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// SignalR Hub Rotası
app.MapHub<LobbyHub>("/lobbyHub");

app.Run();