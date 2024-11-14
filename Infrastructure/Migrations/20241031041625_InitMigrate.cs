using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitMigrate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductionLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionLines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    SerialLength = table.Column<int>(type: "INTEGER", nullable: false),
                    Gtin = table.Column<ulong>(type: "integer", nullable: false),
                    GtinGroup = table.Column<ulong>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.UniqueConstraint("AK_Products_Gtin", x => x.Gtin);
                });

            migrationBuilder.CreateTable(
                name: "ProductionLinesProducts",
                columns: table => new
                {
                    ProductionLinesId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionLinesProducts", x => new { x.ProductionLinesId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_ProductionLinesProducts_ProductionLines_ProductionLinesId",
                        column: x => x.ProductionLinesId,
                        principalTable: "ProductionLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductionLinesProducts_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    SessionType = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductionLineId = table.Column<int>(type: "INTEGER", nullable: false),
                    State = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductionDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Closed = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sessions_ProductionLines_ProductionLineId",
                        column: x => x.ProductionLineId,
                        principalTable: "ProductionLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Sessions_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MarkingCodes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    Code = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false),
                    SessionId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CodeType = table.Column<int>(type: "INTEGER", nullable: false),
                    ParentId = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarkingCodes", x => x.Id);
                    table.UniqueConstraint("AK_MarkingCodes_Code", x => x.Code);
                    table.ForeignKey(
                        name: "FK_MarkingCodes_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MarkingCodesHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    MarkingCodeId = table.Column<long>(type: "INTEGER", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarkingCodesHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarkingCodesHistories_MarkingCodes_MarkingCodeId",
                        column: x => x.MarkingCodeId,
                        principalTable: "MarkingCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MarkingCodes_SessionId",
                table: "MarkingCodes",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_MarkingCodesHistories_MarkingCodeId",
                table: "MarkingCodesHistories",
                column: "MarkingCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionLinesProducts_ProductsId",
                table: "ProductionLinesProducts",
                column: "ProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_ProductId",
                table: "Sessions",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_ProductionLineId",
                table: "Sessions",
                column: "ProductionLineId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MarkingCodesHistories");

            migrationBuilder.DropTable(
                name: "ProductionLinesProducts");

            migrationBuilder.DropTable(
                name: "MarkingCodes");

            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropTable(
                name: "ProductionLines");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
