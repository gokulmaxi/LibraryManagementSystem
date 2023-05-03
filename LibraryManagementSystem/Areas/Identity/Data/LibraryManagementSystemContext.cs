using LibraryManagementSystem.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Areas.Identity.Data;

public class LibraryManagementSystemContext : IdentityDbContext<LMSUser>
{
    public LibraryManagementSystemContext(DbContextOptions<LibraryManagementSystemContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

    public DbSet<LibraryManagementSystem.Models.BookDetails>? BookDetails { get; set; }

    public DbSet<LibraryManagementSystem.Models.ReservationDetails>? ReservationDetails { get; set; }

    public DbSet<LibraryManagementSystem.Models.FineDetails>? FineDetails { get; set; }
}
