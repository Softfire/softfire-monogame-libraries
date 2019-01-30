using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Softfire.MonoGame.CORE.Input;

namespace Softfire.MonoGame.IO
{
    /// <summary>
    /// A Keyboard class for using an installed keyboard.
    /// </summary>
    public partial class IOKeyboard : IMonoGameInputComponent
    {
        /// <summary>
        /// The time between updates.
        /// </summary>
        private double DeltaTime { get; set; }

        /// <summary>
        /// The elapsed time for the keyboard.
        /// </summary>
        private double ElapsedTime { get; set; }

        /// <summary>
        /// The current key state contains all data read from the currently attached keyboard.
        /// </summary>
        private KeyboardState KeyState { get; set; }

        /// <summary>
        /// The previous key state contains all the data from the KeyState during the last update cycle.
        /// </summary>
        /// <remarks> This state is used to determine the state of keys by comparing the KeyState and PreviousKeyState.</remarks>
        private KeyboardState PreviousKeyState { get; set; }

        #region Key State Detection

        /// <summary>
        /// Determines whether a key is in a pressed state and sets the provided flag if true.
        /// </summary>
        /// <param name="key">The <see cref="Keys"/> to check if it is in a pressed state.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        public bool KeyPress(Keys key) => KeyState.IsKeyDown(key) && PreviousKeyState.IsKeyUp(key);

        /// <summary>
        /// Determines whether a key is in a released state and sets the provided flag if true.
        /// </summary>
        /// <param name="key">The <see cref="Keys"/> to check if it is in a released state.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        public bool KeyRelease(Keys key) => KeyState.IsKeyUp(key) && PreviousKeyState.IsKeyDown(key);

        /// <summary>
        /// Determines whether a key is in a held state and sets the provided flag if true.
        /// </summary>
        /// <param name="key">The <see cref="Keys"/> to check if it is in a held state.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        public bool KeyHeld(Keys key) => KeyState.IsKeyDown(key) && PreviousKeyState.IsKeyDown(key);

        #endregion

        #region Input Keyboard Function Keys
            
        #region F1

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool F1KeyPress() => KeyPress(Keys.F1);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool F1KeyRelease() => KeyRelease(Keys.F1);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool F1KeyHeld() => KeyHeld(Keys.F1);

        #endregion

        #region F2

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool F2KeyPress() => KeyPress(Keys.F2);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool F2KeyRelease() => KeyRelease(Keys.F2);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool F2KeyHeld() => KeyHeld(Keys.F2);

        #endregion

        #region F3

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool F3KeyPress() => KeyPress(Keys.F3);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool F3KeyRelease() => KeyRelease(Keys.F3);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool F3KeyHeld() => KeyHeld(Keys.F3);

        #endregion

        #region F4

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool F4KeyPress() => KeyPress(Keys.F4);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool F4KeyRelease() => KeyRelease(Keys.F4);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool F4KeyHeld() => KeyHeld(Keys.F4);

        #endregion

        #region F5

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool F5KeyPress() => KeyPress(Keys.F5);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool F5KeyRelease() => KeyRelease(Keys.F5);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool F5KeyHeld() => KeyHeld(Keys.F5);

        #endregion

        #region F6

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool F6KeyPress() => KeyPress(Keys.F6);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool F6KeyRelease() => KeyRelease(Keys.F6);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool F6KeyHeld() => KeyHeld(Keys.F6);

        #endregion

        #region F7

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool F7KeyPress() => KeyPress(Keys.F7);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool F7KeyRelease() => KeyRelease(Keys.F7);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool F7KeyHeld() => KeyHeld(Keys.F7);

        #endregion

        #region F8

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool F8KeyPress() => KeyPress(Keys.F8);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool F8KeyRelease() => KeyRelease(Keys.F8);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool F8KeyHeld() => KeyHeld(Keys.F8);

        #endregion

        #region F9

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool F9KeyPress() => KeyPress(Keys.F9);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool F9KeyRelease() => KeyRelease(Keys.F9);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool F9KeyHeld() => KeyHeld(Keys.F9);

        #endregion

        #region F10

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool F10KeyPress() => KeyPress(Keys.F10);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool F10KeyRelease() => KeyRelease(Keys.F10);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool F10KeyHeld() => KeyHeld(Keys.F10);

        #endregion

