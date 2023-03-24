using System;
using Ecomm.API.DataAccess;
using Ecomm.API.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Ecomm.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService dataAccess;
        private readonly string DateFormat;
        public PaymentController(IPaymentService dataAccess, IConfiguration configuration)
        {
            this.dataAccess = dataAccess;
            DateFormat = configuration["Constants:DateFormat"];
        }
        [HttpPost("InsertPayment")]
        public IActionResult InsertPayment(Payment payment)
        {
            payment.CreatedAt = DateTime.Now.ToString();
            var id = dataAccess.InsertPayment(payment);
            return Ok(id.ToString());
        }

        [HttpGet("GetPayment/{id}")]
        public IActionResult GetUser(int id)
        {
            var result = dataAccess.GetPayment(id);
            return Ok(result);
        }
    }
}

