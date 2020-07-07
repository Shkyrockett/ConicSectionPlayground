// <copyright file="Interpolation.cs">
//     Copyright © 2019 - 2020 Shkyrockett. All rights reserved.
// </copyright>
// <author id="shkyrockett">Shkyrockett</author>
// <license>
//     Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </license>
// <summary></summary>
// <remarks></remarks>

using System;
using System.Runtime.CompilerServices;
using static ConicSectionPlayground.Mathematics;
using static System.Math;

namespace ConicSectionPlayground
{
    /// <summary>
    /// 
    /// </summary>
    public static class Interpolation
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ConicSectionUnitDifferenceCarteseanY(double y,
            double a1, double b1, double c1, double d1, double e1, double f1, int sign1,
            double a2, double b2, double c2, double d2, double e2, double f2, int sign2)
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double UnitConicSectionCarteseanY(double y, double a, double b, double c, double d, double e, double f, int rootSign)
        {
            var result = (b * y) + e;
            result *= result;
            result -= 4d * c * ((a * (y * y)) + (d * y) + f);
            result = rootSign * Sqrt(result);
            result = -((b * y) + e) + result;
            result = result / 2d / c;
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (double X, double Y) StandardParabola(double t, double a, double b, double c) => (t, (a * (t * t)) + ((b * t) + c)); // Equation for finding the y of a parabola in standard form.

        /// <summary>
        /// Interpolates the parabola vertex.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <param name="a">a.</param>
        /// <param name="h">The h.</param>
        /// <param name="k">The k.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (double X, double Y) VertexParabola(double t, double a, double h, double k) => (t, (a * (t - h) * (t - h)) + k); // Equation for finding the y of a parabola in vertex form.

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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (double X, double Y) Ellipse(double t, double cX, double cY, double r1, double r2, double angle) => Ellipse(Cos(t), Sin(t), cX, cY, r1, r2, Cos(angle), Sin(angle));

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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (double X, double Y) Ellipse(double t, double cX, double cY, double r1, double r2, double cosAngle, double sinAngle) => Ellipse(Cos(t), Sin(t), cX, cY, r1, r2, cosAngle, sinAngle);

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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (double X, double Y) Ellipse(double cosTheta, double sinTheta, double cX, double cY, double r1, double r2, double cosAngle, double sinAngle)
        {
            // Ellipse equation for an ellipse at origin.
            var i = r1 * cosTheta;
            var j = -(r2 * sinTheta);

            // Apply the rotation transformation and translate to new center.
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Span<(double X, double Y)> CircleExtremePoints(double x, double y, double r)
        {
            Span<(double X, double Y)> points = stackalloc (double X, double Y)[4]
            {
                (X: x, Y: y - r),
                (X: x - r, Y: y),
                (X: x, Y: y + r),
                (X: x + r, Y: y),
            };
            return points.ToArray();
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<(double X, double Y)> OrthogonalEllipseExtremePoints(double x, double y, double rX, double rY)
        {
            if (rX == rY)
            {
                return CircleExtremePoints(x, y, rX);
            }

            Span<(double X, double Y)> points = stackalloc (double X, double Y)[4]
            {
                (X: x, Y: y - rY),
                (X: x - rX, Y: y),
                (X: x, Y: y + rY),
                (X: x + rX, Y: y),
            };
            return points.ToArray();
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<(double X, double Y)> EllipseExtremePoints(double x, double y, double rX, double rY, double angle) => EllipseExtremePoints(x, y, rX, rY, Cos(angle), Sin(angle));

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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<(double X, double Y)> EllipseExtremePoints(double x, double y, double rX, double rY, double cosAngle, double sinAngle)
        {
            if (rX == rY)
            {
                return CircleExtremePoints(x, y, rX);
            }

            if (cosAngle == Cos0 && sinAngle == Sin0)
            {
                return OrthogonalEllipseExtremePoints(x, y, rX, rY);
            }

            // Fix imprecise handling of Cos(PI/2) which breaks ellipses at right angles to each other.
            if (sinAngle == 1d || sinAngle == -1d)
            {
                cosAngle = 0d;
            }

            // Calculate the radii of the angle of rotation.
            var a = rX * cosAngle;
            var b = rY * sinAngle;
            var c = rX * sinAngle;
            var d = rY * cosAngle;

            // Find the angles of the Cartesian extremes.
            var a1 = Atan2(-b, a);
            var a2 = Atan2(b, -a); // + PI; // sin(t + pi) = -sin(t); cos(t + pi)=-cos(t)
            var a3 = Atan2(d, c);
            var a4 = Atan2(-d, -c); // + PI; // sin(t + pi) = -sin(t); cos(t + pi)=-cos(t)

            // Return the points of Cartesian extreme of the rotated ellipse.
            Span<(double X, double Y)> points = stackalloc (double X, double Y)[4]
            {
                Ellipse(a1, x, y, rX, rY, cosAngle, sinAngle),
                Ellipse(a2, x, y, rX, rY, cosAngle, sinAngle),
                Ellipse(a3, x, y, rX, rY, cosAngle, sinAngle),
                Ellipse(a4, x, y, rX, rY, cosAngle, sinAngle)
            };
            return points.ToArray();
        }
        #endregion
    }
}
