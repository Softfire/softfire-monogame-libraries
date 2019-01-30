namespace Softfire.MonoGame.CORE.Graphics.Drawing
{
    /// <summary>
    /// Available draw layers.
    /// </summary>
    public enum Layers
    {
        /// <summary>
        /// The base layer. Backgrounds, outlines and borders.
        /// </summary>
        Base,
        /// <summary>
        /// The text layer. For text.
        /// </summary>
        Text,
        /// <summary>
        /// The buttons layer. For buttons.
        /// </summary>
        Buttons,
        /// <summary>
        /// The middle layer. For content that should be drawn over text and buttons.
        /// </summary>
        Middle,
        /// <summary>
        /// The top layer. For any content that should be drawn over all else.
        /// </summary>
        Top
    }
}