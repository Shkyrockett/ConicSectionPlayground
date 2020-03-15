// <copyright file="Mathematics.cs">
//     Copyright © 2019 - 2020 Shkyrockett. All rights reserved.
// </copyright>
// <author id="shkyrockett">Shkyrockett</author>
// <license>
//     Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </license>
// <summary></summary>
// <remarks></remarks>

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;
using static ConicSectionPlayground.Interpolation;
using static System.Math;

namespace ConicSectionPlayground
{
    /// <summary>
    /// 
    /// </summary>
    public static class Mathematics
    {
        #region Constants
        /// <summary>
        /// A value close to 0.
        /// </summary>
        public const double small = 0.1d;

        /// <summary>
        /// The scale per mouse wheel delta.
        /// </summary>
        public const float scale_per_delta = 0.1f / 120f;

        /// <summary>
        /// The one half Value: 1d * 0.5d.
        /// </summary>
        public const double OneHalf = 0.5d;

        /// <summary>
        /// The two thirds Value: 2d / 3d.
        /// </summary>
        public const double TwoThirds = 2d / 3d;

        /// <summary>
        /// One Radian.
        /// </summary>
        /// <remarks><para>PI / 180</para></remarks>
        public const double Radian = PI / 180d; // 0.01745329251994329576923690768489d;

        /// <summary>
        /// One degree.
        /// </summary>
        /// <remarks><para>180 / PI</para></remarks>
        public const double Degree = 180d / PI; // 57.295779513082320876798154814105d;

        /// <summary>
        /// The cosine of 0.
        /// </summary>
        public static readonly double Cos0 = Cos(0d);

        /// <summary>
        /// The sine of 0.
        /// </summary>
        public static readonly double Sin0 = Sin(0d);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNumber(this float number) => !(float.IsNaN(number) || float.IsInfinity(number));

        /// <summary>
        /// Determines whether the specified number is number.
        /// Return true if the number is not infinity or NaN.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns>
        ///   <c>true</c> if the specified number is number; otherwise, <c>false</c>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNumber(this double number) => !(double.IsNaN(number) || double.IsInfinity(number));
        #endregion

        #region Min Point
        /// <summary>
        /// The min point.
        /// </summary>
        /// <param name="point1X">The point1X.</param>
        /// <param name="point1Y">The point1Y.</param>
        /// <param name="point2X">The point2X.</param>
        /// <param name="point2Y">The point2Y.</param>
        /// <returns>The <see cref="ValueTuple{T1, T2}"/>.</returns>
        /// <acknowledgment>
        /// http://www.kevlindev.com/
        /// </acknowledgment>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (double X, double Y) MinPoint(double point1X, double point1Y, double point2X, double point2Y) => (Min(point1X, point2X), Min(point1Y, point2Y));
        #endregion Min Point

        #region Max Point
        /// <summary>
        /// The max point.
        /// </summary>
        /// <param name="point1X">The point1X.</param>
        /// <param name="point1Y">The point1Y.</param>
        /// <param name="point2X">The point2X.</param>
        /// <param name="point2Y">The point2Y.</param>
        /// <returns>The <see cref="ValueTuple{T1, T2}"/>.</returns>
        /// <acknowledgment>
        /// http://www.kevlindev.com/
        /// </acknowledgment>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (double X, double Y) MaxPoint(double point1X, double point1Y, double point2X, double point2Y) => (Max(point1X, point2X), Max(point1Y, point2Y));
        #endregion Max Point

        /// <summary>
        /// Scales the factor.
        /// </summary>
        /// <param name="scale">The scale.</param>
        /// <param name="delta">The delta.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double MouseWheelScaleFactor(double scale, int delta)
        {
            scale += delta * scale_per_delta;
            return (scale <= 0) ? 2f * float.Epsilon : scale;
        }

        /// <summary>
        /// Screens to object transposed matrix.
        /// </summary>
        /// <param name="startingPoint">The starting point.</param>
        /// <param name="Location">The location.</param>
        /// <param name="scale">The scale.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointF ScreenToObjectTransposedMatrix(PointF startingPoint, PointF Location, double scale) => new PointF((float)(Location.X / scale) - startingPoint.X, (float)(Location.Y / scale) - startingPoint.Y);

        /// <summary>
        /// Screens to object transposed matrix.
        /// </summary>
        /// <param name="startingPoint">The starting point.</param>
        /// <param name="Location">The location.</param>
        /// <param name="scale">The scale.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointF ScreenToObjectTransposedMatrix(PointF startingPoint, PointF Location, Vector2 scale) => new PointF((Location.X / scale.X) - startingPoint.X, (Location.Y / scale.Y) - startingPoint.Y);

