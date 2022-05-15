// <copyright file="Line.cs">
//     Copyright © 2019 - 2022 Shkyrockett. All rights reserved.
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
using System.Numerics;
using System.Runtime.CompilerServices;

namespace ConicSectionLibrary;

/// <summary>
/// 
/// </summary>
/// <seealso cref="IGeometry" />
[DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public class Line<T>
    : IGeometry
    where T : INumber<T>
{
    #region Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="Line{T}" /> class.
    /// </summary>
    /// <param name="x">a x.</param>
    /// <param name="y">a y.</param>
    /// <param name="i">The b x.</param>
    /// <param name="j">The b y.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public Line(T x, T y, T i, T j)
    {
        (X, Y, I, J) = (x, y, i, j);
    }
    #endregion

    #region Deconstructors
    /// <summary>
    /// Deconstructs the specified Line.
    /// </summary>
    /// <param name="x">The x.</param>
    /// <param name="y">The y.</param>
    /// <param name="i">The i.</param>
    /// <param name="j">The j.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public void Deconstruct(out T x, out T y, out T i, out T j) => (x, y, i, j) = (X, Y, I, J);
    #endregion

    #region Properties
    /// <summary>
    /// Gets or sets the x.
    /// </summary>
    /// <value>
    /// The x.
    /// </value>
    public T X { get; set; }

    /// <summary>
    /// Gets or sets the y.
    /// </summary>
    /// <value>
    /// The y.
    /// </value>
    public T Y { get; set; }

    /// <summary>
    /// Gets or sets the i.
    /// </summary>
    /// <value>
    /// The i.
    /// </value>
    public T I { get; set; }

    /// <summary>
    /// Gets or sets the j.
    /// </summary>
    /// <value>
    /// The j.
    /// </value>
    public T J { get; set; }

    /// <summary>
    /// Gets or sets the pen.
    /// </summary>
    /// <value>
    /// The pen.
    /// </value>
    public IDisposable? Pen { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>
    /// The name.
    /// </value>
    public string? Name { get; set; }
    #endregion

    #region Events
    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Raises the property changed event.
    /// </summary>
    /// <param name="name">The name.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public void OnPropertyChanged([CallerMemberName] string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    #endregion

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
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public ConicSection<T> ToUnitConicSection() => Conversion.LineToUnitConicSection(X, Y, I, J);

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
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public override string ToString() => $"{nameof(Line<T>)}({nameof(X)}: {X}, {nameof(Y)}: {Y}, {nameof(I)}: {I}, {nameof(J)}: {J})";

    /// <summary>
    /// Gets the debugger display.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private string GetDebuggerDisplay() => ToString();
}
