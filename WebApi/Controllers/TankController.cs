using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class TankController : ControllerBase
    {
        private readonly DBContext context;

        public TankController(DBContext context)
        {
            this.context = context;
        }

        // GET: api/tanks
        [HttpGet("api/tanks")]
        public IEnumerable<Tank> GetTanks()
        {
            var tanks = context.Tank.ToList();
            return tanks;
        }

        // GET api/tanks/5
        [HttpGet("api/tanks/{id}")]
        public async Task<IList<Tank>> GetTankById(int id)
        {
            return await context.Tank
                .Where(x => x.Id == id)
                .ToListAsync();
        }


    }
}
