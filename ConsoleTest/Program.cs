using Domain.Core;
using SelectPdf;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace ConsoleTest
{
    class Program
    {
#pragma warning disable IDE0052 // Remove unread private members
        static readonly string DesktopLoc = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\";
        static readonly string DownloadsLoc = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads\\";
#pragma warning restore IDE0052 // Remove unread private members

        static void Main(string[] args)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception caught:{Environment.NewLine}{ex}");
            }
            Console.WriteLine("Program finished. Press any key to exit");
            Console.ReadKey();
        }

        static void TestEmailRegex()
        {
            var reg = new Regex(@"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$");
            var email = "ultrabstrong@gmail.com";
            Console.WriteLine(reg.IsMatch(email));
        }

        static void TestHTMLtoPDF()
        {
            string filepath = $"{DesktopLoc}sample.html";
            string outpath = $"{DesktopLoc}sample.pdf";
            string html = File.ReadAllText(filepath);

            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
            converter.Options.MarginLeft = 10;
            converter.Options.MarginRight = 10;
            converter.Options.MarginTop = 20;
            converter.Options.MarginBottom = 20;
            converter.Options.PdfDocumentInformation.Author = "ApexProperties";
            converter.Options.PdfDocumentInformation.CreationDate = DateTime.Now;
            converter.Options.PdfDocumentInformation.Title = "Application";
            converter.Options.PdfDocumentInformation.Subject = "Subect";

            PdfDocument doc = converter.ConvertHtmlString(html);
            byte[] pdfBytes = doc.Save();
            doc.Close();

            if (File.Exists(outpath)) { File.Delete(outpath); }
            using (FileStream fs = new FileStream(outpath, FileMode.Create, FileAccess.ReadWrite))
            {
                fs.Write(pdfBytes, 0, pdfBytes.Length);
                fs.Flush();
            }
        }


        static void TestMail()
        {
            MailSettings settings = new MailSettings()
            {
                SMTPServer = "server",
                SMTPUsername = "username",
                SMTPPw = "pw",
                SMTPPort = 25,
                SMTPTo = "to"
            };

            using (var smtpClient = new SmtpClient(settings.SMTPServer)
            {
                Port = settings.SMTPPort,
                Credentials = new NetworkCredential(settings.SMTPUsername, settings.SMTPPw),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network
            })
            {
                MailMessage message = new MailMessage()
                {
                    Subject = $"test subject",
                    From = new MailAddress(settings.SMTPUsername)
                };

                message.Body = $"test body";
                message.IsBodyHtml = false;

                message.To.Add(settings.SMTPTo);

                smtpClient.Send(message);
            }
        }
    }
}
