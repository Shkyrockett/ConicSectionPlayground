// <copyright file="VertexParabola.cs">
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
public class VertexParabola<T>
    : IGeometry
    where T : IFloatingPointIeee754<T>
{
    #region Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="VertexParabola{T}" /> class.
    /// </summary>
    /// <param name="a">a.</param>
    /// <param name="h">The h.</param>
    /// <param name="k">The k.</param>
    /// <param name="i">The i.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public VertexParabola(T a, T h, T k, T? i = default)
    {
        (A, H, K, I) = (a, h, k, i ?? T.Zero);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VertexParabola{T}" /> class.
    /// </summary>
    /// <param name="tuple">The tuple.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public VertexParabola((T a, T h, T k, T i) tuple)
        : this(tuple.a, tuple.h, tuple.k, tuple.i)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="VertexParabola{T}" /> class.
    /// </summary>
    /// <param name="tuple">The tuple.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public VertexParabola((T a, T h, T k) tuple)
        : this(tuple.a, tuple.h, tuple.k, T.Zero)
    { }
    #endregion

    #region Properties
    /// <summary>
    /// Gets or sets the a.
    /// </summary>
    /// <value>
    /// The a.
    /// </value>
    public T A { get; set; }

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
    /// Gets or sets the i.
    /// </summary>
    /// <value>
    /// The i.
    /// </value>
    public T I { get; set; }

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
    /// Converts to standard parabola.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public (T a, T h, T k) ToStandardParabola() => Conversion.VertexParabolaToStandardParabola(A, H, K);

    /// <summary>
    /// Converts to quadratic bezier.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public (T aX, T aY, T bX, T bY, T cX, T cY) ToQuadraticBezier(T left, T right) => Conversion.VertexParabolaToQuadraticBezier(A, H, K, left, right);

    /// <summary>
    /// Converts to quadratic bezier.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="top">The top.</param>
    /// <param name="right">The right.</param>
    /// <param name="bottom">The bottom.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public (T aX, T aY, T bX, T bY, T cX, T cY) ToQuadraticBezier(T left, T top, T right, T bottom) => Conversion.VertexParabolaToQuadraticBezier(A, H, K, I, left, top, right, bottom);

    /// <summary>
    /// Converts to cubic bezier.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public (T aX, T aY, T bX, T bY, T cX, T cY, T dX, T dY) ToCubicBezier(T left, T right) => Conversion.QuadraticBezierToCubicBezier(ToQuadraticBezier(left, right));

    /// <summary>
    /// Converts to cubic bezier.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="top">The top.</param>
    /// <param name="right">The right.</param>
    /// <param name="bottom">The bottom.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public (T aX, T aY, T bX, T bY, T cX, T cY, T dX, T dY) ToCubicBezier(T left, T top, T right, T bottom) => Conversion.QuadraticBezierToCubicBezier(ToQuadraticBezier(left, top, right, bottom));

    /// <summary>
    /// Converts to a conic section.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public ConicSection<T> ToUnitConicSection() => Conversion.VertexParabolaToUnitConicSection((A, H, K));

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
    public override string ToString() => $"{nameof(VertexParabola<T>)}({nameof(A)}: {A}, {nameof(H)}: {H}, {nameof(K)}: {K}, {nameof(I)}: {I})";

    /// <summary>
    /// Gets the debugger display.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private string GetDebuggerDisplay() => ToString();
}
