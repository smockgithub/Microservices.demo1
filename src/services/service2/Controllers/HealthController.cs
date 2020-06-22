﻿using Microsoft.AspNetCore.Mvc;

namespace service2.Controllers
{
    //Consul会通过call这个API来确认Service的健康状态
    [Produces("application/json")]
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok("ok");
    }
}
