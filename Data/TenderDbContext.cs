using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TenderAPI.Models;

namespace TenderAPI.Data;

public partial class TenderDbContext : DbContext
{
    public TenderDbContext()
    {
    }

    public TenderDbContext(DbContextOptions<TenderDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Attachment> Attachments { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<ExpiringPractice> ExpiringPractices { get; set; }

    public virtual DbSet<Practice> Practices { get; set; }

    public virtual DbSet<ProceduresType> ProceduresTypes { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-I3JGH2T\\SQLEXPRESS;Initial Catalog=TenderDB;Integrated Security=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attachment>(entity =>
        {
            entity.Property(e => e.AttachmentId).HasColumnName("attachment_id");
            entity.Property(e => e.Path)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("path");
            entity.Property(e => e.PracticeId).HasColumnName("practice_id");

            entity.HasOne(d => d.Practice).WithMany(p => p.Attachments)
                .HasForeignKey(d => d.PracticeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Attachments_Practices");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Description)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("description");
        });

        modelBuilder.Entity<ExpiringPractice>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("ExpiringPractices");

            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.Authority)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("authority");
            entity.Property(e => e.Criteria)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("criteria");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("customer_name");
            entity.Property(e => e.DateExpire)
                .HasColumnType("date")
                .HasColumnName("date_expire");
            entity.Property(e => e.DateStart)
                .HasColumnType("date")
                .HasColumnName("date_start");
            entity.Property(e => e.Note)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("note");
            entity.Property(e => e.Object)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("object");
            entity.Property(e => e.Platform)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("platform");
            entity.Property(e => e.PracticeId).HasColumnName("practice_id");
            entity.Property(e => e.PrevalentCategory)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("prevalent_category");
            entity.Property(e => e.ProcedureTypeId).HasColumnName("procedure_type_id");
            entity.Property(e => e.ProcurementCode)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("procurement_code");
            entity.Property(e => e.State)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("state");
            entity.Property(e => e.StateId).HasColumnName("state_id");
        });

        modelBuilder.Entity<Practice>(entity =>
        {
            entity.Property(e => e.PracticeId).HasColumnName("practice_id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.Authority)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("authority");
            entity.Property(e => e.Criteria)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("criteria");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.DateExpire)
                .HasColumnType("date")
                .HasColumnName("date_expire");
            entity.Property(e => e.DateStart)
                .HasColumnType("date")
                .HasColumnName("date_start");
            entity.Property(e => e.Note)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("note");
            entity.Property(e => e.Object)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("object");
            entity.Property(e => e.Platform)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("platform");
            entity.Property(e => e.PrevalentCategory)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("prevalent_category");
            entity.Property(e => e.ProcedureTypeId).HasColumnName("procedure_type_id");
            entity.Property(e => e.ProcurementCode)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("procurement_code");
            entity.Property(e => e.StateId).HasColumnName("state_id");

            entity.HasOne(d => d.Customer).WithMany(p => p.Practices)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Practices_Customers");

            entity.HasOne(d => d.ProcedureType).WithMany(p => p.Practices)
                .HasForeignKey(d => d.ProcedureTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Practices_Procedures_types");

            entity.HasOne(d => d.State).WithMany(p => p.Practices)
                .HasForeignKey(d => d.StateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Practices_States");
        });

        modelBuilder.Entity<ProceduresType>(entity =>
        {
            entity.HasKey(e => e.ProcedureTypeId);

            entity.ToTable("Procedures_types");

            entity.Property(e => e.ProcedureTypeId).HasColumnName("procedure_type_id");
            entity.Property(e => e.Description)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("description");
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.Property(e => e.StateId).HasColumnName("state_id");
            entity.Property(e => e.Description)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("description");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Salt)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.Surname)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(25)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
