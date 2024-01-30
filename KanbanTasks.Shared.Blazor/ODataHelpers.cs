using Microsoft.AspNetCore.Components.QuickGrid;
using System.Reflection;
using System.Runtime.Serialization;

namespace KanbanTasks.Shared.Blazor;

internal static class ODataHelpers
{
    private static string GetDataMemberName<T>(string propertyName)
    {
        return (typeof(T)
            .GetProperty(propertyName)?
            .GetCustomAttribute<DataMemberAttribute>()?
            .Name
            ?? propertyName)
            .Replace(".", "/");
    }

    public static string? GetOrderBy<T>(IReadOnlyCollection<SortedProperty> sortByProperties)
    {
        if (sortByProperties.Count == 0)
        {
            return null;
        }

        return string.Join(", ", sortByProperties.Select(x => $"{GetDataMemberName<T>(x.PropertyName)} {(x.Direction == SortDirection.Descending ? "desc" : "asc")}"));
    }
}
