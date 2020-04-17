namespace DigitalAppraiser.Models.DBModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("SelfCustomerDetails")]
    public class SelfCustomerDetails
    {
        [Key]
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string UANNumber { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public int AppraiserId { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset ModifiedOn { get; set; }
    }
}