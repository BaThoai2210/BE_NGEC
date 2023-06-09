﻿using System;
using Ecomm.API.DataAccess;
using Ecomm.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;
using Ecomm.API.Models.Request;

namespace Ecomm.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
	{
        private readonly ICartService dataAccess;
        private readonly string DateFormat;
		public CartController(ICartService dataAccess, IConfiguration configuration)
		{
            this.dataAccess = dataAccess;
            DateFormat = configuration["Constants:DateFormat"];
		}
        [HttpGet("GetActiveCartOfUser/{id}")]
        public IActionResult GetActiveCartOfUser(int id)
        {
            var result = dataAccess.GetActiveCartOfUser(id);
            return Ok(result);
        }

        [HttpGet("GetAllPreviousCartsOfUser/{id}")]
        public IActionResult GetAllPreviousCartsOfUser(int id)
        {
            var result = dataAccess.GetAllPreviousCartsOfUser(id);
            return Ok(result);
        }
        [HttpPost("InsertCartItem/{userid}/{productid}/{quantity}")]
        public IActionResult InsertCartItem(int userid, int productid, int quantity)
        {
            var result = dataAccess.InsertCartItem(userid, productid, quantity);
            return Ok(result ? "inserted" : "insert fail");
        }
        
        [HttpDelete("DeleteCartItem/{userid}/{productid}/{quantity}")]
        public IActionResult DeleteCartItem(int userid, int productid, int quantity)
        {
            var result = dataAccess.DeleteCartItem(userid, productid, quantity);
            return Ok(result ? "deleted" : "delete fail");
        }
    }
}

