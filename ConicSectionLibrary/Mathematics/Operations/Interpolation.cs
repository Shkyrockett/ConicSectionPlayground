// <copyright file="InterpolationD.cs">
//     Copyright © 2019 - 2022 Shkyrockett. All rights reserved.
// </copyright>
// <author id="shkyrockett">Shkyrockett</author>
// <license>
//     Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </license>
// <summary></summary>
// <remarks></remarks>

using System.Numerics;
using System.Runtime.CompilerServices;

namespace ConicSectionLibrary;

/// <summary>
/// 
/// </summary>
public static partial class Interpolation
{
    /// <summary>
    /// Calculate G(x).
    /// </summary>
    /// <param name="y">The x.</param>
    /// <param name="a1">The a1.</param>
    /// <param name="b1">The b1.</param>
    /// <param name="c1">The c1.</param>
    /// <param name="d1">The d1.</param>
    /// <param name="e1">The e1.</param>
    /// <param name="f1">The f1.</param>
    /// <param name="sign1">The sign1.</param>
    /// <param name="a2">The a2.</param>
    /// <param name="b2">The b2.</param>
    /// <param name="c2">The c2.</param>
    /// <param name="d2">The d2.</param>
    /// <param name="e2">The e2.</param>
    /// <param name="f2">The f2.</param>
    /// <param name="sign2">The sign2.</param>
    /// <returns></returns>
    /// <acknowledgment>
    /// http://csharphelper.com/blog/2014/11/draw-a-conic-section-from-its-polynomial-equation-in-c/
    /// </acknowledgment>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static T ConicSectionUnitDifferenceCarteseanY<T>(T y,
        T a1, T b1, T c1, T d1, T e1, T f1, int sign1,
        T a2, T b2, T c2, T d2, T e2, T f2, int sign2) where T : IFloatingPointIeee754<T>
        => UnitConicSectionCarteseanY(y, a1, b1, c1, d1, e1, f1, sign1) - UnitConicSectionCarteseanY(y, a2, b2, c2, d2, e2, f2, sign2);

    /// <summary>
    /// Calculate G1(x).
    /// root_sign is -1 or 1.
    /// </summary>
    /// <param name="y">The x.</param>
    /// <param name="a">a.</param>
    /// <param name="b">The b.</param>
    /// <param name="c">The c.</param>
    /// <param name="d">The d.</param>
    /// <param name="e">The e.</param>
    /// <param name="f">The f.</param>
    /// <param name="rootSign">The root sign.</param>
    /// <returns></returns>
    /// <acknowledgment>
    /// http://csharphelper.com/blog/2014/11/draw-a-conic-section-from-its-polynomial-equation-in-c/
    /// </acknowledgment>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static T UnitConicSectionCarteseanY<T>(T y, T a, T b, T c, T d, T e, T f, int rootSign)
        where T : IFloatingPointIeee754<T>
    {
        T two = T.CreateChecked(2);
        T four = T.CreateChecked(4);
        T sign = T.CreateChecked(rootSign);
        var result = (b * y) + e;
        result *= result;
        result -= four * c * ((a * (y * y)) + (d * y) + f);
        result = sign * T.Sqrt(result);
        result = -((b * y) + e) + result;
        result = result / two / c;
        return result;
    }

    /// <summary>
    /// Interpolates the parabola standard.
    /// </summary>
    /// <param name="t">The t.</param>
    /// <param name="a">a.</param>
    /// <param name="b">The b.</param>
    /// <param name="c">The c.</param>
    /// <returns></returns>
    /// <acknowledgment>
    /// https://www.youtube.com/watch?v=4Af3NBN34ME
    /// </acknowledgment>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static (T X, T Y) StandardParabola<T>(T t, T a, T b, T c) where T : INumber<T> => (t, (a * (t * t)) + ((b * t) + c)); // Equation for finding the y of a parabola in standard form.

    /// <summary>
    /// Interpolates the parabola vertex.
    /// </summary>
    /// <param name="t">The t.</param>
    /// <param name="a">a.</param>
    /// <param name="h">The h.</param>
    /// <param name="k">The k.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static (T X, T Y) VertexParabola<T>(T t, T a, T h, T k) where T : INumber<T> => (t, (a * (t - h) * (t - h)) + k); // Equation for finding the y of a parabola in vertex form.

    /// <summary>
    /// Interpolate a point on an Ellipse.
    /// </summary>
    /// <param name="t">Theta of interpolation.</param>
    /// <param name="cX">Center x-coordinate.</param>
    /// <param name="cY">Center y-coordinate.</param>
    /// <param name="r1">The first radius of the Ellipse.</param>
    /// <param name="r2">The second radius of the Ellipse.</param>
    /// <param name="angle">Angle of rotation of Ellipse about it's center.</param>
    /// <returns>
    /// Interpolated point at theta.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static (T X, T Y) Ellipse<T>(T t, T cX, T cY, T r1, T r2, T angle) where T : IFloatingPointIeee754<T> => Ellipse(T.Cos(t), T.Sin(t), cX, cY, r1, r2, T.Cos(angle), T.Sin(angle));

    /// <summary>
    /// Interpolate a point on an Ellipse.
    /// </summary>
    /// <param name="t">Theta of interpolation.</param>
    /// <param name="cX">Center x-coordinate.</param>
    /// <param name="cY">Center y-coordinate.</param>
    /// <param name="r1">The first radius of the Ellipse.</param>
    /// <param name="r2">The second radius of the Ellipse.</param>
    /// <param name="cosAngle">Horizontal rotation transform of the Ellipse.</param>
    /// <param name="sinAngle">Vertical rotation transform of the Ellipse.</param>
    /// <returns>
    /// Interpolated point at theta.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static (T X, T Y) Ellipse<T>(T t, T cX, T cY, T r1, T r2, T cosAngle, T sinAngle) where T : IFloatingPointIeee754<T> => Ellipse(T.Cos(t), T.Sin(t), cX, cY, r1, r2, cosAngle, sinAngle);

    /// <summary>
    /// Interpolate a point on an Ellipse.
    /// </summary>
    /// <param name="cosTheta">Theta cosine of interpolation.</param>
    /// <param name="sinTheta">Theta sine of interpolation.</param>
    /// <param name="cX">Center x-coordinate.</param>
    /// <param name="cY">Center y-coordinate.</param>
    /// <param name="r1">The first radius of the Ellipse.</param>
    /// <param name="r2">The second radius of the Ellipse.</param>
    /// <param name="cosAngle">Horizontal rotation transform of the Ellipse.</param>
    /// <param name="sinAngle">Vertical rotation transform of the Ellipse.</param>
    /// <returns>
    /// Interpolated point at theta.
    /// </returns>
    /// <acknowledgment>
    /// http://www.vbforums.com/showthread.php?686351-RESOLVED-Elliptical-orbit
    /// </acknowledgment>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static (T X, T Y) Ellipse<T>(T cosTheta, T sinTheta, T cX, T cY, T r1, T r2, T cosAngle, T sinAngle) where T : INumber<T>
    {
        // Ellipse equation for an ellipse at origin.
        var i = r1 * cosTheta;
        var j = -(r2 * sinTheta);

        // Apply the rotation transformation then translate to new center.
        return (
            X: cX + ((i * cosAngle) + (j * sinAngle)),
            Y: cY + ((i * sinAngle) - (j * cosAngle)));
    }

    #region Ellipse Extremes
    /// <summary>
    /// Get the points of the Cartesian extremes of a circle.
    /// </summary>
    /// <param name="x">The x-coordinate of the center of the circle.</param>
    /// <param name="y">The y-coordinate of the center of the circle.</param>
    /// <param name="r">The r.</param>
    /// <returns>
    /// Returns the points of extreme for a circle.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static unsafe Span<(T X, T Y)> CircleExtremePoints<T>(T x, T y, T r)
        where T : INumber<T>
    {
        //Span<(T X, T Y)> points = stackalloc (T X, T Y)[4]
        Span<(T X, T Y)> points = new (T X, T Y)[4]
        {
                (X: x, Y: y - r),
                (X: x - r, Y: y),
                (X: x, Y: y + r),
                (X: x + r, Y: y),
        };
        return points;
    }

    /// <summary>
    /// Get the points of the Cartesian extremes of a rotated ellipse.
    /// </summary>
    /// <param name="x">The x-coordinate of the center of the ellipse.</param>
    /// <param name="y">The y-coordinate of the center of the ellipse.</param>
    /// <param name="rX">The horizontal radius of the ellipse.</param>
    /// <param name="rY">The vertical radius of the ellipse.</param>
    /// <returns>
    /// Returns the points of extreme for an ellipse.
    /// </returns>
    /// <acknowledgment>
    /// Based roughly on the principles found at:
    /// http://stackoverflow.com/questions/87734/how-do-you-calculate-the-axis-aligned-bounding-box-of-an-ellipse
    /// </acknowledgment>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Span<(T X, T Y)> OrthogonalEllipseExtremePoints<T>(T x, T y, T rX, T rY)
        where T : INumber<T>
    {
        if (rX == rY)
        {
            return CircleExtremePoints(x, y, rX);
        }

        //Span<(T X, T Y)> points = stackalloc (T X, T Y)[4]
        Span<(T X, T Y)> points = new (T X, T Y)[4]
        {
                (X: x, Y: y - rY),
                (X: x - rX, Y: y),
                (X: x, Y: y + rY),
                (X: x + rX, Y: y),
        };
        return points;
    }

    /// <summary>
    /// Get the points of the Cartesian extremes of a rotated ellipse.
    /// </summary>
    /// <param name="x">The x-coordinate of the center of the ellipse.</param>
    /// <param name="y">The y-coordinate of the center of the ellipse.</param>
    /// <param name="rX">The horizontal radius of the ellipse.</param>
    /// <param name="rY">The vertical radius of the ellipse.</param>
    /// <param name="angle">The angle of orientation of the ellipse.</param>
    /// <returns>
    /// Returns the points of extreme for an ellipse.
    /// </returns>
    /// <acknowledgment>
    /// Based roughly on the principles found at:
    /// http://stackoverflow.com/questions/87734/how-do-you-calculate-the-axis-aligned-bounding-box-of-an-ellipse
    /// </acknowledgment>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Span<(TOther X, TOther Y)> EllipseExtremePoints<T, TOther>(T x, T y, T rX, T rY, TOther angle) where T : INumber<T> where TOther : IFloatingPointIeee754<TOther> => EllipseExtremePoints(x, y, rX, rY, TOther.Cos(angle), TOther.Sin(angle));

    /// <summary>
    /// Get the points of the Cartesian extremes of a rotated ellipse.
    /// </summary>
    /// <param name="x">The x-coordinate of the center of the ellipse.</param>
    /// <param name="y">The y-coordinate of the center of the ellipse.</param>
    /// <param name="rX">The horizontal radius of the ellipse.</param>
    /// <param name="rY">The vertical radius of the ellipse.</param>
    /// <param name="cosAngle">The cosine component of the angle of orientation of the ellipse.</param>
    /// <param name="sinAngle">The sine component of the angle of orientation of the ellipse.</param>
    /// <returns>
    /// Returns the points of extreme for an ellipse.
    /// </returns>
    /// <acknowledgment>
    /// Based roughly on the principles found at:
    /// http://stackoverflow.com/questions/87734/how-do-you-calculate-the-axis-aligned-bounding-box-of-an-ellipse
    /// </acknowledgment>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Span<(TOther X, TOther Y)> EllipseExtremePoints<T, TOther>(T x, T y, T rX, T rY, TOther cosAngle, TOther sinAngle)
        where T : INumber<T>
        where TOther : IFloatingPointIeee754<TOther>
    {
        TOther cos0 = TOther.Cos(TOther.Zero);
        TOther sin0 = TOther.Sin(TOther.Zero);
        TOther _x = TOther.CreateChecked(x);
        TOther _y = TOther.CreateChecked(y);
        TOther _rX = TOther.CreateChecked(rX);
        TOther _rY = TOther.CreateChecked(rY);

        if (rX == rY)
        {
            return CircleExtremePoints(_x, _y, _rX);
        }

        if (cosAngle == cos0 && sinAngle == sin0)
        {
            return OrthogonalEllipseExtremePoints(_x, _y, _rX, _rY);
        }

        // Fix imprecise handling of Cos(PI/2) which breaks ellipses at right angles to each other.
        if (sinAngle == TOther.One || sinAngle == TOther.NegativeOne)
        {
            cosAngle = TOther.Zero;
        }

        // Calculate the radii of the angle of rotation.
        var a = _rX * cosAngle;
        var b = _rY * sinAngle;
        var c = _rX * sinAngle;
        var d = _rY * cosAngle;

        // Find the angles of the Cartesian extremes.
        var a1 = TOther.Atan2(-b, a);
        var a2 = TOther.Atan2(b, -a); // + PI; // sin(t + pi) = -sin(t); cos(t + pi)=-cos(t)
        var a3 = TOther.Atan2(d, c);
        var a4 = TOther.Atan2(-d, -c); // + PI; // sin(t + pi) = -sin(t); cos(t + pi)=-cos(t)

        // Return the points of Cartesian extreme of the rotated ellipse.
        //Span<(U X, U Y)> points = stackalloc (U X, U Y)[4]
        Span<(TOther X, TOther Y)> points = new (TOther X, TOther Y)[4]
        {
                Ellipse(a1, _x, _y, _rX, _rY, cosAngle, sinAngle),
                Ellipse(a2, _x, _y, _rX, _rY, cosAngle, sinAngle),
                Ellipse(a3, _x, _y, _rX, _rY, cosAngle, sinAngle),
                Ellipse(a4, _x, _y, _rX, _rY, cosAngle, sinAngle)
        };
        return points;
    }
    #endregion
}
