// <copyright file="IntersectionsD.cs">
//     Copyright © 2019 - 2022 Shkyrockett. All rights reserved.
// </copyright>
// <author id="shkyrockett">Shkyrockett</author>
// <license>
//     Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </license>
// <summary></summary>
// <remarks></remarks>

using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using static ConicSectionLibrary.Interpolation;
using static ConicSectionLibrary.Mathematics;

namespace ConicSectionLibrary;

/// <summary>
/// 
/// </summary>
public static partial class Intersections
{
    /// <summary>
    /// Find the intersection between a line and a rectangle.
    /// </summary>
    /// <param name="x1">The a1X.</param>
    /// <param name="y1">The a1Y.</param>
    /// <param name="i1">The a2X.</param>
    /// <param name="j1">The a2Y.</param>
    /// <param name="x2">The r1X.</param>
    /// <param name="y2">The r1Y.</param>
    /// <param name="width">The r2X.</param>
    /// <param name="height">The r2Y.</param>
    /// <param name="epsilon">The <paramref name="epsilon" /> or minimal value to represent a change.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static List<(T X, T Y)> LineRectangleIntersection<T>(
        T x1, T y1, T i1, T j1,
        T x2, T y2, T width, T height,
        double epsilon = double.Epsilon) where T : INumber<T> => LinePointRectangleIntersection(x1, y1, i1, j1, x2, y2, width + x2, height + y2, epsilon);

    /// <summary>
    /// Find the intersection point between two line segments.
    /// </summary>
    /// <param name="lx">The lx.</param>
    /// <param name="ly">The ly.</param>
    /// <param name="li">The li.</param>
    /// <param name="lj">The lj.</param>
    /// <param name="s0X">The s0X.</param>
    /// <param name="s0Y">The s0Y.</param>
    /// <param name="s1X">The s1X.</param>
    /// <param name="s1Y">The s1Y.</param>
    /// <param name="epsilon">The <paramref name="epsilon" /> or minimal value to represent a change.</param>
    /// <returns></returns>
    /// <acknowledgment>
    /// http://www.kevlindev.com/
    /// </acknowledgment>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static List<(T X, T Y)> LineLineSegmentIntersection<T>(
        T lx, T ly, T li, T lj,
        T s0X, T s0Y, T s1X, T s1Y,
        double epsilon = double.Epsilon)
        where T : INumber<T>
    {
        T _epsilon = T.CreateChecked(epsilon);

        // Initialize the intersection result.
        var result = new List<(T X, T Y)>();

        // Translate lines to origin.
        (var vi, var vj) = (s1X - s0X, s1Y - s0Y);

        var ua = (vi * (ly - s0Y)) - (vj * (lx - s0X));
        var ub = (li * (ly - s0Y)) - (lj * (lx - s0X));

        // Calculate the determinant of the coefficient matrix.
        var determinant = (vj * li) - (vi * lj);

        // Check if the lines are parallel.
        if (T.Abs(determinant) < _epsilon)
        {
            if (ua == T.Zero || ub == T.Zero)
            {
                // Line segment is coincident to the Line. There are an infinite number of intersections, but we only care about the start and end points of the line segment.
                result.Add((s0X, s0Y));
                result.Add((s1X, s1Y));
                //result.State |= IntersectionStates.Coincident | IntersectionStates.Parallel | IntersectionStates.Intersection;
            }
            else
            {
                // The Line and line segment are parallel. There are no intersections.
                //result.State |= IntersectionStates.Parallel | IntersectionStates.NoIntersection;
            }
        }
        else
        {
            // Find the index where the intersection point lies on the line.
            var ta = ua / determinant;
            var tb = ub / determinant;

            if (tb >= T.Zero && tb <= T.One)
            {
                // One intersection.
                result.Add((lx + (ta * li), ly + (ta * lj)));
                //result.State |= IntersectionStates.Intersection;
            }
            else
            {
                // The intersection is outside of the bounds of the Line segment. We can break early.
                return result;
            }
        }

        return result;
    }

    /// <summary>
    /// Find the intersection between a line and a rectangle.
    /// </summary>
    /// <param name="x1">The a1X.</param>
    /// <param name="y1">The a1Y.</param>
    /// <param name="i1">The a2X.</param>
    /// <param name="j1">The a2Y.</param>
    /// <param name="x2">The r1X.</param>
    /// <param name="y2">The r1Y.</param>
    /// <param name="x3">The r2X.</param>
    /// <param name="y3">The r2Y.</param>
    /// <param name="epsilon">The <paramref name="epsilon" /> or minimal value to represent a change.</param>
    /// <returns></returns>
    /// <acknowledgment>
    /// http://www.kevlindev.com/
    /// </acknowledgment>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static List<(T X, T Y)> LinePointRectangleIntersection<T>(
        T x1, T y1, T i1, T j1,
        T x2, T y2, T x3, T y3,
        double epsilon = double.Epsilon)
        where T : INumber<T>
    {
        //T _epsilon = T.CreateChecked(epsilon);

        var (minX, minY) = MinPoint(x2, y2, x3, y3);
        var (maxX, maxY) = MaxPoint(x2, y2, x3, y3);
        var (topRightX, topRightY) = (maxX, minY);
        var (bottomLeftX, bottomLeftY) = (minX, maxY);

        // ToDo: Need to determine if duplicates are acceptable, or if this attempt at performance boost is going to waste.
        var intersections = new HashSet<(T X, T Y)>();
        intersections.UnionWith(LineLineSegmentIntersection(x1, y1, i1, j1, minX, minY, topRightX, topRightY, epsilon));
        intersections.UnionWith(LineLineSegmentIntersection(x1, y1, i1, j1, topRightX, topRightY, maxX, maxY, epsilon));
        intersections.UnionWith(LineLineSegmentIntersection(x1, y1, i1, j1, maxX, maxY, bottomLeftX, bottomLeftY, epsilon));
        intersections.UnionWith(LineLineSegmentIntersection(x1, y1, i1, j1, bottomLeftX, bottomLeftY, minX, minY, epsilon));

        //var result = new Intersection(IntersectionStates.NoIntersection, intersections);
        //if (result.Count > 0)
        //{
        //    result.State = IntersectionStates.Intersection;
        //}

        return intersections.ToList();
    }

    /// <summary>
    /// Find the points of intersection between
    /// a conic section and a line.
    /// </summary>
    /// <param name="A">a.</param>
    /// <param name="B">The b.</param>
    /// <param name="C">The c.</param>
    /// <param name="D">The d.</param>
    /// <param name="E">The e.</param>
    /// <param name="F">The f.</param>
    /// <param name="x1">The x1.</param>
    /// <param name="y1">The y1.</param>
    /// <param name="x2">The x2.</param>
    /// <param name="y2">The y2.</param>
    /// <returns></returns>
    /// <acknowledgment>
    /// http://csharphelper.com/blog/2014/11/see-where-a-line-intersects-a-conic-section-in-c/
    /// </acknowledgment>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static List<(T X, T Y)> ConicAndLineSegmentIntersection<T>(
        T A, T B, T C, T D, T E, T F,
        T x1, T y1, T x2, T y2)
        where T : IFloatingPointIeee754<T>
    {
        T two = T.CreateChecked(2);
        T four = T.CreateChecked(4);

        // Get dx and dy;
        var dx = x2 - x1;
        var dy = y2 - y1;

        // Calculate the coefficients for the quadratic formula.
        var a = (A * dx * dx) + (B * dx * dy) + (C * dy * dy);
        var b = (A * two * x1 * dx) + (B * x1 * dy) + (B * y1 * dx) + (C * two * y1 * dy) + (D * dx) + (E * dy);
        var c = (A * x1 * x1) + (B * x1 * y1) + (C * y1 * y1) + (D * x1) + (E * y1) + F;

        // Check the determinant to see how many solutions there are.
        var solutions = new List<(T X, T Y)>();
        var determinant = (b * b) - (four * a * c);
        if (determinant == T.Zero)
        {
            var t = -b / (two * a);
            solutions.Add((x1 + (t * dx), y1 + (t * dy)));
        }
        else if (determinant > T.Zero)
        {
            var root = T.Sqrt((b * b) - (four * a * c));
            var t1 = (-b + root) / (two * a);
            solutions.Add((x1 + (t1 * dx), y1 + (t1 * dy)));
            var t2 = (-b - root) / (two * a);
            solutions.Add((x1 + (t2 * dx), y1 + (t2 * dy)));
        }

        return solutions;
    }

    /// <summary>
    /// Finds the points of intersection.
    /// </summary>
    /// <param name="xmin">The xmin.</param>
    /// <param name="xmax">The xmax.</param>
    /// <param name="a1">The a1.</param>
    /// <param name="b1">The b1.</param>
    /// <param name="c1">The c1.</param>
    /// <param name="d1">The d1.</param>
    /// <param name="e1">The e1.</param>
    /// <param name="f1">The f1.</param>
    /// <param name="a2">The a2.</param>
    /// <param name="b2">The b2.</param>
    /// <param name="c2">The c2.</param>
    /// <param name="d2">The d2.</param>
    /// <param name="e2">The e2.</param>
    /// <param name="f2">The f2.</param>
    /// <returns></returns>
    /// <acknowledgment>
    /// http://csharphelper.com/blog/2014/11/see-where-two-conic-sections-intersect-in-c/
    /// </acknowledgment>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static List<(T X, T Y)> ConicSectionAndConicSectionIntersection<T>(T xmin, T xmax,
        T a1, T b1, T c1, T d1, T e1, T f1,
        T a2, T b2, T c2, T d2, T e2, T f2)
        where T : IFloatingPointIeee754<T>
    {
        var Roots = new List<(T X, T Y)>();
        var RootSign1 = new List<int>();
        var RootSign2 = new List<int>();
        T _small = T.CreateChecked(small);

        // Find roots for each of the difference equations.
        int[] signs = { +1, -1 };
        foreach (var sign1 in signs)
        {
            foreach (var sign2 in signs)
            {
                var points = FindRootsUsingBinaryDivision(
                    xmin, xmax,
                    a1, b1, c1, d1, e1, f1, sign1,
                    a2, b2, c2, d2, e2, f2, sign2);
                if (points.Count > 0)
                {
                    Roots.AddRange(points);
                    for (var i = 0; i < points.Count; i++)
                    {
                        RootSign1.Add(sign1);
                        RootSign2.Add(sign2);
                    }
                }
            }
        }

        // Find corresponding points of intersection.
        var pointsOfIntersection = new List<(T X, T Y)>();
        for (var i = 0; i < Roots.Count; i++)
        {
            var y1 = UnitConicSectionCarteseanY(Roots[i].X, a1, b1, c1, d1, e1, f1, RootSign1[i]);
            var y2 = UnitConicSectionCarteseanY(Roots[i].X, a2, b2, c2, d2, e2, f2, RootSign2[i]);
            // Validation.
            Debug.Assert(T.Abs(y1 - y2) < _small);
            pointsOfIntersection.Add((Roots[i].X, y1));
        }

        return pointsOfIntersection;
    }
}
