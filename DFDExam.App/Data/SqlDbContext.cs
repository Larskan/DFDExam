using Microsoft.EntityFrameworkCore;
using DFDExam.App.Models;

namespace DFDExam.App.Data;

public class SqlDbContext : DbContext
{
    public SqlDbContext(DbContextOptions<SqlDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SqlUser>().HasIndex(u => u.Email); //Index for fair lookup comparison
        // MongoDB auto indexes the Id field, SQL needs an explicit index for fair comparison
    }
}