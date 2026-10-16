namespace Automobiles.Mediator;

/// <summary>
/// Интерфейс медиатора (отправителя сообщений)
/// </summary>
public interface ISender
{
    /// <summary>
    /// Обработка запроса 
    /// </summary>
    /// <typeparam name="TRequest">Тип запроса</typeparam>
    /// <param name="request">Объект запроса</param>
    void Send<TRequest>(TRequest request)
            where TRequest : IRequest;

    /// <summary>
    /// Обработка запроса, возвращающего данные
    /// </summary>
    /// <typeparam name="TResponse">Тип возвращаемых данных</typeparam>
    /// <param name="request">Объект запроса</param>
    /// <returns>значение типа TResponse</returns>
    TResponse Send<TResponse>(IRequest<TResponse> request);
}
