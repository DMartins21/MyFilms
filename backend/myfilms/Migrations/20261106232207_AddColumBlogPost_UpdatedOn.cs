using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace myfilms.Migrations
{
    /// <inheritdoc />
    public partial class AddColumBlogPost_UpdatedOn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "UpdatedOn",
                table: "BlogPosts",
                type: "date",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "BlogPosts");
        }
    }
}
