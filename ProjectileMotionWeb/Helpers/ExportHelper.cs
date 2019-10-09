using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using ProjectileMotionSource.Saving;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace ProjectileMotionWeb.Helpers
{
    public class ExportHelper
    {
        public ExportHelper(string downloadFileName, string contentType, MemoryStream dataStream)
        {
            DownloadFileName = downloadFileName;
            ContentType = contentType;
            DataFileContents = dataStream.ToArray();
        }

        public ExportHelper(string downloadFileName, string contentType, byte[] dataFileContents)
        {
            DownloadFileName = downloadFileName;
            ContentType = contentType;
            DataFileContents = dataFileContents;
        }

        public string DownloadFileName { get; private set; }
        public string ContentType { get; protected set; }
        public byte[] DataFileContents { get; protected set; }

        public FileContentResult GetResultAsContentType ()
        {
            return new FileContentResult(DataFileContents, ContentType)
            {
                FileDownloadName = DownloadFileName
            };
        }
    }


    public class ExportHelperBase64Image : ExportHelper
    {
        public string Base64ImageUrl { get; private set; }

        public ExportHelperBase64Image(string base64ImageUrl, string downloadFileName, string contentType) : base(downloadFileName, contentType, new byte[] { })
        {
            Base64ImageUrl = base64ImageUrl;
            DataFileContents = GetBytes();
        }


        public ExportHelperBase64Image(string base64ImageUrl, string downloadFileName) : base(downloadFileName, null, new byte[] { })
        {
            Base64ImageUrl = base64ImageUrl;
        }


        public FileContentResult GetResultAsJpg()
        {
            using (MemoryStream chartJpgStream = new MemoryStream())
            {
                using (Image chart = Image.FromStream(new MemoryStream(GetBytes())))
                {
                    using (Graphics chartGraphics = Graphics.FromImage(chart))
                    {
                        chartGraphics.DrawImage(chart, new Rectangle(0, 0, chart.Width, chart.Height), 0, 0, chart.Width, chart.Height, GraphicsUnit.Pixel);
                    }

                    chart.Save(chartJpgStream, ImageFormat.Jpeg);
                }

                chartJpgStream.Position = 0;
                DataFileContents = chartJpgStream.ToArray();
                chartJpgStream.Close();
                ContentType = "image/jpg";
                return GetResultAsContentType();
            }
        }


        public string GetDataFromUrl()
        {
            return Regex.Match(Base64ImageUrl, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
        }


        public byte[] GetBytes()
        {
            return Convert.FromBase64String(GetDataFromUrl());
        }


        Bitmap CropImg (Image img, Rectangle rect)
        {
            
            return new Bitmap(img).Clone(rect, img.PixelFormat);
        }


        public Image GetChartImage ()
        {
            return Image.FromStream(new MemoryStream(GetBytes()));
        }


        public XImage ImgToXimg(Bitmap img)
        {
            MemoryStream strm = new MemoryStream();
            
            img.Save(strm, ImageFormat.Png);
            return XImage.FromStream(strm);
        } 

        public FileContentResult GetResultAsPdf()
        {
            DataFileContents = ProjectileMotionFilesSaving.GetMemoryStreamFromPdfDocument((document, page) =>
            {
                using (Image chart = Image.FromStream(new MemoryStream(GetBytes())))
                {
                    int chartHeightRendered = 0;
                    while (chartHeightRendered < chart.Size.Height)
                    {
                        using (XGraphics gfx = XGraphics.FromPdfPage(chartHeightRendered == 0 ? page : document.AddPage(), XPageDirection.Downwards))
                        {
                            gfx.PdfPage.TrimMargins = new TrimMargins()
                            {
                                All = new XUnit(1, XGraphicsUnit.Centimeter)
                            };


                            int chartHeightFullPage = Convert.ToInt32(chart.Size.Width * (gfx.PageSize.Height / gfx.PageSize.Width));
                            int chartHeightToRender = chart.Size.Height - chartHeightRendered < chartHeightFullPage ? chart.Size.Height - chartHeightRendered : chartHeightFullPage;

                            gfx.PdfPage.Orientation = PageOrientation.Portrait;
                            gfx.DrawImage(
                                ImgToXimg(
                                    CropImg(
                                        chart,
                                        new Rectangle(
                                            new Point(0, chartHeightRendered),
                                            new Size(chart.Size.Width, chartHeightToRender)
                                        )
                                    )
                                ), 
                                0, 
                                0, 
                                gfx.PageSize.Width,
                                gfx.PageSize.Width * ((double)chartHeightToRender / chart.Size.Width)
                            );

                            chartHeightRendered += chartHeightToRender;
                        }
                    }
                }
               
                          
            }).ToArray();

            ContentType = "aplication/pdf";

            return GetResultAsContentType();
        }
    }
}