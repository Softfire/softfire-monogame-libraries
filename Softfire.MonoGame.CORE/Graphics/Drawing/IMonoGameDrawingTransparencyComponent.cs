namespace Softfire.MonoGame.CORE.Graphics.Drawing
{
    /// <summary>
    /// An interface for controlling the transparency of 2D objects.
    /// </summary>
    interface IMonoGameDrawingTransparencyComponent
    {
        /// <summary>
        /// The level of transparency.
        /// </summary>
        /// <remarks>Generally from 0 (invisible) to 1 (visible).</remarks>
        float Level { get; set; }
    }
}