namespace DigitalAppraiser.Models.DBModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("OrnamentDetails")]
    public class OrnamentDetails
    {
        [Key]
        public int OrnamentId { get; set; }
        public string Ornament { get; set; }
        public int Quantity { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal StoneWeight { get; set; }
        public int Quality { get; set; }
        public decimal NetWeight { get; set; }
        public decimal MarketPrice { get; set; }
        public int CustomerId { get; set; }
        public int LoanType { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset ModifiedOn { get; set; }
    }
}