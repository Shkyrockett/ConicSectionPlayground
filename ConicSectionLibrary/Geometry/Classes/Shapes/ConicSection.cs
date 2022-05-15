// <copyright file="ConicSection.cs">
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
public class ConicSection<T>
    : IGeometry
    where T : INumber<T>
{
    #region Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="ConicSection{T}" /> class.
    /// </summary>
    /// <param name="a">a.</param>
    /// <param name="b">The b.</param>
    /// <param name="c">The c.</param>
    /// <param name="d">The d.</param>
    /// <param name="e">The e.</param>
    /// <param name="f">The f.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public ConicSection(T a, T b, T c, T d, T e, T f)
    {
        (A, B, C, D, E, F) = (a, b, c, d, e, f);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConicSection{T}" /> class.
    /// </summary>
    /// <param name="tuple">The p.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public ConicSection((T a, T b, T c, T d, T e, T f) tuple)
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
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public void Deconstruct(out T a, out T b, out T c, out T d, out T e, out T f) => (a, b, c, d, e, f) = (A, B, C, D, E, F);
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
    /// Gets or sets the b.
    /// </summary>
    /// <value>
    /// The b.
    /// </value>
    public T B { get; set; }

    /// <summary>
    /// Gets or sets the c.
    /// </summary>
    /// <value>
    /// The c.
    /// </value>
    public T C { get; set; }

    /// <summary>
    /// Gets or sets the d.
    /// </summary>
    /// <value>
    /// The d.
    /// </value>
    public T D { get; set; }

    /// <summary>
    /// Gets or sets the e.
    /// </summary>
    /// <value>
    /// The e.
    /// </value>
    public T E { get; set; }

    /// <summary>
    /// Gets or sets the f.
    /// </summary>
    /// <value>
    /// The f.
    /// </value>
    public T F { get; set; }

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
    public override string ToString() => $"{nameof(ConicSection<T>)}({nameof(A)}: {A}, {nameof(B)}: {B}, {nameof(C)}: {C}, {nameof(D)}: {D}, {nameof(E)}: {E}, {nameof(F)}: {F})";

    /// <summary>
    /// Gets the debugger display.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private string GetDebuggerDisplay() => ToString();
}
