using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ParticleEditor.Helpers
{
    class ApplicationHelper
    {
        public static bool IsDesignMode
    => (bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue);

        public static string DataPath { get; } = "D:/2016-2017/S5 - Tool Development/Exam/ParticleEditor/bin/Debug/";
    }
}
