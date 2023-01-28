using SkiaSharp;
using System.Text.RegularExpressions;

namespace ImageMerging
{
    /*
    * Напишите консольное приложение, объединяющее группу изображений в одно большое. 
    * На входе консоли - каталог и необязательные начальные и конечные индексы изображения. (в реализаци индексы опущены, нужно уточнение)
    * В каталоге изображения в виде Image.1.1.jpg. Image 1.2.jpg Image.3.1... (Image.<строка>.<столбец>.jpg)
    * Если изображение отсутствует - на его месте белый квдарат. Входные изображения имеют одинаковый размер. 
    * (Сделайте его на .Net Core 3.1 или выше, проверьте работу под Linux) (под Linux не проверял)
    */
    public class Program
    {
        const int DefaultImageHeight = 600;
        const int DefaultImageWidth = 800;

        private static void Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                    throw new("Необходимо указать путь к папке с изображениями");

                var path = args[0].Trim();
                var filePaths = GetFileArray(path);
                var imagePaths = FilterImageArray(filePaths);

                using var resultImage = CreateImage(imagePaths);

                var resultName = GenerateResultName();
                var outPath = Path.Combine(path, resultName); //Возможно, стоило создать отдельную папку для финальных изображений

                SaveImage(resultImage, outPath);
                Console.WriteLine($"Создано изображение {outPath}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        //Получение списка файлов из директории
        private static string[] GetFileArray(string path)
        {
            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException($"Папки {path} не существует");

            return Directory.GetFiles(path);
        }

        //Получение из списка файлов изображений формата Image.row.col.jpg
        private static string[] FilterImageArray(string[] images)
        {
            var correctFileName = new Regex(@".*\\Image\.\d+\.\d+\.(jpe?g|JPE?G)$");
            var result = images.Where(i => correctFileName.IsMatch(i)).ToArray();
            return result;
        }

        //Создание финального изображения
        private static SKImage CreateImage(string[] imagePaths)
        {
            //Если в папке нет подходящих изображений, создаётся белая картинка
            if (imagePaths.Length == 0)
                return CreateWhiteImage(DefaultImageWidth, DefaultImageHeight);

            //Если в папке одно подходящее изображение, создаётся его копия
            if (imagePaths.Length == 1)
            {
                using SKBitmap bitmap = SKBitmap.Decode(imagePaths[0]);
                return SKImage.FromBitmap(bitmap);
            }

            //Иначе катинки объединяются
            return MergeImages(imagePaths);
        }

        //Создание белого изображения
        private static SKImage CreateWhiteImage(int width, int height)
        {
            using var tempSurface = SKSurface.Create(new SKImageInfo(width, height));
            using var canvas = tempSurface.Canvas;
            canvas.Clear(SKColors.White);

            return tempSurface.Snapshot();
        }

        //Объединение изображений
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

            //Расчёт количества картинок в каждом ряду и столбце
            var rowLength = 1 + maxRowIndex - minRowIndex; //+1 т.к. по заданию картинки начинаются минимум с 1
            var colLength = 1 + maxColIndex - minColIndex;

            //Расчёт размеров финального изображения при учёте, что у всех картинок одинаковые размеры
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

                //Расчёт отступов от краёв финального изображения в пикселях
                var offsetX = (imageRowIndex - minRowIndex) * imageWidth;
                var offsetY = (imageColIndex - minColIndex) * imageHeight;

                using var bitmap = SKBitmap.Decode(image.Key);
                canvas.DrawBitmap(bitmap, SKRect.Create(offsetX, offsetY, imageWidth, imageHeight));
            }

            return surface.Snapshot();
        }

        //Получение индексов из названия файла
        private static (int, int) GetImageIndexes(string imagePath)
        {
            var fileName = Path.GetFileName(imagePath);
            var parts = fileName.Split('.').ToArray();
            var colIndex = int.Parse(parts[1]);
            var rowIndex = int.Parse(parts[2]);
            return (rowIndex, colIndex);
        }

        //Получение размеров изображения
        private static (int, int) GetImageSize(string imagePath)
        {
            using var bitmap = SKBitmap.Decode(imagePath);
            return (bitmap.Width, bitmap.Height);
        }

        //Генерация названия финального изображения
        private static string GenerateResultName()
        {
            return "Result" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
        }

        //Сохранение изображения
        private static void SaveImage(SKImage image, string path)
        {
            using SKData encoded = image.Encode(SKEncodedImageFormat.Jpeg, 100);
            using Stream outFile = File.OpenWrite(path);
            encoded.SaveTo(outFile);
        }
        
    }
}