using System;

namespace Softfire.MonoGame.CORE.V2.Input
{
    /// <summary>
    /// The mapped input confirmation commands such as <see cref="Confirm"/> and <see cref="Cancel"/>.
    /// </summary>
    [Flags]
    public enum InputMappableConfirmationCommandFlags
    {
        /// <summary>
        /// Confirmation control.
        /// </summary>
        Confirm = 1 << 0,
        /// <summary>
        /// Cancellation control.
        /// </summary>
        Cancel = 1 << 1
    }

    /// <summary>
    /// The mapped input movement commands such as <see cref="MoveUp"/> and <see cref="StrafeLeft"/>.
    /// </summary>
    [Flags]
    public enum InputMappableMovementCommandFlags
    {
        /// <summary>
        /// Upward movement control.
        /// </summary>
        MoveUp = 1 << 2,
        /// <summary>
        /// Downward movement control.
        /// </summary>
        MoveDown = 1 << 3,
        /// <summary>
        /// Leftward movement control.
        /// </summary>
        MoveLeft = 1 << 4,
        /// <summary>
        /// Rightward movement control.
        /// </summary>
        MoveRight = 1 << 5,
        /// <summary>
        /// Upward strafing control.
        /// </summary>
        StrafeUp = 1 << 6,
        /// <summary>
        /// Downward strafing control.
        /// </summary>
        StrafeDown = 1 << 7,
        /// <summary>
        /// Leftward strafing control.
        /// </summary>
        StrafeLeft = 1 << 8,
        /// <summary>
        /// Rightward strafing control.
        /// </summary>
        StrafeRight = 1 << 9,
        /// <summary>
        /// Tabbing control.
        /// </summary>
        Tab = 1 << 10
    }

    /// <summary>
    /// The mapped input camera commands such as <see cref="ZoomIn"/> and <see cref="PanLeft"/>.
    /// </summary>
    [Flags]
    public enum InputMappableCameraCommandFlags
    {
        /// <summary>
        /// Camera toggle control.
        /// </summary>
        Toggle = 1 << 0,
        /// <summary>
        /// Camera centering control.
        /// </summary>
        Center = 1 << 1,
        /// <summary>
        /// Camera follow control.
        /// </summary>
        Follow = 1 << 2,
        /// <summary>
        /// Camera fly mode control.
        /// </summary>
        FlyMode = 1 << 3,
        /// <summary>
        /// Camera zoom in control.
        /// </summary>
        ZoomIn = 1 << 4,
        /// <summary>
        /// Camera zoom out control.
        /// </summary>
        ZoomOut = 1 << 5,
        /// <summary>
        /// Camera pan up control.
        /// </summary>
        PanUp = 1 << 6,
        /// <summary>
        /// Camera pan down control.
        /// </summary>
        PanDown = 1 << 7,
        /// <summary>
        /// Camera pan left control.
        /// </summary>
        PanLeft = 1 << 8,
        /// <summary>
        /// Camera pan right control.
        /// </summary>
        PanRight = 1 << 9,
        /// <summary>
        /// Camera rotate clockwise control.
        /// </summary>
        RotateClockwise = 1 << 10,
        /// <summary>
        /// Camera rotate counter clockwise control.
        /// </summary>
        RotatesCounterClockwise = 1 << 11,
        /// <summary>
        /// Camera mirror mode control.
        /// </summary>
        MirrorMode = 1 << 12
    }

    /// <summary>
    /// The physical actions performed by the mouse such as <see cref="LeftClick"/> and <see cref="RightClick"/>.
    /// </summary>
    [Flags]
    public enum InputMouseActionFlags
    {
        /// <summary>
        /// The mouse's idle flag.
        /// </summary>
        None = 0,
        /// <summary>
        /// The mouse's left click press flag.
        /// </summary>
        LeftClick = 1 << 0,
        /// <summary>
        /// The mouse's middle click press flag.
        /// </summary>
        MiddleClick = 1 << 1,
        /// <summary>
        /// The mouse's right click press flag.
        /// </summary>
        RightClick = 1 << 2,
        /// <summary>
        /// The mouse's button one click press flag.
        /// </summary>
        ButtonOneClick = 1 << 3,
        /// <summary>
        /// The mouse's button two click press flag.
        /// </summary>
        ButtonTwoClick = 1 << 4,
        /// <summary>
        /// The mouse's scroll up flag.
        /// </summary>
        ScrollUp = 1 << 5,
        /// <summary>
        /// The mouse's scroll down flag.
        /// </summary>
        ScrollDown = 1 << 6,
        /// <summary>
        /// The mouse's scroll left flag.
        /// </summary>
        ScrollLeft = 1 << 7,
        /// <summary>
        /// The mouses' scroll right flag.
        /// </summary>
        ScrollRight = 1 << 8,
        /// <summary>
        /// The mouse's forward movement flag.
        /// </summary>
        Forward = 1 << 9,
        /// <summary>
        /// The mouse's backward movement flag.
        /// </summary>
        Backward = 1 << 10,
        /// <summary>
        /// The mouse's leftward movement flag.
        /// </summary>
        Leftward = 1 << 11,
        /// <summary>
        /// The mouse's rightward movement flag.
        /// </summary>
        Rightward = 1 << 12
    }

    /// <summary>
    /// The keyboard function key flags. Used in event handling of keyboard input. See <see cref="F1Key"/> and <see cref="F4Key"/>.
    /// </summary>
    [Flags]
    public enum InputKeyboardFunctionFlags
    {
        /// <summary>
        /// The keyboard's F1 key flag.
        /// </summary>
        F1Key = 1 << 0,
        /// <summary>
        /// The keyboard's F2 key flag.
        /// </summary>
        F2Key = 1 << 1,
        /// <summary>
        /// The keyboard's F3 key flag.
        /// </summary>
        F3Key = 1 << 2,
        /// <summary>
        /// The keyboard's F4 key flag.
        /// </summary>
        F4Key = 1 << 3,
        /// <summary>
        /// The keyboard's F5 key flag.
        /// </summary>
        F5Key = 1 << 4,
        /// <summary>
        /// The keyboard's F6 key flag.
        /// </summary>
        F6Key = 1 << 5,
        /// <summary>
        /// The keyboard's F7 key flag.
        /// </summary>
        F7Key = 1 << 6,
        /// <summary>
        /// The keyboard's F8 key flag.
        /// </summary>
        F8Key = 1 << 7,
        /// <summary>
        /// The keyboard's F9 key flag.
        /// </summary>
        F9Key = 1 << 8,
        /// <summary>
        /// The keyboard's F10 key flag.
        /// </summary>
        F10Key = 1 << 9,
        /// <summary>
        /// The keyboard's F11 key flag.
        /// </summary>
        F11Key = 1 << 10,
        /// <summary>
        /// The keyboard's F12 key flag.
        /// </summary>
        F12Key = 1 << 11
    }

