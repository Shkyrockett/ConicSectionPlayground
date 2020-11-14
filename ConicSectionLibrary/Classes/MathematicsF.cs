// <copyright file="MathematicsF.cs">
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
using static ConicSectionLibrary.InterpolationF;
using static System.MathF;

namespace ConicSectionLibrary
{
    /// <summary>
    /// 
    /// </summary>
    public static class MathematicsF
    {
        #region Constants
        /// <summary>
        /// The one third constant.
        /// </summary>
        public const float OneThird = 1f / 3f;

        /// <summary>
        /// The one half constant.
        /// </summary>
        public const float OneHalf = 1f / 2f;

        /// <summary>
        /// The two thirds constant.
        /// </summary>
        public const float TwoThirds = 2f / 3f;

        /// <summary>
        /// One Radian.
        /// </summary>
        /// <remarks>
        /// PI / 180
        /// </remarks>
        public const float Radian = PI / 180f; // 0.01745329251994329576923690768489d;

        /// <summary>
        /// One degree.
        /// </summary>
        /// <remarks>
        /// 180 / PI
        /// </remarks>
        public const float Degree = 180f / PI; // 57.295779513082320876798154814105d;

        /// <summary>
        /// The cosine of 0.
        /// </summary>
        public static readonly float Cos0 = Cos(0f);

        /// <summary>
        /// The sine of 0.
        /// </summary>
        public static readonly float Sin0 = Sin(0f);

        /// <summary>
        /// A value close to 0.
        /// </summary>
        public const float small = 0.1f;

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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNumber(this float number) => !(float.IsNaN(number) || float.IsInfinity(number));
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (float X, float Y) MinPoint(float point1X, float point1Y, float point2X, float point2Y) => (Min(point1X, point2X), Min(point1Y, point2Y));
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (float X, float Y) MaxPoint(float point1X, float point1Y, float point2X, float point2Y) => (Max(point1X, point2X), Max(point1Y, point2Y));
        #endregion Max Point

        /// <summary>
        /// Scales the factor.
        /// </summary>
        /// <param name="scale">The scale.</param>
        /// <param name="delta">The delta.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float MouseWheelScaleFactor(float scale, int delta)
        {
            scale += delta * scale_per_delta;
            return (scale <= 0) ? 2f * float.Epsilon : scale;
        }

        #region Point Manipulation Methods
        /// <summary>
        /// Inverses the scale point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="scale">The scale.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointF ScreenToObject_(PointF point, float scale)
        {
            var invScale = 1f / scale;
            return new PointF(invScale * point.X, invScale * point.Y);
        }

        /// <summary>
        /// Screens to object.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="scale">The scale.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointF ScreenToObject(PointF point, float scale) => new(point.X / scale, point.Y / scale);

        /// <summary>
        /// Inverses the translation and scale of a point.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <param name="point">The point.</param>
        /// <param name="scale">The scale.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointF ScreenToObject_(PointF offset, PointF point, float scale)
        {
            var invScale = 1f / scale;
            return new PointF((point.X - offset.X) * invScale, (point.Y - offset.Y) * invScale);
        }

        /// <summary>
        /// Screens to object. https://stackoverflow.com/a/37269366
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <param name="point">The screen point.</param>
        /// <param name="scale">The scale.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointF ScreenToObject(PointF offset, PointF point, float scale) => new((point.X - offset.X) / scale, (point.Y - offset.Y) / scale);

        /// <summary>
        /// Screens to object transposed matrix.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <param name="screenPoint">The screen point.</param>
        /// <param name="scale">The scale.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointF ScreenToObjectTransposedMatrix_(PointF offset, PointF screenPoint, float scale)
        {
            var invScale = 1f / scale;
            return new PointF((screenPoint.X * invScale) - offset.X, (screenPoint.Y * invScale) - offset.Y);
        }

        /// <summary>
        /// Screens to object transposed matrix. https://stackoverflow.com/a/37269366
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <param name="screenPoint">The screen point.</param>
        /// <param name="scale">The scale.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointF ScreenToObjectTransposedMatrix(PointF offset, PointF screenPoint, float scale) => new((screenPoint.X / scale) - offset.X, (screenPoint.Y / scale) - offset.Y);

        /// <summary>
        /// Screens to object transposed matrix.
        /// </summary>
        /// <param name="offset">The starting point.</param>
        /// <param name="screenPoint">The location.</param>
        /// <param name="scale">The scale.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointF ScreenToObjectTransposedMatrix(PointF offset, PointF screenPoint, Vector2 scale) => new((screenPoint.X / scale.X) - offset.X, (screenPoint.Y / scale.Y) - offset.Y);

        /// <summary>
        /// Objects to screen.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="scale">The scale.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointF ObjectToScreen(PointF point, float scale) => new(point.X * scale, point.Y * scale);

        /// <summary>
        /// Scales the specified scale.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="scale">The scale.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointF ObjectToScreen(Point point, float scale) => new(point.X * scale, point.Y * scale);

        /// <summary>
        /// Objects to screen. https://stackoverflow.com/a/37269366
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <param name="point">The object point.</param>
        /// <param name="scale">The scale.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointF ObjectToScreen(PointF offset, PointF point, float scale) => new(offset.X + (point.X * scale), offset.Y + (point.Y * scale));

        /// <summary>
        /// Objects to screen transposed matrix. https://stackoverflow.com/a/37269366
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <param name="objectPoint">The object point.</param>
        /// <param name="scale">The scale.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointF ObjectToScreenTransposedMatrix(PointF offset, PointF objectPoint, float scale) => new((offset.X + objectPoint.X) * scale, (offset.Y + objectPoint.Y) * scale);

        /// <summary>
        /// Zooms at. https://stackoverflow.com/a/37269366
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <param name="cursor">The cursor.</param>
        /// <param name="previousScale">The previous scale.</param>
        /// <param name="scale">The scale.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointF ZoomAt(PointF offset, PointF cursor, float previousScale, float scale)
        {
            var point = ScreenToObject(offset, cursor, previousScale);
            point = ObjectToScreen(offset, point, scale);
            return new PointF(offset.X + ((cursor.X - point.X) / scale), offset.Y + ((cursor.Y - point.Y) / scale));
        }

        /// <summary>
        /// Zooms at for a transposed matrix. https://stackoverflow.com/a/37269366
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <param name="cursor">The cursor.</param>
        /// <param name="previousScale">The previous scale.</param>
        /// <param name="scale">The scale.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointF ZoomAtTransposedMatrix(PointF offset, PointF cursor, float previousScale, float scale)
        {
            var point = ScreenToObjectTransposedMatrix(offset, cursor, previousScale);
            point = ObjectToScreenTransposedMatrix(offset, point, scale);
            return new PointF(offset.X + ((cursor.X - point.X) / scale), offset.Y + ((cursor.Y - point.Y) / scale));
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<PointF> FindRootsUsingBinaryDivision(float xmin, float xmax,
            float a1, float b1, float c1, float d1, float e1, float f1, int sign1,
            float a2, float b2, float c2, float d2, float e2, float f2, int sign2)
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
        private static (float x, float y) UseBinaryDivision(float x0, float delta_x,
            float a1, float b1, float c1, float d1, float e1, float f1, int sign1,
            float a2, float b2, float c2, float d2, float e2, float f2, int sign2)
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
            var xmid = 0f;
            var g_xmid = 0f;
            for (var i = 0; i < num_trials; i++)
            {
                // Get values for the midpoint.
                xmid = (xmin + xmax) / 2f;
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
