using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DOL.Migrations
{
    /// <inheritdoc />
    public partial class AddWebAuthnCred : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CreditCards_PublicCardLobby_PublicId",
                table: "CreditCards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PublicCardLobby",
                table: "PublicCardLobby");

            migrationBuilder.RenameTable(
                name: "PublicCardLobby",
                newName: "PublicCards");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PublicCards",
                table: "PublicCards",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "WebAuthnCreds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CredentialId = table.Column<byte[]>(type: "bytea", nullable: false),
                    PublicKey = table.Column<byte[]>(type: "bytea", nullable: false),
                    SignatureCounter = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebAuthnCreds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebAuthnCreds_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WebAuthnCreds_UserId",
                table: "WebAuthnCreds",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CreditCards_PublicCards_PublicId",
                table: "CreditCards",
                column: "PublicId",
                principalTable: "PublicCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CreditCards_PublicCards_PublicId",
                table: "CreditCards");

            migrationBuilder.DropTable(
                name: "WebAuthnCreds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PublicCards",
                table: "PublicCards");

            migrationBuilder.RenameTable(
                name: "PublicCards",
                newName: "PublicCardLobby");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PublicCardLobby",
                table: "PublicCardLobby",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CreditCards_PublicCardLobby_PublicId",
                table: "CreditCards",
                column: "PublicId",
                principalTable: "PublicCardLobby",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
