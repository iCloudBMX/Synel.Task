using System.Linq.Expressions;

namespace Task.Application.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> Filter<T>(this IQueryable<T> queryable, string property, object value)
    {
        return queryable.Filter(property, string.Empty, value);
    }

    public static IQueryable<T> Filter<T>(this IQueryable<T> queryable, string property, string comparison,
        object value)
    {
        if (string.IsNullOrWhiteSpace(property) || value is null || string.IsNullOrWhiteSpace(value.ToString()))
            return queryable;

        var parameter = Expression.Parameter(typeof(T));

        var left = Create(property, parameter);

        try
        {
            var propertyInfo = typeof(T).GetProperty(property);

            var type = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;

            value = type == typeof(DateTime)
                ? Change(DateTime.Parse(value.ToString()!), type)
                : type == typeof(Guid)
                    ? Change(Guid.Parse(value.ToString() ?? string.Empty), type)
                    : Change(value, type);
        }
        catch
        {
            return Enumerable.Empty<T>().AsQueryable();
        }

        var right = Expression.Constant(value, left.Type);

        var body = Create(left, comparison, right);

        var expression = Expression.Lambda<Func<T, bool>>(body, parameter);

        return queryable.Where(expression);
    }

    public static IQueryable<T> Order<T>(this IQueryable<T> queryable, string property, bool ascending)
    {
        if (queryable is null || string.IsNullOrWhiteSpace(property)) return queryable;

        var parameter = Expression.Parameter(typeof(T));

        var body = Create(property, parameter);

        var expression = (dynamic)Expression.Lambda(body, parameter);

        return ascending
            ? Queryable.OrderBy(queryable, expression)
            : Queryable.OrderByDescending(queryable, expression);
    }

    public static IQueryable<T> Page<T>(this IQueryable<T> queryable, int index, int size)
    {
        if (queryable is null || index <= 0 || size <= 0) return queryable;

        return queryable.Skip((index - 1) * size).Take(size);
    }

    private static object Change(object value, Type type)
    {
        if (type.BaseType == typeof(Enum))
        {
            value = Enum.Parse(type, value.ToString());
        }

        return Convert.ChangeType(value, type);
    }

    private static Expression Create(string property, Expression parameter)
    {
        return property.Split('.').Aggregate(parameter, Expression.Property);
    }

    private static Expression Create(Expression left, string comparison, Expression right)
    {
        if (string.IsNullOrWhiteSpace(comparison) && left.Type == typeof(string))
        {
            var leftToLower = Expression.Call(left, nameof(string.ToLower), Type.EmptyTypes);
            var rightToLower = Expression.Call(right, nameof(string.ToLower), Type.EmptyTypes);

            return Expression.Call(leftToLower, nameof(string.Contains), Type.EmptyTypes, rightToLower);
        }

        var type = comparison switch
        {
            "<" => ExpressionType.LessThan,
            "<=" => ExpressionType.LessThanOrEqual,
            ">" => ExpressionType.GreaterThan,
            ">=" => ExpressionType.GreaterThanOrEqual,
            "!=" => ExpressionType.NotEqual,
            _ => ExpressionType.Equal
        };

        return Expression.MakeBinary(type, left, right);
    }
}