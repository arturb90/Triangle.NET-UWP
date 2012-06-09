﻿// -----------------------------------------------------------------------
// <copyright file="Zoom.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MeshExplorer.Rendering
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Drawing;

    /// <summary>
    /// Manages the current world to screen transformation
    /// </summary>
    public class Zoom
    {
        // The complete mesh
        Rectangle Screen { get; set; }

        // The complete mesh
        RectangleF World { get; set; }

        // The current viewport (visible mesh)
        public RectangleF Viewport { get; set; }

        // Current scale (zoom level)
        public int Level { get; set; }

        // Add a margin to clip region (5% of viewport width on each side)
        public float ClipMargin { get; set; }

        public Zoom()
        {
            Level = -1;
        }

        public void Initialize(Rectangle screen, RectangleF world)
        {
            this.Screen = screen;

            this.World = world;
            this.Level = 1;

            // Add a margin so there's some space around the border
            float worldMargin = (world.Width < world.Height) ? world.Height * 0.05f : world.Width * 0.05f;

            // Get the initial viewport (complete mesh centered on the screen)
            float screenRatio = screen.Width / (float)screen.Height;
            float worldRatio = world.Width / world.Height;

            float scale = (world.Width + worldMargin) / screen.Width;

            if (screenRatio > worldRatio)
            {
                scale = (world.Height + worldMargin) / screen.Height;
            }

            float centerX = world.X + world.Width / 2;
            float centerY = world.Y + world.Height / 2;

            // TODO: Add initial margin
            this.Viewport = new RectangleF(centerX - screen.Width * scale / 2,
                centerY - screen.Height * scale / 2,
                screen.Width * scale,
                screen.Height * scale);

            this.ClipMargin = this.Viewport.Width * 0.05f;

            this.World = this.Viewport;
        }

        public void Resize(Rectangle screen, RectangleF world)
        {
            this.Screen = screen;

            this.World = world;
            this.Level = 1;

            // Add a margin so there's some space around the border
            float worldMargin = (World.Width < World.Height) ? World.Height * 0.05f : World.Width * 0.05f;

            // Get the initial viewport (complete mesh centered on the screen)
            float screenRatio = screen.Width / (float)screen.Height;
            float worldRatio = World.Width / World.Height;

            float scale = (World.Width + worldMargin) / screen.Width;

            if (screenRatio > worldRatio)
            {
                scale = (World.Height + worldMargin) / screen.Height;
            }

            float centerX = World.X + World.Width / 2;
            float centerY = World.Y + World.Height / 2;

            // TODO: Add initial margin
            this.Viewport = new RectangleF(centerX - screen.Width * scale / 2,
                centerY - screen.Height * scale / 2,
                screen.Width * scale,
                screen.Height * scale);

            this.ClipMargin = this.Viewport.Width * 0.05f;

            this.World = this.Viewport;
        }

        /// <summary>
        /// Zoom in or out of the viewport.
        /// </summary>
        /// <param name="amount">Zoom amount</param>
        /// <param name="focusX">Relative x point position</param>
        /// <param name="focusY">Relative y point position</param>
        public bool Update(int amount, float focusX, float focusY)
        {
            float width, height;

            if (amount > 0) // Zoom in
            {
                this.Level++;

                if (this.Level > 50)
                {
                    this.Level = 50;
                    return false;
                }

                width = Viewport.Width / 1.1f;
                height = Viewport.Height / 1.1f;
            }
            else
            {
                this.Level--;

                if (this.Level < 1)
                {
                    this.Level = 1;
                    this.Viewport = this.World;
                    return false;
                }

                width = Viewport.Width * 1.1f;
                height = Viewport.Height * 1.1f;
            }

            // Current focus on viewport
            float x = Viewport.X + Viewport.Width * focusX;
            float y = Viewport.Y + Viewport.Height * (1 - focusY);

            // New left and top positions
            x = x - width * focusX;
            y = y - height * (1 - focusY);

            // Check if outside of world
            if (x < World.X)
            {
                x = World.X;
            }
            else if (x + width > World.Right)
            {
                x = World.Right - width;
            }

            if (y < World.Y)
            {
                y = World.Y;
            }
            else if (y + height > World.Bottom)
            {
                y = World.Bottom - height;
            }

            // Set new viewport
            this.Viewport = new RectangleF(x, y, width, height);

            this.ClipMargin = this.Viewport.Width * 0.05f;

            return true;
        }

        public bool ViewportContains(PointF pt)
        {
            return (pt.X > Viewport.X && pt.X < Viewport.Right
                && pt.Y > Viewport.Y && pt.Y < Viewport.Bottom);
        }

        public PointF WorldToScreen(PointF pt)
        {
            return new PointF((pt.X - Viewport.X) / Viewport.Width * Screen.Width,
                (1 - (pt.Y - Viewport.Y) / Viewport.Height) * Screen.Height);
        }

        public PointF ScreenToWorld(float ptX, float ptY)
        {
            return new PointF(Viewport.X + Viewport.Width * ptX,
                Viewport.Y + Viewport.Height * (1 - ptY));
        }

        public void Reset()
        {
            this.Viewport = this.World;
            this.Level = 1;
        }
    }
}