        /// <summary>
        /// Scales the specified scale.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="scale">The scale.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointF ObjectToScreen(PointF point, float scale) => new PointF(point.X * scale, point.Y * scale);

        /// <summary>
        /// Scales the specified scale.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="scale">The scale.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointF ObjectToScreen(Point point, float scale) => new PointF(point.X * scale, point.Y * scale);

        /// <summary>
        /// Objects to screen.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <param name="point">The point.</param>
        /// <param name="scale">The scale.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointF ObjectToScreen(PointF offset, PointF point, float scale) => new PointF((offset.X * scale) - point.X, (offset.Y * scale) - point.Y);

        ///// <summary>
        ///// Zooms to region.
        ///// </summary>
        ///// <param name="rectangle">The rectangle.</param>
        ///// <acknowledgment>
        ///// https://www.cyotek.com/blog/zooming-to-fit-a-region-in-a-scrollablecontrol
        ///// </acknowledgment>
        //public virtual void ZoomToRegion(RectangleF rectangle)
        //{
        //    double ratioX;
        //    double ratioY;
        //    int cx;
        //    int cy;

        //    ratioX = this.ClientSize.Width / rectangle.Width;
        //    ratioY = this.ClientSize.Height / rectangle.Height;
        //    cx = (int)(rectangle.X + (rectangle.Width / 2));
        //    cy = (int)(rectangle.Y + (rectangle.Height / 2));

        //    this.Zoom = (int)(Math.Min(ratioX, ratioY) * 100);
        //    this.CenterAt(new Point(cx, cy));
        //}

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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<PointF> FindRootsUsingBinaryDivision(double xmin, double xmax,
            double a1, double b1, double c1, double d1, double e1, double f1, int sign1,
            double a2, double b2, double c2, double d2, double e2, double f2, int sign2)
        {
            var roots = new List<PointF>();
            const int num_tests = 100;
            var delta_x = (xmax - xmin) / (num_tests - 1);

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
                    foreach (var pt in roots)
                    {
                        if (Abs(pt.X - x) < small)
                        {
                            is_new = false;
                            break;
                        }
                    }

                    // If this is a new point, save it.
                    if (is_new)
                    {
                        roots.Add(new PointF((float)x, (float)y));

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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static (double x, double y) UseBinaryDivision(double x0, double delta_x,
            double a1, double b1, double c1, double d1, double e1, double f1, int sign1,
            double a2, double b2, double c2, double d2, double e2, double f2, int sign2)
        {
            const int num_trials = 200;
            const int sgn_nan = -2;

            // Get G(x) for the bounds.
            var xmin = x0;
            var g_xmin = ConicSectionUnitDifferenceCarteseanY(xmin,
                a1, b1, c1, d1, e1, f1, sign1,
                a2, b2, c2, d2, e2, f2, sign2);
            if (Abs(g_xmin) < small)
            {
                return (xmin, g_xmin);
            }

            var xmax = xmin + delta_x;
            var g_xmax = ConicSectionUnitDifferenceCarteseanY(xmax,
                a1, b1, c1, d1, e1, f1, sign1,
                a2, b2, c2, d2, e2, f2, sign2);
            if (Abs(g_xmax) < small)
            {
                return (xmax, g_xmax);
            }

            // Get the sign of the values.
            int sgn_min, sgn_max;
            if (IsNumber(g_xmin))
            {
                sgn_min = Sign(g_xmin);
            }
            else
            {
                sgn_min = sgn_nan;
            }

            if (IsNumber(g_xmax))
            {
                sgn_max = Sign(g_xmax);
            }
            else
            {
                sgn_max = sgn_nan;
            }

            // If the two values have the same sign,
            // then there is no root here.
            if (sgn_min == sgn_max)
            {
                return (1, float.NaN);
            }

            // Use binary division to find the point of intersection.
            var xmid = 0d;
            var g_xmid = 0d;
            for (var i = 0; i < num_trials; i++)
            {
                // Get values for the midpoint.
                xmid = (xmin + xmax) / 2d;
                g_xmid = ConicSectionUnitDifferenceCarteseanY(xmid,
                    a1, b1, c1, d1, e1, f1, sign1,
                    a2, b2, c2, d2, e2, f2, sign2);
                int sgn_mid;
                if (IsNumber(g_xmid))
                {
                    sgn_mid = Sign(g_xmid);
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

            if (IsNumber(g_xmid) && (Abs(g_xmid) < small))
            {
                return (xmid, g_xmid);
            }
            else if (IsNumber(g_xmin) && (Abs(g_xmin) < small))
            {
                return (xmin, g_xmin);
            }
            else if (IsNumber(g_xmax) && (Abs(g_xmax) < small))
            {
                return (xmax, g_xmax);
            }
            else
            {
                return (xmid, float.NaN);
            }
        }
    }
}