        #region F11

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool F11KeyPress() => KeyPress(Keys.F11);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool F11KeyRelease() => KeyRelease(Keys.F11);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool F11KeyHeld() => KeyHeld(Keys.F11);

        #endregion

        #region F12

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool F12KeyPress() => KeyPress(Keys.F12);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool F12KeyRelease() => KeyRelease(Keys.F12);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool F12KeyHeld() => KeyHeld(Keys.F12);

        #endregion

        #endregion

        #region Input Keyboard NumPad Keys
        
        #region Num Lock

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool NumLockKeyPress() => KeyPress(Keys.NumLock);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool NumLockKeyRelease() => KeyRelease(Keys.NumLock);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool NumLockKeyHeld() => KeyHeld(Keys.NumLock);

        #endregion

        #region Divide

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool DivideKeyPress() => KeyPress(Keys.Divide);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool DivideKeyRelease() => KeyRelease(Keys.Divide);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool DivideKeyHeld() => KeyHeld(Keys.Divide);

        #endregion

        #region Multiply

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool MultiplyKeyPress() => KeyPress(Keys.Multiply);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool MultiplyKeyRelease() => KeyRelease(Keys.Multiply);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool MultiplyKeyHeld() => KeyHeld(Keys.Multiply);

        #endregion

        #region Subtract

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool SubtractKeyPress() => KeyPress(Keys.Subtract);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool SubtractKeyRelease() => KeyRelease(Keys.Subtract);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool SubtractKeyHeld() => KeyHeld(Keys.Subtract);

        #endregion

        #region Add

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool AddKeyPress() => KeyPress(Keys.Add);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool AddKeyRelease() => KeyRelease(Keys.Add);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool AddKeyHeld() => KeyHeld(Keys.Add);

        #endregion

        #region Num Pad 7

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool NumPad7KeyPress() => KeyPress(Keys.NumPad7);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool NumPad7KeyRelease() => KeyRelease(Keys.NumPad7);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool NumPad7KeyHeld() => KeyHeld(Keys.NumPad7);

        #endregion

        #region Num Pad 8

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool NumPad8KeyPress() => KeyPress(Keys.NumPad8);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool NumPad8KeyRelease() => KeyRelease(Keys.NumPad8);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool NumPad8KeyHeld() => KeyHeld(Keys.NumPad8);

        #endregion

        #region Num Pad 9

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool NumPad9KeyPress() => KeyPress(Keys.NumPad9);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool NumPad9KeyRelease() => KeyRelease(Keys.NumPad9);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool NumPad9KeyHeld() => KeyHeld(Keys.NumPad9);

        #endregion

        #region Num Pad 4

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool NumPad4KeyPress() => KeyPress(Keys.NumPad4);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool NumPad4KeyRelease() => KeyRelease(Keys.NumPad4);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool NumPad4KeyHeld() => KeyHeld(Keys.NumPad4);

        #endregion

        #region Num Pad 5

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool NumPad5KeyPress() => KeyPress(Keys.NumPad5);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool NumPad5KeyRelease() => KeyRelease(Keys.NumPad5);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool NumPad5KeyHeld() => KeyHeld(Keys.NumPad5);

        #endregion

        #region Num Pad 6

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool NumPad6KeyPress() => KeyPress(Keys.NumPad6);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool NumPad6KeyRelease() => KeyRelease(Keys.NumPad6);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool NumPad6KeyHeld() => KeyHeld(Keys.NumPad6);

        #endregion

        #region Num Pad 1

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool NumPad1KeyPress() => KeyPress(Keys.NumPad1);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool NumPad1KeyRelease() => KeyRelease(Keys.NumPad1);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool NumPad1KeyHeld() => KeyHeld(Keys.NumPad1);

        #endregion

        #region Num Pad 2

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool NumPad2KeyPress() => KeyPress(Keys.NumPad2);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool NumPad2KeyRelease() => KeyRelease(Keys.NumPad2);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool NumPad2KeyHeld() => KeyHeld(Keys.NumPad2);

        #endregion

        #region Num Pad 3

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool NumPad3KeyPress() => KeyPress(Keys.NumPad3);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool NumPad3KeyRelease() => KeyRelease(Keys.NumPad3);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool NumPad3KeyHeld() => KeyHeld(Keys.NumPad3);

        #endregion

        #region Num Pad 0

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool NumPad0KeyPress() => KeyPress(Keys.NumPad0);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool NumPad0KeyRelease() => KeyRelease(Keys.NumPad0);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool NumPad0KeyHeld() => KeyHeld(Keys.NumPad0);

