using PdfiumViewer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace PdfToImage
{
    class Program
    {
        static void Main(string[] args)
        {
            var projectDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

            try
            {
                var document = PdfDocument.Load(projectDir + @"\Samples\input.pdf");
                var pageCount = document.PageCount;
                for (var i = 0; i < pageCount; i++)
                {
                    const int dpi = 300;
                    using (var image = document.Render(i, dpi, dpi, PdfRenderFlags.CorrectFromDpi))
                    {
                        var encoder = ImageCodecInfo.GetImageEncoders()
                            .First(c => c.FormatID == ImageFormat.Jpeg.Guid);
                        var encParams = new EncoderParameters(1)
                        {
                            Param =
                            {
                                [0] = new EncoderParameter(
                                    System.Drawing.Imaging.Encoder.Quality, 100L)
                            }
                    };
                        image.Save(projectDir+ @"\Samples\output.png", encoder, encParams);
                        //var imageStream = new MemoryStream();
                    }
                }
                document.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Press enter to continue...");
            Console.ReadLine();
        }
    }
}
