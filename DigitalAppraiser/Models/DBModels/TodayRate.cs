namespace DigitalAppraiser.Models.DBModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TodayRate")]
    public partial class TodayRate
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public decimal Rate { get; set; }
        public int? BankId { get; set; }
        public int? AppraiserId { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset ModifiedOn { get; set; }
        public bool IsActive { get; set; }
        public virtual AppraiserDetail AppraiserDetail { get; set; }
    }
}
