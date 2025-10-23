using System.ComponentModel.DataAnnotations;

namespace ConsoleAppDbTest.Domain;

public class Book : BaseEntity
{
    [MaxLength(255)]
    public string Title { get; set; } = default!;

    // nullable FK and entity - this relationship is optional
    public int? PersonId { get; set; }
    public Person? Person { get; set; }


    public ICollection<BookAuthor>? BookAuthors { get; set; }
}
