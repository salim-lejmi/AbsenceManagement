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

            modelBuilder.Entity<T_Seance>().HasData(
                new T_Seance { CodeSeance = 1, NomSeance = "S1", HeureDebut = DateTime.Parse("08:00"), HeureFin = DateTime.Parse("10:00") },
                new T_Seance { CodeSeance = 2, NomSeance = "S2", HeureDebut = DateTime.Parse("10:00"), HeureFin = DateTime.Parse("12:00") },
                new T_Seance { CodeSeance = 3, NomSeance = "S3", HeureDebut = DateTime.Parse("14:00"), HeureFin = DateTime.Parse("16:00") },
                new T_Seance { CodeSeance = 4, NomSeance = "S4", HeureDebut = DateTime.Parse("16:00"), HeureFin = DateTime.Parse("18:00") }
            );

            modelBuilder.Entity<T_Matiere>().HasData(
                new T_Matiere { CodeMatiere = 1, NomMatiere = "Anglais", NbrHeureCoursParSemaine = 2, NbrHeureTDParSemaine = 1, NbrHeureTPParSemaine = 0 },
                new T_Matiere { CodeMatiere = 2, NomMatiere = "Web Dev", NbrHeureCoursParSemaine = 3, NbrHeureTDParSemaine = 2, NbrHeureTPParSemaine = 1 },
                new T_Matiere { CodeMatiere = 3, NomMatiere = "SGBD", NbrHeureCoursParSemaine = 3, NbrHeureTDParSemaine = 2, NbrHeureTPParSemaine = 1 },
                new T_Matiere { CodeMatiere = 4, NomMatiere = "Programmation", NbrHeureCoursParSemaine = 4, NbrHeureTDParSemaine = 2, NbrHeureTPParSemaine = 2 },
                new T_Matiere { CodeMatiere = 5, NomMatiere = "Scrum", NbrHeureCoursParSemaine = 2, NbrHeureTDParSemaine = 1, NbrHeureTPParSemaine = 0 }
            );

            modelBuilder.Entity<T_Groupe>().HasData(
                new T_Groupe { CodeGroupe = 1, NomGroupe = "Groupe 1" },
                new T_Groupe { CodeGroupe = 2, NomGroupe = "Groupe 2" },
                new T_Groupe { CodeGroupe = 3, NomGroupe = "Groupe 3" }
            );

            modelBuilder.Entity<T_Grade>().HasData(
                new T_Grade { CodeGrade = 1, NomGrade = "Grade 1" },
                new T_Grade { CodeGrade = 2, NomGrade = "Grade 2" },
                new T_Grade { CodeGrade = 3, NomGrade = "Grade 3" }
            );

            modelBuilder.Entity<T_Departement>().HasData(
                new T_Departement { CodeDepartement = 1, NomDepartement = "Departement 1" },
                new T_Departement { CodeDepartement = 2, NomDepartement = "Departement 2" },
                new T_Departement { CodeDepartement = 3, NomDepartement = "Departement 3" }
            );

            modelBuilder.Entity<T_Classe>().HasData(
                new T_Classe { CodeClasse = 1, NomClasse = "Classe 1", CodeGroupe = 1, CodeDepartement = 1 },
                new T_Classe { CodeClasse = 2, NomClasse = "Classe 2", CodeGroupe = 1, CodeDepartement = 2 },
                new T_Classe { CodeClasse = 3, NomClasse = "Classe 3", CodeGroupe = 2, CodeDepartement = 2 },
                new T_Classe { CodeClasse = 4, NomClasse = "Classe 4", CodeGroupe = 2, CodeDepartement = 3 },
                new T_Classe { CodeClasse = 5, NomClasse = "Classe 5", CodeGroupe = 3, CodeDepartement = 3 }
            );

            modelBuilder.Entity<T_Etudiant>().HasData(
                new T_Etudiant
                {
                    CodeEtudiant = 1,
                    Nom = "John",
                    Prenom = "Doe",
                    DateNaissance = DateTime.Parse("2000-01-01"),
                    CodeClasse = 1,
                    NumInscription = "12345",
                    Adresse = "Adresse Etudiant",
                    Mail = "student@gmail.com",
                    Tel = "12345678",
                    Password = "0000"

                }
            );

            modelBuilder.Entity<T_Enseignant>().HasData(
                new T_Enseignant
                {
                    CodeEnseignant = 1,
                    Nom = "Richard",
                    Prenom = "Roe",
                    DateRecrutement = DateTime.Parse("2015-01-01"),
                    Adresse = "Adresse Enseignant",
                    Mail = "teacher@gmail.com",
                    Tel = "87654321",
                    Password = "0000",
                    CodeDepartement = 1,
                    CodeGrade = 1
                }
            );
            modelBuilder.Entity<T_Responsable>().HasData(
new T_Responsable
{
CodeResponsable = 1,
Nom = "John",
Prenom = "Smith",
Mail = "responsable@gmail.com",
Password = "0000"
}
);

            modelBuilder.Entity<T_User>().HasData(
       new T_User
       {
           UserId = 1,
           Email = "admin@gmail.com",
           Password = "admin", 
           UserType = "Admin"
       }
    );
            modelBuilder.Entity<T_User>().HasData(
 new T_User
 {
     UserId = 2,
     Email = "teacher@gmail.com",
     Password = "0000",
     UserType = "Teacher"
 }
);
            modelBuilder.Entity<T_User>().HasData(
 new T_User
 {
     UserId = 3,
     Email = "student@gmail.com",
     Password = "0000",
     UserType = "Student"
 }
);
            modelBuilder.Entity<T_User>().HasData(
 new T_User
 {
     UserId = 4,
     Email = "responsable@gmail.com",
     Password = "0000",
     UserType = "Responsable"
 }
);
   


        }


    }
}