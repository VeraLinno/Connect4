namespace ConsoleAppDbTest.Domain;

public class BookAuthor : BaseEntity
{
    // FK is mandatory - so this is mandatory relationship
    public int BookId { get; set; }
    // entity is nullable, because maybe we did not do the sql join (fetching extra data)
    public Book? Book { get; set; }

    public int AuthorId { get; set; }
    public Author? Author { get; set; }
}

// select * from BookAuthors
// id, BookId, AuthorId
// cannot create Book entity, because no data for it. so it should be null.

// sql join
// select * from BookAuthors, Books, Author where BookAuthors.BookId = Books.Id and BookAuthors.AuthorId = Author.Id
// BookAuthors.id, BookAuthors.BookId, BookAuthors.AuthorId, Books.Id, Books.Title, Author.Id, Author.Name

