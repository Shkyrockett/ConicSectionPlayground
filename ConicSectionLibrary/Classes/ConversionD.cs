// <copyright file="ConversionD.cs">
//     Copyright © 2019 - 2020 Shkyrockett. All rights reserved.
// </copyright>
// <author id="shkyrockett">Shkyrockett</author>
// <license>
//     Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </license>
// <summary></summary>
// <remarks></remarks>

using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using static ConicSectionLibrary.MathematicsD;
using static System.Math;

namespace ConicSectionLibrary
{
    /// <summary>
    /// 
    /// </summary>
    public static class ConversionD
    {
        #region Trig
        /// <summary>
        /// Convert Degrees to Radians.
        /// </summary>
        /// <param name="degrees">Angle in Degrees.</param>
        /// <returns>
        /// Angle in Radians.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double DegreesToRadians(this double degrees) => degrees * Radian;

        /// <summary>
        /// Convert Radians to Degrees.
        /// </summary>
        /// <param name="radians">Angle in Radians.</param>
        /// <returns>
        /// Angle in Degrees.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double RadiansToDegrees(this double radians) => radians * Degree;
        #endregion

        #region Cast Conversion
        /// <summary>
        /// Converts <see cref="ValueTuple" /> to <see cref="PointF" />.
        /// </summary>
        /// <param name="tuple">The tuple.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointF ToPointF(this (double X, double Y) tuple) => new((float)tuple.X, (float)tuple.Y);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ConicSectionType IdentifyConicSectionType(double a, double b, double c, double d, double e, double f)
        {
            _ = d;
            _ = e;
            const double epsilon = 0.00001d;

            // Calculate the determinant.
            var determinant = (b * b) - (4d * a * c);

            if (Abs(determinant) < epsilon)
            {
                return (a == 0 && b == 0 && c == 0) ? ConicSectionType.Line : ConicSectionType.Parabola;
            }
            else if (determinant < 0d)
            {
                return f == 0 ? ConicSectionType.Point : (Abs(a) < epsilon) && (Abs(b) < epsilon) ? ConicSectionType.Circle : ConicSectionType.Ellipse;
            }
            else
            {
                return Abs(a + c) < epsilon ? Sign(a) != Sign(b) ? ConicSectionType.CrossingLines : ConicSectionType.RectangularHyperbola : ConicSectionType.Hyperbola;
            }
        }

        #region Conic Section
        /// <summary>
        /// Point to conic section.
        /// </summary>
        /// <param name="h">The h.</param>
        /// <param name="k">The k.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ConicSection PointToUnitConicSection(double h, double k) => new(
                a: 1d,
                b: 1d,
                c: 0d, -2d * h,
                e: -2d * k,
                f: (h * h) + (k * k) - 1d);

        /// <summary>
        /// Lines to conic section.
        /// </summary>
        /// <param name="x">a x.</param>
        /// <param name="y">a y.</param>
        /// <param name="i">The b x.</param>
        /// <param name="j">The b y.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ConicSection LineToUnitConicSection(double x, double y, double i, double j)
        {
            var dX = i;// - x;
            var dY = j;// - y;
            return new ConicSection(
                a: (x * x) + (2d * x * dX) + (dX * dX),
                b: (x * y) + (x * dX) + (y * dX) + (dX * dY),
                c: (y * y) + (2d * y * dY) + (dY * dY),
                d: x + dX,
                e: y + dY,
                f: 0d);
        }

        /// <summary>
        /// Line Segments to conic section.
        /// </summary>
        /// <param name="x1">a x.</param>
        /// <param name="y1">a y.</param>
        /// <param name="x2">The b x.</param>
        /// <param name="y2">The b y.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ConicSection LineSegmentToUnitConicSection(double x1, double y1, double x2, double y2)
        {
            var dX = x2 - x1;
            var dY = y2 - y1;
            return new ConicSection(
                a: (x1 * x1) + (2d * x1 * dX) + (dX * dX),
                b: (x1 * y1) + (x1 * dX) + (y1 * dX) + (dX * dY),
                c: (y1 * y1) + (2d * y1 * dY) + (dY * dY),
                d: x1 + dX,
                e: y1 + dY,
                f: 0d);
        }

