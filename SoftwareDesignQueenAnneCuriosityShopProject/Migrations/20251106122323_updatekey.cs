using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftwareDesignQueenAnneCuriosityShopProject.Migrations
{
    public partial class updatekey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClassAdviser",
                columns: table => new
                {
                    ClassAdviserID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "varchar(50)", nullable: false),
                    LastName = table.Column<string>(type: "varchar(50)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassAdviser", x => x.ClassAdviserID);
                });

            migrationBuilder.CreateTable(
                name: "Parent",
                columns: table => new
                {
                    ParentID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "varchar(50)", nullable: false),
                    LastName = table.Column<string>(type: "varchar(50)", nullable: false),
                    Email = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parent", x => x.ParentID);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "varchar(50)", nullable: false),
                    LastName = table.Column<string>(type: "varchar(50)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "varchar(15)", nullable: false),
                    Email = table.Column<string>(type: "varchar(100)", nullable: false),
                    IsAdmin = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "Advisory",
                columns: table => new
                {
                    AdvisoryID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "varchar(100)", nullable: false),
                    SectionName = table.Column<string>(type: "varchar(50)", nullable: false),
                    SchoolYear = table.Column<string>(type: "varchar(9)", nullable: false),
                    ClassAdviserID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Advisory", x => x.AdvisoryID);
                    table.ForeignKey(
                        name: "FK_Advisory_ClassAdviser_ClassAdviserID",
                        column: x => x.ClassAdviserID,
                        principalTable: "ClassAdviser",
                        principalColumn: "ClassAdviserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contact",
                columns: table => new
                {
                    ContactID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PhoneNumber = table.Column<string>(type: "varchar(50)", nullable: false),
                    Network = table.Column<string>(type: "varchar(50)", nullable: false),
                    ParentID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact", x => x.ContactID);
                    table.ForeignKey(
                        name: "FK_Contact_Parent_ParentID",
                        column: x => x.ParentID,
                        principalTable: "Parent",
                        principalColumn: "ParentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    StudentID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "varchar(50)", nullable: false),
                    LastName = table.Column<string>(type: "varchar(50)", nullable: false),
                    LRN = table.Column<int>(type: "INTEGER", nullable: false),
                    EnrollmentStatus = table.Column<bool>(type: "INTEGER", nullable: false),
                    PhoneNumber = table.Column<string>(type: "varchar(15)", nullable: false),
                    AdvisoryID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.StudentID);
                    table.ForeignKey(
                        name: "FK_Student_Advisory_AdvisoryID",
                        column: x => x.AdvisoryID,
                        principalTable: "Advisory",
                        principalColumn: "AdvisoryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Attendance",
                columns: table => new
                {
                    AttendanceID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    isPresent = table.Column<bool>(type: "INTEGER", nullable: false),
                    StudentID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendance", x => x.AttendanceID);
                    table.ForeignKey(
                        name: "FK_Attendance_Student_StudentID",
                        column: x => x.StudentID,
                        principalTable: "Student",
                        principalColumn: "StudentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Relationship",
                columns: table => new
                {
                    RelationshipID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TypeOfRelationship = table.Column<int>(type: "INTEGER", nullable: false),
                    StudentID = table.Column<int>(type: "INTEGER", nullable: true),
                    ParentID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Relationship", x => x.RelationshipID);
                    table.ForeignKey(
                        name: "FK_Relationship_Parent_ParentID",
                        column: x => x.ParentID,
                        principalTable: "Parent",
                        principalColumn: "ParentID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Relationship_Student_StudentID",
                        column: x => x.StudentID,
                        principalTable: "Student",
                        principalColumn: "StudentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    NotificationID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Message = table.Column<string>(type: "varchar(255)", nullable: false),
                    AttendanceID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.NotificationID);
                    table.ForeignKey(
                        name: "FK_Notification_Attendance_AttendanceID",
                        column: x => x.AttendanceID,
                        principalTable: "Attendance",
                        principalColumn: "AttendanceID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Delivered",
                columns: table => new
                {
                    DeliveredID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DateTimeSent = table.Column<DateTime>(type: "datetime", nullable: false),
                    NotificationID = table.Column<int>(type: "INTEGER", nullable: true),
                    ContactID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Delivered", x => x.DeliveredID);
                    table.ForeignKey(
                        name: "FK_Delivered_Contact_ContactID",
                        column: x => x.ContactID,
                        principalTable: "Contact",
                        principalColumn: "ContactID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Delivered_Notification_NotificationID",
                        column: x => x.NotificationID,
                        principalTable: "Notification",
                        principalColumn: "NotificationID",
                        onDelete: ReferentialAction.Cascade);
                });

            // Indexes
            migrationBuilder.CreateIndex(name: "IX_Advisory_ClassAdviserID", table: "Advisory", column: "ClassAdviserID");
            migrationBuilder.CreateIndex(name: "IX_Attendance_StudentID", table: "Attendance", column: "StudentID");
            migrationBuilder.CreateIndex(name: "IX_Contact_ParentID", table: "Contact", column: "ParentID");
            migrationBuilder.CreateIndex(name: "IX_Delivered_ContactID", table: "Delivered", column: "ContactID");
            migrationBuilder.CreateIndex(name: "IX_Delivered_NotificationID", table: "Delivered", column: "NotificationID");
            migrationBuilder.CreateIndex(name: "IX_Notification_AttendanceID", table: "Notification", column: "AttendanceID");
            migrationBuilder.CreateIndex(name: "IX_Relationship_ParentID", table: "Relationship", column: "ParentID");
            migrationBuilder.CreateIndex(name: "IX_Relationship_StudentID", table: "Relationship", column: "StudentID");
            migrationBuilder.CreateIndex(name: "IX_Student_AdvisoryID", table: "Student", column: "AdvisoryID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Delivered");
            migrationBuilder.DropTable(name: "Relationship");
            migrationBuilder.DropTable(name: "User");
            migrationBuilder.DropTable(name: "Contact");
            migrationBuilder.DropTable(name: "Notification");
            migrationBuilder.DropTable(name: "Parent");
            migrationBuilder.DropTable(name: "Attendance");
            migrationBuilder.DropTable(name: "Student");
            migrationBuilder.DropTable(name: "Advisory");
            migrationBuilder.DropTable(name: "ClassAdviser");
        }
    }
}
