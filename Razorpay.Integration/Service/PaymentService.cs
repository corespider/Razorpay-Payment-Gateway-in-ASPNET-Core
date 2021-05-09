using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Razorpay.Integration.Models;
using Razorpay.Integration.Service;
namespace Razorpay.Integration.Service
{
    public class PaymentService : IPaymentService
    {
       
        public Task<MerchantOrder> ProcessMerchantOrder(PaymentRequest payRequest)
        {
            try
            {
                // Generate random receipt number for order
                Random randomObj = new Random();
                string transactionId = randomObj.Next(10000000, 100000000).ToString();

                Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient("rzp_test_8P7RhnsImxd2OR", "kD8tw7ECYsTTZnx0OyrKI4kh");
                Dictionary<string, object> options = new Dictionary<string, object>();
                options.Add("amount", payRequest.Amount * 100);
                options.Add("receipt", transactionId);
                options.Add("currency", "INR");
                options.Add("payment_capture", "0"); // 1 - automatic  , 0 - manual
                //options.Add("Notes", "Test Payment of Merchant");

                Razorpay.Api.Order orderResponse = client.Order.Create(options);
                string orderId = orderResponse["id"].ToString();

                MerchantOrder order = new MerchantOrder
                {
                    OrderId = orderResponse.Attributes["id"],
                    RazorpayKey = "rzp_test_8P7RhnsImxd2OR",
                    Amount = payRequest.Amount * 100,
                    Currency = "INR",
                    Name = payRequest.Name,
                    Email = payRequest.Email,
                    PhoneNumber = payRequest.PhoneNumber,
                    Address = payRequest.Address,
                    Description = "Order by Merchant"
                };
                return Task.FromResult(order);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<string> CompleteOrderProcess(IHttpContextAccessor _httpContextAccessor)
        {
            try
            {
                string paymentId = _httpContextAccessor.HttpContext.Request.Form["rzp_paymentid"];

                // This is orderId
                string orderId = _httpContextAccessor.HttpContext.Request.Form["rzp_orderid"];

                Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient("rzp_test_8P7RhnsImxd2OR", "kD8tw7ECYsTTZnx0OyrKI4kh");

                Razorpay.Api.Payment payment = client.Payment.Fetch(paymentId);

                // This code is for capture the payment 
                Dictionary<string, object> options = new Dictionary<string, object>();
                options.Add("amount", payment.Attributes["amount"]);
                Razorpay.Api.Payment paymentCaptured = payment.Capture(options);
                string amt = paymentCaptured.Attributes["amount"];
                return paymentCaptured.Attributes["status"];
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
