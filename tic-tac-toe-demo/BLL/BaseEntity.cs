namespace BLL;

public class BaseEntity
{
    // Let's create the id's on the code side, not in db
    public Guid Id { get; set; } = Guid.NewGuid();

}