    /// <summary>
    /// The keyboard num pad key flags. Used in event handling of keyboard input. See <see cref="NumLockKey"/> and <see cref="NumPad0Key"/>.
    /// </summary>
    [Flags]
    public enum InputKeyboardNumPadFlags
    {
        /// <summary>
        /// The keyboard's Num Lock key flag.
        /// </summary>
        NumLockKey = 1 << 0,
        /// <summary>
        /// The keyboard's Divide key flag.
        /// </summary>
        DivideKey = 1 << 1,
        /// <summary>
        /// The keyboard's Multiply key flag.
        /// </summary>
        MultiplyKey = 1 << 2,
        /// <summary>
        /// The keyboard's Subtract key flag.
        /// </summary>
        SubtractKey = 1 << 3,
        /// <summary>
        /// The keyboard's Add key flag.
        /// </summary>
        AddKey = 1 << 4,
        /// <summary>
        /// The keyboard's Decimal key flag.
        /// </summary>
        DecimalKey = 1 << 5,
        /// <summary>
        /// The keyboard's NumPad 0 key flag.
        /// </summary>
        NumPad0Key = 1 << 6,
        /// <summary>
        /// The keyboard's NumPad 1 key flag.
        /// </summary>
        NumPad1Key = 1 << 7,
        /// <summary>
        /// The keyboard's NumPad 2 key flag.
        /// </summary>
        NumPad2Key = 1 << 8,
        /// <summary>
        /// The keyboard's NumPad 3 key flag.
        /// </summary>
        NumPad3Key = 1 << 9,
        /// <summary>
        /// The keyboard's NumPad 4 key flag.
        /// </summary>
        NumPad4Key = 1 << 10,
        /// <summary>
        /// The keyboard's NumPad 5 key flag.
        /// </summary>
        NumPad5Key = 1 << 11,
        /// <summary>
        /// The keyboard's NumPad 6 key flag.
        /// </summary>
        NumPad6Key = 1 << 12,
        /// <summary>
        /// The keyboard's NumPad 7 key flag.
        /// </summary>
        NumPad7Key = 1 << 13,
        /// <summary>
        /// The keyboard's NumPad 8 key flag.
        /// </summary>
        NumPad8Key = 1 << 14,
        /// <summary>
        /// The keyboard's NumPad 9 key flag.
        /// </summary>
        NumPad9Key = 1 << 15
    }

    /// <summary>
    /// The keyboard command key flags. Used in event handling of keyboard input. See <see cref="EnterKey"/> and <see cref="SpaceKey"/>.
    /// </summary>
    [Flags]
    public enum InputKeyboardCommandFlags
    {
        /// <summary>
        /// The keyboard's Insert key flag.
        /// </summary>
        InsertKey = 1 << 0,
        /// <summary>
        /// The keyboard's Home key flag.
        /// </summary>
        HomeKey = 1 << 1,
        /// <summary>
        /// The keyboard's Page Up key flag.
        /// </summary>
        PageUpKey = 1 << 2,
        /// <summary>
        /// The keyboard's Delete key flag.
        /// </summary>
        DeleteKey = 1 << 3,
        /// <summary>
        /// The keyboard's End key flag.
        /// </summary>
        EndKey = 1 << 4,
        /// <summary>
        /// The keyboard's Page Down key flag.
        /// </summary>
        PageDownKey = 1 << 5,
        /// <summary>
        /// The keyboard's Print Screen key flag.
        /// </summary>
        PrintScreenKey = 1 << 6,
        /// <summary>
        /// The keyboard's Pause key flag.
        /// </summary>
        PauseKey = 1 << 7,
        /// <summary>
        /// The keyboard's Scroll Lock key flag.
        /// </summary>
        ScrollLockKey = 1 << 8,
        /// <summary>
        /// The keyboard's Oem Tilde key flag.
        /// </summary>
        OemTildeKey = 1 << 9,
        /// <summary>
        /// The keyboard's Oem Minus key flag.
        /// </summary>
        OemMinusKey = 1 << 10,
        /// <summary>
        /// The keyboard's Oem Plus key flag.
        /// </summary>
        OemPlusKey = 1 << 11,
        /// <summary>
        /// The keyboard's Backspace key flag.
        /// </summary>
        BackspaceKey = 1 << 12,
        /// <summary>
        /// The keyboard's Tab key flag.
        /// </summary>
        TabKey = 1 << 13,
        /// <summary>
        /// The keyboard's Oem Open Brackets key flag.
        /// </summary>
        OemOpenBracketsKey = 1 << 14,
        /// <summary>
        /// The keyboard's Oem Close Brackets key flag.
        /// </summary>
        OemCloseBracketsKey = 1 << 15,
        /// <summary>
        /// The keyboard's Oem Pipe key flag.
        /// </summary>
        OemPipeKey = 1 << 16,
        /// <summary>
        /// The keyboard's Caps Lock key flag.
        /// </summary>
        CapsLockKey = 1 << 17,
        /// <summary>
        /// The keyboard's Oem Semicolon key flag.
        /// </summary>
        OemSemicolonKey = 1 << 18,
        /// <summary>
        /// The keyboard's Oem Quotes key flag.
        /// </summary>
        OemQuotesKey = 1 << 19,
        /// <summary>
        /// The keyboard's Enter key flag.
        /// </summary>
        EnterKey = 1 << 20,
        /// <summary>
        /// The keyboard's Left Shift key flag.
        /// </summary>
        LeftShiftKey = 1 << 21,
        /// <summary>
        /// The keyboard's Oem Comma key flag.
        /// </summary>
        OemCommaKey = 1 << 22,
        /// <summary>
        /// The keyboard's Oem Period key flag.
        /// </summary>
        OemPeriodKey = 1 << 23,
        /// <summary>
        /// The keyboard's Oem Question key flag.
        /// </summary>
        OemQuestionKey = 1 << 24,
        /// <summary>
        /// The keyboard's Right Shift key flag.
        /// </summary>
        RightShiftKey = 1 << 25,
        /// <summary>
        /// The keyboard's Left CTRL key flag.
        /// </summary>
        LeftCtrlKey = 1 << 26,
        /// <summary>
        /// The keyboard's Left ALT key flag.
        /// </summary>
        LeftAltKey = 1 << 27,
        /// <summary>
        /// The keyboard's Space key flag.
        /// </summary>
        SpaceKey = 1 << 28,
        /// <summary>
        /// The keyboard's Right ALT key flag.
        /// </summary>
        RightAltKey = 1 << 29,
        /// <summary>
        /// The keyboard's Right CTRL key flag.
        /// </summary>
        RightCtrlKey = 1 << 30,
        /// <summary>
        /// The keyboard's Escape key flag.
        /// </summary>
        EscapeKey = 1 << 31
    }

