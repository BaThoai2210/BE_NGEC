using System;
using Ecomm.API.Models;

namespace Ecomm.API.DataAccess
{
	public interface IPaymentMethodService : IDisposable
	{
        List<PaymentMethod> GetPaymentMethods();
        PaymentMethod GetPaymentMethodById(int id);
        bool InsertPaymentMethod(PaymentMethod method);
        bool UpdatePaymentMethod(PaymentMethod method);
        bool DeletePaymentMethod(int id);
    }
}

