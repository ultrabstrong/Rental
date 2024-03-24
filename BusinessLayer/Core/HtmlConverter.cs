using SelectPdf;
using System;

namespace BusinessLayer.Core
{
    public static class HtmlConverter
    {
        public static byte[] ToPDF(string html, string author, string title, string subject)
        {
            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
            converter.Options.MarginLeft = 10;
            converter.Options.MarginRight = 10;
            converter.Options.MarginTop = 20;
            converter.Options.MarginBottom = 20;
            converter.Options.PdfDocumentInformation.Author = author;
            converter.Options.PdfDocumentInformation.Title = title;
            converter.Options.PdfDocumentInformation.Subject = subject;
            converter.Options.PdfDocumentInformation.CreationDate = DateTime.Now;
            PdfDocument doc = converter.ConvertHtmlString(html);
            byte[] pdfBytes = doc.Save();
            return pdfBytes;
        }
    }
}
