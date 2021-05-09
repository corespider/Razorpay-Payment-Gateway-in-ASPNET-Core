using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Razorpay.Integration.Models;
using Razorpay.Integration.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Razorpay.Integration.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ILogger<PaymentController> _logger;
        private readonly IPaymentService _service;
        private IHttpContextAccessor _httpContextAccessor;
        public PaymentController(ILogger<PaymentController> logger, IPaymentService service, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ProcessRequestOrder(PaymentRequest _paymentRequest)
        {
            MerchantOrder _marchantOrder = await _service.ProcessMerchantOrder(_paymentRequest);
            return View("Payment", _marchantOrder);
        }
        [HttpPost]
        public async Task<IActionResult> CompleteOrderProcess()
        {
            string PaymentMessage =await _service.CompleteOrderProcess(_httpContextAccessor);
            if (PaymentMessage == "captured")
            {
                return RedirectToAction("Success");
            }
            else
            {
                return RedirectToAction("Failed");
            }
        }
        public IActionResult Success()
        {
            return View();
        }

        public IActionResult Failed()
        {
            return View();
        }
    }
}
