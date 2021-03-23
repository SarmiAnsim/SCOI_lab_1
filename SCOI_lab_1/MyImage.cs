using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Drawing;
using System.Data;

namespace SCOI_lab_1
{
    class MyImage
    {
        public Image image { get; set; }

        public DataTable GraphData;
        public MyImage(Image image) 
        {   
            this.image = image;
            GraphData = new DataTable("GD");
            GraphData.Columns.Add("Brightness", typeof(int));
            GraphData.Columns.Add("Amount", typeof(int));
            for(int i = 0; i < 256; ++i)
            {
                DataRow row = GraphData.NewRow();
                row[0] = i;
                row[1] = 0;
                GraphData.Rows.Add(row);
            }
        }
        public MyImage(string path) : this(Image.FromFile(path)) 
        {
            GraphData = new DataTable("GD");
            GraphData.Columns.Add("Brightness", typeof(int));
            GraphData.Columns.Add("Amount", typeof(int));
            for (int i = 0; i < 256; ++i)
            {
                DataRow row = GraphData.NewRow();
                row[0] = i;
                row[1] = 0;
                GraphData.Rows.Add(row);
            }
        }
        public static List<int> BarGraphData(Image image, bool list)
        {
            Bitmap Bimage = new Bitmap(image);
            Rectangle rect = new Rectangle(0, 0, Bimage.Width, Bimage.Height);
            BitmapData bmpData =
                Bimage.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            IntPtr ptr = bmpData.Scan0;

            int bytes = Math.Abs(bmpData.Stride) * Bimage.Height;
            byte[] bgrAValues = new byte[bytes];

            System.Runtime.InteropServices.Marshal.Copy(ptr, bgrAValues, 0, bytes);

            List<int> resilt = new List<int>(new int[256]);

            for (int counter = 1; counter < bgrAValues.Length; counter += 4)
            {
                int c = (bgrAValues[counter] + bgrAValues[counter + 1] + bgrAValues[counter + 2]) / 3;
                ++resilt[c];
            }

            Bimage.UnlockBits(bmpData);
            Bimage.Dispose();

            return resilt;
        }
        public static DataTable BarGraphData(Image image)
        {
            DataTable GraphData = new DataTable("GD");
            GraphData.Columns.Add("Brightness", typeof(int));
            GraphData.Columns.Add("Amount", typeof(int));
            for (int i = 0; i < 256; ++i)
            {
                DataRow row = GraphData.NewRow();
                row[0] = i;
                row[1] = 0;
                GraphData.Rows.Add(row);
            }

            Bitmap Bimage = new Bitmap(image);
            Rectangle rect = new Rectangle(0, 0, Bimage.Width, Bimage.Height);
            BitmapData bmpData =
                Bimage.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            IntPtr ptr = bmpData.Scan0;

            int bytes = Math.Abs(bmpData.Stride) * Bimage.Height;
            byte[] bgrAValues = new byte[bytes];

            System.Runtime.InteropServices.Marshal.Copy(ptr, bgrAValues, 0, bytes);

            for (int counter = 1; counter < bgrAValues.Length; counter += 4)
            {
                int c = (bgrAValues[counter] + bgrAValues[counter + 1] + bgrAValues[counter + 2]) / 3;
                GraphData.Rows[c][1] = (int)GraphData.Rows[c][1] + 1;
            }

            Bimage.UnlockBits(bmpData);
            Bimage.Dispose();

            return GraphData;
        }
        public void BarGraphDataUpdate()
        {
            Bitmap Bimage = new Bitmap(image);
            Rectangle rect = new Rectangle(0, 0, Bimage.Width, Bimage.Height);
            BitmapData bmpData =
                Bimage.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            IntPtr ptr = bmpData.Scan0;

            int bytes = Math.Abs(bmpData.Stride) * Bimage.Height;
            byte[] bgrAValues = new byte[bytes];

            System.Runtime.InteropServices.Marshal.Copy(ptr, bgrAValues, 0, bytes);

            foreach (DataRow row in GraphData.Rows)
                row[1] = 0;

            for (int counter = 1; counter < bgrAValues.Length; counter += 4)
            {
                int c = (bgrAValues[counter] + bgrAValues[counter + 1] + bgrAValues[counter + 2]) / 3;
                GraphData.Rows[c][1] = (int)GraphData.Rows[c][1] + 1;
            }

            Bimage.UnlockBits(bmpData);
            Bimage.Dispose();
        }
        public static Image SetImgChannelValue(Image imgPic, float imgOpac, int imgRed = 1, int imgGreen = 1, int imgBlue = 1)
        {
            Bitmap bmpPic = new Bitmap(imgPic.Width, imgPic.Height);
            Graphics gfxPic = Graphics.FromImage(bmpPic);
            ColorMatrix cmxPic = new ColorMatrix();
            cmxPic.Matrix00 = imgRed;
            cmxPic.Matrix11 = imgGreen;
            cmxPic.Matrix22 = imgBlue;
            cmxPic.Matrix33 = imgOpac;

            ImageAttributes iaPic = new ImageAttributes();
            iaPic.SetColorMatrix(cmxPic, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            gfxPic.DrawImage(imgPic, new Rectangle(0, 0, bmpPic.Width, bmpPic.Height), 0, 0, imgPic.Width, imgPic.Height, GraphicsUnit.Pixel, iaPic);
            gfxPic.Dispose();
            iaPic.Dispose();

            return bmpPic;
        }
        public static Bitmap Resize(Image img, Size newSize)
        {
            if (img.Size != newSize)
                return new Bitmap(img, newSize);
            else
                return (Bitmap)img;
        }
        public static Bitmap Resize(Image routine_img, Image resize_img)
        {
            if (routine_img.Size != resize_img.Size)
                return new Bitmap(resize_img, new Size(routine_img.Width, routine_img.Height));
            else
                return (Bitmap)resize_img;
        }
        public static int Clamp(int value)
        {
            if (value < 0)
                return 0;
            if (value > 255)
                return 255;
            return value;
        }
        public void LUBitsAndChange(Image img, int action)
        {
            Bitmap basis_image = new Bitmap(image, Math.Max(image.Width, img.Width), Math.Max(image.Height, img.Height));
            Bitmap supplement_image = new Bitmap(img, Math.Max(image.Width, img.Width), Math.Max(image.Height, img.Height));

            // Lock the bitmap's bits.  
            Rectangle basis_rect = new Rectangle(0, 0, basis_image.Width, basis_image.Height);
            BitmapData basis_bmpData =
                basis_image.LockBits(basis_rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            Rectangle supplement_rect = new Rectangle(0, 0, supplement_image.Width, supplement_image.Height);
            BitmapData supplement_bmpData =
                supplement_image.LockBits(supplement_rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            // Get the address of the first line.
            IntPtr basis_ptr = basis_bmpData.Scan0;
            IntPtr supplement_ptr = supplement_bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int basis_bytes = Math.Abs(basis_bmpData.Stride) * basis_image.Height;
            byte[] basis_bgrAValues = new byte[basis_bytes];

            int supplement_bytes = Math.Abs(supplement_bmpData.Stride) * supplement_image.Height;
            byte[] supplement_bgrAValues = new byte[supplement_bytes];

            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(basis_ptr, basis_bgrAValues, 0, basis_bytes);

            System.Runtime.InteropServices.Marshal.Copy(supplement_ptr, supplement_bgrAValues, 0, supplement_bytes);

            // Set every third value to 255. A 24bpp bitmap will look red.  
            for (int counter = 0; counter < basis_bgrAValues.Length; counter += 4)
            {
                Color first = Color.FromArgb(
                    basis_bgrAValues[counter + 3],
                    basis_bgrAValues[counter + 2],
                    basis_bgrAValues[counter + 1],
                    basis_bgrAValues[counter]);
                Color second = Color.FromArgb(
                    supplement_bgrAValues[counter + 3],
                    supplement_bgrAValues[counter + 2],
                    supplement_bgrAValues[counter + 1],
                    supplement_bgrAValues[counter]);
                Color result = first;

                switch (action)
                {
                    case 0:
                        break;
                    case 1:
                        result = Add(first, second);
                        break;
                    case 2:
                        result = Subtract(first, second);
                        break;
                    case 3:
                        result = Multiply(first, second);
                        break;
                    case 4:
                        result = AverageFrom(first, second);
                        break;
                    case 5:
                        result = Min(first, second);
                        break;
                    case 6:
                        result = Max(first, second);
                        break;
                    case 7:
                        result = second;
                        break;
                }

                basis_bgrAValues[counter + 3] = (byte)result.A;
                basis_bgrAValues[counter + 2] = (byte)result.R;
                basis_bgrAValues[counter + 1] = (byte)result.G;
                basis_bgrAValues[counter] = (byte)result.B;
            }

            // Copy the RGB values back to the bitmap
            System.Runtime.InteropServices.Marshal.Copy(basis_bgrAValues, 0, basis_ptr, basis_bytes);

            // Unlock the bits.
            basis_image.UnlockBits(basis_bmpData);
            supplement_image.UnlockBits(supplement_bmpData);

            supplement_image.Dispose();
            image.Dispose();
            image = basis_image;
        }
        public void Add(Image img)
        {
            Bitmap basis_image = new Bitmap(image, Math.Max(image.Width, img.Width), Math.Max(image.Height, img.Height));
            Bitmap supplement_image = new Bitmap(img, Math.Max(image.Width, img.Width), Math.Max(image.Height, img.Height));
            Bitmap result = new Bitmap(basis_image.Width, basis_image.Height);

            for (int j = 0; j < basis_image.Height; ++j)
                for (int i = 0; i < basis_image.Width; ++i)
                {
                    Color first = basis_image.GetPixel(i, j);
                    Color second = supplement_image.GetPixel(i, j);
                    Color resultColor = Color.FromArgb(
                        Clamp(first.A + second.A),
                        Clamp((int)(first.R * ((float)first.A / 255)) + (int)(second.R * ((float)second.A / 255))),
                        Clamp((int)(first.G * ((float)first.A / 255)) + (int)(second.G * ((float)second.A / 255))),
                        Clamp((int)(first.B * ((float)first.A / 255)) + (int)(second.B * ((float)second.A / 255))));

                    result.SetPixel(i, j, resultColor);
                }

            img.Dispose();
            basis_image.Dispose();
            supplement_image.Dispose();
            image.Dispose();

            image = result;
        }
        public Color Add(Color first, Color second)
        {
            return Color.FromArgb(
                        Clamp(first.A + second.A),
                        Clamp((int)(first.R * ((float)first.A / 255)) + (int)(second.R * ((float)second.A / 255))),
                        Clamp((int)(first.G * ((float)first.A / 255)) + (int)(second.G * ((float)second.A / 255))),
                        Clamp((int)(first.B * ((float)first.A / 255)) + (int)(second.B * ((float)second.A / 255))));
        }
        public void Subtract(Image img)
        {
            Bitmap basis_image = new Bitmap(image, Math.Max(image.Width, img.Width), Math.Max(image.Height, img.Height));
            Bitmap supplement_image = new Bitmap(img, Math.Max(image.Width, img.Width), Math.Max(image.Height, img.Height));
            Bitmap result = new Bitmap(basis_image.Width, basis_image.Height);

            for (int j = 0; j < basis_image.Height; ++j)
                for (int i = 0; i < basis_image.Width; ++i)
                {
                    Color first = basis_image.GetPixel(i, j);
                    Color second = supplement_image.GetPixel(i, j);
                    Color resultColor = Color.FromArgb(
                        Clamp(first.A + second.A),
                        Clamp((int)(first.R * ((float)first.A / 255)) - (int)(second.R * ((float)second.A / 255))),
                        Clamp((int)(first.G * ((float)first.A / 255)) - (int)(second.G * ((float)second.A / 255))),
                        Clamp((int)(first.B * ((float)first.A / 255)) - (int)(second.B * ((float)second.A / 255))));

                    result.SetPixel(i, j, resultColor);
                }

            img.Dispose();
            basis_image.Dispose();
            supplement_image.Dispose();
            image.Dispose();

            image = result;
        }
        public Color Subtract(Color first, Color second)
        {
            return Color.FromArgb(
                        Clamp(first.A + second.A),
                        Clamp((int)(first.R * ((float)first.A / 255)) - (int)(second.R * ((float)second.A / 255))),
                        Clamp((int)(first.G * ((float)first.A / 255)) - (int)(second.G * ((float)second.A / 255))),
                        Clamp((int)(first.B * ((float)first.A / 255)) - (int)(second.B * ((float)second.A / 255))));
        }
        public void Multiply(Image img)
        {
            Bitmap basis_image = new Bitmap(image, Math.Max(image.Width, img.Width), Math.Max(image.Height, img.Height));
            Bitmap supplement_image = new Bitmap(img, Math.Max(image.Width, img.Width), Math.Max(image.Height, img.Height));
            Bitmap result = new Bitmap(basis_image.Width, basis_image.Height);

            for (int j = 0; j < basis_image.Height; ++j)
                for (int i = 0; i < basis_image.Width; ++i)
                {
                    Color first = basis_image.GetPixel(i, j);
                    Color second = supplement_image.GetPixel(i, j);
                    Color resultColor = Color.FromArgb(
                        Clamp(first.A + second.A),
                        Clamp((int)((first.R * ((float)first.A / 255)) + (first.R * ((float)first.A / 255)) * (int)(second.R * ((float)second.A / 255)) / 255)),
                        Clamp((int)((first.G * ((float)first.A / 255)) + (first.G * ((float)first.A / 255)) * (int)(second.G * ((float)second.A / 255)) / 255)),
                        Clamp((int)((first.B * ((float)first.A / 255)) + (first.B * ((float)first.A / 255)) * (int)(second.B * ((float)second.A / 255)) / 255)));

                    result.SetPixel(i, j, resultColor);
                }

            img.Dispose();
            basis_image.Dispose();
            supplement_image.Dispose();
            image.Dispose();

            image = result;
        }
        public Color Multiply(Color first, Color second)
        {
            return Color.FromArgb(
                        Clamp(first.A + second.A),
                        Clamp((int)((first.R * ((float)first.A / 255)) + (first.R * ((float)first.A / 255)) * (int)(second.R * ((float)second.A / 255)) / 255)),
                        Clamp((int)((first.G * ((float)first.A / 255)) + (first.G * ((float)first.A / 255)) * (int)(second.G * ((float)second.A / 255)) / 255)),
                        Clamp((int)((first.B * ((float)first.A / 255)) + (first.B * ((float)first.A / 255)) * (int)(second.B * ((float)second.A / 255)) / 255)));
        }
        public void AverageFrom(Image img)
        {
            Bitmap basis_image = new Bitmap(image, Math.Max(image.Width, img.Width), Math.Max(image.Height, img.Height));
            Bitmap supplement_image = new Bitmap(img, Math.Max(image.Width, img.Width), Math.Max(image.Height, img.Height));
            Bitmap result = new Bitmap(basis_image.Width, basis_image.Height);

            for (int j = 0; j < basis_image.Height; ++j)
                for (int i = 0; i < basis_image.Width; ++i)
                {
                    Color first = basis_image.GetPixel(i, j);
                    Color second = supplement_image.GetPixel(i, j);
                    Color resultColor = Color.FromArgb(
                        Clamp(first.A + second.A),
                        Clamp((int)((first.R * ((float)first.A / 255)) + (int)(second.R * ((float)second.A / 255)) / 2)),
                        Clamp((int)((first.G * ((float)first.A / 255)) + (int)(second.G * ((float)second.A / 255)) / 2)),
                        Clamp((int)((first.B * ((float)first.A / 255)) + (int)(second.B * ((float)second.A / 255)) / 2)));

                    result.SetPixel(i, j, resultColor);
                }

            img.Dispose();
            basis_image.Dispose();
            supplement_image.Dispose();
            image.Dispose();

            image = result;
        }
        public Color AverageFrom(Color first, Color second)
        {
            return Color.FromArgb(
                        Clamp(first.A + second.A),
                        Clamp((int)((first.R * ((float)first.A / 255)) + (int)(second.R * ((float)second.A / 255)) / 2)),
                        Clamp((int)((first.G * ((float)first.A / 255)) + (int)(second.G * ((float)second.A / 255)) / 2)),
                        Clamp((int)((first.B * ((float)first.A / 255)) + (int)(second.B * ((float)second.A / 255)) / 2)));
        }
        public void Min(Image img)
        {
            Bitmap basis_image = new Bitmap(image, Math.Max(image.Width, img.Width), Math.Max(image.Height, img.Height));
            Bitmap supplement_image = new Bitmap(img, Math.Max(image.Width, img.Width), Math.Max(image.Height, img.Height));
            Bitmap result = new Bitmap(basis_image.Width, basis_image.Height);

            for (int j = 0; j < basis_image.Height; ++j)
                for (int i = 0; i < basis_image.Width; ++i)
                {
                    Color first = basis_image.GetPixel(i, j);
                    Color second = supplement_image.GetPixel(i, j);
                    Color resultColor = Color.FromArgb(
                        Clamp(first.A + second.A),
                        Clamp(Math.Min((int)(first.R * ((float)first.A / 255)), (int)(second.R * ((float)second.A / 255)))),
                        Clamp(Math.Min((int)(first.G * ((float)first.A / 255)), (int)(second.G * ((float)second.A / 255)))),
                        Clamp(Math.Min((int)(first.B * ((float)first.A / 255)), (int)(second.B * ((float)second.A / 255)))));

                    result.SetPixel(i, j, resultColor);
                }

            img.Dispose();
            basis_image.Dispose();
            supplement_image.Dispose();
            image.Dispose();

            image = result;
        }
        public Color Min(Color first, Color second)
        {
            return Color.FromArgb(
                        Clamp(first.A + second.A),
                        Clamp(Math.Min((int)(first.R * ((float)first.A / 255)), (int)(second.R * ((float)second.A / 255)))),
                        Clamp(Math.Min((int)(first.G * ((float)first.A / 255)), (int)(second.G * ((float)second.A / 255)))),
                        Clamp(Math.Min((int)(first.B * ((float)first.A / 255)), (int)(second.B * ((float)second.A / 255)))));
        }
        public void Max(Image img)
        {
            Bitmap basis_image = new Bitmap(image, Math.Max(image.Width, img.Width), Math.Max(image.Height, img.Height));
            Bitmap supplement_image = new Bitmap(img, Math.Max(image.Width, img.Width), Math.Max(image.Height, img.Height));
            Bitmap result = new Bitmap(basis_image.Width, basis_image.Height);

            for (int j = 0; j < basis_image.Height; ++j)
                for (int i = 0; i < basis_image.Width; ++i)
                {
                    Color first = basis_image.GetPixel(i, j);
                    Color second = supplement_image.GetPixel(i, j);
                    Color resultColor = Color.FromArgb(
                        Clamp(first.A + second.A),
                        Clamp(Math.Max((int)(first.R * ((float)first.A / 255)), (int)(second.R * ((float)second.A / 255)))),
                        Clamp(Math.Max((int)(first.G * ((float)first.A / 255)), (int)(second.G * ((float)second.A / 255)))),
                        Clamp(Math.Max((int)(first.B * ((float)first.A / 255)), (int)(second.B * ((float)second.A / 255)))));

                    result.SetPixel(i, j, resultColor);
                }

            img.Dispose();
            basis_image.Dispose();
            supplement_image.Dispose();
            image.Dispose();

            image = result;
        }
        public Color Max(Color first, Color second)
        {
            return Color.FromArgb(
                        Clamp(first.A + second.A),
                        Clamp(Math.Max((int)(first.R * ((float)first.A / 255)), (int)(second.R * ((float)second.A / 255)))),
                        Clamp(Math.Max((int)(first.G * ((float)first.A / 255)), (int)(second.G * ((float)second.A / 255)))),
                        Clamp(Math.Max((int)(first.B * ((float)first.A / 255)), (int)(second.B * ((float)second.A / 255)))));
        }
        public void Overlay(Image img)
        {
            image.Dispose();
            image = new Bitmap(img, Math.Max(image.Width, img.Width), Math.Max(image.Height, img.Height));

            img.Dispose();
        }
    }
}
