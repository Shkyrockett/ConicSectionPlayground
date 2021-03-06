// <copyright file="CubicBezierSegment.cs">
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
    public class CubicBezierSegment
        : IGeometry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CubicBezierSegment" /> class.
        /// </summary>
        /// <param name="aX">a x.</param>
        /// <param name="aY">a y.</param>
        /// <param name="bX">The b x.</param>
        /// <param name="bY">The b y.</param>
        /// <param name="cX">The c x.</param>
        /// <param name="cY">The c y.</param>
        /// <param name="dX">The d x.</param>
        /// <param name="dY">The d y.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public CubicBezierSegment(double aX, double aY, double bX, double bY, double cX, double cY, double dX, double dY)
        {
            (AX, AY, BX, BY, CX, CY, DX, DY) = (aX, aY, bX, bY, cX, cY, dX, dY);
        }

        /// <summary>
        /// Deconstructs the specified CubicBezierSegment.
        /// </summary>
        /// <param name="aX">a x.</param>
        /// <param name="aY">a y.</param>
        /// <param name="bX">The b x.</param>
        /// <param name="bY">The b y.</param>
        /// <param name="cX">The c x.</param>
        /// <param name="cY">The c y.</param>
        /// <param name="dX">The d x.</param>
        /// <param name="dY">The d y.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Deconstruct(out double aX, out double aY, out double bX, out double bY, out double cX, out double cY, out double dX, out double dY) => (aX, aY, bX, bY, cX, cY, dX, dY) = (AX, AY, BX, BY, CX, CY, DX, DY);

        /// <summary>
        /// Gets or sets the ax.
        /// </summary>
        /// <value>
        /// The ax.
        /// </value>
        public double AX { get; set; }

        /// <summary>
        /// Gets or sets the ay.
        /// </summary>
        /// <value>
        /// The ay.
        /// </value>
        public double AY { get; set; }

        /// <summary>
        /// Gets or sets the bx.
        /// </summary>
        /// <value>
        /// The bx.
        /// </value>
        public double BX { get; set; }

        /// <summary>
        /// Gets or sets the by.
        /// </summary>
        /// <value>
        /// The by.
        /// </value>
        public double BY { get; set; }

        /// <summary>
        /// Gets or sets the cx.
        /// </summary>
        /// <value>
        /// The cx.
        /// </value>
        public double CX { get; set; }

        /// <summary>
        /// Gets or sets the cy.
        /// </summary>
        /// <value>
        /// The cy.
        /// </value>
        public double CY { get; set; }

        /// <summary>
        /// Gets or sets the dx.
        /// </summary>
        /// <value>
        /// The dx.
        /// </value>
        public double DX { get; set; }

        /// <summary>
        /// Gets or sets the dy.
        /// </summary>
        /// <value>
        /// The dy.
        /// </value>
        public double DY { get; set; }

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
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

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
        /// Raises the property changed event.
        /// </summary>
        /// <param name="name">The name.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnPropertyChanged([CallerMemberName] string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

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
        public override string ToString() => $"{nameof(CubicBezierSegment)}({nameof(AX)}: {AX}, {nameof(AY)}: {AY}, {nameof(BX)}: {BX}, {nameof(BY)}: {BY}, {nameof(CX)}: {CX}, {nameof(CY)}: {CY}, {nameof(DX)}: {DX}, {nameof(DY)}: {DY})";

        /// <summary>
        /// Gets the debugger display.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string GetDebuggerDisplay() => ToString();
    }
}
