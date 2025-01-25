using ConsoleHealthCheck.Services;
using Quartz;
using Quartz.Impl;
using Quartz.Simpl;
using Quartz.Spi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
builder.Services.AddSingleton<IJobFactory, SimpleJobFactory>();
builder.Services.AddSingleton<ISchedulerService, SchedulerService>();
builder.Services.AddSingleton<IDiscordBotService, DiscordBotService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

var schedulerService = app.Services.GetRequiredService<ISchedulerService>();
await schedulerService.StartAsync();
var discordBotService = app.Services.GetRequiredService<IDiscordBotService>();
await discordBotService.MainAsync();

app.Run();