using ApplicationDatabase.Context;
using Infrastructure.Entities.Pills;
using Microsoft.EntityFrameworkCore;
using ReminderApi.Interfaces;

namespace ReminderApi.Repositories.Pills;

public class PillRepository : IPillRepository
{
    private readonly ApplicationContext _context;

    public PillRepository(ApplicationContext context)
    {
        _context = context;
    }
    
    public async Task<ICollection<Pill>> GetAllPill()
    {
        return await _context.Pills.ToListAsync();
    }
    
    public async Task<ICollection<Pill>> GetAllPillNames()
    {
        return await _context.Pills
            .Select(p => new Pill
            {
                Id = p.Id,
                Name = p.Name
            })
            .ToListAsync();
    }

    public async Task<Pill> GetPill(Guid Id)
    {
        var targetPill = await _context.Pills.FirstOrDefaultAsync(p => p.Id == Id);
        
        // if (targetPill != null)
        // {
        //     return targetPill;
        // }
        // return await _context.Pills.FirstAsync();
        
        //return targetPill != null ? targetPill : await _context.Pills.FirstAsync();
        
        return targetPill ?? await _context.Pills.FirstAsync();
    }

    public async Task<bool> CreatePill(Pill pill)
    {
        try
        {
            await _context.Pills.AddAsync(pill);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("При попытке создать запись о лекарстве произошла ошибка!");
            Console.WriteLine(e);
            
        }

        return false;
    }

    public async Task<bool> DeletePill(Guid Id)
    {
        try
        {
            Pill pill = await GetPill(Id);
            _context.Pills.Remove(pill);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("При попытке удалить запись о лекарстве произошла ошибка!");
            Console.WriteLine(e);
        }

        return false;
    }

    public async Task<bool> UpdatePill(Guid Id, Pill pill)
    {
        try
        {
            Pill currentPill = await GetPill(Id);

            currentPill.Name = pill.Name;
            currentPill.Description = pill.Description;
            currentPill.Link = pill.Link;
            
            _context.Pills.Update(currentPill);
            await _context.SaveChangesAsync();
            
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("При попытке обновить запись о лекарстве произошла ошибка!");
            Console.WriteLine(e);
        }

        return false;
    }
}