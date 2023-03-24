using System;
using Ecomm.API.DataAccess;
using Ecomm.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;
using Ecomm.API.Models.Request;
using System.Data.SqlClient;
namespace Ecomm.API.DataAccess
{
	public class OrderService : IOrderService
	{
        #region Declare

        #endregion
        private readonly IConfiguration configuration;
        private readonly IUserService userService;
        private readonly ICartService cartService;
        private readonly IPaymentService paymentService;
        private readonly string dbconnection;
        private readonly string dateformat;
        public OrderService(IConfiguration configuration,IUserService userService, ICartService cartService, IPaymentService paymentService)
        {
            this.configuration = configuration;
            dbconnection = this.configuration["ConnectionStrings:DB"];
            dateformat = this.configuration["Constants:DateFormat"];
            this.userService = userService;
            this.cartService = cartService;
            this.paymentService = paymentService;
        }
		 public int InsertOrder(Order order)
         {
            int value = 0;

            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                string query = "INSERT INTO Orders (UserId, CartId, PaymentId, CreatedAt) values (@uid, @cid, @pid, @cat);";

                command.CommandText = query;
                command.Parameters.Add("@uid", System.Data.SqlDbType.Int).Value = order.User.Id;
                command.Parameters.Add("@cid", System.Data.SqlDbType.Int).Value = order.Cart.Id;
                command.Parameters.Add("@cat", System.Data.SqlDbType.NVarChar).Value = order.CreatedAt;
                command.Parameters.Add("@pid", System.Data.SqlDbType.Int).Value = order.Payment.Id;

                connection.Open();
                value = command.ExecuteNonQuery();

                if (value > 0)
                {
                    query = "UPDATE Carts SET Ordered='true', OrderedOn='" + DateTime.Now.ToString(dateformat) + "' WHERE CartId=" + order.Cart.Id + ";";
                    command.CommandText = query;
                    command.ExecuteNonQuery();

                    query = "SELECT TOP 1 Id FROM Orders ORDER BY Id DESC;";
                    command.CommandText = query;
                    value = (int)command.ExecuteScalar();
                }
                else
                {
                    value = 0;
                }
                connection.Close();
            }

            return value;
         }

        public List<Order> GetAllOrder()
        {
            var orders = new List<Order>();
            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                string query = "SELECT * FROM Orders";
                command.CommandText = query;

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var order = new Order();
                    order.Id = (int)reader["Id"];
                    order.User = userService.GetUser((int)reader["UserId"]);
                    order.Cart = cartService.GetCart((int)reader["CartId"]);
                    order.Payment = paymentService.GetPayment((int)reader["PaymentId"]);
                    order.CreatedAt = (String)reader["CreatedAt"];
                    orders.Add(order);
                }
            }
            return orders;
        }
        public Order GetOrder(int id)
        {
            var order = new Order();
            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                string query = "SELECT * FROM Orders WHERE UserId=" + id + ";";
                command.CommandText = query;

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    order.Id = (int)reader["Id"];
                    order.User = userService.GetUser((int)reader["UserId"]);
                    order.Cart = cartService.GetCart((int)reader["CartId"]);
                    order.Payment = paymentService.GetPayment((int)reader["PaymentId"]);
                    order.CreatedAt = (String)reader["CreatedAt"];
                }
            }
            return order;
        }
        public bool Update(Order order)
        {
            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };
                connection.Open();

                string query = "SELECT COUNT(*) FROM Orders WHERE Id ='" + order.Id + "' ;";
                command.CommandText = query;
                int count = (int)command.ExecuteScalar();
                if (count == 0)
                {
                    connection.Close();
                    return false;
                }

                query = "UPDATE Orders " +
                        "SET CreatedAt = @ca " +
                        "WHERE Id=" + order.Id + ";";

                command.CommandText = query;
                command.Parameters.Add("@ca", System.Data.SqlDbType.NVarChar).Value = order.CreatedAt;

                command.ExecuteNonQuery();
            }
            return true;
        }
        public bool Delete(int id)
        {
            var orders = new List<Order>();
            using (SqlConnection connection = new(dbconnection)) // ket noi database
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };
                connection.Open();
                string query = "SELECT COUNT(*) FROM Orders WHERE Id ='" + id + "';";
                command.CommandText = query;
                int count = (int)command.ExecuteScalar();
                if (count == 0)
                {
                    connection.Close();
                    return false;
                }
                query = "SELECT COUNT(*) FROM Users WHERE UserId ='" + id + "';";
                command.CommandText = query;
                count = (int)command.ExecuteScalar();
                if (count != 0)
                {
                    connection.Close();
                    return false;
                }
                query = "DELETE FROM Orders WHERE Id ='" + id + "';";
                command.CommandText = query;

                command.ExecuteNonQuery();
            }
            return true;
        }
    }
}