    /// <summary>
    /// The keyboard special key flags. Used in event handling of keyboard input. See <see cref="LeftWindowsKey"/> and <see cref="RightWindowsKey"/>.
    /// </summary>
    [Flags]
    public enum InputKeyboardSpecialFlags
    {
        /// <summary>
        /// The keyboard's left Windows key flag.
        /// </summary>
        LeftWindowsKey = 1 << 0,
        /// <summary>
        /// The keyboard's right Windows key flag.
        /// </summary>
        RightWindowsKey = 1 << 1
    }

    /// <summary>
    /// The keyboard arrow key flags. Used in event handling of keyboard input. See <see cref="UpKey"/> and <see cref="LeftKey"/>.
    /// </summary>
    [Flags]
    public enum InputKeyboardArrowFlags : byte
    {
        /// <summary>
        /// The keyboard's Up key flag.
        /// </summary>
        UpKey = 1 << 0,
        /// <summary>
        /// The keyboard's Down key flag.
        /// </summary>
        DownKey = 1 << 1,
        /// <summary>
        /// The keyboard's Left key flag.
        /// </summary>
        LeftKey = 1 << 2,
        /// <summary>
        /// The keyboard's Right key flag.
        /// </summary>
        RightKey = 1 << 3
    }

    /// <summary>
    /// The keyboard letter key flags. Used in event handling of keyboard input. See <see cref="AKey"/> and <see cref="ZKey"/>.
    /// </summary>
    [Flags]
    public enum InputKeyboardLetterFlags
    {
        /// <summary>
        /// The keyboard's Q key flag.
        /// </summary>
        QKey = 1 << 0,
        /// <summary>
        /// The keyboard's W key flag.
        /// </summary>
        WKey = 1 << 1,
        /// <summary>
        /// The keyboard's E key flag.
        /// </summary>
        EKey = 1 << 2,
        /// <summary>
        /// The keyboard's R key flag.
        /// </summary>
        RKey = 1 << 3,
        /// <summary>
        /// The keyboard's T key flag.
        /// </summary>
        TKey = 1 << 4,
        /// <summary>
        /// The keyboard's Y key flag.
        /// </summary>
        YKey = 1 << 5,
        /// <summary>
        /// The keyboard's U key flag.
        /// </summary>
        UKey = 1 << 6,
        /// <summary>
        /// The keyboard's I key flag.
        /// </summary>
        IKey = 1 << 7,
        /// <summary>
        /// The keyboard's O key flag.
        /// </summary>
        OKey = 1 << 8,
        /// <summary>
        /// The keyboard's P key flag.
        /// </summary>
        PKey = 1 << 9,
        /// <summary>
        /// The keyboard's A key flag.
        /// </summary>
        AKey = 1 << 10,
        /// <summary>
        /// The keyboard's S key flag.
        /// </summary>
        SKey = 1 << 11,
        /// <summary>
        /// The keyboard's D key flag.
        /// </summary>
        DKey = 1 << 12,
        /// <summary>
        /// The keyboard's F key flag.
        /// </summary>
        FKey = 1 << 13,
        /// <summary>
        /// The keyboard's G key flag.
        /// </summary>
        GKey = 1 << 14,
        /// <summary>
        /// The keyboard's H key flag.
        /// </summary>
        HKey = 1 << 15,
        /// <summary>
        /// The keyboard's J key flag.
        /// </summary>
        JKey = 1 << 16,
        /// <summary>
        /// The keyboard's K key flag.
        /// </summary>
        KKey = 1 << 17,
        /// <summary>
        /// The keyboard's L key flag.
        /// </summary>
        LKey = 1 << 18,
        /// <summary>
        /// The keyboard's Z key flag.
        /// </summary>
        ZKey = 1 << 19,
        /// <summary>
        /// The keyboard's X key flag.
        /// </summary>
        XKey = 1 << 20,
        /// <summary>
        /// The keyboard's C key flag.
        /// </summary>
        CKey = 1 << 21,
        /// <summary>
        /// The keyboard's V key flag.
        /// </summary>
        VKey = 1 << 22,
        /// <summary>
        /// The keyboard's B key flag.
        /// </summary>
        BKey = 1 << 23,
        /// <summary>
        /// The keyboard's N key flag.
        /// </summary>
        NKey = 1 << 24,
        /// <summary>
        /// The keyboard's M key flag.
        /// </summary>
        MKey = 1 << 25
    }

    /// <summary>
    /// The keyboard number key flags. Used in event handling of keyboard input. See <see cref="D0Key"/> and <see cref="D9Key"/>.
    /// </summary>
    [Flags]
    public enum InputKeyboardNumberFlags
    {
        /// <summary>
        /// The keyboard's D1 key flag.
        /// </summary>
        D1Key = 1 << 0,
        /// <summary>
        /// The keyboard's D2 key flag.
        /// </summary>
        D2Key = 1 << 1,
        /// <summary>
        /// The keyboard's D3 key  flag.
        /// </summary>
        D3Key = 1 << 2,
        /// <summary>
        /// The keyboard's D4 key flag.
        /// </summary>
        D4Key = 1 << 3,
        /// <summary>
        /// The keyboard's D5 key flag.
        /// </summary>
        D5Key = 1 << 4,
        /// <summary>
        /// The keyboard's D6 key flag.
        /// </summary>
        D6Key = 1 << 5,
        /// <summary>
        /// The keyboard's D7 key flag.
        /// </summary>
        D7Key = 1 << 6,
        /// <summary>
        /// The keyboard's D8 key flag.
        /// </summary>
        D8Key = 1 << 7,
        /// <summary>
        /// The keyboard's D9 key flag.
        /// </summary>
        D9Key = 1 << 8,
        /// <summary>
        /// The keyboard's D0 key flag.
        /// </summary>
        D0Key = 1 << 9
    }

