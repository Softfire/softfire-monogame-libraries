namespace Softfire.MonoGame.CORE.Common
{
    /// <summary>
    /// An interface for defining layers in components.
    /// </summary>
    public interface IMonoGameLayerComponent
    {
        /// <summary>
        /// The component's current layer.
        /// </summary>
        int Layer { get; set; }
    }
}