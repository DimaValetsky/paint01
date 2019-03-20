using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls.Primitives;
using System.IO;

namespace MyPaint01
{
    
    enum DrawType { nothing, pencil, brush, line, ellipse, rectangle, triangle, arrow, heart, fill, erase, text};

    
    class Document  : Window
    {
        public Canvas canvas;  
        public DrawType drawType;
        public Document(Canvas c)
        {
            drawType = DrawType.nothing;
            canvas = c;
        }

        public void DrawCapture(Shape shape)
        {
            double[] dashes = { 2, 2 };
            shape.StrokeDashArray = new System.Windows.Media.DoubleCollection(dashes);
           
            canvas.Children.Add(shape);
        }

        public void DrawShape(ContentControl control, int outline)
        {
            canvas.Children.RemoveAt(canvas.Children.Count - 1);
            RefreshCanvas();
            if (outline == 1)
            {
                ((Shape)control.Content).StrokeDashArray = null;
            }
            else if (outline == 2)
            {
                double[] dashes = { 4, 4 };
                ((Shape)control.Content).StrokeDashArray = new System.Windows.Media.DoubleCollection(dashes);
            }
            else
            {
                double[] dashes = { 4, 1, 4, 1 };
                
                ((Shape)control.Content).StrokeDashArray = new System.Windows.Media.DoubleCollection(dashes);
            }
            
            canvas.Children.Add(control);
       
        }

        public void DrawShape(Shape shape, int outline)
        {
            RefreshCanvas();
            if (outline == 1)
            {
                shape.StrokeDashArray = null;
            }
            else if (outline == 2)
            {
                double[] dashes = { 4, 4 };
                shape.StrokeDashArray = new System.Windows.Media.DoubleCollection(dashes);
            }
            else
            {
                double[] dashes = { 4, 1, 4, 1 };
                
                shape.StrokeDashArray = new System.Windows.Media.DoubleCollection(dashes);
            }
            
            canvas.Children.Add(shape);
            
        }

        

     

        public void RemoveShape(ContentControl shape)
        {
            canvas.Children.Remove(shape);
        }

        public string OpenFile()
        {
            canvas.Children.Clear();
            System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
            dlg.Title = "Choose an image file";
            dlg.Filter = "Bitmap files (*.bmp)|*.bmp|JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|PNG (*.png)|*.png|All files (*.*)|*.*";
            try
            {
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    ImageBrush brush = new ImageBrush();
                    
                    
                    BitmapImage img = new BitmapImage(new Uri(dlg.FileName, UriKind.Relative));
                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(img));
                    string tempPath = CreateTempFile();
                    using (var stream = System.IO.File.Open(tempPath, System.IO.FileMode.Open))
                    {
                        encoder.Save(stream);
                        stream.Close();
                    }
                    BitmapImage temp = new BitmapImage(new Uri(tempPath, UriKind.Relative));
                    brush.ImageSource = temp;
                    
                    
                    canvas.Background = brush;
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error: Could not read file from disk.\nOriginal error: " + ex.Message);
            }
            return dlg.FileName;
        }


        public void SaveFile(string path)
        {
            Rect bounds = VisualTreeHelper.GetDescendantBounds(canvas);
            double dpi = 96d;


            RenderTargetBitmap rtb = new RenderTargetBitmap((int)bounds.Width, (int)bounds.Height, dpi, dpi, System.Windows.Media.PixelFormats.Default);


            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext dc = dv.RenderOpen())
            {
                VisualBrush vb = new VisualBrush(canvas);
                dc.DrawRectangle(vb, null, new Rect(new System.Windows.Point(), bounds.Size));
            }
            rtb.Render(dv);
            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(rtb));

            try
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream();

                pngEncoder.Save(ms);

                ms.Close();
                ms.Dispose();
                System.Windows.Forms.SaveFileDialog dlg = new System.Windows.Forms.SaveFileDialog();
                dlg.Title = "Save as";
                dlg.Filter = "Bitmap files (*.bmp)|*.bmp|JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|PNG (*.png)|*.png|All files (*.*)|*.*";
                if (path == null)
                {
                    if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {

                        string fileName = dlg.FileName;
                        System.IO.File.WriteAllBytes(fileName, ms.ToArray());
                    }
                }
                else
                {
                    System.IO.File.WriteAllBytes(path, ms.ToArray());
                }
                
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string CreateTempFile()
        {
            string fileName = string.Empty;

            try
            {
                fileName = System.IO.Path.GetTempFileName();

               
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(fileName);

                
                fileInfo.Attributes = System.IO.FileAttributes.Temporary;
               
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to create tempfile\nDetail: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return fileName;
        }

        public UIElement GetHitElement(System.Drawing.Point hittedPoint)
        {
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            Shape temp;
            for (int i = canvas.Children.Count - 1; i >= 0; i--)
            {
                temp = (Shape)canvas.Children[i];
                System.Drawing.Size childSize = new System.Drawing.Size((int)temp.Width, (int)temp.Height);
                System.Drawing.Point childPos = new System.Drawing.Point();
                childPos.X = (int)canvas.Children[i].TranslatePoint(new System.Windows.Point(0, 0), canvas).X;
                childPos.Y = (int)canvas.Children[i].TranslatePoint(new System.Windows.Point(0, 0), canvas).Y;
                path.AddRectangle(new System.Drawing.Rectangle(childPos, childSize));
                if (path.IsVisible(hittedPoint))
                {
                    return canvas.Children[i];
                }
            }
            return null;
        }

       

        public void InsertText(ContentControl control)
        {
            canvas.Children.Add(control);  
        }

        

        private ImageSource BitmapToImageSource(Bitmap bm)
        {
            System.Windows.Media.Imaging.BitmapSource b = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bm.GetHbitmap(), IntPtr.Zero, System.Windows.Int32Rect.Empty, BitmapSizeOptions.FromWidthAndHeight(bm.Width, bm.Height));
            return b;
        }

        

        public Bitmap CanvasToBitmap(Canvas cv)
        {
            Bitmap bm;
            Rect bounds = VisualTreeHelper.GetDescendantBounds(cv);
            double dpi = 96d;
            RenderTargetBitmap renderBitmap = new RenderTargetBitmap((int)bounds.Width, (int)bounds.Height, dpi, dpi, System.Windows.Media.PixelFormats.Default);


            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext dc = dv.RenderOpen())
            {
                VisualBrush vb = new VisualBrush(cv);
                dc.DrawRectangle(vb, null, new Rect(new System.Windows.Point(), bounds.Size));
            }
            renderBitmap.Render(dv);

            MemoryStream stream = new MemoryStream();
            BitmapEncoder encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
            encoder.Save(stream);
            bm = new System.Drawing.Bitmap(stream);
            return bm;
        }
        
        public void RefreshCanvas()
        {
            System.Windows.Controls.Image img = new System.Windows.Controls.Image();
            img.Width = canvas.ActualWidth;
            img.Height = canvas.ActualHeight;
            img.Source = BitmapToImageSource(CanvasToBitmap(canvas));
            canvas.Children.Clear();
            canvas.Children.Add(img);
        }
    }
}
