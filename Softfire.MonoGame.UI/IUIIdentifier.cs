namespace Softfire.MonoGame.UI
{
    /// <summary>
    /// An interface used for enforcing identification properties on classes.
    /// </summary>
    internal interface IUIIdentifier
    {
        /// <summary>
        /// A unique id.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// A unique name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The order number. Can be used for sorting.
        /// </summary>
        int OrderNumber { get; set; }
    }
}