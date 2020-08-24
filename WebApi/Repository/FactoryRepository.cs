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
            return await db.Factory.Include(f => f.Units).ToListAsync();
        }

        public async Task<Factory> GetId(int id)
        {
            return await db.Factory.Include(f => f.Units).FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task Post(Factory factory)
        {
            await db.AddAsync(factory);
            await db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            try
            {
                var factory = db.Factory./*Include(f => f.Units).*/FirstOrDefaultAsync(f => f.Id == id);
                db.Remove(factory.Result);
                await db.SaveChangesAsync();
            }
            catch(Exception e)
            {
                throw new Exception("400 Невозможно удалить завод", e);
            }
            
            
        }

        public async Task Update(Factory factory)
        {
            db.Update(factory);
            await db.SaveChangesAsync();
        }
    }
}
