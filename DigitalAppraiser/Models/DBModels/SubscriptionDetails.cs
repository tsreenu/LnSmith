using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalAppraiser.Models.DBModels
{
    public class SubscriptionDetails
    {
        [Key]
        public int SubscriptionId { get; set; }
        public int AppraiserId { get; set; }
        public int PlanId { get; set; }
        public DateTimeOffset SubscriptionStartDate { get; set; }
        public DateTimeOffset SubscriptionEndDate { get; set; }
        public bool IsActive { get; set; }

        [NotMapped]
        public string PlanName { get; set; }
    }
}