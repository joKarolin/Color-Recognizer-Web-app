using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing;
using System.IO;

namespace ColorRecognizerWebApp.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public IFormFile UploadedImage { get; set; }
        public string ImagePath { get; set; }
        public string HexColor { get; set; }
        public string RGBColor { get; set; }

        public void OnPost()
        {
            if (UploadedImage != null)
            {
                // Save the uploaded image to wwwroot/images folder
                var fileName = Path.GetFileName(UploadedImage.FileName);
                var filePath = Path.Combine("wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    UploadedImage.CopyTo(stream);
                }

                ImagePath = $"/images/{fileName}";

                // Extract color from the center of the image
                using (var bitmap = new Bitmap(filePath))
                {
                    int x = bitmap.Width / 2;
                    int y = bitmap.Height / 2;

                    Color color = bitmap.GetPixel(x, y);
                    HexColor = ColorTranslator.ToHtml(color);
                    RGBColor = $"rgb({color.R}, {color.G}, {color.B})";
                }
            }
        }
    }
}
