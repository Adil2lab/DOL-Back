using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DOL.Migrations
{
    /// <inheritdoc />
    public partial class AddSomeFeatureToCards : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "CreditCards",
                type: "character varying(25)",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "Issuer",
                table: "CreditCards",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastTransactionedAt",
                table: "CreditCards",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "PublicId",
                table: "CreditCards",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "PublicCardLobby",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Hashed = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicCardLobby", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CreditCards_PublicId",
                table: "CreditCards",
                column: "PublicId");

            migrationBuilder.AddForeignKey(
                name: "FK_CreditCards_PublicCardLobby_PublicId",
                table: "CreditCards",
                column: "PublicId",
                principalTable: "PublicCardLobby",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CreditCards_PublicCardLobby_PublicId",
                table: "CreditCards");

            migrationBuilder.DropTable(
                name: "PublicCardLobby");

            migrationBuilder.DropIndex(
                name: "IX_CreditCards_PublicId",
                table: "CreditCards");

            migrationBuilder.DropColumn(
                name: "Issuer",
                table: "CreditCards");

            migrationBuilder.DropColumn(
                name: "LastTransactionedAt",
                table: "CreditCards");

            migrationBuilder.DropColumn(
                name: "PublicId",
                table: "CreditCards");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "CreditCards",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(25)",
                oldMaxLength: 25);
        }
    }
}
