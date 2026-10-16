namespace Automobiles.Domain;

/// <summary>
/// Класс предметной области — Автомобиль (Вариант 16)
/// </summary>
public class Car : IEquatable<Car>
{
    /// <summary>
    /// Название (марка и модель) автомобиля
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Год выпуска
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// Мощность двигателя (л.с.)
    /// </summary>
    public int Horsepower { get; set; }

    /// <summary>
    /// Является ли автомобиль электрическим
    /// </summary>
    public bool IsElectric { get; set; }

    public bool Equals(Car? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Name == other.Name
            && Year == other.Year
            && Horsepower == other.Horsepower
            && IsElectric == other.IsElectric;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Car);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Year, Horsepower, IsElectric);
    }

    public override string ToString()
    {
        return $"{Name}, {Year} г., {Horsepower} л.с., " +
               $"{(IsElectric ? "электрический" : "бензиновый/дизельный")}";
    }
}
