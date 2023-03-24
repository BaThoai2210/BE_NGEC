using System;
using Ecomm.API.Models;
namespace Ecomm.API.DataAccess
{
	public interface IUserService
	{
		bool InsertUser(User user);
        string IsUserPresent(string email, string password);
		List<User> GetAllUser();
        User GetUser(int id);
		bool Update(User user);
		bool Delete(int id);
    }
}