        #endregion

        #region Decimal

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool DecimalKeyPress() => KeyPress(Keys.Decimal);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool DecimalKeyRelease() => KeyRelease(Keys.Decimal);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool DecimalKeyHeld() => KeyHeld(Keys.Decimal);

        #endregion

        #endregion

        #region Input Keyboard Number Keys

        #region D1

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool D1KeyPress() => KeyPress(Keys.D1);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool D1KeyRelease() => KeyRelease(Keys.D1);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool D1KeyHeld() => KeyHeld(Keys.D1);

        #endregion

        #region D2

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool D2KeyPress() => KeyPress(Keys.D2);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool D2KeyRelease() => KeyRelease(Keys.D2);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool D2KeyHeld() => KeyHeld(Keys.D2);

        #endregion

        #region D3

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool D3KeyPress() => KeyPress(Keys.D3);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool D3KeyRelease() => KeyRelease(Keys.D3);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool D3KeyHeld() => KeyHeld(Keys.D3);

        #endregion

        #region D4

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool D4KeyPress() => KeyPress(Keys.D4);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool D4KeyRelease() => KeyRelease(Keys.D4);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool D4KeyHeld() => KeyHeld(Keys.D4);

        #endregion

        #region D5

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool D5KeyPress() => KeyPress(Keys.D5);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool D5KeyRelease() => KeyRelease(Keys.D5);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool D5KeyHeld() => KeyHeld(Keys.D5);

        #endregion

        #region D6

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool D6KeyPress() => KeyPress(Keys.D6);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool D6KeyRelease() => KeyRelease(Keys.D6);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool D6KeyHeld() => KeyHeld(Keys.D6);

        #endregion

        #region D7

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool D7KeyPress() => KeyPress(Keys.D7);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool D7KeyRelease() => KeyRelease(Keys.D7);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool D7KeyHeld() => KeyHeld(Keys.D7);

        #endregion

        #region D8

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool D8KeyPress() => KeyPress(Keys.D8);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool D8KeyRelease() => KeyRelease(Keys.D8);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool D8KeyHeld() => KeyHeld(Keys.D8);

        #endregion

        #region D9

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool D9KeyPress() => KeyPress(Keys.D9);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool D9KeyRelease() => KeyRelease(Keys.D9);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool D9KeyHeld() => KeyHeld(Keys.D9);

        #endregion

        #region D0

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool D0KeyPress() => KeyPress(Keys.D0);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool D0KeyRelease() => KeyRelease(Keys.D0);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool D0KeyHeld() => KeyHeld(Keys.D0);

        #endregion

        #endregion

        #region Input Keyboard Command Keys

        #region Escape

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool EscapeKeyPress() => KeyPress(Keys.Escape);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool EscapeKeyRelease() => KeyRelease(Keys.Escape);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool EscapeKeyHeld() => KeyHeld(Keys.Escape);

        #endregion

        #region Print Screen

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool PrintScreenKeyPress() => KeyPress(Keys.PrintScreen);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool PrintScreenKeyRelease() => KeyRelease(Keys.PrintScreen);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool PrintScreenKeyHeld() => KeyHeld(Keys.PrintScreen);

        #endregion

        #region Pause

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool PauseKeyPress() => KeyPress(Keys.Pause);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool PauseKeyRelease() => KeyRelease(Keys.Pause);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool PauseKeyHeld() => KeyHeld(Keys.Pause);

        #endregion

        #region Scroll Lock

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool ScrollLockKeyPress() => KeyPress(Keys.Scroll);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool ScrollLockKeyRelease() => KeyRelease(Keys.Scroll);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool ScrollLockKeyHeld() => KeyHeld(Keys.Scroll);

        #endregion

        #region Oem Tilde

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool OemTildeKeyPress() => KeyPress(Keys.OemTilde);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool OemTildeKeyRelease() => KeyRelease(Keys.OemTilde);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool OemTildeKeyHeld() => KeyHeld(Keys.OemTilde);

        #endregion

        #region Oem Minus

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool OemMinusKeyPress() => KeyPress(Keys.OemMinus);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool OemMinusKeyRelease() => KeyRelease(Keys.OemMinus);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool OemMinusKeyHeld() => KeyHeld(Keys.OemMinus);

