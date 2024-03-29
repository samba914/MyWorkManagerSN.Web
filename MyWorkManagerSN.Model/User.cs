﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyWorkManagerSN.Model
{
    public class User : mwmObject
    {
        public string? Username { get; set; }
        public string Email { get; set; }
        public string? Mobile { get; set; }
        public string? Devise { get; set; }
        public string? ProfileImagePath { get; set; }
        [NotMapped]
        public IFormFile ProfileImageFile { get; set; }
        public Address? Address { get; set; }
        public string? CompanyName { get; set; }
        public bool ShowImageOnInvoice { get; set; }
        public bool IsTrial { get; set; }
        public DateTime TrialEndDate { get; set; }
        public DateTime DateOfSubscription { get; set; }
        public bool HaveActiveContractOrTrial { get; set; }
        public string? ContractId { get; set; }
        [ForeignKey("ContractId")]
        public virtual Contract? Contract { get; set; }
        public AccountOptions AccountOptions { get; set; }
    }
}