    /// <summary>
    /// The physical actions performed by the gamepad such as <see cref="AButton"/> and <see cref="BButton"/>.
    /// </summary>
    [Flags]
    public enum InputGamepadActionFlags
    {
        /// <summary>
        /// The gamepad's idle flag.
        /// </summary>
        None = 0,
        /// <summary>
        /// The gamepad's A button flag.
        /// </summary>
        AButton = 1 << 0,
        /// <summary>
        /// The gamepad's B button flag.
        /// </summary>
        BButton = 1 << 1,
        /// <summary>
        /// The gamepad's X button flag.
        /// </summary>
        XButton = 1 << 2,
        /// <summary>
        /// The gamepad's Y button flag.
        /// </summary>
        YButton = 1 << 3,
        /// <summary>
        /// The gamepad's left analog stick button flag.
        /// </summary>
        AnalogLeftStickButton = 1 << 4,
        /// <summary>
        /// The gamepad's right analog stick button flag.
        /// </summary>
        AnalogRightStickButton = 1 << 5,
        /// <summary>
        /// The gamepad's left trigger button flag.
        /// </summary>
        TriggerLeftButton = 1 << 6,
        /// <summary>
        /// The gamepad's right trigger button flag.
        /// </summary>
        TriggerRightButton = 1 << 7,
        /// <summary>
        /// The gamepad's left shoulder button flag.
        /// </summary>
        ShoulderLeftButton = 1 << 8,
        /// <summary>
        /// The gamepad's right shoulder button flag.
        /// </summary>
        ShoulderRightButton = 1 << 9,
        /// <summary>
        /// The gamepad's d-pad's up button flag.
        /// </summary>
        DPadUpButton = 1 << 10,
        /// <summary>
        /// The gamepad's d-pad's down button flag.
        /// </summary>
        DPadDownButton = 1 << 11,
        /// <summary>
        /// The gamepad's d-pad's left button flag.
        /// </summary>
        DPadLeftButton = 1 << 12,
        /// <summary>
        /// The gamepad's d-pad's right button flag.
        /// </summary>
        DPadRightButton = 1 << 13,
        /// <summary>
        /// The gamepad's left trigger pressure flag.
        /// </summary>
        TriggerLeftPressure = 1 << 14,
        /// <summary>
        /// The gamepad's right trigger pressure flag.
        /// </summary>
        TriggerRightPressure = 1 << 15,
        /// <summary>
        /// The gamepad's left analog stick up flag.
        /// </summary>
        AnalogLeftStickUp = 1 << 16,
        /// <summary>
        /// The gamepad's left analog stick down flag.
        /// </summary>
        AnalogLeftStickDown = 1 << 17,
        /// <summary>
        /// The gamepad's left analog stick left flag.
        /// </summary>
        AnalogLeftStickLeft = 1 << 18,
        /// <summary>
        /// The gamepad's left analog stick right flag.
        /// </summary>
        AnalogLeftStickRight = 1 << 19,
        /// <summary>
        /// The gamepad's right analog stick up flag.
        /// </summary>
        AnalogRightStickUp = 1 << 20,
        /// <summary>
        /// The gamepad's right analog stick down flag.
        /// </summary>
        AnalogRightStickDown = 1 << 21,
        /// <summary>
        /// The gamepad's right analog stick left flag.
        /// </summary>
        AnalogRightStickLeft = 1 << 22,
        /// <summary>
        /// The gamepad's right analog stick right flag.
        /// </summary>
        AnalogRightStickRight = 1 << 23
    }

    /// <summary>
    /// The available action states for input commands.
    /// </summary>
    [Flags]
    public enum InputActionStateFlags
    {
        /// <summary>
        /// The idle state flag.
        /// </summary>
        Idle,
        /// <summary>
        /// The press state flag.
        /// </summary>
        Press,
        /// <summary>
        /// The release state flag.
        /// </summary>
        Release,
        /// <summary>
        /// The held state flag.
        /// </summary>
        Held
    }

    /// <summary>
    /// The available scroll directions.
    /// </summary>
    public enum ScrollDirections
    {
        /// <summary>
        /// The vertical scroll option.
        /// </summary>
        Vertical,
        /// <summary>
        /// The horizontal scroll option.
        /// </summary>
        Horizontal
    }

    /// <summary>
    /// The input flags used for key assignment and actions.
    /// </summary>
    public class InputFlags
    {
        #region Mapped Controls

        /// <summary>
        /// Passes the mapped confirmation command flags to the subscribed objects.
        /// </summary>
        public InputMappableConfirmationCommandFlags MappedConfirmationCommandFlags { get; private set; } = 0;

        /// <summary>
        /// Passes the mapped camera command flags to the subscribed objects.
        /// </summary>
        public InputMappableMovementCommandFlags MappedMovementCommandFlags { get; private set; } = 0;

        /// <summary>
        /// Passes the mapped camera command flags to the subscribed objects.
        /// </summary>
        public InputMappableCameraCommandFlags MappedCameraCommandFlags { get; private set; } = 0;

        #endregion

        #region Input Flags

        /// <summary>
        /// Passes the set keyboard function input flags to the subscribed objects.
        /// </summary>
        public InputKeyboardFunctionFlags KeyboardFunctionFlags { get; private set; } = 0;

        /// <summary>
        /// Passes the set keyboard NumPad input flags to the subscribed objects.
        /// </summary>
        public InputKeyboardNumPadFlags KeyboardNumPadFlags { get; private set; } = 0;

        /// <summary>
        /// Passes the set keyboard numbers input flags to the subscribed objects.
        /// </summary>
        public InputKeyboardNumberFlags KeyboardNumberFlags { get; private set; } = 0;

        /// <summary>
        /// Passes the set keyboard command input flags to the subscribed objects.
        /// </summary>
        public InputKeyboardCommandFlags KeyboardCommandFlags { get; private set; } = 0;

        /// <summary>
        /// Passes the set keyboard special input flags to the subscribed objects.
        /// </summary>
        public InputKeyboardSpecialFlags KeyboardSpecialFlags { get; private set; } = 0;

        /// <summary>
        /// Passes the set keyboard arrow input flags to the subscribed objects.
        /// </summary>
        public InputKeyboardArrowFlags KeyboardArrowFlags { get; private set; } = 0;

