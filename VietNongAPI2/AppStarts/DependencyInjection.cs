using BusinessLayer.Service.Interface;
using BusinessLayer.Service;
using DataLayer.GenericRepository;
using DataLayer.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using BOs.Models;

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
            services.AddScoped<IUserService, UserServices>();




        }




        public static IServiceCollection AddWebAPIService(this IServiceCollection services)
        {
            // use DI here
           
            services.AddScoped<IAuthServices, AuthServices>();
            services.AddScoped<IUserService, UserServices>();




            // auto mapper
            services.AddAutoMapper(typeof(AutoMapperConfig).Assembly);

            services.AddHttpContextAccessor();

            return services;
        }
    }
}
