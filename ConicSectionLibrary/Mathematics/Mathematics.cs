// <copyright file="MathematicsD.cs">
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
using static ConicSectionLibrary.Interpolation;
using static System.Math;

namespace ConicSectionLibrary;

/// <summary>
/// The mathematics.
/// </summary>
public static partial class Mathematics
{
    #region Constants
    /// <summary>
    /// The one third constant.
    /// </summary>
    public const double OneThird = 1d / 3d;

    /// <summary>
    /// The one half constant.
    /// </summary>
    public const double OneHalf = 1d / 2d;

    /// <summary>
    /// The two thirds constant.
    /// </summary>
    public const double TwoThirds = 2d / 3d;

    /// <summary>
    /// One Radian.
    /// </summary>
    /// <remarks>
    /// PI / 180
    /// </remarks>
    public const double Radian = PI / 180d; // 0.01745329251994329576923690768489d;

    /// <summary>
    /// One degree.
    /// </summary>
    /// <remarks>
    /// 180 / PI
    /// </remarks>
    public const double Degree = 180d / PI; // 57.295779513082320876798154814105d;

    /// <summary>
    /// The cosine of 0.
    /// </summary>
    public static readonly double Cos0 = Cos(0d);

    /// <summary>
    /// The sine of 0.
    /// </summary>
    public static readonly double Sin0 = Sin(0d);

    /// <summary>
    /// A value close to 0.
    /// </summary>
    public const double small = 0.1d;

    /// <summary>
    /// The scale per mouse wheel delta.
    /// </summary>
    public const float scale_per_delta = 0.1f / 120f;
    #endregion

    #region Is Number
    /// <summary>
    /// Determines whether the specified number is number.
    /// Return true if the number is not infinity or NaN.
    /// </summary>
    /// <param name="number">The number.</param>
    /// <returns>
    ///   <c>true</c> if the specified number is number; otherwise, <c>false</c>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool IsNumber<T>(this T number) where T : IFloatingPointIeee754<T> => !(T.IsNaN(number) || T.IsInfinity(number));
    #endregion

    #region Min Point
    /// <summary>
    /// The min point.
    /// </summary>
    /// <param name="point1X">The point1X.</param>
    /// <param name="point1Y">The point1Y.</param>
    /// <param name="point2X">The point2X.</param>
    /// <param name="point2Y">The point2Y.</param>
    /// <returns>
    /// The <see cref="ValueTuple{T1, T2}" />.
    /// </returns>
    /// <acknowledgment>
    /// http://www.kevlindev.com/
    /// </acknowledgment>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static (T X, T Y) MinPoint<T>(T point1X, T point1Y, T point2X, T point2Y) where T : INumber<T> => (T.Min(point1X, point2X), T.Min(point1Y, point2Y));
    #endregion Min Point

    #region Max Point
    /// <summary>
    /// The max point.
    /// </summary>
    /// <param name="point1X">The point1X.</param>
    /// <param name="point1Y">The point1Y.</param>
    /// <param name="point2X">The point2X.</param>
    /// <param name="point2Y">The point2Y.</param>
    /// <returns>
    /// The <see cref="ValueTuple{T1, T2}" />.
    /// </returns>
    /// <acknowledgment>
    /// http://www.kevlindev.com/
    /// </acknowledgment>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static (T X, T Y) MaxPoint<T>(T point1X, T point1Y, T point2X, T point2Y) where T : INumber<T> => (T.Max(point1X, point2X), T.Max(point1Y, point2Y));
    #endregion Max Point

    /// <summary>
    /// Scales the factor.
    /// </summary>
    /// <param name="scale">The scale.</param>
    /// <param name="delta">The delta.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static T MouseWheelScaleFactor<T>(T scale, int delta)
        where T : IFloatingPointIeee754<T>
    {
        T two = T.CreateChecked(2);
        T _scale_per_delta = T.CreateChecked(scale_per_delta);
        T _delta = T.CreateChecked(delta);
        scale += _delta * _scale_per_delta;
        return (scale <= T.Zero) ? two * T.Epsilon : scale;
    }

