namespace DigitalAppraiser.Models.DBModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("BankCustomerDetails")]
    public class BankCustomerDetails
    {
        [Key]
        public int CustomerId { get; set; }
        public string Branch { get; set; }
        public string Name { get; set; }
        public int RelationType { get; set; }
        public string RelationName { get; set; }
        public string MobileNumber { get; set; }
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