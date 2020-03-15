// <copyright file="Rendering.cs">
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
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;

namespace ConicSectionPlayground
{
    /// <summary>
    /// 
    /// </summary>
    public static class Rendering
    {
        /// <summary>
        /// Draws the conic section.
        /// </summary>
        /// <param name="gr">The gr.</param>
        /// <param name="pen">The pen.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="conicSection">The conic section.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawConicSection(Graphics gr, Pen pen, Point offset, float scale, ConicSection conicSection)
        {
            (var a, var b, var c, var d, var e, var f) = conicSection;

            // Get the X coordinate bounds.
            var xmin = 0f;
            var xmax = xmin + gr.VisibleClipBounds.Width;

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
                    xmid1 = x - 1;
                    break;
                }
                ln_points.Add(new PointF(x, (float)y));
            }

            // Get points for the positive root on the left.
            var lp_points = new List<PointF>();
            for (var x = xmid1; x >= xmin; x--)
            {
                var y = Interpolation.UnitConicSectionCarteseanY(x, a, b, c, d, e, f, +1);
                if (y.IsNumber())
                {
                    lp_points.Add(new PointF(x, (float)y));
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
                        xmid2 = x + 1;
                        break;
                    }
                    rp_points.Add(new PointF(x, (float)y));
                }

