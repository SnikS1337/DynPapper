using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Microsoft.Win32;
using System.Windows.Threading;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Controls;
using System.Windows.Input;

namespace Wallpaper
{
    public partial class MainWindow : Window
    {
        private BitmapSource _selectedImage;
        private DispatcherTimer _animationTimer;
        private double _animationTime = 0;
        private bool _isWallpaperActive = false;
        private IntPtr _wallpaperHandle;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SystemParametersInfo(UInt32 action, UInt32 uParam, String vParam, UInt32 winIni);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        public MainWindow()
        {
            InitializeComponent();
            InitializeAnimationTimer();
        }

        private void InitializeAnimationTimer()
        {
            _animationTimer = new DispatcherTimer();
            _animationTimer.Tick += AnimationTimer_Tick;
            _animationTimer.Interval = TimeSpan.FromMilliseconds(1000.0 / 60.0); // 60 FPS default
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            if (_selectedImage == null || !_isWallpaperActive) return;

            _animationTime += 0.016; // Approximately 60 FPS
            UpdateWallpaperAnimation();
        }

        private void UpdateWallpaperAnimation()
        {
            var selectedAnimation = AnimationTypeComboBox.SelectedItem as ComboBoxItem;
            if (selectedAnimation == null) return;

            var animationType = selectedAnimation.Content.ToString();
            var fps = (int)FpsSlider.Value;
            _animationTimer.Interval = TimeSpan.FromMilliseconds(1000.0 / fps);

            // Create animated bitmap based on selected animation type
            var animatedBitmap = CreateAnimatedBitmap(_selectedImage, animationType);
            SetWallpaper(animatedBitmap);
        }

        private WriteableBitmap CreateAnimatedBitmap(BitmapSource source, string animationType)
        {
            var width = source.PixelWidth;
            var height = source.PixelHeight;
            var stride = width * 4;
            var pixels = new byte[height * stride];

            source.CopyPixels(pixels, stride, 0);

            var writeableBitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null);
            writeableBitmap.WritePixels(new Int32Rect(0, 0, width, height), pixels, stride, 0);

            switch (animationType)
            {
                case "Parallax Effect":
                    ApplyParallaxEffect(writeableBitmap);
                    break;
                case "Wave Effect":
                    ApplyWaveEffect(writeableBitmap);
                    break;
                case "Zoom Effect":
                    ApplyZoomEffect(writeableBitmap);
                    break;
                case "Custom Animation":
                    ApplyCustomAnimation(writeableBitmap);
                    break;
            }

            return writeableBitmap;
        }

        private void ApplyParallaxEffect(WriteableBitmap bitmap)
        {
            var width = bitmap.PixelWidth;
            var height = bitmap.PixelHeight;
            var pixels = new byte[height * width * 4];
            bitmap.CopyPixels(pixels, width * 4, 0);

            for (int y = 0; y < height; y++)
            {
                var offset = (int)(Math.Sin(_animationTime + y * 0.01) * 10);
                for (int x = 0; x < width; x++)
                {
                    var sourceX = (x + offset + width) % width;
                    var targetIndex = (y * width + x) * 4;
                    var sourceIndex = (y * width + sourceX) * 4;

                    pixels[targetIndex] = pixels[sourceIndex];
                    pixels[targetIndex + 1] = pixels[sourceIndex + 1];
                    pixels[targetIndex + 2] = pixels[sourceIndex + 2];
                    pixels[targetIndex + 3] = pixels[sourceIndex + 3];
                }
            }

            bitmap.WritePixels(new Int32Rect(0, 0, width, height), pixels, width * 4, 0);
        }

        private void ApplyWaveEffect(WriteableBitmap bitmap)
        {
            var width = bitmap.PixelWidth;
            var height = bitmap.PixelHeight;
            var pixels = new byte[height * width * 4];
            bitmap.CopyPixels(pixels, width * 4, 0);

            for (int y = 0; y < height; y++)
            {
                var waveOffset = (int)(Math.Sin(_animationTime * 2 + y * 0.05) * 20);
                for (int x = 0; x < width; x++)
                {
                    var sourceX = (x + waveOffset + width) % width;
                    var targetIndex = (y * width + x) * 4;
                    var sourceIndex = (y * width + sourceX) * 4;

                    pixels[targetIndex] = pixels[sourceIndex];
                    pixels[targetIndex + 1] = pixels[sourceIndex + 1];
                    pixels[targetIndex + 2] = pixels[sourceIndex + 2];
                    pixels[targetIndex + 3] = pixels[sourceIndex + 3];
                }
            }

            bitmap.WritePixels(new Int32Rect(0, 0, width, height), pixels, width * 4, 0);
        }

