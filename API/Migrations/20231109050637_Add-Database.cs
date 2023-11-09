using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class AddDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "tb_m_accounts",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    otp = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    is_used = table.Column<bool>(type: "bit", nullable: false),
                    expired_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    role_guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    modified_date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_accounts", x => x.guid);
                    table.ForeignKey(
                        name: "FK_tb_m_accounts_tb_m_roles_role_guid",
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
                    foto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    phone_number = table.Column<string>(type: "nvarchar(16)", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    grade = table.Column<int>(type: "int", nullable: true),
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
                name: "tb_m_interviews",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    type = table.Column<int>(type: "int", nullable: true),
                    remarks = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status_intervew = table.Column<int>(type: "int", nullable: true),
                    start_contract = table.Column<DateTime>(type: "datetime2", nullable: true),
                    end_contract = table.Column<DateTime>(type: "datetime2", nullable: true),
                    employee_guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    owner_guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    modified_date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_interviews", x => x.guid);
                    table.ForeignKey(
                        name: "FK_tb_m_interviews_tb_m_employees_employee_guid",
                        column: x => x.employee_guid,
                        principalTable: "tb_m_employees",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tb_tr_cv",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    cv = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    modified_date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_tr_cv", x => x.guid);
                    table.ForeignKey(
                        name: "FK_tb_tr_cv_tb_m_employees_guid",
                        column: x => x.guid,
                        principalTable: "tb_m_employees",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tb_m_ratings",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    rate = table.Column<int>(type: "int", nullable: true),
                    feedback = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    modified_date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_ratings", x => x.guid);
                    table.ForeignKey(
                        name: "FK_tb_m_ratings_tb_m_interviews_guid",
                        column: x => x.guid,
                        principalTable: "tb_m_interviews",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tb_m_skills",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    cv_guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    modified_date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_skills", x => x.guid);
                    table.ForeignKey(
                        name: "FK_tb_m_skills_tb_tr_cv_cv_guid",
                        column: x => x.cv_guid,
                        principalTable: "tb_tr_cv",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "tb_m_employees",
                columns: new[] { "guid", "company_id", "created_date", "email", "first_name", "foto", "gender", "grade", "last_name", "modified_date", "phone_number", "status" },
                values: new object[] { new Guid("d29cb2be-6c0e-4c9b-b12c-6c2b95aca1d0"), null, null, "admin@mail.com", "Admin", null, 1, null, "One", null, "00000000000", 3 });

            migrationBuilder.InsertData(
                table: "tb_m_roles",
                columns: new[] { "guid", "created_date", "modified_date", "name" },
                values: new object[,]
                {
                    { new Guid("0111002e-1a28-4dea-880e-0b5333128804"), null, null, "idle" },
                    { new Guid("776e06de-3098-4b97-ac8c-59654f6103fd"), null, null, "client" },
                    { new Guid("e468ce6f-5348-415f-b98b-cfb6e7adcc7a"), null, null, "admin" }
                });

            migrationBuilder.InsertData(
                table: "tb_m_accounts",
                columns: new[] { "guid", "created_date", "expired_time", "is_used", "modified_date", "otp", "password", "role_guid", "status" },
                values: new object[] { new Guid("d29cb2be-6c0e-4c9b-b12c-6c2b95aca1d0"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, 0, "$2a$12$HPqiEAputH12BYl8dpoPAOBVt5YmXF5x0dQi7JrLH.wJ8hEg5qp0G", new Guid("e468ce6f-5348-415f-b98b-cfb6e7adcc7a"), 1 });

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_accounts_role_guid",
                table: "tb_m_accounts",
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
                name: "IX_tb_m_interviews_employee_guid",
                table: "tb_m_interviews",
                column: "employee_guid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_skills_cv_guid",
                table: "tb_m_skills",
                column: "cv_guid");

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
                name: "tb_m_accounts");

            migrationBuilder.DropTable(
                name: "tb_m_ratings");

            migrationBuilder.DropTable(
                name: "tb_m_skills");

            migrationBuilder.DropTable(
                name: "tb_m_roles");

            migrationBuilder.DropTable(
                name: "tb_m_interviews");

            migrationBuilder.DropTable(
                name: "tb_tr_cv");

            migrationBuilder.DropTable(
                name: "tb_m_employees");

            migrationBuilder.DropTable(
                name: "tb_m_companies");
        }
    }
}
