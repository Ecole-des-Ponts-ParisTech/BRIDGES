using System;
using System.Collections.Generic;
using System.Text;

namespace BRIDGES
{
    /// <summary>
    /// Static class defining global settings for the BRIDGES framework.
    /// </summary>
    public static class Settings
    {
        /// <summary>
        /// Absolute angular precision (in radians).
        /// </summary>
        public const double AngularPrecision = Math.PI / 1e4;

        /// <summary>
        /// Absolute linear precision.
        /// </summary>
        public const double AbsolutePrecision = 1e-8;
    }
}
