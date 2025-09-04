using E_CommerceSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceSystem.Controllers
{
    [Authorize(Roles = "admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class LogController : ControllerBase
    {
       
            private readonly IAppLogger<LogController> _logger;
            private readonly IWebHostEnvironment _environment;

            public LogController(IAppLogger<LogController> logger, IWebHostEnvironment environment)
            {
                _logger = logger;
                _environment = environment;
            }

            [HttpGet("files")]
            public IActionResult GetLogFiles()
            {
                try
                {
                    var logPath = Path.Combine(_environment.WebRootPath, "logs");
                    if (!Directory.Exists(logPath))
                        return Ok(new { Message = "No log files found" });

                    var logFiles = Directory.GetFiles(logPath, "*.txt")
                        .Select(f => new FileInfo(f))
                        .OrderByDescending(f => f.CreationTime)
                        .Select(f => new
                        {
                            Name = f.Name,
                            Size = f.Length,
                            Created = f.CreationTime,
                            LastModified = f.LastWriteTime
                        });

                    return Ok(logFiles);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error retrieving log files");
                    return StatusCode(500, "Error retrieving log files");
                }
            }

            [HttpGet("file/{fileName}")]
            public IActionResult GetLogFileContent(string fileName)
            {
                try
                {
                    var logPath = Path.Combine(_environment.WebRootPath, "logs");
                    var filePath = Path.Combine(logPath, fileName);

                    if (!System.IO.File.Exists(filePath))
                        return NotFound("Log file not found");

                    // Basic security check to prevent directory traversal
                    if (!filePath.StartsWith(logPath))
                        return BadRequest("Invalid file path");

                    var content = System.IO.File.ReadAllText(filePath);
                    return Ok(content);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error reading log file: {FileName}", fileName);
                    return StatusCode(500, "Error reading log file");
                }
            }
        
    }
}
