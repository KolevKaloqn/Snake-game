using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Snake
{
    //this clas is a container for all image assets
    public static class Images
    {
        public readonly static ImageSource Empty = LoadImage("Empty.png");
        public readonly static ImageSource Body = LoadImage("Body.png");
        public readonly static ImageSource Food = LoadImage("Food.png");
        private static ImageSource LoadImage(string fileName)
        { 
            return new BitmapImage(new Uri($"Assets/{fileName}", UriKind.Relative)); 
        }

    }
}
