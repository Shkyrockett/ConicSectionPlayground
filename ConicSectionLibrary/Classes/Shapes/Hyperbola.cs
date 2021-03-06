// <copyright file="Hyperbola.cs">
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
    public class Hyperbola
        : IGeometry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Hyperbola" /> class.
        /// </summary>
        /// <param name="h">The h.</param>
        /// <param name="k">The k.</param>
        /// <param name="rX">The r x.</param>
        /// <param name="rY">The r y.</param>
        /// <param name="a">a.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Hyperbola(double h, double k, double rX, double rY, double a)
        {
            (H, K, RX, RY, A, ConicSection) = (h, k, rX, rY, a, ToUnitConicSection());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Hyperbola" /> class.
        /// </summary>
        /// <param name="tuple">The tuple.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Hyperbola((double h, double k, double rX, double rY, double a) tuple)
            : this(tuple.h, tuple.k, tuple.rX, tuple.rY, tuple.a)
        { }

        /// <summary>
        /// Deconstructs the specified Hyperbola.
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
        /// Gets or sets the rx.
        /// </summary>
        /// <value>
        /// The rx.
        /// </value>
        public double RX { get; set; }

        /// <summary>
        /// Gets or sets the ry.
        /// </summary>
        /// <value>
        /// The ry.
        /// </value>
        public double RY { get; set; }

        /// <summary>
        /// Gets or sets a.
        /// </summary>
        /// <value>
        /// The a.
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
        /// Gets or sets the conic section.
        /// </summary>
        /// <value>
        /// The conic section.
        /// </value>
        public ConicSection ConicSection { get; set; }

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
        /// Translates the specified delta.
        /// </summary>
        /// <param name="delta">The delta.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IGeometry Translate(Vector2 delta) => throw new NotImplementedException();

        /// <summary>
        /// Queries whether the shape includes the specified point in it's geometry.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool Includes(PointF point) => throw new NotImplementedException();

        /// <summary>
        /// Converts to a conic section.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ConicSection ToUnitConicSection() => ConversionD.HyperbolaToConicSection(RX, RY, H, K, A);

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public string ToString(string format, IFormatProvider formatProvider) => ToString();
        
        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString() => $"{nameof(Hyperbola)}({nameof(H)}: {H}, {nameof(K)}: {K}, {nameof(RX)}: {RX}, {nameof(RY)}: {RY}, {nameof(A)}: {A})";

        /// <summary>
        /// Gets the debugger display.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string GetDebuggerDisplay() => ToString();
    }
}
