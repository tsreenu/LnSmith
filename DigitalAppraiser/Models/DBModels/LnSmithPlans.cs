using System.ComponentModel.DataAnnotations;

namespace DigitalAppraiser.Models.DBModels
{
    public class LnSmithPlans
    {
        [Key]
        public int PlanId { get; set; }
        public string PlanName { get; set; }
        public double PlanValue { get; set; }
        public int PlanDuration { get; set; }
        public bool IsActive { get; set; }
    }
}