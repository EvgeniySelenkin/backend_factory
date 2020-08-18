using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi
{
    //[Route("api/[controller]")]
    [ApiController]
    public class FactoryController : ControllerBase
    {
        private readonly FactoryRepository repo;
        public FactoryController(FactoryRepository repo)
        {
            this.repo = repo;
        }

        // GET: api/factories
        [HttpGet("api/factories")]
        public async Task<IEnumerable<Factory>> GetFactories()
        {
            var factories = await repo.GetAll();
            return factories;
        }

        // GET api/factories/5
        [HttpGet("api/factories/{id}")]
        public async Task<Factory> GetFactoryById(int id)
        {
            return await repo.GetId(id);
        }

        [HttpPost("api/factories")]
        public async Task PostFactory(Factory factory)
        {
            await repo.Post(factory);
        }

        [HttpDelete("api/factories/{id}")]
        public async Task DeleteFactory(int id)
        {
            await repo.Delete(id);
        }

        [HttpPut("api/factories")]
        public async Task UpdateFactory(Factory factory)
        {
            await repo.Update(factory);
        }
    }
}
