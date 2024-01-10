using System;
using OptimizingLastMile.Entites;
using OptimizingLastMile.Models.LogicHandle;
using OptimizingLastMile.Models.Params.Orders;

namespace OptimizingLastMile.Services.Others;

public interface IPropertyMappingService
{
    Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();
}

public class PropertyMappingService : IPropertyMappingService
{
    private IList<IPropertyMapping> _propertyMappings = new List<IPropertyMapping>();

    public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
    {
        var matchingMapping = _propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();

        if (matchingMapping.Count() == 1)
        {
            return matchingMapping.First().MappingDictionary;
        }

        throw new Exception($"Cannot find exact property mapping instance for <{typeof(TSource)}, {typeof(TDestination)}>");
    }

    public PropertyMappingService()
    {
        _propertyMappings.Add(new PropertyMapping<OrderParam, OrderInformation>(_orders));
    }

    private Dictionary<string, PropertyMappingValue> _orders = new(StringComparer.OrdinalIgnoreCase)
        {
            { "ExpectedShippingDate", new PropertyMappingValue(new List<string> { "ExpectedShippingDate" }, isRevert: false) }
        };
}