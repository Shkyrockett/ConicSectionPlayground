// <copyright file="Hyperbola.cs">
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
public class Hyperbola<T>
    : IGeometry
    where T : IFloatingPointIeee754<T>
{
    #region Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="Hyperbola{T}" /> class.
    /// </summary>
    /// <param name="h">The h.</param>
    /// <param name="k">The k.</param>
    /// <param name="rX">The r x.</param>
    /// <param name="rY">The r y.</param>
    /// <param name="a">a.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public Hyperbola(T h, T k, T rX, T rY, T a)
    {
        (H, K, RX, RY, A, ConicSection) = (h, k, rX, rY, a, ToUnitConicSection());
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Hyperbola{T}" /> class.
    /// </summary>
    /// <param name="tuple">The tuple.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public Hyperbola((T h, T k, T rX, T rY, T a) tuple)
        : this(tuple.h, tuple.k, tuple.rX, tuple.rY, tuple.a)
    { }
    #endregion

    #region Deconstructors
    /// <summary>
    /// Deconstructs the specified Hyperbola.
    /// </summary>
    /// <param name="h">The h.</param>
    /// <param name="k">The k.</param>
    /// <param name="rX">The r x.</param>
    /// <param name="rY">The r y.</param>
    /// <param name="a">a.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public void Deconstruct(out T h, out T k, out T rX, out T rY, out T a) => (h, k, rX, rY, a) = (H, K, RX, RY, A);
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
    /// Gets or sets the rx.
    /// </summary>
    /// <value>
    /// The rx.
    /// </value>
    public T RX { get; set; }

    /// <summary>
    /// Gets or sets the ry.
    /// </summary>
    /// <value>
    /// The ry.
    /// </value>
    public T RY { get; set; }

    /// <summary>
    /// Gets or sets a.
    /// </summary>
    /// <value>
    /// The a.
    /// </value>
    public T A { get; set; }

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

    /// <summary>
    /// Gets or sets the conic section.
    /// </summary>
    /// <value>
    /// The conic section.
    /// </value>
    public ConicSection<T> ConicSection { get; set; }
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
    public ConicSection<T> ToUnitConicSection() => Conversion.HyperbolaToConicSection(RX, RY, H, K, A);

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
    public override string ToString() => $"{nameof(Hyperbola<T>)}({nameof(H)}: {H}, {nameof(K)}: {K}, {nameof(RX)}: {RX}, {nameof(RY)}: {RY}, {nameof(A)}: {A})";

    /// <summary>
    /// Gets the debugger display.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private string GetDebuggerDisplay() => ToString();
}
