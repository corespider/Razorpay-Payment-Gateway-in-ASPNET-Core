using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Razorpay.Integration.Models
{
    public class MerchantOrder
    {
        public string OrderId { get; set; }
        public string RazorpayKey { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
    }
}
