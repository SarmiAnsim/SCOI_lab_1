using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Drawing;
using System.Data;
using System.Threading;
using System.Runtime.InteropServices;

namespace SCOI_lab_1
{
    class MyImage
    {
        [DllImport("HightSpeedImageProc.dll", EntryPoint = "ChangeBytes", CallingConvention = CallingConvention.StdCall)]
        static extern void ChangeBytes(byte[] basis_bgrAValues, byte[] supplement_bgrAValues, int count, int action);

        [DllImport("HightSpeedImageProc.dll", EntryPoint = "FuncChangeBytes", CallingConvention = CallingConvention.StdCall)]
        static extern void FuncChangeBytes(byte[] AbgrValues, int count, int[] func, bool flag, int[] out_GD);

        [DllImport("HightSpeedImageProc.dll", EntryPoint = "GetGraphData", CallingConvention = CallingConvention.StdCall)]
        static extern void GetGraphData(byte[] bgrAValues, int count, int[] out_GD);

        [DllImport("HightSpeedImageProc.dll", EntryPoint = "AverageBrightness", CallingConvention = CallingConvention.StdCall)]
        static extern void AverageBrightness(byte[] bgrAValues, int count, int[] out_t);

        [DllImport("HightSpeedImageProc.dll", EntryPoint = "OtsuCriterion", CallingConvention = CallingConvention.StdCall)]
        static extern void OtsuCriterion(int[] L, int count, int[] out_t);

        [DllImport("HightSpeedImageProc.dll", EntryPoint = "GlobalBinarize", CallingConvention = CallingConvention.StdCall)]
        static extern void GlobalBinarize(byte[] bgrAValues, int count, int t);

        [DllImport("HightSpeedImageProc.dll", EntryPoint = "LocalBinarize", CallingConvention = CallingConvention.StdCall)]
        static extern void LocalBinarize(byte[] bgrAValues, int[] size, int a, float k, int version);

        [DllImport("HightSpeedImageProc.dll", EntryPoint = "GetGauss", CallingConvention = CallingConvention.StdCall)]
        public static extern void GetGauss(double[] out_val, double sig, int a, int b);

        [DllImport("HightSpeedImageProc.dll", EntryPoint = "LineFilter", CallingConvention = CallingConvention.StdCall)]
        static extern void LineFilter(byte[] bgrAValues, int[] size, double[] M, int a, int b, double[] test);

        [DllImport("HightSpeedImageProc.dll", EntryPoint = "MedianFilter", CallingConvention = CallingConvention.StdCall)]
        static extern void MedianFilter(byte[] bgrAValues, int[] size, int a, int b, double[] test);
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
        public static DataTable CPU_BarGraphData(Image image)
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

            int[] GD = new int[256];
            GetGraphData(bgrAValues, bytes, GD);
            for (int i = 0; i < 256; ++i)
            {
                GraphData.Rows[i][1] = GD[i];
            }

            Bimage.UnlockBits(bmpData);
            Bimage.Dispose();

            return GraphData;
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
            byte[] AbgrValues = new byte[bytes];

            System.Runtime.InteropServices.Marshal.Copy(ptr, AbgrValues, 0, bytes);

            for (int counter = 0; counter < AbgrValues.Length; counter += 4)
            {
                int c = (AbgrValues[counter] + AbgrValues[counter + 1] + AbgrValues[counter + 2]) / 3;
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
            byte[] AbgrValues = new byte[bytes];

            System.Runtime.InteropServices.Marshal.Copy(ptr, AbgrValues, 0, bytes);

            foreach (DataRow row in GraphData.Rows)
                row[1] = 0;

            for (int counter = 1; counter < AbgrValues.Length; counter += 4)
            {
                int c = (AbgrValues[counter] + AbgrValues[counter + 1] + AbgrValues[counter + 2]) / 3;
                GraphData.Rows[c][1] = (int)GraphData.Rows[c][1] + 1;
            }

            Bimage.UnlockBits(bmpData);
            Bimage.Dispose();
        }
        public static (DataTable, Image) CPP_ProcessAndBarGraphData(Image image, List<int> func, CancellationToken token)
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
            image.Dispose();
            Rectangle rect = new Rectangle(0, 0, Bimage.Width, Bimage.Height);
            BitmapData bmpData =
                Bimage.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            IntPtr ptr = bmpData.Scan0;

