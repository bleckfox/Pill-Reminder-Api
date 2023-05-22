using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities;

/// <summary>
/// Базовый класс сущности с Id - первичным ключом
/// </summary>
public class BaseEntity
{
    /// <summary>
    /// Идентификатор (Guid)
    /// </summary>
    [Key]
    public Guid Id { get; set; }
}