using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using CalendarMvc.Models;

namespace CalendarMvc.Migrations
{
    [DbContext(typeof(CalendarMvcContext))]
    [Migration("20170706134048_EventId")]
    partial class EventId
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

                    b.Property<string>("Description");

                    b.Property<string>("EntityId");

                    b.Property<string>("EventId");

                    b.Property<string>("Location");

                    b.Property<string>("PrimaryAttendeeEmail")
                        .IsRequired();

                    b.Property<string>("PrimaryAttendeeName");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(60);

                    b.HasKey("ID");

                    b.ToTable("Event");
                });
        }
    }
}
