using System;
using OptimizingLastMile.Models.LogicHandle;
using System.Linq.Dynamic.Core;

namespace OptimizingLastMile.Utils;

public static class IQueryableExtensions
{
    public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string orderBy, Dictionary<string, PropertyMappingValue> mappingDictionary)
    {
        string orderByString = "";

        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (mappingDictionary == null)
        {
            throw new ArgumentNullException(nameof(mappingDictionary));
        }

        if (string.IsNullOrWhiteSpace(orderBy))
        {
            return source;
        }

        var orderBySplit = orderBy.Split(',');

        foreach (var orderByClause in orderBySplit.Reverse())
        {
            var trimedOrderByClause = orderByClause.Trim();

            var orderDesc = trimedOrderByClause.StartsWith('-');
            var orderAsc = trimedOrderByClause.StartsWith('+');

            var propertyName = orderDesc || orderAsc ? trimedOrderByClause.Remove(0, 1) : trimedOrderByClause;

            if (mappingDictionary.ContainsKey(propertyName))
            {
                var propertyMappingValue = mappingDictionary[propertyName];

                if (propertyMappingValue == null)
                {
                    throw new ArgumentNullException("propertyMappingValue");
                }

                foreach (var desProp in propertyMappingValue.DestinationProperties)
                {
                    if (propertyMappingValue.IsRevert)
                    {
                        orderDesc = !orderDesc;
                    }

                    orderByString = orderByString + (string.IsNullOrWhiteSpace(orderByString) ? string.Empty : ", ")
                                                  + desProp
                                                  + (orderDesc ? " descending" : " ascending");
                }
            }
        }

        return string.IsNullOrWhiteSpace(orderByString) ? source : source.OrderBy(orderByString);
    }
}

