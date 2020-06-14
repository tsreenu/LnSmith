using DigitalAppraiser.Models.DBModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DigitalAppraiser.Models.ViewModels
{
    public class BankCustomerModel
    {
        public BankCustomerDetails bankCustomer { get; set; }
        public IEnumerable<SelectListItem> Quantity { get; set; }
        public IEnumerable<SelectListItem> RelationTypes { get; set; }
        [Required]
        [DisplayName("Bank")]
        public int selectedBank { get; set; }
        public IEnumerable<SelectListItem> Banks { get; set; }
        public DBModels.OrnamentDetails ornamentDetails { get; set; }
        public List<DBModels.OrnamentDetails> ornamentsList { get; set; }
    }
}