            int bytes = Math.Abs(bmpData.Stride) * Bimage.Height;
            byte[] AbgrValues = new byte[bytes];

            System.Runtime.InteropServices.Marshal.Copy(ptr, AbgrValues, 0, bytes);

            int[] GD = new int[256];
            FuncChangeBytes(AbgrValues, bytes, func.ToArray(), token.IsCancellationRequested, GD);
            for(int i = 0; i < 256; ++i)
            {
                GraphData.Rows[i][1] = GD[i];
            }

            System.Runtime.InteropServices.Marshal.Copy(AbgrValues, 0, ptr, bytes);

            Bimage.UnlockBits(bmpData);

            return (GraphData, Bimage);
        }
        public static (DataTable, Image) ProcessAndBarGraphData(Image image, List<int> func, CancellationToken token)
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
            image.Dispose();
            Rectangle rect = new Rectangle(0, 0, Bimage.Width, Bimage.Height);
            BitmapData bmpData =
                Bimage.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            IntPtr ptr = bmpData.Scan0;

            int bytes = Math.Abs(bmpData.Stride) * Bimage.Height;
            byte[] AbgrValues = new byte[bytes];

            System.Runtime.InteropServices.Marshal.Copy(ptr, AbgrValues, 0, bytes);

            for (int counter = 0; counter < AbgrValues.Length && !token.IsCancellationRequested; counter += 4)
            {
                AbgrValues[counter] = (byte)func[AbgrValues[counter]];
                AbgrValues[counter + 1] = (byte)func[AbgrValues[counter + 1]];
                AbgrValues[counter + 2] = (byte)func[AbgrValues[counter + 2]];

                int c = (AbgrValues[counter] + AbgrValues[counter + 1] + AbgrValues[counter + 2]) / 3;
                GraphData.Rows[c][1] = (int)GraphData.Rows[c][1] + 1;
            }
            System.Runtime.InteropServices.Marshal.Copy(AbgrValues, 0, ptr, bytes);

            Bimage.UnlockBits(bmpData);

