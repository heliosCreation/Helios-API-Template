using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Template.Identity.Migrations
{
    public partial class Added_RefreshToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Token = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    JwtId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    Invalidated = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Token);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "01B168FE-810B-432D-9010-233BA0B380E9",
                column: "ConcurrencyStamp",
                value: "8bf86748-b1b2-46a6-b107-cf6edb30842c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2301D884-221A-4E7D-B509-0113DCC043E1",
                column: "ConcurrencyStamp",
                value: "49e18086-b9d0-4040-90ba-d6d44dc73937");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "78A7570F-3CE5-48BA-9461-80283ED1D94D",
                column: "ConcurrencyStamp",
                value: "1fb642a1-a688-440b-99d8-6317eda92ff3");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7D9B7113-A8F8-4035-99A7-A20DD400F6A3",
                column: "ConcurrencyStamp",
                value: "f8e0389e-9b6c-47f7-af9f-179f1864c2e5");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9f93e84d-bcfe-4dab-9ba7-cb7065a63524",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "94fd9e97-c6b4-407d-9984-3d1a258d6f54", "AQAAAAEAACcQAAAAEDAwWdB3CfwXL4D2leEwiyTLDgU+iagsaCP2vhZh4iKpRQf4Bipt+qFLVh+JFOZK6Q==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f1aafc30-5e54-4550-a5a1-4df0704b3258",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "39e48427-d81e-4686-887f-f05fd08c73e7", "AQAAAAEAACcQAAAAEOKTb+NyPdJcvZPYg4pgqZDcG+2pLdxw/1IU4sJPSdM57GjBkPhy1PyJkeXAOzlGKw==" });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "01B168FE-810B-432D-9010-233BA0B380E9",
                column: "ConcurrencyStamp",
                value: "db1ade5e-4484-4229-9bd9-793a33fbf83f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2301D884-221A-4E7D-B509-0113DCC043E1",
                column: "ConcurrencyStamp",
                value: "38fffb80-161e-4397-9580-cc901f63b4fa");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "78A7570F-3CE5-48BA-9461-80283ED1D94D",
                column: "ConcurrencyStamp",
                value: "6017d569-5b63-44db-b323-b148e01ae290");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7D9B7113-A8F8-4035-99A7-A20DD400F6A3",
                column: "ConcurrencyStamp",
                value: "b1c5ab12-0d91-4d14-b8c6-a82d770a6f07");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9f93e84d-bcfe-4dab-9ba7-cb7065a63524",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "55ec46e4-b7ea-41ca-a396-7eed9d3f94aa", "AQAAAAEAACcQAAAAEM0sZ8QfVK/RCAkKfnjcIL8M0G/Pgg3sR04l+pz0MobuQ/9Ax19iVrggIaOaSyFuBA==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f1aafc30-5e54-4550-a5a1-4df0704b3258",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "957d6332-ed84-4c1d-9803-f1475a0e4c71", "AQAAAAEAACcQAAAAELNyBgbYTrDn7e+iqE+rq7mC/b5l+XJ+DcsLET1XX96b1Qg2gUJDJ5qT2vqa7S+UPg==" });
        }
    }
}
