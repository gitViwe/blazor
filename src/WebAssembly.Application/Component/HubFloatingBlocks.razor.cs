using Microsoft.AspNetCore.Components;

namespace WebAssembly.Application.Component;

public partial class HubFloatingBlocks
{
    private const int MAX_COUNT = 15;

    /// <summary>
    /// Defines the number of floating blocks to the maximum of 15
    /// </summary>
    [Parameter] public int BlockCount { get; set; } = 10;

    /// <summary>
    /// Gets the current value of <see cref="BlockCount"/> up to the value of <see cref="MAX_COUNT"/>
    /// </summary>
    private int GetBlockCount()
    {
        return BlockCount <= MAX_COUNT ? BlockCount : MAX_COUNT;
    }
}
