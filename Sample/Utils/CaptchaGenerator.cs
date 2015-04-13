using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace Sample.Utils
{
    public class CaptchaGenerator
    {
        public static Captcha Create()
        {
            var captcha = new Captcha();
            var rnd = new Random(Guid.NewGuid().GetHashCode());
            var text = rnd.Next(0, 9999).ToString().PadLeft(4, '0');

            captcha.Text = text;

            var bmp = ConvertTexttoImage(text, "Microsoft JhengHei", 24);

            captcha.Image = bmp;

            return captcha;
        }

        public static Bitmap ConvertTexttoImage(string txt, string fontname, int fontsize)
        {
            //creating bitmap image
            Bitmap bmp = new Bitmap(1, 1);

            //FromImage method creates a new Graphics from the specified Image.
            Graphics graphics = Graphics.FromImage(bmp);
            // Create the Font object for the image text drawing.
            Font font = new Font(fontname, fontsize);
            // Instantiating object of Bitmap image again with the correct size for the text and font.
            SizeF stringSize = graphics.MeasureString(txt, font);
            bmp = new Bitmap(bmp, (int) stringSize.Width, (int) stringSize.Height);
            graphics = Graphics.FromImage(bmp);

            /* It can also be a way
           bmp = new Bitmap(bmp, new Size((int)graphics.MeasureString(txt, font).Width, (int)graphics.MeasureString(txt, font).Height));*/

            //Draw Specified text with specified format 
            graphics.FillRectangle(new SolidBrush(Color.Black), 0, 0, bmp.Width, bmp.Height);
            graphics.DrawString(txt, font, Brushes.Red, 0, 0);
            font.Dispose();
            graphics.Flush();
            graphics.Dispose();
            return bmp; //return Bitmap Image 
        }
    }

    public class Captcha
    {
        public string Text { get; set; }
        public Bitmap Image { get; set; }
    }
}