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
        private readonly TankRepository repo;
        public TankController(TankRepository repo)
        {
            this.repo = repo;
        }

        // GET: api/tanks
        [HttpGet("api/tanks")]
        public async Task<IEnumerable<Tank>> GetTanks()
        {
            var tanks = await repo.GetAll();
            return tanks;
        }

        // GET api/tanks/5
        [HttpGet("api/tanks/{id}")]
        public async Task<Tank> GetTankById(int id)
        {
            return await repo.GetId(id);
        }

        [HttpPost("api/tanks")]
        public async Task PosTank(Tank tank)
        {
            await repo.Post(tank);
        }

        [HttpDelete("api/tanks/{id}")]
        public async Task DeleteTank(int id)
        {
            await repo.Delete(id);
        }

        [HttpPut("api/tanks")]
        public async Task UpdateFactory(Tank tank)
        {
            await repo.Update(tank);
        }


    }
}