        private void ApplyZoomEffect(WriteableBitmap bitmap)
        {
            var width = bitmap.PixelWidth;
            var height = bitmap.PixelHeight;
            var pixels = new byte[height * width * 4];
            bitmap.CopyPixels(pixels, width * 4, 0);

            var scale = 1.0 + Math.Sin(_animationTime) * 0.1;
            var centerX = width / 2;
            var centerY = height / 2;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var dx = (x - centerX) / scale;
                    var dy = (y - centerY) / scale;
                    var sourceX = (int)(centerX + dx);
                    var sourceY = (int)(centerY + dy);

                    if (sourceX >= 0 && sourceX < width && sourceY >= 0 && sourceY < height)
                    {
                        var targetIndex = (y * width + x) * 4;
                        var sourceIndex = (sourceY * width + sourceX) * 4;

                        pixels[targetIndex] = pixels[sourceIndex];
                        pixels[targetIndex + 1] = pixels[sourceIndex + 1];
                        pixels[targetIndex + 2] = pixels[sourceIndex + 2];
                        pixels[targetIndex + 3] = pixels[sourceIndex + 3];
                    }
                }
            }

            bitmap.WritePixels(new Int32Rect(0, 0, width, height), pixels, width * 4, 0);
        }

        private void ApplyCustomAnimation(WriteableBitmap bitmap)
        {
            var width = bitmap.PixelWidth;
            var height = bitmap.PixelHeight;
            var pixels = new byte[height * width * 4];
            bitmap.CopyPixels(pixels, width * 4, 0);

            var centerX = width / 2;
            var centerY = height / 2;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var dx = x - centerX;
                    var dy = y - centerY;
                    var distance = Math.Sqrt(dx * dx + dy * dy);
                    var angle = Math.Atan2(dy, dx);

                    var wave = Math.Sin(distance * 0.1 - _animationTime * 2) * 10;
                    var sourceX = (int)(x + Math.Cos(angle) * wave);
                    var sourceY = (int)(y + Math.Sin(angle) * wave);

                    if (sourceX >= 0 && sourceX < width && sourceY >= 0 && sourceY < height)
                    {
                        var targetIndex = (y * width + x) * 4;
                        var sourceIndex = (sourceY * width + sourceX) * 4;

                        pixels[targetIndex] = pixels[sourceIndex];
                        pixels[targetIndex + 1] = pixels[sourceIndex + 1];
                        pixels[targetIndex + 2] = pixels[sourceIndex + 2];
                        pixels[targetIndex + 3] = pixels[sourceIndex + 3];
                    }
                }
            }

            bitmap.WritePixels(new Int32Rect(0, 0, width, height), pixels, width * 4, 0);
        }

        private void SetWallpaper(WriteableBitmap bitmap)
        {
            try
            {
                var tempPath = System.IO.Path.Combine(
                    System.IO.Path.GetTempPath(),
                    $"wallpaper_{DateTime.Now.Ticks}.png"
                );

                using (var fileStream = new System.IO.FileStream(tempPath, System.IO.FileMode.Create))
                {
                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bitmap));
                    encoder.Save(fileStream);
                }

                SystemParametersInfo(0x0014, 0, tempPath, 0x01 | 0x02);
                System.IO.File.Delete(tempPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error setting wallpaper: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    _selectedImage = new BitmapImage(new Uri(openFileDialog.FileName));
                    PreviewImage.Source = _selectedImage;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ApplyWallpaper_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedImage == null)
            {
                MessageBox.Show("Please select an image first.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _isWallpaperActive = true;
            _animationTimer.Start();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            _animationTimer.Stop();
            base.OnClosing(e);
        }
    }
} 