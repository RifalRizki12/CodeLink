using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class CreateDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tb_m_experiences",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    position = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    company = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    modified_date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_experiences", x => x.guid);
                });

            migrationBuilder.CreateTable(
                name: "tb_m_roles",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(25)", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    modified_date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_roles", x => x.guid);
                });

            migrationBuilder.CreateTable(
                name: "tb_m_skills",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    hard = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    soft = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    modified_date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_skills", x => x.guid);
                });

            migrationBuilder.CreateTable(
                name: "tb_m_accounts",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    otp = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    is_used = table.Column<bool>(type: "bit", nullable: false),
                    expired_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    modified_date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_accounts", x => x.guid);
                });

            migrationBuilder.CreateTable(
                name: "tb_m_accounts_roles",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    account_guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    role_guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    modified_date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_accounts_roles", x => x.guid);
                    table.ForeignKey(
                        name: "FK_tb_m_accounts_roles_tb_m_accounts_account_guid",
                        column: x => x.account_guid,
                        principalTable: "tb_m_accounts",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tb_m_accounts_roles_tb_m_roles_role_guid",
                        column: x => x.role_guid,
                        principalTable: "tb_m_roles",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tb_m_companies",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    address = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    employee_guid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    modified_date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_companies", x => x.guid);
                });

            migrationBuilder.CreateTable(
                name: "tb_m_employees",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    first_name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    gender = table.Column<int>(type: "int", nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    phone_number = table.Column<string>(type: "nvarchar(16)", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    hire_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    expired_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    company_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    modified_date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_employees", x => x.guid);
                    table.ForeignKey(
                        name: "FK_tb_m_employees_tb_m_companies_company_id",
                        column: x => x.company_id,
                        principalTable: "tb_m_companies",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tb_m_experiences_skills",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    skills_guid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    experiences_guid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    employee_guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    modified_date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_experiences_skills", x => x.guid);
                    table.ForeignKey(
                        name: "FK_tb_m_experiences_skills_tb_m_employees_employee_guid",
                        column: x => x.employee_guid,
                        principalTable: "tb_m_employees",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tb_m_experiences_skills_tb_m_experiences_experiences_guid",
                        column: x => x.experiences_guid,
                        principalTable: "tb_m_experiences",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tb_m_experiences_skills_tb_m_skills_skills_guid",
                        column: x => x.skills_guid,
                        principalTable: "tb_m_skills",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tb_m_ratings",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    rating = table.Column<int>(type: "int", nullable: false),
                    employee_guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    modified_date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_ratings", x => x.guid);
                    table.ForeignKey(
                        name: "FK_tb_m_ratings_tb_m_employees_employee_guid",
                        column: x => x.employee_guid,
                        principalTable: "tb_m_employees",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tb_m_salaries",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    basic_salary = table.Column<int>(type: "int", nullable: false),
                    overtime_pay = table.Column<int>(type: "int", nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    modified_date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_salaries", x => x.guid);
                    table.ForeignKey(
                        name: "FK_tb_m_salaries_tb_m_employees_guid",
                        column: x => x.guid,
                        principalTable: "tb_m_employees",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tb_m_test",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    employee_guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    modified_date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_test", x => x.guid);
                    table.ForeignKey(
                        name: "FK_tb_m_test_tb_m_employees_employee_guid",
                        column: x => x.employee_guid,
                        principalTable: "tb_m_employees",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_accounts_roles_account_guid",
                table: "tb_m_accounts_roles",
                column: "account_guid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_accounts_roles_role_guid",
                table: "tb_m_accounts_roles",
                column: "role_guid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_companies_employee_guid",
                table: "tb_m_companies",
                column: "employee_guid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_employees_company_id",
                table: "tb_m_employees",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_employees_email",
                table: "tb_m_employees",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_employees_phone_number",
                table: "tb_m_employees",
                column: "phone_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_experiences_skills_employee_guid",
                table: "tb_m_experiences_skills",
                column: "employee_guid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_experiences_skills_experiences_guid",
                table: "tb_m_experiences_skills",
                column: "experiences_guid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_experiences_skills_skills_guid",
                table: "tb_m_experiences_skills",
                column: "skills_guid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_ratings_employee_guid",
                table: "tb_m_ratings",
                column: "employee_guid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_test_employee_guid",
                table: "tb_m_test",
                column: "employee_guid");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_m_accounts_tb_m_employees_guid",
                table: "tb_m_accounts",
                column: "guid",
                principalTable: "tb_m_employees",
                principalColumn: "guid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_m_companies_tb_m_employees_employee_guid",
                table: "tb_m_companies",
                column: "employee_guid",
                principalTable: "tb_m_employees",
                principalColumn: "guid",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_m_companies_tb_m_employees_employee_guid",
                table: "tb_m_companies");

            migrationBuilder.DropTable(
                name: "tb_m_accounts_roles");

            migrationBuilder.DropTable(
                name: "tb_m_experiences_skills");

            migrationBuilder.DropTable(
                name: "tb_m_ratings");

            migrationBuilder.DropTable(
                name: "tb_m_salaries");

            migrationBuilder.DropTable(
                name: "tb_m_test");

            migrationBuilder.DropTable(
                name: "tb_m_accounts");

            migrationBuilder.DropTable(
                name: "tb_m_roles");

            migrationBuilder.DropTable(
                name: "tb_m_experiences");

            migrationBuilder.DropTable(
                name: "tb_m_skills");

            migrationBuilder.DropTable(
                name: "tb_m_employees");

            migrationBuilder.DropTable(
                name: "tb_m_companies");
        }
    }
}
