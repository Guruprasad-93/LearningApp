using gym_app.Business.AuthService;
using gym_app.Business.UserService;
using gym_app.Domain.Interfaces.BusinessInterface;
using gym_app.Domain.Interfaces.RepositoryInterface;
using gym_app.Infrastructure.Repository;
using gym_app.Infrastructure.Repository.UserRepository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gym_app.IOC
{
    public static class TokenUser
    {
        public static void RegisterService(IServiceCollection serviceCollection)
        {
            RegisterAuthService(serviceCollection);
            RegisterAuthRepository(serviceCollection);
            RegisterUserService(serviceCollection);
            RegisterUserRepository(serviceCollection);
        }
        private static void RegisterAuthService(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IAuthService, AuthService>();
        }

        private static void RegisterAuthRepository(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IAuthRepository, AuthRepository>();
        }

        private static void RegisterUserService(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IUserService, UserService>();
        }
        private static void RegisterUserRepository(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IUserRepository, UserRepository>();
        }

    }
}
