using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using madwords.Repos;
using madwords.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace madwords.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MadwordTemplateController : ControllerBase
    {
        readonly IMadwordRepository repo;

        public MadwordTemplateController(IMadwordRepository r)
        {
            repo = r;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var madwordTemplates = repo.MadwordTemplates.ToList<MadwordTemplate>();
            if (madwordTemplates.Count == 0)
            {
                return NotFound(); // status code 404
            }
            else
            {
                return Ok(madwordTemplates);
            }
        }
    }
}
