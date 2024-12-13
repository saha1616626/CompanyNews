using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CompanyNews.Helpers
{
    /// <summary>
    /// Класс для обработки изображений
    /// </summary>
    public static class WorkingWithImage
    {
        public static CroppedBitmap? AddImageFromDialogBox(int sizeImage)
        {
			// Создание диалогового окна выбора файла
			Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
			openFileDialog.Filter = "Изображения (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";

            // Состояние диалогового окна
			if (openFileDialog.ShowDialog() == true)
            {
				// Загрузка изображения из выбранного файла
				byte[] image;
				using (var fileSream = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
				{
					image = new byte[fileSream.Length];
					fileSream.Read(image, 0, (int)fileSream.Length);
				}

				// Проверка полученного изображения
				if (image.Length > 0 || image != null)
				{
					// Изменение размера изображения
					return ResizingPhotos(image, sizeImage);
				}
			}

			return null;
		}

		// Обрезаем изображение под нужные параметры
		public static CroppedBitmap ResizingPhotos(byte[] image, int desiredSize)
		{
			// создание BitmapImage из загруженного изображения
			BitmapImage selectedImage = new BitmapImage();
			selectedImage.BeginInit();
			selectedImage.StreamSource = new MemoryStream(image);
			selectedImage.EndInit();

			int width = selectedImage.PixelWidth; // получаем ширину
			int height = selectedImage.PixelHeight; // получаем высоту
			double aspectRatio = (double)width / height;

			if (aspectRatio > 1) // если ширина больше высоты, то мы устанавливаем нужный размер высоты, а ширину исходя из пропорций
			{
				width = desiredSize;
				height = (int)(desiredSize / aspectRatio);
			}
			else // если высота больше ширины, то высоте ставим нужный размер, а ширину выставляем исходя из пропорций
			{
				height = desiredSize;
				width = (int)(desiredSize * aspectRatio);
			}

			// масштабируем изображение
			TransformedBitmap resizedBitmap = new TransformedBitmap(selectedImage, new ScaleTransform(
				(double)width / selectedImage.PixelWidth,
				(double)height / selectedImage.PixelHeight));

			// обрезаем изображение под квадрат
			int croppedSize = Math.Min(resizedBitmap.PixelWidth, resizedBitmap.PixelHeight); // находим самую корткую сторону
																							 // получаем положение центральной точки в изображении для ровного обрезания
			int x = (resizedBitmap.PixelWidth - croppedSize) / 2;
			int y = (resizedBitmap.PixelHeight - croppedSize) / 2;

			CroppedBitmap croppedBitmap = new CroppedBitmap(resizedBitmap, new Int32Rect(x, y, croppedSize, croppedSize));

			return croppedBitmap;
		}

		/// <summary>
		/// Конвертация изображения для записи в базу данных
		/// </summary>
		public static byte[] ConvertingImageForWritingDatabase(CroppedBitmap croppedBitmap)
		{
			// Преобразование CroppedBitmap в BitmapSource
			BitmapSource bitmapSource = croppedBitmap;

			// Создание MemoryStream для записи данных
			using (MemoryStream ms = new MemoryStream())
			{
				// Сохранение BitmapSource в MemoryStream в формате PNG
				PngBitmapEncoder encoder = new PngBitmapEncoder();
				encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
				encoder.Save(ms);

				return ms.ToArray(); // получаем массив байтов из MemoryStream
			}
		}

		/// <summary>
		/// Преобразование изображения в CroppedBitmap
		/// </summary>
		public static CroppedBitmap? ConvertImageCroppedBitmap(BitmapImage bitmapImage, int desiredSize)
		{
			if (bitmapImage == null) { return null; }

			// создание BitmapImage из загруженного изображения
			BitmapImage selectedImage = new BitmapImage();
			selectedImage = bitmapImage;

			int width = selectedImage.PixelWidth; // получаем ширину
			int height = selectedImage.PixelHeight; // получаем высоту
			double aspectRatio = (double)width / height;

			if (aspectRatio > 1) // если ширина больше высоты, то мы устанавливаем нужный размер высоты, а ширину исходя из пропорций
			{
				width = desiredSize;
				height = (int)(desiredSize / aspectRatio);
			}
			else // если высота больше ширины, то высоте ставим нужный размер, а ширину выставляем исходя из пропорций
			{
				height = desiredSize;
				width = (int)(desiredSize * aspectRatio);
			}

			// масштабируем изображение
			TransformedBitmap resizedBitmap = new TransformedBitmap(selectedImage, new ScaleTransform(
				(double)width / selectedImage.PixelWidth,
				(double)height / selectedImage.PixelHeight));

			// обрезаем изображение под квадрат
			int croppedSize = Math.Min(resizedBitmap.PixelWidth, resizedBitmap.PixelHeight); // находим самую корткую сторону
																							 // получаем положение центральной точки в изображении для ровного обрезания
			int x = (resizedBitmap.PixelWidth - croppedSize) / 2;
			int y = (resizedBitmap.PixelHeight - croppedSize) / 2;

			CroppedBitmap croppedBitmap = new CroppedBitmap(resizedBitmap, new Int32Rect(x, y, croppedSize, croppedSize));

			return croppedBitmap;
		}

	}
}
