using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using WebApi.Odt;

namespace WebApi
{
    public class UnitRepository
    {
        private readonly DBContext db;
        public UnitRepository(DBContext context)
        {
            db = context;
        }

        public async Task<IEnumerable<Unit>> GetAll()
        {
            return await db.Unit./*Include(u => u.Factory).Include(u => u.Tanks).*/ToListAsync();
        }

        public async Task<Unit> GetId(int id)
        {
            return await db.Unit.Include(u => u.Factory).Include(u => u.Tanks).FirstOrDefaultAsync(u => u.Id == id);
        }

        public void Post(Unit unit)
        {
            db.Add(unit);
            db.SaveChanges();
        }

        public async Task Delete(int id)
        {
            var unit = await db.Unit.FirstOrDefaultAsync(u => u.Id == id);
            db.Remove(unit);
            await db.SaveChangesAsync();
            
        }

        public async Task Update(Unit unit)
        {
            db.Update(unit);
            await db.SaveChangesAsync();
        }

        public async Task<bool> FindId(int id)
        {
            var unit = await db.Unit.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
            return unit != null;
        }

        public async Task<IList<EventOdt>> UnitEvents(int unitId)
        {
            var events = await db.Event.Where(e => e.UnitId == unitId).Take(100).OrderBy(e => e.EventId).ToListAsync();
            return events;
        }
    }
}
