using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BigDataAcademy.Model.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "claim_hub_claim",
                columns: table => new
                {
                    claim_id = table.Column<long>(type: "bigint", nullable: false),
                    ingestion_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    product_line = table.Column<string>(type: "text", nullable: true),
                    state = table.Column<string>(type: "text", nullable: true),
                    policy_number = table.Column<string>(type: "text", nullable: true),
                    loss_date = table.Column<DateOnly>(type: "date", nullable: true),
                    create_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    claim_broker = table.Column<string>(type: "text", nullable: true),
                    claim_tier = table.Column<string>(type: "text", nullable: true),
                    currency = table.Column<string>(type: "text", nullable: true),
                    fault_rating = table.Column<string>(type: "text", nullable: true),
                    how_reported = table.Column<string>(type: "text", nullable: true),
                    loss_cause = table.Column<string>(type: "text", nullable: true),
                    loss_type = table.Column<string>(type: "text", nullable: true),
                    situation = table.Column<string>(type: "text", nullable: true),
                    policy_source_system = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_claim_hub_claim", x => x.claim_id);
                });

            migrationBuilder.CreateTable(
                name: "claim_hub_exposure",
                columns: table => new
                {
                    exposure_id = table.Column<long>(type: "bigint", nullable: false),
                    ingestion_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    claim_id = table.Column<long>(type: "bigint", nullable: false),
                    exposure_type = table.Column<string>(type: "text", nullable: true),
                    total_payments = table.Column<decimal>(type: "numeric", nullable: true),
                    source_id = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_claim_hub_exposure", x => x.exposure_id);
                });

            migrationBuilder.CreateTable(
                name: "claim_hub_motor",
                columns: table => new
                {
                    motor_id = table.Column<long>(type: "bigint", nullable: false),
                    ingestion_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    loss_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    vehicle_registration_number = table.Column<string>(type: "text", nullable: true),
                    year = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    make = table.Column<string>(type: "text", nullable: true),
                    model = table.Column<string>(type: "text", nullable: true),
                    engine_capacity = table.Column<int>(type: "integer", nullable: true),
                    colour = table.Column<string>(type: "text", nullable: true),
                    damage_description = table.Column<string>(type: "text", nullable: true),
                    total_lost_decision = table.Column<string>(type: "text", nullable: true),
                    pre_accident_value = table.Column<decimal>(type: "numeric", nullable: true),
                    repair_cost = table.Column<decimal>(type: "numeric", nullable: true),
                    average_mileage_for_valuation = table.Column<int>(type: "integer", nullable: true),
                    policy_number = table.Column<string>(type: "text", nullable: true),
                    driver_date_of_birth = table.Column<DateOnly>(type: "date", nullable: true),
                    fuel_type = table.Column<string>(type: "text", nullable: true),
                    doors = table.Column<int>(type: "integer", nullable: true),
                    seats = table.Column<int>(type: "integer", nullable: true),
                    source_id = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_claim_hub_motor", x => x.motor_id);
                });

            migrationBuilder.CreateTable(
                name: "claim_pro_claim",
                columns: table => new
                {
                    claim_id = table.Column<long>(type: "bigint", nullable: false),
                    ingestion_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    product_line = table.Column<string>(type: "text", nullable: true),
                    state = table.Column<string>(type: "text", nullable: true),
                    policy_number = table.Column<string>(type: "text", nullable: true),
                    loss_date = table.Column<DateOnly>(type: "date", nullable: true),
                    create_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    claim_broker = table.Column<string>(type: "text", nullable: true),
                    claim_tier = table.Column<string>(type: "text", nullable: true),
                    currency = table.Column<string>(type: "text", nullable: true),
                    fault_rating = table.Column<string>(type: "text", nullable: true),
                    how_reported = table.Column<string>(type: "text", nullable: true),
                    loss_cause = table.Column<string>(type: "text", nullable: true),
                    loss_type = table.Column<string>(type: "text", nullable: true),
                    situation = table.Column<string>(type: "text", nullable: true),
                    policy_source_system = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_claim_pro_claim", x => x.claim_id);
                });

            migrationBuilder.CreateTable(
                name: "claim_pro_exposure",
                columns: table => new
                {
                    exposure_id = table.Column<long>(type: "bigint", nullable: false),
                    ingestion_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    claim_id = table.Column<long>(type: "bigint", nullable: false),
                    exposure_type = table.Column<string>(type: "text", nullable: true),
                    total_payments = table.Column<decimal>(type: "numeric", nullable: true),
                    source_id = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_claim_pro_exposure", x => x.exposure_id);
                });

            migrationBuilder.CreateTable(
                name: "claim_pro_motor",
                columns: table => new
                {
                    motor_id = table.Column<long>(type: "bigint", nullable: false),
                    ingestion_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    loss_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    vehicle_registration_number = table.Column<string>(type: "text", nullable: true),
                    year = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    make = table.Column<string>(type: "text", nullable: true),
                    model = table.Column<string>(type: "text", nullable: true),
                    engine_capacity = table.Column<int>(type: "integer", nullable: true),
                    colour = table.Column<string>(type: "text", nullable: true),
                    damage_description = table.Column<string>(type: "text", nullable: true),
                    total_lost_decision = table.Column<string>(type: "text", nullable: true),
                    pre_accident_value = table.Column<decimal>(type: "numeric", nullable: true),
                    repair_cost = table.Column<decimal>(type: "numeric", nullable: true),
                    average_mileage_for_valuation = table.Column<int>(type: "integer", nullable: true),
                    policy_number = table.Column<string>(type: "text", nullable: true),
                    driver_date_of_birth = table.Column<DateOnly>(type: "date", nullable: true),
                    fuel_type = table.Column<string>(type: "text", nullable: true),
                    doors = table.Column<int>(type: "integer", nullable: true),
                    seats = table.Column<int>(type: "integer", nullable: true),
                    source_id = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_claim_pro_motor", x => x.motor_id);
                });

            migrationBuilder.CreateTable(
                name: "claim_zone_claim",
                columns: table => new
                {
                    claim_id = table.Column<long>(type: "bigint", nullable: false),
                    ingestion_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    product_line = table.Column<string>(type: "text", nullable: true),
                    state = table.Column<string>(type: "text", nullable: true),
                    policy_number = table.Column<string>(type: "text", nullable: true),
                    loss_date = table.Column<DateOnly>(type: "date", nullable: true),
                    create_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    claim_broker = table.Column<string>(type: "text", nullable: true),
                    claim_tier = table.Column<string>(type: "text", nullable: true),
                    currency = table.Column<string>(type: "text", nullable: true),
                    fault_rating = table.Column<string>(type: "text", nullable: true),
                    how_reported = table.Column<string>(type: "text", nullable: true),
                    loss_cause = table.Column<string>(type: "text", nullable: true),
                    loss_type = table.Column<string>(type: "text", nullable: true),
                    situation = table.Column<string>(type: "text", nullable: true),
                    policy_source_system = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_claim_zone_claim", x => x.claim_id);
                });

            migrationBuilder.CreateTable(
                name: "claim_zone_motor",
                columns: table => new
                {
                    motor_id = table.Column<long>(type: "bigint", nullable: false),
                    ingestion_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    loss_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    vehicle_registration_number = table.Column<string>(type: "text", nullable: true),
                    year = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    make = table.Column<string>(type: "text", nullable: true),
                    model = table.Column<string>(type: "text", nullable: true),
                    engine_capacity = table.Column<int>(type: "integer", nullable: true),
                    colour = table.Column<string>(type: "text", nullable: true),
                    damage_description = table.Column<string>(type: "text", nullable: true),
                    total_lost_decision = table.Column<string>(type: "text", nullable: true),
                    pre_accident_value = table.Column<decimal>(type: "numeric", nullable: true),
                    repair_cost = table.Column<decimal>(type: "numeric", nullable: true),
                    average_mileage_for_valuation = table.Column<int>(type: "integer", nullable: true),
                    policy_number = table.Column<string>(type: "text", nullable: true),
                    driver_date_of_birth = table.Column<DateOnly>(type: "date", nullable: true),
                    fuel_type = table.Column<string>(type: "text", nullable: true),
                    doors = table.Column<int>(type: "integer", nullable: true),
                    seats = table.Column<int>(type: "integer", nullable: true),
                    source_id = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_claim_zone_motor", x => x.motor_id);
                });

            migrationBuilder.CreateTable(
                name: "insure_wave_claim",
                columns: table => new
                {
                    claim_id = table.Column<long>(type: "bigint", nullable: false),
                    ingestion_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    product_line = table.Column<string>(type: "text", nullable: true),
                    state = table.Column<string>(type: "text", nullable: true),
                    policy_number = table.Column<string>(type: "text", nullable: true),
                    loss_date = table.Column<DateOnly>(type: "date", nullable: true),
                    create_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    claim_broker = table.Column<string>(type: "text", nullable: true),
                    claim_tier = table.Column<string>(type: "text", nullable: true),
                    currency = table.Column<string>(type: "text", nullable: true),
                    fault_rating = table.Column<string>(type: "text", nullable: true),
                    how_reported = table.Column<string>(type: "text", nullable: true),
                    loss_cause = table.Column<string>(type: "text", nullable: true),
                    loss_type = table.Column<string>(type: "text", nullable: true),
                    situation = table.Column<string>(type: "text", nullable: true),
                    policy_source_system = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_insure_wave_claim", x => x.claim_id);
                });

            migrationBuilder.CreateTable(
                name: "insure_wave_motor",
                columns: table => new
                {
                    motor_id = table.Column<long>(type: "bigint", nullable: false),
                    ingestion_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    loss_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    vehicle_registration_number = table.Column<string>(type: "text", nullable: true),
                    year = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    make = table.Column<string>(type: "text", nullable: true),
                    model = table.Column<string>(type: "text", nullable: true),
                    engine_capacity = table.Column<int>(type: "integer", nullable: true),
                    colour = table.Column<string>(type: "text", nullable: true),
                    damage_description = table.Column<string>(type: "text", nullable: true),
                    total_lost_decision = table.Column<string>(type: "text", nullable: true),
                    pre_accident_value = table.Column<decimal>(type: "numeric", nullable: true),
                    repair_cost = table.Column<decimal>(type: "numeric", nullable: true),
                    average_mileage_for_valuation = table.Column<int>(type: "integer", nullable: true),
                    policy_number = table.Column<string>(type: "text", nullable: true),
                    driver_date_of_birth = table.Column<DateOnly>(type: "date", nullable: true),
                    fuel_type = table.Column<string>(type: "text", nullable: true),
                    doors = table.Column<int>(type: "integer", nullable: true),
                    seats = table.Column<int>(type: "integer", nullable: true),
                    source_id = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_insure_wave_motor", x => x.motor_id);
                });

            migrationBuilder.CreateTable(
                name: "integration_event",
                columns: table => new
                {
                    event_id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    body = table.Column<string>(type: "text", nullable: false),
                    process_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    processed = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_integration_event", x => x.event_id);
                });

            migrationBuilder.CreateTable(
                name: "claim_zone_exposure",
                columns: table => new
                {
                    exposure_id = table.Column<long>(type: "bigint", nullable: false),
                    ingestion_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    claim_id = table.Column<long>(type: "bigint", nullable: false),
                    exposure_type = table.Column<string>(type: "text", nullable: true),
                    total_payments = table.Column<decimal>(type: "numeric", nullable: true),
                    source_id = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_claim_zone_exposure", x => x.exposure_id);
                    table.ForeignKey(
                        name: "FK_claim_zone_exposure_claim_zone_claim_claim_id",
                        column: x => x.claim_id,
                        principalTable: "claim_zone_claim",
                        principalColumn: "claim_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "insure_wave_exposure",
                columns: table => new
                {
                    exposure_id = table.Column<long>(type: "bigint", nullable: false),
                    ingestion_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    claim_id = table.Column<long>(type: "bigint", nullable: false),
                    exposure_type = table.Column<string>(type: "text", nullable: true),
                    total_payments = table.Column<decimal>(type: "numeric", nullable: true),
                    source_id = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_insure_wave_exposure", x => x.exposure_id);
                    table.ForeignKey(
                        name: "FK_insure_wave_exposure_insure_wave_claim_claim_id",
                        column: x => x.claim_id,
                        principalTable: "insure_wave_claim",
                        principalColumn: "claim_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_claim_zone_exposure_claim_id",
                table: "claim_zone_exposure",
                column: "claim_id");

            migrationBuilder.CreateIndex(
                name: "IX_insure_wave_exposure_claim_id",
                table: "insure_wave_exposure",
                column: "claim_id");

            migrationBuilder.Sql("CREATE VIEW view_claim_pro_claim AS SELECT * FROM claim_pro_claim");
            migrationBuilder.Sql("CREATE VIEW view_claim_pro_motor AS SELECT * FROM claim_pro_motor");
            migrationBuilder.Sql("CREATE VIEW view_claim_pro_exposure AS SELECT * FROM claim_pro_exposure");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW view_claim_pro_claim");
            migrationBuilder.Sql("DROP VIEW view_claim_pro_motor");
            migrationBuilder.Sql("DROP VIEW view_claim_pro_exposure");

            migrationBuilder.DropTable(
                name: "claim_hub_claim");

            migrationBuilder.DropTable(
                name: "claim_hub_exposure");

            migrationBuilder.DropTable(
                name: "claim_hub_motor");

            migrationBuilder.DropTable(
                name: "claim_pro_claim");

            migrationBuilder.DropTable(
                name: "claim_pro_exposure");

            migrationBuilder.DropTable(
                name: "claim_pro_motor");

            migrationBuilder.DropTable(
                name: "claim_zone_exposure");

            migrationBuilder.DropTable(
                name: "claim_zone_motor");

            migrationBuilder.DropTable(
                name: "insure_wave_exposure");

            migrationBuilder.DropTable(
                name: "insure_wave_motor");

            migrationBuilder.DropTable(
                name: "integration_event");

            migrationBuilder.DropTable(
                name: "claim_zone_claim");

            migrationBuilder.DropTable(
                name: "insure_wave_claim");
        }
    }
}