        #endregion

        #region Oem Plus

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool OemPlusKeyPress() => KeyPress(Keys.OemPlus);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool OemPlusKeyRelease() => KeyRelease(Keys.OemPlus);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool OemPlusKeyHeld() => KeyHeld(Keys.OemPlus);

        #endregion

        #region Backspace

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool BackspaceKeyPress() => KeyPress(Keys.Back);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool BackspaceKeyRelease() => KeyRelease(Keys.Back);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool BackspaceKeyHeld() => KeyHeld(Keys.Back);

        #endregion

        #region Tab

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool TabKeyPress() => KeyPress(Keys.Tab);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool TabKeyRelease() => KeyRelease(Keys.Tab);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool TabKeyHeld() => KeyHeld(Keys.Tab);

        #endregion

        #region Oem Open Brackets

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool OemOpenBracketsKeyPress() => KeyPress(Keys.OemOpenBrackets);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool OemOpenBracketsKeyRelease() => KeyRelease(Keys.OemOpenBrackets);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool OemOpenBracketsKeyHeld() => KeyHeld(Keys.OemOpenBrackets);

        #endregion

        #region Oem Close Brackets

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool OemCloseBracketsKeyPress() => KeyPress(Keys.OemCloseBrackets);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool OemCloseBracketsKeyRelease() => KeyRelease(Keys.OemCloseBrackets);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool OemCloseBracketsKeyHeld() => KeyHeld(Keys.OemCloseBrackets);

        #endregion

        #region Oem Pipe

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool OemPipeKeyPress() => KeyPress(Keys.OemPipe);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool OemPipeKeyRelease() => KeyRelease(Keys.OemPipe);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool OemPipeKeyHeld() => KeyHeld(Keys.OemPipe);

        #endregion

        #region Caps Lock

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool CapsLockKeyPress() => KeyPress(Keys.CapsLock);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool CapsLockKeyRelease() => KeyRelease(Keys.CapsLock);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool CapsLockKeyHeld() => KeyHeld(Keys.CapsLock);

        #endregion

        #region Oem Semicolon

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool OemSemicolonKeyPress() => KeyPress(Keys.OemSemicolon);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool OemSemicolonKeyRelease() => KeyRelease(Keys.OemSemicolon);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool OemSemicolonKeyHeld() => KeyHeld(Keys.OemSemicolon);

        #endregion

        #region Oem Quotes

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool OemQuotesKeyPress() => KeyPress(Keys.OemQuotes);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool OemQuotesKeyRelease() => KeyRelease(Keys.OemQuotes);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool OemQuotesKeyHeld() => KeyHeld(Keys.OemQuotes);

        #endregion

        #region Enter

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool EnterKeyPress() => KeyPress(Keys.Enter);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool EnterKeyRelease() => KeyRelease(Keys.Enter);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool EnterKeyHeld() => KeyHeld(Keys.Enter);

        #endregion

        #region Left Shift

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool LeftShiftKeyPress() => KeyPress(Keys.LeftShift);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool LeftShiftKeyRelease() => KeyRelease(Keys.LeftShift);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool LeftShiftKeyHeld() => KeyHeld(Keys.LeftShift);

        #endregion

        #region Oem Comma

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool OemCommaKeyPress() => KeyPress(Keys.OemComma);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool OemCommaKeyRelease() => KeyRelease(Keys.OemComma);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool OemCommaKeyHeld() => KeyHeld(Keys.OemComma);

        #endregion

        #region Oem Period

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool OemPeriodKeyPress() => KeyPress(Keys.OemPeriod);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool OemPeriodKeyRelease() => KeyRelease(Keys.OemPeriod);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool OemPeriodKeyHeld() => KeyHeld(Keys.OemPeriod);

        #endregion

        #region Oem Question

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool OemQuestionKeyPress() => KeyPress(Keys.OemQuestion);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool OemQuestionKeyRelease() => KeyRelease(Keys.OemQuestion);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool OemQuestionKeyHeld() => KeyHeld(Keys.OemQuestion);

        #endregion

        #region Right Shift

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool RightShiftKeyPress() => KeyPress(Keys.RightShift);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool RightShiftKeyRelease() => KeyRelease(Keys.RightShift);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool RightShiftKeyHeld() => KeyHeld(Keys.RightShift);

        #endregion

