namespace Blazor.Component.Manager;

public sealed class CascadingHubFeatureManagerContext
{
    public IEnumerable<string> Features { get; init; } = [];
    public bool IsFeatureEnabled(string feature) => string.IsNullOrWhiteSpace(feature) is false && Features.Contains(feature);
}