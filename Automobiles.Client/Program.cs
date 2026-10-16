using System.Reflection;
using Automobiles.Domain;
using Automobiles.Mediator;
using Automobiles.UseCases;

// a) Создание коллекции объектов класса Car
var cars = new List<Car>
{
    new Car { Name = "Toyota Camry", Year = 2023, Horsepower = 203, IsElectric = false },
    new Car { Name = "Tesla Model 3", Year = 2024, Horsepower = 283, IsElectric = true },
    new Car { Name = "BMW X5", Year = 2022, Horsepower = 335, IsElectric = false },
    new Car { Name = "Hyundai Ioniq 5", Year = 2024, Horsepower = 320, IsElectric = true },
    new Car { Name = "Lada Vesta", Year = 2021, Horsepower = 113, IsElectric = false }
};

// b) Создание экземпляра ISender с передачей сборки UseCases
ISender sender = new Sender(Assembly.GetAssembly(typeof(SaveData)));

const string fileName = "cars.json";

// c) Запись коллекции в файл с помощью ISender
sender.Send(new SaveData(cars, fileName));
Console.WriteLine($"Коллекция автомобилей сохранена в файл '{fileName}'.");

// d) Чтение данных из файла с помощью ISender
var loadedCars = sender.Send<IEnumerable<Car>>(new ReadFile(fileName));
Console.WriteLine($"\nКоллекция автомобилей, прочитанная из файла '{fileName}':");

foreach (var car in loadedCars)
{
    Console.WriteLine($"  {car}");
}

// e) Сравнение коллекций
var loadedList = loadedCars.ToList();
bool areEqual = cars.Count == loadedList.Count
    && cars.Zip(loadedList, (a, b) => a.Equals(b)).All(x => x);

Console.WriteLine();
if (areEqual)
{
    Console.WriteLine("✓ Коллекции равны: данные из файла совпадают с исходными.");
}
else
{
    Console.WriteLine("✗ Коллекции НЕ равны: данные из файла отличаются от исходных.");
}
