using System;
using System.Collections.Generic;
using Api.Grpc.IssueBoard.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Grpc.IssueBoard.Data;

public partial class IssuesDbContext : DbContext
{
    public IssuesDbContext()
    {
    }

    public IssuesDbContext(DbContextOptions<IssuesDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Issue> Issues { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost;Database=Samples;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Command Timeout=0");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Issue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Issues__3214EC07FBC9A89A");

            entity.Property(e => e.AuthorName).HasMaxLength(50);
            entity.Property(e => e.Category).HasMaxLength(30);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.Resolution).HasMaxLength(2000);
            entity.Property(e => e.ResolverName).HasMaxLength(50);
            entity.Property(e => e.Status).HasComment("0:未着手, 1:着手中, 2:解決失敗, 3:課題確認不能, 4:解決済み");
            entity.Property(e => e.Title).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
