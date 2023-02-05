namespace Shared.Service;

/// <summary>
/// Allows access to device storage
/// </summary>
public interface IStorageService
{
    /// <summary>
    /// Get an object from the storage
    /// </summary>
    /// <typeparam name="TResult">The data type of the object</typeparam>
    /// <param name="key">The identifier name for the object</param>
    /// <param name="token">Propagates notification that operations should be canceled.</param>
    /// <returns>The object type based on the key provided. Returns the object's default value if unsuccessful.</returns>
    ValueTask<TResult> GetAsync<TResult>(string key, CancellationToken token = default);

    /// <summary>
    /// Delete an object from the storage
    /// </summary>
    /// <param name="key">The identifier name for the object</param>
    /// <param name="token">Propagates notification that operations should be canceled.</param>
    ValueTask RemoveAsync(string key, CancellationToken token = default);

    /// <summary>
    /// Store an object to the storage
    /// </summary>
    /// <typeparam name="TData">The data type of the object</typeparam>
    /// <param name="key">The identifier name for the object</param>
    /// <param name="data">The object to store</param>
    /// <param name="token">Propagates notification that operations should be canceled.</param>
    ValueTask SetAsync<TData>(string key, TData data, CancellationToken token = default);
}
