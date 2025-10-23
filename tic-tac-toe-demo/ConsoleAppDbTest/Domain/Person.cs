using System.ComponentModel.DataAnnotations;

namespace ConsoleAppDbTest.Domain;

public class Person : BaseEntity
{
    [MaxLength(128)]
    public string FirstName { get; set; } = default!;
    
    [MaxLength(128)]
    public string LastName { get; set; } = default!;


    // it's null when there was no query done to fetch data
    // its empty, when query was done, but no data in db
    public ICollection<Book>? Books { get; set; }

    public override string ToString()
    {
        return $"{Id} {FirstName} {LastName} Books count: {Books?.Count.ToString() ?? "null"}";
    }
}