using ApartmentWeb.Models.Application;
using ApartmentWeb.Models.Maintenance;
using Domain.Core;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using logrm = Resources.Website.Logs;
using rm = Resources.Website.Home;

namespace ApartmentWeb.Controllers
{
    public class HomeController : Controller
    {
        public static readonly string Name = nameof(HomeController).Replace(nameof(Controller), "");

        public ActionResult Index() => View();

        public ActionResult DownloadApplication() => View();

        public ActionResult TenantInfo() => View();

        public ActionResult ContactUs() => View();

        public ActionResult Apply() => View(new Application());

        public ActionResult MaintenanceRequest() => View(new MaintenanceRequest());


        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitMaintenanceRequest(MaintenanceRequest maintenanceRequest)
        {
            if (!ModelState.IsValid)
            {
                // Uncomment to view errors
                // List<KeyValuePair<string, ModelState>> Errors = ModelState.Where(m => m.Value.Errors.Count > 0).ToList();
                return Json(new { hasValidationErrors = true });
            }

            Log.Logger.Debug(logrm.creatingViewHtml);
            var html = RenderRazorViewToString(nameof(this.MaintenanceRequest), maintenanceRequest);

            try
            {
                // Create PDF of application
                Log.Logger.Debug(logrm.convertingHtmlToPDF);
                var pdf = HtmlConverter.ToPdf(html,
                    Shared.Configuration.CompanyName,
                    $"{maintenanceRequest.FirstName} {maintenanceRequest.LastName} Maintenance Request",
                    $"Maintenance request for {maintenanceRequest.RentalAddress} from {maintenanceRequest.FirstName} {maintenanceRequest.LastName}");

                OutputSamplesIfDebug(html, pdf);

                // Send maintenance request in email
                Log.Logger.Debug(logrm.sendingMaintEmail);
                using (var pdfStream = new MemoryStream(pdf))
                using (var emailService = new EmailService(Shared.Configuration.MailSettings))
                {
                    await emailService.SendEmailAsync(maintenanceRequest, pdfStream);
                }
                Log.Logger.Debug(logrm.finishedSendingMaintEmail);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, logrm.failSubmitMaintReq);
            }

            return Json(new { success = true, redirectUrl = Url.Action(nameof(Index), Name) });
        }

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitApplication(Application application)
        {
            if (!ModelState.IsValid)
            {
                // Uncomment to view errors
                // List<KeyValuePair<string, ModelState>> Errors = ModelState.Where(m => m.Value.Errors.Count > 0).ToList();
                return Json(new { hasValidationErrors = true });
            }

            Log.Logger.Debug(logrm.creatingViewHtml);
            var html = RenderRazorViewToString(nameof(this.Apply), application);
            try
            {
                // Create PDF of application
                Log.Logger.Debug(logrm.convertingHtmlToPDF);
                var pdf = HtmlConverter.ToPdf(html,
                    Shared.Configuration.CompanyName,
                    $"{application.PersonalInfo.FirstName} {application.PersonalInfo.LastName} Application",
                    $"Application for {application.RentalAddress} from {application.PersonalInfo.FirstName} {application.PersonalInfo.LastName}; Co-Applicants : {application.OtherApplicants}");

                OutputSamplesIfDebug(html, pdf);

                // Send application in email
                Log.Logger.Debug(logrm.sendingEmail);
                using (MemoryStream pdfStream = new MemoryStream(pdf))
                using (var emailService = new EmailService(Shared.Configuration.MailSettings))
                {
                    await emailService.SendEmailAsync(application, pdfStream);
                }
                Log.Logger.Debug(logrm.finishedSendingEmail);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, logrm.failSubmitApp);
            }

            return Json(new { success = true, redirectUrl = Url.Action(nameof(Index), Name) });
        }

        private string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                ViewBag.inlinecss = "Yes";
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        private void OutputSamplesIfDebug(string html, byte[] pdf)
        {
#if DEBUG
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            // Output sample PDF and HTML for debugging
            System.IO.File.WriteAllText($@"{desktop}\sample.html", html);
            if (System.IO.File.Exists($@"{desktop}\sample.pdf")) { System.IO.File.Delete($@"{desktop}\sample.pdf"); }
            using (FileStream fs = new FileStream($@"{desktop}\sample.pdf", FileMode.Create, FileAccess.ReadWrite))
            {
                fs.Write(pdf, 0, pdf.Length);
                fs.Flush();
            }
#endif
        }

        public ActionResult ReloadConfig()
        {
            Shared.LoadConfiguration();
            return Content(rm.HOME_CONFIG_RELOADED);
        }
    }
}