        /// <summary>
        /// Parabolas to conic section.
        /// </summary>
        /// <param name="tuple">The tuple.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ConicSection VertexParabolaToUnitConicSection((double a, double h, double k) tuple) => ParabolaToConicSection(tuple.a, tuple.h, tuple.k);

        /// <summary>
        /// Convert a parabola from vertex form into standard form.
        /// </summary>
        /// <param name="a">The <paramref name="a" /> component of the parabola.</param>
        /// <param name="h">The horizontal component of the parabola vertex.</param>
        /// <param name="k">The vertical component of the parabola vertex.</param>
        /// <returns>
        /// Returns  <see cref="ValueTuple{T1, T2, T3}" /> representing the a, b, and c values of the standard form of a parabola.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ConicSection ParabolaToConicSection(double a, double h, double k)
        {
            // ToDo: Fix. This does not work as expected.
            var b = -2d * a * h;
            return new ConicSection(b, 0d, a, b * b / (4d * a), k, 1d);
        }

        /// <summary>
        /// Parabolas to conic section.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="h">The h.</param>
        /// <param name="k">The k.</param>
        /// <param name="i">The i.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ConicSection ParabolaToConicSection(double a, double h, double k, double i)
        {
            // ToDo: Fix. This does not work as expected.
            var cos = Cos(i);
            var sin = Sin(i);
            _ = cos;
            _ = sin;
            var b = -2d * a * h;
            return new ConicSection(a, 0d, b, 0d, (b * b / (4d * a)) + k, 1d);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ConicSection ParabolaToConicSection2(double dX, double dY, double sX, double sY) =>
            // ToDo: Figure out what this is doing.
            new(
                a: 0,
                b: 0,
                c: -sY * sY,
                d: sX,
                e: 2d * sY * dY,
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ConicSection FindUnitConicSectionFromPoints(PointF a, PointF b, PointF c, PointF d, PointF e)
        {
            const int rows = 5;
            const int cols = 5;

            // Build the augmented matrix.
            var points = new[] { a, b, c, d, e };
            var arr = new double[rows, cols + 2];
            for (var row = 0; row < rows; row++)
            {
                arr[row, 0] = points[row].X * points[row].X;
                arr[row, 1] = points[row].X * points[row].Y;
                arr[row, 2] = points[row].Y * points[row].Y;
                arr[row, 3] = points[row].X;
                arr[row, 4] = points[row].Y;
                arr[row, 5] = -1;
                arr[row, 6] = 0;
            }

            // Perform Gaussian elimination.
            for (var r = 0; r < rows - 1; r++)
            {
                // Zero out all entries in column r after this row.
                // See if this row has a non-zero entry in column r.
                if (Abs(arr[r, r]) < double.Epsilon)
                {
                    // Too close to zero. Try to swap with a later row.
                    for (var r2 = r + 1; r2 < rows; r2++)
                    {
                        if (Abs(arr[r2, r]) > double.Epsilon)
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
                if (Abs(arr[r, r]) > double.Epsilon)
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
            if (arr[rows - 1, cols - 1] == 0)
            {
                // We have no solution.
                // See if all of the entries in this row are 0.
                var all_zeros = true;
                for (var j = 0; j <= cols + 1; j++)
                {
                    if (arr[rows - 1, j] != 0)
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
                return new ConicSection(arr[0, cols + 1], arr[1, cols + 1], arr[2, cols + 1], arr[3, cols + 1], arr[4, cols + 1], 1);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ConicSection CircleToUnitConicSection(double r, double h, double k)
        {
            if (r == 0)
            {
                return PointToUnitConicSection(h, k);
            }

            h += r;
            k += r;

            var r2 = r * r;

            return new ConicSection(
                a: 1d / r2,
                b: 1d / r2,
                c: 0d, -2d * h / r2,
                e: -2d * k / r2,
                f: (h * h / r2) + (k * k / r2) - 1d);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ConicSection OrthogonalEllipseToUnitConicSection(double a, double b, double h, double k)
        {
            if (a == b)
            {
                return CircleToUnitConicSection(a, h, k);
            }

            // Shift over to the center.
            h += a;
            k += b;

            var coefA = 1d / (a * a);
            var coefB = 0d;
            var coefC = 1d / (b * b);
            const double scale = 1d;
            return new ConicSection(
                a: coefA,
                b: coefB,
                c: coefC,
                d: -2d * h * coefA,
                e: -2d * k * coefC,
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ConicSection EllipseToUnitConicSection(double a, double b, double h, double k, double angle) => EllipseToUnitConicSection(a, b, h, k, Cos(angle), Sin(angle));

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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ConicSection EllipseToUnitConicSection(double a, double b, double h, double k, double cos, double sin)
        {
            if (a == b)
            {
                return OrthogonalEllipseToUnitConicSection(a, b, h, k);
            }

            // Fix imprecise handling of Cos(PI/2) which breaks ellipses at right angles to each other.
            if (sin == 1d || sin == -1d)
            {
                cos = 0d;
            }

            // Shift over to the center.
            h += a;
            k += b;

            var coefA = (cos * cos / (a * a)) + (sin * sin / (b * b));
            var coefB = 2d * sin * cos * ((1d / (a * a)) - (1d / (b * b)));
            var coefC = (sin * sin / (a * a)) + (cos * cos / (b * b));
            const double scale = 1d;
            return new ConicSection(
                a: coefA,
                b: coefB,
                c: coefC,
                d: (-2d * h * coefA) - (k * coefB),
                e: (-2d * k * coefC) - (h * coefB),
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ConicSection EllipseToConicSection(double h, double k, double a, double b, double angle) => EllipseToConicSection(h, k, a, b, Cos(angle), Sin(angle));

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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ConicSection EllipseToConicSection(double h, double k, double a, double b, double cos, double sin)
        {
            // Fix imprecise handling of Cos(PI/2) which breaks ellipses at right angles to each other.
            if (sin == 1d || sin == -1d)
            {
                cos = 0d;
            }

            var coefA = (cos * cos * (a * a)) + (sin * sin * (b * b));
            var coefB = 2d * sin * cos * ((b * b) - (a * a));
            var coefC = (sin * sin * (a * a)) + (cos * cos * (b * b));
            return new ConicSection(
                a: coefA,
                b: coefB,
                c: coefC,
                d: (-2d * h * coefA) - (k * coefB),
                e: (-2d * k * coefC) - (h * coefB),
                f: (h * h * coefA) + (h * k * coefB) + (k * k * coefC) - ((a * a) * (b * b)));
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ConicSection HyperbolaToConicSection(double a, double b, double h, double k, double angle) => HyperbolaToConicSection(a, b, h, k, Cos(angle), Sin(angle));

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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ConicSection HyperbolaToConicSection(double rX, double rY, double h, double k, double cos, double sin)
        {
            _ = rX;
            _ = rY;
            _ = h;
            _ = k;
            _ = cos;
            // Fix imprecise handling of Cos(PI/2) which breaks ellipses at right angles to each other.
            if (sin == 1d || sin == -1d)
            {
                cos = 0d;
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ConicSection RescaleConicSection(ConicSection conicSection)
        {
            var min = SexticAbsMin(conicSection.A, conicSection.B, conicSection.C, conicSection.D, conicSection.E, conicSection.F);
            var scale = 1d / min;
            return new ConicSection(conicSection.A *= scale, conicSection.B *= scale, conicSection.C *= scale, conicSection.D *= scale, conicSection.E *= scale, conicSection.F *= scale);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe double[,] ConicSectionToMatrix(double a, double b, double c, double d, double e, double f) => new  double[3,3] { { a, b * 0.5d, d * 0.5d }, { b * 0.5d, c, e * 0.5d }, { d * 0.5d, e * 0.5d, f } };

        /// <summary>
        /// Converts a matrix form of a Conic Section to Standard Conic Section form.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (double a, double b, double c, double d, double e, double f) MatrixToConicSection(double[,] matrix)
        {
            if (matrix.GetLength(0) != 3 || matrix.GetLength(1) != 3) throw new Exception();
            return (matrix[0, 0], matrix[0, 1] * 2d, matrix[1, 1], matrix[0, 2] * 2d, matrix[1, 2] * 2, matrix[2, 2]);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (double a, double b, double c, double d, double e, double f) RescaleSextic(double a, double b, double c, double d, double e, double f)
        {
            var min = SexticAbsMin(a, b, c, d, e, f);
            var scale = 1d / min;
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double SexticAbsMin(double a, double b, double c, double d, double e, double f)
        {
            var min = Abs(a);
            min = Min(min, Abs(b));
            min = Min(min, Abs(c));
            min = Min(min, Abs(d));
            min = Min(min, Abs(e));
            min = Min(min, Abs(f));
            return min;
        }

        /// <summary>
        /// Standards the parabola to vertex parabola.
        /// </summary>
        /// <param name="tuple">The tuple.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (double a, double h, double k) StandardParabolaToVertexParabola((double a, double b, double c) tuple) => StandardParabolaToVertexParabola(tuple.a, tuple.b, tuple.c);

        /// <summary>
        /// Convert a parabola from standard form into vertex form.
        /// </summary>
        /// <param name="a">The <paramref name="a" /> component of the parabola.</param>
        /// <param name="b">The <paramref name="b" /> component of the parabola.</param>
        /// <param name="c">The <paramref name="c" /> component of the parabola.</param>
        /// <returns>
        /// Returns  <see cref="ValueTuple{T1, T2, T3}" /> representing the a, h, and k values of the vertex form of a parabola.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (double a, double h, double k) StandardParabolaToVertexParabola(double a, double b, double c) => (a, h: -(b / (2d * a)), k: -(b * b / (4d * a)) + c);

        /// <summary>
        /// Vertexes the parabola to standard parabola.
        /// </summary>
        /// <param name="tuple">The tuple.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (double a, double b, double c) VertexParabolaToStandardParabola((double a, double h, double k) tuple) => VertexParabolaToStandardParabola(tuple.a, tuple.h, tuple.k);

        /// <summary>
        /// Vertexes the parabola to standard parabola.
        /// </summary>
        /// <param name="tuple">The tuple.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (double a, double b, double c, double i) VertexParabolaToStandardParabola((double a, double h, double k, double i) tuple) => VertexParabolaToStandardParabola(tuple.a, tuple.h, tuple.k, tuple.i);

        /// <summary>
        /// Convert a parabola from vertex form into standard form.
        /// </summary>
        /// <param name="a">The <paramref name="a" /> component of the parabola.</param>
        /// <param name="h">The horizontal component of the parabola vertex.</param>
        /// <param name="k">The vertical component of the parabola vertex.</param>
        /// <returns>
        /// Returns  <see cref="ValueTuple{T1, T2, T3}" /> representing the a, b, and c values of the standard form of a parabola.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (double a, double b, double c) VertexParabolaToStandardParabola(double a, double h, double k)
        {
            var b = -2d * a * h;
            return (a, b, c: (b * b / (4d * a)) + k);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (double a, double b, double c, double i) VertexParabolaToStandardParabola(double a, double h, double k, double i)
        {
            var b = -2d * a * h;
            return (a, b, c: (b * b / (4d * a)) + k, i);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (double ax, double ay, double bx, double by, double cx, double cy) StandardParabolaToQuadraticBezier(double a, double b, double c, double x1, double x2)
        {
            // Get the vertical components of the end points.
            var y1 = (a * x1 * x1) + (x1 * b) + c;
            var y2 = (a * x2 * x2) + (x2 * b) + c;
            // Find the intersection of the tangents at the end nodes to find the center node.
            var cx = (x2 + x1) * 0.5d;
            var cy = (a * ((x2 * x1) - (x1 * x1))) + (b * (x2 - x1) * 0.5d) + y1;
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (double ax, double ay, double bx, double by, double cx, double cy) StandardParabolaToQuadraticBezier(double a, double b, double c, double angle, double x1, double y1, double x2, double y2) => StandardParabolaToQuadraticBezier(a, b, c, Cos(angle), Sin(angle), x1, y1, x2, y2);

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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (double ax, double ay, double bx, double by, double cx, double cy) StandardParabolaToQuadraticBezier(double a, double b, double c, double cos, double sin, double left, double top, double right, double bottom)
        {
            // Apply a rotation transformation to the x values.
            var lx = (left * cos) - (top * sin);
            var rx = (right * cos) - (bottom * sin);

            // Get the vertical components of the end points.
            var ly = (a * lx * lx) + (lx * b) + c;
            var ry = (a * rx * rx) + (rx * b) + c;

            // Find the intersection of the tangents at the end nodes to find the center node.
            var cx = (rx + lx) * 0.5d;
            var cy = (a * ((rx * lx) - (lx * lx))) + (b * (rx - lx) * 0.5d) + ly;
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (double ax, double ay, double bx, double by, double cx, double cy) VertexParabolaToQuadraticBezier(double a, double h, double k, double left, double right)
        {
            // Get the vertical components of the end points.
            var y1 = (a * ((h * h) + (-2d * h * left) + (left * left))) + k;
            var y2 = (a * ((h * h) + (-2d * h * right) + (right * right))) + k;

            // Find the intersection of the tangents at the end nodes to find the center node.
            var cx = (right + left) * 0.5;
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (double ax, double ay, double bx, double by, double cx, double cy) VertexParabolaToQuadraticBezier(double a, double h, double k, double angle, double left, double top, double right, double bottom) => VertexParabolaToQuadraticBezier(a, h, k, Cos(angle), Sin(angle), left, top, right, bottom);

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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (double ax, double ay, double bx, double by, double cx, double cy) VertexParabolaToQuadraticBezier(double a, double h, double k, double cos, double sin, double left, double top, double right, double bottom)
        {
            // ToDo: Figure out why rotation isn't working, and the Bezier is getting cut off weird with certain rectangles.

            // ToDo: Figure out how to actually apply the rotation. This currently just changes the start and end points

            // Apply a rotation transformation to the x values.
            var lx = (left * cos) - (bottom * sin); // Usually 0;
            var rx = (right * cos) - (top * sin);

            // Get the vertical components of the end points.
            var y1 = (a * (lx - h) * (lx - h)) + k;
            var y2 = (a * (rx - h) * (rx - h)) + k;

            // Find the intersection of the tangents at the end nodes to find the center node.
            var cx = (rx + lx) * 0.5;
            var cy = (a * ((h * lx) + (lx * rx) - (h * rx) - (lx * lx))) + y1;
            return (lx, y1, cx, cy, rx, y2);
        }

        /// <summary>
        /// Quadratic bezier to cubic bezier.
        /// </summary>
        /// <param name="tuple">The tuple.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (double aX, double aY, double bX, double bY, double cX, double cY, double dX, double dY) QuadraticBezierToCubicBezier((double aX, double aY, double bX, double bY, double cX, double cY) tuple) => QuadraticBezierToCubicBezier(tuple.aX, tuple.aY, tuple.bX, tuple.bY, tuple.cX, tuple.cY);

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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (double aX, double aY, double bX, double bY, double cX, double cY, double dX, double dY) QuadraticBezierToCubicBezier(double aX, double aY, double bX, double bY, double cX, double cY) => (aX, aY, aX + (TwoThirds * (bX - aX)), aY + (TwoThirds * (bY - aY)), cX + (TwoThirds * (bX - cX)), cY + (TwoThirds * (bY - cY)), cX, cY);
    }
}
