using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace DigitalAppraiser.Models.ViewModels
{
    public class SelfCustomerModel
    {
        public DBModels.SelfCustomerDetails selfCustomer { get; set; }
        public DBModels.OrnamentDetails ornamentDetails { get; set; }
        public IEnumerable<SelectListItem> Quantity { get; set; }
    }
}