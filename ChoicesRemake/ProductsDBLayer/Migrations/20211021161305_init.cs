using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductsDBLayer.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    cat_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    value = table.Column<string>(type: "varchar(512)", unicode: false, maxLength: 512, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_38", x => x.cat_id);
                });

            migrationBuilder.CreateTable(
                name: "Color",
                columns: table => new
                {
                    color_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    value = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Color", x => x.color_id);
                });

            migrationBuilder.CreateTable(
                name: "Description",
                columns: table => new
                {
                    desc_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    long_description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_27", x => x.desc_id);
                });

            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    image_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    location = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    mini_desc = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.image_id);
                });

            migrationBuilder.CreateTable(
                name: "Mass",
                columns: table => new
                {
                    mass_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    massInKg = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mass", x => x.mass_id);
                });

            migrationBuilder.CreateTable(
                name: "Misc_Detail",
                columns: table => new
                {
                    detail_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    key = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    value = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_42", x => x.detail_id);
                });

            migrationBuilder.CreateTable(
                name: "Size",
                columns: table => new
                {
                    size_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WidthInMM = table.Column<double>(type: "float", nullable: true),
                    LengthInMM = table.Column<double>(type: "float", nullable: true),
                    HeightInMM = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Size", x => x.size_id);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    prod_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    image_id = table.Column<long>(type: "bigint", nullable: false),
                    size_id = table.Column<long>(type: "bigint", nullable: false),
                    cat_id = table.Column<long>(type: "bigint", nullable: false),
                    color_id = table.Column<long>(type: "bigint", nullable: false),
                    mass_id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    brand = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    designer = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    price = table.Column<decimal>(type: "money", nullable: false),
                    desc_id = table.Column<long>(type: "bigint", nullable: false),
                    detail_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_69", x => new { x.prod_id, x.image_id, x.size_id, x.cat_id, x.color_id, x.mass_id });
                    table.ForeignKey(
                        name: "FK_48",
                        column: x => x.image_id,
                        principalTable: "Image",
                        principalColumn: "image_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_51",
                        column: x => x.desc_id,
                        principalTable: "Description",
                        principalColumn: "desc_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_54",
                        column: x => x.size_id,
                        principalTable: "Size",
                        principalColumn: "size_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_57",
                        column: x => x.cat_id,
                        principalTable: "Category",
                        principalColumn: "cat_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_60",
                        column: x => x.mass_id,
                        principalTable: "Mass",
                        principalColumn: "mass_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_63",
                        column: x => x.color_id,
                        principalTable: "Color",
                        principalColumn: "color_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_66",
                        column: x => x.detail_id,
                        principalTable: "Misc_Detail",
                        principalColumn: "detail_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "fkIdx_50",
                table: "Product",
                column: "image_id");

            migrationBuilder.CreateIndex(
                name: "fkIdx_53",
                table: "Product",
                column: "desc_id");

            migrationBuilder.CreateIndex(
                name: "fkIdx_56",
                table: "Product",
                column: "size_id");

            migrationBuilder.CreateIndex(
                name: "fkIdx_59",
                table: "Product",
                column: "cat_id");

            migrationBuilder.CreateIndex(
                name: "fkIdx_62",
                table: "Product",
                column: "mass_id");

            migrationBuilder.CreateIndex(
                name: "fkIdx_65",
                table: "Product",
                column: "color_id");

            migrationBuilder.CreateIndex(
                name: "fkIdx_68",
                table: "Product",
                column: "detail_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.DropTable(
                name: "Description");

            migrationBuilder.DropTable(
                name: "Size");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Mass");

            migrationBuilder.DropTable(
                name: "Color");

            migrationBuilder.DropTable(
                name: "Misc_Detail");
        }
    }
}
