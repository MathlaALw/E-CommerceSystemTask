using E_CommerceSystem.Models;
using E_CommerceSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace E_CommerceSystem.Controllers
{
    [Authorize(Roles = "admin")] // Only admin can access these endpoints
    [ApiController]
    [Route("api/[Controller]")]

    public class ReportController
    {
    }
}
