namespace Infrastructure.AppEnvironment;

/// <summary>
/// Модель настроек приложения
/// </summary>
public class AppSettings
{
    /// <summary>
    /// Строка подключения к базе данных
    /// </summary>
    public string ConnectionString { get; set; } = null!;
}