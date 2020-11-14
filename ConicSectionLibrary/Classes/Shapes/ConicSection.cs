// <copyright file="ConicSection.cs">
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
    public class ConicSection
        : IGeometry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConicSection" /> class.
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
            (A, B, C, D, E, F) = (a, b, c, d, e, f);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConicSection" /> class.
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
        /// Gets or sets the a.
        /// </summary>
        /// <value>
        /// The a.
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
        public ConicSectionType Type => ConversionD.IdentifyConicSectionType(A, B, C, D, E, F);

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
        /// Occurs when [property changed].
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        public IGeometry Translate(Vector2 delta) => throw new NotImplementedException();

        public bool Includes(PointF point) => throw new NotImplementedException();

        /// <summary>
        /// Raises the property changed event.
        /// </summary>
        /// <param name="name">The name.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnPropertyChanged([CallerMemberName] string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public string ToString(string format, IFormatProvider formatProvider) => ToString();

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString() => $"{nameof(ConicSection)}({nameof(A)}: {A}, {nameof(B)}: {B}, {nameof(C)}: {C}, {nameof(D)}: {D}, {nameof(E)}: {E}, {nameof(F)}: {F})";

        /// <summary>
        /// Gets the debugger display.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string GetDebuggerDisplay() => ToString();
    }
}
