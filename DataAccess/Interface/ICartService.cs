using System;
using Ecomm.API.Models;

namespace Ecomm.API.DataAccess
{
	public interface ICartService
	{
        bool InsertCartItem(int userId, int productId, int quantity);
        bool DeleteCartItem(int userId, int productId, int quantity);
        Cart GetActiveCartOfUser(int userid);
        Cart GetCart(int cartid);
        List<Cart> GetAllPreviousCartsOfUser(int userid);
	}
}

