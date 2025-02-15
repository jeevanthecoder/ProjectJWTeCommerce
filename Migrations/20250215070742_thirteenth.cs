using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectJWTeCommerce.Migrations
{
    /// <inheritdoc />
    public partial class thirteenth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cart",
                columns: table => new
                {
                    CId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cart", x => x.CId);
                });

            migrationBuilder.CreateTable(
                name: "sellers",
                columns: table => new
                {
                    SId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SellerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SellerPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SellerEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoOfProducts = table.Column<int>(type: "int", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sellers", x => x.SId);
                });

            migrationBuilder.CreateTable(
                name: "UserDetails",
                columns: table => new
                {
                    UId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserEmail = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sellerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDetails", x => x.UId);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Pid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SId = table.Column<int>(type: "int", nullable: false),
                    PTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PCategory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PImageURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_Product_sellers_SId",
                        column: x => x.SId,
                        principalTable: "sellers",
                        principalColumn: "SId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    AId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pincode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Landmark = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HouseDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StreetDetails = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.AId);
                    table.ForeignKey(
                        name: "FK_Address_UserDetails_userId",
                        column: x => x.userId,
                        principalTable: "UserDetails",
                        principalColumn: "UId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Features",
                columns: table => new
                {
                    FId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PId = table.Column<int>(type: "int", nullable: false),
                    FName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Features", x => x.FId);
                    table.ForeignKey(
                        name: "FK_Features_Product_PId",
                        column: x => x.PId,
                        principalTable: "Product",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemQuantity",
                columns: table => new
                {
                    QId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CId = table.Column<int>(type: "int", nullable: true),
                    PId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemQuantity", x => x.QId);
                    table.ForeignKey(
                        name: "FK_ItemQuantity_Cart_CId",
                        column: x => x.CId,
                        principalTable: "Cart",
                        principalColumn: "CId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemQuantity_Product_PId",
                        column: x => x.PId,
                        principalTable: "Product",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Address_userId",
                table: "Address",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Features_PId",
                table: "Features",
                column: "PId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemQuantity_CId",
                table: "ItemQuantity",
                column: "CId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemQuantity_PId",
                table: "ItemQuantity",
                column: "PId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_SId",
                table: "Product",
                column: "SId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDetails_UserEmail",
                table: "UserDetails",
                column: "UserEmail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserDetails_UserName",
                table: "UserDetails",
                column: "UserName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "Features");

            migrationBuilder.DropTable(
                name: "ItemQuantity");

            migrationBuilder.DropTable(
                name: "UserDetails");

            migrationBuilder.DropTable(
                name: "Cart");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "sellers");
        }
    }
}
