namespace Softfire.MonoGame.CORE.V2.Common
{
    /// <summary>
    /// An interface for determining whether the component is active.
    /// </summary>
    public interface IMonoGameActiveComponent
    {
        /// <summary>
        /// Determines whether the component is active.
        /// </summary>
        bool IsActive { get; }
    }
}