// <copyright file="ConversionD.cs">
//     Copyright © 2019 - 2022 Shkyrockett. All rights reserved.
// </copyright>
// <author id="shkyrockett">Shkyrockett</author>
// <license>
//     Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </license>
// <summary></summary>
// <remarks></remarks>

using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;
using static ConicSectionLibrary.Mathematics;

namespace ConicSectionLibrary;

/// <summary>
/// 
/// </summary>
public static partial class Conversion
{
    #region Trig
    /// <summary>
    /// Convert Degrees to Radians.
    /// </summary>
    /// <param name="degrees">Angle in Degrees.</param>
    /// <returns>
    /// Angle in Radians.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static T DegreesToRadians<T>(this T degrees) where T : IFloatingPointIeee754<T> => degrees * (T.Pi / T.CreateChecked(180));

    /// <summary>
    /// Convert Radians to Degrees.
    /// </summary>
    /// <param name="radians">Angle in Radians.</param>
    /// <returns>
    /// Angle in Degrees.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static T RadiansToDegrees<T>(this T radians) where T : IFloatingPointIeee754<T> => radians * (T.CreateChecked(180) / T.Pi);
    #endregion

    #region Cast Conversion
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static TResult Cast<T, TResult>(this T value) where T : INumber<T> where TResult : INumber<TResult> => TResult.CreateChecked(value);

    /// <summary>
    /// Converts <see cref="ValueTuple" /> to <see cref="PointF" />.
    /// </summary>
    /// <param name="tuple">The tuple.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static PointF ToPointF<T>(this (T X, T Y) tuple) where T : INumber<T> => new(Cast<T, float>(tuple.X), Cast<T, float>(tuple.Y));

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="point"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static (T X, T Y) ToTuple<T>(this Point point) where T : INumber<T> => (T.CreateChecked(point.X), T.CreateChecked(point.Y));

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="point"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static (T X, T Y) ToTuple<T>(this PointF point) where T : INumber<T> => (T.CreateChecked(point.X), T.CreateChecked(point.Y));
    #endregion

    /// <summary>
    /// Identifies the type of the conic section.
    /// https://math.libretexts.org/Bookshelves/Precalculus/Book%3A_Precalculus_(OpenStax)/10%3A_Analytic_Geometry/10.4%3A_Rotation_of_Axes
    /// https://www.shelovesmath.com/precal/conics/
    /// </summary>
    /// <param name="a">a.</param>
    /// <param name="b">The b.</param>
    /// <param name="c">The c.</param>
    /// <param name="d">The d.</param>
    /// <param name="e">The e.</param>
    /// <param name="f">The f.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static ConicSectionType IdentifyConicSectionType<T>(T a, T b, T c, T d, T e, T f)
        where T : INumber<T>
    {
        _ = d;
        _ = e;
        T epsilon = T.CreateChecked(0.00001d);
        T four = T.CreateChecked(4);

        // Calculate the determinant.
        var determinant = (b * b) - (four * a * c);

        if (T.Abs(determinant) < epsilon)
        {
            return (a == T.Zero && b == T.Zero && c == T.Zero) ? ConicSectionType.Line : ConicSectionType.Parabola;
        }
        else if (determinant < T.Zero)
        {
            return f == T.Zero ? ConicSectionType.Point : (T.Abs(a) < epsilon) && (T.Abs(b) < epsilon) ? ConicSectionType.Circle : ConicSectionType.Ellipse;
        }
        else
        {
            return T.Abs(a + c) < epsilon ? T.Sign(a) != T.Sign(b) ? ConicSectionType.CrossingLines : ConicSectionType.RectangularHyperbola : ConicSectionType.Hyperbola;
        }
    }

