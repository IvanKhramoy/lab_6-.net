using System.Text.Json;
using Automobiles.Domain;
using Automobiles.Mediator;

namespace Automobiles.UseCases;

/// <summary>
/// Запрос на сохранение коллекции в файл
/// </summary>
/// <param name="data">Коллекция объектов</param>
/// <param name="fileName">имя файла</param>
public sealed record SaveData(IEnumerable<Car> data, string fileName)
    : IRequest;

/// <summary>
/// Обработчик запроса
/// </summary>
public class SaveDataHandler : IRequestHandler<SaveData>
{
    public void Handle(SaveData request)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
        string json = JsonSerializer.Serialize(request.data, options);
        File.WriteAllText(request.fileName, json);
    }
}
