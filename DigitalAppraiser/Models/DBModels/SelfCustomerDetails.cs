namespace DigitalAppraiser.Models.DBModels
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("SelfCustomerDetails")]
    public class SelfCustomerDetails
    {
        [Key]
        public int CustomerId { get; set; }
        [Required]
        public string Name { get; set; }
        public string UANNumber { get; set; }
        [Required]
        [MaxLength(10, ErrorMessage = "Mobile number lenth should be 10.")]
        [MinLength(10, ErrorMessage = "Mobile number lenth should be 10.")]
        [DisplayName("Mobile number")]
        public string MobileNumber { get; set; }
        [Required]
        public string Address { get; set; }
        public int AppraiserId { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        [Required]
        public DateTimeOffset CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset ModifiedOn { get; set; }
    }
}