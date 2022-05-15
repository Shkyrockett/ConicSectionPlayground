// <copyright file="Circle.cs">
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
/// <seealso cref="IGeometry" />
[DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public class Circle<T>
    : IGeometry
    where T : INumber<T>
{
    #region Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="Circle{T}" /> class.
    /// </summary>
    /// <param name="h">The h.</param>
    /// <param name="k">The k.</param>
    /// <param name="r">The r.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public Circle(T h, T k, T r)
    {
        (H, K, R) = (h, k, r);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Circle{T}" /> class.
    /// </summary>
    /// <param name="tuple">The tuple.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public Circle((T h, T k, T r) tuple)
        : this(tuple.h, tuple.k, tuple.r)
    { }
    #endregion

    #region Deconstructors
    /// <summary>
    /// Deconstructs the specified h.
    /// </summary>
    /// <param name="h">The h.</param>
    /// <param name="k">The k.</param>
    /// <param name="r">The r.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public void Deconstruct(out T h, out T k, out T r) => (h, k, r) = (H, K, R);
    #endregion

    #region Properties
    /// <summary>
    /// Gets or sets the h.
    /// </summary>
    /// <value>
    /// The h.
    /// </value>
    public T H { get; set; }

    /// <summary>
    /// Gets or sets the k.
    /// </summary>
    /// <value>
    /// The k.
    /// </value>
    public T K { get; set; }

    /// <summary>
    /// Gets or sets the r.
    /// </summary>
    /// <value>
    /// The r.
    /// </value>
    public T R { get; set; }

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
    /// Occurs when [property changed].
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
    public ConicSection<T> ToUnitConicSection() => Conversion.CircleToUnitConicSection(R, H, K);

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
    public override string ToString() => $"{nameof(Circle<T>)}({nameof(H)}: {H}, {nameof(K)}: {K}, {nameof(R)}: {R})";

    /// <summary>
    /// Gets the debugger display.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private string GetDebuggerDisplay() => ToString();
}
