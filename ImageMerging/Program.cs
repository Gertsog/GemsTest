using SkiaSharp;

namespace ImageMerging
{
    internal class Program
    {
        const int DefaultImageHeight = 600;
        const int DefaultImageWidth = 800;

        static void Main(string[] args)
        {
            //Console.WriteLine("Введите путь к папке с изображениями:");
            //var inputText = Console.ReadLine();

            var path = ".\\Images1";
            var images = GetImageList(path);

            if (images.Length == 0)
            {
                var resultImage = CreateWhiteImage(DefaultImageWidth, DefaultImageHeight);
                var resultName = GenerateResultName();
                var outPath = Path.Combine(path, resultName);
                SaveImage(resultImage, outPath);
            }

            foreach(var image in images)
                Console.WriteLine(image);
        }

        static string[] GetImageList(string path)
        {
            if (Directory.Exists(path))
                return Directory.GetFiles(path);

            return Array.Empty<string>();
        }

        static SKImage CreateWhiteImage(int width, int height)
        {
            using var tempSurface = SKSurface.Create(new SKImageInfo(width, height));
            var canvas = tempSurface.Canvas;
            canvas.Clear(SKColors.White);

            return tempSurface.Snapshot();
        }

        static string GenerateResultName()
        {
            return "Result" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
        }

        static void SaveImage(SKImage image, string path)
        {
            using SKData encoded = image.Encode(SKEncodedImageFormat.Jpeg, 100);
            using Stream outFile = File.OpenWrite(path);
            encoded.SaveTo(outFile);
        }
    }
}