// <copyright file="WinformsExtentions.cs">
//     Copyright © 2020 - 2022 Shkyrockett. All rights reserved.
// </copyright>
// <author id="shkyrockett">Shkyrockett</author>
// <license>
//     Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </license>
// <summary></summary>
// <remarks></remarks>

using ConicSectionLibrary;
using System.Drawing.Drawing2D;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace ConicSectionPlayground;

/// <summary>
/// The winforms extentions.
/// </summary>
public static class WinformsExtentions
{
    /// <summary>
    /// Draws the point.
    /// </summary>
    /// <param name="point">The point.</param>
    /// <param name="gr">The gr.</param>
    /// <param name="pen">The pen.</param>
    /// <param name="offset">The offset.</param>
    /// <param name="scale">The scale.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void DrawPoint<T>((T X, T Y) point, Graphics gr, Pen pen, PointF offset, float scale)
        where T : IFloatingPointIeee754<T>
    {
        T r = T.CreateChecked(4);
        T two = T.CreateChecked(2);
        var old = gr.Transform;
        gr.ResetTransform();
        gr.ScaleTransform(scale, scale);
        gr.TranslateTransform(offset.X, offset.Y);

        gr.DrawEllipse(pen, Conversion.Cast<T, float>(point.X - r), Conversion.Cast<T, float>(point.Y - r), Conversion.Cast<T, float>(two * r), Conversion.Cast<T, float>(two * r));

        gr.ResetTransform();
        gr.Transform = old;
    }

