namespace DigitalAppraiser.Models.DBModels
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("LoanDetails")]
    public class LoanDetails
    {
        public int ID { get; set; }
        public string LoanId { get; set; }
        public int CustomerId { get; set; }
        public int LoanType { get; set; }
        public decimal LoanAmount { get; set; }
        public decimal Interest { get; set; }
        public decimal EMI { get; set; }
        public decimal? CollectedAmount { get; set; }
        public decimal? CollectedInterest { get; set; }
        public int? NoOfDays { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset ModifiedOn { get; set; }
    }
}