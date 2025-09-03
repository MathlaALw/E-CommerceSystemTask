namespace E_CommerceSystem.Models
{
    public class ActiveCustomerDTO
    {
        public int UserId { get; set; } // User ID
        public string UserName { get; set; } // User Name
        public string Email { get; set; } // User Email
        public int OrderCount { get; set; } // Number of Orders
        public decimal TotalSpent { get; set; } // Total Amount Spent

    }
}
