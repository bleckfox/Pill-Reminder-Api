namespace Infrastructure.Entities.Pills;

public class Pill : BaseEntity
{
    /// <summary>
    /// Наименование лекарства
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Ссылка на справочник в интернете
    /// </summary>
    public string Link { get; set; } = string.Empty;
    
    /// <summary>
    /// Описание лекарства (в формате html кода)
    /// </summary>
    public string Description { get; set; } = string.Empty;
}