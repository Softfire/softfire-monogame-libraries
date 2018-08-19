namespace Softfire.MonoGame.UI
{
    internal interface IUIIdentifier
    {
        /// <summary>
        /// Id.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Order Number.
        /// </summary>
        int OrderNumber { get; set; }
    }
}