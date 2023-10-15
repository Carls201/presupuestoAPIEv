using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class InitDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoriaGastos",
                columns: table => new
                {
                    id_categoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriaGastos", x => x.id_categoria);
                });

            migrationBuilder.CreateTable(
                name: "FuenteIngresos",
                columns: table => new
                {
                    id_fuente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuenteIngresos", x => x.id_fuente);
                });

            migrationBuilder.CreateTable(
                name: "MetaAhorros",
                columns: table => new
                {
                    id_meta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetaAhorros", x => x.id_meta);
                });

            migrationBuilder.CreateTable(
                name: "Rols",
                columns: table => new
                {
                    id_rol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    rol = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rols", x => x.id_rol);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    id_usuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_rol = table.Column<int>(type: "int", nullable: false),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    apellido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    edad = table.Column<int>(type: "int", nullable: false),
                    direccion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.id_usuario);
                    table.ForeignKey(
                        name: "FK_Usuarios_Rols_id_rol",
                        column: x => x.id_rol,
                        principalTable: "Rols",
                        principalColumn: "id_rol",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ahorros",
                columns: table => new
                {
                    id_ahorro = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_usuario = table.Column<int>(type: "int", nullable: false),
                    id_meta = table.Column<int>(type: "int", nullable: false),
                    monto = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ahorros", x => x.id_ahorro);
                    table.ForeignKey(
                        name: "FK_Ahorros_MetaAhorros_id_meta",
                        column: x => x.id_meta,
                        principalTable: "MetaAhorros",
                        principalColumn: "id_meta",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ahorros_Usuarios_id_usuario",
                        column: x => x.id_usuario,
                        principalTable: "Usuarios",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Gastos",
                columns: table => new
                {
                    id_gasto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_usuario = table.Column<int>(type: "int", nullable: false),
                    id_categoria = table.Column<int>(type: "int", nullable: false),
                    monto = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gastos", x => x.id_gasto);
                    table.ForeignKey(
                        name: "FK_Gastos_CategoriaGastos_id_categoria",
                        column: x => x.id_categoria,
                        principalTable: "CategoriaGastos",
                        principalColumn: "id_categoria",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Gastos_Usuarios_id_usuario",
                        column: x => x.id_usuario,
                        principalTable: "Usuarios",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ingresos",
                columns: table => new
                {
                    id_ingreso = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_usuario = table.Column<int>(type: "int", nullable: false),
                    id_fuente = table.Column<int>(type: "int", nullable: false),
                    monto = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingresos", x => x.id_ingreso);
                    table.ForeignKey(
                        name: "FK_Ingresos_FuenteIngresos_id_fuente",
                        column: x => x.id_fuente,
                        principalTable: "FuenteIngresos",
                        principalColumn: "id_fuente",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ingresos_Usuarios_id_usuario",
                        column: x => x.id_usuario,
                        principalTable: "Usuarios",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ahorros_id_meta",
                table: "Ahorros",
                column: "id_meta");

            migrationBuilder.CreateIndex(
                name: "IX_Ahorros_id_usuario",
                table: "Ahorros",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Gastos_id_categoria",
                table: "Gastos",
                column: "id_categoria");

            migrationBuilder.CreateIndex(
                name: "IX_Gastos_id_usuario",
                table: "Gastos",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Ingresos_id_fuente",
                table: "Ingresos",
                column: "id_fuente");

            migrationBuilder.CreateIndex(
                name: "IX_Ingresos_id_usuario",
                table: "Ingresos",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_id_rol",
                table: "Usuarios",
                column: "id_rol");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ahorros");

            migrationBuilder.DropTable(
                name: "Gastos");

            migrationBuilder.DropTable(
                name: "Ingresos");

            migrationBuilder.DropTable(
                name: "MetaAhorros");

            migrationBuilder.DropTable(
                name: "CategoriaGastos");

            migrationBuilder.DropTable(
                name: "FuenteIngresos");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Rols");
        }
    }
}
