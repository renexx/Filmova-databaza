using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FilmDat.DAL.Migrations
{
    public partial class UpdatedSeed2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: new Guid("7d71a9ec-8633-4a42-b929-194863c2fe9d"),
                column: "Rating",
                value: 69L);

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] {"Id", "Date", "FilmId", "NickName", "Rating", "TextReview"},
                values: new object[]
                {
                    new Guid("54caf507-8046-4587-a93d-9f1bf5cf1f91"),
                    new DateTime(2002, 7, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    new Guid("16d3e5e1-a52a-4fbc-ac16-305491fe0b8e"), "Brano", 99L, "Gargantua je riadne delo."
                });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] {"Id", "Date", "FilmId", "NickName", "Rating", "TextReview"},
                values: new object[]
                {
                    new Guid("0ec5146a-6564-4285-ae12-b7b5621ab852"),
                    new DateTime(1999, 8, 5, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    new Guid("16d3e5e1-a52a-4fbc-ac16-305491fe0b8e"), "Jozef", 74L, "To bol mindfuck."
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: new Guid("0ec5146a-6564-4285-ae12-b7b5621ab852"));

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: new Guid("54caf507-8046-4587-a93d-9f1bf5cf1f91"));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: new Guid("7d71a9ec-8633-4a42-b929-194863c2fe9d"),
                column: "Rating",
                value: 82L);
        }
    }
}