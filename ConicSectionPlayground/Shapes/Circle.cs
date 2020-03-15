// <copyright file="Circle.cs">
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
    public class Circle
        : IShape
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Circle"/> class.
        /// </summary>
        /// <param name="h">The h.</param>
        /// <param name="k">The k.</param>
        /// <param name="r">The r.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Circle(double h, double k, double r)
        {
            H = h;
            K = k;
            R = r;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Circle"/> class.
        /// </summary>
        /// <param name="tuple">The tuple.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Circle((double h, double k, double r) tuple)
            : this(tuple.h, tuple.k, tuple.r)
        { }

        /// <summary>
        /// Deconstructs the specified h.
        /// </summary>
        /// <param name="h">The h.</param>
        /// <param name="k">The k.</param>
        /// <param name="r">The r.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Deconstruct(out double h, out double k, out double r) => (h, k, r) = (H, K, R);

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
        /// Gets or sets the r.
        /// </summary>
        /// <value>
        /// The r.
        /// </value>
        public double R { get; set; }

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
        public void DrawShape(Graphics gr, Point offset, float scale) => Rendering.DrawCircle(gr, Pen ?? Pens.Black, offset, scale, this);

        /// <summary>
        /// Converts to a conic section.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ConicSection ToUnitConicSection() => Conversion.CircleToUnitConicSection(R, H, K);

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString() => $"{nameof(Circle)}({nameof(H)}: {H}, {nameof(K)}: {K}, {nameof(R)}: {R})";
    }
}
