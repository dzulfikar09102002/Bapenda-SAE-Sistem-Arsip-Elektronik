using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Sistem_Pemberkasan.Models.EF;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Berka> Berkas { get; set; }

    public virtual DbSet<DetailDokuman> DetailDokumen { get; set; }

    public virtual DbSet<MBidang> MBidangs { get; set; }

    public virtual DbSet<MDokuman> MDokumen { get; set; }

    public virtual DbSet<MFormDokuman> MFormDokumen { get; set; }

    public virtual DbSet<MKategoriBerka> MKategoriBerkas { get; set; }

    public virtual DbSet<MKategoriPendukungBerka> MKategoriPendukungBerkas { get; set; }

    public virtual DbSet<MPegawai> MPegawais { get; set; }

    public virtual DbSet<MRole> MRoles { get; set; }

    public virtual DbSet<MUser> MUsers { get; set; }

    public virtual DbSet<PendukungBerka> PendukungBerkas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=10.21.39.88;port=3306;database=sistem_pemberkasan;user=spb_mgg;password=Bapenda.2024", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.30-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Berka>(entity =>
        {
            entity.HasKey(e => e.IdBerkas).HasName("PRIMARY");

            entity.ToTable("berkas");

            entity.HasIndex(e => e.IdKategoriBerkas, "FK_BERKAS_RELATIONS_M_KATEGO");

            entity.Property(e => e.IdBerkas)
                .ValueGeneratedNever()
                .HasColumnName("ID_BERKAS");
            entity.Property(e => e.AlamatPemohon)
                .HasMaxLength(255)
                .HasColumnName("ALAMAT_PEMOHON");
            entity.Property(e => e.BeInsertBy).HasColumnName("BE_INSERT_BY");
            entity.Property(e => e.BeInsertDate)
                .HasColumnType("timestamp")
                .HasColumnName("BE_INSERT_DATE");
            entity.Property(e => e.IdKategoriBerkas).HasColumnName("ID_KATEGORI_BERKAS");
            entity.Property(e => e.KeteranganPst).HasColumnName("KETERANGAN_PST");
            entity.Property(e => e.NamaPemohon)
                .HasMaxLength(255)
                .HasColumnName("NAMA_PEMOHON");
            entity.Property(e => e.NoReferensi)
                .HasMaxLength(50)
                .HasColumnName("NO_REFERENSI");
            entity.Property(e => e.NoSk)
                .HasMaxLength(50)
                .HasColumnName("NO_SK");
            entity.Property(e => e.NoTelpPemohon)
                .HasMaxLength(50)
                .HasColumnName("NO_TELP_PEMOHON");
            entity.Property(e => e.Nop)
                .HasMaxLength(50)
                .HasColumnName("NOP");
            entity.Property(e => e.StatusBerkas).HasColumnName("STATUS_BERKAS");
            entity.Property(e => e.TanggalUnggah)
                .HasColumnType("timestamp")
                .HasColumnName("TANGGAL_UNGGAH");

            entity.HasOne(d => d.IdKategoriBerkasNavigation).WithMany(p => p.Berkas)
                .HasForeignKey(d => d.IdKategoriBerkas)
                .HasConstraintName("FK_BERKAS_RELATIONS_M_KATEGO");
        });

        modelBuilder.Entity<DetailDokuman>(entity =>
        {
            entity.HasKey(e => e.IdDetailDokumen).HasName("PRIMARY");

            entity.ToTable("detail_dokumen");

            entity.HasIndex(e => e.IdBerkas, "FK_DETAIL_D_RELATIONS_BERKAS");

            entity.HasIndex(e => e.IdDokumen, "FK_DETAIL_D_RELATIONS_M_DOKUME");

            entity.Property(e => e.IdDetailDokumen)
                .ValueGeneratedNever()
                .HasColumnName("ID_DETAIL_DOKUMEN");
            entity.Property(e => e.FileBloob).HasColumnName("fileBloob");
            entity.Property(e => e.IdBerkas).HasColumnName("ID_BERKAS");
            entity.Property(e => e.IdDokumen).HasColumnName("ID_DOKUMEN");
            entity.Property(e => e.NamaDetailDokumen)
                .HasMaxLength(255)
                .HasColumnName("NAMA_DETAIL_DOKUMEN");

            entity.HasOne(d => d.IdBerkasNavigation).WithMany(p => p.DetailDokumen)
                .HasForeignKey(d => d.IdBerkas)
                .HasConstraintName("FK_DETAIL_D_RELATIONS_BERKAS");

            entity.HasOne(d => d.IdDokumenNavigation).WithMany(p => p.DetailDokumen)
                .HasForeignKey(d => d.IdDokumen)
                .HasConstraintName("FK_DETAIL_D_RELATIONS_M_DOKUME");
        });

        modelBuilder.Entity<MBidang>(entity =>
        {
            entity.HasKey(e => e.IdBidang).HasName("PRIMARY");

            entity.ToTable("m_bidang");

            entity.Property(e => e.IdBidang)
                .ValueGeneratedNever()
                .HasColumnName("ID_BIDANG");
            entity.Property(e => e.BInsertBy).HasColumnName("B_INSERT_BY");
            entity.Property(e => e.BInsertDate)
                .HasColumnType("timestamp")
                .HasColumnName("B_INSERT_DATE");
            entity.Property(e => e.NamaBidang)
                .HasMaxLength(255)
                .HasColumnName("NAMA_BIDANG");
            entity.Property(e => e.Singkatan)
                .HasMaxLength(20)
                .HasColumnName("SINGKATAN");
            entity.Property(e => e.StatusBidang).HasColumnName("STATUS_BIDANG");
        });

        modelBuilder.Entity<MDokuman>(entity =>
        {
            entity.HasKey(e => e.IdDokumen).HasName("PRIMARY");

            entity.ToTable("m_dokumen");

            entity.Property(e => e.IdDokumen)
                .ValueGeneratedNever()
                .HasColumnName("ID_DOKUMEN");
            entity.Property(e => e.DInsertBy).HasColumnName("D_INSERT_BY");
            entity.Property(e => e.DInsertDate)
                .HasColumnType("timestamp")
                .HasColumnName("D_INSERT_DATE");
            entity.Property(e => e.NamaDokumen)
                .HasMaxLength(255)
                .HasColumnName("NAMA_DOKUMEN");
            entity.Property(e => e.StatusDokumen).HasColumnName("STATUS_DOKUMEN");
        });

        modelBuilder.Entity<MFormDokuman>(entity =>
        {
            entity.HasKey(e => e.IdFormDokumen).HasName("PRIMARY");

            entity.ToTable("m_form_dokumen");

            entity.HasIndex(e => e.IdDokumen, "FK_M_FORM_D_RELATIONS_M_DOKUME");

            entity.HasIndex(e => e.IdKategoriBerkas, "FK_M_FORM_D_RELATIONS_M_KATEGO");

            entity.Property(e => e.IdFormDokumen)
                .ValueGeneratedNever()
                .HasColumnName("ID_FORM_DOKUMEN");
            entity.Property(e => e.FdInsertBy).HasColumnName("FD_INSERT_BY");
            entity.Property(e => e.FdInsertDate)
                .HasColumnType("timestamp")
                .HasColumnName("FD_INSERT_DATE");
            entity.Property(e => e.IdDokumen).HasColumnName("ID_DOKUMEN");
            entity.Property(e => e.IdKategoriBerkas).HasColumnName("ID_KATEGORI_BERKAS");
            entity.Property(e => e.StatusFormDokumen).HasColumnName("STATUS_FORM_DOKUMEN");

            entity.HasOne(d => d.IdDokumenNavigation).WithMany(p => p.MFormDokumen)
                .HasForeignKey(d => d.IdDokumen)
                .HasConstraintName("FK_M_FORM_D_RELATIONS_M_DOKUME");

            entity.HasOne(d => d.IdKategoriBerkasNavigation).WithMany(p => p.MFormDokumen)
                .HasForeignKey(d => d.IdKategoriBerkas)
                .HasConstraintName("FK_M_FORM_D_RELATIONS_M_KATEGO");
        });

        modelBuilder.Entity<MKategoriBerka>(entity =>
        {
            entity.HasKey(e => e.IdKategoriBerkas).HasName("PRIMARY");

            entity.ToTable("m_kategori_berkas");

            entity.HasIndex(e => e.IdBidang, "FK_M_KATEGO_RELATIONS_M_BIDANG");

            entity.Property(e => e.IdKategoriBerkas)
                .ValueGeneratedNever()
                .HasColumnName("ID_KATEGORI_BERKAS");
            entity.Property(e => e.IdBidang).HasColumnName("ID_BIDANG");
            entity.Property(e => e.JenisKategoriBerkas)
                .HasMaxLength(255)
                .HasColumnName("JENIS_KATEGORI_BERKAS");
            entity.Property(e => e.KbInsertBy).HasColumnName("KB_INSERT_BY");
            entity.Property(e => e.KbInsertDate)
                .HasColumnType("timestamp")
                .HasColumnName("KB_INSERT_DATE");
            entity.Property(e => e.StatusKategoriBerkas).HasColumnName("STATUS_KATEGORI_BERKAS");

            entity.HasOne(d => d.IdBidangNavigation).WithMany(p => p.MKategoriBerkas)
                .HasForeignKey(d => d.IdBidang)
                .HasConstraintName("FK_M_KATEGO_RELATIONS_M_BIDANG");
        });

        modelBuilder.Entity<MKategoriPendukungBerka>(entity =>
        {
            entity.HasKey(e => e.IdKategoriPendukungBerkas).HasName("PRIMARY");

            entity.ToTable("m_kategori_pendukung_berkas");

            entity.Property(e => e.IdKategoriPendukungBerkas)
                .ValueGeneratedNever()
                .HasColumnName("ID_KATEGORI_PENDUKUNG_BERKAS");
            entity.Property(e => e.KpInsertBy).HasColumnName("KP_INSERT_BY");
            entity.Property(e => e.KpInsertDate)
                .HasColumnType("timestamp")
                .HasColumnName("KP_INSERT_DATE");
            entity.Property(e => e.NamaKategoriPendukungBerkas)
                .HasMaxLength(255)
                .HasColumnName("NAMA_KATEGORI_PENDUKUNG_BERKAS");
            entity.Property(e => e.StatusKategoriPendukung).HasColumnName("STATUS_KATEGORI_PENDUKUNG");
        });

        modelBuilder.Entity<MPegawai>(entity =>
        {
            entity.HasKey(e => e.Nik).HasName("PRIMARY");

            entity.ToTable("m_pegawai");

            entity.HasIndex(e => e.IdBidang, "FK_M_PEGAWA_RELATIONS_M_BIDANG");

            entity.Property(e => e.Nik)
                .HasMaxLength(16)
                .HasColumnName("NIK");
            entity.Property(e => e.IdBidang).HasColumnName("ID_BIDANG");
            entity.Property(e => e.JenisKelamin).HasColumnName("JENIS_KELAMIN");
            entity.Property(e => e.NamaPegawai)
                .HasMaxLength(255)
                .HasColumnName("NAMA_PEGAWAI");
            entity.Property(e => e.NoTelepon)
                .HasMaxLength(50)
                .HasColumnName("NO_TELEPON");
            entity.Property(e => e.PInsertBy).HasColumnName("P_INSERT_BY");
            entity.Property(e => e.PInsertDate)
                .HasColumnType("timestamp")
                .HasColumnName("P_INSERT_DATE");
            entity.Property(e => e.StatusPegawai).HasColumnName("STATUS_PEGAWAI");

            entity.HasOne(d => d.IdBidangNavigation).WithMany(p => p.MPegawais)
                .HasForeignKey(d => d.IdBidang)
                .HasConstraintName("FK_M_PEGAWA_RELATIONS_M_BIDANG");
        });

        modelBuilder.Entity<MRole>(entity =>
        {
            entity.HasKey(e => e.IdRole).HasName("PRIMARY");

            entity.ToTable("m_role");

            entity.Property(e => e.IdRole)
                .ValueGeneratedNever()
                .HasColumnName("ID_ROLE");
            entity.Property(e => e.JenisRole)
                .HasMaxLength(255)
                .HasColumnName("JENIS_ROLE");
            entity.Property(e => e.RInsertBy).HasColumnName("R_INSERT_BY");
            entity.Property(e => e.RInsertDate)
                .HasColumnType("timestamp")
                .HasColumnName("R_INSERT_DATE");
            entity.Property(e => e.StatusRole).HasColumnName("STATUS_ROLE");
        });

        modelBuilder.Entity<MUser>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PRIMARY");

            entity.ToTable("m_user");

            entity.HasIndex(e => e.Nik, "FK_M_USER_RELATIONS_M_PEGAWA");

            entity.HasIndex(e => e.IdRole, "FK_M_USER_RELATIONS_M_ROLE");

            entity.Property(e => e.IdUser)
                .ValueGeneratedNever()
                .HasColumnName("ID_USER");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("EMAIL");
            entity.Property(e => e.IdRole).HasColumnName("ID_ROLE");
            entity.Property(e => e.Nik)
                .HasMaxLength(16)
                .HasColumnName("NIK");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.StatusUser).HasColumnName("STATUS_USER");
            entity.Property(e => e.UInsertBy).HasColumnName("U_INSERT_BY");
            entity.Property(e => e.UInsertDate)
                .HasColumnType("timestamp")
                .HasColumnName("U_INSERT_DATE");

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.MUsers)
                .HasForeignKey(d => d.IdRole)
                .HasConstraintName("FK_M_USER_RELATIONS_M_ROLE");

            entity.HasOne(d => d.NikNavigation).WithMany(p => p.MUsers)
                .HasForeignKey(d => d.Nik)
                .HasConstraintName("FK_M_USER_RELATIONS_M_PEGAWA");
        });

        modelBuilder.Entity<PendukungBerka>(entity =>
        {
            entity.HasKey(e => e.IdPendukungBerkas).HasName("PRIMARY");

            entity.ToTable("pendukung_berkas");

            entity.HasIndex(e => e.IdBerkas, "FK_PENDUKUN_RELATIONS_BERKAS");

            entity.HasIndex(e => e.IdKategoriPendukungBerkas, "FK_PENDUKUN_RELATIONS_M_KATEGO");

            entity.Property(e => e.IdPendukungBerkas)
                .ValueGeneratedNever()
                .HasColumnName("ID_PENDUKUNG_BERKAS");
            entity.Property(e => e.FileBloob).HasColumnName("fileBloob");
            entity.Property(e => e.IdBerkas).HasColumnName("ID_BERKAS");
            entity.Property(e => e.IdKategoriPendukungBerkas).HasColumnName("ID_KATEGORI_PENDUKUNG_BERKAS");
            entity.Property(e => e.KeteranganPendukungBerkas).HasColumnName("KETERANGAN_PENDUKUNG_BERKAS");
            entity.Property(e => e.NamaDokumenPendukungBerkas)
                .HasMaxLength(255)
                .HasColumnName("NAMA_DOKUMEN_PENDUKUNG_BERKAS");
            entity.Property(e => e.PbInsertBy).HasColumnName("PB_INSERT_BY");
            entity.Property(e => e.PbInsertDate)
                .HasColumnType("timestamp")
                .HasColumnName("PB_INSERT_DATE");
            entity.Property(e => e.StatusPendukungBerkas).HasColumnName("STATUS_PENDUKUNG_BERKAS");

            entity.HasOne(d => d.IdBerkasNavigation).WithMany(p => p.PendukungBerkas)
                .HasForeignKey(d => d.IdBerkas)
                .HasConstraintName("FK_PENDUKUN_RELATIONS_BERKAS");

            entity.HasOne(d => d.IdKategoriPendukungBerkasNavigation).WithMany(p => p.PendukungBerkas)
                .HasForeignKey(d => d.IdKategoriPendukungBerkas)
                .HasConstraintName("FK_PENDUKUN_RELATIONS_M_KATEGO");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
