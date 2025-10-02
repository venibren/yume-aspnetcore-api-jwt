using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yume.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:citext", ",,")
                .Annotation("Npgsql:PostgresExtension:pgcrypto", ",,");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Email = table.Column<string>(type: "citext", maxLength: 256, nullable: false),
                    EmailVerified = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Username = table.Column<string>(type: "citext", maxLength: 32, nullable: false),
                    Nickname = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    Discriminator = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValue: new DateTimeOffset(new DateTime(2025, 10, 1, 6, 30, 58, 206, DateTimeKind.Unspecified).AddTicks(6444), new TimeSpan(0, 0, 0, 0, 0))),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValue: new DateTimeOffset(new DateTime(2025, 10, 1, 6, 30, 58, 206, DateTimeKind.Unspecified).AddTicks(6681), new TimeSpan(0, 0, 0, 0, 0))),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuthUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    MfaEmailEnabled = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    MfaTotpEnabled = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    MfaBackupEnabled = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuthUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserCustomizations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Avatar = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    Banner = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    Description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    Theme = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCustomizations", x => x.Id);
                    table.ForeignKey(
                        name: "fk_user_customization_user_id",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuthUserHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Timestamp = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValue: new DateTimeOffset(new DateTime(2025, 10, 1, 6, 30, 58, 204, DateTimeKind.Unspecified).AddTicks(2523), new TimeSpan(0, 0, 0, 0, 0))),
                    Success = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Reason = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    IpAddress = table.Column<string>(type: "text", nullable: false),
                    Agent = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthUserHistory", x => x.Id);
                    table.ForeignKey(
                        name: "fk_auth_user_history_user_id",
                        column: x => x.UserId,
                        principalTable: "AuthUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AuthUserMfaBackups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CodeHash = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValue: new DateTimeOffset(new DateTime(2025, 10, 1, 6, 30, 58, 204, DateTimeKind.Unspecified).AddTicks(6265), new TimeSpan(0, 0, 0, 0, 0))),
                    ActivatedDate = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthUserMfaBackups", x => x.Id);
                    table.ForeignKey(
                        name: "fk_auth_user_mfabackup_user_id",
                        column: x => x.UserId,
                        principalTable: "AuthUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuthUserMfaTotps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    SecretHash = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValue: new DateTimeOffset(new DateTime(2025, 10, 1, 6, 30, 58, 204, DateTimeKind.Unspecified).AddTicks(9486), new TimeSpan(0, 0, 0, 0, 0)))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthUserMfaTotps", x => x.Id);
                    table.ForeignKey(
                        name: "fk_auth_user_mfatotp_user_id",
                        column: x => x.UserId,
                        principalTable: "AuthUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthUserHistory_UserId",
                table: "AuthUserHistory",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthUserMfaBackups_UserId",
                table: "AuthUserMfaBackups",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuthUserMfaTotps_UserId",
                table: "AuthUserMfaTotps",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuthUsers_UserId",
                table: "AuthUsers",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserCustomizations_UserId",
                table: "UserCustomizations",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_user_email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_user_username_full",
                table: "Users",
                columns: new[] { "Username", "Discriminator" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthUserHistory");

            migrationBuilder.DropTable(
                name: "AuthUserMfaBackups");

            migrationBuilder.DropTable(
                name: "AuthUserMfaTotps");

            migrationBuilder.DropTable(
                name: "UserCustomizations");

            migrationBuilder.DropTable(
                name: "AuthUsers");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
