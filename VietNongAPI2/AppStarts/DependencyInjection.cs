using BusinessLayer.Service.Interface;
using BusinessLayer.Service;
using DataLayer.GenericRepository;
using DataLayer.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using BOs.Models;
using DataLayer.Repository;

namespace VietNongAPI2.AppStarts
{
    public static class DependencyInjectionContainers
    {

        public static void InstallService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRouting(options =>
            {
                options.LowercaseUrls = true; ;
                options.LowercaseQueryStrings = true;
            });
            services.AddDbContext<VietNongContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DBDefault"));
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            
            // use DI here
            //services.AddScoped<IUserService, UserServices>();

        }

        public static IServiceCollection AddWebAPIService(this IServiceCollection services)
        {
            // use DI here
           
            services.AddScoped<IAuthServices, AuthServices>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IRevenueRepository, RevenueRepository>();
            services.AddScoped<IRevenueService, RevenueService>();
            services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
            services.AddScoped<IOrderDetailService, OrderDetailService>();
            services.AddScoped<IOrderHistoryRepository, OrderHistoryRepository>();
            services.AddScoped<IOrderHistoryService, OrderHistoryService>();
            services.AddScoped<ISellerRepository, SellerRepository>();
            services.AddScoped<ISellerService, SellerService>();



            // auto mapper
            services.AddAutoMapper(typeof(AutoMapperConfig).Assembly);

            services.AddHttpContextAccessor();

            return services;
        }
    }
}
