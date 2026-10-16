using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Hafi_Abi_Doğum_Günü.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    QuestionText = table.Column<string>(type: "text", nullable: false),
                    OptionA = table.Column<string>(type: "text", nullable: false),
                    OptionB = table.Column<string>(type: "text", nullable: false),
                    OptionC = table.Column<string>(type: "text", nullable: false),
                    OptionD = table.Column<string>(type: "text", nullable: false),
                    CorrectOption = table.Column<string>(type: "text", nullable: false),
                    OrderIndex = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuizResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlayerName = table.Column<string>(type: "text", nullable: false),
                    PlayerTitle = table.Column<string>(type: "text", nullable: false),
                    Score = table.Column<int>(type: "integer", nullable: false),
                    PlayedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizResults", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rsvps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GuestName = table.Column<string>(type: "text", nullable: false),
                    IsAttending = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rsvps", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CorrectOption", "OptionA", "OptionB", "OptionC", "OptionD", "OrderIndex", "QuestionText" },
                values: new object[,]
                {
                    { 1, "A", "Bol Tereyağlı İskender", "Anne Mantısı", "Acılı Adana Kebap", "Gece Yarısı Kokoreçi", 1, "Ahmet Hafi abi en çok hangi yemeği yerken 'İşte hayat bu!' der?" },
                    { 2, "B", "Trafik kilitlenmiş abi", "5 dakikaya oradayım (Daha evden çıkmadı)", "Alarm çalmamış ya", "Arabanın anahtarını bulamadım", 2, "Buluşmalara geç kaldığında Hafi abinin en klasik bahanesi nedir?" },
                    { 3, "A", "Hallederiz sıkıntı yok", "Bak şimdi sana ne anlatacağım", "O iş öyle olmaz", "Çay söyleyin de içelim", 3, "Ahmet Hafi abinin ortamda en çok kullandığı favori kelimesi/repliği hangisidir?" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "QuizResults");

            migrationBuilder.DropTable(
                name: "Rsvps");
        }
    }
}
