using System.Text.Json;
using Automobiles.Domain;
using Automobiles.Mediator;

namespace Automobiles.UseCases;

/// <summary>
/// Запрос на чтение файла
/// </summary>
/// <param name="FileName">Имя файла</param>
public sealed record ReadFile(string FileName)
    : IRequest<IEnumerable<Car>>;

/// <summary>
/// Обработчик запроса чтения файла
/// </summary>
public class ReadFileHandler
    : IRequestHandler<ReadFile, IEnumerable<Car>>
{
    public IEnumerable<Car> Handle(ReadFile request)
    {
        string json = File.ReadAllText(request.FileName);
        var result = JsonSerializer.Deserialize<List<Car>>(json);
        return result ?? new List<Car>();
    }
}