        #region Left Ctrl

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool LeftCtrlKeyPress() => KeyPress(Keys.LeftControl);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool LeftCtrlKeyRelease() => KeyRelease(Keys.LeftControl);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool LeftCtrlKeyHeld() => KeyHeld(Keys.LeftControl);

        #endregion

        #region Left Alt

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool LeftAltKeyPress() => KeyPress(Keys.LeftAlt);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool LeftAltKeyRelease() => KeyRelease(Keys.LeftAlt);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool LeftAltKeyHeld() => KeyHeld(Keys.LeftAlt);

        #endregion

        #region Space

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool SpaceKeyPress() => KeyPress(Keys.Space);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool SpaceKeyRelease() => KeyRelease(Keys.Space);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool SpaceKeyHeld() => KeyHeld(Keys.Space);

        #endregion

        #region Right Alt

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool RightAltKeyPress() => KeyPress(Keys.RightAlt);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool RightAltKeyRelease() => KeyRelease(Keys.RightAlt);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool RightAltKeyHeld() => KeyHeld(Keys.RightAlt);

        #endregion

        #region Right Ctrl

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool RightCtrlKeyPress() => KeyPress(Keys.RightControl);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool RightCtrlKeyRelease() => KeyRelease(Keys.RightControl);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool RightCtrlKeyHeld() => KeyHeld(Keys.RightControl);

        #endregion

        #region Insert

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool InsertKeyPress() => KeyPress(Keys.Insert);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool InsertKeyRelease() => KeyRelease(Keys.Insert);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool InsertKeyHeld() => KeyHeld(Keys.Insert);

        #endregion

        #region Home

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool HomeKeyPress() => KeyPress(Keys.Home);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool HomeKeyRelease() => KeyRelease(Keys.Home);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool HomeKeyHeld() => KeyHeld(Keys.Home);

        #endregion

        #region Page Up

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool PageUpKeyPress() => KeyPress(Keys.PageUp);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool PageUpKeyRelease() => KeyRelease(Keys.PageUp);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool PageUpKeyHeld() => KeyHeld(Keys.PageUp);

        #endregion

        #region Delete

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool DeleteKeyPress() => KeyPress(Keys.Delete);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool DeleteKeyRelease() => KeyRelease(Keys.Delete);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool DeleteKeyHeld() => KeyHeld(Keys.Delete);

        #endregion

        #region End

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool EndKeyPress() => KeyPress(Keys.End);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool EndKeyRelease() => KeyRelease(Keys.End);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool EndKeyHeld() => KeyHeld(Keys.End);

        #endregion

        #region Page Down

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool PageDownKeyPress() => KeyPress(Keys.PageDown);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool PageDownKeyRelease() => KeyRelease(Keys.PageDown);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool PageDownKeyHeld() => KeyHeld(Keys.PageDown);

        #endregion

        #endregion

        #region Input Keyboard Special Keys

        #region LeftWindows

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool LeftWindowsKeyPress() => KeyPress(Keys.LeftWindows);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool LeftWindowsKeyRelease() => KeyRelease(Keys.LeftWindows);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool LeftWindowsKeyHeld() => KeyHeld(Keys.LeftWindows);

        #endregion

        #region RightWindows

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool RightWindowsKeyPress() => KeyPress(Keys.RightWindows);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool RightWindowsKeyRelease() => KeyRelease(Keys.RightWindows);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool RightWindowsKeyHeld() => KeyHeld(Keys.RightWindows);

        #endregion

        #endregion

        #region Input Keyboard Arrow Keys

        /// <summary>
        /// The physical functions performed by the keyboard such as <see cref="InputKeyboardArrowFlags.UpKey"/> and <see cref="InputKeyboardArrowFlags.DownKey"/>.
        /// </summary>
        public InputKeyboardArrowFlags InputArrowFlags { get; private set; } = 0;

        /// <summary>
        /// Determines whether a key is in a pressed state and sets the provided flag if true.
        /// </summary>
        /// <param name="flag">The <see cref="InputKeyboardArrowFlags"/> to set if the key is in a pressed state.</param>
        /// <param name="key">The <see cref="Keys"/> to check if it is in a pressed state.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        public bool KeyPress(InputKeyboardArrowFlags flag, Keys key) => KeyState.IsKeyDown(key) && PreviousKeyState.IsKeyUp(key);

