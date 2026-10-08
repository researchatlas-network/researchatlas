namespace ResearchAtlas.Domain.Abstractions.Policies
{
    /// <summary>
    /// Represents a domain policy that encapsulates a specific behavior or strategy.
    /// </summary>
    /// <typeparam name="T">The type of object the policy operates on.</typeparam>
    public interface IPolicy<T>
    {
        /// <summary>
        /// Gets the name of the policy.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Evaluates whether the policy applies to the given object.
        /// </summary>
        /// <param name="obj">The object to evaluate.</param>
        /// <returns>true if the policy applies; otherwise, false.</returns>
        bool Applies(T obj);
    }
}