    #region Conic Section
    /// <summary>
    /// Point to conic section.
    /// </summary>
    /// <param name="h">The h.</param>
    /// <param name="k">The k.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static ConicSection<T> PointToUnitConicSection<T>(T h, T k) where T : INumber<T> => new(
            a: T.One,
            b: T.One,
            c: T.Zero, -T.CreateChecked(2d) * h,
            e: -T.CreateChecked(2d) * k,
            f: (h * h) + (k * k) - T.One);

    /// <summary>
    /// Lines to conic section.
    /// </summary>
    /// <param name="x">a x.</param>
    /// <param name="y">a y.</param>
    /// <param name="i">The b x.</param>
    /// <param name="j">The b y.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static ConicSection<T> LineToUnitConicSection<T>(T x, T y, T i, T j)
        where T : INumber<T>
    {
        T two = T.CreateChecked(2);
        var dX = i;// - x;
        var dY = j;// - y;

        return new ConicSection<T>(
            a: (x * x) + (two * x * dX) + (dX * dX),
            b: (x * y) + (x * dX) + (y * dX) + (dX * dY),
            c: (y * y) + (two * y * dY) + (dY * dY),
            d: x + dX,
            e: y + dY,
            f: T.Zero);
    }

    /// <summary>
    /// Line Segments to conic section.
    /// </summary>
    /// <param name="x1">a x.</param>
    /// <param name="y1">a y.</param>
    /// <param name="x2">The b x.</param>
    /// <param name="y2">The b y.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static ConicSection<T> LineSegmentToUnitConicSection<T>(T x1, T y1, T x2, T y2)
        where T : INumber<T>
    {
        T two = T.CreateChecked(2);
        var dX = x2 - x1;
        var dY = y2 - y1;
        return new ConicSection<T>(
            a: (x1 * x1) + (two * x1 * dX) + (dX * dX),
            b: (x1 * y1) + (x1 * dX) + (y1 * dX) + (dX * dY),
            c: (y1 * y1) + (two * y1 * dY) + (dY * dY),
            d: x1 + dX,
            e: y1 + dY,
            f: T.Zero);
    }

    /// <summary>
    /// Parabolas to conic section.
    /// </summary>
    /// <param name="tuple">The tuple.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static ConicSection<T> VertexParabolaToUnitConicSection<T>((T a, T h, T k) tuple) where T : INumber<T> => ParabolaToConicSection(tuple.a, tuple.h, tuple.k);

    /// <summary>
    /// Convert a parabola from vertex form into standard form.
    /// </summary>
    /// <param name="a">The <paramref name="a" /> component of the parabola.</param>
    /// <param name="h">The horizontal component of the parabola vertex.</param>
    /// <param name="k">The vertical component of the parabola vertex.</param>
    /// <returns>
    /// Returns  <see cref="ValueTuple{T1, T2, T3}" /> representing the a, b, and c values of the standard form of a parabola.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static ConicSection<T> ParabolaToConicSection<T>(T a, T h, T k)
        where T : INumber<T>
    {
        T two = T.CreateChecked(2);
        T four = T.CreateChecked(4);
        // ToDo: Fix. This does not work as expected.
        var b = -two * a * h;
        return new ConicSection<T>(b, T.Zero, a, b * b / (four * a), k, T.One);
    }

    /// <summary>
    /// Parabolas to conic section.
    /// </summary>
    /// <param name="a">a.</param>
    /// <param name="h">The h.</param>
    /// <param name="k">The k.</param>
    /// <param name="i">The i.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static ConicSection<T> ParabolaToConicSection<T>(T a, T h, T k, T i)
        where T : IFloatingPointIeee754<T>
    {
        T two = T.CreateChecked(2);
        T four = T.CreateChecked(4);
        // ToDo: Fix. This does not work as expected.
        var cos = T.Cos(i);
        var sin = T.Sin(i);
        _ = cos;
        _ = sin;
        var b = -two * a * h;
        return new ConicSection<T>(a, T.Zero, b, T.Zero, (b * b / (four * a)) + k, T.One);
    }

    /// <summary>
    /// Parabola to conic section.
    /// </summary>
    /// <param name="dX">The dx.</param>
    /// <param name="dY">The dy.</param>
    /// <param name="sX">The sx.</param>
    /// <param name="sY">The sy.</param>
    /// <returns></returns>
    /// <acknowledgment>
    /// http://csharphelper.com/blog/2014/11/see-where-a-parabola-and-hyperbola-intersect-in-c/
    /// </acknowledgment>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static ConicSection<T> ParabolaToConicSection2<T>(T dX, T dY, T sX, T sY) where T : INumber<T> =>
        // ToDo: Figure out what this is doing.
        new(
            a: T.Zero,
            b: T.Zero,
            c: -sY * sY,
            d: sX,
            e: T.CreateChecked(2) * sY * dY,
            f: (-sX * dX) - (dY * dY));

    /// <summary>
    /// Finds the conic section.
    /// </summary>
    /// <param name="a">a.</param>
    /// <param name="b">The b.</param>
    /// <param name="c">The c.</param>
    /// <param name="d">The d.</param>
    /// <param name="e">The e.</param>
    /// <returns></returns>
    /// <acknowledgment>
    /// http://csharphelper.com/blog/2014/11/select-a-conic-section-in-c/
    /// </acknowledgment>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static ConicSection<T>? FindUnitConicSectionFromPoints<T>((T X, T Y) a, (T X, T Y) b, (T X, T Y) c, (T X, T Y) d, (T X, T Y) e)
        where T : IFloatingPointIeee754<T>
    {
        const int rows = 5;
        const int cols = 5;

        // Build the augmented matrix.
        var points = new[] { a, b, c, d, e };
        var arr = new T[rows, cols + 2];
        for (var row = 0; row < rows; row++)
        {
            arr[row, 0] = points[row].X * points[row].X;
            arr[row, 1] = points[row].X * points[row].Y;
            arr[row, 2] = points[row].Y * points[row].Y;
            arr[row, 3] = points[row].X;
            arr[row, 4] = points[row].Y;
            arr[row, 5] = T.NegativeOne;
            arr[row, 6] = T.Zero;
        }

        // Perform Gaussian elimination.
        for (var r = 0; r < rows - 1; r++)
        {
            // Zero out all entries in column r after this row.
            // See if this row has a non-zero entry in column r.
            if (T.Abs(arr[r, r]) < T.Epsilon)
            {
                // Too close to zero. Try to swap with a later row.
                for (var r2 = r + 1; r2 < rows; r2++)
                {
                    if (T.Abs(arr[r2, r]) > T.Epsilon)
                    {
                        // This row will work. Swap them.
                        for (var j = 0; j <= cols; j++)
                        {
                            var tmp = arr[r, j];
                            arr[r, j] = arr[r2, j];
                            arr[r2, j] = tmp;
                        }
                        break;
                    }
                }
            }

            // If this row has a non-zero entry in column r, use it.
            if (T.Abs(arr[r, r]) > T.Epsilon)
            {
                // Zero out this column in later rows.
                for (var r2 = r + 1; r2 < rows; r2++)
                {
                    var factor = -arr[r2, r] / arr[r, r];
                    for (var j = r; j <= cols; j++)
                    {
                        arr[r2, j] = arr[r2, j] + (factor * arr[r, j]);
                    }
                }
            }
        }

        // See if we have a solution.
        if (arr[rows - 1, cols - 1] == T.Zero)
        {
            // We have no solution.
            // See if all of the entries in this row are 0.
            var all_zeros = true;
            for (var j = 0; j <= cols + 1; j++)
            {
                if (arr[rows - 1, j] != T.Zero)
                {
                    all_zeros = false;
                    break;
                }
            }
            if (all_zeros)
            {
                //MessageBox.Show("The solution is not unique");
            }
            else
            {
                //MessageBox.Show("There is no solution");
            }
            return null;
        }
        else
        {
            // Back solve.
            for (var r = rows - 1; r >= 0; r--)
            {
                var tmp = arr[r, cols];
                for (var r2 = r + 1; r2 < rows; r2++)
                {
                    tmp -= arr[r, r2] * arr[r2, cols + 1];
                }

                arr[r, cols + 1] = tmp / arr[r, r];
            }

            // Return the results.
            return new ConicSection<T>(arr[0, cols + 1], arr[1, cols + 1], arr[2, cols + 1], arr[3, cols + 1], arr[4, cols + 1], T.One);
        }
    }

    /// <summary>
    /// Converts the circle to conic section sextic.
    /// </summary>
    /// <param name="r">a.</param>
    /// <param name="h">The h.</param>
    /// <param name="k">The k.</param>
    /// <returns></returns>
    /// <acknowledgment>
    /// http://csharphelper.com/blog/2014/11/calculate-the-formula-for-an-ellipse-selected-by-the-user-in-c/
    /// http://csharphelper.com/blog/2014/11/see-where-two-ellipses-intersect-in-c-part-1/
    /// </acknowledgment>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static ConicSection<T> CircleToUnitConicSection<T>(T r, T h, T k)
        where T : INumber<T>
    {
        if (r == T.Zero)
        {
            return PointToUnitConicSection(h, k);
        }

        T two = T.CreateChecked(2);

        h += r;
        k += r;

        var r2 = r * r;

        return new ConicSection<T>(
            a: T.One / r2,
            b: T.One / r2,
            c: T.Zero, -two * h / r2,
            e: -two * k / r2,
            f: (h * h / r2) + (k * k / r2) - T.One);
    }

    /// <summary>
    /// Converts an Orthographic ellipse to conic section.
    /// </summary>
    /// <param name="a">The r x.</param>
    /// <param name="b">The r y.</param>
    /// <param name="h">The h.</param>
    /// <param name="k">The k.</param>
    /// <returns></returns>
    /// <acknowledgment>
    /// http://csharphelper.com/blog/2014/11/calculate-the-formula-for-an-ellipse-selected-by-the-user-in-c/
    /// http://csharphelper.com/blog/2014/11/see-where-two-ellipses-intersect-in-c-part-1/
    /// </acknowledgment>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static ConicSection<T> OrthogonalEllipseToUnitConicSection<T>(T a, T b, T h, T k)
        where T : INumber<T>
    {
        if (a == b)
        {
            return CircleToUnitConicSection(a, h, k);
        }

        T two = T.CreateChecked(2);

        // Shift over to the center.
        h += a;
        k += b;

        var coefA = T.One / (a * a);
        var coefB = T.Zero;
        var coefC = T.One / (b * b);
        var scale = T.One;
        return new ConicSection<T>(
            a: coefA,
            b: coefB,
            c: coefC,
            d: -two * h * coefA,
            e: -two * k * coefC,
            f: (h * h * coefA) + (k * k * coefC) - scale);
    }

    /// <summary>
    /// Converts the ellipse to conic section sextic.
    /// </summary>
    /// <param name="a">a.</param>
    /// <param name="b">The b.</param>
    /// <param name="h">The h.</param>
    /// <param name="k">The k.</param>
    /// <param name="angle">The theta.</param>
    /// <returns></returns>
    /// <acknowledgment>
    /// https://math.stackexchange.com/a/2989928
    /// </acknowledgment>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static ConicSection<T> EllipseToUnitConicSection<T>(T a, T b, T h, T k, T angle) where T : IFloatingPointIeee754<T> => EllipseToUnitConicSection(a, b, h, k, T.Cos(angle), T.Sin(angle));

    /// <summary>
    /// Converts the ellipse to conic section sextic.
    /// </summary>
    /// <param name="a">a.</param>
    /// <param name="b">The b.</param>
    /// <param name="h">The h.</param>
    /// <param name="k">The k.</param>
    /// <param name="cos">The cos.</param>
    /// <param name="sin">The sin.</param>
    /// <returns></returns>
    /// <acknowledgment>
    /// https://math.stackexchange.com/a/2989928
    /// </acknowledgment>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static ConicSection<T> EllipseToUnitConicSection<T>(T a, T b, T h, T k, T cos, T sin)
        where T : IFloatingPointIeee754<T>
    {
        if (a == b)
        {
            return OrthogonalEllipseToUnitConicSection(a, b, h, k);
        }

        T two = T.CreateChecked(2);

        // Fix imprecise handling of Cos(PI/2) which breaks ellipses at right angles to each other.
        if (sin == T.One || sin == T.NegativeOne)
        {
            cos = T.Zero;
        }

        // Shift over to the center.
        h += a;
        k += b;

        var coefA = (cos * cos / (a * a)) + (sin * sin / (b * b));
        var coefB = two * sin * cos * ((T.One / (a * a)) - (T.One / (b * b)));
        var coefC = (sin * sin / (a * a)) + (cos * cos / (b * b));
        var scale = T.One;
        return new ConicSection<T>(
            a: coefA,
            b: coefB,
            c: coefC,
            d: (-two * h * coefA) - (k * coefB),
            e: (-two * k * coefC) - (h * coefB),
            f: (h * h * coefA) + (h * k * coefB) + (k * k * coefC) - scale);
    }

    /// <summary>
    /// Calculates a conic section polynomial that represents the provided rotated ellipse.
    /// </summary>
    /// <param name="h">The center X coordinate.</param>
    /// <param name="k">The center Y coordinate.</param>
    /// <param name="a">The width of the ellipse.</param>
    /// <param name="b">The height of the ellipse.</param>
    /// <param name="angle">The angle.</param>
    /// <returns></returns>
    /// <acknowledgment>
    /// https://en.wikipedia.org/wiki/Ellipse#General_ellipse
    /// </acknowledgment>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static ConicSection<T> EllipseToConicSection<T>(T h, T k, T a, T b, T angle) where T : IFloatingPointIeee754<T> => EllipseToConicSection(h, k, a, b, T.Cos(angle), T.Sin(angle));

    /// <summary>
    /// Calculates a conic section polynomial that represents the provided rotated ellipse.
    /// </summary>
    /// <param name="h">The center X coordinate.</param>
    /// <param name="k">The center Y coordinate.</param>
    /// <param name="a">The width of the ellipse.</param>
    /// <param name="b">The height of the ellipse.</param>
    /// <param name="cos">The cosine of the angle of the ellipse.</param>
    /// <param name="sin">The sine of the angle of the ellipse.</param>
    /// <returns></returns>
    /// <acknowledgment>
    /// https://en.wikipedia.org/wiki/Ellipse#General_ellipse
    /// </acknowledgment>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static ConicSection<T> EllipseToConicSection<T>(T h, T k, T a, T b, T cos, T sin)
        where T : IFloatingPointIeee754<T>
    {
        // Fix imprecise handling of Cos(PI/2) which breaks ellipses at right angles to each other.
        if (sin == T.One || sin == T.NegativeOne)
        {
            cos = T.Zero;
        }

        T two = T.CreateChecked(2);

        var coefA = (cos * cos * (a * a)) + (sin * sin * (b * b));
        var coefB = two * sin * cos * ((b * b) - (a * a));
        var coefC = (sin * sin * (a * a)) + (cos * cos * (b * b));
        return new ConicSection<T>(
            a: coefA,
            b: coefB,
            c: coefC,
            d: (-two * h * coefA) - (k * coefB),
            e: (-two * k * coefC) - (h * coefB),
            f: (h * h * coefA) + (h * k * coefB) + (k * k * coefC) - (a * a * (b * b)));
    }

    /// <summary>
    /// Hyperbolas to conic section.
    /// </summary>
    /// <param name="a">The r x.</param>
    /// <param name="b">The r y.</param>
    /// <param name="h">The h.</param>
    /// <param name="k">The k.</param>
    /// <param name="angle">a.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static ConicSection<T> HyperbolaToConicSection<T>(T a, T b, T h, T k, T angle) where T : IFloatingPointIeee754<T> => HyperbolaToConicSection(a, b, h, k, T.Cos(angle), T.Sin(angle));

    /// <summary>
    /// Hyperbolas to conic section.
    /// </summary>
    /// <param name="rX">The r x.</param>
    /// <param name="rY">The r y.</param>
    /// <param name="h">The h.</param>
    /// <param name="k">The k.</param>
    /// <param name="cos">The cos.</param>
    /// <param name="sin">The sin.</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static ConicSection<T> HyperbolaToConicSection<T>(T rX, T rY, T h, T k, T cos, T sin)
        where T : IFloatingPointIeee754<T>
    {
        _ = rX;
        _ = rY;
        _ = h;
        _ = k;
        _ = cos;
        // Fix imprecise handling of Cos(PI/2) which breaks ellipses at right angles to each other.
        if (sin == T.One || sin == T.NegativeOne)
        {
            cos = T.Zero;
            _ = cos;
        }
        // ToDo: Implement
        throw new NotImplementedException();
    }

    /// <summary>
    /// Rescales the sextic.
    /// </summary>
    /// <param name="conicSection">The conic section.</param>
    /// <returns></returns>
    /// <acknowledgment>
    /// http://csharphelper.com/blog/2014/11/select-a-conic-section-in-c/
    /// </acknowledgment>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static ConicSection<T> RescaleConicSection<T>(ConicSection<T> conicSection) where T : INumber<T>
    {
        var min = SexticAbsMin(conicSection.A, conicSection.B, conicSection.C, conicSection.D, conicSection.E, conicSection.F);
        var scale = T.One / min;
        return new ConicSection<T>(conicSection.A *= scale, conicSection.B *= scale, conicSection.C *= scale, conicSection.D *= scale, conicSection.E *= scale, conicSection.F *= scale);
    }

    /// <summary>
    /// Converts a conic section to matrix form.
    /// </summary>
    /// <param name="a">a.</param>
    /// <param name="b">The b.</param>
    /// <param name="c">The c.</param>
    /// <param name="d">The d.</param>
    /// <param name="e">The e.</param>
    /// <param name="f">The f.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static unsafe T[,] ConicSectionToMatrix<T>(T a, T b, T c, T d, T e, T f)
        where T : INumber<T>
    {
        T oneHalf = T.CreateChecked(0.5);
        return new T[3, 3] { { a, b * oneHalf, d * oneHalf }, { b * oneHalf, c, e * oneHalf }, { d * oneHalf, e * oneHalf, f } };
    }

    /// <summary>
    /// Converts a matrix form of a Conic Section to Standard Conic Section form.
    /// </summary>
    /// <param name="matrix">The matrix.</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static (T a, T b, T c, T d, T e, T f) MatrixToConicSection<T>(T[,] matrix)
        where T : INumber<T>
    {
        T oneHalf = T.CreateChecked(0.5);
        T two = T.CreateChecked(2);
        if (matrix.GetLength(0) != 3 || matrix.GetLength(1) != 3) throw new Exception();
        return (matrix[0, 0], matrix[0, 1] * oneHalf, matrix[1, 1], matrix[0, 2] * oneHalf, matrix[1, 2] * two, matrix[2, 2]);
    }
    #endregion

    /// <summary>
    /// Rescales the sextic.
    /// </summary>
    /// <param name="a">a.</param>
    /// <param name="b">The b.</param>
    /// <param name="c">The c.</param>
    /// <param name="d">The d.</param>
    /// <param name="e">The e.</param>
    /// <param name="f">The f.</param>
    /// <returns></returns>
    /// <acknowledgment>
    /// http://csharphelper.com/blog/2014/11/select-a-conic-section-in-c/
    /// </acknowledgment>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static (T a, T b, T c, T d, T e, T f) RescaleSextic<T>(T a, T b, T c, T d, T e, T f)
        where T : INumber<T>
    {
        var min = SexticAbsMin(a, b, c, d, e, f);
        var scale = T.One / min;
        return (a *= scale, b *= scale, c *= scale, d *= scale, e *= scale, f *= scale);
    }

    /// <summary>
    /// Find the nearest value to 0 in a Sextic polynomial.
    /// </summary>
    /// <param name="a">a.</param>
    /// <param name="b">The b.</param>
    /// <param name="c">The c.</param>
    /// <param name="d">The d.</param>
    /// <param name="e">The e.</param>
    /// <param name="f">The f.</param>
    /// <returns></returns>
    /// <acknowledgment>
    /// http://csharphelper.com/blog/2014/11/select-a-conic-section-in-c/
    /// </acknowledgment>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static T SexticAbsMin<T>(T a, T b, T c, T d, T e, T f)
        where T : INumber<T>
    {
        var min = T.Abs(a);
        min = T.Min(min, T.Abs(b));
        min = T.Min(min, T.Abs(c));
        min = T.Min(min, T.Abs(d));
        min = T.Min(min, T.Abs(e));
        min = T.Min(min, T.Abs(f));
        return min;
    }

    /// <summary>
    /// Standards the parabola to vertex parabola.
    /// </summary>
    /// <param name="tuple">The tuple.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static (T a, T h, T k) StandardParabolaToVertexParabola<T>((T a, T b, T c) tuple) where T : INumber<T> => StandardParabolaToVertexParabola(tuple.a, tuple.b, tuple.c);

    /// <summary>
    /// Convert a parabola from standard form into vertex form.
    /// </summary>
    /// <param name="a">The <paramref name="a" /> component of the parabola.</param>
    /// <param name="b">The <paramref name="b" /> component of the parabola.</param>
    /// <param name="c">The <paramref name="c" /> component of the parabola.</param>
    /// <returns>
    /// Returns  <see cref="ValueTuple{T1, T2, T3}" /> representing the a, h, and k values of the vertex form of a parabola.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static (T a, T h, T k) StandardParabolaToVertexParabola<T>(T a, T b, T c)
        where T : INumber<T>
    {
        T two = T.CreateChecked(2);
        T four = T.CreateChecked(4);
        return (a, h: -(b / (two * a)), k: -(b * b / (four * a)) + c);
    }

    /// <summary>
    /// Vertexes the parabola to standard parabola.
    /// </summary>
    /// <param name="tuple">The tuple.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static (T a, T b, T c) VertexParabolaToStandardParabola<T>((T a, T h, T k) tuple) where T : INumber<T> => VertexParabolaToStandardParabola(tuple.a, tuple.h, tuple.k);

    /// <summary>
    /// Vertexes the parabola to standard parabola.
    /// </summary>
    /// <param name="tuple">The tuple.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static (T a, T b, T c, T i) VertexParabolaToStandardParabola<T>((T a, T h, T k, T i) tuple) where T : INumber<T> => VertexParabolaToStandardParabola(tuple.a, tuple.h, tuple.k, tuple.i);

    /// <summary>
    /// Convert a parabola from vertex form into standard form.
    /// </summary>
    /// <param name="a">The <paramref name="a" /> component of the parabola.</param>
    /// <param name="h">The horizontal component of the parabola vertex.</param>
    /// <param name="k">The vertical component of the parabola vertex.</param>
    /// <returns>
    /// Returns  <see cref="ValueTuple{T1, T2, T3}" /> representing the a, b, and c values of the standard form of a parabola.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static (T a, T b, T c) VertexParabolaToStandardParabola<T>(T a, T h, T k)
        where T : INumber<T>
    {
        T two = T.CreateChecked(2);
        T four = T.CreateChecked(4);
        var b = -two * a * h;
        return (a, b, c: (b * b / (four * a)) + k);
    }

    /// <summary>
    /// Convert a parabola from vertex form into standard form.
    /// </summary>
    /// <param name="a">The <paramref name="a" /> component of the parabola.</param>
    /// <param name="h">The horizontal component of the parabola vertex.</param>
    /// <param name="k">The vertical component of the parabola vertex.</param>
    /// <param name="i">The i.</param>
    /// <returns>
    /// Returns  <see cref="ValueTuple{T1, T2, T3}" /> representing the a, b, and c values of the standard form of a parabola.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static (T a, T b, T c, T i) VertexParabolaToStandardParabola<T>(T a, T h, T k, T i)
        where T : INumber<T>
    {
        T two = T.CreateChecked(2);
        T four = T.CreateChecked(4);
        var b = -two * a * h;
        return (a, b, c: (b * b / (four * a)) + k, i);
    }

    /// <summary>
    /// Find the Quadratic Bezier curve that represents the parabola.
    /// </summary>
    /// <param name="a">The a component of the Parabola.</param>
    /// <param name="b">The b component of the Parabola.</param>
    /// <param name="c">The c component of the Parabola.</param>
    /// <param name="x1">The first x position to crop the parabola.</param>
    /// <param name="x2">The second x position to crop the parabola.</param>
    /// <returns>
    /// Returns the control point locations of a Quadric Bezier curve.
    /// </returns>
    /// <acknowledgment>
    /// https://math.stackexchange.com/a/1258196
    /// </acknowledgment>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static (T ax, T ay, T bx, T by, T cx, T cy) StandardParabolaToQuadraticBezier<T>(T a, T b, T c, T x1, T x2)
        where T : INumber<T>
    {
        T oneHalf = T.CreateChecked(0.5);
        // Get the vertical components of the end points.
        var y1 = (a * x1 * x1) + (x1 * b) + c;
        var y2 = (a * x2 * x2) + (x2 * b) + c;
        // Find the intersection of the tangents at the end nodes to find the center node.
        var cx = (x2 + x1) * oneHalf;
        var cy = (a * ((x2 * x1) - (x1 * x1))) + (b * (x2 - x1) * oneHalf) + y1;
        return (x1, y1, cx, cy, x2, y2);
    }

    /// <summary>
    /// Find the Quadratic Bezier curve that represents the parabola.
    /// </summary>
    /// <param name="a">The a component of the Parabola.</param>
    /// <param name="b">The b component of the Parabola.</param>
    /// <param name="c">The c component of the Parabola.</param>
    /// <param name="angle">The angle.</param>
    /// <param name="x1">The first x position to crop the parabola.</param>
    /// <param name="y1">The y1.</param>
    /// <param name="x2">The second x position to crop the parabola.</param>
    /// <param name="y2">The y2.</param>
    /// <returns>
    /// Returns the control point locations of a Quadric Bezier curve.
    /// </returns>
    /// <acknowledgment>
    /// https://math.stackexchange.com/a/1258196
    /// </acknowledgment>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static (T ax, T ay, T bx, T by, T cx, T cy) StandardParabolaToQuadraticBezier<T>(T a, T b, T c, T angle, T x1, T y1, T x2, T y2) where T : IFloatingPointIeee754<T> => StandardParabolaToQuadraticBezier(a, b, c, T.Cos(angle), T.Sin(angle), x1, y1, x2, y2);

    /// <summary>
    /// Find the Quadratic Bezier curve that represents the parabola.
    /// </summary>
    /// <param name="a">The a component of the Parabola.</param>
    /// <param name="b">The b component of the Parabola.</param>
    /// <param name="c">The c component of the Parabola.</param>
    /// <param name="cos">The cos.</param>
    /// <param name="sin">The sin.</param>
    /// <param name="left">The first x position to crop the parabola.</param>
    /// <param name="top">The top.</param>
    /// <param name="right">The second x position to crop the parabola.</param>
    /// <param name="bottom">The bottom.</param>
    /// <returns>
    /// Returns the control point locations of a Quadric Bezier curve.
    /// </returns>
    /// <acknowledgment>
    /// https://math.stackexchange.com/a/1258196
    /// </acknowledgment>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static (T ax, T ay, T bx, T by, T cx, T cy) StandardParabolaToQuadraticBezier<T>(T a, T b, T c, T cos, T sin, T left, T top, T right, T bottom)
        where T : IFloatingPointIeee754<T>
    {
        T oneHalf = T.CreateChecked(0.5);
        // Apply a rotation transformation to the x values.
        var lx = (left * cos) - (top * sin);
        var rx = (right * cos) - (bottom * sin);

        // Get the vertical components of the end points.
        var ly = (a * lx * lx) + (lx * b) + c;
        var ry = (a * rx * rx) + (rx * b) + c;

        // Find the intersection of the tangents at the end nodes to find the center node.
        var cx = (rx + lx) * oneHalf;
        var cy = (a * ((rx * lx) - (lx * lx))) + (b * (rx - lx) * oneHalf) + ly;
        return (lx, ly, cx, cy, rx, ry);
    }

    /// <summary>
    /// Find the Quadratic Bezier curve that represents the parabola.
    /// </summary>
    /// <param name="a">The a component of the Parabola.</param>
    /// <param name="h">The horizontal component of the vertex of the parabola.</param>
    /// <param name="k">The vertical component of the vertex of the parabola.</param>
    /// <param name="left">The first x position to crop the parabola.</param>
    /// <param name="right">The second x position to crop the parabola.</param>
    /// <returns>
    /// Returns the control point locations of a Quadric Bezier curve.
    /// </returns>
    /// <acknowledgment>
    /// https://math.stackexchange.com/a/1258196
    /// </acknowledgment>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static (T ax, T ay, T bx, T by, T cx, T cy) VertexParabolaToQuadraticBezier<T>(T a, T h, T k, T left, T right)
        where T : INumber<T>
    {
        T oneHalf = T.CreateChecked(0.5);
        T two = T.CreateChecked(2);
        // Get the vertical components of the end points.
        var y1 = (a * ((h * h) + (-two * h * left) + (left * left))) + k;
        var y2 = (a * ((h * h) + (-two * h * right) + (right * right))) + k;

        // Find the intersection of the tangents at the end nodes to find the center node.
        var cx = (right + left) * oneHalf;
        var cy = (a * ((h * left) + (left * right) - (h * right) - (left * left))) + y1;
        return (left, y1, cx, cy, right, y2);
    }

    /// <summary>
    /// Find the Quadratic Bezier curve that represents the parabola.
    /// </summary>
    /// <param name="a">The a component of the Parabola.</param>
    /// <param name="h">The horizontal component of the vertex of the parabola.</param>
    /// <param name="k">The vertical component of the vertex of the parabola.</param>
    /// <param name="angle">The angle.</param>
    /// <param name="left">The first x position to crop the parabola.</param>
    /// <param name="top">The top.</param>
    /// <param name="right">The second x position to crop the parabola.</param>
    /// <param name="bottom">The bottom.</param>
    /// <returns>
    /// Returns the control point locations of a Quadric Bezier curve.
    /// </returns>
    /// <acknowledgment>
    /// https://math.stackexchange.com/a/1258196
    /// </acknowledgment>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static (T ax, T ay, T bx, T by, T cx, T cy) VertexParabolaToQuadraticBezier<T>(T a, T h, T k, T angle, T left, T top, T right, T bottom) where T : IFloatingPointIeee754<T> => VertexParabolaToQuadraticBezier(a, h, k, T.Cos(angle), T.Sin(angle), left, top, right, bottom);

    /// <summary>
    /// Find the Quadratic Bezier curve that represents the parabola.
    /// </summary>
    /// <param name="a">The a component of the Parabola.</param>
    /// <param name="h">The horizontal component of the vertex of the parabola.</param>
    /// <param name="k">The vertical component of the vertex of the parabola.</param>
    /// <param name="cos">The cos.</param>
    /// <param name="sin">The sin.</param>
    /// <param name="left">The first x position to crop the parabola.</param>
    /// <param name="top">The top.</param>
    /// <param name="right">The second x position to crop the parabola.</param>
    /// <param name="bottom">The bottom.</param>
    /// <returns>
    /// Returns the control point locations of a Quadric Bezier curve.
    /// </returns>
    /// <acknowledgment>
    /// https://math.stackexchange.com/a/1258196
    /// </acknowledgment>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static (T ax, T ay, T bx, T by, T cx, T cy) VertexParabolaToQuadraticBezier<T>(T a, T h, T k, T cos, T sin, T left, T top, T right, T bottom)
        where T : IFloatingPointIeee754<T>
    {
        // ToDo: Figure out why rotation isn't working, and the Bezier is getting cut off weird with certain rectangles.

        // ToDo: Figure out how to actually apply the rotation. This currently just changes the start and end points

        T oneHalf = T.CreateChecked(0.5);

        // Apply a rotation transformation to the x values.
        var lx = (left * cos) - (bottom * sin); // Usually 0;
        var rx = (right * cos) - (top * sin);

        // Get the vertical components of the end points.
        var y1 = (a * (lx - h) * (lx - h)) + k;
        var y2 = (a * (rx - h) * (rx - h)) + k;

        // Find the intersection of the tangents at the end nodes to find the center node.
        var cx = (rx + lx) * oneHalf;
        var cy = (a * ((h * lx) + (lx * rx) - (h * rx) - (lx * lx))) + y1;
        return (lx, y1, cx, cy, rx, y2);
    }

    /// <summary>
    /// Quadratic bezier to cubic bezier.
    /// </summary>
    /// <param name="tuple">The tuple.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static (T aX, T aY, T bX, T bY, T cX, T cY, T dX, T dY) QuadraticBezierToCubicBezier<T>((T aX, T aY, T bX, T bY, T cX, T cY) tuple) where T : INumber<T> => QuadraticBezierToCubicBezier(tuple.aX, tuple.aY, tuple.bX, tuple.bY, tuple.cX, tuple.cY);

    /// <summary>
    /// Raise a Quadratic Bezier to a Cubic Bezier.
    /// </summary>
    /// <param name="aX">The x-component of the starting point.</param>
    /// <param name="aY">The y-component of the starting point.</param>
    /// <param name="bX">The x-component of the handle.</param>
    /// <param name="bY">The y-component of the handle.</param>
    /// <param name="cX">The x-component of the end point.</param>
    /// <param name="cY">The y-component of the end point.</param>
    /// <returns>
    /// Returns Quadratic Bézier curve from a cubic curve.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static (T aX, T aY, T bX, T bY, T cX, T cY, T dX, T dY) QuadraticBezierToCubicBezier<T>(T aX, T aY, T bX, T bY, T cX, T cY)
        where T : INumber<T>
    {
        T twoThirds = T.CreateChecked(TwoThirds);
        return (aX, aY, aX + (twoThirds * (bX - aX)), aY + (twoThirds * (bY - aY)), cX + (twoThirds * (bX - cX)), cY + (twoThirds * (bY - cY)), cX, cY);
    }
}
