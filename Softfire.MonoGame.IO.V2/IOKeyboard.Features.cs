using Softfire.MonoGame.CORE.V2.Input;

namespace Softfire.MonoGame.IO.V2
{
    public partial class IOKeyboard
    {
        /// <summary>
        /// Repeats a character for X number of times.
        /// </summary>
        /// <param name="character">The character to repeat. Intaken as a <see cref="char"/>.</param>
        /// <param name="numberOfTimesToRepeat">The number of times to repeat the character. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a new string containing the repeated character.</returns>
        public string Repeat(char character, int numberOfTimesToRepeat) => new string(character, numberOfTimesToRepeat);

        /// <summary>
        /// Used to write text to string variables.
        /// </summary>
        /// <param name="sender">The subscribed object.</param>
        /// <param name="args">The <see cref="InputEventArgs"/> sent with the call.</param>
        public void InputString(object sender, InputEventArgs args)
        {
            if (!string.IsNullOrWhiteSpace(args.InputString) &&
                args.InputString.Length > 0)
            {
                #region Lowercase

                if (args.InputString.Length < args.MaxLength &&
                    (args.InputFlags.IsFlagSet(InputKeyboardCommandFlags.LeftShiftKey) || args.InputFlags.IsFlagSet(InputKeyboardCommandFlags.RightShiftKey)))
                {
                    if (args.InputFlags.IsFlagSet(InputActionStateFlags.Press))
                    {
                        #region q

                        if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.QKey))
                        {
                            args.InputString += "q";
                        }

                        #endregion

                        #region w

                        if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.WKey))
                        {
                            args.InputString += "w";
                        }

                        #endregion

                        #region e

