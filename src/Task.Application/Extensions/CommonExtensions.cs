namespace Task.Application.Extensions;

public static class CommonExtensions
{
    public static List<string> GetColumnNames<T>() where T : class
    {
        return typeof(T)
            .GetProperties()
            .Select(e => e.Name)
            .ToList();
    }

    public static bool ListNullOrEmpty<T>(this IEnumerable<T> list) where T : class
    {
        if (list is null)
        {
            return true;
        }

        return !list.Any();
    }
}