        /// <summary>
        /// Passes the set keyboard letters input flags to the subscribed objects.
        /// </summary>
        public InputKeyboardLetterFlags KeyboardLetterFlags { get; private set; } = 0;

        /// <summary>
        /// Passes the set gamepad action input flags to the subscribed objects.
        /// </summary>
        public InputGamepadActionFlags GamepadActionFlags { get; private set; } = 0;

        /// <summary>
        /// Passes the set mouse action input flags to the subscribed objects.
        /// </summary>
        public InputMouseActionFlags MouseActionFlags { get; private set; } = 0;

        #endregion
        
        /// <summary>
        /// Input flags for keyboards, mice and gamepads.
        /// </summary>
        public InputFlags()
        {
            
        }
        
        #region Input Action State Flags

        /// <summary>
        /// Passes the set input state flags to the subscribed objects.
        /// </summary>
        public InputActionStateFlags ActionStateFlags { get; private set; } = 0;

        /// <summary>
        /// Sets the keyboard flag.
        /// </summary>
        /// <param name="flag">The flag to check, if set in the <see cref="InputActionStateFlags"/> property.</param>
        /// <param name="result">The result of the operation.</param>
        /// <returns>Returns the result of operation as a <see cref="bool"/>.</returns>
        public bool SetFlag(InputActionStateFlags flag, bool result)
        {
            if (result)
            {
                AddFlag(flag);
            }

            return result;
        }

        /// <summary>
        /// Determines whether the <see cref="InputActionStateFlags"/> flag is set.
        /// </summary>
        /// <param name="flag">The flag to check, if set in the <see cref="InputActionStateFlags"/> property.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the flag is set.</returns>
        public bool IsFlagSet(InputActionStateFlags flag)
        {
            return (ActionStateFlags & flag) == flag;
        }

        /// <summary>
        /// Removes the <see cref="InputActionStateFlags"/> flag, if set.
        /// </summary>
        /// <param name="flag">The flag to remove, if set in the <see cref="InputActionStateFlags"/> property.</param>
        public void RemoveFlag(InputActionStateFlags flag)
        {
            if (IsFlagSet(flag))
            {
                ActionStateFlags &= ~flag;
            }
        }

        /// <summary>
        /// Adds the <see cref="InputActionStateFlags"/> flag.
        /// </summary>
        /// <param name="flag">The flag to add in the <see cref="InputActionStateFlags"/> property.</param>
        public void AddFlag(InputActionStateFlags flag)
        {
            ActionStateFlags |= flag;
        }

        #endregion

        #region Input Keyboard Function Flag Methods
        
        /// <summary>
        /// Sets the keyboard flag.
        /// </summary>
        /// <param name="flag">The flag to check, if set in the <see cref="InputKeyboardFunctionFlags"/> property.</param>
        /// <param name="result">The result of the operation.</param>
        /// <returns>Returns the result of operation as a <see cref="bool"/>.</returns>
        public bool SetFlag(InputKeyboardFunctionFlags flag, bool result)
        {
            if (result)
            {
                AddFlag(flag);
            }

            return result;
        }

        /// <summary>
        /// Determines whether the <see cref="InputKeyboardFunctionFlags"/> flag is set.
        /// </summary>
        /// <param name="flag">The flag to check, if set in the <see cref="InputKeyboardFunctionFlags"/> property.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the flag is set.</returns>
        public bool IsFlagSet(InputKeyboardFunctionFlags flag)
        {
            return (KeyboardFunctionFlags & flag) == flag;
        }

        /// <summary>
        /// Removes the <see cref="InputKeyboardFunctionFlags"/> flag, if set.
        /// </summary>
        /// <param name="flag">The flag to remove, if set in the <see cref="InputKeyboardFunctionFlags"/> property.</param>
        public void RemoveFlag(InputKeyboardFunctionFlags flag)
        {
            if (IsFlagSet(flag))
            {
                KeyboardFunctionFlags &= ~flag;
            }
        }

        /// <summary>
        /// Adds the <see cref="InputKeyboardFunctionFlags"/> flag.
        /// </summary>
        /// <param name="flag">The flag to add in the <see cref="InputKeyboardFunctionFlags"/> property.</param>
        public void AddFlag(InputKeyboardFunctionFlags flag)
        {
            KeyboardFunctionFlags |= flag;
        }

        #endregion

        #region Input Keyboard NumPad Flag Methods

        /// <summary>
        /// Sets the keyboard flag.
        /// </summary>
        /// <param name="flag">The flag to check, if set in the <see cref="InputKeyboardNumPadFlags"/> property.</param>
        /// <param name="result">The result of the operation.</param>
        /// <returns>Returns the result of operation as a <see cref="bool"/>.</returns>
        public bool SetFlag(InputKeyboardNumPadFlags flag, bool result)
        {
            if (result)
            {
                AddFlag(flag);
            }

            return result;
        }

        /// <summary>
        /// Determines whether the <see cref="InputKeyboardNumPadFlags"/> flag is set.
        /// </summary>
        /// <param name="flag">The flag to check, if set in the <see cref="InputKeyboardNumPadFlags"/> property.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the flag is set.</returns>
        public bool IsFlagSet(InputKeyboardNumPadFlags flag)
        {
            return (KeyboardNumPadFlags & flag) == flag;
        }

        /// <summary>
        /// Removes the <see cref="InputKeyboardNumPadFlags"/> flag, if set.
        /// </summary>
        /// <param name="flag">The flag to remove, if set in the <see cref="InputKeyboardNumPadFlags"/> property.</param>
        public void RemoveFlag(InputKeyboardNumPadFlags flag)
        {
            if (IsFlagSet(flag))
            {
                KeyboardNumPadFlags &= ~flag;
            }
        }

        /// <summary>
        /// Adds the <see cref="InputKeyboardNumPadFlags"/> flag.
        /// </summary>
        /// <param name="flag">The flag to add in the <see cref="InputKeyboardNumPadFlags"/> property.</param>
        public void AddFlag(InputKeyboardNumPadFlags flag)
        {
            KeyboardNumPadFlags |= flag;
        }

        #endregion

        #region Input Keyboard Number Flag Methods