    #region Point Manipulation Methods
    /// <summary>
    /// Inverses the scale point.
    /// </summary>
    /// <param name="point">The point.</param>
    /// <param name="scale">The scale.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static (T X, T Y) ScreenToObject_<T>((T X, T Y) point, T scale)
        where T : INumber<T>
    {
        var invScale = T.One / scale;
        return new(invScale * point.X, invScale * point.Y);
    }

    /// <summary>
    /// Screens to object.
    /// </summary>
    /// <param name="point">The point.</param>
    /// <param name="scale">The scale.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static (T X, T Y) ScreenToObject<T>((T X, T Y) point, T scale) where T : INumber<T> => new(point.X / scale, point.Y / scale);

    /// <summary>
    /// Inverses the translation and scale of a point.
    /// </summary>
    /// <param name="offset">The offset.</param>
    /// <param name="point">The point.</param>
    /// <param name="scale">The scale.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static (T X, T Y) ScreenToObject_<T>((T X, T Y) offset, (T X, T Y) point, T scale) where T : INumber<T>
    {
        var invScale = T.One / scale;
        return new((point.X - offset.X) * invScale, (point.Y - offset.Y) * invScale);
    }

    /// <summary>
    /// Screens to object. https://stackoverflow.com/a/37269366
    /// </summary>
    /// <param name="offset">The offset.</param>
    /// <param name="point">The screen point.</param>
    /// <param name="scale">The scale.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static (T X, T Y) ScreenToObject<T>((T X, T Y) offset, (T X, T Y) point, T scale) where T : INumber<T> => new((point.X - offset.X) / scale, (point.Y - offset.Y) / scale);

    /// <summary>
    /// Screens to object transposed matrix.
    /// </summary>
    /// <param name="offset">The offset.</param>
    /// <param name="screenPoint">The screen point.</param>
    /// <param name="scale">The scale.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static (T X, T Y) ScreenToObjectTransposedMatrix_<T>((T X, T Y) offset, (T X, T Y) screenPoint, T scale) where T : INumber<T>
    {
        var invScale = T.One / scale;
        return new((screenPoint.X * invScale) - offset.X, (screenPoint.Y * invScale) - offset.Y);
    }

    /// <summary>
    /// Screens to object transposed matrix.
    /// </summary>
    /// <param name="startingPoint">The starting point.</param>
    /// <param name="Location">The location.</param>
    /// <param name="scale">The scale.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static (T X, T Y) ScreenToObjectTransposedMatrix<T>((T X, T Y) startingPoint, (T X, T Y) Location, T scale) where T : INumber<T> => new((Location.X / scale) - startingPoint.X, (Location.Y / scale) - startingPoint.Y);

    /// <summary>
    /// Screens to object transposed matrix.
    /// </summary>
    /// <param name="startingPoint">The starting point.</param>
    /// <param name="Location">The location.</param>
    /// <param name="scale">The scale.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static (T X, T Y) ScreenToObjectTransposedMatrix<T>((T X, T Y) startingPoint, (T X, T Y) Location, (T X, T Y) scale) where T : INumber<T> => new((Location.X / scale.X) - startingPoint.X, (Location.Y / scale.Y) - startingPoint.Y);

    /// <summary>
    /// Scales the specified scale.
    /// </summary>
    /// <param name="point">The point.</param>
    /// <param name="scale">The scale.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static (T X, T Y) ObjectToScreen<T>((T X, T Y) point, T scale) where T : INumber<T> => new(point.X * scale, point.Y * scale);

    /// <summary>
    /// Objects to screen. https://stackoverflow.com/a/37269366
    /// </summary>
    /// <param name="offset">The offset.</param>
    /// <param name="point">The object point.</param>
    /// <param name="scale">The scale.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static (T X, T Y) ObjectToScreen<T>((T X, T Y) offset, (T X, T Y) point, T scale) where T : INumber<T> => new((offset.X * scale) - point.X, (offset.Y * scale) - point.Y);

