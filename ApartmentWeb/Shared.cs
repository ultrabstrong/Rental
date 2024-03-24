using ApartmentWeb.SiteConfiguration;
using Corely.Core;
using Corely.Data.Serialization;
using Corely.Extensions;
using Corely.Logging;
using Domain.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Web;
using rm = Resources.Website.Logs;

namespace ApartmentWeb
{
    public static class Shared
    {
        private static readonly string _datadir = $"{HttpContext.Current.Server.MapPath("~")}/data/";
        private static readonly string _logdir = $"{HttpContext.Current.Server.MapPath("~")}/logs/";
        private static readonly string _configfile = $"{HttpContext.Current.Server.MapPath("~")}/data/serviceconfig.xml";

        private static object ConfigLock { get; set; } = new object();

        static Shared()
        {
            if (!Directory.Exists(_datadir))
            {
                Directory.CreateDirectory(_datadir);
            }
            if (!Directory.Exists(_logdir))
            {
                Directory.CreateDirectory(_logdir);
            }

            Logger = new FileLogger("WebApp", _logdir, "LogFile");
            Logger.FileLogManagementPolicy.DeleteDaysOld = 90;
            try
            {
                if (!File.Exists(_configfile))
                {
                    Configuration = new SiteConfig()
                    {
                        LogLevel = LogLevel.WARN.GetLogLevelName(),
                        SiteDetails = new SiteDetails()
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
                            TenantInfoDocs = new NamedValues(new Dictionary<string, string>()
                            {
                                { "DisplayName", "FileName" }
                            })
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
                Logger.WriteLog(rm.failGetSvcConfig, ex, LogLevel.ERROR);
            }
        }

        public static FileLogger Logger { get; set; }

        public static SiteConfig Configuration { get; set; }

        public static string Version => FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;

        public static void LoadConfiguration()
        {
            lock (ConfigLock)
            {
                Configuration = XmlSerializer.DeSerializeFromFile<SiteConfig>(_configfile);

                if (Configuration != null)
                {
                    try
                    {
                        // Set log level if it is not already set
                        if (string.IsNullOrWhiteSpace(Configuration.LogLevel))
                        {
                            Configuration.LogLevel = LogLevel.WARN.ToString();
                            SaveConfiguration();
                        }

                        Logger.LogLevel = Configuration.LogLevel.TryParseLogLevel();
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog(rm.failSetLogLevel, ex, LogLevel.ERROR);
                    }
                }
            }
        }

        public static void SaveConfiguration()
        {
            lock (ConfigLock)
            {
                XmlSerializer.SerializeToFile(Configuration, _configfile);
            }
        }
    }
}