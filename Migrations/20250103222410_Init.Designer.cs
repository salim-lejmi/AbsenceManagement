﻿// <auto-generated />
using System;
using Absence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AbsenceManagement.Migrations
{
    [DbContext(typeof(AbsenceDbContext))]
    [Migration("20250103222410_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Absence.Models.T_Classe", b =>
                {
                    b.Property<int>("CodeClasse")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CodeClasse"));

                    b.Property<int>("CodeDepartement")
                        .HasColumnType("int");

                    b.Property<int>("CodeGroupe")
                        .HasColumnType("int");

                    b.Property<string>("NomClasse")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CodeClasse");

                    b.HasIndex("CodeDepartement");

                    b.HasIndex("CodeGroupe");

                    b.ToTable("Classes");
                });

            modelBuilder.Entity("Absence.Models.T_Departement", b =>
                {
                    b.Property<int>("CodeDepartement")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CodeDepartement"));

                    b.Property<string>("NomDepartement")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CodeDepartement");

                    b.ToTable("Departements");
                });

            modelBuilder.Entity("Absence.Models.T_Enseignant", b =>
                {
                    b.Property<int>("CodeEnseignant")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CodeEnseignant"));

                    b.Property<string>("Adresse")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CodeDepartement")
                        .HasColumnType("int");

                    b.Property<int>("CodeGrade")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateRecrutement")
                        .HasColumnType("datetime2");

                    b.Property<string>("Mail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Prenom")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Tel")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CodeEnseignant");

                    b.HasIndex("CodeDepartement");

                    b.HasIndex("CodeGrade");

                    b.ToTable("Enseignants");
                });

            modelBuilder.Entity("Absence.Models.T_Etudiant", b =>
                {
                    b.Property<int>("CodeEtudiant")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CodeEtudiant"));

                    b.Property<string>("Adresse")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CodeClasse")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateNaissance")
                        .HasColumnType("datetime2");

                    b.Property<string>("Mail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NumInscription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Prenom")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Tel")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CodeEtudiant");

                    b.HasIndex("CodeClasse");

                    b.ToTable("Etudiants");
                });

            modelBuilder.Entity("Absence.Models.T_FicheAbsence", b =>
                {
                    b.Property<int>("CodeFicheAbsence")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CodeFicheAbsence"));

                    b.Property<int>("CodeClasse")
                        .HasColumnType("int");

                    b.Property<int>("CodeEnseignant")
                        .HasColumnType("int");

                    b.Property<int>("CodeMatiere")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateJour")
                        .HasColumnType("datetime2");

                    b.HasKey("CodeFicheAbsence");

                    b.HasIndex("CodeClasse");

                    b.HasIndex("CodeEnseignant");

                    b.HasIndex("CodeMatiere");

                    b.ToTable("FichesAbsence");
                });

            modelBuilder.Entity("Absence.Models.T_FicheAbsenceSeance", b =>
                {
                    b.Property<int>("CodeFicheAbsence")
                        .HasColumnType("int");

                    b.Property<int>("CodeSeance")
                        .HasColumnType("int");

                    b.Property<int>("CodeFicheAbsenceSeance")
                        .HasColumnType("int");

                    b.HasKey("CodeFicheAbsence", "CodeSeance");

                    b.HasIndex("CodeSeance");

                    b.ToTable("FicheAbsenceSeances");
                });

            modelBuilder.Entity("Absence.Models.T_Grade", b =>
                {
                    b.Property<int>("CodeGrade")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CodeGrade"));

                    b.Property<string>("NomGrade")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CodeGrade");

                    b.ToTable("Grades");
                });

            modelBuilder.Entity("Absence.Models.T_Groupe", b =>
                {
                    b.Property<int>("CodeGroupe")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CodeGroupe"));

                    b.Property<string>("NomGroupe")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CodeGroupe");

                    b.ToTable("Groupes");
                });

            modelBuilder.Entity("Absence.Models.T_LigneFicheAbsence", b =>
                {
                    b.Property<int>("CodeFicheAbsence")
                        .HasColumnType("int");

                    b.Property<int>("CodeEtudiant")
                        .HasColumnType("int");

                    b.HasKey("CodeFicheAbsence", "CodeEtudiant");

                    b.HasIndex("CodeEtudiant");

                    b.ToTable("LignesFicheAbsence");
                });

            modelBuilder.Entity("Absence.Models.T_Matiere", b =>
                {
                    b.Property<int>("CodeMatiere")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CodeMatiere"));

                    b.Property<int>("NbrHeureCoursParSemaine")
                        .HasColumnType("int");

                    b.Property<int>("NbrHeureTDParSemaine")
                        .HasColumnType("int");

                    b.Property<int>("NbrHeureTPParSemaine")
                        .HasColumnType("int");

                    b.Property<string>("NomMatiere")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CodeMatiere");

                    b.ToTable("Matieres");
                });

            modelBuilder.Entity("Absence.Models.T_Seance", b =>
                {
                    b.Property<int>("CodeSeance")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CodeSeance"));

                    b.Property<DateTime>("HeureDebut")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("HeureFin")
                        .HasColumnType("datetime2");

                    b.Property<string>("NomSeance")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CodeSeance");

                    b.ToTable("Seances");
                });

            modelBuilder.Entity("Absence.Models.T_Classe", b =>
                {
                    b.HasOne("Absence.Models.T_Departement", "Departement")
                        .WithMany("Classes")
                        .HasForeignKey("CodeDepartement")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Absence.Models.T_Groupe", "Groupe")
                        .WithMany("Classes")
                        .HasForeignKey("CodeGroupe")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Departement");

                    b.Navigation("Groupe");
                });

            modelBuilder.Entity("Absence.Models.T_Enseignant", b =>
                {
                    b.HasOne("Absence.Models.T_Departement", "Departement")
                        .WithMany("Enseignants")
                        .HasForeignKey("CodeDepartement")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Absence.Models.T_Grade", "Grade")
                        .WithMany("Enseignants")
                        .HasForeignKey("CodeGrade")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Departement");

                    b.Navigation("Grade");
                });

            modelBuilder.Entity("Absence.Models.T_Etudiant", b =>
                {
                    b.HasOne("Absence.Models.T_Classe", "Classe")
                        .WithMany("Etudiants")
                        .HasForeignKey("CodeClasse")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Classe");
                });

            modelBuilder.Entity("Absence.Models.T_FicheAbsence", b =>
                {
                    b.HasOne("Absence.Models.T_Classe", "Classe")
                        .WithMany("FichesAbsence")
                        .HasForeignKey("CodeClasse")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Absence.Models.T_Enseignant", "Enseignant")
                        .WithMany("FichesAbsence")
                        .HasForeignKey("CodeEnseignant")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Absence.Models.T_Matiere", "Matiere")
                        .WithMany("FichesAbsence")
                        .HasForeignKey("CodeMatiere")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Classe");

                    b.Navigation("Enseignant");

                    b.Navigation("Matiere");
                });

            modelBuilder.Entity("Absence.Models.T_FicheAbsenceSeance", b =>
                {
                    b.HasOne("Absence.Models.T_FicheAbsence", "FicheAbsence")
                        .WithMany("FichesAbsenceSeances")
                        .HasForeignKey("CodeFicheAbsence")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Absence.Models.T_Seance", "Seance")
                        .WithMany("FichesAbsenceSeances")
                        .HasForeignKey("CodeSeance")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FicheAbsence");

                    b.Navigation("Seance");
                });

            modelBuilder.Entity("Absence.Models.T_LigneFicheAbsence", b =>
                {
                    b.HasOne("Absence.Models.T_Etudiant", "Etudiant")
                        .WithMany("LignesFicheAbsence")
                        .HasForeignKey("CodeEtudiant")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Absence.Models.T_FicheAbsence", "FicheAbsence")
                        .WithMany("LignesFicheAbsence")
                        .HasForeignKey("CodeFicheAbsence")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Etudiant");

                    b.Navigation("FicheAbsence");
                });

            modelBuilder.Entity("Absence.Models.T_Classe", b =>
                {
                    b.Navigation("Etudiants");

                    b.Navigation("FichesAbsence");
                });

            modelBuilder.Entity("Absence.Models.T_Departement", b =>
                {
                    b.Navigation("Classes");

                    b.Navigation("Enseignants");
                });

            modelBuilder.Entity("Absence.Models.T_Enseignant", b =>
                {
                    b.Navigation("FichesAbsence");
                });

            modelBuilder.Entity("Absence.Models.T_Etudiant", b =>
                {
                    b.Navigation("LignesFicheAbsence");
                });

            modelBuilder.Entity("Absence.Models.T_FicheAbsence", b =>
                {
                    b.Navigation("FichesAbsenceSeances");

                    b.Navigation("LignesFicheAbsence");
                });

            modelBuilder.Entity("Absence.Models.T_Grade", b =>
                {
                    b.Navigation("Enseignants");
                });

            modelBuilder.Entity("Absence.Models.T_Groupe", b =>
                {
                    b.Navigation("Classes");
                });

            modelBuilder.Entity("Absence.Models.T_Matiere", b =>
                {
                    b.Navigation("FichesAbsence");
                });

            modelBuilder.Entity("Absence.Models.T_Seance", b =>
                {
                    b.Navigation("FichesAbsenceSeances");
                });
#pragma warning restore 612, 618
        }
    }
}