using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyNews.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AvailableCategoriesUser");

            migrationBuilder.CreateTable(
                name: "NewsCategoriesWorkDepartment",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    workDepartmentId = table.Column<int>(type: "int", nullable: false),
                    newsCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsCategoriesWorkDepartment", x => x.id);
                    table.ForeignKey(
                        name: "FK_NewsCategoriesWorkDepartment_NewsCategory_newsCategoryId",
                        column: x => x.newsCategoryId,
                        principalTable: "NewsCategory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NewsCategoriesWorkDepartment_WorkDepartment_workDepartmentId",
                        column: x => x.workDepartmentId,
                        principalTable: "WorkDepartment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NewsCategoriesWorkDepartment_newsCategoryId",
                table: "NewsCategoriesWorkDepartment",
                column: "newsCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsCategoriesWorkDepartment_workDepartmentId",
                table: "NewsCategoriesWorkDepartment",
                column: "workDepartmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NewsCategoriesWorkDepartment");

            migrationBuilder.CreateTable(
                name: "AvailableCategoriesUser",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    accountId = table.Column<int>(type: "int", nullable: false),
                    newsCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvailableCategoriesUser", x => x.id);
                    table.ForeignKey(
                        name: "FK_AvailableCategoriesUser_Account_accountId",
                        column: x => x.accountId,
                        principalTable: "Account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AvailableCategoriesUser_NewsCategory_newsCategoryId",
                        column: x => x.newsCategoryId,
                        principalTable: "NewsCategory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AvailableCategoriesUser_accountId",
                table: "AvailableCategoriesUser",
                column: "accountId");

            migrationBuilder.CreateIndex(
                name: "IX_AvailableCategoriesUser_newsCategoryId",
                table: "AvailableCategoriesUser",
                column: "newsCategoryId");
        }
    }
}
