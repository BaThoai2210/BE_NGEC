using Ecomm.API.DataAccess;
using Ecomm.API.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecomm.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        readonly IOrderService dataAccess;
        private readonly string DateFormat;
        public OrderController(IOrderService dataAccess, IConfiguration configuration)
        {
            this.dataAccess = dataAccess;
            DateFormat = configuration["Constants:DateFormat"];
        }

        [HttpPost("InsertOrder")]
        public IActionResult InsertOrder(Order order)
        {
            order.CreatedAt = DateTime.Now.ToString();
            var id = dataAccess.InsertOrder(order);
            return Ok(id.ToString());
        }

        [HttpGet("GetOrders")]
        public IActionResult GetAllOrder()
        {
            var result = dataAccess.GetAllOrder();
            return Ok(result);
        }

        [HttpGet("GetOrders/{id}")]
        public IActionResult GetOrder(int id)
        {
            var result = dataAccess.GetOrder(id);
            return Ok(result);
        }

        [HttpDelete("Delete")]
        public IActionResult Delete(int id)
        {
            var result = dataAccess.Delete(id);
            return Ok(result ? "deleted" : "delete fail");
        }

        [HttpPut("Update")]
        public IActionResult Update(Order order)
        {
            var result = dataAccess.Update(order);
            return Ok(result ? "updated" : "update fail");
        }
    }
}
