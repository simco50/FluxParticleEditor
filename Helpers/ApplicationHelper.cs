using System.ComponentModel;
using System.Windows;
using System;

namespace ParticleEditor.Helpers
{
    class ApplicationHelper
    {
        public static bool IsDesignMode
    => (bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue);

        public static string DataPath { get; } = "D:/2016-2017/S5 - Tool Development/Exam/ParticleEditor/bin/Debug/";

        public static float Round(float nr, int decimals = 0)
        {
            return (float)(Math.Round(nr, decimals));
        }
    }
}
