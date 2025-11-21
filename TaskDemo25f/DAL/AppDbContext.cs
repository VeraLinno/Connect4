using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public required DbSet<Category> Categories { get; set; }
    public required DbSet<Priority> Priorities { get; set; }
    public required DbSet<ToDo> Todos { get; set; }
}