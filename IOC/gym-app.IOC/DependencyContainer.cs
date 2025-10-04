using gym_app.Business.AuthService;
using gym_app.Business.Global;
using gym_app.Business.MembershipService;
using gym_app.Business.UserService;
using gym_app.Domain.Entities;
using gym_app.Domain.Interfaces.BusinessInterface;
using gym_app.Domain.Interfaces.RepositoryInterface;
using gym_app.Infrastructure.MembershipRepository;
using gym_app.Infrastructure.Repository;
using gym_app.Infrastructure.Repository.GlobalRepository;
using gym_app.Infrastructure.Repository.UserRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;



namespace gym_app.IOC
{
    public static class DependencyContainer
    {
        //public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        //{
        //    // DbContext
        //    services.AddDbContext<ApplicationDbContext>(options =>
        //        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        //    // Identity
        //    //services.AddIdentity<ApplicationUser, IdentityRole>()
        //    //    .AddEntityFrameworkStores<ApplicationDbContext>()
        //    //    .AddDefaultTokenProviders();

        //    // Repositories / Services
        //    services.AddScoped<IAuthService, AuthService>();
        //    services.AddScoped<IAuthRepository, AuthRepository>();

        //    return services;
        //}

        public static void RegisterService(IServiceCollection services, IConfiguration configuration)
        {

            // ✅ Register your repositories
            services.AddTransient<IAuthRepository, AuthRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IMembershipRepository, MembershipRepository>();
            services.AddTransient<IRoleRepository, RoleRepository>();

            // ✅ Register your services
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRoleService, RoleService>();
            //TokenUser.RegisterService(services);
        }
    }
}
