namespace DigitalAppraiser.Models.DBModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class AppraiserDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AppraiserDetail()
        {
            AppraiserBanks = new HashSet<AppraiserBank>();
            TodayRates = new HashSet<TodayRate>();
        }

        [Key]
        public int AppraiserId { get; set; }
        public string AppraiserName { get; set; }
        public string AppraiserNumber { get; set; }
        public string MobileNumber { get; set; }
        public string Password { get; set; }
        public int? StateId { get; set; }
        public int? CityId { get; set; }
        public string ShopName { get; set; }
        public bool? IsSelfLoan { get; set; }
        public string ShopAddress { get; set; }
        public string ShopNumber { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset ModifiedOn { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AppraiserBank> AppraiserBanks { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TodayRate> TodayRates { get; set; }
    }
}
