using ElevenNote.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ElevenNote.Data;

public class ApplicationDbContext : IdentityDbContext<UserEntity, RoleEntity, int, UserClaimEntity,
    UserRoleEntity, UserLoginEntity, RoleClaimEntity, UserTokenEntity>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public override DbSet<UserEntity> Users { get; set; } = null!;
    public DbSet<NoteEntity> Notes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); //anything that base does behind the scenes will continue

        modelBuilder.Entity<UserEntity>()
        .ToTable("Users")
        .Ignore(u => u.UserName); //Ignore duplicate column

        modelBuilder.Entity<RoleEntity>().ToTable("Roles");
        modelBuilder.Entity<UserRoleEntity>().ToTable("UserClaims");
        modelBuilder.Entity<UserClaimEntity>().ToTable("UserLogins");
        modelBuilder.Entity<UserTokenEntity>().ToTable("UserTokens");
        modelBuilder.Entity<RoleClaimEntity>().ToTable("RoleClaims");
        
        modelBuilder.Entity<NoteEntity>()
            .HasOne(n => n.Owner)
            .WithMany(p => p.Notes)
            .HasForeignKey(n => n.OwnerId);
    }
}