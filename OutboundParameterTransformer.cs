using System.Text.RegularExpressions;

namespace cortado;

public class OutboundParameterTransformer : IOutboundParameterTransformer
{
    public string TransformOutbound(object? value)
    {
        return value == null 
            ? null 
            : Regex.Replace(
                    value.ToString(), 
                    "([a-z])([A-Z])", 
                    "$1-$2", 
                    RegexOptions.CultureInvariant)
                .ToLowerInvariant();
    }
}