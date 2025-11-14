using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Priority : BaseEntity
{
    [MaxLength(128)]
    public string PriorityName { get; set; } = default!;
    public string SortValue { get; set; }
}