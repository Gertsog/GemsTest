using SkiaSharp;
using System.Text.RegularExpressions;

namespace ImageMerging
{
    public class Program
    {
        const int DefaultImageHeight = 600;
        const int DefaultImageWidth = 800;

        private static void Main(string[] args)
        {
            Console.WriteLine("Введите путь к папке с изображениями:");
            var inputText = Console.ReadLine();
            try
            {
                var path = Path.GetFullPath(inputText);
                var filePaths = GetFileArray(path);
                var imagePaths = FilterImageArray(filePaths);

                using var resultImage = CreateImage(imagePaths);

                var resultName = GenerateResultName();
                var outPath = Path.Combine(path, resultName);

                SaveImage(resultImage, outPath);
                Console.WriteLine($"Создано изображение {outPath}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static string[] GetFileArray(string path)
        {
            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException($"Папки {path} не существует");

            return Directory.GetFiles(path);
        }

        private static string[] FilterImageArray(string[] images)
        {
            var correctFileName = new Regex(@".*\\Image\.\d+\.\d+\.(jpe?g|JPE?G)$");
            var result = images.Where(i => correctFileName.IsMatch(i)).ToArray();
            return result;
        }

        private static SKImage CreateWhiteImage(int width, int height)
        {
            using var tempSurface = SKSurface.Create(new SKImageInfo(width, height));
            var canvas = tempSurface.Canvas;
            canvas.Clear(SKColors.White);

            return tempSurface.Snapshot();
        }

        private static string GenerateResultName()
        {
            return "Result" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
        }

        private static void SaveImage(SKImage image, string path)
        {
            using SKData encoded = image.Encode(SKEncodedImageFormat.Jpeg, 100);
            using Stream outFile = File.OpenWrite(path);
            encoded.SaveTo(outFile);
        }

        private static SKImage CreateImage(string[] imagePaths)
        {
            if (imagePaths.Length == 0)
               return CreateWhiteImage(DefaultImageWidth, DefaultImageHeight);

            if (imagePaths.Length == 1)
            {
                using SKBitmap bitmap = SKBitmap.Decode(imagePaths[0]);
                return SKImage.FromBitmap(bitmap);
            }

            return MergeImages(imagePaths);
        }

        private static SKImage MergeImages(string[] imagePaths)
        {
            var imageIndexes = new Dictionary<string, (int, int)>();

            foreach (string image in imagePaths)
            {
                var indexes = GetImageIndexes(image);
                imageIndexes.Add(image, indexes);
            }

            var rowIndexes = imageIndexes.Values.Select(i => i.Item1).ToArray();
            var colIndexes = imageIndexes.Values.Select(i => i.Item2).ToArray();

            var minRowIndex = rowIndexes.Min();
            var minColIndex = colIndexes.Min();
            var maxRowIndex = rowIndexes.Max();
            var maxColIndex = colIndexes.Max();

            var rowLength = 1 + maxRowIndex - minRowIndex;
            var colLength = 1 + maxColIndex - minColIndex;

            var imageSize = GetImageSize(imagePaths[0]);
            var resultWidth = rowLength * imageSize.Item1;
            var resultHeigth = colLength * imageSize.Item2;

            using var surface = SKSurface.Create(new SKImageInfo(resultWidth, resultHeigth));
            using var canvas = surface.Canvas;

            canvas.Clear(SKColors.White);
            foreach (var image in imageIndexes)
            {
                var imageWidth = imageSize.Item1;
                var imageHeight = imageSize.Item2;
                var imageRowIndex = image.Value.Item1;
                var imageColIndex = image.Value.Item2;

                using var bitmap = SKBitmap.Decode(image.Key);


                var offsetX = (imageRowIndex - minRowIndex) * imageWidth;
                var offsetY = (imageColIndex - minColIndex) * imageHeight;
                canvas.DrawBitmap(bitmap, SKRect.Create(offsetX, offsetY, imageWidth, imageHeight));
            }

            return surface.Snapshot();
        }

        private static(int, int) GetImageSize(string imagePath)
        {
            using var bitmap = SKBitmap.Decode(imagePath);
            return (bitmap.Width, bitmap.Height);
        }

        private static (int, int) GetImageIndexes(string imagePath)
        {
            var fileName = Path.GetFileName(imagePath);
            var parts = fileName.Split('.').ToArray();
            var colIndex = int.Parse(parts[1]);
            var rowIndex = int.Parse(parts[2]);
            return (rowIndex, colIndex);
        }
    }
}