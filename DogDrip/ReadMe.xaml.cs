using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;

namespace DogDrip
{
    /// <summary>
    /// ReadMe.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ReadMe : Window
    {
        public ReadMe()
        {
            InitializeComponent();

            System.Drawing.Bitmap img = DogDrip.Properties.Resources.image;

            MemoryStream imgStream = new MemoryStream();

            img.Save(imgStream, System.Drawing.Imaging.ImageFormat.Bmp);

            imgStream.Seek(0, SeekOrigin.Begin);

            BitmapFrame newimg = BitmapFrame.Create(imgStream);

            image1.Source = newimg;
        }
    }
}
