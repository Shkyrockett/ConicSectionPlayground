// <copyright file="WinformsExtentions.cs">
//     Copyright © 2020 Shkyrockett. All rights reserved.
// </copyright>
// <author id="shkyrockett">Shkyrockett</author>
// <license>
//     Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </license>
// <summary></summary>
// <remarks></remarks>

using ConicSectionLibrary;
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawPoint((double X, double Y) point, Graphics gr, Pen pen, PointF offset, float scale)
        {
            const double r = 4;
            var old = gr.Transform;
            gr.ResetTransform();
            gr.ScaleTransform(scale, scale);
            gr.TranslateTransform(offset.X, offset.Y);

            gr.DrawEllipse(pen, (float)(point.X - r), (float)(point.Y - r), (float)(2d * r), (float)(2d * r));

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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawShape(this IGeometry shape, Graphics gr, PointF offset, float scale)
        {
            switch (shape)
            {
                case Circle s:
                    DrawShape(s, gr, offset, scale);
                    break;
                case Ellipse s:
                    DrawShape(s, gr, offset, scale);
                    break;
                case ConicSection s:
                    DrawShape(s, gr, offset, scale);
                    break;
                case StandardParabola s:
                    DrawShape(s, gr, offset, scale);
                    break;
                case VertexParabola s:
                    DrawShape(s, gr, offset, scale);
                    break;
                case Hyperbola s:
                    DrawShape(s, gr, offset, scale);
                    break;
                case Line s:
                    DrawShape(s, gr, offset, scale);
                    break;
                case QuadraticBezierSegment s:
                    DrawShape(s, gr, offset, scale);
                    break;
                case CubicBezierSegment s:
                    DrawShape(s, gr, offset, scale);
                    break;
                case Group s:
                    DrawShape(s, gr, offset, scale);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawShape(this Circle circle, Graphics gr, PointF offset, float scale)
        {
            var r = circle.R;
            var old = gr.Transform;
            gr.ResetTransform();
            gr.ScaleTransform(scale, scale);
            gr.TranslateTransform(offset.X, offset.Y);

            var pen = (Pen)(circle.Pen ?? Pens.Black);
            gr.DrawEllipse(pen, (float)(circle.H - r), (float)(circle.K - r), (float)(2d * r), (float)(2d * r));

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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawShape(this Ellipse ellipse, Graphics gr, PointF offset, float scale)
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

            var pen = (Pen)(ellipse.Pen ?? Pens.Black);
            gr.DrawEllipse(pen, (float)ellipse.H, (float)ellipse.K, (float)ellipse.DX, (float)ellipse.DY);

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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawShape(this ConicSection conicSection, Graphics gr, PointF offset, float scale)
        {
            if (conicSection.Type == ConicSectionType.Point)
            {
                //DrawPoint(gr, pen,offset,scale, ());
            }

            (var a, var b, var c, var d, var e, var f) = conicSection;

            // Get the X coordinate bounds.
            var xmin = 0f;
            var xmax = xmin + gr.VisibleClipBounds.Width;

            // Find the smallest X coordinate with a real value.
            for (var x = xmin; x < xmax; x++)
            {
                var y = InterpolationD.UnitConicSectionCarteseanY(x, a, b, c, d, e, f, -1);
                if (y.IsNumber())
                {
                    xmin = x;
                    break;
                }
            }

            // Find the largest X coordinate with a real value.
            for (var x = xmax; x > xmin; x--)
            {
                var y = InterpolationD.UnitConicSectionCarteseanY(x, a, b, c, d, e, f, -1);
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
                var y = InterpolationD.UnitConicSectionCarteseanY(x, a, b, c, d, e, f, -1);
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
                var y = InterpolationD.UnitConicSectionCarteseanY(x, a, b, c, d, e, f, +1);
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
                    var y = InterpolationD.UnitConicSectionCarteseanY(x, a, b, c, d, e, f, +1);
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
                    var y = InterpolationD.UnitConicSectionCarteseanY(x, a, b, c, d, e, f, -1);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawShape(this StandardParabola parabola, Graphics gr, PointF offset, float scale)
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

            var pen = (Pen)(parabola.Pen ?? Pens.Black);
            gr.DrawBezier(pen, (float)aX, (float)aY, (float)bX, (float)bY, (float)cX, (float)cY, (float)dX, (float)dY);

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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawShape(this VertexParabola parabola, Graphics gr, PointF offset, float scale)
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

            var pen = (Pen)(parabola.Pen ?? Pens.Black);
            gr.DrawBezier(pen, (float)aX, (float)aY, (float)bX, (float)bY, (float)cX, (float)cY, (float)dX, (float)dY);

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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawShape(this Hyperbola shape, Graphics gr, PointF offset, float scale) => DrawShape(shape.ConicSection = shape.ConicSection is null ? shape.ToUnitConicSection() : shape.ConicSection, gr, offset, scale);

        /// <summary>
        /// Draws the shape.
        /// </summary>
        /// <param name="line">The shape.</param>
        /// <param name="gr">The gr.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="scale">The scale.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawShape(this Line line, Graphics gr, PointF offset, float scale)
        {
            var bounds = gr.ClipBounds;
            var points = IntersectionsD.LineRectangleIntersection(line.X, line.Y, line.I, line.J, bounds.X, bounds.Y, bounds.Width, bounds.Height);
            if (points.Count > 1)
            {
                var old = gr.Transform;
                gr.ResetTransform();
                gr.ScaleTransform(scale, scale);
                gr.TranslateTransform(offset.X, offset.Y);

                var pen = (Pen)(line.Pen ?? Pens.Black);
                gr.DrawLine(pen, points[0].X, points[0].Y, points[1].X, points[1].Y);

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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawShape(this QuadraticBezierSegment quadraticBezierSegment, Graphics gr, PointF offset, float scale)
        {
            var (aX, aY, bX, bY, cX, cY, dX, dY) = ConversionD.QuadraticBezierToCubicBezier(quadraticBezierSegment.AX, quadraticBezierSegment.AY, quadraticBezierSegment.BX, quadraticBezierSegment.BY, quadraticBezierSegment.CX, quadraticBezierSegment.CY);

            var old = gr.Transform;
            gr.ResetTransform();
            gr.ScaleTransform(scale, scale);
            gr.TranslateTransform(offset.X, offset.Y);

            var pen = (Pen)(quadraticBezierSegment.Pen ?? Pens.Black);
            gr.DrawBezier(pen, (float)aX, (float)aY, (float)bX, (float)bY, (float)cX, (float)cY, (float)dX, (float)dY);

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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawShape(this CubicBezierSegment cubicBezierSegment, Graphics gr, PointF offset, float scale)
        {
            var old = gr.Transform;
            gr.ResetTransform();
            gr.ScaleTransform(scale, scale);
            gr.TranslateTransform(offset.X, offset.Y);

            var pen = (Pen)(cubicBezierSegment.Pen ?? Pens.Black);
            gr.DrawBezier(pen, (float)cubicBezierSegment.AX, (float)cubicBezierSegment.AX, (float)cubicBezierSegment.BX, (float)cubicBezierSegment.BX, (float)cubicBezierSegment.CX, (float)cubicBezierSegment.CX, (float)cubicBezierSegment.DX, (float)cubicBezierSegment.DX);

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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawShape(this Group group, Graphics gr, PointF offset, float scale)
        {
            foreach (var shape in group.Shapes)
            {
                shape.DrawShape(gr, offset, scale);
            }
        }
    }
}
