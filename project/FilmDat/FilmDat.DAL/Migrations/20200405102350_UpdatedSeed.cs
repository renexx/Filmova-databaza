using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FilmDat.DAL.Migrations
{
    public partial class UpdatedSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] {"Id", "Date", "FilmId", "NickName", "Rating", "TextReview"},
                values: new object[]
                {
                    new Guid("7d71a9ec-8633-4a42-b929-194863c2fe9d"),
                    new DateTime(2003, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    new Guid("088e40b8-63f6-4089-bfa9-4146e36e888c"), "Branimir", 82L, "John je skvely."
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: new Guid("7d71a9ec-8633-4a42-b929-194863c2fe9d"));
        }
    }
}