    /// <summary>
    /// Objects to screen transposed matrix. https://stackoverflow.com/a/37269366
    /// </summary>
    /// <param name="offset">The offset.</param>
    /// <param name="objectPoint">The object point.</param>
    /// <param name="scale">The scale.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static (T X, T Y) ObjectToScreenTransposedMatrix<T>((T X, T Y) offset, (T X, T Y) objectPoint, T scale) where T : INumber<T> => new((offset.X + objectPoint.X) * scale, (offset.Y + objectPoint.Y) * scale);

    /// <summary>
    /// Zooms at. https://stackoverflow.com/a/37269366
    /// </summary>
    /// <param name="offset">The offset.</param>
    /// <param name="cursor">The cursor.</param>
    /// <param name="previousScale">The previous scale.</param>
    /// <param name="scale">The scale.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static (T X, T Y) ZoomAt<T>((T X, T Y) offset, (T X, T Y) cursor, T previousScale, T scale) where T : INumber<T>
    {
        var point = ScreenToObject(offset, cursor, previousScale);
        point = ObjectToScreen(offset, point, scale);
        return new(offset.X + ((cursor.X - point.X) / scale), offset.Y + ((cursor.Y - point.Y) / scale));
    }

    /// <summary>
    /// Zooms at for a transposed matrix. https://stackoverflow.com/a/37269366
    /// </summary>
    /// <param name="offset">The offset.</param>
    /// <param name="cursor">The cursor.</param>
    /// <param name="previousScale">The previous scale.</param>
    /// <param name="scale">The scale.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static (T X, T Y) ZoomAtTransposedMatrix<T>((T X, T Y) offset, (T X, T Y) cursor, T previousScale, T scale) where T : INumber<T>
    {
        var point = ScreenToObjectTransposedMatrix(offset, cursor, previousScale);
        point = ObjectToScreenTransposedMatrix(offset, point, scale);
        return new(offset.X + ((cursor.X - point.X) / scale), offset.Y + ((cursor.Y - point.Y) / scale));
    }
    #endregion Point Manipulation Methods

