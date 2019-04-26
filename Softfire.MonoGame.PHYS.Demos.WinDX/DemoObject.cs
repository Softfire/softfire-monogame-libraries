using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Softfire.MonoGame.CORE.Common;

namespace Softfire.MonoGame.PHYS.Demos.WinDX
{
    public class DemoObject : MonoGameObject
    {
        private Texture2D Texture { get; set; }

        private string TexturePath { get; set; }

        public DemoObject(MonoGameObject parent, int id, string name, string texturePath, Vector2 position = default,
                          int width = 10, int height = 10, bool isVisible = true) : base(parent, id, name, position, width, height, isVisible)
        {
            TexturePath = texturePath;
        }

        public override void LoadContent(ContentManager content = null)
        {
            if (!string.IsNullOrWhiteSpace(TexturePath) &&
                content != null)
            {
                Texture = content.Load<Texture2D>(TexturePath);
            }

            base.LoadContent(content);
        }

        public override void Draw(SpriteBatch spriteBatch, Matrix transform = default)
        {
            spriteBatch.Draw(Texture, Transform.WorldPosition(), Color.White);

            base.Draw(spriteBatch, transform);
        }
    }
}