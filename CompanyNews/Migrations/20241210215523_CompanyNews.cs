using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyNews.Migrations
{
    /// <inheritdoc />
    public partial class CompanyNews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NewsCategory",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    isArchived = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsCategory", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "WorkDepartment",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkDepartment", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "NewsPost",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    newsCategoryId = table.Column<int>(type: "int", nullable: false),
                    datePublication = table.Column<DateTime>(type: "datetime2", nullable: false),
                    image = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    message = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsPost", x => x.id);
                    table.ForeignKey(
                        name: "FK_NewsPost_NewsCategory_newsCategoryId",
                        column: x => x.newsCategoryId,
                        principalTable: "NewsCategory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    login = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    accountRole = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    workDepartmentId = table.Column<int>(type: "int", nullable: true),
                    phoneNumber = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    surname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    patronymic = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    image = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    isProfileBlocked = table.Column<bool>(type: "bit", nullable: false),
                    reasonBlockingAccount = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    isCanLeaveComments = table.Column<bool>(type: "bit", nullable: false),
                    reasonBlockingMessages = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.id);
                    table.ForeignKey(
                        name: "FK_Account_WorkDepartment_workDepartmentId",
                        column: x => x.workDepartmentId,
                        principalTable: "WorkDepartment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

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

            migrationBuilder.CreateTable(
                name: "MessageUser",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    datePublication = table.Column<DateTime>(type: "datetime2", nullable: false),
                    newsPostId = table.Column<int>(type: "int", nullable: false),
                    accountId = table.Column<int>(type: "int", nullable: false),
                    message = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    rejectionReason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageUser", x => x.id);
                    table.ForeignKey(
                        name: "FK_MessageUser_Account_accountId",
                        column: x => x.accountId,
                        principalTable: "Account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MessageUser_NewsPost_newsPostId",
                        column: x => x.newsPostId,
                        principalTable: "NewsPost",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_workDepartmentId",
                table: "Account",
                column: "workDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AvailableCategoriesUser_accountId",
                table: "AvailableCategoriesUser",
                column: "accountId");

            migrationBuilder.CreateIndex(
                name: "IX_AvailableCategoriesUser_newsCategoryId",
                table: "AvailableCategoriesUser",
                column: "newsCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageUser_accountId",
                table: "MessageUser",
                column: "accountId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageUser_newsPostId",
                table: "MessageUser",
                column: "newsPostId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsPost_newsCategoryId",
                table: "NewsPost",
                column: "newsCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AvailableCategoriesUser");

            migrationBuilder.DropTable(
                name: "MessageUser");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "NewsPost");

            migrationBuilder.DropTable(
                name: "WorkDepartment");

            migrationBuilder.DropTable(
                name: "NewsCategory");
        }
    }
}
