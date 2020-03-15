﻿// <copyright file="Hyperbola.cs">
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
    public class Hyperbola
        : IShape
    {
        /// <summary>
        /// The conic section
        /// </summary>
        private ConicSection conicSection;

        /// <summary>
        /// Initializes a new instance of the <see cref="Hyperbola"/> class.
        /// </summary>
        /// <param name="h">The h.</param>
        /// <param name="k">The k.</param>
        /// <param name="rX">The r x.</param>
        /// <param name="rY">The r y.</param>
        /// <param name="a">a.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Hyperbola(double h, double k, double rX, double rY, double a)
        {
            H = h;
            K = k;
            RX = rX;
            RY = rY;
            A = a;
            conicSection = ToUnitConicSection();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Hyperbola"/> class.
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
        /// a.
        /// </value>
        public double A { get; set; }

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
        public void DrawShape(Graphics gr, Point offset, float scale) => Rendering.DrawConicSection(gr, Pen ?? Pens.Black, offset, scale, conicSection = conicSection is null ? ToUnitConicSection() : conicSection);

        /// <summary>
        /// Converts to a conic section.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ConicSection ToUnitConicSection() => Conversion.HyperbolaToConicSection(RX, RY, H, K, A);

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString() => $"{nameof(Hyperbola)}({nameof(H)}: {H}, {nameof(K)}: {K}, {nameof(RX)}: {RX}, {nameof(RY)}: {RY}, {nameof(A)}: {A})";
    }
}