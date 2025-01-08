using Microsoft.EntityFrameworkCore;
using Absence.Models;

namespace Absence.Data
{
    public class AbsenceDbContext : DbContext
    {
        public AbsenceDbContext(DbContextOptions<AbsenceDbContext> options) : base(options) { }

        public DbSet<T_Classe> Classes { get; set; }
        public DbSet<T_Departement> Departements { get; set; }
        public DbSet<T_Enseignant> Enseignants { get; set; }
        public DbSet<T_Etudiant> Etudiants { get; set; }
        public DbSet<T_FicheAbsence> FichesAbsence { get; set; }
        public DbSet<T_FicheAbsenceSeance> FicheAbsenceSeances { get; set; }
        public DbSet<T_Grade> Grades { get; set; }
        public DbSet<T_Groupe> Groupes { get; set; }
        public DbSet<T_LigneFicheAbsence> LignesFicheAbsence { get; set; }
        public DbSet<T_Matiere> Matieres { get; set; }
        public DbSet<T_Seance> Seances { get; set; }
        public DbSet<T_User> Users { get; set; }
        public DbSet<T_Responsable> Responsables { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<T_FicheAbsenceSeance>()
                .HasKey(fas => new { fas.CodeFicheAbsence, fas.CodeSeance });

            modelBuilder.Entity<T_LigneFicheAbsence>()
                .HasKey(lfa => new { lfa.CodeFicheAbsence, lfa.CodeEtudiant });

            modelBuilder.Entity<T_FicheAbsence>()
                .HasOne(f => f.Classe)
                .WithMany(c => c.FichesAbsence)
                .HasForeignKey(f => f.CodeClasse)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<T_FicheAbsence>()
                .HasOne(f => f.Matiere)
                .WithMany(m => m.FichesAbsence)
                .HasForeignKey(f => f.CodeMatiere)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<T_FicheAbsence>()
                .HasOne(f => f.Enseignant)
                .WithMany(e => e.FichesAbsence)
                .HasForeignKey(f => f.CodeEnseignant)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<T_Groupe>()
                .HasMany(g => g.Classes)
                .WithOne(c => c.Groupe)
                .HasForeignKey(c => c.CodeGroupe);

            modelBuilder.Entity<T_Departement>()
                .HasMany(d => d.Classes)
                .WithOne(c => c.Departement)
                .HasForeignKey(c => c.CodeDepartement);

            modelBuilder.Entity<T_Departement>()
                .HasMany(d => d.Enseignants)
                .WithOne(e => e.Departement)
                .HasForeignKey(e => e.CodeDepartement);

            modelBuilder.Entity<T_Grade>()
                .HasMany(g => g.Enseignants)
                .WithOne(e => e.Grade)
                .HasForeignKey(e => e.CodeGrade);

            modelBuilder.Entity<T_Seance>()
                .HasMany(s => s.FichesAbsenceSeances)
                .WithOne(fas => fas.Seance)
                .HasForeignKey(fas => fas.CodeSeance);

            modelBuilder.Entity<T_Classe>()
                .HasMany(c => c.Etudiants)
                .WithOne(e => e.Classe)
                .HasForeignKey(e => e.CodeClasse);
            modelBuilder.Entity<T_User>().HasData(
         new T_User
         {
             UserId = 1,
             Email = "admin@gmail.com",
             Password = "admin",
             UserType = "Admin"
         }
     );


        }

    }
}