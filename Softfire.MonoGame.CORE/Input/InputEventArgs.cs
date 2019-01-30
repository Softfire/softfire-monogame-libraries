using System;
using Microsoft.Xna.Framework;

namespace Softfire.MonoGame.CORE.Input
{
    /// <summary>
    /// Input event arguments that can be passed to subscribed objects.
    /// </summary>
    public class InputEventArgs : EventArgs
    {
        /// <summary>
        /// Creates an empty instance of an <see cref="InputEventArgs"/>.
        /// </summary>
        public new static InputEventArgs Empty => new InputEventArgs();

        /// <summary>
        /// Passes the player index to the subscribed objects.
        /// </summary>
        public int PlayerIndex { get; set; }

        /// <summary>
        /// Passes the input <see cref="string"/> to the subscribed objects.
        /// </summary>
        public string InputString { get; set; }

        /// <summary>
        /// Passes the input <see cref="int"/> to the subscribed objects.
        /// </summary>
        public int InputInteger { get; set; }

        /// <summary>
        /// Passes the input <see cref="float"/> to the subscribed objects.
        /// </summary>
        public float InputFloat { get; set; }

        /// <summary>
        /// Passes the input <see cref="double"/> to the subscribed objects.
        /// </summary>
        public double InputDouble { get; set; }

        /// <summary>
        /// Passes the input's minimum length to the subscribed objects.
        /// </summary>
        public int MinLength { get; set; }

        /// <summary>
        /// Passes the input's maximum length to the subscribed objects.
        /// </summary>
        public int MaxLength { get; set; }

        /// <summary>
        /// Passes the input's rotation to the subscribed objects.
        /// </summary>
        public double InputRotation { get; set; }

        /// <summary>
        /// Passes the input device's movement deltas to the subscribed objects.
        /// </summary>
        public Vector2 InputDeltas { get; set; }

        /// <summary>
        /// Passes the input device's scroll velocity to the subscribed objects.
        /// </summary>
        public Vector2 InputScrollVelocity { get; set; }

        /// <summary>
        /// Passes the input device's rectangle to the subscribed objects.
        /// </summary>
        public RectangleF InputRectangle { get; set; }

        /// <summary>
        /// Passes the input tab order id to the subscribed objects.
        /// </summary>
        public int InputTabOrderId { get; set; }

        /// <summary>
        /// Passes the available input device flags to the subscribed objects. 
        /// </summary>
        public InputFlags InputFlags { get; }

        /// <summary>
        /// Passes the available input device states to the subscribed objects. 
        /// </summary>
        public InputStates InputStates { get; }

        /// <summary>
        /// An arguments class for event driven input.
        /// </summary>
        public InputEventArgs()
        {
            PlayerIndex = 1;
            InputString = string.Empty;
            MinLength = int.MinValue;
            MaxLength = int.MaxValue;
            InputDeltas = Vector2.Zero;
            InputRectangle = RectangleF.Empty;
            InputTabOrderId = 0;
            InputFlags = new InputFlags();
            InputStates = new InputStates();
        }

        /// <summary>
        /// Clears all values to their defaults.
        /// </summary>
        /// <param name="retainInputRectangleHistory">Determines whether to retain the last <see cref="InputRectangle"/> entry or clear it on calling <see cref="Clear(bool)"/>.</param>
        public void Clear(bool retainInputRectangleHistory = true)
        {
            InputDeltas = Vector2.Zero;
            InputDouble = 0d;
            InputFlags.Clear();
            InputFloat = 0f;
            InputInteger = 0;
            InputRotation = 0d;
            InputTabOrderId = 0;
            InputStates.Clear();
            InputString = string.Empty;
            PlayerIndex = 1;

            if (retainInputRectangleHistory)
            {
                InputRectangle = RectangleF.Empty;
            }
        }

        /// <summary>
        /// Copies the input of the passed in <see cref="InputEventArgs"/>.
        /// </summary>
        /// <param name="input">The <see cref="InputEventArgs"/> to copy.</param>
        public void Copy(InputEventArgs input)
        {
            PlayerIndex = input.PlayerIndex;
            InputString = input.InputString;
            InputInteger = input.InputInteger;
            InputFloat = input.InputFloat;
            InputDouble = input.InputDouble;
            MinLength = input.MinLength;
            MaxLength = input.MaxLength;
            InputRotation = input.InputRotation;
            InputDeltas = input.InputDeltas;
            InputScrollVelocity = input.InputScrollVelocity;
            InputRectangle = input.InputRectangle;
            InputTabOrderId = input.InputTabOrderId;
            InputFlags.Copy(input.InputFlags);
            InputStates.Copy(input.InputStates);
        }
    }
}