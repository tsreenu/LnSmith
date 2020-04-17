namespace DigitalAppraiser.Models.DBModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class AppraiserBank
    {
        [Key]
        public int AppraiserBankId { get; set; }
        public int? AppriaserId { get; set; }
        public int? BankId { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset ModifiedOn { get; set; }

        public virtual AppraiserDetail AppraiserDetail { get; set; }
    }
}