        /// <summary>
        /// Sets the keyboard flag.
        /// </summary>
        /// <param name="flag">The flag to check, if set in the <see cref="InputKeyboardNumberFlags"/> property.</param>
        /// <param name="result">The result of the operation.</param>
        /// <returns>Returns the result of operation as a <see cref="bool"/>.</returns>
        public bool SetFlag(InputKeyboardNumberFlags flag, bool result)
        {
            if (result)
            {
                AddFlag(flag);
            }

            return result;
        }

        /// <summary>
        /// Determines whether the <see cref="InputKeyboardNumberFlags"/> flag is set.
        /// </summary>
        /// <param name="flag">The flag to check, if set in the <see cref="InputKeyboardNumberFlags"/> property.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the flag is set.</returns>
        public bool IsFlagSet(InputKeyboardNumberFlags flag)
        {
            return (KeyboardNumberFlags & flag) == flag;
        }

        /// <summary>
        /// Removes the <see cref="InputKeyboardNumberFlags"/> flag, if set.
        /// </summary>
        /// <param name="flag">The flag to remove, if set in the <see cref="InputKeyboardNumberFlags"/> property.</param>
        public void RemoveFlag(InputKeyboardNumberFlags flag)
        {
            if (IsFlagSet(flag))
            {
                KeyboardNumberFlags &= ~flag;
            }
        }

        /// <summary>
        /// Adds the <see cref="InputKeyboardNumberFlags"/> flag.
        /// </summary>
        /// <param name="flag">The flag to add in the <see cref="InputKeyboardNumberFlags"/> property.</param>
        public void AddFlag(InputKeyboardNumberFlags flag)
        {
            KeyboardNumberFlags |= flag;
        }

        #endregion

        #region Input Keyboard Command Flag Methods

        /// <summary>
        /// Sets the keyboard flag.
        /// </summary>
        /// <param name="flag">The flag to check, if set in the <see cref="InputKeyboardCommandFlags"/> property.</param>
        /// <param name="result">The result of the operation.</param>
        /// <returns>Returns the result of operation as a <see cref="bool"/>.</returns>
        public bool SetFlag(InputKeyboardCommandFlags flag, bool result)
        {
            if (result)
            {
                AddFlag(flag);
            }

            return result;
        }

        /// <summary>
        /// Determines whether the <see cref="InputKeyboardCommandFlags"/> flag is set.
        /// </summary>
        /// <param name="flag">The flag to check, if set in the <see cref="InputKeyboardCommandFlags"/> property.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the flag is set.</returns>
        public bool IsFlagSet(InputKeyboardCommandFlags flag)
        {
            return (KeyboardCommandFlags & flag) == flag;
        }

        /// <summary>
        /// Removes the <see cref="InputKeyboardCommandFlags"/> flag, if set.
        /// </summary>
        /// <param name="flag">The flag to remove, if set in the <see cref="InputKeyboardCommandFlags"/> property.</param>
        public void RemoveFlag(InputKeyboardCommandFlags flag)
        {
            if (IsFlagSet(flag))
            {
                KeyboardCommandFlags &= ~flag;
            }
        }

        /// <summary>
        /// Adds the <see cref="InputKeyboardCommandFlags"/> flag.
        /// </summary>
        /// <param name="flag">The flag to add in the <see cref="InputKeyboardCommandFlags"/> property.</param>
        public void AddFlag(InputKeyboardCommandFlags flag)
        {
            KeyboardCommandFlags |= flag;
        }

        #endregion

        #region Input Keyboard Special Flag Methods

        /// <summary>
        /// Sets the keyboard flag.
        /// </summary>
        /// <param name="flag">The flag to check, if set in the <see cref="InputKeyboardSpecialFlags"/> property.</param>
        /// <param name="result">The result of the operation.</param>
        /// <returns>Returns the result of operation as a <see cref="bool"/>.</returns>
        public bool SetFlag(InputKeyboardSpecialFlags flag, bool result)
        {
            if (result)
            {
                AddFlag(flag);
            }

            return result;
        }

        /// <summary>
        /// Determines whether the <see cref="InputKeyboardSpecialFlags"/> flag is set.
        /// </summary>
        /// <param name="flag">The flag to check, if set in the <see cref="InputKeyboardSpecialFlags"/> property.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the flag is set.</returns>
        public bool IsFlagSet(InputKeyboardSpecialFlags flag)
        {
            return (KeyboardSpecialFlags & flag) == flag;
        }

        /// <summary>
        /// Removes the <see cref="InputKeyboardSpecialFlags"/> flag, if set.
        /// </summary>
        /// <param name="flag">The flag to remove, if set in the <see cref="InputKeyboardSpecialFlags"/> property.</param>
        public void RemoveFlag(InputKeyboardSpecialFlags flag)
        {
            if (IsFlagSet(flag))
            {
                KeyboardSpecialFlags &= ~flag;
            }
        }

        /// <summary>
        /// Adds the <see cref="InputKeyboardSpecialFlags"/> flag.
        /// </summary>
        /// <param name="flag">The flag to add in the <see cref="InputKeyboardSpecialFlags"/> property.</param>
        public void AddFlag(InputKeyboardSpecialFlags flag)
        {
            KeyboardSpecialFlags |= flag;
        }

        #endregion

        #region Input Keyboard Arrow Flag Methods

        /// <summary>
        /// Sets the keyboard flag.
        /// </summary>
        /// <param name="flag">The flag to check, if set in the <see cref="InputKeyboardArrowFlags"/> property.</param>
        /// <param name="result">The result of the operation.</param>
        /// <returns>Returns the result of operation as a <see cref="bool"/>.</returns>
        public bool SetFlag(InputKeyboardArrowFlags flag, bool result)
        {
            if (result)
            {
                AddFlag(flag);
            }

            return result;
        }

        /// <summary>
        /// Determines whether the <see cref="InputKeyboardArrowFlags"/> flag is set.
        /// </summary>
        /// <param name="flag">The flag to check, if set in the <see cref="InputKeyboardArrowFlags"/> property.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the flag is set.</returns>
        public bool IsFlagSet(InputKeyboardArrowFlags flag)
        {
            return (KeyboardArrowFlags & flag) == flag;
        }

        /// <summary>
        /// Removes the <see cref="InputKeyboardArrowFlags"/> flag, if set.
        /// </summary>
        /// <param name="flag">The flag to remove, if set in the <see cref="InputKeyboardArrowFlags"/> property.</param>
        public void RemoveFlag(InputKeyboardArrowFlags flag)
        {
            if (IsFlagSet(flag))
            {
                KeyboardArrowFlags &= ~flag;
            }
        }

