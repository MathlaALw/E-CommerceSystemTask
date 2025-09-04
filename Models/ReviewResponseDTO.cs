namespace E_CommerceSystem.Models
{
    public class ReviewResponseDTO
    {
        public int ReviewID { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
    }
}
