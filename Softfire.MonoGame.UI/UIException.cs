using System;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Softfire.MonoGame.LOG;

namespace Softfire.MonoGame.UI
{
    public class UIException : Exception
    {
        /// <summary>
        /// Logger.
        /// </summary>
        public Logger Logger { get; }

        public UIException(LogTypes logType, string message)
        {
            Logger = new Logger(@"Config\Logs\UI");
            Logger.Write(logType, message, useInlineLayout: false);
        }

        public async Task Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}