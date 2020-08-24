using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
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
            return await db.Tank.Include(t => t.Unit).ToListAsync();
        }

        public async Task<Tank> GetId(int id)
        {
            return await db.Tank.Include(t => t.Unit).FirstOrDefaultAsync(t => t.Id == id);
        }

        public void Post(Tank tank)
        {
            db.Add(tank);
            db.SaveChanges();
        }

        public async Task Delete(int id)
        {
            try
            {
                var tank = await db.Tank.FirstOrDefaultAsync(t => t.Id == id);
                db.Remove(tank);
                await db.SaveChangesAsync();
            }
            catch (ArgumentNullException ex)
            {
                throw new Exception("404 Резервуар не найден.", ex);
            }
            catch (Exception e)
            {
                throw new Exception("400 Невозможно удалить резервуар", e);
            }
            
        }

        public async Task Update(Tank tank)
        {
            try
            {
                db.Update(tank);
                await db.SaveChangesAsync();
            }
            catch (ArgumentNullException ex)
            {
                throw new Exception("404 Резервуар не найден.", ex);
            }
        }

    }
}
