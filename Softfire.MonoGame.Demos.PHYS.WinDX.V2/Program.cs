﻿using System;

namespace Softfire.MonoGame.Demos.PHYS.WinDX.V2
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using var game = new Demo();
            game.Run();
        }
    }
}