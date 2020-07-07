// <copyright file="Form1.cs">
//     Copyright © 2019 - 2020 Shkyrockett. All rights reserved.
// </copyright>
// <author id="shkyrockett">Shkyrockett</author>
// <license>
//     Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </license>
// <summary></summary>
// <remarks></remarks>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using static ConicSectionPlayground.Mathematics;

namespace ConicSectionPlayground
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class Form1
        : Form
    {
        #region Fields
        /// <summary>
        /// The scale.
        /// </summary>
        private double scale = 1;

        /// <summary>
        /// The starting point
        /// </summary>
        private PointF startingPoint = PointF.Empty;

        /// <summary>
        /// The moving point
        /// </summary>
        private Point panPoint = Point.Empty;

        /// <summary>
        /// The panning
        /// </summary>
        private bool panning = false;

        /// <summary>
        /// The group
        /// </summary>
        [TypeConverter(typeof(ExpandableCollectionConverter))]
        private readonly Group group;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Form1" /> class.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Form1()
        {
            InitializeComponent();

            SetDoubleBuffered(picCanvas);

            var ellipse = (h: 100d, k: 100d, a: 50d, b: 25d, angle: 30d.DegreesToRadians());
            var vertexParabola = (0.025d, 200d, 100d, 30d.DegreesToRadians());

            var ellipse1 = new Ellipse(ellipse) { Pen = Pens.Green, Name = "Ellipse" };
            var conicSection = new Ellipse(ellipse).ToUnitConicSection();
            conicSection.Pen = Pens.Red;
            conicSection.Name = "Conic Ellipse";
            var conicSection1 = Conversion.RescaleConicSection(new Ellipse(ellipse).ToUnitConicSection());
            conicSection1.Pen = Pens.DarkBlue;
            conicSection1.Name = "Scaled Conic Ellipse";
            var vertexParabola1 = new VertexParabola(vertexParabola) { Pen = Pens.Blue, Name = "Vertex Parabola" };
            //var standardParabola = new StandardParabola(Conversion.VertexParabolaToStandardParabola(vertexParabola)) { Pen = Pens.Red };
            var conicSection2 = new VertexParabola(vertexParabola).ToUnitConicSection();
            conicSection2.Pen = Pens.DarkOrange;
            conicSection2.Name = "Vertex Parabola";
            //var conicSection2 = new StandardParabola(Conversion.VertexParabolaToStandardParabola(vertexParabola)).ToUnitConicSection();
            //conicSection2.Pen = Pens.Red;
            var conicSection3 = new Line(100d, 100d, 200d, 200d).ToUnitConicSection();
            conicSection3.Pen = Pens.CornflowerBlue;
            conicSection3.Name = "Unit Conic Section";
            var conicSection4 = Conversion.ParabolaToConicSection2(50d, 100d, 10d, 1d);
            conicSection4.Pen = Pens.MediumTurquoise;
            conicSection4.Name = "Parabola Conic Section";

            group = new Group(new List<IShape> {
                ellipse1,
                conicSection,
                conicSection1,
                vertexParabola1,
                //standardParabola,
                conicSection2,
                //conicSection2,
                conicSection3,
                conicSection4,
            });

            //var t1 = Conversion.EllipseToUnitConicSection(ellipse.a, ellipse.b, ellipse.h, ellipse.k, Math.Cos(ellipse.angle), Math.Sin(ellipse.angle));
            //var t2 = Conversion.EllipseConicSectionPolynomial(ellipse.h, ellipse.k, ellipse.a, ellipse.b, Math.Cos(ellipse.angle), Math.Sin(ellipse.angle));

            propertyGrid1.SelectedObject = group;
            propertyGrid1.ExpandAllGridItems();
        }
        #endregion

        #region Events
        /// <summary>
        /// Handles the Load event of the Form1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Form1_Load(object sender, EventArgs e)
        { }

        /// <summary>
        /// Handles the Paint event of the PicCanvas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PaintEventArgs" /> instance containing the event data.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void PicCanvas_Paint(object sender, PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            foreach (var shape in group?.Shapes)
            {
                shape.DrawShape(g, panPoint, (float)scale);
            }
        }

        /// <summary>
        /// Handles the MouseDown event of the Form1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void PicCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle || e.Button == MouseButtons.Left && !panning)
            {
                panning = true;
                startingPoint = ScreenToObjectTransposedMatrix(panPoint, e.Location, scale);
                panPoint = Point.Truncate(ScreenToObjectTransposedMatrix(startingPoint, e.Location, scale));
                picCanvas.Invalidate();
            }
        }

        /// <summary>
        /// Handles the MouseMove event of the Form1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void PicCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle || e.Button == MouseButtons.Left)
            {
                if (panning)
                {
                    panPoint = Point.Truncate(ScreenToObjectTransposedMatrix(startingPoint, e.Location, scale));
                    picCanvas.Invalidate();
                }
            }
        }

        /// <summary>
        /// Handles the MouseUp event of the Form1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void PicCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle || e.Button == MouseButtons.Left && panning)
            {
                panning = false;
                picCanvas.Invalidate();
            }
        }

        /// <summary>
        /// Handles the MouseWheel event of the Form1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void PicCanvas_MouseWheel(object sender, MouseEventArgs e)
        {
            startingPoint = ScreenToObjectTransposedMatrix(panPoint, e.Location, scale);
            scale = MouseWheelScaleFactor(scale, e.Delta);
            panPoint = Point.Truncate(ScreenToObjectTransposedMatrix(startingPoint, e.Location, scale));
            picCanvas.Invalidate();
        }

        /// <summary>
        /// Handles the Resize event of the PicCanvas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void PicCanvas_Resize(object sender, EventArgs e)
        {
            base.OnResize(e);
            picCanvas.Invalidate();
        }

        /// <summary>
        /// Handles the PropertyValueChanged event of the PropertyGrid1 control.
        /// </summary>
        /// <param name="s">The source of the event.</param>
        /// <param name="e">The <see cref="PropertyValueChangedEventArgs" /> instance containing the event data.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void PropertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e) => picCanvas.Invalidate();
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