        /// <summary>
        /// Adds the <see cref="InputKeyboardArrowFlags"/> flag.
        /// </summary>
        /// <param name="flag">The flag to add in the <see cref="InputKeyboardArrowFlags"/> property.</param>
        public void AddFlag(InputKeyboardArrowFlags flag)
        {
            KeyboardArrowFlags |= flag;
        }

        #endregion

        #region Input Keyboard Letter Flag Methods

        /// <summary>
        /// Sets the keyboard flag.
        /// </summary>
        /// <param name="flag">The flag to check, if set in the <see cref="InputKeyboardLetterFlags"/> property.</param>
        /// <param name="result">The result of the operation.</param>
        /// <returns>Returns the result of operation as a <see cref="bool"/>.</returns>
        public bool SetFlag(InputKeyboardLetterFlags flag, bool result)
        {
            if (result)
            {
                AddFlag(flag);
            }

            return result;
        }

        /// <summary>
        /// Determines whether the <see cref="InputKeyboardLetterFlags"/> flag is set.
        /// </summary>
        /// <param name="flag">The flag to check, if set in the <see cref="InputKeyboardLetterFlags"/> property.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the flag is set.</returns>
        public bool IsFlagSet(InputKeyboardLetterFlags flag)
        {
            return (KeyboardLetterFlags & flag) == flag;
        }

        /// <summary>
        /// Removes the <see cref="InputKeyboardLetterFlags"/> flag, if set.
        /// </summary>
        /// <param name="flag">The flag to remove, if set in the <see cref="InputKeyboardLetterFlags"/> property.</param>
        public void RemoveFlag(InputKeyboardLetterFlags flag)
        {
            if (IsFlagSet(flag))
            {
                KeyboardLetterFlags &= ~flag;
            }
        }

        /// <summary>
        /// Adds the <see cref="InputKeyboardLetterFlags"/> flag.
        /// </summary>
        /// <param name="flag">The flag to add in the <see cref="InputKeyboardLetterFlags"/> property.</param>
        public void AddFlag(InputKeyboardLetterFlags flag)
        {
            KeyboardLetterFlags |= flag;
        }

        #endregion
        
        #region Input Gamepad Action Flag Methods

        /// <summary>
        /// Sets the gamepad action flag.
        /// </summary>
        /// <param name="flag">The flag to check, if set in the <see cref="InputGamepadActionFlags"/> property.</param>
        /// <param name="result">The result of the operation.</param>
        /// <returns>Returns the result of operation as a <see cref="bool"/>.</returns>
        public bool SetFlag(InputGamepadActionFlags flag, bool result)
        {
            if (result)
            {
                AddFlag(flag);
            }

            return result;
        }

        /// <summary>
        /// Determines whether the <see cref="InputGamepadActionFlags"/> flag is set.
        /// </summary>
        /// <param name="flag">The flag to check, if set in the <see cref="InputGamepadActionFlags"/> property.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the flag is set.</returns>
        public bool IsFlagSet(InputGamepadActionFlags flag)
        {
            return (GamepadActionFlags & flag) == flag;
        }

        /// <summary>
        /// Removes the <see cref="InputGamepadActionFlags"/> flag, if set.
        /// </summary>
        /// <param name="flag">The flag to remove, if set in the <see cref="InputGamepadActionFlags"/> property.</param>
        public void RemoveFlag(InputGamepadActionFlags flag)
        {
            if (IsFlagSet(flag))
            {
                GamepadActionFlags &= ~flag;
            }
        }

        /// <summary>
        /// Adds the <see cref="InputGamepadActionFlags"/> flag.
        /// </summary>
        /// <param name="flag">The flag to add in the <see cref="InputGamepadActionFlags"/> property.</param>
        public void AddFlag(InputGamepadActionFlags flag)
        {
            GamepadActionFlags |= flag;
        }

        #endregion

        #region Input Mouse Action Flag Methods

        /// <summary>
        /// Sets the mouse action flag.
        /// </summary>
        /// <param name="flag">The flag to check, if set in the <see cref="InputMouseActionFlags"/> property.</param>
        /// <param name="result">The result of the operation.</param>
        /// <returns>Returns the result of operation as a <see cref="bool"/>.</returns>
        public bool SetFlag(InputMouseActionFlags flag, bool result)
        {
            if (result)
            {
                AddFlag(flag);
            }

            return result;
        }

        /// <summary>
        /// Determines whether the <see cref="InputMouseActionFlags"/> flag is set.
        /// </summary>
        /// <param name="flag">The flag to check, if set in the <see cref="InputMouseActionFlags"/> property.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the flag is set.</returns>
        public bool IsFlagSet(InputMouseActionFlags flag)
        {
            return (MouseActionFlags & flag) == flag;
        }

        /// <summary>
        /// Removes the <see cref="InputMouseActionFlags"/> flag, if set.
        /// </summary>
        /// <param name="flag">The flag to remove, if set in the <see cref="InputMouseActionFlags"/> property.</param>
        public void RemoveFlag(InputMouseActionFlags flag)
        {
            if (IsFlagSet(flag))
            {
                MouseActionFlags &= ~flag;
            }
        }

        /// <summary>
        /// Adds the <see cref="InputMouseActionFlags"/> flag.
        /// </summary>
        /// <param name="flag">The flag to add in the <see cref="InputMouseActionFlags"/> property.</param>
        public void AddFlag(InputMouseActionFlags flag)
        {
            MouseActionFlags |= flag;
        }

        #endregion

        #region Input Mapped Confirmation Command Flag Methods

        /// <summary>
        /// Sets the mapped confirmation command flag.
        /// </summary>
        /// <param name="flag">The flag to check, if set in the <see cref="InputMappableConfirmationCommandFlags"/> property.</param>
        /// <param name="result">The result of the operation.</param>
        /// <returns>Returns the result of operation as a <see cref="bool"/>.</returns>
        public bool SetFlag(InputMappableConfirmationCommandFlags flag, bool result)
        {
            if (result)
            {
                AddFlag(flag);
            }

            return result;
        }

        /// <summary>
        /// Determines whether the <see cref="InputMappableConfirmationCommandFlags"/> flag is set.
        /// </summary>
        /// <param name="flag">The flag to check, if set in the <see cref="InputMappableConfirmationCommandFlags"/> property.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the flag is set.</returns>
        public bool IsFlagSet(InputMappableConfirmationCommandFlags flag)
        {
            return (MappedConfirmationCommandFlags & flag) == flag;
        }

