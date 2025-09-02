using AutoMapper;

namespace E_CommerceSystem
{
    public class MappingProfile : Profile 
    {

        public MappingProfile()
        {
            CreateMap<Models.UserDTO, Models.User>();
            CreateMap<Models.User, Models.UserDTO>();

            CreateMap<Models.CategoryDTO, Models.Category>();
            CreateMap<Models.Category, Models.CategoryDTO>();

            CreateMap<Models.ProductDTO, Models.Product>();
            CreateMap<Models.Product, Models.ProductDTO>();



        }
    }
}
