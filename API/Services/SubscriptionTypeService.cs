using System.Numerics;
using API.Data;
using API.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class SubscriptionTypeService : ISubscriptionTypeService
    {
        private readonly EasySubContext _context;
        public SubscriptionTypeService(EasySubContext context)
        {
            _context = context;
        }
        public async Task<SubscriptionType> CreateType(SubscriptionType type)
        {
            _context.SubscriptionTypes.Add(type);
            await _context.SaveChangesAsync();
            return type;
        }

        public async Task<bool> DeleteType(int id)
        {
            var type = await _context.SubscriptionTypes.FindAsync(id);
            if (type == null)
                return false;

            _context.SubscriptionTypes.Remove(type);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<SubscriptionType>> GetAllTypes()
        {
            return await _context.SubscriptionTypes.ToListAsync();
        }

        public async Task<SubscriptionType?> GetTypeById(int id)
        {
            return await _context.SubscriptionTypes.FindAsync(id);
        }
    }
}
