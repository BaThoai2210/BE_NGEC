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
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration configuration;
        private readonly string dbconnection;
        private readonly string dateformat;
        private readonly IUserService userService;
        private readonly IPaymentMethodService paymentMethodService;
        public PaymentService(IConfiguration configuration, IUserService userService, IPaymentMethodService paymentMethodService)
        {
            this.configuration = configuration;
            dbconnection = this.configuration["ConnectionStrings:DB"];
            dateformat = this.configuration["Constants:DateFormat"];
            this.userService = userService;
            this.paymentMethodService = paymentMethodService;
        }

        public int InsertPayment(Payment payment)
        {
            int value = 0;
            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                string query = @"INSERT INTO Payments (PaymentMethodId, UserId, TotalAmount, ShippingCharges, AmountReduced, AmountPaid, CreatedAt) 
                                VALUES (@pmid, @uid, @ta, @sc, @ar, @ap, @cat);";

                command.CommandText = query;
                command.Parameters.Add("@pmid", System.Data.SqlDbType.Int).Value = payment.PaymentMethod.Id;
                command.Parameters.Add("@uid", System.Data.SqlDbType.Int).Value = payment.User.Id;
                command.Parameters.Add("@ta", System.Data.SqlDbType.NVarChar).Value = payment.TotalAmount;
                command.Parameters.Add("@sc", System.Data.SqlDbType.NVarChar).Value = payment.ShipingCharges;
                command.Parameters.Add("@ar", System.Data.SqlDbType.NVarChar).Value = payment.AmountReduced;
                command.Parameters.Add("@ap", System.Data.SqlDbType.NVarChar).Value = payment.AmountPaid;
                command.Parameters.Add("@cat", System.Data.SqlDbType.NVarChar).Value = payment.CreatedAt;

                connection.Open();
                value = command.ExecuteNonQuery();

                if (value > 0)
                {
                    query = "SELECT TOP 1 Id FROM Payments ORDER BY Id DESC;";
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
        public Payment GetPayment(int id)
        {
            var payment = new Payment();
            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                string query = "SELECT * FROM Payments WHERE UserId=" + id + ";";
                command.CommandText = query;

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    payment.Id = (int)reader["Id"];
                    payment.User = userService.GetUser((int)reader["UserId"]);
                    payment.PaymentMethod = paymentMethodService.GetPaymentMethodById((int)reader["PaymentMethodId"]);
                    payment.TotalAmount = (int)reader["TotalAmount"];
                    payment.ShipingCharges = (int)reader["ShipingCharges"];
                    payment.AmountReduced = (int)reader["AmountReduced"];
                    payment.AmountPaid = (int)reader["AmountPaid"];
                    payment.CreatedAt = (string)reader["CreatedAt"];
                }
            }
            return payment;
        }
    }
}

