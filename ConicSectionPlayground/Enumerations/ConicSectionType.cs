// <copyright file="ConicType.cs">
//     Copyright © 2019 - 2020 Shkyrockett. All rights reserved.
// </copyright>
// <author id="shkyrockett">Shkyrockett</author>
// <license>
//     Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </license>
// <summary></summary>
// <remarks></remarks>

namespace ConicSectionPlayground
{
    /// <summary>
    /// Enumeration of types of Conics.
    /// https://www.ck12.org/book/CK-12-College-Precalculus/section/11.7/
    /// https://en.wikipedia.org/wiki/Degenerate_conic
    /// http://www.math.com/tables/algebra/conics.htm
    /// </summary>
    public enum ConicSectionType
    {
        /// <summary>
        /// The conic is a point.
        /// </summary>
        Point,

        /// <summary>
        /// The conic is a circle.
        /// </summary>
        Circle,

        /// <summary>
        /// The conic is a ellipse.
        /// </summary>
        Ellipse,

        /// <summary>
        /// The conic is a parabola.
        /// </summary>
        Parabola,

        /// <summary>
        /// The conic is a line.
        /// </summary>
        Line,

        /// <summary>
        /// The conic is a pair of parallel line.
        /// </summary>
        ParalellLines,

        /// <summary>
        /// The conic is a pair of intersecting lines.
        /// </summary>
        CrossingLines,

        /// <summary>
        /// The conic is a rectangular hyperbola.
        /// </summary>
        RectangularHyperbola,

        /// <summary>
        /// The conic is a hyperbola.
        /// </summary>
        Hyperbola,
    }
}