    /// <summary>
    /// Draws the shape.
    /// </summary>
    /// <param name="shape">The shape.</param>
    /// <param name="gr">The gr.</param>
    /// <param name="offset">The offset.</param>
    /// <param name="scale">The scale.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void DrawShape<T>(this IGeometry shape, Graphics gr, PointF offset, float scale)
        where T : IFloatingPointIeee754<T>
    {
        switch (shape)
        {
            case Circle<T> s:
                DrawShape(s, gr, offset, scale);
                break;
            case Ellipse<T> s:
                DrawShape(s, gr, offset, scale);
                break;
            case ConicSection<T> s:
                DrawShape(s, gr, offset, scale);
                break;
            case StandardParabola<T> s:
                DrawShape(s, gr, offset, scale);
                break;
            case VertexParabola<T> s:
                DrawShape(s, gr, offset, scale);
                break;
            case Hyperbola<T> s:
                DrawShape(s, gr, offset, scale);
                break;
            case Line<T> s:
                DrawShape(s, gr, offset, scale);
                break;
            case QuadraticBezierSegment<T> s:
                DrawShape(s, gr, offset, scale);
                break;
            case CubicBezierSegment<T> s:
                DrawShape(s, gr, offset, scale);
                break;
            case Group s:
                DrawShape<T>(s, gr, offset, scale);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Draws the shape.
    /// </summary>
    /// <param name="circle">The shape.</param>
    /// <param name="gr">The gr.</param>
    /// <param name="offset">The offset.</param>
    /// <param name="scale">The scale.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void DrawShape<T>(this Circle<T> circle, Graphics gr, PointF offset, float scale)
        where T : IFloatingPointIeee754<T>
    {
        var r = circle.R;
        var old = gr.Transform;
        gr.ResetTransform();
        gr.ScaleTransform(scale, scale);
        gr.TranslateTransform(offset.X, offset.Y);

        var pen = (Pen)(circle.Pen ?? Pens.Black);
        gr.DrawEllipse(pen, Conversion.Cast<T, float>(circle.H - r), Conversion.Cast<T, float>(circle.K - r), Conversion.Cast<T, float>(T.CreateChecked(2) * r), Conversion.Cast<T, float>(T.CreateChecked(2) * r));

        gr.ResetTransform();
        gr.Transform = old;
    }

    /// <summary>
    /// Draws the shape.
    /// </summary>
    /// <param name="ellipse">The shape.</param>
    /// <param name="gr">The gr.</param>
    /// <param name="offset">The offset.</param>
    /// <param name="scale">The scale.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void DrawShape<T>(this Ellipse<T> ellipse, Graphics gr, PointF offset, float scale)
        where T : IFloatingPointIeee754<T>
    {
        var old = gr.Transform;
        gr.ResetTransform();
        var mat = new Matrix();
        {
            mat.Scale(scale, scale);
            mat.Translate(offset.X, offset.Y);
            mat.RotateAt(Conversion.Cast<T, float>(ellipse.A.RadiansToDegrees()), ellipse.Center.ToPointF());
        }
        gr.Transform = mat;

        var pen = (Pen)(ellipse.Pen ?? Pens.Black);
        gr.DrawEllipse(pen, Conversion.Cast<T, float>(ellipse.H), Conversion.Cast<T, float>(ellipse.K), Conversion.Cast<T, float>(ellipse.DX), Conversion.Cast<T, float>(ellipse.DY));

        gr.ResetTransform();
        gr.Transform = old;
    }

    /// <summary>
    /// Draws the shape.
    /// </summary>
    /// <param name="conicSection">The shape.</param>
    /// <param name="gr">The gr.</param>
    /// <param name="offset">The offset.</param>
    /// <param name="scale">The scale.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void DrawShape<T>(this ConicSection<T> conicSection, Graphics gr, PointF offset, float scale)
        where T : IFloatingPointIeee754<T>
    {
        if (conicSection.Type == ConicSectionType.Point)
        {
            //DrawPoint(gr, pen,offset,scale, ());
        }

        (var a, var b, var c, var d, var e, var f) = conicSection;

        // Get the X coordinate bounds.
        var xmin = T.Zero;
        var xmax = xmin + T.CreateChecked(gr.VisibleClipBounds.Width);

        // Find the smallest X coordinate with a real value.
        for (var x = xmin; x < xmax; x++)
        {
            var y = Interpolation.UnitConicSectionCarteseanY(x, a, b, c, d, e, f, -1);
            if (y.IsNumber())
            {
                xmin = x;
                break;
            }
        }

        // Find the largest X coordinate with a real value.
        for (var x = xmax; x > xmin; x--)
        {
            var y = Interpolation.UnitConicSectionCarteseanY(x, a, b, c, d, e, f, -1);
            if (y.IsNumber())
            {
                xmax = x;
                break;
            }
        }

        // Get points for the negative root on the left.
        var ln_points = new List<PointF>();
        var xmid1 = xmax;
        for (var x = xmin; x < xmax; x++)
        {
            var y = Interpolation.UnitConicSectionCarteseanY(x, a, b, c, d, e, f, -1);
            if (!y.IsNumber())
            {
                xmid1 = x - T.One;
                break;
            }
            ln_points.Add(new PointF(Conversion.Cast<T, float>(x), Conversion.Cast<T, float>(y)));
        }

        // Get points for the positive root on the left.
        var lp_points = new List<PointF>();
        for (var x = xmid1; x >= xmin; x--)
        {
            var y = Interpolation.UnitConicSectionCarteseanY(x, a, b, c, d, e, f, +1);
            if (y.IsNumber())
            {
                lp_points.Add(new PointF(Conversion.Cast<T, float>(x), Conversion.Cast<T, float>(y)));
            }
        }

        // Make the curves on the right if needed.
        var rp_points = new List<PointF>();
        var rn_points = new List<PointF>();
        var xmid2 = xmax;
        if (xmid1 < xmax)
        {
            // Get points for the positive root on the right.
            for (var x = xmax; x > xmid1; x--)
            {
                var y = Interpolation.UnitConicSectionCarteseanY(x, a, b, c, d, e, f, +1);
                if (!y.IsNumber())
                {
                    xmid2 = x + T.One;
                    break;
                }
                rp_points.Add(new PointF(Conversion.Cast<T, float>(x), Conversion.Cast<T, float>(y)));
            }

            // Get points for the negative root on the right.
            for (var x = xmid2; x <= xmax; x++)
            {
                var y = Interpolation.UnitConicSectionCarteseanY(x, a, b, c, d, e, f, -1);
                if (y.IsNumber())
                {
                    rn_points.Add(new PointF(Conversion.Cast<T, float>(x), Conversion.Cast<T, float>(y)));
                }
            }
        }

        // Connect curves if appropriate.
        // Connect the left curves on the left.
        if (xmin > T.Zero)
        {
            lp_points.Add(ln_points[0]);
        }
        if (lp_points.Count < 1)
        {
            return;
        }
        // Connect the left curves on the right.
        if (xmid1 < T.CreateChecked(gr.VisibleClipBounds.Width))
        {
            ln_points.Add(lp_points[0]);
        }

        // Make sure we have the right curves.
        if (rp_points.Count > 0)
        {
            // Connect the right curves on the left.
            rp_points.Add(rn_points[0]);

            // Connect the right curves on the right.
            if (xmax < T.CreateChecked(gr.VisibleClipBounds.Width))
            {
                rn_points.Add(rp_points[0]);
            }
        }

        // Draw the curves.
        var old = gr.Transform;
        gr.ResetTransform();
        gr.ScaleTransform(scale, scale);
        gr.TranslateTransform(offset.X, offset.Y);

        var pen = (Pen)(conicSection.Pen ?? Pens.Black);
        if (ln_points.Count > 1)
        {
            gr.DrawLines(pen, ln_points.ToArray());
        }

        if (lp_points.Count > 1)
        {
            gr.DrawLines(pen, lp_points.ToArray());
        }

        if (rp_points.Count > 1)
        {
            gr.DrawLines(pen, rp_points.ToArray());
        }

        if (rn_points.Count > 1)
        {
            gr.DrawLines(pen, rn_points.ToArray());
        }

        gr.ResetTransform();
        gr.Transform = old;
    }

    /// <summary>
    /// Draws the shape.
    /// </summary>
    /// <param name="parabola">The shape.</param>
    /// <param name="gr">The gr.</param>
    /// <param name="offset">The offset.</param>
    /// <param name="scale">The scale.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void DrawShape<T>(this StandardParabola<T> parabola, Graphics gr, PointF offset, float scale)
        where T : IFloatingPointIeee754<T>
    {
        var bounds = gr.VisibleClipBounds;
        var (aX, aY, bX, bY, cX, cY, dX, dY)
            = (parabola.I < T.Zero || parabola.I > T.Zero || parabola.I < T.Pi || parabola.I > T.Pi)
            ? parabola.ToCubicBezier(T.CreateChecked(bounds.Left + offset.X), T.CreateChecked(bounds.Top + offset.Y), T.CreateChecked(bounds.Right + offset.X), T.CreateChecked(bounds.Bottom + offset.Y))
            : parabola.ToCubicBezier(T.CreateChecked(bounds.Left + offset.X), T.CreateChecked(bounds.Right + offset.X));

        var old = gr.Transform;
        gr.ResetTransform();
        gr.ScaleTransform(scale, scale);
        gr.TranslateTransform(offset.X, offset.Y);

        var pen = (Pen)(parabola.Pen ?? Pens.Black);
        gr.DrawBezier(pen, Conversion.Cast<T, float>(aX), Conversion.Cast<T, float>(aY), Conversion.Cast<T, float>(bX), Conversion.Cast<T, float>(bY), Conversion.Cast<T, float>(cX), Conversion.Cast<T, float>(cY), Conversion.Cast<T, float>(dX), Conversion.Cast<T, float>(dY));

        gr.ResetTransform();
        gr.Transform = old;
    }

    /// <summary>
    /// Draws the shape.
    /// </summary>
    /// <param name="parabola">The shape.</param>
    /// <param name="gr">The gr.</param>
    /// <param name="offset">The offset.</param>
    /// <param name="scale">The scale.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void DrawShape<T>(this VertexParabola<T> parabola, Graphics gr, PointF offset, float scale)
        where T : IFloatingPointIeee754<T>
    {
        var bounds = gr.VisibleClipBounds;
        var (aX, aY, bX, bY, cX, cY, dX, dY)
            = (parabola.I < T.Zero || parabola.I > T.Zero || parabola.I < T.Pi || parabola.I > T.Pi)
            ? parabola.ToCubicBezier(T.CreateChecked(bounds.Left + offset.X), T.CreateChecked(bounds.Top + offset.Y), T.CreateChecked(bounds.Right + offset.X), T.CreateChecked(bounds.Bottom + offset.Y))
            : parabola.ToCubicBezier(T.CreateChecked(bounds.Left + offset.X), T.CreateChecked(bounds.Right + offset.X));

        var old = gr.Transform;
        gr.ResetTransform();
        gr.ScaleTransform(scale, scale);
        gr.TranslateTransform(offset.X, offset.Y);

        var pen = (Pen)(parabola.Pen ?? Pens.Black);
        gr.DrawBezier(pen, Conversion.Cast<T, float>(aX), Conversion.Cast<T, float>(aY), Conversion.Cast<T, float>(bX), Conversion.Cast<T, float>(bY), Conversion.Cast<T, float>(cX), Conversion.Cast<T, float>(cY), Conversion.Cast<T, float>(dX), Conversion.Cast<T, float>(dY));

        gr.ResetTransform();
        gr.Transform = old;
    }

    /// <summary>
    /// Draws the shape.
    /// </summary>
    /// <param name="shape">The shape.</param>
    /// <param name="gr">The gr.</param>
    /// <param name="offset">The offset.</param>
    /// <param name="scale">The scale.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void DrawShape<T>(this Hyperbola<T> shape, Graphics gr, PointF offset, float scale) where T : IFloatingPointIeee754<T> => DrawShape(shape.ConicSection = shape.ConicSection is null ? shape.ToUnitConicSection() : shape.ConicSection, gr, offset, scale);

    /// <summary>
    /// Draws the shape.
    /// </summary>
    /// <param name="line">The shape.</param>
    /// <param name="gr">The gr.</param>
    /// <param name="offset">The offset.</param>
    /// <param name="scale">The scale.</param>
    /// <exception cref="NotImplementedException"></exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void DrawShape<T>(this Line<T> line, Graphics gr, PointF offset, float scale)
        where T : IFloatingPointIeee754<T>
    {
        var bounds = gr.ClipBounds;
        var points = Intersections.LineRectangleIntersection(line.X, line.Y, line.I, line.J, T.CreateChecked(bounds.X), T.CreateChecked(bounds.Y), T.CreateChecked(bounds.Width), T.CreateChecked(bounds.Height));
        if (points.Count > 1)
        {
            var old = gr.Transform;
            gr.ResetTransform();
            gr.ScaleTransform(scale, scale);
            gr.TranslateTransform(offset.X, offset.Y);

            var pen = (Pen)(line.Pen ?? Pens.Black);
            gr.DrawLine(pen, Conversion.Cast<T, float>(points[0].X), Conversion.Cast<T, float>(points[0].Y), Conversion.Cast<T, float>(points[1].X), Conversion.Cast<T, float>(points[1].Y));

            gr.ResetTransform();
            gr.Transform = old;
        }
    }

    /// <summary>
    /// Draws the shape.
    /// </summary>
    /// <param name="quadraticBezierSegment">The shape.</param>
    /// <param name="gr">The gr.</param>
    /// <param name="offset">The offset.</param>
    /// <param name="scale">The scale.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void DrawShape<T>(this QuadraticBezierSegment<T> quadraticBezierSegment, Graphics gr, PointF offset, float scale)
        where T : IFloatingPointIeee754<T>
    {
        var (aX, aY, bX, bY, cX, cY, dX, dY) = Conversion.QuadraticBezierToCubicBezier(quadraticBezierSegment.AX, quadraticBezierSegment.AY, quadraticBezierSegment.BX, quadraticBezierSegment.BY, quadraticBezierSegment.CX, quadraticBezierSegment.CY);

        var old = gr.Transform;
        gr.ResetTransform();
        gr.ScaleTransform(scale, scale);
        gr.TranslateTransform(offset.X, offset.Y);

        var pen = (Pen)(quadraticBezierSegment.Pen ?? Pens.Black);
        gr.DrawBezier(pen, Conversion.Cast<T, float>(aX), Conversion.Cast<T, float>(aY), Conversion.Cast<T, float>(bX), Conversion.Cast<T, float>(bY), Conversion.Cast<T, float>(cX), Conversion.Cast<T, float>(cY), Conversion.Cast<T, float>(dX), Conversion.Cast<T, float>(dY));

        gr.ResetTransform();
        gr.Transform = old;
    }

    /// <summary>
    /// Draws the shape.
    /// </summary>
    /// <param name="cubicBezierSegment">The shape.</param>
    /// <param name="gr">The gr.</param>
    /// <param name="offset">The offset.</param>
    /// <param name="scale">The scale.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void DrawShape<T>(this CubicBezierSegment<T> cubicBezierSegment, Graphics gr, PointF offset, float scale)
        where T : IFloatingPointIeee754<T>
    {
        var old = gr.Transform;
        gr.ResetTransform();
        gr.ScaleTransform(scale, scale);
        gr.TranslateTransform(offset.X, offset.Y);

        var pen = (Pen)(cubicBezierSegment.Pen ?? Pens.Black);
        gr.DrawBezier(pen, Conversion.Cast<T, float>(cubicBezierSegment.AX), Conversion.Cast<T, float>(cubicBezierSegment.AX), Conversion.Cast<T, float>(cubicBezierSegment.BX), Conversion.Cast<T, float>(cubicBezierSegment.BX), Conversion.Cast<T, float>(cubicBezierSegment.CX), Conversion.Cast<T, float>(cubicBezierSegment.CX), Conversion.Cast<T, float>(cubicBezierSegment.DX), Conversion.Cast<T, float>(cubicBezierSegment.DX));

        gr.ResetTransform();
        gr.Transform = old;
    }

    /// <summary>
    /// Draws the shape.
    /// </summary>
    /// <param name="group">The group.</param>
    /// <param name="gr">The gr.</param>
    /// <param name="offset">The offset.</param>
    /// <param name="scale">The scale.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void DrawShape<T>(this Group group, Graphics gr, PointF offset, float scale)
        where T : IFloatingPointIeee754<T>
    {
        foreach (var shape in group.Shapes)
        {
            shape.DrawShape<T>(gr, offset, scale);
        }
    }
}