                // Get points for the negative root on the right.
                for (var x = xmid2; x <= xmax; x++)
                {
                    var y = Interpolation.UnitConicSectionCarteseanY(x, a, b, c, d, e, f, -1);
                    if (y.IsNumber())
                    {
                        rn_points.Add(new PointF(x, (float)y));
                    }
                }
            }

            // Connect curves if appropriate.
            // Connect the left curves on the left.
            if (xmin > 0)
            {
                lp_points.Add(ln_points[0]);
            }
            if (lp_points.Count < 1)
            {
                return;
            }
            // Connect the left curves on the right.
            if (xmid1 < gr.VisibleClipBounds.Width)
            {
                ln_points.Add(lp_points[0]);
            }

            // Make sure we have the right curves.
            if (rp_points.Count > 0)
            {
                // Connect the right curves on the left.
                rp_points.Add(rn_points[0]);

                // Connect the right curves on the right.
                if (xmax < gr.VisibleClipBounds.Width)
                {
                    rn_points.Add(rp_points[0]);
                }
            }

            // Draw the curves.
            var old = gr.Transform;
            gr.ResetTransform();
            gr.ScaleTransform(scale, scale);
            gr.TranslateTransform(offset.X, offset.Y);

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
        /// Draws the circle.
        /// </summary>
        /// <param name="gr">The gr.</param>
        /// <param name="pen">The pen.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="circle">The circle.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawCircle(Graphics gr, Pen pen, Point offset, float scale, Circle circle)
        {
            var r = circle.R;
            var old = gr.Transform;
            gr.ResetTransform();
            gr.ScaleTransform(scale, scale);
            gr.TranslateTransform(offset.X, offset.Y);

            gr.DrawEllipse(pen, (float)(circle.H - r), (float)(circle.K - r), (float)(2d * r), (float)(2d * r));

            gr.ResetTransform();
            gr.Transform = old;
        }

        /// <summary>
        /// The draw ellipse.
        /// </summary>
        /// <param name="gr">The gr.</param>
        /// <param name="pen">The pen.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="ellipse">The ellipse.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawEllipse(Graphics gr, Pen pen, Point offset, float scale, Ellipse ellipse)
        {
            var old = gr.Transform;
            gr.ResetTransform();
            var mat = new Matrix();
            {
                mat.Scale(scale, scale);
                mat.Translate(offset.X, offset.Y);
                mat.RotateAt((float)ellipse.A.RadiansToDegrees(), ellipse.Center);
            }
            gr.Transform = mat;

            gr.DrawEllipse(pen, (float)ellipse.H, (float)ellipse.K, (float)ellipse.DX, (float)ellipse.DY);

            gr.ResetTransform();
            gr.Transform = old;
        }

        /// <summary>
        /// The draw parabola.
        /// </summary>
        /// <param name="gr">The gr.</param>
        /// <param name="pen">The pen.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="parabola">The parabola.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawStandardParabola(Graphics gr, Pen pen, Point offset, float scale, StandardParabola parabola)
        {
            var bounds = gr.VisibleClipBounds;
            var (aX, aY, bX, bY, cX, cY, dX, dY)
                = (parabola.I < 0 || parabola.I > 0 || parabola.I < Math.PI || parabola.I > Math.PI)
                ? parabola.ToCubicBezier(bounds.Left, bounds.Top, bounds.Right, bounds.Bottom)
                : parabola.ToCubicBezier(bounds.Left, bounds.Right);

            var old = gr.Transform;
            gr.ResetTransform();
            gr.ScaleTransform(scale, scale);
            gr.TranslateTransform(offset.X, offset.Y);

            gr.DrawBezier(pen, (float)aX, (float)aY, (float)bX, (float)bY, (float)cX, (float)cY, (float)dX, (float)dY);

            gr.ResetTransform();
            gr.Transform = old;
        }

        /// <summary>
        /// Draws the vertex parabola.
        /// </summary>
        /// <param name="gr">The gr.</param>
        /// <param name="pen">The pen.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="parabola">The parabola.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawVertexParabola(Graphics gr, Pen pen, Point offset, float scale, VertexParabola parabola)
        {
            var bounds = gr.VisibleClipBounds;
            var (aX, aY, bX, bY, cX, cY, dX, dY)
                = (parabola.I < 0 || parabola.I > 0 || parabola.I < Math.PI || parabola.I > Math.PI)
                ? parabola.ToCubicBezier(bounds.Left, bounds.Top, bounds.Right, bounds.Bottom)
                : parabola.ToCubicBezier(bounds.Left, bounds.Right);

            var old = gr.Transform;
            gr.ResetTransform();
            gr.ScaleTransform(scale, scale);
            gr.TranslateTransform(offset.X, offset.Y);

            gr.DrawBezier(pen, (float)aX, (float)aY, (float)bX, (float)bY, (float)cX, (float)cY, (float)dX, (float)dY);

            gr.ResetTransform();
            gr.Transform = old;
        }

        /// <summary>
        /// Draws the line.
        /// </summary>
        /// <param name="gr">The gr.</param>
        /// <param name="pen">The pen.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="line">The line.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawLine(Graphics gr, Pen pen, Point offset, float scale, Line line)
        {
            var bounds = gr.ClipBounds;
            var points = Intersections.LineRectangleIntersection(line.X, line.Y, line.I, line.J, bounds.X, bounds.Y, bounds.Width, bounds.Height);
            if (points.Count > 1)
            {
                var old = gr.Transform;
                gr.ResetTransform();
                gr.ScaleTransform(scale, scale);
                gr.TranslateTransform(offset.X, offset.Y);

                gr.DrawLine(pen, points[0].X, points[0].Y, points[1].X, points[1].Y);

                gr.ResetTransform();
                gr.Transform = old;
            }
        }

        /// <summary>
        /// Draws the cubic bezier.
        /// </summary>
        /// <param name="gr">The gr.</param>
        /// <param name="pen">The pen.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="cubicBezierSegment">The cubic bezier segment.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawCubicBezier(Graphics gr, Pen pen, Point offset, float scale, CubicBezierSegment cubicBezierSegment)
        {
            var old = gr.Transform;
            gr.ResetTransform();
            gr.ScaleTransform(scale, scale);
            gr.TranslateTransform(offset.X, offset.Y);

            gr.DrawBezier(pen, (float)cubicBezierSegment.AX, (float)cubicBezierSegment.AX, (float)cubicBezierSegment.BX, (float)cubicBezierSegment.BX, (float)cubicBezierSegment.CX, (float)cubicBezierSegment.CX, (float)cubicBezierSegment.DX, (float)cubicBezierSegment.DX);

            gr.ResetTransform();
            gr.Transform = old;
        }

        /// <summary>
        /// Draws the quadratic bezier.
        /// </summary>
        /// <param name="gr">The gr.</param>
        /// <param name="pen">The pen.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="quadraticBezierSegment">The quadratic bezier segment.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawQuadraticBezier(Graphics gr, Pen pen, Point offset, float scale, QuadraticBezierSegment quadraticBezierSegment)
        {
            var (aX, aY, bX, bY, cX, cY, dX, dY) = Conversion.QuadraticBezierToCubicBezier(quadraticBezierSegment.AX, quadraticBezierSegment.AY, quadraticBezierSegment.BX, quadraticBezierSegment.BY, quadraticBezierSegment.CX, quadraticBezierSegment.CY);

            var old = gr.Transform;
            gr.ResetTransform();
            gr.ScaleTransform(scale, scale);
            gr.TranslateTransform(offset.X, offset.Y);

            gr.DrawBezier(pen, (float)aX, (float)aY, (float)bX, (float)bY, (float)cX, (float)cY, (float)dX, (float)dY);

            gr.ResetTransform();
            gr.Transform = old;
        }
    }
}
