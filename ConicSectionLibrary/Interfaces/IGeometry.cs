// <copyright file="IGeometry.cs">
//     Copyright © 2019 - 2020 Shkyrockett. All rights reserved.
// </copyright>
// <author id="shkyrockett">Shkyrockett</author>
// <license>
//     Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </license>
// <summary></summary>
// <remarks></remarks>

using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Numerics;

namespace ConicSectionLibrary
{
    /// <summary>
    /// The IGeometry interface.
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public interface IGeometry
        : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the pen.
        /// </summary>
        /// <value>
        /// The pen.
        /// </value>
        //[Editor("System.Drawing.Design.UITypeEditor.ColorEditor, System.Design", typeof(UITypeEditor))]
        public IDisposable Pen { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        #region Methods
        /// <summary>
        /// Translates the specified delta.
        /// </summary>
        /// <param name="delta">The delta.</param>
        /// <returns></returns>
        IGeometry Translate(Vector2 delta);

        /// <summary>
        /// Queries whether the shape includes the specified point in it's geometry.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns></returns>
        bool Includes(PointF point);

        /// <summary>
        /// Creates a string representation of this <see cref="IGeometry" /> struct based on the format string and IFormatProvider passed in.
        /// If the provider is null, the CurrentCulture is used.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> representation of this object.
        /// </returns>
        public string? ToString() => ToString("R" /* format string */, CultureInfo.InvariantCulture /* format provider */);

        /// <summary>
        /// Creates a string representation of this <see cref="IGeometry" /> struct based on the format string and IFormatProvider passed in.
        /// If the provider is null, the CurrentCulture is used.
        /// </summary>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns></returns>
        public string? ToString(IFormatProvider formatProvider) => ToString("R" /* format string */, CultureInfo.InvariantCulture /* format provider */);

        /// <summary>
        /// Creates a string representation of this <see cref="IGeometry" /> struct based on the format string and IFormatProvider passed in.
        /// If the provider is null, the CurrentCulture is used.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns></returns>
        string? ToString(string format, IFormatProvider formatProvider);
        #endregion
    }

    /// <summary>
    /// The IGeometry interface.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGeometry<T>
        : IGeometry
    {
    }
}
