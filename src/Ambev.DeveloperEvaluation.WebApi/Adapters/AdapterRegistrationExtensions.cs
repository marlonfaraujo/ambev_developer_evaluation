using Ambev.DeveloperEvaluation.Application.Requests;
using MediatR;
using System.Reflection;

namespace Ambev.DeveloperEvaluation.WebApi.Adapters
{
    public static class AdapterRegistrationExtensions
    {
        public static IServiceCollection RegisterAllAdapterHandlers(this IServiceCollection services, Assembly assembly)
        {
            var requestTypes = assembly
                .GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .Select(t => new
                {
                    Type = t,
                    Interface = t.GetInterfaces()
                        .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestApplication<>))
                })
                .Where(x => x.Interface != null)
                .ToList();

            foreach (var req in requestTypes)
            {
                var requestType = req.Type;
                var resultType = req.Interface.GetGenericArguments()[0];

                // Cria os tipos genéricos fechados
                var adapterType = typeof(MediatRRequestAdapter<,>).MakeGenericType(requestType, resultType);
                var handlerType = typeof(MediatRRequestHandlerAdapter<,>).MakeGenericType(requestType, resultType);
                var interfaceType = typeof(IRequestHandler<,>).MakeGenericType(adapterType, resultType);

                // Registra no container
                services.AddScoped(interfaceType, handlerType);
             }

            return services;
        }

        public static IServiceCollection RegisterAllApplicationHandlers(this IServiceCollection services, Assembly assembly)
        {
            var handlers = assembly
                .GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .Select(t => new
                {
                    Implementation = t,
                    Interface = t.GetInterfaces()
                        .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestApplicationHandler<,>))
                })
                .Where(x => x.Interface != null);

            foreach (var h in handlers)
            {
                services.AddScoped(h.Interface, h.Implementation);
            }

            return services;
        }
    }
}
