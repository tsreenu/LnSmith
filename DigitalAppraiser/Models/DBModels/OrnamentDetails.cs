namespace DigitalAppraiser.Models.DBModels
{
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("OrnamentDetails")]
    public class OrnamentDetails
    {
        [Key]
        public int OrnamentId { get; set; }
        [Required]
        public string Ornament { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal GrossWeight { get; set; }
        public decimal StoneWeight { get; set; }
        public int Quality { get; set; }
        [Required]
        public decimal NetWeight { get; set; }
        [Required]
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