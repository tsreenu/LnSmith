﻿using System;
using System.Collections.Generic;

namespace DigitalAppraiser.Models.ViewModels
{
    public class CustomerLoanData
    {
        public DateTimeOffset Date { get; set; }
        public string BankName { get; set; }
        public string LoanId { get; set; }
        public string CustomerName { get; set; }
        public string MobileNumber { get; set; }
        public string Area { get; set; }
        public decimal LoanAmount { get; set; }
        public decimal InterestRate { get; set; }
        public bool IsActive { get; set; }
    }
    public class CustomerLoanDataModel
    {
        public IEnumerable<CustomerLoanData> customerDataList { get; set; }
    }
}