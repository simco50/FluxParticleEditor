using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ParticleEditor.Debugging;

namespace ParticleEditor.Helpers
{
    public static class ImageLoader
    {
        private static readonly string FallbackImage = "../../Resources/ErrorTexture.jpg";
        public static ImageSource ToImageSource(string filePath)
        {
            DebugLog.Log($"Loading image at '{filePath}'...");
            string rootPath = ApplicationHelper.IsDesignMode ? ApplicationHelper.DataPath : "";
            string fileName = Path.Combine(rootPath, filePath);
            fileName = Path.GetFullPath(fileName);

            byte[] data;
            if (!File.Exists(fileName))
            {
                DebugLog.Log($"Image at location '{fileName}' could not be found. Using fallback image",
                    "Error loading image", LogSeverity.Warning);
                fileName = $"{rootPath}{FallbackImage}";
            }
            data = File.ReadAllBytes(fileName);
            var imageData = new BitmapImage();
            imageData.BeginInit();
            imageData.StreamSource = new MemoryStream(data);
            imageData.EndInit();
            imageData.Freeze();
            DebugLog.Log($"Image loading successful");
            return imageData;
        }
    }
}
