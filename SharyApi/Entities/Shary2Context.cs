using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace SharyApi.Entities
{
    public partial class Shary2Context : DbContext
    {
        public Shary2Context()
        {
        }

        public Shary2Context(DbContextOptions<Shary2Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Business> Businesses { get; set; }
        public virtual DbSet<BusinessAddress> BusinessAddresses { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<FoodDonation> FoodDonations { get; set; }
        public virtual DbSet<Individual> Individuals { get; set; }
        public virtual DbSet<MealPrice> MealPrices { get; set; }
        public virtual DbSet<MoneyDonation> MoneyDonations { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<ReceivedMeal> ReceivedMeals { get; set; }
        public virtual DbSet<SharedMeal> SharedMeals { get; set; }
        public virtual DbSet<SharedSolidarityMeal> SharedSolidarityMeals { get; set; }
        public virtual DbSet<SolidarityDinnerDonation> SolidarityDinnerDonations { get; set; }
        public virtual DbSet<SolidarityMealPrice> SolidarityMealPrices { get; set; }
        public virtual DbSet<Station> Stations { get; set; }
        public virtual DbSet<StationAddress> StationAddresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Business>(entity =>
            {
                entity.ToTable("Business");

                entity.HasIndex(e => e.Username, "UQ__Business__536C85E4F43D1FE1")
                    .IsUnique();

                entity.HasIndex(e => e.Salt, "UQ__Business__A152BCCE9CB96E67")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.BankAccount)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Salt)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.Tin)
                    .IsRequired()
                    .HasMaxLength(9)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BusinessAddress>(entity =>
            {
                entity.ToTable("BusinessAddress");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.BusinessId).HasColumnName("BusinessID");

                entity.Property(e => e.CityId).HasColumnName("CityID");

                entity.Property(e => e.Street)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.StreetNumber)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Business)
                    .WithMany(p => p.BusinessAddresses)
                    .HasForeignKey(d => d.BusinessId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BusinessA__Busin__68487DD7");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.BusinessAddresses)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BusinessA__CityI__6754599E");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("City");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Cities)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__City__CountryID__6477ECF3");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("Country");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Currency>(entity =>
            {
                entity.ToTable("Currency");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FoodDonation>(entity =>
            {
                entity.ToTable("FoodDonation");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.BusinessId).HasColumnName("BusinessID");

                entity.Property(e => e.DonationDateTime).HasColumnType("datetime");

                entity.Property(e => e.StationId).HasColumnName("StationID");

                entity.HasOne(d => d.Business)
                    .WithMany(p => p.FoodDonations)
                    .HasForeignKey(d => d.BusinessId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FoodDonat__Busin__114A936A");

                entity.HasOne(d => d.Station)
                    .WithMany(p => p.FoodDonations)
                    .HasForeignKey(d => d.StationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FoodDonat__Stati__10566F31");
            });

            modelBuilder.Entity<Individual>(entity =>
            {
                entity.ToTable("Individual");

                entity.HasIndex(e => e.Username, "UQ__Individu__536C85E439577FBB")
                    .IsUnique();

                entity.HasIndex(e => e.Salt, "UQ__Individu__A152BCCE593B065C")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Salt)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MealPrice>(entity =>
            {
                entity.ToTable("MealPrice");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.CurrencyId).HasColumnName("CurrencyID");

                entity.Property(e => e.ValidFrom).HasColumnType("date");

                entity.Property(e => e.ValidTo).HasColumnType("date");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.MealPrices)
                    .HasForeignKey(d => d.CurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MealPrice__Curre__00200768");
            });

            modelBuilder.Entity<MoneyDonation>(entity =>
            {
                entity.ToTable("MoneyDonation");

                entity.HasIndex(e => e.StripePaymentId, "UQ__MoneyDon__4DBD22ECB3E4E668")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.DonationDateTime).HasColumnType("datetime");

                entity.Property(e => e.IndividualId).HasColumnName("IndividualID");

                entity.Property(e => e.MealPriceId).HasColumnName("MealPriceID");

                entity.Property(e => e.StripePaymentId)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("StripePaymentID");

                entity.HasOne(d => d.Individual)
                    .WithMany(p => p.MoneyDonations)
                    .HasForeignKey(d => d.IndividualId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MoneyDona__Indiv__05D8E0BE");

                entity.HasOne(d => d.MealPrice)
                    .WithMany(p => p.MoneyDonations)
                    .HasForeignKey(d => d.MealPriceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MoneyDona__MealP__06CD04F7");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.BusinessId).HasColumnName("BusinessID");

                entity.Property(e => e.PaymentDateTime).HasColumnType("datetime");

                entity.Property(e => e.SolidarityMealPriceId).HasColumnName("SolidarityMealPriceID");

                entity.HasOne(d => d.Business)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.BusinessId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Payment__Busines__6EF57B66");

                entity.HasOne(d => d.SolidarityMealPrice)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.SolidarityMealPriceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Payment__Solidar__6FE99F9F");
            });

            modelBuilder.Entity<ReceivedMeal>(entity =>
            {
                entity.ToTable("ReceivedMeal");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.FoodDonationId).HasColumnName("FoodDonationID");

                entity.Property(e => e.ReceivedDateTime).HasColumnType("datetime");

                entity.Property(e => e.StationId).HasColumnName("StationID");

                entity.HasOne(d => d.FoodDonation)
                    .WithMany(p => p.ReceivedMeals)
                    .HasForeignKey(d => d.FoodDonationId)
                    .HasConstraintName("FK_ReceivedMeal_FoodDonation");

                entity.HasOne(d => d.Station)
                    .WithMany(p => p.ReceivedMeals)
                    .HasForeignKey(d => d.StationId)
                    .HasConstraintName("FK__ReceivedM__Stati__09A971A2");
            });

            modelBuilder.Entity<SharedMeal>(entity =>
            {
                entity.ToTable("SharedMeal");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.ShareDateTime).HasColumnType("datetime");

                entity.Property(e => e.StationId).HasColumnName("StationID");

                entity.HasOne(d => d.Station)
                    .WithMany(p => p.SharedMeals)
                    .HasForeignKey(d => d.StationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SharedMea__Stati__7A672E12");
            });

            modelBuilder.Entity<SharedSolidarityMeal>(entity =>
            {
                entity.ToTable("SharedSolidarityMeal");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.BusinessAddressId).HasColumnName("BusinessAddressID");

                entity.Property(e => e.ShareDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.BusinessAddress)
                    .WithMany(p => p.SharedSolidarityMeals)
                    .HasForeignKey(d => d.BusinessAddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SharedSol__Busin__02FC7413");
            });

            modelBuilder.Entity<SolidarityDinnerDonation>(entity =>
            {
                entity.ToTable("SolidarityDinnerDonation");

                entity.HasIndex(e => e.StripePaymentId, "UQ__Solidari__4DBD22EC6722B8A8")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.StripePaymentId)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("StripePaymentID");

                entity.Property(e => e.BusinessId).HasColumnName("BusinessID");

                entity.Property(e => e.DonationDateTime).HasColumnType("datetime");

                entity.Property(e => e.IndividualId).HasColumnName("IndividualID");

                entity.HasOne(d => d.Business)
                    .WithMany(p => p.SolidarityDinnerDonations)
                    .HasForeignKey(d => d.BusinessId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Solidarit__Busin__0C85DE4D");

                entity.HasOne(d => d.Individual)
                    .WithMany(p => p.SolidarityDinnerDonations)
                    .HasForeignKey(d => d.IndividualId)
                    .HasConstraintName("FK__Solidarit__Indiv__0D7A0286");
            });

            modelBuilder.Entity<SolidarityMealPrice>(entity =>
            {
                entity.ToTable("SolidarityMealPrice");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.BusinessId).HasColumnName("BusinessID");

                entity.Property(e => e.CurrencyId).HasColumnName("CurrencyID");

                entity.Property(e => e.ValidFrom).HasColumnType("date");

                entity.Property(e => e.ValidTo).HasColumnType("date");

                entity.HasOne(d => d.Business)
                    .WithMany(p => p.SolidarityMealPrices)
                    .HasForeignKey(d => d.BusinessId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Solidarit__Busin__6B24EA82");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.SolidarityMealPrices)
                    .HasForeignKey(d => d.CurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Solidarit__Curre__6C190EBB");
            });

            modelBuilder.Entity<Station>(entity =>
            {
                entity.ToTable("Station");

                entity.HasIndex(e => e.Username, "UQ__Station__536C85E445683064")
                    .IsUnique();

                entity.HasIndex(e => e.Salt, "UQ__Station__A152BCCE86AEB23F")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.Salt)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.StationAddressId).HasColumnName("StationAddressID");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.StationAddress)
                    .WithMany(p => p.Stations)
                    .HasForeignKey(d => d.StationAddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Station__Station__778AC167");
            });

            modelBuilder.Entity<StationAddress>(entity =>
            {
                entity.ToTable("StationAddress");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.CityId).HasColumnName("CityID");

                entity.Property(e => e.Street)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.StreetNumber)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.City)
                    .WithMany(p => p.StationAddresses)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__StationAd__CityI__72C60C4A");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
