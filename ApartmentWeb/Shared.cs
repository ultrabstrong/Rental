using ApartmentWeb.Models.Site;
using Domain.Core;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Web;
using System.Xml.Serialization;
using rm = Resources.Website.Logs;

namespace ApartmentWeb
{
    public static class Shared
    {
        private static readonly string _dataDir = $"{HttpContext.Current.Server.MapPath("~")}/data/";
        private static readonly string _configFile = $"{HttpContext.Current.Server.MapPath("~")}/data/serviceconfig.xml";

        private static object ConfigLock { get; set; } = new object();

        static Shared()
        {
            if (!Directory.Exists(_dataDir))
            {
                Directory.CreateDirectory(_dataDir);
            }

            try
            {
                if (!File.Exists(_configFile))
                {
                    Configuration = new SiteDetails()
                    {
                        CompanyName = "CompanyName",
                        CompanyShortName = "CompanyShortName",
                        EmailAddress = "EmailAddress",
                        PhoneNumber = "PhoneNumber",
                        Address = "Address",
                        MailSettings = new MailSettings()
                        {
                            SMTPServer = "SMTPServer",
                            SMTPUsername = "SMTPUsername",
                            SMTPPw = "SMTPPw",
                            SMTPPort = 0,
                            SMTPTo = "SMTPTo"
                        },
                        ShowDownloadApplication = true,
                        TenantInfoShowTrash = false,
                        TenantInfoPostOfficeAddress = "PostOfficeAddress",
                        TenantInfoDocs = new List<TenantInfoDoc>()
                        {
                            new TenantInfoDoc() {DisplayName = "DisplayName", FileName = "FileName" }
                        }
                    };

                    SaveConfiguration();
                }
                else
                {
                    LoadConfiguration();
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error(rm.failGetSvcConfig, ex);
            }
        }

        public static SiteDetails Configuration { get; set; }

        public static string Version => FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;

        public static void LoadConfiguration()
        {
            lock (ConfigLock)
            {
                var xmlSerializer = new XmlSerializer(typeof(SiteDetails));
                using (var fs = new FileStream(_configFile, FileMode.Open, FileAccess.Read))
                {
                    Configuration = (SiteDetails)xmlSerializer.Deserialize(fs);
                }
            }
        }

        public static void SaveConfiguration()
        {
            lock (ConfigLock)
            {
                var xmlSerializer = new XmlSerializer(typeof(SiteDetails));
                using (var fs = new FileStream(_configFile, FileMode.Create, FileAccess.Write))
                {
                    xmlSerializer.Serialize(fs, Configuration);
                }
            }
        }
    }
}