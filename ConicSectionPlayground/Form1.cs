// <copyright file="Form1.cs">
//     Copyright © 2019 - 2020 Shkyrockett. All rights reserved.
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
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using static ConicSectionLibrary.MathematicsF;

namespace ConicSectionPlayground
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class Form1
        : Form
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Form1" /> class.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Form1()
        {
            InitializeComponent();

            var ellipse = (h: 100d, k: 100d, a: 50d, b: 25d, angle: 30d.DegreesToRadians());
            var vertexParabola = (0.025d, 200d, 100d, 30d.DegreesToRadians());

            var ellipse1 = new Ellipse(ellipse) { Pen = Pens.Green, Name = "Ellipse" };
            var conicSection = new Ellipse(ellipse).ToUnitConicSection();
            conicSection.Pen = Pens.Red;
            conicSection.Name = "Conic Ellipse";
            var conicSection1 = ConversionD.RescaleConicSection(new Ellipse(ellipse).ToUnitConicSection());
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
            var conicSection4 = ConversionD.ParabolaToConicSection2(50d, 100d, 10d, 1d);
            conicSection4.Pen = Pens.MediumTurquoise;
            conicSection4.Name = "Parabola Conic Section";

            canvasControl.Document = new Group(new List<IGeometry> {
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

            propertyGrid1.SelectedObject = canvasControl.Document;
            propertyGrid1.ExpandAllGridItems();
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Handles the Load event of the Form1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Form1_Load(object sender, EventArgs e)
        { }

        /// <summary>
        /// Handles the PropertyValueChanged event of the PropertyGrid1 control.
        /// </summary>
        /// <param name="s">The source of the event.</param>
        /// <param name="e">The <see cref="PropertyValueChangedEventArgs" /> instance containing the event data.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void PropertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e) => canvasControl.Invalidate();

        /// <summary>
        /// Handles the Click event of the ButtonResetPan control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ButtonResetPan_Click(object sender, EventArgs e)
        {
            canvasControl.Pan = new PointF(0f, 0f);
        }

        /// <summary>
        /// Handles the Click event of the ButtonResetScale control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ButtonResetScale_Click(object sender, EventArgs e)
        {
            canvasControl.Zoom = 1;
        }
        #endregion
    }
}
