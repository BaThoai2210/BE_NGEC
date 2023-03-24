using System;
using Ecomm.API.Models;

namespace Ecomm.API.DataAccess
{
	public interface IOrderService
	{
		public int InsertOrder(Order order);
		List<Order> GetAllOrder();
        Order GetOrder(int id);
		bool Update(Order order);
        bool Delete(int id);
    }
}

