using System.Reflection;

namespace Automobiles.Mediator;

/// <summary>
/// Реализация интерфейса ISender с использованием Reflection
/// для автоматического поиска и регистрации обработчиков запросов
/// </summary>
public class Sender : ISender
{
    /// <summary>
    /// Сборка для поиска хэндлеров
    /// </summary>
    private readonly Assembly? _assembly;

    /// <summary>
    /// Словарь соответствия типа интерфейса хендлера 
    /// и типа (класса) реализации интерфейса хэндлера
    /// </summary>
    private readonly Dictionary<Type, Type> _handlerImplementations = new();

    public Sender(Assembly? assembly = null)
    {
        _assembly = assembly ?? Assembly.GetCallingAssembly();
        RegisterHandlers(_assembly);
    }

    /// <summary>
    /// Запись в словарь всех типов интерфейсов хендлеров 
    /// и их реализаций, имеющихся в сборке
    /// </summary>
    /// <param name="assembly">сборка для поиска хэндлеров</param>
    private void RegisterHandlers(Assembly assembly)
    {
        var handlerPairs = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract)
            .SelectMany(t => t.GetInterfaces()
                .Where(i => i.IsGenericType &&
                    (i.GetGenericTypeDefinition() == typeof(IRequestHandler<>) ||
                     i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)))
                .Select(i => new { InterfaceType = i, ImplementationType = t }));

        foreach (var pair in handlerPairs)
        {
            _handlerImplementations[pair.InterfaceType] = pair.ImplementationType;
        }
    }

    public void Send<TRequest>(TRequest request)
            where TRequest : IRequest
    {
        Type handlerType = typeof(IRequestHandler<>)
               .MakeGenericType(request.GetType());
        try
        {
            var handler = CreateHandler(handlerType);
            handler.Handle((dynamic)request);
        }
        catch (Exception e)
        {
            System.Console.WriteLine(e.Message);
            throw;
        }
    }

    public TResponse Send<TResponse>(IRequest<TResponse> request)
    {
        Type handlerType = typeof(IRequestHandler<,>)
               .MakeGenericType(request.GetType(), typeof(TResponse));
        try
        {
            var handler = CreateHandler(handlerType);
            return handler.Handle((dynamic)request);
        }
        catch (Exception e)
        {
            System.Console.WriteLine(e.Message);
            throw;
        }
    }

    /// <summary>
    /// Получить объект handler
    /// </summary>
    /// <param name="handlerType">Искомый тип</param>
    /// <returns>объект handler</returns>
    /// <exception cref="NotImplementedException"></exception>
    dynamic CreateHandler(Type handlerType)
    {
        if (_handlerImplementations.TryGetValue(handlerType, out var implementationType)
            && implementationType is not null)
        {
            return Activator.CreateInstance(implementationType)!;
        }
        throw new NotImplementedException("Handler not found");
    }
}