        /// <summary>
        /// Determines whether a key is in a released state and sets the provided flag if true.
        /// </summary>
        /// <param name="flag">The <see cref="InputKeyboardArrowFlags"/> to set if the key is in a released state.</param>
        /// <param name="key">The <see cref="Keys"/> to check if it is in a released state.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        public bool KeyRelease(InputKeyboardArrowFlags flag, Keys key) => KeyState.IsKeyUp(key) && PreviousKeyState.IsKeyDown(key);

        /// <summary>
        /// Determines whether a key is in a held state and sets the provided flag if true.
        /// </summary>
        /// <param name="flag">The <see cref="InputKeyboardArrowFlags"/> to set if the key is in a held state.</param>
        /// <param name="key">The <see cref="Keys"/> to check if it is in a held state.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        public bool KeyHeld(InputKeyboardArrowFlags flag, Keys key) => KeyState.IsKeyDown(key) && PreviousKeyState.IsKeyDown(key);

        #region Up

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool UpKeyPress() => KeyPress(InputKeyboardArrowFlags.UpKey, Keys.Up);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool UpKeyRelease() => KeyRelease(InputKeyboardArrowFlags.UpKey, Keys.Up);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool UpKeyHeld() => KeyHeld(InputKeyboardArrowFlags.UpKey, Keys.Up);

        #endregion

        #region Down

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool DownKeyPress() => KeyPress(InputKeyboardArrowFlags.DownKey, Keys.Down);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool DownKeyRelease() => KeyRelease(InputKeyboardArrowFlags.DownKey, Keys.Down);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool DownKeyHeld() => KeyHeld(InputKeyboardArrowFlags.DownKey, Keys.Down);

        #endregion

        #region Left

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool LeftKeyPress() => KeyPress(InputKeyboardArrowFlags.LeftKey, Keys.Left);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool LeftKeyRelease() => KeyRelease(InputKeyboardArrowFlags.LeftKey, Keys.Left);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool LeftKeyHeld() => KeyHeld(InputKeyboardArrowFlags.LeftKey, Keys.Left);

        #endregion

        #region Right

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool RightKeyPress() => KeyPress(InputKeyboardArrowFlags.RightKey, Keys.Right);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool RightKeyRelease() => KeyRelease(InputKeyboardArrowFlags.RightKey, Keys.Right);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool RightKeyHeld() => KeyHeld(InputKeyboardArrowFlags.RightKey, Keys.Right);

        #endregion

        #endregion

        #region Input Keyboard Letter Keys

        #region Q

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool QKeyPress() => KeyPress(Keys.Q);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool QKeyRelease() => KeyRelease(Keys.Q);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool QKeyHeld() => KeyHeld(Keys.Q);

        #endregion

        #region W

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool WKeyPress() => KeyPress(Keys.W);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool WKeyRelease() => KeyRelease(Keys.W);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool WKeyHeld() => KeyHeld(Keys.W);

        #endregion

        #region E

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool EKeyPress() => KeyPress(Keys.E);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool EKeyRelease() => KeyRelease(Keys.E);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool EKeyHeld() => KeyHeld(Keys.E);

        #endregion

        #region R

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool RKeyPress() => KeyPress(Keys.R);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool RKeyRelease() => KeyRelease(Keys.R);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool RKeyHeld() => KeyHeld(Keys.R);

        #endregion

        #region T

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool TKeyPress() => KeyPress(Keys.T);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool TKeyRelease() => KeyRelease(Keys.T);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool TKeyHeld() => KeyHeld(Keys.T);

        #endregion

        #region Y

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool YKeyPress() => KeyPress(Keys.Y);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool YKeyRelease() => KeyRelease(Keys.Y);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool YKeyHeld() => KeyHeld(Keys.Y);

        #endregion

        #region U

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool UKeyPress() => KeyPress(Keys.U);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool UKeyRelease() => KeyRelease(Keys.U);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool UKeyHeld() => KeyHeld(Keys.U);

        #endregion

        #region I

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool IKeyPress() => KeyPress(Keys.I);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool IKeyRelease() => KeyRelease(Keys.I);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool IKeyHeld() => KeyHeld(Keys.I);

        #endregion

        #region O

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool OKeyPress() => KeyPress(Keys.O);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool OKeyRelease() => KeyRelease(Keys.O);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool OKeyHeld() => KeyHeld(Keys.O);

        #endregion

