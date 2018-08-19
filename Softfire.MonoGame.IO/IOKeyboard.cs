using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Softfire.MonoGame.IO
{
    public static class IOKeyboard
    {
        /// <summary>
        /// Delta Time.
        /// Time between updates.
        /// </summary>
        private static double DeltaTime { get; set; }

        /// <summary>
        /// Elapsed Time.
        /// </summary>
        private static double ElapsedTime { get; set; }

        /// <summary>
        /// Keyboard KeyState.
        /// KeyState contains all data read from the currently attached Keyboard.
        /// </summary>
        private static KeyboardState KeyState { get; set; }

        /// <summary>
        /// Previous KeyState.
        /// PreviousKeyState contains all the data from the currently attached Keyboard during the last Update cycle.
        /// Used to determine if keys were pressed by comparing the KeyState and PreviousKeyState states of keys.
        /// </summary>
        private static KeyboardState PreviousKeyState { get; set; }

        /// <summary>
        /// Is In Use?
        /// </summary>
        public static bool IsInUse { get; private set; }

        /// <summary>
        /// Input String.
        /// Used to write text to string variables.
        /// Accepts commands from a standard US layout Keyobard.
        /// </summary>
        /// <param name="max">Intakes an int to define the maximum amout of characters in the returned string.</param>
        /// <param name="value">Intakes the current string being modified.</param>
        /// <returns>Returns the modified string.</returns>
        public static string InputString(string value, int max)
        {
            value = value.Trim();

            #region Lowercase

            if (value.Length < max &
                KeyHeld(Keys.LeftShift) == false &
                KeyHeld(Keys.RightShift) == false)
            {
                #region q

                if (KeyPress(Keys.Q))
                {
                    value += "q";
                }

                #endregion

                #region w

                if (KeyPress(Keys.W))
                {
                    value += "w";
                }

                #endregion

                #region e

                if (KeyPress(Keys.E))
                {
                    value += "e";
                }

                #endregion

                #region r

                if (KeyPress(Keys.R))
                {
                    value += "r";
                }

                #endregion

                #region t

                if (KeyPress(Keys.T))
                {
                    value += "t";
                }

                #endregion

                #region y

                if (KeyPress(Keys.Y))
                {
                    value += "y";
                }

                #endregion

                #region u

                if (KeyPress(Keys.U))
                {
                    value += "u";
                }

                #endregion

                #region i

                if (KeyPress(Keys.I))
                {
                    value += "i";
                }

                #endregion

                #region o

                if (KeyPress(Keys.O))
                {
                    value += "o";
                }

                #endregion

                #region p

                if (KeyPress(Keys.P))
                {
                    value += "p";
                }

                #endregion

                #region a

                if (KeyPress(Keys.A))
                {
                    value += "a";
                }

                #endregion

                #region s

                if (KeyPress(Keys.S))
                {
                    value += "s";
                }

                #endregion

                #region d

                if (KeyPress(Keys.D))
                {
                    value += "d";
                }

                #endregion

                #region f

                if (KeyPress(Keys.F))
                {
                    value += "f";
                }

                #endregion

                #region g

                if (KeyPress(Keys.G))
                {
                    value += "g";
                }

                #endregion

                #region h

                if (KeyPress(Keys.H))
                {
                    value += "h";
                }

                #endregion

                #region j

                if (KeyPress(Keys.J))
                {
                    value += "j";
                }

                #endregion

                #region k

                if (KeyPress(Keys.K))
                {
                    value += "k";
                }

                #endregion

                #region l

                if (KeyPress(Keys.L))
                {
                    value += "l";
                }

                #endregion

                #region z

                if (KeyPress(Keys.Z))
                {
                    value += "z";
                }

                #endregion

                #region x

                if (KeyPress(Keys.X))
                {
                    value += "x";
                }

                #endregion

                #region c

                if (KeyPress(Keys.C))
                {
                    value += "c";
                }

                #endregion

                #region v

                if (KeyPress(Keys.V))
                {
                    value += "v";
                }

                #endregion

                #region b

                if (KeyPress(Keys.B))
                {
                    value += "b";
                }

                #endregion

                #region n

                if (KeyPress(Keys.N))
                {
                    value += "n";
                }

                #endregion

                #region m

                if (KeyPress(Keys.M))
                {
                    value += "m";
                }

                #endregion
            }

            #endregion

            #region Uppercase

            if (value.Length < max &
                (KeyHeld(Keys.LeftShift) || KeyHeld(Keys.RightShift)))
            {
                #region Q

                if (KeyPress(Keys.Q))
                {
                    value += "Q";
                }

                #endregion

                #region W

                if (KeyPress(Keys.W))
                {
                    value += "W";
                }

                #endregion

                #region E

                if (KeyPress(Keys.E))
                {
                    value += "E";
                }

                #endregion

                #region R

                if (KeyPress(Keys.R))
                {
                    value += "R";
                }

                #endregion

                #region T

                if (KeyPress(Keys.T))
                {
                    value += "T";
                }

                #endregion

                #region Y

                if (KeyPress(Keys.Y))
                {
                    value += "Y";
                }

                #endregion

                #region U

                if (KeyPress(Keys.U))
                {
                    value += "U";
                }

                #endregion

                #region I

                if (KeyPress(Keys.I))
                {
                    value += "I";
                }

                #endregion

                #region O

                if (KeyPress(Keys.O))
                {
                    value += "O";
                }

                #endregion

                #region P

                if (KeyPress(Keys.P))
                {
                    value += "P";
                }

                #endregion

                #region A

                if (KeyPress(Keys.A))
                {
                    value += "A";
                }

                #endregion

                #region S

                if (KeyPress(Keys.S))
                {
                    value += "S";
                }

                #endregion

                #region D

                if (KeyPress(Keys.D))
                {
                    value += "D";
                }

                #endregion

                #region F

                if (KeyPress(Keys.F))
                {
                    value += "F";
                }

                #endregion

                #region G

                if (KeyPress(Keys.G))
                {
                    value += "G";
                }

                #endregion

                #region H

                if (KeyPress(Keys.H))
                {
                    value += "H";
                }

                #endregion

                #region J

                if (KeyPress(Keys.J))
                {
                    value += "J";
                }

                #endregion

                #region K

                if (KeyPress(Keys.K))
                {
                    value += "k";
                }

                #endregion

                #region L

                if (KeyPress(Keys.L))
                {
                    value += "L";
                }

                #endregion

                #region Z

                if (KeyPress(Keys.Z))
                {
                    value += "Z";
                }

                #endregion

                #region X

                if (KeyPress(Keys.X))
                {
                    value += "X";
                }

                #endregion

                #region C

                if (KeyPress(Keys.C))
                {
                    value += "C";
                }

                #endregion

                #region V

                if (KeyPress(Keys.V))
                {
                    value += "V";
                }

                #endregion

                #region B

                if (KeyPress(Keys.B))
                {
                    value += "B";
                }

                #endregion

                #region N

                if (KeyPress(Keys.N))
                {
                    value += "N";
                }

                #endregion

                #region M

                if (KeyPress(Keys.M))
                {
                    value += "M";
                }

                #endregion
            }

            #endregion

            #region Numbers

            if (value.Length < max &
                KeyHeld(Keys.LeftShift) == false &
                KeyHeld(Keys.RightShift) == false)
            {
                #region 0

                if (KeyPress(Keys.NumPad0) ||
                    KeyPress(Keys.D0))
                {
                    value += 0;
                }

                #endregion

                #region 1

                if (KeyPress(Keys.NumPad1) ||
                    KeyPress(Keys.D1))
                {
                    value += 1;
                }

                #endregion

                #region 2

                if (KeyPress(Keys.NumPad2) ||
                    KeyPress(Keys.D2))
                {
                    value += 2;
                }

                #endregion

                #region 3

                if (KeyPress(Keys.NumPad3) ||
                    KeyPress(Keys.D3))
                {
                    value += 3;
                }

                #endregion

                #region 4

                if (KeyPress(Keys.NumPad4) ||
                    KeyPress(Keys.D4))
                {
                    value += 4;
                }

                #endregion

                #region 5

                if (KeyPress(Keys.NumPad5) ||
                    KeyPress(Keys.D5))
                {
                    value += 5;
                }

                #endregion

                #region 6

                if (KeyPress(Keys.NumPad6) ||
                    KeyPress(Keys.D6))
                {
                    value += 6;
                }

                #endregion

                #region 7

                if (KeyPress(Keys.NumPad7) ||
                    KeyPress(Keys.D7))
                {
                    value += 7;
                }

                #endregion

                #region 8

                if (KeyPress(Keys.NumPad8) ||
                    KeyPress(Keys.D8))
                {
                    value += 8;
                }

                #endregion

                #region 9

                if (KeyPress(Keys.NumPad9) ||
                    KeyPress(Keys.D9))
                {
                    value += 9;
                }

                #endregion

            }

            #endregion

            #region Special Keys

            if (value.Length < max)
            {
                #region !

                if ((KeyHeld(Keys.LeftShift) || KeyHeld(Keys.RightShift)) &&
                    KeyPress(Keys.D1))
                {
                    value += "!";
                }

                #endregion

                #region @

                if ((KeyHeld(Keys.LeftShift) || KeyHeld(Keys.RightShift)) &&
                    KeyPress(Keys.D2))
                {
                    value += "@";
                }

                #endregion

                #region #

                if ((KeyHeld(Keys.LeftShift) || KeyHeld(Keys.RightShift)) &&
                    KeyPress(Keys.D3))
                {
                    value += "#";
                }

                #endregion

                #region $

                if ((KeyHeld(Keys.LeftShift) || KeyHeld(Keys.RightShift)) &&
                    KeyPress(Keys.D4))
                {
                    value += "$";
                }

                #endregion

                #region %

                if ((KeyHeld(Keys.LeftShift) || KeyHeld(Keys.RightShift)) &&
                    KeyPress(Keys.D5))
                {
                    value += "%";
                }

                #endregion

                #region ^

                if ((KeyHeld(Keys.LeftShift) || KeyHeld(Keys.RightShift)) &&
                    KeyPress(Keys.D6))
                {
                    value += "^";
                }

                #endregion

                #region &

                if ((KeyHeld(Keys.LeftShift) || KeyHeld(Keys.RightShift)) &&
                    KeyPress(Keys.D7))
                {
                    value += "&";
                }

                #endregion

                #region *

                if (((KeyHeld(Keys.LeftShift) || KeyHeld(Keys.RightShift)) &&
                    KeyPress(Keys.D8)) |
                    KeyPress(Keys.Multiply))
                {
                    value += "*";
                }

                #endregion

                #region (

                if ((KeyHeld(Keys.LeftShift) || KeyHeld(Keys.RightShift)) &&
                    KeyPress(Keys.D9))
                {
                    value += "(";
                }

                #endregion

                #region )

                if ((KeyHeld(Keys.LeftShift) || KeyHeld(Keys.RightShift)) &&
                    KeyPress(Keys.D0))
                {
                    value += ")";
                }

                #endregion

                #region -

                if (((KeyHeld(Keys.LeftShift) == false || KeyHeld(Keys.RightShift) == false) &&
                    KeyPress(Keys.OemMinus)) |
                    KeyPress(Keys.Subtract))
                {
                    value += "-";
                }

                #endregion

                #region _

                if ((KeyHeld(Keys.LeftShift) || KeyHeld(Keys.RightShift)) &&
                    KeyPress(Keys.OemMinus))
                {
                    value += "_";
                }

                #endregion

                #region =

                if ((KeyHeld(Keys.LeftShift) == false || KeyHeld(Keys.RightShift) == false) &&
                    KeyPress(Keys.OemPlus))
                {
                    value += "=";
                }

                #endregion

                #region +

                if (((KeyHeld(Keys.LeftShift) || KeyHeld(Keys.RightShift)) &&
                    KeyPress(Keys.OemPlus)) |
                    KeyPress(Keys.Add))
                {
                    value += "+";
                }

                #endregion

                #region [

                if ((KeyHeld(Keys.LeftShift) == false || KeyHeld(Keys.RightShift) == false) &&
                    KeyPress(Keys.OemOpenBrackets))
                {
                    value += "[";
                }

                #endregion

                #region {

                if ((KeyHeld(Keys.LeftShift) || KeyHeld(Keys.RightShift)) &&
                    KeyPress(Keys.OemOpenBrackets))
                {
                    value += "{";
                }

                #endregion

                #region ]

                if ((KeyHeld(Keys.LeftShift) == false || KeyHeld(Keys.RightShift) == false) &&
                    KeyPress(Keys.OemCloseBrackets))
                {
                    value += "]";
                }

                #endregion

                #region }

                if ((KeyHeld(Keys.LeftShift) || KeyHeld(Keys.RightShift)) &&
                    KeyPress(Keys.OemCloseBrackets))
                {
                    value += "}";
                }

                #endregion

                #region \

                if ((KeyHeld(Keys.LeftShift) == false || KeyHeld(Keys.RightShift) == false) &&
                    KeyPress(Keys.OemBackslash))
                {
                    value += "\\";
                }

                #endregion

                #region |

                if (((KeyHeld(Keys.LeftShift) || KeyHeld(Keys.RightShift)) &&
                    KeyPress(Keys.OemBackslash)) |
                    KeyPress(Keys.OemPipe))
                {
                    value += "|";
                }

                #endregion

                #region ;

                if ((KeyHeld(Keys.LeftShift) == false || KeyHeld(Keys.RightShift) == false) &&
                    KeyPress(Keys.OemSemicolon))
                {
                    value += ";";
                }

                #endregion

                #region :

                if ((KeyHeld(Keys.LeftShift) || KeyHeld(Keys.RightShift)) &&
                    KeyPress(Keys.OemSemicolon))
                {
                    value += ":";
                }

                #endregion

                #region '

                if ((KeyHeld(Keys.LeftShift) == false || KeyHeld(Keys.RightShift) == false) &&
                    KeyPress(Keys.OemQuotes))
                {
                    value += "'";
                }

                #endregion

                #region "

                if ((KeyHeld(Keys.LeftShift) || KeyHeld(Keys.RightShift)) &&
                    KeyPress(Keys.OemQuotes))
                {
                    value += "\"";
                }

                #endregion

                #region ,

                if ((KeyHeld(Keys.LeftShift) == false || KeyHeld(Keys.RightShift) == false) &&
                    KeyPress(Keys.OemComma))
                {
                    value += ",";
                }

                #endregion

                #region <

                if ((KeyHeld(Keys.LeftShift) || KeyHeld(Keys.RightShift)) &&
                    KeyPress(Keys.OemComma))
                {
                    value += "<";
                }

                #endregion

                #region .

                if (((KeyHeld(Keys.LeftShift) == false || KeyHeld(Keys.RightShift) == false) &&
                    KeyPress(Keys.OemPeriod)) |
                    KeyPress(Keys.Decimal))
                {
                    value += ".";
                }

                #endregion

                #region >

                if ((KeyHeld(Keys.LeftShift) || KeyHeld(Keys.RightShift)) &&
                    KeyPress(Keys.OemPeriod))
                {
                    value += ">";
                }

                #endregion

                #region /

                if (((KeyHeld(Keys.LeftShift) == false || KeyHeld(Keys.RightShift) == false) &&
                    KeyPress(Keys.OemQuestion)) |
                    KeyPress(Keys.Divide))
                {
                    value += "/";
                }

                #endregion

                #region ?

                if ((KeyHeld(Keys.LeftShift) || KeyHeld(Keys.RightShift)) &&
                    KeyPress(Keys.OemQuestion))
                {
                    value += "?";
                }

                #endregion
            }

            #endregion

            #region Functions

            #region Space

            if (value.Length < max &
                KeyPress(Keys.Space))
            {
                value += " ";
            }

            #endregion

            #region Delete

            if (value.Length > 0 & (KeyPress(Keys.Delete) ||
                                    KeyPress(Keys.Back)))
            {
                value = value.Remove(value.Length - 1, 1);
            }

            #endregion

            #endregion

            return value;
        }

        /// <summary>
        /// Input Clamp.
        /// </summary>
        /// <param name="value">The value being modified. Intaken as an int.</param>
        /// <param name="min">The minimum number. Intaken as an int.</param>
        /// <param name="max">The maximum number. Intaken as an int.</param>
        /// <returns>Returns the modified int.</returns>
        public static int InputClamp(int value, int min, int max)
        {
            return MathHelper.Clamp(value, min, max);
        }

        /// <summary>
        /// Input Clamp.
        /// </summary>
        /// <param name="value">The value being modified. Intaken as a float.</param>
        /// <param name="min">The minimum number. Intaken as a float.</param>
        /// <param name="max">The maximum number. Intaken as a float.</param>
        /// <returns>Returns the modified float.</returns>
        public static float InputClamp(float value, float min, float max)
        {
            return MathHelper.Clamp(value, min, max);
        }

        /// <summary>
        /// Input Clamp.
        /// </summary>
        /// <param name="value">The value being modified. Intaken as a double.</param>
        /// <param name="min">The minimum number. Intaken as a double.</param>
        /// <param name="max">The maximum number. Intaken as a double.</param>
        /// <returns>Returns the modified double.</returns>
        public static double InputClamp(double value, double min, double max)
        {
            if (value < min)
            {
                value = min;
            }
            else if (value > max)
            {
                value = max;
            }

            return value;
        }

        /// <summary>
        /// Input Clamp.
        /// </summary>
        /// <param name="value">The value being modified. Intaken as a decimal.</param>
        /// <param name="min">The minimum number. Intaken as a decimal.</param>
        /// <param name="max">The maximum number. Intaken as a decimal.</param>
        /// <returns>Returns the modified decimal.</returns>
        public static decimal InputClamp(decimal value, decimal min, decimal max)
        {
            if (value < min)
            {
                value = min;
            }
            else if (value > max)
            {
                value = max;
            }

            return value;
        }

        /// <summary>
        /// Menu Entry Selection.
        /// This method is used to update the Menu Entry Index currently selected.
        /// Can be used for menus with sub menus. Simply increment/decrement by 0.1.
        /// </summary>
        /// <param name="currentMenuEntryIndex">The current menu entry index. Intaken as a int.</param>
        /// <param name="previousEntryKey">The Key used to select the previous entry number.</param>
        /// <param name="nextEntryKey">he Key used to select the next entry number.</param>
        /// <param name="firstEntry">The first menu entry in the menu part. Intaken as an int.</param>
        /// <param name="lastEntry">The last menu entry in the menu part. Intaken as an int.</param>
        /// <param name="firstMenu">A bool indicating whether this is the first menu in a series.</param>
        /// <param name="lastMenu">A bool indicating whether this is the last menu in a series.</param>
        /// <param name="isLoopingOnEntries">A bool indicating whether the menu index will return to the first or last entry upon exceeding the menu entry index.</param>
        /// <returns></returns>
        public static int MenuEntrySelection(int currentMenuEntryIndex, Keys previousEntryKey, Keys nextEntryKey,
                                                int firstEntry = 1, int lastEntry = 1,
                                                bool firstMenu = true, bool lastMenu = true,
                                                bool isLoopingOnEntries = true)
        {
            if (currentMenuEntryIndex >= firstEntry &&
                currentMenuEntryIndex <= lastEntry)
            {
                // Previous Entry.
                if (KeyPress(previousEntryKey))
                {
                    currentMenuEntryIndex--;

                    if (firstMenu)
                    {
                        if (currentMenuEntryIndex < firstEntry)
                        {
                            currentMenuEntryIndex = isLoopingOnEntries == false ? firstEntry : lastEntry;
                        }
                    }
                }

                // Next Entry.
                if (KeyPress(nextEntryKey))
                {
                    currentMenuEntryIndex++;

                    if (lastMenu)
                    {
                        if (currentMenuEntryIndex > lastEntry)
                        {
                            currentMenuEntryIndex = isLoopingOnEntries == false ? lastEntry : firstEntry;
                        }
                    }
                }
            }
            else if (currentMenuEntryIndex < firstEntry)
            {
                currentMenuEntryIndex = firstEntry;
            }
            else if (currentMenuEntryIndex > lastEntry)
            {
                currentMenuEntryIndex = lastEntry;
            }

            return currentMenuEntryIndex;
        }

        /// <summary>
        /// Key Press Method.
        /// Can be used to detect if a key was pressed.
        /// </summary>
        /// <param name="key">Intakes a Input.Keys key to check if it was pressed.</param>
        /// <returns></returns>
        public static bool KeyPress(Keys key)
        {
            return KeyState.IsKeyDown(key) && PreviousKeyState.IsKeyUp(key);
        }

        /// <summary>
        /// Key Held Method.
        /// Can be used to detect if a key is being held down.
        /// </summary>
        /// <param name="key">Intakes a Input.Keys key to check if it is being held down.</param>
        /// <returns></returns>
        public static bool KeyHeld(Keys key)
        {
            return PreviousKeyState.IsKeyDown(key) && KeyState.IsKeyDown(key);
        }

        /// <summary>
        /// Repeat.
        /// </summary>
        /// <param name="character">The character to repeat.</param>
        /// <param name="numberOfTimesToRepeat">The number of times to repeat the character</param>
        /// <returns>Returns a new string containing the repeated character.</returns>
        public static string Repeat(this char character, int numberOfTimesToRepeat)
        {
            return new string(character, numberOfTimesToRepeat);
        }

        /// <summary>
        /// Update Method.
        /// Updates KeyStates.
        /// </summary>
        /// <param name="gameTime">Intakes MonoGame GameTime.</param>
        public static void Update(GameTime gameTime)
        {
            DeltaTime = gameTime.ElapsedGameTime.TotalSeconds;

            PreviousKeyState = KeyState;
            KeyState = Keyboard.GetState();

            IsInUse = KeyState != PreviousKeyState;
        }
    }
}