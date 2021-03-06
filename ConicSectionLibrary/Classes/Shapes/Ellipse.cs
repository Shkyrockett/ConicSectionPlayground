// <copyright file="Ellipse.cs">
//     Copyright © 2019 - 2020 Shkyrockett. All rights reserved.
// </copyright>
// <author id="shkyrockett">Shkyrockett</author>
// <license>
//     Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </license>
// <summary></summary>
// <remarks></remarks>

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace ConicSectionLibrary
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="IGeometry" />
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class Ellipse
        : IGeometry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Ellipse" /> class.
        /// </summary>
        /// <param name="h">The h.</param>
        /// <param name="k">The k.</param>
        /// <param name="rX">The r x.</param>
        /// <param name="rY">The r y.</param>
        /// <param name="a">a.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Ellipse(double h, double k, double rX, double rY, double a)
        {
            (H, K, RX, RY, A) = (h, k, rX, rY, a);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ellipse" /> class.
        /// </summary>
        /// <param name="tuple">The tuple.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Ellipse((double h, double k, double rX, double rY, double a) tuple)
            : this(tuple.h, tuple.k, tuple.rX, tuple.rY, tuple.a)
        { }

        /// <summary>
        /// Deconstructs the specified Ellipse.
        /// </summary>
        /// <param name="h">The h.</param>
        /// <param name="k">The k.</param>
        /// <param name="rX">The r x.</param>
        /// <param name="rY">The r y.</param>
        /// <param name="a">a.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Deconstruct(out double h, out double k, out double rX, out double rY, out double a) => (h, k, rX, rY, a) = (H, K, RX, RY, A);

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
        /// Gets or sets the Radius x.
        /// </summary>
        /// <value>
        /// The rx.
        /// </value>
        public double RX { get; set; }

        /// <summary>
        /// Gets the Diameter x.
        /// </summary>
        /// <value>
        /// The dx.
        /// </value>
        public double DX { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => RX * 2d; }

        /// <summary>
        /// Gets or sets the Radius y.
        /// </summary>
        /// <value>
        /// The ry.
        /// </value>
        public double RY { get; set; }

        /// <summary>
        /// Gets the Diameter y.
        /// </summary>
        /// <value>
        /// The dy.
        /// </value>
        public double DY { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => RY * 2d; }

        /// <summary>
        /// Gets or sets a.
        /// </summary>
        /// <value>
        /// a.
        /// </value>
        public double A { get; set; }

        /// <summary>
        /// Gets or sets the pen.
        /// </summary>
        /// <value>
        /// The pen.
        /// </value>
        public IDisposable Pen { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets the center.
        /// </summary>
        /// <value>
        /// The center.
        /// </value>
        public PointF Center { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => new((float)((0.5d * (RX * 2d)) + H), (float)((0.5d * (RY * 2d)) + K)); }

        ///// <summary>
        ///// Gets the unit conic section.
        ///// </summary>
        ///// <value>
        ///// The unit conic section.
        ///// </value>
        //[MergableProperty(true)]
        //[Category("Properties")]
        //public ConicSection UnitConicSection => ToUnitConicSection();

        ///// <summary>
        ///// Gets the conic section.
        ///// </summary>
        ///// <value>
        ///// The conic section.
        ///// </value>
        //[MergableProperty(true)]
        //[Category("Properties")]
        //public ConicSection ConicSection => ToConicSection();

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
        /// Converts to a unit conic section.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ConicSection ToUnitConicSection() => ConversionD.EllipseToUnitConicSection(RX, RY, H, K, A);

        /// <summary>
        /// Translates the specified delta.
        /// </summary>
        /// <param name="delta">The delta.</param>
        /// <returns></returns>
        public IGeometry Translate(Vector2 delta) => throw new NotImplementedException();

        /// <summary>
        /// Includeses the specified point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns></returns>
        public bool Includes(PointF point) => throw new NotImplementedException();

        /// <summary>
        /// Converts to a conic section.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ConicSection ToConicSection() => ConversionD.EllipseToConicSection(RX, RY, H, K, A);

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns></returns>
        public string ToString(string format, IFormatProvider formatProvider) => ToString();
        
        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString() => $"{nameof(Ellipse)}({nameof(H)}: {H}, {nameof(K)}: {K}, {nameof(RX)}: {RX}, {nameof(RY)}: {RY}, {nameof(A)}: {A})";

        /// <summary>
        /// Gets the debugger display.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string GetDebuggerDisplay() => ToString();
    }
}
