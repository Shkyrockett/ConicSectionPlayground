// <copyright file="Program.cs">
//     Copyright � 2019 - 2020 Shkyrockett. All rights reserved.
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
using System.Windows.Forms;

namespace ConicSectionPlayground
{
    /// <summary>
    /// The program class.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            TypeDescriptor.AddAttributes(typeof(PointF), new TypeConverterAttribute(typeof(ExpandableObjectConverter)));
            TypeDescriptor.AddAttributes(typeof(PointF), new SerializableAttribute());
            
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using var mainForm = new Form1();
            Application.Run(mainForm);
        }
    }
}
