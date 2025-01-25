using Microsoft.AspNetCore.Mvc;
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
        try
        {
            _schedulerService.ScheduleJob(consoleName, cronExpression);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}