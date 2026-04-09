using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BolsaEmpleos.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialPostgres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "empresas",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    razon_social = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    numero_identificacion = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    correo_electronico = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    contrasena_hash = table.Column<string>(type: "text", nullable: false),
                    telefono = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    sitio_web = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fecha_modificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_empresas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "habilidades",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    categoria = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fecha_modificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_habilidades", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "jovenes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    apellido = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    correo_electronico = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    contrasena_hash = table.Column<string>(type: "text", nullable: false),
                    telefono = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    fecha_nacimiento = table.Column<DateOnly>(type: "date", nullable: false),
                    nivel_educativo = table.Column<int>(type: "integer", nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fecha_modificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_jovenes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ofertas_trabajo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    empresa_id = table.Column<int>(type: "integer", nullable: false),
                    titulo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    descripcion = table.Column<string>(type: "text", nullable: false),
                    ubicacion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    salario = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: true),
                    estado = table.Column<int>(type: "integer", nullable: false),
                    fecha_cierre = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fecha_modificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ofertas_trabajo", x => x.id);
                    table.ForeignKey(
                        name: "FK_ofertas_trabajo_empresas_empresa_id",
                        column: x => x.empresa_id,
                        principalTable: "empresas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cursos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    titulo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    descripcion = table.Column<string>(type: "text", nullable: false),
                    habilidad_id = table.Column<int>(type: "integer", nullable: false),
                    duracion_horas = table.Column<decimal>(type: "numeric(4,1)", precision: 4, scale: 1, nullable: false),
                    url_material = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    puntaje_minimo_aprobacion = table.Column<int>(type: "integer", nullable: false, defaultValue: 70),
                    fecha_creacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fecha_modificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cursos", x => x.id);
                    table.ForeignKey(
                        name: "FK_cursos_habilidades_habilidad_id",
                        column: x => x.habilidad_id,
                        principalTable: "habilidades",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "curricula",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    joven_id = table.Column<int>(type: "integer", nullable: false),
                    resumen_profesional = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    titulo_profesional = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    url_portfolio = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fecha_modificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_curricula", x => x.id);
                    table.ForeignKey(
                        name: "FK_curricula_jovenes_joven_id",
                        column: x => x.joven_id,
                        principalTable: "jovenes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "postulaciones",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    joven_id = table.Column<int>(type: "integer", nullable: false),
                    oferta_trabajo_id = table.Column<int>(type: "integer", nullable: false),
                    estado = table.Column<int>(type: "integer", nullable: false),
                    fecha_postulacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fecha_modificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_postulaciones", x => x.id);
                    table.ForeignKey(
                        name: "FK_postulaciones_jovenes_joven_id",
                        column: x => x.joven_id,
                        principalTable: "jovenes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_postulaciones_ofertas_trabajo_oferta_trabajo_id",
                        column: x => x.oferta_trabajo_id,
                        principalTable: "ofertas_trabajo",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "requisitos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    oferta_trabajo_id = table.Column<int>(type: "integer", nullable: false),
                    habilidad_id = table.Column<int>(type: "integer", nullable: false),
                    tipo_requisito = table.Column<int>(type: "integer", nullable: false),
                    descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fecha_modificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_requisitos", x => x.id);
                    table.ForeignKey(
                        name: "FK_requisitos_habilidades_habilidad_id",
                        column: x => x.habilidad_id,
                        principalTable: "habilidades",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_requisitos_ofertas_trabajo_oferta_trabajo_id",
                        column: x => x.oferta_trabajo_id,
                        principalTable: "ofertas_trabajo",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "evaluaciones",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    joven_id = table.Column<int>(type: "integer", nullable: false),
                    curso_id = table.Column<int>(type: "integer", nullable: false),
                    puntaje_obtenido = table.Column<int>(type: "integer", nullable: true),
                    estado = table.Column<int>(type: "integer", nullable: false),
                    fecha_inicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    fecha_fin = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    intentos = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    fecha_creacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fecha_modificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_evaluaciones", x => x.id);
                    table.ForeignKey(
                        name: "FK_evaluaciones_cursos_curso_id",
                        column: x => x.curso_id,
                        principalTable: "cursos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_evaluaciones_jovenes_joven_id",
                        column: x => x.joven_id,
                        principalTable: "jovenes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "curriculum_habilidades",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    curriculum_id = table.Column<int>(type: "integer", nullable: false),
                    habilidad_id = table.Column<int>(type: "integer", nullable: false),
                    obtenida_por_curso = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    fecha_agregado = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fecha_modificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_curriculum_habilidades", x => x.id);
                    table.ForeignKey(
                        name: "FK_curriculum_habilidades_curricula_curriculum_id",
                        column: x => x.curriculum_id,
                        principalTable: "curricula",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_curriculum_habilidades_habilidades_habilidad_id",
                        column: x => x.habilidad_id,
                        principalTable: "habilidades",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_curricula_joven_id",
                table: "curricula",
                column: "joven_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_curriculum_habilidades_curriculum_id_habilidad_id",
                table: "curriculum_habilidades",
                columns: new[] { "curriculum_id", "habilidad_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_curriculum_habilidades_habilidad_id",
                table: "curriculum_habilidades",
                column: "habilidad_id");

            migrationBuilder.CreateIndex(
                name: "IX_cursos_habilidad_id",
                table: "cursos",
                column: "habilidad_id");

            migrationBuilder.CreateIndex(
                name: "IX_empresas_correo_electronico",
                table: "empresas",
                column: "correo_electronico",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_empresas_numero_identificacion",
                table: "empresas",
                column: "numero_identificacion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_evaluaciones_curso_id",
                table: "evaluaciones",
                column: "curso_id");

            migrationBuilder.CreateIndex(
                name: "IX_evaluaciones_joven_id",
                table: "evaluaciones",
                column: "joven_id");

            migrationBuilder.CreateIndex(
                name: "IX_habilidades_nombre",
                table: "habilidades",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_jovenes_correo_electronico",
                table: "jovenes",
                column: "correo_electronico",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ofertas_trabajo_empresa_id",
                table: "ofertas_trabajo",
                column: "empresa_id");

            migrationBuilder.CreateIndex(
                name: "IX_postulaciones_joven_id_oferta_trabajo_id",
                table: "postulaciones",
                columns: new[] { "joven_id", "oferta_trabajo_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_postulaciones_oferta_trabajo_id",
                table: "postulaciones",
                column: "oferta_trabajo_id");

            migrationBuilder.CreateIndex(
                name: "IX_requisitos_habilidad_id",
                table: "requisitos",
                column: "habilidad_id");

            migrationBuilder.CreateIndex(
                name: "IX_requisitos_oferta_trabajo_id",
                table: "requisitos",
                column: "oferta_trabajo_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "curriculum_habilidades");

            migrationBuilder.DropTable(
                name: "evaluaciones");

            migrationBuilder.DropTable(
                name: "postulaciones");

            migrationBuilder.DropTable(
                name: "requisitos");

            migrationBuilder.DropTable(
                name: "curricula");

            migrationBuilder.DropTable(
                name: "cursos");

            migrationBuilder.DropTable(
                name: "ofertas_trabajo");

            migrationBuilder.DropTable(
                name: "jovenes");

            migrationBuilder.DropTable(
                name: "habilidades");

            migrationBuilder.DropTable(
                name: "empresas");
        }
    }
}
