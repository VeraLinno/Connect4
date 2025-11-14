using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Category : BaseEntity
{
    [MaxLength(128)]
    public string CategoryName { get; set; } = default!;
    public string SortValue { get; set; }
    
    public ICollection<ToDo>? ToDo { get; set; }
}