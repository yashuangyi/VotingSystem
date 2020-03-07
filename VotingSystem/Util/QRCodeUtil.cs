using QRCoder;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace VotingSystem.Util
{
    /// <summary>
    /// 二维码生成.
    /// </summary>
    public class QRCodeUtil
    {
        /// <summary>
        /// 生成设备编号，二维码图片，并返回链接地址.
        /// </summary>
        /// <param name="webRootPath">.</param>
        /// <param name="CodeStr">..</param>
        /// <param name="instrumentName">...</param>
        /// <returns>....</returns>
        public static string QRCode(string webRootPath, string CodeStr, string instrumentName, string imgName)
        {
            if (string.IsNullOrWhiteSpace(webRootPath) || string.IsNullOrWhiteSpace(CodeStr) || string.IsNullOrWhiteSpace(instrumentName))
            {
                return string.Empty;
            }

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(CodeStr, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);

            Bitmap qrCodeImage = qrCode.GetGraphic(20);

            Bitmap newImg = new Bitmap(980, 1180);

            Graphics gra = Graphics.FromImage(newImg);

            gra.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            gra.FillRectangle(new SolidBrush(Color.White), 0, 0, 980, 1180);
            gra.DrawImage(qrCodeImage, 0, 0, qrCodeImage.Width, qrCodeImage.Height);

            SolidBrush sbrush = new SolidBrush(Color.Black);

            if (instrumentName.Length < 13)
            {
                gra.DrawString(instrumentName, new Font(SystemFonts.DefaultFont.Name, 40, FontStyle.Bold), sbrush, new PointF(300, 1000));
            }
            else
            {
                gra.DrawString(instrumentName, new Font(SystemFonts.DefaultFont.Name, 40, FontStyle.Bold), sbrush, new PointF(80, 1000));
            }

            var fileFolder = Path.Combine(webRootPath, "QRCode");

            if (!Directory.Exists(fileFolder))
            {
                Directory.CreateDirectory(fileFolder);
            }

            string fileName = fileFolder + Path.DirectorySeparatorChar + imgName + ".png";
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            newImg.Save(fileName, ImageFormat.Jpeg);
            newImg.Dispose();
            qrCodeImage.Dispose();

            return "/QRCode/" + imgName + ".png";
        }

        /// <summary>
        /// 生成开箱，二维码图片，并返回链接地址,文件路径.
        /// </summary>
        /// <param name="webRootPath">.</param>
        /// <param name="CodeStr">..</param>
        /// <param name="path">文件路径.</param>
        /// <returns>...</returns>
        public static string OpenQRCode(string webRootPath, string CodeStr, out string path)
        {
            path = string.Empty;

            if (string.IsNullOrWhiteSpace(webRootPath) || string.IsNullOrWhiteSpace(CodeStr))
            {
                return string.Empty;
            }

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(CodeStr, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);

            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            string dateStr = DateTime.Now.ToString("yyyyMMdd");
            var fileFolder = Path.Combine(webRootPath, "OpenQRCode", dateStr);

            if (!Directory.Exists(fileFolder))
            {
                Directory.CreateDirectory(fileFolder);
            }

            path = fileFolder + Path.DirectorySeparatorChar + CodeStr + ".png";

            qrCodeImage.Save(path, ImageFormat.Png);

            qrCodeImage.Dispose();

            return "/OpenQRCode/" + dateStr + "/" + CodeStr + ".png";
        }

        /// <summary>
        /// 生成开箱，二维码图片，并返回byte数组.
        /// </summary>
        /// <param name="CodeStr">.</param>
        /// <returns>..</returns>
        public static byte[] OpenQRCodeBytes(string CodeStr)
        {
            if (string.IsNullOrWhiteSpace(CodeStr))
            {
                return null;
            }

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(CodeStr, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);

            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            Stream stream = new MemoryStream();
            qrCodeImage.Save(stream, ImageFormat.Png);
            qrCode.Dispose();
            byte[] buffer = new byte[stream.Length];

            if (stream.CanWrite)
            {
                stream.Write(buffer, 0, buffer.Length);
            }

            stream.Dispose();

            return buffer;
        }
    }
}