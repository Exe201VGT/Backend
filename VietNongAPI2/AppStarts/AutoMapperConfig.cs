using AutoMapper;
using BOs.Models;
using BusinessLayer.Modal.Request;
using BusinessLayer.Modal.Response;

namespace VietNongAPI2.AppStarts
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Category, CategoryDTO>();
            CreateMap<CategoryCreateDTO, Category>();
            CreateMap<CategoryUpdateDTO, Category>();
            CreateMap<Product, ProductDTO>();
            CreateMap<ProductCreateDTO, Product>();
            CreateMap<ProductUpdateDTO, Product>();
            CreateMap<Revenue, RevenueDTO>();
            CreateMap<Order, OrderDTO>();
            CreateMap<OrderCreateDTO, Order>();
            CreateMap<OrderDetail, OrderDetailDTO>();
            CreateMap<OrderDetailCreateDTO, OrderDetail>();
            CreateMap<OrderDetailUpdateDTO, OrderDetail>();
            CreateMap<OrderHistory, OrderHistoryDTO>();
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();
            CreateMap<User, UserProfileDTO>();
            CreateMap<UserProfileUpdateDTO, User>();
            CreateMap<UserStatusUpdateDTO, User>();
            CreateMap<Seller, SellerDTO>();
            CreateMap<SellerRegisterDTO, Seller>();
            CreateMap<SellerUpdateDTO, Seller>();
            CreateMap<Cart, CartDTO>();
            CreateMap<CartItem, CartItemDTO>()
    .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))    // Map Product.Name to CartItemDTO.ProductName
    .ForMember(dest => dest.ProductImage, opt => opt.MapFrom(src => src.Product.ProductImage)); // Map Product.ProductImage to CartItemDTO.ProductImage
            CreateMap<Product, CartItemDTO>();
        }
    }
}