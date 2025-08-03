using System.Linq.Expressions;
using Ardalis.GuardClauses;

namespace Researcher.Application.Common.Services;

/// <summary>
/// Вспомогательные методы для работы с рефлексией и построением выражений.
/// </summary>
public static class ReflectionHelper
{
    /// <summary>
    /// Создает лямбда-выражение для доступа к свойству по его имени.
    /// </summary>
    /// <typeparam name="T">Тип объекта.</typeparam>
    /// <param name="propertyName">Имя свойства (может содержать вложенные свойства через точку, например "Address.City").</param>
    /// <returns>Лямбда-выражение типа Expression</returns>
    /// <exception cref="ArgumentException">Если propertyName пуст или null.</exception>
    public static Expression<Func<T, object>> GetPropertyExpression<T>(string propertyName)
    {
        Guard.Against.NullOrWhiteSpace(propertyName);

        var param = Expression.Parameter(typeof(T), "x");

        Expression propertyAccess = param;
        foreach (var prop in propertyName.Split('.'))
        {
            propertyAccess = Expression.PropertyOrField(propertyAccess, prop);
        }

        if (propertyAccess.Type.IsValueType)
            propertyAccess = Expression.Convert(propertyAccess, typeof(object));

        return Expression.Lambda<Func<T, object>>(propertyAccess, param);
    }
}