// <copyright file="Ellipse.cs">
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
public class Ellipse<T>
    : IGeometry
    where T : IFloatingPointIeee754<T>
{
    #region Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="Ellipse{T}" /> class.
    /// </summary>
    /// <param name="h">The h.</param>
    /// <param name="k">The k.</param>
    /// <param name="r">The r x.</param>
    /// <param name="a">a.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public Ellipse(T h, T k, T r, T a)
        : this(h, k, r, r, a)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Ellipse{T}" /> class.
    /// </summary>
    /// <param name="h">The h.</param>
    /// <param name="k">The k.</param>
    /// <param name="rX">The r x.</param>
    /// <param name="rY">The r y.</param>
    /// <param name="a">a.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public Ellipse(T h, T k, T rX, T rY, T a)
    {
        (H, K, RX, RY, A) = (h, k, rX, rY, a);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Ellipse{T}" /> class.
    /// </summary>
    /// <param name="tuple">The tuple.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public Ellipse((T h, T k, T rX, T rY, T a) tuple)
        : this(tuple.h, tuple.k, tuple.rX, tuple.rY, tuple.a)
    { }
    #endregion

    #region Deconstructors
    /// <summary>
    /// Deconstructs the specified Ellipse.
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
    /// Gets or sets the Radius x.
    /// </summary>
    /// <value>
    /// The rx.
    /// </value>
    public T RX { get; set; }

    /// <summary>
    /// Gets the Diameter x.
    /// </summary>
    /// <value>
    /// The dx.
    /// </value>
    public T DX { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => RX * T.CreateChecked(2); }

    /// <summary>
    /// Gets or sets the Radius y.
    /// </summary>
    /// <value>
    /// The ry.
    /// </value>
    public T RY { get; set; }

    /// <summary>
    /// Gets the Diameter y.
    /// </summary>
    /// <value>
    /// The dy.
    /// </value>
    public T DY { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => RY * T.CreateChecked(2); }

    /// <summary>
    /// Gets or sets a.
    /// </summary>
    /// <value>
    /// a.
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
    /// Gets the center.
    /// </summary>
    /// <value>
    /// The center.
    /// </value>
    public (T X, T Y) Center
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        get
        {
            T oneHalf = T.CreateChecked(0.5);
            T two = T.CreateChecked(2);
            return new(((oneHalf * (RX * two)) + H), ((oneHalf * (RY * two)) + K));
        }
    }

    ///// <summary>
    ///// Gets the unit conic section.
    ///// </summary>
    ///// <value>
    ///// The unit conic section.
    ///// </value>
    //[MergableProperty(true)]
    //[Category("Properties")]
    //public ConicSection UnitConicSection => ToUnitConicSection();

    ///// <summary>
    ///// Gets the conic section.
    ///// </summary>
    ///// <value>
    ///// The conic section.
    ///// </value>
    //[MergableProperty(true)]
    //[Category("Properties")]
    //public ConicSection ConicSection => ToConicSection();
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
    /// Converts to a unit conic section.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public ConicSection<T> ToUnitConicSection() => Conversion.EllipseToUnitConicSection(RX, RY, H, K, A);

    /// <summary>
    /// Translates the specified delta.
    /// </summary>
    /// <param name="delta">The delta.</param>
    /// <returns></returns>
    public IGeometry Translate(Vector2 delta) => throw new NotImplementedException();

    /// <summary>
    /// Includes the specified point.
    /// </summary>
    /// <param name="point">The point.</param>
    /// <returns></returns>
    public bool Includes(PointF point) => throw new NotImplementedException();

    /// <summary>
    /// Converts to a conic section.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public ConicSection<T> ToConicSection() => Conversion.EllipseToConicSection(RX, RY, H, K, A);

    /// <summary>
    /// Converts to string.
    /// </summary>
    /// <param name="format">The format.</param>
    /// <param name="formatProvider">The format provider.</param>
    /// <returns></returns>
    public string ToString(string format, IFormatProvider formatProvider) => ToString();

    /// <summary>
    /// Converts to string.
    /// </summary>
    /// <returns>
    /// A <see cref="string" /> that represents this instance.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public override string ToString() => $"{nameof(Ellipse<T>)}({nameof(H)}: {H}, {nameof(K)}: {K}, {nameof(RX)}: {RX}, {nameof(RY)}: {RY}, {nameof(A)}: {A})";

    /// <summary>
    /// Gets the debugger display.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private string GetDebuggerDisplay() => ToString();
}
