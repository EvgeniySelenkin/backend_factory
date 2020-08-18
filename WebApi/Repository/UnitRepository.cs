using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            return await db.Unit.ToListAsync();
        }

        public async Task<Unit> GetId(int id)
        {
            return await db.Unit.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task Post(Unit unit)
        {
            await db.AddAsync(unit);
            await db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var unit = db.Unit.FirstOrDefaultAsync(u => u.Id == id);
            db.Remove(unit.Result);
            await db.SaveChangesAsync();
        }

        public async Task Update(Unit unit)
        {
            db.Update(unit);
            await db.SaveChangesAsync();
        }
    }
}
