using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input.InputListeners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JohnCricketFishingGame.Source
{
    public class GameInput
    {
        public readonly GamePadListener gamePadListener;
        public readonly KeyboardListener keyboardListener;

        public GameInput()
        {
            gamePadListener = new GamePadListener();
            keyboardListener = new KeyboardListener();
            
        }

        /// <summary>
        /// Only used for selected fish movement
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Vector2 HandleInput(Keys key)
        {
            switch (key)
            {
                case Keys.Up:
                case Keys.W:
                    return -Vector2.UnitY;
                case Keys.Down:
                case Keys.S:
                    return Vector2.UnitY;
                case Keys.Left:
                case Keys.A:
                    return -Vector2.UnitX;
                case Keys.Right:
                case Keys.D:
                    return Vector2.UnitX;
                default:
                    return Vector2.Zero;
            }
        }


    }
}
