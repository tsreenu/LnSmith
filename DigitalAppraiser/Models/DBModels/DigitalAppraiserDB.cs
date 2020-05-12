namespace DigitalAppraiser.Models.DBModels
{
    using System.Data.Entity;

    public partial class DigitalAppraiserDB : DbContext
    {
        public DigitalAppraiserDB()
            : base("name=DigitalAppraiserDB")
        {
        }

        public virtual DbSet<AppraiserBank> AppraiserBanks { get; set; }
        public virtual DbSet<AppraiserDetail> AppraiserDetails { get; set; }
        public virtual DbSet<BankMaster> BankMasters { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<TodayRate> TodayRates { get; set; }
        public virtual DbSet<SelfCustomerDetails> SelfCustomerDetails { get; set; }
        public virtual DbSet<OrnamentDetails> OrnamentDetails { get; set; }
        public virtual DbSet<LoanDetails> LoanDetails { get; set; }
        public virtual DbSet<BankCustomerDetails> BankCustomerDetails { get; set; }
        public virtual DbSet<LnSmithPlans> LnSmithPlans { get; set; }
        public virtual DbSet<SubscriptionDetails> SubscriptionDetails { get; set; }
        public virtual DbSet<PaytmResponse> PaytmResponse { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppraiserBank>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<AppraiserBank>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<AppraiserDetail>()
                .Property(e => e.AppraiserName)
                .IsUnicode(false);

            modelBuilder.Entity<AppraiserDetail>()
                .Property(e => e.AppraiserNumber)
                .IsUnicode(false);

            modelBuilder.Entity<AppraiserDetail>()
                .Property(e => e.MobileNumber)
                .IsUnicode(false);

            modelBuilder.Entity<AppraiserDetail>()
                .Property(e => e.ShopName)
                .IsUnicode(false);

            modelBuilder.Entity<AppraiserDetail>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<AppraiserDetail>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<AppraiserDetail>()
                .HasMany(e => e.AppraiserBanks)
                .WithOptional(e => e.AppraiserDetail)
                .HasForeignKey(e => e.AppriaserId);

            modelBuilder.Entity<BankMaster>()
                .Property(e => e.BankName)
                .IsUnicode(false);

            modelBuilder.Entity<BankMaster>()
                .Property(e => e.BankCode)
                .IsUnicode(false);

            modelBuilder.Entity<BankMaster>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<BankMaster>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<City>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<City>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<State>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<State>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<TodayRate>()
                .Property(e => e.Rate)
                .HasPrecision(30, 15);

            modelBuilder.Entity<TodayRate>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<TodayRate>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<SelfCustomerDetails>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<SelfCustomerDetails>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<OrnamentDetails>()
               .Property(e => e.CreatedBy)
               .IsUnicode(false);

            modelBuilder.Entity<OrnamentDetails>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<LoanDetails>()
               .Property(e => e.CreatedBy)
               .IsUnicode(false);

            modelBuilder.Entity<LoanDetails>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<LoanDetails>()
               .Property(e => e.CreatedBy)
               .IsUnicode(false);

            modelBuilder.Entity<LoanDetails>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);
        }
    }
}
