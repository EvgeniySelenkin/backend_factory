using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi
{
    public class FactoryRepository
    {
        private readonly DBContext db;
        public FactoryRepository(DBContext context)
        {
            db = context;
        }

        public async Task<IEnumerable<Factory>> GetAll()
        {
            return await db.Factory./*Include(f => f.Units).*/ToListAsync();
        }

        public async Task<Factory> GetId(int id)
        {
            return await db.Factory.Include(f => f.Units).FirstOrDefaultAsync(f => f.Id == id);
        }

        public void Post(Factory factory)
        {
            db.Add(factory);
            db.SaveChanges();
        }

        public async Task Delete(int id)
        {
            var factory = await db.Factory./*Include(f => f.Units).*/FirstOrDefaultAsync(f => f.Id == id);
            db.Remove(factory);
            await db.SaveChangesAsync();
            
            
        }

        public async Task Update(Factory factory)
        {
            db.Update(factory);
            await db.SaveChangesAsync();
        }

        public async Task<bool> FindId(int id)
        {
            var factory = await db.Factory.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id);
            return factory != null;
        }
    }
}
