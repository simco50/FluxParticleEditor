using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ParticleEditor.Helpers.Data
{
    public static class ImageLoader
    {
        private static readonly string FallbackImage = "./Resources/ErrorTexture.jpg";
        public static ImageSource ToImageSource(string filePath, out string usedFilePath)
        {
            DebugLog.Log($"Loading image at '{filePath}'...", "Image loading");
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
            usedFilePath = fileName;
            data = File.ReadAllBytes(fileName);
            var imageData = new BitmapImage();
            imageData.BeginInit();
            imageData.StreamSource = new MemoryStream(data);
            imageData.EndInit();
            imageData.Freeze();
            DebugLog.Log($"Image loading successful", "Image loading");
            return imageData;
        }
    }
}
