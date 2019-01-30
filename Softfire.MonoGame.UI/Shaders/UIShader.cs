using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Softfire.MonoGame.UI.Shaders
{
     public abstract class UIShader : BasicEffect
    {
        protected UIShader(GraphicsDevice device) : this(device, Matrix.Identity, Matrix.Identity, device.Viewport.Width, device.Viewport.Height)
        {
        }

        protected UIShader(GraphicsDevice device, int width, int height) : this(device, Matrix.Identity, Matrix.Identity, width, height)
        {
        }

        protected UIShader(GraphicsDevice device, Matrix world, Matrix view, int width, int height) : base(device)
        {
            var projection = Matrix.CreateOrthographicOffCenter(0, width, height, 0, 0, 1);
            var halfPixelOffset = Matrix.CreateTranslation(-0.5f, -0.5f, 0);
            World = world;
            View = view;
            Projection = halfPixelOffset * projection;

            TextureEnabled = true;
            VertexColorEnabled = true;
        }

        protected internal UIShader(BasicEffect cloneSource) : base(cloneSource)
        {
        }
    }
}