using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Absence.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departements",
                columns: table => new
                {
                    CodeDepartement = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomDepartement = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departements", x => x.CodeDepartement);
                });

            migrationBuilder.CreateTable(
                name: "Grades",
                columns: table => new
                {
                    CodeGrade = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomGrade = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grades", x => x.CodeGrade);
                });

            migrationBuilder.CreateTable(
                name: "Groupes",
                columns: table => new
                {
                    CodeGroupe = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomGroupe = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groupes", x => x.CodeGroupe);
                });

            migrationBuilder.CreateTable(
                name: "Matieres",
                columns: table => new
                {
                    CodeMatiere = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomMatiere = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NbrHeureCoursParSemaine = table.Column<int>(type: "int", nullable: false),
                    NbrHeureTDParSemaine = table.Column<int>(type: "int", nullable: false),
                    NbrHeureTPParSemaine = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matieres", x => x.CodeMatiere);
                });

            migrationBuilder.CreateTable(
                name: "Responsables",
                columns: table => new
                {
                    CodeResponsable = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prenom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Responsables", x => x.CodeResponsable);
                });

            migrationBuilder.CreateTable(
                name: "Seances",
                columns: table => new
                {
                    CodeSeance = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomSeance = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HeureDebut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HeureFin = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seances", x => x.CodeSeance);
                });

            migrationBuilder.CreateTable(
                name: "Enseignants",
                columns: table => new
                {
                    CodeEnseignant = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prenom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateRecrutement = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Adresse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodeDepartement = table.Column<int>(type: "int", nullable: false),
                    CodeGrade = table.Column<int>(type: "int", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enseignants", x => x.CodeEnseignant);
                    table.ForeignKey(
                        name: "FK_Enseignants_Departements_CodeDepartement",
                        column: x => x.CodeDepartement,
                        principalTable: "Departements",
                        principalColumn: "CodeDepartement",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Enseignants_Grades_CodeGrade",
                        column: x => x.CodeGrade,
                        principalTable: "Grades",
                        principalColumn: "CodeGrade",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    CodeClasse = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomClasse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodeGroupe = table.Column<int>(type: "int", nullable: false),
                    CodeDepartement = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.CodeClasse);
                    table.ForeignKey(
                        name: "FK_Classes_Departements_CodeDepartement",
                        column: x => x.CodeDepartement,
                        principalTable: "Departements",
                        principalColumn: "CodeDepartement",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Classes_Groupes_CodeGroupe",
                        column: x => x.CodeGroupe,
                        principalTable: "Groupes",
                        principalColumn: "CodeGroupe",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Etudiants",
                columns: table => new
                {
                    CodeEtudiant = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prenom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateNaissance = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CodeClasse = table.Column<int>(type: "int", nullable: false),
                    NumInscription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adresse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Etudiants", x => x.CodeEtudiant);
                    table.ForeignKey(
                        name: "FK_Etudiants_Classes_CodeClasse",
                        column: x => x.CodeClasse,
                        principalTable: "Classes",
                        principalColumn: "CodeClasse",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FichesAbsence",
                columns: table => new
                {
                    CodeFicheAbsence = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateJour = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CodeMatiere = table.Column<int>(type: "int", nullable: false),
                    CodeEnseignant = table.Column<int>(type: "int", nullable: false),
                    CodeClasse = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FichesAbsence", x => x.CodeFicheAbsence);
                    table.ForeignKey(
                        name: "FK_FichesAbsence_Classes_CodeClasse",
                        column: x => x.CodeClasse,
                        principalTable: "Classes",
                        principalColumn: "CodeClasse",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FichesAbsence_Enseignants_CodeEnseignant",
                        column: x => x.CodeEnseignant,
                        principalTable: "Enseignants",
                        principalColumn: "CodeEnseignant",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FichesAbsence_Matieres_CodeMatiere",
                        column: x => x.CodeMatiere,
                        principalTable: "Matieres",
                        principalColumn: "CodeMatiere",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TeacherId = table.Column<int>(type: "int", nullable: true),
                    StudentId = table.Column<int>(type: "int", nullable: true),
                    ResponsableId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Enseignants_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Enseignants",
                        principalColumn: "CodeEnseignant");
                    table.ForeignKey(
                        name: "FK_Users_Etudiants_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Etudiants",
                        principalColumn: "CodeEtudiant");
                    table.ForeignKey(
                        name: "FK_Users_Responsables_ResponsableId",
                        column: x => x.ResponsableId,
                        principalTable: "Responsables",
                        principalColumn: "CodeResponsable");
                });

            migrationBuilder.CreateTable(
                name: "FicheAbsenceSeances",
                columns: table => new
                {
                    CodeFicheAbsence = table.Column<int>(type: "int", nullable: false),
                    CodeSeance = table.Column<int>(type: "int", nullable: false),
                    CodeFicheAbsenceSeance = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FicheAbsenceSeances", x => new { x.CodeFicheAbsence, x.CodeSeance });
                    table.ForeignKey(
                        name: "FK_FicheAbsenceSeances_FichesAbsence_CodeFicheAbsence",
                        column: x => x.CodeFicheAbsence,
                        principalTable: "FichesAbsence",
                        principalColumn: "CodeFicheAbsence",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FicheAbsenceSeances_Seances_CodeSeance",
                        column: x => x.CodeSeance,
                        principalTable: "Seances",
                        principalColumn: "CodeSeance",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LignesFicheAbsence",
                columns: table => new
                {
                    CodeFicheAbsence = table.Column<int>(type: "int", nullable: false),
                    CodeEtudiant = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LignesFicheAbsence", x => new { x.CodeFicheAbsence, x.CodeEtudiant });
                    table.ForeignKey(
                        name: "FK_LignesFicheAbsence_Etudiants_CodeEtudiant",
                        column: x => x.CodeEtudiant,
                        principalTable: "Etudiants",
                        principalColumn: "CodeEtudiant",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LignesFicheAbsence_FichesAbsence_CodeFicheAbsence",
                        column: x => x.CodeFicheAbsence,
                        principalTable: "FichesAbsence",
                        principalColumn: "CodeFicheAbsence",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Departements",
                columns: new[] { "CodeDepartement", "NomDepartement" },
                values: new object[,]
                {
                    { 1, "Departement 1" },
                    { 2, "Departement 2" },
                    { 3, "Departement 3" }
                });

            migrationBuilder.InsertData(
                table: "Grades",
                columns: new[] { "CodeGrade", "NomGrade" },
                values: new object[,]
                {
                    { 1, "Grade 1" },
                    { 2, "Grade 2" },
                    { 3, "Grade 3" }
                });

            migrationBuilder.InsertData(
                table: "Groupes",
                columns: new[] { "CodeGroupe", "NomGroupe" },
                values: new object[,]
                {
                    { 1, "Groupe 1" },
                    { 2, "Groupe 2" },
                    { 3, "Groupe 3" }
                });

            migrationBuilder.InsertData(
                table: "Matieres",
                columns: new[] { "CodeMatiere", "NbrHeureCoursParSemaine", "NbrHeureTDParSemaine", "NbrHeureTPParSemaine", "NomMatiere" },
                values: new object[,]
                {
                    { 1, 2, 1, 0, "Anglais" },
                    { 2, 3, 2, 1, "Web Dev" },
                    { 3, 3, 2, 1, "SGBD" },
                    { 4, 4, 2, 2, "Programmation" },
                    { 5, 2, 1, 0, "Scrum" }
                });

            migrationBuilder.InsertData(
                table: "Responsables",
                columns: new[] { "CodeResponsable", "Mail", "Nom", "Password", "Prenom" },
                values: new object[] { 1, "responsable@gmail.com", "John", "0000", "Smith" });

            migrationBuilder.InsertData(
                table: "Seances",
                columns: new[] { "CodeSeance", "HeureDebut", "HeureFin", "NomSeance" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 10, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 10, 10, 0, 0, 0, DateTimeKind.Unspecified), "S1" },
                    { 2, new DateTime(2025, 1, 10, 10, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 10, 12, 0, 0, 0, DateTimeKind.Unspecified), "S2" },
                    { 3, new DateTime(2025, 1, 10, 14, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 10, 16, 0, 0, 0, DateTimeKind.Unspecified), "S3" },
                    { 4, new DateTime(2025, 1, 10, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 10, 18, 0, 0, 0, DateTimeKind.Unspecified), "S4" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Email", "Password", "ResponsableId", "StudentId", "TeacherId", "UserType" },
                values: new object[,]
                {
                    { 1, "admin@gmail.com", "admin", null, null, null, "Admin" },
                    { 2, "teacher@gmail.com", "0000", null, null, null, "Teacher" },
                    { 3, "student@gmail.com", "0000", null, null, null, "Student" },
                    { 4, "responsable@gmail.com", "0000", null, null, null, "Responsable" }
                });

            migrationBuilder.InsertData(
                table: "Classes",
                columns: new[] { "CodeClasse", "CodeDepartement", "CodeGroupe", "NomClasse" },
                values: new object[,]
                {
                    { 1, 1, 1, "Classe 1" },
                    { 2, 2, 1, "Classe 2" },
                    { 3, 2, 2, "Classe 3" },
                    { 4, 3, 2, "Classe 4" },
                    { 5, 3, 3, "Classe 5" }
                });

            migrationBuilder.InsertData(
                table: "Enseignants",
                columns: new[] { "CodeEnseignant", "Adresse", "CodeDepartement", "CodeGrade", "DateRecrutement", "Mail", "Nom", "Password", "Prenom", "Tel" },
                values: new object[] { 1, "Adresse Enseignant", 1, 1, new DateTime(2015, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "teacher@gmail.com", "Richard", "0000", "Roe", "87654321" });

            migrationBuilder.InsertData(
                table: "Etudiants",
                columns: new[] { "CodeEtudiant", "Adresse", "CodeClasse", "DateNaissance", "Mail", "Nom", "NumInscription", "Password", "Prenom", "Tel" },
                values: new object[] { 1, "Adresse Etudiant", 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "student@gmail.com", "John", "12345", "0000", "Doe", "12345678" });

            migrationBuilder.CreateIndex(
                name: "IX_Classes_CodeDepartement",
                table: "Classes",
                column: "CodeDepartement");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_CodeGroupe",
                table: "Classes",
                column: "CodeGroupe");

            migrationBuilder.CreateIndex(
                name: "IX_Enseignants_CodeDepartement",
                table: "Enseignants",
                column: "CodeDepartement");

            migrationBuilder.CreateIndex(
                name: "IX_Enseignants_CodeGrade",
                table: "Enseignants",
                column: "CodeGrade");

            migrationBuilder.CreateIndex(
                name: "IX_Etudiants_CodeClasse",
                table: "Etudiants",
                column: "CodeClasse");

            migrationBuilder.CreateIndex(
                name: "IX_FicheAbsenceSeances_CodeSeance",
                table: "FicheAbsenceSeances",
                column: "CodeSeance");

            migrationBuilder.CreateIndex(
                name: "IX_FichesAbsence_CodeClasse",
                table: "FichesAbsence",
                column: "CodeClasse");

            migrationBuilder.CreateIndex(
                name: "IX_FichesAbsence_CodeEnseignant",
                table: "FichesAbsence",
                column: "CodeEnseignant");

            migrationBuilder.CreateIndex(
                name: "IX_FichesAbsence_CodeMatiere",
                table: "FichesAbsence",
                column: "CodeMatiere");

            migrationBuilder.CreateIndex(
                name: "IX_LignesFicheAbsence_CodeEtudiant",
                table: "LignesFicheAbsence",
                column: "CodeEtudiant");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ResponsableId",
                table: "Users",
                column: "ResponsableId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_StudentId",
                table: "Users",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_TeacherId",
                table: "Users",
                column: "TeacherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FicheAbsenceSeances");

            migrationBuilder.DropTable(
                name: "LignesFicheAbsence");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Seances");

            migrationBuilder.DropTable(
                name: "FichesAbsence");

            migrationBuilder.DropTable(
                name: "Etudiants");

            migrationBuilder.DropTable(
                name: "Responsables");

            migrationBuilder.DropTable(
                name: "Enseignants");

            migrationBuilder.DropTable(
                name: "Matieres");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropTable(
                name: "Grades");

            migrationBuilder.DropTable(
                name: "Departements");

            migrationBuilder.DropTable(
                name: "Groupes");
        }
    }
}
