using Task.Application.Contracts;

namespace Task.Application.Extensions;

public static class GridExtensions
{
    public static Grid<T> Grid<T>(this IQueryable<T> queryable, GridParameters parameters)
    {
        return new(queryable, parameters);
    }

    public static Task<Grid<T>> GridAsync<T>(this IQueryable<T> queryable, GridParameters parameters)
    {
        return System.Threading.Tasks.Task.FromResult(new Grid<T>(queryable, parameters));
    }
}