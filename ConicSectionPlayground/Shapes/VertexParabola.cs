// <copyright file="VertexParabola.cs">
//     Copyright © 2019 - 2020 Shkyrockett. All rights reserved.
// </copyright>
// <author id="shkyrockett">Shkyrockett</author>
// <license>
//     Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </license>
// <summary></summary>
// <remarks></remarks>

using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace ConicSectionPlayground
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="IShape" />
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class VertexParabola
        : IShape
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VertexParabola" /> class.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="h">The h.</param>
        /// <param name="k">The k.</param>
        /// <param name="i">The i.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public VertexParabola(double a, double h, double k, double i = 0)
        {
            (A, H, K, I) = (a, h, k, i);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexParabola" /> class.
        /// </summary>
        /// <param name="tuple">The tuple.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public VertexParabola((double a, double h, double k, double i) tuple)
            : this(tuple.a, tuple.h, tuple.k, tuple.i)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexParabola" /> class.
        /// </summary>
        /// <param name="tuple">The tuple.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public VertexParabola((double a, double h, double k) tuple)
            : this(tuple.a, tuple.h, tuple.k, 0)
        { }

        /// <summary>
        /// Gets or sets the a.
        /// </summary>
        /// <value>
        /// The a.
        /// </value>
        public double A { get; set; }

        /// <summary>
        /// Gets or sets the h.
        /// </summary>
        /// <value>
        /// The h.
        /// </value>
        public double H { get; set; }

        /// <summary>
        /// Gets or sets the k.
        /// </summary>
        /// <value>
        /// The k.
        /// </value>
        public double K { get; set; }

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
        public void DrawShape(Graphics gr, Point offset, float scale) => Rendering.DrawVertexParabola(gr, Pen ?? Pens.Black, offset, scale, this);// Rendering.DrawConicSection(gr, pen, conicSection = conicSection is null ? ToConicSection() : conicSection);

        /// <summary>
        /// Converts to standard parabola.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (double a, double h, double k) ToStandardParabola() => Conversion.VertexParabolaToStandardParabola(A, H, K);

        /// <summary>
        /// Converts to quadratic bezier.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (double aX, double aY, double bX, double bY, double cX, double cY) ToQuadraticBezier(double left, double right) => Conversion.VertexParabolaToQuadraticBezier(A, H, K, left, right);

        /// <summary>
        /// Converts to quadratic bezier.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="top">The top.</param>
        /// <param name="right">The right.</param>
        /// <param name="bottom">The bottom.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (double aX, double aY, double bX, double bY, double cX, double cY) ToQuadraticBezier(double left, double top, double right, double bottom) => Conversion.VertexParabolaToQuadraticBezier(A, H, K, I, left, top, right, bottom);

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
        public ConicSection ToUnitConicSection() => Conversion.VertexParabolaToUnitConicSection((A, H, K));

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString() => $"{nameof(VertexParabola)}({nameof(A)}: {A}, {nameof(H)}: {H}, {nameof(K)}: {K}, {nameof(I)}: {I})";

        /// <summary>
        /// Gets the debugger display.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string GetDebuggerDisplay() => ToString();
    }
}
