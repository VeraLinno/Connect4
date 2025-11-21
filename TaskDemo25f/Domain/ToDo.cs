using System.ComponentModel.DataAnnotations;

namespace Domain;

public class ToDo : BaseEntity
{
    [MaxLength(128)]
    public string Description { get; set; } = default!;
    public bool IsDone { get; set; }
    public bool IsHidden { get; set; }

    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }
    
    public Guid PriorityId { get; set; }
    public Priority? Priority { get; set; }
}