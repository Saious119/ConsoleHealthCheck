using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ConsoleHealthCheck.Models;
using ConsoleHealthCheck.Services;

namespace ConsoleHealthCheck.Controllers;

public class ScheduleController : ControllerBase
{
    private readonly SchedulerService _schedulerService;

    public ScheduleController(SchedulerService schedulerService)
    {
        _schedulerService = schedulerService;
    }
    public IActionResult CreateSchedule(string consoleName, string cronExpression)
    {
        _schedulerService.ScheduleJob(consoleName, cronExpression);
        return RedirectToAction("Index");
    }
}