using System;
using Ecomm.API.Models;

namespace Ecomm.API.DataAccess
{
	public interface IPaymentService
	{
        int InsertPayment(Payment payment);
        Payment GetPayment(int id);
    }
}

