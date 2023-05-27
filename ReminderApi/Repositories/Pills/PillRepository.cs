using ApplicationDatabase.Context;
using Infrastructure.Entities.Pills;
using Microsoft.EntityFrameworkCore;
using ReminderApi.Interfaces.Pills;

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

    public async Task<ICollection<Pill>> GetPillByName(string name)
    {
        return await _context.Pills.Where(p => p.Name.Contains(name)).ToListAsync();
    }

    public async Task<Pill> GetPill(Guid id)
    {
        var targetPill = await _context.Pills.FirstOrDefaultAsync(p => p.Id == id);
        
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

    public async Task<bool> DeletePill(Guid id)
    {
        try
        {
            Pill pill = await GetPill(id);
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

    public async Task<bool> UpdatePill(Guid id, Pill pill)
    {
        try
        {
            Pill currentPill = await GetPill(id);

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

    public async Task<bool> FetchPill()
    {
        try
        {

            Parser parser = new Parser(new HttpClient(), this);

            List<string> links = parser.GetLinksToPill().GetAwaiter().GetResult();

            foreach (string link in links)
            {
                await parser.FetchPillInformationAsync(link);
            }

        }
        catch (Exception e)
        {
            Console.WriteLine("При попытке получить данные о лекарстве произошла ошибка!");
            Console.WriteLine(e);
        }
        
        return false;
    }
}