using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

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
            return await db.Unit.Include(u => u.Factory).Include(u => u.Tanks).ToListAsync();
        }

        public async Task<Unit> GetId(int id)
        {
            return await db.Unit.Include(u => u.Factory).Include(u => u.Tanks).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task Post(Unit unit)
        {
            await db.AddAsync(unit);
            await db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            try
            {
                var unit = db.Unit.FirstOrDefaultAsync(u => u.Id == id);
                db.Remove(unit.Result);
                await db.SaveChangesAsync();
            }
            catch (ArgumentNullException ex)
            {
                throw new Exception("404 Установка не найден.", ex);
            }
            catch (Exception e)
            {
                throw new Exception("400 Невозможно удалить установку", e);
            }
            
        }

        public async Task Update(Unit unit)
        {
            try
            {
                db.Update(unit);
                await db.SaveChangesAsync();
            }
            catch (ArgumentNullException ex)
            {
                throw new Exception("404 Установка не найден.", ex);
            }
        }
    }
}
