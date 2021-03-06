using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Odt;

namespace WebApi.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class TankController : ControllerBase
    {
        private readonly TankRepository repo;
        private readonly IMapper mapper;
        public TankController(TankRepository repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }
        [Authorize]
        // GET: api/tanks
        [HttpGet("api/tanks")]
        public async Task<IEnumerable<TankOdt>> GetTanks()
        {
            var tanks = await repo.GetAll();
            var ods = new List<TankOdt>();
            foreach(Tank tank in tanks)
            {
                var odt = mapper.Map<TankOdt>(tank);
                //odt.Unit = mapper.Map<UnitListOdt>(tank.Unit);
                ods.Add(odt);
            }
            return ods;
        }
        [Authorize]
        // GET api/tanks/5
        [HttpGet("api/tanks/{id}")]
        public async Task<TankOdt> GetTankById(int id)
        {
            var tank = await repo.GetId(id);
            var odt = mapper.Map<TankOdt>(tank);
            odt.Unit = mapper.Map<UnitListOdt>(tank.Unit);
            return odt;
        }
        [Authorize(Roles = "admin")]
        [HttpPost("api/tanks")]
        public void PostTank(Tank tank)
        {
            repo.Post(tank);
        }
        [Authorize(Roles = "admin")]
        [HttpDelete("api/tanks/{id}")]
        public async Task DeleteTank(int id)
        {
            var tank = await GetTankById(id);
            if (tank == null)
                NotFound();
            await repo.Delete(id);
        }
        [Authorize(Roles = "admin")]
        [HttpPut("api/tanks")]
        public async Task UpdateTank(Tank tank)
        {
            if (await FindTankById(tank.Id))
                NotFound();
            await repo.Update(tank);
        }

        private async Task<bool> FindTankById(int id)
        {
            return await repo.FindId(id);
        }
    }
}
