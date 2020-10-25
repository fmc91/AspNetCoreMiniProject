using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RightFlightWeb.EntityModel
{
    public partial class FlightReservationContext : DbContext
    {
        public FlightReservationContext() { }

        public FlightReservationContext(DbContextOptions<FlightReservationContext> options) : base(options) { }

        public virtual DbSet<Aircraft> Aircraft { get; set; }
        public virtual DbSet<Airline> Airline { get; set; }
        public virtual DbSet<Airport> Airport { get; set; }
        public virtual DbSet<Booking> Booking { get; set; }
        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<Flight> Flight { get; set; }
        public virtual DbSet<Nationality> Nationality { get; set; }
        public virtual DbSet<Passenger> Passenger { get; set; }
        public virtual DbSet<Route> Route { get; set; }
        public virtual DbSet<RouteAircraft> RouteAircraft { get; set; }
        public virtual DbSet<TravelClass> TravelClass { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=flight_reservation;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Aircraft>(entity =>
            {
                entity.HasKey(e => e.IcaoTypeCode)
                    .HasName("PK__aircraft__5A1DF9D279722BCB");

                entity.ToTable("aircraft");

                entity.Property(e => e.IcaoTypeCode)
                    .HasColumnName("icao_type_code")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Model)
                    .IsRequired()
                    .HasColumnName("model")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Airline>(entity =>
            {
                entity.HasKey(e => e.IataAirlineCode)
                    .HasName("PK__airline__19E924EE4621405D");

                entity.ToTable("airline");

                entity.Property(e => e.IataAirlineCode)
                    .HasColumnName("iata_airline_code")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.LogoFilename)
                    .HasColumnName("logo_filename")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Airport>(entity =>
            {
                entity.HasKey(e => e.IataAirportCode)
                    .HasName("PK__airport__57E8CB97F986E363");

                entity.ToTable("airport");

                entity.Property(e => e.IataAirportCode)
                    .HasColumnName("iata_airport_code")
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CityCode)
                    .HasColumnName("city_code")
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Airport)
                    .HasForeignKey(d => d.CityCode)
                    .HasConstraintName("FK__airport__city_co__73BA3083");
            });

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.ToTable("booking");

                entity.HasIndex(e => e.BookingReference)
                    .HasName("UQ__booking__BADA4559D62879D1")
                    .IsUnique();

                entity.Property(e => e.BookingId).HasColumnName("booking_id");

                entity.Property(e => e.BookingReference)
                    .IsRequired()
                    .HasColumnName("booking_reference")
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Cost)
                    .HasColumnName("cost")
                    .HasColumnType("decimal(8, 2)");

                entity.Property(e => e.FlightId).HasColumnName("flight_id");

                entity.Property(e => e.TimeOfBooking)
                    .HasColumnName("time_of_booking")
                    .HasColumnType("datetime");

                entity.Property(e => e.TravelClassCode)
                    .IsRequired()
                    .HasColumnName("travel_class_code")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.Flight)
                    .WithMany(p => p.Booking)
                    .HasForeignKey(d => d.FlightId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__booking__flight___22751F6C");

                entity.HasOne(d => d.TravelClassCodeNavigation)
                    .WithMany(p => p.Booking)
                    .HasForeignKey(d => d.TravelClassCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__booking__travel___236943A5");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.HasKey(e => e.IataCityCode)
                    .HasName("PK__city__B3E7D24A10397E40");

                entity.ToTable("city");

                entity.Property(e => e.IataCityCode)
                    .HasColumnName("iata_city_code")
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CountryCode)
                    .IsRequired()
                    .HasColumnName("country_code")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TimeZone)
                    .IsRequired()
                    .HasColumnName("time_zone")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.City)
                    .HasForeignKey(d => d.CountryCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__city__country_co__6FE99F9F");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(e => e.IsoCountryCode)
                    .HasName("PK__country__5276C2FD23F04861");

                entity.ToTable("country");

                entity.Property(e => e.IsoCountryCode)
                    .HasColumnName("iso_country_code")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Flight>(entity =>
            {
                entity.ToTable("flight");

                entity.Property(e => e.FlightId).HasColumnName("flight_id");

                entity.Property(e => e.FlightNumber)
                    .IsRequired()
                    .HasColumnName("flight_number")
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.RouteAircraftId).HasColumnName("route_aircraft_id");

                entity.Property(e => e.ScheduledDeparture)
                    .HasColumnName("scheduled_departure")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.RouteAircraft)
                    .WithMany(p => p.Flight)
                    .HasForeignKey(d => d.RouteAircraftId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__flight__route_ai__1EA48E88");
            });

            modelBuilder.Entity<Nationality>(entity =>
            {
                entity.ToTable("nationality");

                entity.Property(e => e.NationalityId).HasColumnName("nationality_id");

                entity.Property(e => e.CountryName)
                    .IsRequired()
                    .HasColumnName("country_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Passenger>(entity =>
            {
                entity.ToTable("passenger");

                entity.Property(e => e.PassengerId).HasColumnName("passenger_id");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasColumnName("address")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.BookingId).HasColumnName("booking_id");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasColumnName("city")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasColumnName("country")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DateOfBirth)
                    .HasColumnName("date_of_birth")
                    .HasColumnType("date");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("first_name")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("last_name")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.NationalityId).HasColumnName("nationality_id");

                entity.Property(e => e.Postcode)
                    .IsRequired()
                    .HasColumnName("postcode")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.Passenger)
                    .HasForeignKey(d => d.BookingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__passenger__booki__2739D489");

                entity.HasOne(d => d.Nationality)
                    .WithMany(p => p.Passenger)
                    .HasForeignKey(d => d.NationalityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__passenger__natio__282DF8C2");
            });

            modelBuilder.Entity<Route>(entity =>
            {
                entity.ToTable("route");

                entity.Property(e => e.RouteId).HasColumnName("route_id");

                entity.Property(e => e.AirlineCode)
                    .IsRequired()
                    .HasColumnName("airline_code")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.DestinationAirportCode)
                    .IsRequired()
                    .HasColumnName("destination_airport_code")
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.OriginAirportCode)
                    .IsRequired()
                    .HasColumnName("origin_airport_code")
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.PricingScheme)
                    .IsRequired()
                    .HasColumnName("pricing_scheme")
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.HasOne(d => d.Airline)
                    .WithMany(p => p.Route)
                    .HasForeignKey(d => d.AirlineCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__route__airline_c__160F4887");

                entity.HasOne(d => d.Destination)
                    .WithMany(p => p.DestinationRoute)
                    .HasForeignKey(d => d.DestinationAirportCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__route__destinati__17F790F9");

                entity.HasOne(d => d.Origin)
                    .WithMany(p => p.OriginRoute)
                    .HasForeignKey(d => d.OriginAirportCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__route__origin_ai__17036CC0");
            });

            modelBuilder.Entity<RouteAircraft>(entity =>
            {
                entity.ToTable("route_aircraft");

                entity.Property(e => e.RouteAircraftId).HasColumnName("route_aircraft_id");

                entity.Property(e => e.FlightDuration).HasColumnName("flight_duration");

                entity.Property(e => e.IcaoTypeCode)
                    .IsRequired()
                    .HasColumnName("icao_type_code")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.RouteId).HasColumnName("route_id");

                entity.HasOne(d => d.Aircraft)
                    .WithMany(p => p.RouteAircraft)
                    .HasForeignKey(d => d.IcaoTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__route_air__icao___1AD3FDA4");

                entity.HasOne(d => d.Route)
                    .WithMany(p => p.RouteAircraft)
                    .HasForeignKey(d => d.RouteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__route_air__route__1BC821DD");
            });

            modelBuilder.Entity<TravelClass>(entity =>
            {
                entity.HasKey(e => e.TravelClassCode)
                    .HasName("PK__travel_c__A6F0509721AD15FC");

                entity.ToTable("travel_class");

                entity.Property(e => e.TravelClassCode)
                    .HasColumnName("travel_class_code")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
