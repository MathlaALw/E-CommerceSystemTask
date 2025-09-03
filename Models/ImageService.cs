namespace E_CommerceSystem.Models
{
    public class ImageService
    {
        private readonly IWebHostEnvironment _environment; // To access web hosting environment details
        private readonly IConfiguration _configuration; // To access configuration settings

        public ImageService(IWebHostEnvironment environment, IConfiguration configuration) // Constructor with dependency injection
        {
            _environment = environment; // Access to web hosting environment
            _configuration = configuration;
        }

    }
}
