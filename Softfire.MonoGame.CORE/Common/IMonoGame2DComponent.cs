using Softfire.MonoGame.CORE.Graphics.Transforms;
using Softfire.MonoGame.CORE.Input;

namespace Softfire.MonoGame.CORE.Common
{
    /// <summary>
    /// An interface for defining 2D components.
    /// </summary>
    public interface IMonoGame2DComponent : IMonoGameIdentifierComponent, IMonoGameVisibleComponent, IMonoGameFocusComponent, IMonoGameLoadComponent,
                                            IMonoGameUpdateComponent, IMonoGameDrawComponent, IMonoGameBoundsComponent, IMonoGameActiveComponent,
                                            IMonoGameInputTabComponent, IMonoGameLayerComponent, IMonoGameParentChildComponent
    {
        /// <summary>
        /// A 2D transformation class for positioning, scaling and rotating objects.
        /// </summary>
        Transform2D Transform { get; }

        /// <summary>
        /// A movement class for manipulating an object's position.
        /// </summary>
        Movement Movement { get; }

        /// <summary>
        /// A <see cref="SizeF"/> class for defining and manipulating the object's height and width.
        /// </summary>
        SizeF Size { get; }
    }
}