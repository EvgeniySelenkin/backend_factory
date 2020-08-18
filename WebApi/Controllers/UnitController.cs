using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class UnitController : ControllerBase
    {
        private readonly DBContext context;
        public UnitController(DBContext context)
        {
            this.context = context;
        }

        // GET: api/units
        [HttpGet("api/units")]
        public IEnumerable<Unit> GetUnits()
        {
            return context.Unit.ToList();
        }

        // GET api/units/5
        [HttpGet("api/units/{id}")]
        public async Task<IList<Unit>> GetUnitById(int id)
        {
            return await context.Unit
                .Where(x => x.Id == id)
                .ToListAsync();
        }
    }
}
