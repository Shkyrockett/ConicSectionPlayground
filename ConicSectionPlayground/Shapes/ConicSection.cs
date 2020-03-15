// <copyright file="ConicSection.cs">
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
    public class ConicSection
        : IShape
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConicSection"/> class.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <param name="c">The c.</param>
        /// <param name="d">The d.</param>
        /// <param name="e">The e.</param>
        /// <param name="f">The f.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ConicSection(double a, double b, double c, double d, double e, double f)
        {
            A = a;
            B = b;
            C = c;
            D = d;
            E = e;
            F = f;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConicSection"/> class.
        /// </summary>
        /// <param name="tuple">The p.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ConicSection((double a, double b, double c, double d, double e, double f) tuple)
            : this(tuple.a, tuple.b, tuple.c, tuple.d, tuple.e, tuple.f)
        { }

        /// <summary>
        /// Deconstructs the specified a.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <param name="c">The c.</param>
        /// <param name="d">The d.</param>
        /// <param name="e">The e.</param>
        /// <param name="f">The f.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Deconstruct(out double a, out double b, out double c, out double d, out double e, out double f) => (a, b, c, d, e, f) = (A, B, C, D, E, F);

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
        /// Gets or sets the d.
        /// </summary>
        /// <value>
        /// The d.
        /// </value>
        public double D { get; set; }

        /// <summary>
        /// Gets or sets the e.
        /// </summary>
        /// <value>
        /// The e.
        /// </value>
        public double E { get; set; }

        /// <summary>
        /// Gets or sets the f.
        /// </summary>
        /// <value>
        /// The f.
        /// </value>
        public double F { get; set; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public ConicSectionType Type => Conversion.IdentifyConicSectionType(A, B, C, D, E, F);

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
        public void DrawShape(Graphics gr, Point offset, float scale) => Rendering.DrawConicSection(gr, Pen ?? Pens.Black, offset, scale, this);

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString() => $"{nameof(ConicSection)}({nameof(A)}: {A}, {nameof(B)}: {B}, {nameof(C)}: {C}, {nameof(D)}: {D}, {nameof(E)}: {E}, {nameof(F)}: {F})";
    }
}
