using AutoMapper; 
using E_CommerceSystem.Models;
using E_CommerceSystem.Repositories;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Security.Cryptography;
using AutoMapper;
using E_CommerceSystem.Exceptions;

namespace E_CommerceSystem.Services
{
    public class ReviewService : IReviewService
    {
        // Dependencies
        private readonly IReviewRepo _reviewRepo;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly IOrderProductsService _orderProductsService;
        private readonly ILogger<ReviewService> _logger;
        // AutoMapper
        private readonly IMapper _mapper;
        public ReviewService(IReviewRepo reviewRepo, IProductService productService, IOrderProductsService orderProductsService, IMapper mapper , IOrderService orderService, ILogger<ReviewService> logger)
        {
            _reviewRepo = reviewRepo;
            _productService = productService;
            _orderProductsService = orderProductsService;
            _orderService = orderService;
            _mapper = mapper;
            _logger = logger;
        }

        // Get all reviews with pagination
        public IEnumerable<Review> GetAllReviews(int pageNumber, int pageSize, int pid)
        {
            // Base query
            var query = _reviewRepo.GetReviewByProductId(pid);

            // Pagination
            var pagedProducts = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return pagedProducts;
        }

        // Get review by product id and user id
        public Review GetReviewsByProductIdAndUserId(int pid, int uid)
        {
            return _reviewRepo.GetReviewsByProductIdAndUserId(pid, uid);
        }

        // Get review by id
        public Review GetReviewById(int rid)
        {
            var review = _reviewRepo.GetReviewById(rid);
            if (review == null)
                throw new KeyNotFoundException($"Review with ID {rid} not found.");
            return review;
        }

        // Get reviews by product id
        public IEnumerable<Review> GetReviewByProductId(int pid)
        {
            return _reviewRepo.GetReviewByProductId(pid);
        }
        // Add review
        public void AddReview(int uid, int pid, ReviewDTO reviewDTO)
        {
            try
            { 
            _logger.LogInformation("User {UserId} attempting to add review for product {ProductId}", uid, pid);
            // Check if the user has already added a review for this product
            var existingReview = GetReviewsByProductIdAndUserId(pid, uid);

            if (existingReview != null)
            {
                _logger.LogWarning("User {UserId} attempted to review product {ProductId} multiple times", uid, pid);
                throw new BadRequestException($"You have already reviewed this product.");
            }
            // Check if the user has purchased this product
            bool hasPurchased = false;
            var orders = _orderService.GetOrderByUserId(uid);

            foreach (var order in orders)
            {
                // Only consider delivered orders for reviews
                if (order.Status != OrderStatus.Delivered)
                    continue;

                var orderProducts = _orderProductsService.GetOrdersByOrderId(order.OID);
                if (orderProducts.Any(op => op.PID == pid))
                {
                    hasPurchased = true;
                    break;
                }
            }

            if (!hasPurchased)
            {
                _logger.LogWarning("User {UserId} attempted to review product {ProductId} without purchasing it", uid, pid);
                throw new BadRequestException("You can only review products you've purchased and received.");
            }
            // Add review
            var review = new Review
            {
                PID = pid,
                UID = uid,
                Comment = reviewDTO.Comment,
                Rating = reviewDTO.Rating,
                ReviewDate = DateTime.Now
            };
            _reviewRepo.AddReview(review);
            _logger.LogInformation("Review added successfully for product {ProductId} by user {UserId}", pid, uid);
            // Recalculate and update the product's overall rating
            RecalculateProductRating(pid);
              }
             catch (Exception ex) when(ex is not BadRequestException)
             {
                _logger.LogError(ex, "Error adding review for product {ProductId} by user {UserId}", pid, uid);
                throw new AppException($"Error adding review: {ex.Message}", 500, false);
            }
        }

        // Update review
        public void UpdateReview(int rid, ReviewDTO reviewDTO)
        {
            var review = GetReviewById(rid);

            //review.ReviewDate = DateTime.Now;
            //review.Rating = reviewDTO.Rating;
            //review.Comment = reviewDTO.Comment;
            if (review == null)
                throw new KeyNotFoundException($"Review with ID {rid} not found.");
            _mapper.Map(reviewDTO, review);
            _reviewRepo.UpdateReview(review);

            RecalculateProductRating(review.Rating);
        }

        // Delete review
        public void DeleteReview(int rid)
        {
            var review = _reviewRepo.GetReviewById(rid);
            if (review == null)
                throw new KeyNotFoundException($"Review with ID {rid} not found.");

            _reviewRepo.DeleteReview(rid);
            RecalculateProductRating(review.PID);
        }

        // Helper method to recalculate and update product rating
        private void RecalculateProductRating(int pid)
        {
            // get all reviews for the product
            var reviews = _reviewRepo.GetAllReviews();

            var product = _productService.GetProductById(pid);

            // Calculate the average rating
            var averageRating = reviews.Average(r => r.Rating);

            // Update the product's overall rating (convert double to decimal)
            product.OverallRating = Convert.ToDecimal(averageRating);

            // Save the updated product
            _productService.UpdateProduct(product);
        }


        // Helper Method to check if the user have purchased the product
        public bool HasUserPurchasedProduct(int userId, int productId)
        {
            var orders = _orderService.GetOrderByUserId(userId);

            foreach (var order in orders)
            {
                // Only consider completed orders (delivered)
                if (order.Status == OrderStatus.Delivered)
                {
                    var orderProducts = _orderProductsService.GetOrdersByOrderId(order.OID);
                    if (orderProducts.Any(op => op.PID == productId))
                    {
                        return true;
                    }
                }
            }

            return false;
        }



    }
}
