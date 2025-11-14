namespace Domain;

public class ToDo
{
    public string Description { get; set; }
    public string IsDone { get; set; }
    public string IsHidden { get; set; }

    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }
    
    public Guid PriorityId { get; set; }
    public Priority? Priority { get; set; }
}