using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Priority : BaseEntity
{
    [MaxLength(128)] 
    public string PriorityName { get; set; } = default!;
    public int SortValue { get; set; }

    public ICollection<ToDo>? Todos { get; set; }
}