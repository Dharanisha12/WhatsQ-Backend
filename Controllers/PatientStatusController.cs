using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WhatsQ.Providers;
using WhatsQ.Models; // Ensure correct namespace

[Route("api/[controller]")]
[ApiController]
public class PatientStatusController : ControllerBase
{
    private readonly IPatientStatusProvider _provider;

    public PatientStatusController(IPatientStatusProvider provider)
    {
        _provider = provider;
    }

    // Endpoint to get today's patient status count
    [HttpGet("getTodayStats")]
    public async Task<IActionResult> GetTodayStats()
    {
        var stats = await _provider.GetPatientStatusForToday();

        if (stats != null && stats.Count > 0)
            return Ok(stats);

        return NotFound("No data found for today.");
    }
}
