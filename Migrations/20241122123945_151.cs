using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace project.Migrations
{
    /// <inheritdoc />
    public partial class _151 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cd_group",
                columns: table => new
                {
                    group_id = table.Column<int>(type: "integer", nullable: false, comment: "Идентификатор записи группы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    c_group_name = table.Column<string>(type: "varchar", maxLength: 100, nullable: false, comment: "Название группы")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cd_group_group_id", x => x.group_id);
                });

            migrationBuilder.CreateTable(
                name: "cd_object",
                columns: table => new
                {
                    object_id = table.Column<int>(type: "integer", nullable: false, comment: "Идентификатор записи предмета")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    c_object_name = table.Column<string>(type: "varchar", maxLength: 100, nullable: false, comment: "Название предмета")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cd_object_object_id", x => x.object_id);
                });

            migrationBuilder.CreateTable(
                name: "cd_student",
                columns: table => new
                {
                    student_id = table.Column<int>(type: "integer", nullable: false, comment: "Идентификатор записи студента")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    c_student_firstname = table.Column<string>(type: " varchar ", maxLength: 100, nullable: false, comment: "Имя студента"),
                    c_student_lastname = table.Column<string>(type: " varchar ", maxLength: 100, nullable: false, comment: "Фамилия студента"),
                    c_student_middlename = table.Column<string>(type: " varchar ", maxLength: 100, nullable: false, comment: "Отчество студента"),
                    f_group_id = table.Column<int>(type: " int4 ", nullable: false, comment: "Идентификатор группы")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cd_student_student_id", x => x.student_id);
                    table.ForeignKey(
                        name: "fk_f_group_id",
                        column: x => x.f_group_id,
                        principalTable: "cd_group",
                        principalColumn: "group_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cd_curriculum",
                columns: table => new
                {
                    curriculum_id = table.Column<int>(type: "integer", nullable: false, comment: "Идентификатор учебного плана")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    f_group_id = table.Column<int>(type: "int", nullable: false, comment: "Идентификатор группы"),
                    f_object_id = table.Column<int>(type: "int", nullable: false, comment: "Идентификатор предмета"),
                    hours = table.Column<int>(type: "int", nullable: false, comment: "Количество часов для учебного плана")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cd_curriculum_curriculum_id", x => x.curriculum_id);
                    table.ForeignKey(
                        name: "FK_cd_curriculum_cd_group_f_group_id",
                        column: x => x.f_group_id,
                        principalTable: "cd_group",
                        principalColumn: "group_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_cd_curriculum_cd_object_f_object_id",
                        column: x => x.f_object_id,
                        principalTable: "cd_object",
                        principalColumn: "object_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_cd_curriculum_f_group_id",
                table: "cd_curriculum",
                column: "f_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_cd_curriculum_f_object_id",
                table: "cd_curriculum",
                column: "f_object_id");

            migrationBuilder.CreateIndex(
                name: "idx_cd_student_fk_f_group_id",
                table: "cd_student",
                column: "f_group_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cd_curriculum");

            migrationBuilder.DropTable(
                name: "cd_student");

            migrationBuilder.DropTable(
                name: "cd_object");

            migrationBuilder.DropTable(
                name: "cd_group");
        }
    }
}
