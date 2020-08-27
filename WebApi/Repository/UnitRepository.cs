﻿using Microsoft.EntityFrameworkCore;
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
