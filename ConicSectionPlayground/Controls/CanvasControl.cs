// <copyright file="CanvasControl.cs">
//     Copyright © 2020 Shkyrockett. All rights reserved.
// </copyright>
// <author id="shkyrockett">Shkyrockett</author>
// <license>
//     Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </license>
// <summary></summary>
// <remarks></remarks>

using ConicSectionLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ConicSectionLibrary.MathematicsF;

namespace ConicSectionPlayground
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    public partial class CanvasControl
        : UserControl
    {
        #region Fields
        /// <summary>
        /// The group
        /// </summary>
        [TypeConverter(typeof(ExpandableCollectionConverter))]
        private Group group;

        /// <summary>
        /// The offset x.
        /// </summary>
        private float offsetX;

        /// <summary>
        /// The offset y.
        /// </summary>
        private float offsetY;

        /// <summary>
        /// The panning
        /// </summary>
        private bool panning;

        /// <summary>
        /// The starting point
        /// </summary>
        private PointF startingPoint = PointF.Empty;

        /// <summary>
        /// The pan point
        /// </summary>
        private PointF panPoint = PointF.Empty;

        /// <summary>
        /// The scale.
        /// </summary>
        private float scale = 1f;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CanvasControl"/> class.
        /// </summary>
        public CanvasControl()
        {
            DoubleBuffered = true;
            SetDoubleBuffered(this);
            InitializeComponent();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the background color for the control.
        /// </summary>
        //[DefaultValue(SystemColors.Window)]
        public new Color BackColor { get; set; }

        /// <summary>
        /// Gets or sets the border style of the user control.
        /// </summary>
        [DefaultValue(BorderStyle.FixedSingle)]
        public new BorderStyle BorderStyle { get; set; }

        /// <summary>
        /// Gets or sets the ghost polygon pen.
        /// </summary>
        /// <value>
        /// The ghost polygon pen.
        /// </value>
        public Pen GhostPolygonPen { get; set; }

        /// <summary>
        /// Gets or sets the handle radius.
        /// </summary>
        /// <value>
        /// The handle radius.
        /// </value>
        [DefaultValue(3)]
        public int HandleRadius { get; set; }

        /// <summary>
        /// We're over an object if the distance squared
        /// between the mouse and the object is less than this.
        /// </summary>
        private int HandleRadiusSquared => HandleRadius * HandleRadius;

        /// <summary>
        /// Gets or sets the group.
        /// </summary>
        /// <value>
        /// The group.
        /// </value>
        public Group Document { get => group; set => group = value; }

        /// <summary>
        /// Gets or sets the pan point.
        /// </summary>
        /// <value>
        /// The pan point.
        /// </value>
        public PointF Pan
        {
            get => panPoint; set
            {
                panPoint = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the scale.
        /// </summary>
        /// <value>
        /// The scale.
        /// </value>
        public float Zoom
        {
            get => scale; set
            {
                scale = value;
                Invalidate();
            }
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Handles the Load event of the CanvasControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CanvasControl_Load(object sender, EventArgs e)
        { }

        /// <summary>
        /// Handles the Paint event of the CanvasControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PaintEventArgs"/> instance containing the event data.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CanvasControl_Paint(object sender, PaintEventArgs e)
        {
            //base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(BackColor);
            g.ScaleTransform(scale, scale);
            g.TranslateTransform(panPoint.X, panPoint.Y);

            if (group is not null)
            {
                foreach (var shape in group?.Shapes)
                {
                    shape.DrawShape(g, panPoint, scale);
                }
            }

            using var forePen = new Pen(ForeColor);
            g.ResetTransform();
            g.DrawRectangle(forePen, 0, 0, Width - 1, Height - 1);
            TextRenderer.DrawText(g, $"Pan Point: {panPoint}\n\rScale: {scale}\n\r🌎 Mouse Pos: {MousePosition}", Font, new Point(0, 0), ForeColor);
        }

        /// <summary>
        /// Handles the MouseDown event of the CanvasControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CanvasControl_MouseDown(object sender, MouseEventArgs e)
        {
            var translateScalePoint = ScreenToObjectTransposedMatrix(panPoint, e.Location, scale);
            var scalePoint = ScreenToObject(e.Location, scale);

            if (e.Button == MouseButtons.Middle && !panning)
            {
                panning = true;
                startingPoint = translateScalePoint;
                panPoint = ScreenToObjectTransposedMatrix(startingPoint, e.Location, scale);

                // Get ready to work on the new polygon.
                MouseMove -= CanvasControl_MouseMove_NotDrawing;
                MouseMove += CanvasControl_MouseMove_Panning;
                MouseUp += CanvasControl_MouseUp_Panning;

                Invalidate();
            }
        }

        /// <summary>
        /// Handles the MouseMove event of the CanvasControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CanvasControl_MouseMove_NotDrawing(object sender, MouseEventArgs e)
        {
            var mousePoint = ScreenToObjectTransposedMatrix(panPoint, e.Location, scale);
            //Cursor =
            //    MouseIsOverCornerPoint(mousePoint, group).Success ? Cursors.Arrow :
            //    MouseIsOverEdge(mousePoint, group).Success ? Cursors.Cross :
            //    MouseIsOverPolygon(mousePoint, group).Success ? Cursors.Hand :
            //    Cursors.Cross;
            Invalidate();
        }

        /// <summary>
        /// Handles the Panning event of the Canvas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CanvasControl_MouseMove_Panning(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle && panning)
            {
                panPoint = ScreenToObjectTransposedMatrix(startingPoint, e.Location, scale);

                // Redraw.
                Invalidate();
            }
        }

        /// <summary>
        /// Handles the MouseUp Panning event of the Canvas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CanvasControl_MouseUp_Panning(object sender, MouseEventArgs e)
        {
            MouseMove += CanvasControl_MouseMove_NotDrawing;
            MouseMove -= CanvasControl_MouseMove_Panning;
            MouseUp -= CanvasControl_MouseUp_Panning;

            if (e.Button == MouseButtons.Middle && panning)
            {
                panning = false;

                // Redraw.
                Invalidate();
            }
        }

        /// <summary>
        /// Handles the MouseWheel event of the Canvas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CanvasControl_MouseWheel(object sender, MouseEventArgs e)
        {
            var previousScale = scale;
            scale = MouseWheelScaleFactor(scale, e.Delta);
            scale = scale < scale_per_delta ? scale_per_delta : scale;

            panPoint = ZoomAtTransposedMatrix(panPoint, e.Location, previousScale, scale);

            // Redraw.
            Invalidate();
        }

        /// <summary>
        /// Handles the Resize event of the CanvasControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CanvasControl_Resize(object sender, EventArgs e)
        {
            //base.OnResize(e);
            Invalidate();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Set an arbitrary control to double-buffer.
        /// </summary>
        /// <param name="control">The control to set as double buffered.</param>
        /// <remarks>
        /// Taxes: Remote Desktop Connection and painting: http://blogs.msdn.com/oldnewthing/archive/2006/01/03/508694.aspx
        /// </remarks>
        private static void SetDoubleBuffered(Control control)
        {
            if (SystemInformation.TerminalServerSession)
            {
                return;
            }

            System.Reflection.PropertyInfo aProp = typeof(Control).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            aProp.SetValue(control, true, null);
        }
        #endregion
    }
}