            return (GraphData, Bimage);
        }
        public static Image CPP_GlobalBinarize(Image image, int version)
        {
            Bitmap Bimage = new Bitmap(image);
            Rectangle rect = new Rectangle(0, 0, Bimage.Width, Bimage.Height);
            BitmapData bmpData =
                Bimage.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            IntPtr ptr = bmpData.Scan0;

            int bytes = Math.Abs(bmpData.Stride) * Bimage.Height;
            byte[] bgrAValues = new byte[bytes];

            System.Runtime.InteropServices.Marshal.Copy(ptr, bgrAValues, 0, bytes);

            int[] t = new int[1];
            if(version == 0)
                AverageBrightness(bgrAValues, bytes, t);
            else
            {
                int[] GD = new int[256];
                GetGraphData(bgrAValues, bytes, GD);
                OtsuCriterion(GD, bytes / 4, t);
            }

            GlobalBinarize(bgrAValues, bytes, t[0]);

            System.Runtime.InteropServices.Marshal.Copy(bgrAValues, 0, ptr, bytes);

            Bimage.UnlockBits(bmpData);

            return Bimage;
        }
        public static Image CPP_LocalBinarize(Image image, int version, int a, float k)
        {
            Bitmap Bimage = new Bitmap(image);
            Rectangle rect = new Rectangle(0, 0, Bimage.Width, Bimage.Height);
            BitmapData bmpData =
                Bimage.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            IntPtr ptr = bmpData.Scan0;

            int bytes = Math.Abs(bmpData.Stride) * Bimage.Height;
            byte[] bgrAValues = new byte[bytes];

            System.Runtime.InteropServices.Marshal.Copy(ptr, bgrAValues, 0, bytes);

            LocalBinarize(bgrAValues, new int[2] { Bimage.Height, Bimage.Width }, a, k, version);

            System.Runtime.InteropServices.Marshal.Copy(bgrAValues, 0, ptr, bytes);

            Bimage.UnlockBits(bmpData);

            return Bimage;
        }
        public static Image CPP_LineFilter(Image image, List<double> M, int a, int b)
        {
            Bitmap Bimage = new Bitmap(image);
            Rectangle rect = new Rectangle(0, 0, Bimage.Width, Bimage.Height);
            BitmapData bmpData =
                Bimage.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            IntPtr ptr = bmpData.Scan0;

            int bytes = Math.Abs(bmpData.Stride) * Bimage.Height;
            byte[] bgrAValues = new byte[bytes];

            System.Runtime.InteropServices.Marshal.Copy(ptr, bgrAValues, 0, bytes);

            double[] test = new double[Bimage.Height * Bimage.Width];

            LineFilter(bgrAValues, new int[2] { Bimage.Height, Bimage.Width }, M.ToArray(), a, b, test);

            System.Runtime.InteropServices.Marshal.Copy(bgrAValues, 0, ptr, bytes);

            Bimage.UnlockBits(bmpData);

            return Bimage;
        }
        public static Image CPP_MedianFilter(Image image, int a, int b)
        {
            Bitmap Bimage = new Bitmap(image);
            Rectangle rect = new Rectangle(0, 0, Bimage.Width, Bimage.Height);
            BitmapData bmpData =
                Bimage.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            IntPtr ptr = bmpData.Scan0;

            int bytes = Math.Abs(bmpData.Stride) * Bimage.Height;
            byte[] bgrAValues = new byte[bytes];

            System.Runtime.InteropServices.Marshal.Copy(ptr, bgrAValues, 0, bytes);

            double[] test = new double[Bimage.Height * Bimage.Width];

            MedianFilter(bgrAValues, new int[2] { Bimage.Height, Bimage.Width }, a, b, test);

            System.Runtime.InteropServices.Marshal.Copy(bgrAValues, 0, ptr, bytes);

            Bimage.UnlockBits(bmpData);

            return Bimage;
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
        public static Image CastingToGray(Image imgPic)
        {
            Bitmap bmpPic = new Bitmap(imgPic.Width, imgPic.Height);
            Graphics gfxPic = Graphics.FromImage(bmpPic);
            ColorMatrix cmxPic = new ColorMatrix();
            cmxPic.Matrix00 = 0.2125f;
            cmxPic.Matrix01 = 0.2125f;
            cmxPic.Matrix02 = 0.2125f;

            cmxPic.Matrix10 = 0.7154f;
            cmxPic.Matrix11 = 0.7154f;
            cmxPic.Matrix12 = 0.7154f;

            cmxPic.Matrix20 = 0.0721f;
            cmxPic.Matrix21 = 0.0721f;
            cmxPic.Matrix22 = 0.0721f;

            ImageAttributes iaPic = new ImageAttributes();
            iaPic.SetColorMatrix(cmxPic, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            gfxPic.DrawImage(imgPic, new Rectangle(0, 0, bmpPic.Width, bmpPic.Height), 0, 0, imgPic.Width, imgPic.Height, GraphicsUnit.Pixel, iaPic);
            gfxPic.Dispose();
            iaPic.Dispose();

            return bmpPic;
        }
        public static int Clamp(int value)
        {
            if (value < 0)
                return 0;
            if (value > 255)
                return 255;
            return value;
        }
        public void CPP_LUBitsAndChange(Image img, int action)
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
            ChangeBytes(basis_bgrAValues, supplement_bgrAValues, basis_bytes, action);

            // Copy the RGB values back to the bitmap
            System.Runtime.InteropServices.Marshal.Copy(basis_bgrAValues, 0, basis_ptr, basis_bytes);

            // Unlock the bits.
            basis_image.UnlockBits(basis_bmpData);
            supplement_image.UnlockBits(supplement_bmpData);

            supplement_image.Dispose();
            image.Dispose();
            image = basis_image;
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
