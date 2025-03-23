using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class SubscriptionPlanService : ISubscriptionPlanService
    {
        private readonly EasySubContext _context;

        public SubscriptionPlanService(EasySubContext context)
        {
            _context = context;
        }

        public async Task<List<SubscriptionPlan>> GetAllPlans()
        {
            return await _context.SubscriptionPlans.AsNoTracking().Include(a => a.Brand).Include(a => a.SubscriptionType).ToListAsync();
        }

        public async Task<SubscriptionPlan?> GetPlanById(int id)
        {
            return await _context.SubscriptionPlans.AsNoTracking().Include(a => a.Brand).Include(a => a.SubscriptionType).FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<SubscriptionPlan> CreatePlan(SubscriptionPlan plan)
        {
            _context.SubscriptionPlans.Add(plan);
            await _context.SaveChangesAsync();
            return plan;
        }

        public async Task<bool> DeletePlan(int id)
        {
            var plan = await _context.SubscriptionPlans.AsNoTracking().Include(a => a.Brand).Include(a => a.SubscriptionType).FirstOrDefaultAsync(a => a.Id == id);
            if (plan == null)
                return false;

            _context.SubscriptionPlans.Remove(plan);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<SubscriptionPlan>> GetPlansByBrandId(int brandId)
        {
            return await _context.SubscriptionPlans.AsNoTracking()
                .Where(sp => sp.BrandId == brandId)
                .Include(sp => sp.SubscriptionType).Include(a => a.Brand)
                .ToListAsync();
        }

    }
}
