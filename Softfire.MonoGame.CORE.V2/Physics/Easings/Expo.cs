﻿// TERMS OF USE - EASING EQUATIONS
// 
// Open source under the BSD License. 
// 
// Copyright © 2001 Robert Penner
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// 
// Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
// Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
// Neither the name of the author nor the names of contributors may be used to endorse or promote products derived from this software without specific prior written permission.
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;

namespace Softfire.MonoGame.CORE.V2.Physics.Easings
{
    /// <summary>
    /// An easing curve for an exponential function, accelerating to and from zero velocity.
    /// </summary>
    public static class Expo
    {
        /// <summary>
        /// The In function performs an exponential easing over time.
        /// Used to accelerate the interpolation from zero velocity.
        /// </summary>
        /// <param name="t">The current time or position. Intaken as a <see cref="double"/>.</param>
        /// <param name="b">The initial starting value for the easing. Intaken as a <see cref="double"/>.</param>
        /// <param name="c">The change in value to occur over the duration of the easing. Intaken as a <see cref="double"/>.</param>
        /// <param name="d">The amount of time, in seconds, to perform the easing. Intaken as a <see cref="double"/>.</param>
        /// <returns>Returns the eased value as a <see cref="double"/>.</returns>
        public static double In(double t, double b, double c, double d)
        {
            return (t == 0) ? b : c * Math.Pow(2d, 10d * (t / d - 1d)) + b;
        }

        /// <summary>
        /// The Out function performs an exponential easing over time.
        /// Used to decelerate the interpolation to zero velocity.
        /// </summary>
        /// <param name="t">The current time or position. Intaken as a <see cref="double"/>.</param>
        /// <param name="b">The initial starting value for the easing. Intaken as a <see cref="double"/>.</param>
        /// <param name="c">The change in value to occur over the duration of the easing. Intaken as a <see cref="double"/>.</param>
        /// <param name="d">The amount of time, in seconds, to perform the easing. Intaken as a <see cref="double"/>.</param>
        /// <returns>Returns the eased value as a <see cref="double"/>.</returns>
        public static double Out(double t, double b, double c, double d)
        {
            return (t == d) ? b + c : c * (-Math.Pow(2d, -10d * t / d) + 1d) + b;
        }

        /// <summary>
        /// The InOut function performs an <see cref="In"/> then an <see cref="Out"/>.
        /// </summary>
        /// <param name="t">The current time or position. Intaken as a <see cref="double"/>.</param>
        /// <param name="b">The initial starting value for the easing. Intaken as a <see cref="double"/>.</param>
        /// <param name="c">The change in value to occur over the duration of the easing. Intaken as a <see cref="double"/>.</param>
        /// <param name="d">The amount of time, in seconds, to perform the easing. Intaken as a <see cref="double"/>.</param>
        /// <returns>Returns the eased value as a <see cref="double"/>.</returns>
        public static double InOut(double t, double b, double c, double d)
        {
            if (t == 0)
            {
                return b;
            }

            if (t == d)
            {
                return b + c;
            }

            if ((t /= d / 2) < 1)
            {
                return c / 2d * Math.Pow(2d, 10d * (t - 1d)) + b;
            }

            return c / 2d * (-Math.Pow(2d, -10d * --t) + 2d) + b;
        }

        /// <summary>
        /// The OutIn function performs an <see cref="Out"/> then an <see cref="In"/>.
        /// </summary>
        /// <param name="t">The current time or position. Intaken as a <see cref="double"/>.</param>
        /// <param name="b">The initial starting value for the easing. Intaken as a <see cref="double"/>.</param>
        /// <param name="c">The change in value to occur over the duration of the easing. Intaken as a <see cref="double"/>.</param>
        /// <param name="d">The amount of time, in seconds, to perform the easing. Intaken as a <see cref="double"/>.</param>
        /// <returns>Returns the eased value as a <see cref="double"/>.</returns>
        public static double OutIn(double t, double b, double c, double d)
        {
            if (t < d / 2)
            {
                return Out(t * 2, b, c / 2, d);
            }

            return In((t * 2) - d, b + c / 2, c / 2, d);
        }
    }
}