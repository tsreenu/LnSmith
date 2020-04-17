using System;

namespace DigitalAppraiser.Models.ViewModels
{
    public class SelfCustomerModel
    {
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