                        if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.EKey))
                        {
                            args.InputString += "e";
                        }

                        #endregion

                        #region r

                        if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.RKey))
                        {
                            args.InputString += "r";
                        }

                        #endregion

                        #region t

                        if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.TKey))
                        {
                            args.InputString += "t";
                        }

                        #endregion

                        #region y

                        if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.YKey))
                        {
                            args.InputString += "y";
                        }

                        #endregion

                        #region u

                        if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.UKey))
                        {
                            args.InputString += "u";
                        }

                        #endregion

                        #region i

                        if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.IKey))
                        {
                            args.InputString += "i";
                        }

                        #endregion

                        #region o

                        if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.OKey))
                        {
                            args.InputString += "o";
                        }

                        #endregion

                        #region p

                        if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.PKey))
                        {
                            args.InputString += "p";
                        }

                        #endregion

                        #region a

                        if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.AKey))
                        {
                            args.InputString += "a";
                        }

                        #endregion

                        #region s

                        if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.SKey))
                        {
                            args.InputString += "s";
                        }

                        #endregion

                        #region d

                        if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.DKey))
                        {
                            args.InputString += "d";
                        }

                        #endregion

                        #region f

                        if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.FKey))
                        {
                            args.InputString += "f";
                        }

                        #endregion

                        #region g

                        if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.GKey))
                        {
                            args.InputString += "g";
                        }

                        #endregion

                        #region h

                        if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.HKey))
                        {
                            args.InputString += "h";
                        }

                        #endregion

                        #region j

                        if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.JKey))
                        {
                            args.InputString += "j";
                        }

                        #endregion

                        #region k

                        if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.KKey))
                        {
                            args.InputString += "k";
                        }

                        #endregion

                        #region l

                        if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.LKey))
                        {
                            args.InputString += "l";
                        }

                        #endregion

                        #region z

                        if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.ZKey))
                        {
                            args.InputString += "z";
                        }

                        #endregion

                        #region x

                        if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.XKey))
                        {
                            args.InputString += "x";
                        }

                        #endregion

                        #region c

                        if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.CKey))
                        {
                            args.InputString += "c";
                        }

                        #endregion

                        #region v

                        if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.VKey))
                        {
                            args.InputString += "v";
                        }

                        #endregion

                        #region b

                        if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.BKey))
                        {
                            args.InputString += "b";
                        }

                        #endregion

                        #region n

                        if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.NKey))
                        {
                            args.InputString += "n";
                        }

                        #endregion

                        #region m

                        if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.MKey))
                        {
                            args.InputString += "m";
                        }

                        #endregion
                    }
                }

                #endregion

                #region Uppercase

                if (args.InputString.Length < args.MaxLength &&
                    (args.InputFlags.IsFlagSet(InputKeyboardCommandFlags.LeftShiftKey) || args.InputFlags.IsFlagSet(InputKeyboardCommandFlags.RightShiftKey)))
                {
                    #region Q

                    if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.QKey))
                    {
                        args.InputString += "Q";
                    }

                    #endregion

                    #region W

                    if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.WKey))
                    {
                        args.InputString += "W";
                    }

                    #endregion

                    #region E

                    if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.EKey))
                    {
                        args.InputString += "E";
                    }

                    #endregion

                    #region R

                    if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.RKey))
                    {
                        args.InputString += "R";
                    }

                    #endregion

                    #region T

                    if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.TKey))
                    {
                        args.InputString += "T";
                    }

                    #endregion

                    #region Y

                    if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.YKey))
                    {
                        args.InputString += "Y";
                    }

                    #endregion

                    #region U

                    if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.UKey))
                    {
                        args.InputString += "U";
                    }

                    #endregion

                    #region I

                    if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.IKey))
                    {
                        args.InputString += "I";
                    }

                    #endregion

                    #region O

                    if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.OKey))
                    {
                        args.InputString += "O";
                    }

                    #endregion

                    #region P

                    if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.PKey))
                    {
                        args.InputString += "P";
                    }

                    #endregion

                    #region A

                    if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.AKey))
                    {
                        args.InputString += "A";
                    }

                    #endregion

                    #region S

                    if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.SKey))
                    {
                        args.InputString += "S";
                    }

                    #endregion

                    #region D

                    if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.DKey))
                    {
                        args.InputString += "D";
                    }

                    #endregion

                    #region F

                    if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.FKey))
                    {
                        args.InputString += "F";
                    }

                    #endregion

                    #region G

                    if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.GKey))
                    {
                        args.InputString += "G";
                    }

                    #endregion

                    #region H

                    if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.HKey))
                    {
                        args.InputString += "H";
                    }

                    #endregion

                    #region J

                    if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.JKey))
                    {
                        args.InputString += "J";
                    }

                    #endregion

                    #region K

                    if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.KKey))
                    {
                        args.InputString += "k";
                    }

                    #endregion

                    #region L

                    if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.LKey))
                    {
                        args.InputString += "L";
                    }

                    #endregion

                    #region Z

                    if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.ZKey))
                    {
                        args.InputString += "Z";
                    }

                    #endregion

                    #region X

                    if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.XKey))
                    {
                        args.InputString += "X";
                    }

                    #endregion

                    #region C

                    if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.CKey))
                    {
                        args.InputString += "C";
                    }

                    #endregion

                    #region V

                    if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.VKey))
                    {
                        args.InputString += "V";
                    }

                    #endregion

                    #region B

                    if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.BKey))
                    {
                        args.InputString += "B";
                    }

                    #endregion

                    #region N

                    if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.NKey))
                    {
                        args.InputString += "N";
                    }

                    #endregion

                    #region M

                    if (args.InputFlags.IsFlagSet(InputKeyboardLetterFlags.MKey))
                    {
                        args.InputString += "M";
                    }

                    #endregion
                }

                #endregion

                #region Numbers

                if (args.InputString.Length < args.MaxLength &&
                    !args.InputFlags.IsFlagSet(InputKeyboardCommandFlags.LeftShiftKey) &&
                    !args.InputFlags.IsFlagSet(InputKeyboardCommandFlags.RightShiftKey))
                {
                    #region 0

                    if (args.InputFlags.IsFlagSet(InputKeyboardNumPadFlags.NumPad0Key) ||
                        args.InputFlags.IsFlagSet(InputKeyboardNumberFlags.D0Key))
                    {
                        args.InputString += 0;
                    }

                    #endregion

                    #region 1

                    if (args.InputFlags.IsFlagSet(InputKeyboardNumPadFlags.NumPad1Key) ||
                        args.InputFlags.IsFlagSet(InputKeyboardNumberFlags.D1Key))
                    {
                        args.InputString += 1;
                    }

                    #endregion

                    #region 2

                    if (args.InputFlags.IsFlagSet(InputKeyboardNumPadFlags.NumPad2Key) ||
                        args.InputFlags.IsFlagSet(InputKeyboardNumberFlags.D2Key))
                    {
                        args.InputString += 2;
                    }

                    #endregion

                    #region 3

                    if (args.InputFlags.IsFlagSet(InputKeyboardNumPadFlags.NumPad3Key) ||
                        args.InputFlags.IsFlagSet(InputKeyboardNumberFlags.D3Key))
                    {
                        args.InputString += 3;
                    }

                    #endregion

                    #region 4

                    if (args.InputFlags.IsFlagSet(InputKeyboardNumPadFlags.NumPad4Key) ||
                        args.InputFlags.IsFlagSet(InputKeyboardNumberFlags.D4Key))
                    {
                        args.InputString += 4;
                    }

                    #endregion

                    #region 5

                    if (args.InputFlags.IsFlagSet(InputKeyboardNumPadFlags.NumPad5Key) ||
                        args.InputFlags.IsFlagSet(InputKeyboardNumberFlags.D5Key))
                    {
                        args.InputString += 5;
                    }

                    #endregion

                    #region 6

                    if (args.InputFlags.IsFlagSet(InputKeyboardNumPadFlags.NumPad6Key) ||
                        args.InputFlags.IsFlagSet(InputKeyboardNumberFlags.D6Key))
                    {
                        args.InputString += 6;
                    }

                    #endregion

                    #region 7

                    if (args.InputFlags.IsFlagSet(InputKeyboardNumPadFlags.NumPad7Key) ||
                        args.InputFlags.IsFlagSet(InputKeyboardNumberFlags.D7Key))
                    {
                        args.InputString += 7;
                    }

                    #endregion

                    #region 8

                    if (args.InputFlags.IsFlagSet(InputKeyboardNumPadFlags.NumPad8Key) ||
                        args.InputFlags.IsFlagSet(InputKeyboardNumberFlags.D8Key))
                    {
                        args.InputString += 8;
                    }

                    #endregion

                    #region 9

                    if (args.InputFlags.IsFlagSet(InputKeyboardNumPadFlags.NumPad9Key) ||
                        args.InputFlags.IsFlagSet(InputKeyboardNumberFlags.D9Key))
                    {
                        args.InputString += 9;
                    }

                    #endregion

                }

                #endregion

                #region Special Keys

                if (args.InputString.Length < args.MaxLength)
                {
                    if (args.InputFlags.IsFlagSet(InputKeyboardCommandFlags.LeftShiftKey) ||
                        args.InputFlags.IsFlagSet(InputKeyboardCommandFlags.RightShiftKey))
                    {
                        #region !

                        if (args.InputFlags.IsFlagSet(InputKeyboardNumberFlags.D1Key))
                        {
                            args.InputString += "!";
                        }

                        #endregion

                        #region @

                        if (args.InputFlags.IsFlagSet(InputKeyboardNumberFlags.D2Key))
                        {
                            args.InputString += "@";
                        }

                        #endregion

                        #region #

                        if (args.InputFlags.IsFlagSet(InputKeyboardNumberFlags.D3Key))
                        {
                            args.InputString += "#";
                        }

                        #endregion

                        #region $

                        if (args.InputFlags.IsFlagSet(InputKeyboardNumberFlags.D4Key))
                        {
                            args.InputString += "$";
                        }

                        #endregion

                        #region %

                        if (args.InputFlags.IsFlagSet(InputKeyboardNumberFlags.D5Key))
                        {
                            args.InputString += "%";
                        }

                        #endregion

                        #region ^

                        if (args.InputFlags.IsFlagSet(InputKeyboardNumberFlags.D6Key))
                        {
                            args.InputString += "^";
                        }

                        #endregion

                        #region &

                        if (args.InputFlags.IsFlagSet(InputKeyboardNumberFlags.D7Key))
                        {
                            args.InputString += "&";
                        }

                        #endregion

                        #region *

                        if (args.InputFlags.IsFlagSet(InputKeyboardNumberFlags.D8Key) ||
                            args.InputFlags.IsFlagSet(InputKeyboardNumPadFlags.MultiplyKey))
                        {
                            args.InputString += "*";
                        }

                        #endregion

                        #region (

                        if (args.InputFlags.IsFlagSet(InputKeyboardNumberFlags.D9Key))
                        {
                            args.InputString += "(";
                        }

                        #endregion

                        #region )

                        if (args.InputFlags.IsFlagSet(InputKeyboardNumberFlags.D0Key))
                        {
                            args.InputString += ")";
                        }

                        #endregion

                        #region _

                        if (args.InputFlags.IsFlagSet(InputKeyboardCommandFlags.OemMinusKey))
                        {
                            args.InputString += "_";
                        }

                        #endregion

                        #region +

                        if (args.InputFlags.IsFlagSet(InputKeyboardCommandFlags.OemPlusKey) ||
                            args.InputFlags.IsFlagSet(InputKeyboardNumPadFlags.AddKey))
                        {
                            args.InputString += "+";
                        }

                        #endregion

                        #region {

                        if (args.InputFlags.IsFlagSet(InputKeyboardCommandFlags.OemOpenBracketsKey))
                        {
                            args.InputString += "{";
                        }

                        #endregion

                        #region }

                        if (args.InputFlags.IsFlagSet(InputKeyboardCommandFlags.OemCloseBracketsKey))
                        {
                            args.InputString += "}";
                        }

                        #endregion

                        #region |

                        if (args.InputFlags.IsFlagSet(InputKeyboardCommandFlags.OemPipeKey))
                        {
                            args.InputString += "|";
                        }

                        #endregion

                        #region :

                        if (args.InputFlags.IsFlagSet(InputKeyboardCommandFlags.OemSemicolonKey))
                        {
                            args.InputString += ":";
                        }

                        #endregion

                        #region "

                        if (args.InputFlags.IsFlagSet(InputKeyboardCommandFlags.OemQuotesKey))
                        {
                            args.InputString += "\"";
                        }

                        #endregion

                        #region <

                        if (args.InputFlags.IsFlagSet(InputKeyboardCommandFlags.OemCommaKey))
                        {
                            args.InputString += "<";
                        }

                        #endregion

                        #region >

                        if (args.InputFlags.IsFlagSet(InputKeyboardCommandFlags.OemPeriodKey))
                        {
                            args.InputString += ">";
                        }

                        #endregion

                        #region ?

                        if (args.InputFlags.IsFlagSet(InputKeyboardCommandFlags.OemQuestionKey))
                        {
                            args.InputString += "?";
                        }

                        #endregion
                    }

                    if (!args.InputFlags.IsFlagSet(InputKeyboardCommandFlags.LeftShiftKey) ||
                        !args.InputFlags.IsFlagSet(InputKeyboardCommandFlags.RightShiftKey))
                    {
                        #region -

                        if (args.InputFlags.IsFlagSet(InputKeyboardCommandFlags.OemMinusKey) ||
                            args.InputFlags.IsFlagSet(InputKeyboardNumPadFlags.SubtractKey))
                        {
                            args.InputString += "-";
                        }

                        #endregion

                        #region =

                        if (args.InputFlags.IsFlagSet(InputKeyboardCommandFlags.OemPlusKey))
                        {
                            args.InputString += "=";
                        }

                        #endregion

                        #region [

                        if (args.InputFlags.IsFlagSet(InputKeyboardCommandFlags.OemOpenBracketsKey))
                        {
                            args.InputString += "[";
                        }

                        #endregion

                        #region ]

                        if (args.InputFlags.IsFlagSet(InputKeyboardCommandFlags.OemCloseBracketsKey))
                        {
                            args.InputString += "]";
                        }

                        #endregion

                        #region \

                        if (args.InputFlags.IsFlagSet(InputKeyboardCommandFlags.OemPipeKey))
                        {
                            args.InputString += @"\";
                        }

                        #endregion

                        #region ;

                        if (args.InputFlags.IsFlagSet(InputKeyboardCommandFlags.OemSemicolonKey))
                        {
                            args.InputString += ";";
                        }

                        #endregion

                        #region '

                        if (args.InputFlags.IsFlagSet(InputKeyboardCommandFlags.OemQuotesKey))
                        {
                            args.InputString += "'";
                        }

                        #endregion

                        #region ,

                        if (args.InputFlags.IsFlagSet(InputKeyboardCommandFlags.OemCommaKey))
                        {
                            args.InputString += ",";
                        }

                        #endregion

                        #region .

                        if (args.InputFlags.IsFlagSet(InputKeyboardCommandFlags.OemPeriodKey) ||
                            args.InputFlags.IsFlagSet(InputKeyboardNumPadFlags.DecimalKey))
                        {
                            args.InputString += ".";
                        }

                        #endregion

                        #region /

                        if (args.InputFlags.IsFlagSet(InputKeyboardCommandFlags.OemQuestionKey) ||
                            args.InputFlags.IsFlagSet(InputKeyboardNumPadFlags.DivideKey))
                        {
                            args.InputString += "/";
                        }

                        #endregion
                    }

                }

                #endregion

                #region Functions

                #region Space

                if (args.InputString.Length < args.MaxLength &&
                    args.InputFlags.IsFlagSet(InputKeyboardCommandFlags.SpaceKey))
                {
                    args.InputString += " ";
                }

                #endregion

                #region Delete

                if (args.InputString.Length > 0 &&
                    (args.InputFlags.IsFlagSet(InputKeyboardCommandFlags.DeleteKey) ||
                     args.InputFlags.IsFlagSet(InputKeyboardCommandFlags.BackspaceKey)))
                {
                    args.InputString = args.InputString.Remove(args.InputString.Length - 1, 1);
                }

                #endregion

                #endregion
            }
        }
    }
}