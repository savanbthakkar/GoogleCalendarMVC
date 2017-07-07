using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using CalendarMvc.Models;

namespace CalendarMvc.Migrations
{
    [DbContext(typeof(CalendarMvcContext))]
    [Migration("20170627161936_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CalendarMvc.Models.Event", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<string>("Location");

                    b.Property<string>("PrimaryAttendeeEmail");

                    b.Property<string>("PrimaryAttendeeName");

                    b.Property<string>("Title")
                        .HasMaxLength(60);

                    b.HasKey("ID");

                    b.ToTable("Event");
                });
        }
    }
}
