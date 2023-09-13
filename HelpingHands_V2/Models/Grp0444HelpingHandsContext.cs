using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace HelpingHands_V2.Models;

public partial class Grp0444HelpingHandsContext : DbContext
{
    public Grp0444HelpingHandsContext()
    {
    }

    public Grp0444HelpingHandsContext(DbContextOptions<Grp0444HelpingHandsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CareContract> CareContracts { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Condition> Conditions { get; set; }

    public virtual DbSet<EndUsers> EndUsers { get; set; }

    public virtual DbSet<Nurse> Nurses { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<PatientCondition> PatientConditions { get; set; }

    public virtual DbSet<PrefferedSuburb> PrefferedSuburbs { get; set; }

    public virtual DbSet<Suburb> Suburbs { get; set; }

    public virtual DbSet<Visit> Visits { get; set; }

    public virtual DbSet<Wound> Wounds { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LITHI_MGWEBI\\SQLEXPRESS;Database=GRP-04-44-HelpingHands;Integrated Security=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CareContract>(entity =>
        {
            entity.HasKey(e => e.ContractId);

            entity.ToTable("CareContract");

            entity.Property(e => e.AddressLineOne).HasMaxLength(50);
            entity.Property(e => e.AddressLineTwo).HasMaxLength(50);
            entity.Property(e => e.ContractDate).HasColumnType("date");
            entity.Property(e => e.ContractStatus).HasMaxLength(5);
            entity.Property(e => e.EndDate).HasColumnType("date");
            entity.Property(e => e.StartDate).HasColumnType("date");

            entity.HasOne(d => d.Nurse).WithMany(p => p.CareContracts)
                .HasForeignKey(d => d.NurseId)
                .HasConstraintName("FK_CareContract_Nurse");

            entity.HasOne(d => d.Patient).WithMany(p => p.CareContracts)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CareContract_Patient");

            entity.HasOne(d => d.Suburb).WithMany(p => p.CareContracts)
                .HasForeignKey(d => d.SuburbId)
                .HasConstraintName("FK_CareContract_Suburb");

            entity.HasOne(d => d.Wound).WithMany(p => p.CareContracts)
                .HasForeignKey(d => d.WoundId)
                .HasConstraintName("FK_CareContract_Wound");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.ToTable("City");

            entity.Property(e => e.CityAbbreviation).HasMaxLength(5);
            entity.Property(e => e.CityName).HasMaxLength(30);
        });

        modelBuilder.Entity<Condition>(entity =>
        {
            entity.ToTable("Condition");

            entity.Property(e => e.ConditionName).HasMaxLength(30);
        });

        modelBuilder.Entity<EndUsers>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("EndUser");

            entity.Property(e => e.ApplicationType).HasMaxLength(5);
            entity.Property(e => e.ContactNumber).HasMaxLength(20);
            entity.Property(e => e.DateOfBirth).HasColumnType("date");
            entity.Property(e => e.Email).HasMaxLength(40);
            entity.Property(e => e.Firstname).HasMaxLength(20);
            entity.Property(e => e.Gender).HasMaxLength(15);
            entity.Property(e => e.Idnumber)
                .HasMaxLength(15)
                .HasColumnName("IDNumber");
            entity.Property(e => e.Lastname).HasMaxLength(20);
            entity.Property(e => e.Password).HasMaxLength(30);
            entity.Property(e => e.ProfilePictureName).HasMaxLength(50);
            entity.Property(e => e.UserType).HasMaxLength(5);
            entity.Property(e => e.Username).HasMaxLength(20);
        });

        modelBuilder.Entity<Nurse>(entity =>
        {
            entity.ToTable("Nurse");

            entity.Property(e => e.NurseId).ValueGeneratedNever();
            entity.Property(e => e.Grade).HasMaxLength(5);

            entity.HasOne(d => d.NurseNavigation).WithOne(p => p.Nurse)
                .HasForeignKey<Nurse>(d => d.NurseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Nurse_EndUser");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.ToTable("Patient");

            entity.Property(e => e.PatientId).ValueGeneratedNever();
            entity.Property(e => e.AddressLineOne).HasMaxLength(50);
            entity.Property(e => e.AddressLineTwo).HasMaxLength(50);
            entity.Property(e => e.Icename)
                .HasMaxLength(20)
                .HasColumnName("ICEName");
            entity.Property(e => e.Icenumber)
                .HasMaxLength(20)
                .HasColumnName("ICENumber");

            entity.HasOne(d => d.PatientNavigation).WithOne(p => p.Patient)
                .HasForeignKey<Patient>(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Patient_EndUser");

            entity.HasOne(d => d.Suburb).WithMany(p => p.Patients)
                .HasForeignKey(d => d.SuburbId)
                .HasConstraintName("FK_Patient_Suburb");
        });

        modelBuilder.Entity<PatientCondition>(entity =>
        {
            entity.HasKey(e => new { e.PatientId, e.ConditionId });

            entity.ToTable("PatientCondition");

            entity.HasOne(d => d.Condition).WithMany(p => p.PatientConditions)
                .HasForeignKey(d => d.ConditionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PatientCondition_Condition");

            entity.HasOne(d => d.Patient).WithMany(p => p.PatientConditions)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PatientCondition_Patient");
        });

        modelBuilder.Entity<PrefferedSuburb>(entity =>
        {
            entity.HasKey(e => new { e.NurseId, e.SuburbId });

            entity.ToTable("PrefferedSuburb");

            entity.HasOne(d => d.Nurse).WithMany(p => p.PrefferedSuburbs)
                .HasForeignKey(d => d.NurseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PrefferedSuburb_Nurse");

            entity.HasOne(d => d.Suburb).WithMany(p => p.PrefferedSuburbs)
                .HasForeignKey(d => d.SuburbId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PrefferedSuburb_Suburb");
        });

        modelBuilder.Entity<Suburb>(entity =>
        {
            entity.ToTable("Suburb");

            entity.Property(e => e.SuburbName).HasMaxLength(30);

            entity.HasOne(d => d.City).WithMany(p => p.Suburbs)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Suburb_City");
        });

        modelBuilder.Entity<Visit>(entity =>
        {
            entity.ToTable("Visit");

            entity.Property(e => e.ApproxTime).HasPrecision(0);
            entity.Property(e => e.Arrival).HasPrecision(0);
            entity.Property(e => e.Departure).HasPrecision(0);
            entity.Property(e => e.VisitDate).HasColumnType("date");
            entity.Property(e => e.WoundCondition).HasMaxLength(150);

            entity.HasOne(d => d.Contract).WithMany(p => p.Visits)
                .HasForeignKey(d => d.ContractId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Visit_CareContract");
        });

        modelBuilder.Entity<Wound>(entity =>
        {
            entity.HasKey(e => e.WoundId).HasName("PK_Wounf");

            entity.ToTable("Wound");

            entity.Property(e => e.Grade).HasMaxLength(5);
            entity.Property(e => e.WoundName).HasMaxLength(20);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
