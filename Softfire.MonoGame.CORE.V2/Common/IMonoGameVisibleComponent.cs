namespace Softfire.MonoGame.CORE.V2.Common
{
    /// <summary>
    /// An interface for determining whether the component is visible.
    /// </summary>
    public interface IMonoGameVisibleComponent
    {
        /// <summary>
        /// Determines whether the object is visible.
        /// </summary>
        bool IsVisible { get; set; }
    }
}