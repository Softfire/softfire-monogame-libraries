using Microsoft.Xna.Framework;
using Softfire.MonoGame.CORE.V2.Input;

namespace Softfire.MonoGame.IO.V2
{
    public partial class IOCamera2D
    {
        /// <summary>
        /// Moves the camera.
        /// </summary>
        /// <param name="deltas">The input device's deltas. Intaken as a <see cref="Vector2"/>.</param>
        public void Move(Vector2 deltas) => Position -= Vector2.Transform(deltas, Matrix.CreateRotationZ(-RotationAngle));

        /// <summary>
        /// Focuses the target position.
        /// </summary>
        /// <param name="targetPosition">A target position to focus at the camera's center. Intaken as a <see cref="Vector2"/>.</param>
        public void FocusTarget(Vector2 targetPosition)
        {
            Position = targetPosition - new Vector2(View.WorldWidth / 2f, View.Height / 2f);
        }

        #region Controls

        /// <summary>
        /// Are camera controls enabled?
        /// </summary>
        private bool AreControlsEnabled { get; set; }

        /// <summary>
        /// The camera's controls.
        /// </summary>
        private void Controls()
        {
            if (AreControlsEnabled)
            {
                #region Moving

                if (InputEvents.InputFlags.IsFlagSet(InputMappableCameraCommandFlags.FlyMode))
                {
                    Move(InputEvents.InputDeltas);
                }

                #endregion

                #region Panning

                // Up
                if (InputEvents.InputFlags.IsFlagSet(InputMappableCameraCommandFlags.PanUp))
                {
                    Position -= InputEvents.InputScrollVelocity;
                }

                // Down
                if (InputEvents.InputFlags.IsFlagSet(InputMappableCameraCommandFlags.PanDown))
                {
                    Position += InputEvents.InputScrollVelocity;
                }

                // Left
                if (InputEvents.InputFlags.IsFlagSet(InputMappableCameraCommandFlags.PanLeft))
                {
                    Position -= InputEvents.InputScrollVelocity;
                }

                // Right
                if (InputEvents.InputFlags.IsFlagSet(InputMappableCameraCommandFlags.PanRight))
                {
                    Position += InputEvents.InputScrollVelocity;
                }

                #endregion

                #region Zooming

                if (IsZoomable)
                {
                    if (InputEvents.InputFlags.IsFlagSet(InputMappableCameraCommandFlags.ZoomIn))
                    {
                        Zoom += ZoomIncrement;
                    }

                    if (InputEvents.InputFlags.IsFlagSet(InputMappableCameraCommandFlags.ZoomOut))
                    {
                        Zoom -= ZoomIncrement;
                    }
                }

                #endregion

                #region Rotating

                if (InputEvents.InputFlags.IsFlagSet(InputMappableCameraCommandFlags.RotateClockwise))
                {
                    RotationAngle += (float)InputEvents.InputRotation;
                }

                if (InputEvents.InputFlags.IsFlagSet(InputMappableCameraCommandFlags.RotatesCounterClockwise))
                {
                    RotationAngle -= (float)InputEvents.InputRotation;
                }

                #endregion
            }
        }

        #endregion

        #region Zooming

        /// <summary>
        /// Is the camera's zoom function enabled?
        /// </summary>
        public bool IsZoomable { get; set; }

        /// <summary>
        /// The camera's minimum zoom level.
        /// </summary>
        public float ZoomLevelMinimum { get; set; } = 0.10f;

        /// <summary>
        /// The camera's maximum zoom level.
        /// </summary>
        public float ZoomLevelMaximum { get; set; } = 2.00f;

        /// <summary>
        /// The camera's internal zoom level value.
        /// </summary>
        private float _zoomLevel = 1f;

        /// <summary>
        /// Camera Zoom.
        /// </summary>
        private float Zoom
        {
            get => _zoomLevel;
            set => _zoomLevel = value <= ZoomLevelMaximum && value >= ZoomLevelMinimum ? value : _zoomLevel;
        }

        /// <summary>
        /// The camera's current zoom increment.
        /// </summary>
        public float ZoomIncrement { get; set; } = ZoomIncrements.Tenth;

        /// <summary>
        /// Camera Zoom Increments.
        /// </summary>
        public struct ZoomIncrements
        {
            /// <summary>
            /// A fine increment of one one hundredth with a value of <value>0.01f</value>.
            /// </summary>
            public const float Fine = 0.01f;
            /// <summary>
            /// An increment of one fifth with a value of <value>0.05f</value>.
            /// </summary>
            public const float Fifth = 0.05f;
            /// <summary>
            /// An increment of one tenth with a value of <value>0.10f</value>.
            /// </summary>
            public const float Tenth = 0.10f;
            /// <summary>
            /// An increment of one quarter with a value of <value>0.25f</value>.
            /// </summary>
            public const float Quarter = 0.25f;
            /// <summary>
            /// An increment of one half with a value of <value>0.50f</value>.
            /// </summary>
            public const float Half = 0.50f;
            /// <summary>
            /// An increment of one with a value of <value>1.00f</value>.
            /// </summary>
            public const float One = 1.00f;
        }

        #endregion
    }
}