        #region P

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool PKeyPress() => KeyPress(Keys.P);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool PKeyRelease() => KeyRelease(Keys.P);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool PKeyHeld() => KeyHeld(Keys.P);

        #endregion

        #region A

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool AKeyPress() => KeyPress(Keys.A);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool AKeyRelease() => KeyRelease(Keys.A);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool AKeyHeld() => KeyHeld(Keys.A);

        #endregion

        #region S

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool SKeyPress() => KeyPress(Keys.S);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool SKeyRelease() => KeyRelease(Keys.S);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool SKeyHeld() => KeyHeld(Keys.S);

        #endregion

        #region D

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool DKeyPress() => KeyPress(Keys.D);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool DKeyRelease() => KeyRelease(Keys.D);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool DKeyHeld() => KeyHeld(Keys.D);

        #endregion

        #region F

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool FKeyPress() => KeyPress(Keys.F);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool FKeyRelease() => KeyRelease(Keys.F);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool FKeyHeld() => KeyHeld(Keys.F);

        #endregion

        #region G

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool GKeyPress() => KeyPress(Keys.G);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool GKeyRelease() => KeyRelease(Keys.G);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool GKeyHeld() => KeyHeld(Keys.G);

        #endregion

        #region H

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool HKeyPress() => KeyPress(Keys.H);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool HKeyRelease() => KeyRelease(Keys.H);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool HKeyHeld() => KeyHeld(Keys.H);

        #endregion

        #region J

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool JKeyPress() => KeyPress(Keys.J);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool JKeyRelease() => KeyRelease(Keys.J);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool JKeyHeld() => KeyHeld(Keys.J);

        #endregion

        #region K

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool KKeyPress() => KeyPress(Keys.K);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool KKeyRelease() => KeyRelease(Keys.K);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool KKeyHeld() => KeyHeld(Keys.K);

        #endregion

        #region L

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool LKeyPress() => KeyPress(Keys.L);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool LKeyRelease() => KeyRelease(Keys.L);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool LKeyHeld() => KeyHeld(Keys.L);

        #endregion

        #region Z

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool ZKeyPress() => KeyPress(Keys.Z);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool ZKeyRelease() => KeyRelease(Keys.Z);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool ZKeyHeld() => KeyHeld(Keys.Z);

        #endregion

        #region X

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool XKeyPress() => KeyPress(Keys.X);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool XKeyRelease() => KeyRelease(Keys.X);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool XKeyHeld() => KeyHeld(Keys.X);

        #endregion

        #region C

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool CKeyPress() => KeyPress(Keys.C);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool CKeyRelease() => KeyRelease(Keys.C);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool CKeyHeld() => KeyHeld(Keys.C);

        #endregion

        #region V

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool VKeyPress() => KeyPress(Keys.V);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool VKeyRelease() => KeyRelease(Keys.V);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool VKeyHeld() => KeyHeld(Keys.V);

        #endregion

        #region B

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool BKeyPress() => KeyPress(Keys.B);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool BKeyRelease() => KeyRelease(Keys.B);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool BKeyHeld() => KeyHeld(Keys.B);

        #endregion

        #region N

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool NKeyPress() => KeyPress(Keys.N);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool NKeyRelease() => KeyRelease(Keys.N);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool NKeyHeld() => KeyHeld(Keys.N);

        #endregion

        #region M

        /// <summary>
        /// Determines whether the keyboard key is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a pressed state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was pressed.</remarks>
        public bool MKeyPress() => KeyPress(Keys.M);

        /// <summary>
        /// Determines whether the keyboard key is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a released state.</returns>
        /// <remarks>Sets the appropriate key flag if the key was released.</remarks>
        public bool MKeyRelease() => KeyRelease(Keys.M);

        /// <summary>
        /// Determines whether the keyboard key is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the key is in a held state.</returns>
        /// <remarks>Sets the appropriate key flag if the key is being held.</remarks>
        public bool MKeyHeld() => KeyHeld(Keys.M);

        #endregion

        #endregion

        /// <summary>
        /// The keyboard's update method.
        /// </summary>
        /// <param name="gameTime">Intakes MonoGame <see cref="GameTime"/>.</param>
        public void Update(GameTime gameTime)
        {
            ElapsedTime += DeltaTime = gameTime.ElapsedGameTime.TotalSeconds;

            PreviousKeyState = KeyState;
            KeyState = Keyboard.GetState();
        }
    }
}