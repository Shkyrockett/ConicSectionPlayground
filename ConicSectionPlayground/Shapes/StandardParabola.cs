// <copyright file="StandardParabola.cs">
//     Copyright © 2019 - 2020 Shkyrockett. All rights reserved.
// </copyright>
// <author id="shkyrockett">Shkyrockett</author>
// <license>
//     Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </license>
// <summary></summary>
// <remarks></remarks>

using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace ConicSectionPlayground
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="ConicSectionPlayground.IShape" />
    public class StandardParabola
        : IShape
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardParabola"/> class.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <param name="c">The c.</param>
        /// <param name="i">The i.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public StandardParabola(double a, double b, double c, double i = 0d)
        {
            A = a;
            B = b;
            C = c;
            I = i;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardParabola"/> class.
        /// </summary>
        /// <param name="tuple">The tuple.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public StandardParabola((double a, double b, double c, double i) tuple)
            : this(tuple.a, tuple.b, tuple.c, tuple.i)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardParabola"/> class.
        /// </summary>
        /// <param name="tuple">The tuple.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public StandardParabola((double a, double b, double c) tuple)
            : this(tuple.a, tuple.b, tuple.c, 0d)
        { }

        /// <summary>
        /// Gets or sets a.
        /// </summary>
        /// <value>
        /// a.
        /// </value>
        public double A { get; set; }

        /// <summary>
        /// Gets or sets the b.
        /// </summary>
        /// <value>
        /// The b.
        /// </value>
        public double B { get; set; }

        /// <summary>
        /// Gets or sets the c.
        /// </summary>
        /// <value>
        /// The c.
        /// </value>
        public double C { get; set; }

        /// <summary>
        /// Gets or sets the i.
        /// </summary>
        /// <value>
        /// The i.
        /// </value>
        public double I { get; set; }

        /// <summary>
        /// Gets or sets the pen.
        /// </summary>
        /// <value>
        /// The pen.
        /// </value>
        public Pen Pen { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Occurs when [property changed].
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the property changed event.
        /// </summary>
        /// <param name="name">The name.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnPropertyChanged([CallerMemberName] string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        /// <summary>
        /// Draws the shape.
        /// </summary>
        /// <param name="gr">The gr.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="scale">The scale.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawShape(Graphics gr, Point offset, float scale) => Rendering.DrawStandardParabola(gr, Pen ?? Pens.Black, offset, scale, this);// Rendering.DrawConicSection(gr, pen, conicSection = conicSection is null ? ToConicSection() : conicSection);

        /// <summary>
        /// Converts to vertex parabola.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (double a, double h, double k) ToVertexParabola() => Conversion.StandardParabolaToVertexParabola(A, B, C);

        /// <summary>
        /// Converts to quadratic bezier.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (double aX, double aY, double bX, double bY, double cX, double cY) ToQuadraticBezier(double left, double right) => Conversion.StandardParabolaToQuadraticBezier(A, B, C, left, right);

        /// <summary>
        /// Converts to quadratic bezier.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="top">The top.</param>
        /// <param name="right">The right.</param>
        /// <param name="bottom">The bottom.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (double aX, double aY, double bX, double bY, double cX, double cY) ToQuadraticBezier(double left, double top, double right, double bottom) => Conversion.StandardParabolaToQuadraticBezier(A, B, C, I, left, top, right, bottom);

        /// <summary>
        /// Converts to cubic bezier.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (double aX, double aY, double bX, double bY, double cX, double cY, double dX, double dY) ToCubicBezier(double left, double right) => Conversion.QuadraticBezierToCubicBezier(ToQuadraticBezier(left, right));

        /// <summary>
        /// Converts to cubic bezier.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="top">The top.</param>
        /// <param name="right">The right.</param>
        /// <param name="bottom">The bottom.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (double aX, double aY, double bX, double bY, double cX, double cY, double dX, double dY) ToCubicBezier(double left, double top, double right, double bottom) => Conversion.QuadraticBezierToCubicBezier(ToQuadraticBezier(left, top, right, bottom));

        /// <summary>
        /// Converts to a conic section.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ConicSection ToUnitConicSection() => Conversion.VertexParabolaToUnitConicSection(ToVertexParabola());

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString() => $"{nameof(StandardParabola)}({nameof(A)}: {A}, {nameof(B)}: {B}, {nameof(C)}: {C}, {nameof(I)}: {I})";
    }
}
