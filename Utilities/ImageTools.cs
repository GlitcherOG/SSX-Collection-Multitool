using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Drawing;
using System.IO;

namespace SSXMultiTool.Utilities
{
    public class ImageTools
    {
        // Make sure to add 'using ImageSharp.Formats.Bmp;' or another encoder if needed.
        public static System.Drawing.Bitmap ImageSharpToBitmap(SixLabors.ImageSharp.Image img)
        {
            if (img == null)
            {
                return null;
            }

            // Use a MemoryStream to transfer the image data
            using (var stream = new MemoryStream())
            {
                // Save the ImageSharp image to the stream in a compatible format (e.g., BmpFormat)
                // The stream must be kept alive during the conversion process.
                img.Save(stream, new SixLabors.ImageSharp.Formats.Png.PngEncoder());
                stream.Position = 0;

                // Create a System.Drawing.Bitmap from the stream
                var bitmap = new System.Drawing.Bitmap(stream);

                // Return the Bitmap (which is a System.Drawing.Image)
                return bitmap;
            }
        }
    }
}
