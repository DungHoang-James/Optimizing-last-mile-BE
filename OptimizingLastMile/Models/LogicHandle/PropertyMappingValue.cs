using System;
namespace OptimizingLastMile.Models.LogicHandle;

public class PropertyMappingValue
{
    public IEnumerable<string> DestinationProperties { get; private set; }
    public bool IsRevert { get; set; }
    public PropertyMappingValue(IEnumerable<string> destinationProperties, bool isRevert = false)
    {
        DestinationProperties = destinationProperties ?? throw new ArgumentNullException(nameof(destinationProperties));
        IsRevert = isRevert;
    }
}