    /// <summary>
    /// Finds the roots using binary division.
    /// </summary>
    /// <param name="xmin">The xmin.</param>
    /// <param name="xmax">The xmax.</param>
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
    /// http://csharphelper.com/blog/2014/11/see-where-two-conic-sections-intersect-in-c/
    /// </acknowledgment>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static List<(T X, T Y)> FindRootsUsingBinaryDivision<T>(T xmin, T xmax,
        T a1, T b1, T c1, T d1, T e1, T f1, int sign1,
        T a2, T b2, T c2, T d2, T e2, T f2, int sign2)
        where T : IFloatingPointIeee754<T>
    {
        var roots = new List<(T X, T Y)>();
        const int num_tests = 100;
        var delta_x = (xmax - xmin) / (T.CreateChecked(num_tests) - T.One);
        T _small = T.CreateChecked(small);

        // Loop over the possible x values looking for roots.
        var x0 = xmin;
        for (var i = 0; i < num_tests; i++)
        {
            // Try to find a root in this range.
            (var x, var y) = UseBinaryDivision(x0, delta_x,
                a1, b1, c1, d1, e1, f1, sign1,
                a2, b2, c2, d2, e2, f2, sign2);

            // See if we have already found this root.
            if (IsNumber(y))
            {
                var is_new = true;
                foreach (var (X, Y) in roots)
                {
                    if (T.Abs(X - x) < _small)
                    {
                        is_new = false;
                        break;
                    }
                }

                // If this is a new point, save it.
                if (is_new)
                {
                    roots.Add((x, y));

                    // If we've found two roots, we won't find any more.
                    if (roots.Count > 1)
                    {
                        return roots;
                    }
                }
            }

            x0 += delta_x;
        }

        return roots;
    }

    /// <summary>
    /// Find a root by using binary division.
    /// </summary>
    /// <param name="x0">The x0.</param>
    /// <param name="delta_x">The delta x.</param>
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
    /// http://csharphelper.com/blog/2014/11/see-where-two-conic-sections-intersect-in-c/
    /// </acknowledgment>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private static (T x, T y) UseBinaryDivision<T>(T x0, T delta_x,
        T a1, T b1, T c1, T d1, T e1, T f1, int sign1,
        T a2, T b2, T c2, T d2, T e2, T f2, int sign2)
        where T : IFloatingPointIeee754<T>
    {
        const int num_trials = 200;
        const int sgn_nan = -2;
        T two = T.CreateChecked(2);
        T _small = T.CreateChecked(small);

        // Get G(x) for the bounds.
        var xmin = x0;
        var g_xmin = ConicSectionUnitDifferenceCarteseanY(xmin,
            a1, b1, c1, d1, e1, f1, sign1,
            a2, b2, c2, d2, e2, f2, sign2);
        if (T.Abs(g_xmin) < _small)
        {
            return (xmin, g_xmin);
        }

        var xmax = xmin + delta_x;
        var g_xmax = ConicSectionUnitDifferenceCarteseanY(xmax,
            a1, b1, c1, d1, e1, f1, sign1,
            a2, b2, c2, d2, e2, f2, sign2);
        if (T.Abs(g_xmax) < _small)
        {
            return (xmax, g_xmax);
        }

        // Get the sign of the values.
        int sgn_min, sgn_max;
        if (IsNumber(g_xmin))
        {
            sgn_min = (T.Sign(g_xmin));
        }
        else
        {
            sgn_min = sgn_nan;
        }

        if (IsNumber(g_xmax))
        {
            sgn_max = (T.Sign(g_xmax));
        }
        else
        {
            sgn_max = sgn_nan;
        }

        // If the two values have the same sign,
        // then there is no root here.
        if (sgn_min == sgn_max)
        {
            return (T.One, T.NaN);
        }

        // Use binary division to find the point of intersection.
        var xmid = T.Zero;
        var g_xmid = T.Zero;
        for (var i = 0; i < num_trials; i++)
        {
            // Get values for the midpoint.
            xmid = (xmin + xmax) / two;
            g_xmid = ConicSectionUnitDifferenceCarteseanY(xmid,
                a1, b1, c1, d1, e1, f1, sign1,
                a2, b2, c2, d2, e2, f2, sign2);
            int sgn_mid;
            if (IsNumber(g_xmid))
            {
                sgn_mid = (T.Sign(g_xmid));
            }
            else
            {
                sgn_mid = sgn_nan;
            }

            // If sgn_mid is 0, gxmid is 0 so this is the root.
            if (sgn_mid == 0)
            {
                break;
            }

            // See which half contains the root.
            if (sgn_mid == sgn_min)
            {
                // The min and mid values have the same sign.
                // Search the right half.
                xmin = xmid;
                g_xmin = g_xmid;
            }
            else if (sgn_mid == sgn_max)
            {
                // The max and mid values have the same sign.
                // Search the left half.
                xmax = xmid;
                g_xmax = g_xmid;
            }
            else
            {
                // The three values have different signs.
                // Assume min or max is NaN.
                if (sgn_min == sgn_nan)
                {
                    // Value g_xmin is NaN. Use the right half.
                    xmin = xmid;
                    g_xmin = g_xmid;
                }
                else if (sgn_max == sgn_nan)
                {
                    // Value g_xmax is NaN. Use the right half.
                    xmax = xmid;
                    g_xmax = g_xmid;
                }
                else
                {
                    // This is a weird case. Just trap it.
                    //throw new InvalidOperationException(
                    //    "Unexpected difference curve. " +
                    //    "Cannot find a root between X = " +
                    //    xmin + " and X = " + xmax);
                }
            }
        }

        if (IsNumber(g_xmid) && (T.Abs(g_xmid) < _small))
        {
            return (xmid, g_xmid);
        }
        else if (IsNumber(g_xmin) && (T.Abs(g_xmin) < _small))
        {
            return (xmin, g_xmin);
        }
        else if (IsNumber(g_xmax) && (T.Abs(g_xmax) < _small))
        {
            return (xmax, g_xmax);
        }
        else
        {
            return (xmid, T.NaN);
        }
    }
}
