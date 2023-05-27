using Infrastructure.Entities.Pills;

namespace ReminderApi.Interfaces.Pills;

public interface IPillRepository
{
    /// <summary>
    /// Получить все данные о лекарствах
    /// </summary>
    /// <returns>Полный список лекарств</returns>
    Task<ICollection<Pill>> GetAllPill();

    /// <summary>
    /// Получить информацию о лекарствах по имени
    /// </summary>
    /// <param name="name">Наименование лекарства</param>
    /// <returns>Массив лекарств</returns>
    Task<ICollection<Pill>> GetPillByName(string name);

    /// <summary>
    /// Получить информацию о конкретном лекарстве
    /// </summary>
    /// <param name="id">Идентификатор лекарства</param>
    /// <returns>Информаццию о конкретном лекарстве</returns>
    Task<Pill> GetPill(Guid id);

    /// <summary>
    /// Создание нового лекарства
    /// </summary>
    /// <param name="pill">Информация о лекарстве</param>
    /// <returns>Признак создалось ли</returns>
    Task<bool> CreatePill(Pill pill);
    
    /// <summary>
    /// Удаление лекарства
    /// </summary>
    /// <param name="id">Идентификатор лекарства</param>
    /// <returns>Признак удалилось ли</returns>
    Task<bool> DeletePill(Guid id);
    
    /// <summary>
    /// Обновление информации о лекарстве
    /// </summary>
    /// <param name="id">Идентификатор лекарства</param>
    /// <param name="pill">Информация о лекарстве</param>
    /// <returns>Признак обновилось ли</returns>
    Task<bool> UpdatePill(Guid id, Pill pill);

    /// <summary>
    /// Метод загрузки данных о лекарстве
    /// </summary>
    /// <returns>Признак удалось ли получить данные</returns>
    Task<bool> FetchPill();
}