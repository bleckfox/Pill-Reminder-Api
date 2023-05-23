using Infrastructure.Entities.Pills;

namespace ReminderApi.Interfaces;

public interface IPillRepository
{
    /// <summary>
    /// Получить все данные о лекарствах
    /// </summary>
    /// <returns>Полный список лекарств</returns>
    Task<ICollection<Pill>> GetAllPill();
    
    /// <summary>
    /// Получить все названия лекарств
    /// </summary>
    /// <returns>Полный список лекарств</returns>
    Task<ICollection<Pill>> GetAllPillNames();

    /// <summary>
    /// Получить информацию о конкретном лекарстве
    /// </summary>
    /// <param name="Id">Идентификатор лекарства</param>
    /// <returns>Информаццию о конкретном лекарстве</returns>
    Task<Pill> GetPill(Guid Id);

    /// <summary>
    /// Создание нового лекарства
    /// </summary>
    /// <param name="pill">Информация о лекарстве</param>
    /// <returns>Признак создалось ли</returns>
    Task<bool> CreatePill(Pill pill);
    
    /// <summary>
    /// Удаление лекарства
    /// </summary>
    /// <param name="Id">Идентификатор лекарства</param>
    /// <returns>Признак удалилось ли</returns>
    Task<bool> DeletePill(Guid Id);
    
    /// <summary>
    /// Обновление информации о лекарстве
    /// </summary>
    /// <param name="Id">Идентификатор лекарства</param>
    /// <param name="pill">Информация о лекарстве</param>
    /// <returns>Признак обновилось ли</returns>
    Task<bool> UpdatePill(Guid Id, Pill pill);
}