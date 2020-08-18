using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi
{
    public class TankRepository
    {
        private readonly DBContext db;
        public TankRepository(DBContext context)
        {
            db = context;
        }

        public async Task<IEnumerable<Tank>> GetAll()
        {
            return await db.Tank.ToListAsync();
        }

        public async Task<Tank> GetId(int id)
        {
            return await db.Tank.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task Post(Tank tank)
        {
            await db.AddAsync(tank);
            await db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var tank = db.Tank.FirstOrDefaultAsync(t => t.Id == id);
            db.Remove(tank.Result);
            await db.SaveChangesAsync();
        }

        public async Task Update(Tank tank)
        {
            db.Update(tank);
            await db.SaveChangesAsync();
        }
    }
}
