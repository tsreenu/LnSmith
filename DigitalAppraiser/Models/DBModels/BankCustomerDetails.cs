namespace DigitalAppraiser.Models.DBModels
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("BankCustomerDetails")]
    public class BankCustomerDetails
    {
        [Key]
        public int CustomerId { get; set; }
        [Required]
        public string Branch { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [DisplayName("Relation Type")]
        public int RelationType { get; set; }
        [Required]
        [DisplayName("Relation Name")]
        public string RelationName { get; set; }

        [MaxLength(10, ErrorMessage = "Mobile number lenth should be 10.")]
        [MinLength(10, ErrorMessage = "Mobile number lenth should be 10.")]
        [DisplayName("Mobile number")]
        [Required]
        public string MobileNumber { get; set; }
        [Required]
        public string Address { get; set; }
        //public string AccountDetails { get; set; }
        //public string AadharNumber { get; set; }
        //public string PanNumber { get; set; }
        //public string Caste { get; set; }
        public int AppraiserId { get; set; }
        public int BankId { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset ModifiedOn { get; set; }
    }
}