namespace AoC.Library.Runner;

[AttributeUsage(AttributeTargets.Class)]
public class DateInfoAttribute : Attribute
{
    public int Year { get; }

    public int Day { get; }

    public AdventParts PartsReady { get; }

    public DateInfoAttribute(int year, int day, AdventParts partsReady)
    {
        Year = year;
        Day = day;
        PartsReady = partsReady;
    }
}