        /// <summary>
        /// Removes the <see cref="InputMappableConfirmationCommandFlags"/> flag, if set.
        /// </summary>
        /// <param name="flag">The flag to remove, if set in the <see cref="InputMappableConfirmationCommandFlags"/> property.</param>
        public void RemoveFlag(InputMappableConfirmationCommandFlags flag)
        {
            if (IsFlagSet(flag))
            {
                MappedConfirmationCommandFlags &= ~flag;
            }
        }

        /// <summary>
        /// Adds the <see cref="InputMappableConfirmationCommandFlags"/> flag.
        /// </summary>
        /// <param name="flag">The flag to add in the <see cref="InputMappableConfirmationCommandFlags"/> property.</param>
        public void AddFlag(InputMappableConfirmationCommandFlags flag)
        {
            MappedConfirmationCommandFlags |= flag;
        }

        #endregion

        #region Input Mapped Movement Command Flag Methods

        /// <summary>
        /// Sets the mapped movement command flag.
        /// </summary>
        /// <param name="flag">The flag to check, if set in the <see cref="InputMappableMovementCommandFlags"/> property.</param>
        /// <param name="result">The result of the operation.</param>
        /// <returns>Returns the result of operation as a <see cref="bool"/>.</returns>
        public bool SetFlag(InputMappableMovementCommandFlags flag, bool result)
        {
            if (result)
            {
                AddFlag(flag);
            }

            return result;
        }

        /// <summary>
        /// Determines whether the <see cref="InputMappableMovementCommandFlags"/> flag is set.
        /// </summary>
        /// <param name="flag">The flag to check, if set in the <see cref="InputMappableMovementCommandFlags"/> property.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the flag is set.</returns>
        public bool IsFlagSet(InputMappableMovementCommandFlags flag)
        {
            return (MappedMovementCommandFlags & flag) == flag;
        }

        /// <summary>
        /// Removes the <see cref="InputMappableMovementCommandFlags"/> flag, if set.
        /// </summary>
        /// <param name="flag">The flag to remove, if set in the <see cref="InputMappableMovementCommandFlags"/> property.</param>
        public void RemoveFlag(InputMappableMovementCommandFlags flag)
        {
            if (IsFlagSet(flag))
            {
                MappedMovementCommandFlags &= ~flag;
            }
        }

        /// <summary>
        /// Adds the <see cref="InputMappableMovementCommandFlags"/> flag.
        /// </summary>
        /// <param name="flag">The flag to add in the <see cref="InputMappableMovementCommandFlags"/> property.</param>
        public void AddFlag(InputMappableMovementCommandFlags flag)
        {
            MappedMovementCommandFlags |= flag;
        }

        #endregion

        #region Input Mapped Camera Command Flag Methods

        /// <summary>
        /// Sets the mapped camera command flag.
        /// </summary>
        /// <param name="flag">The flag to check, if set in the <see cref="InputMappableCameraCommandFlags"/> property.</param>
        /// <param name="result">The result of the operation.</param>
        /// <returns>Returns the result of operation as a <see cref="bool"/>.</returns>
        public bool SetFlag(InputMappableCameraCommandFlags flag, bool result)
        {
            if (result)
            {
                AddFlag(flag);
            }

            return result;
        }

        /// <summary>
        /// Determines whether the <see cref="InputMappableCameraCommandFlags"/> flag is set.
        /// </summary>
        /// <param name="flag">The flag to check, if set in the <see cref="InputMappableCameraCommandFlags"/> property.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the flag is set.</returns>
        public bool IsFlagSet(InputMappableCameraCommandFlags flag)
        {
            return (MappedCameraCommandFlags & flag) == flag;
        }

        /// <summary>
        /// Removes the <see cref="InputMappableCameraCommandFlags"/> flag, if set.
        /// </summary>
        /// <param name="flag">The flag to remove, if set in the <see cref="InputMappableCameraCommandFlags"/> property.</param>
        public void RemoveFlag(InputMappableCameraCommandFlags flag)
        {
            if (IsFlagSet(flag))
            {
                MappedCameraCommandFlags &= ~flag;
            }
        }

        /// <summary>
        /// Adds the <see cref="InputMappableCameraCommandFlags"/> flag.
        /// </summary>
        /// <param name="flag">The flag to add in the <see cref="InputMappableCameraCommandFlags"/> property.</param>
        public void AddFlag(InputMappableCameraCommandFlags flag)
        {
            MappedCameraCommandFlags |= flag;
        }

        #endregion

        /// <summary>
        /// Clears all flags.
        /// </summary>
        public void Clear()
        {
            ActionStateFlags = 0;
            KeyboardFunctionFlags = 0;
            KeyboardNumPadFlags = 0;
            KeyboardNumberFlags = 0;
            KeyboardCommandFlags = 0;
            KeyboardSpecialFlags = 0;
            KeyboardArrowFlags = 0;
            KeyboardLetterFlags = 0;
            GamepadActionFlags = 0;
            MouseActionFlags = 0;
            MappedConfirmationCommandFlags = 0;
            MappedMovementCommandFlags = 0;
            MappedCameraCommandFlags = 0;
        }

        /// <summary>
        /// Copies the input flags of the passed in <see cref="InputFlags"/>.
        /// </summary>
        /// <param name="input">The <see cref="InputFlags"/> to copy.</param>
        public void Copy(InputFlags input)
        {
            ActionStateFlags = input.ActionStateFlags;
            KeyboardFunctionFlags = input.KeyboardFunctionFlags;
            KeyboardNumPadFlags = input.KeyboardNumPadFlags;
            KeyboardNumberFlags = input.KeyboardNumberFlags;
            KeyboardCommandFlags = input.KeyboardCommandFlags;
            KeyboardSpecialFlags = input.KeyboardSpecialFlags;
            KeyboardArrowFlags = input.KeyboardArrowFlags;
            KeyboardLetterFlags = input.KeyboardLetterFlags;
            GamepadActionFlags = input.GamepadActionFlags;
            MouseActionFlags = input.MouseActionFlags;
            MappedConfirmationCommandFlags = input.MappedConfirmationCommandFlags;
            MappedMovementCommandFlags = input.MappedMovementCommandFlags;
            MappedCameraCommandFlags = input.MappedCameraCommandFlags;
        }
    }
}