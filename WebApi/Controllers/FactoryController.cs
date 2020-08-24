﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace WebApi
{
    //[Route("api/[controller]")]
    [ApiController]
    public class FactoryController : ControllerBase
    {
        private readonly FactoryRepository repo;
        private readonly IMapper mapper;
        public FactoryController(FactoryRepository repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        // GET: api/factories
        [HttpGet("api/factories")]
        public IEnumerable<FactoryOdt> GetFactories()
        {
            var factories = repo.GetAll().Result;
            var ods = new List<FactoryOdt>();
            foreach(Factory factory in factories)
            {
                var odt = mapper.Map<Factory, FactoryOdt>(factory);
                odt.Units = mapper.Map<ICollection<Unit>, ICollection<UnitListOdt>>(factory.Units);
                ods.Add(odt);
            }
            return ods;
        }

        // GET api/factories/5
        [HttpGet("api/factories/{id}")]
        public FactoryOdt GetFactoryById(int id)
        {
            var factory = repo.GetId(id).Result;
            var odt = mapper.Map<Factory, FactoryOdt>(factory);
            odt.Units = mapper.Map<ICollection<Unit>, ICollection<UnitListOdt>>(factory.Units);
            return odt;
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
