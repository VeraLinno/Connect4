// See https://aka.ms/new-console-template for more information

using ConsoleAppDbTest.Dal;
using ConsoleAppDbTest.Domain;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, DB demo!");

var homeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
homeDirectory = homeDirectory + Path.DirectorySeparatorChar;

// We are using SQLite
var connectionString = $"Data Source={homeDirectory}app.db";

var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlite(connectionString)
    .EnableDetailedErrors()
    .EnableSensitiveDataLogging()
    //.LogTo(Console.WriteLine)
    .Options;

// gets disposed correctly, when variable goes out of scope
using var context = new AppDbContext(contextOptions);

var person = new Person() { FirstName = "Andres", LastName = "Käver" };

Console.WriteLine(person);
context.Persons.Add(person);

Console.WriteLine(person);

context.SaveChanges();

Console.WriteLine(person);

Console.WriteLine("-------------");

foreach (var dbPerson in context.Persons.Include(p => p.Books))
{
    Console.WriteLine(dbPerson);
}