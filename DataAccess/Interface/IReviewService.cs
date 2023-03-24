using System;
using Ecomm.API.Models;

namespace Ecomm.API.DataAccess
{
	public interface IReviewService
	{
        void InsertReview(Review review);
        List<Review> GetProductReviews(int productId);
	}
}

