using System;

namespace ApartmentWeb.SiteConfiguration
{
    [Serializable]
    public class SiteConfig
    {
        public SiteDetails SiteDetails { get; set; }

        public string LogLevel { get; set; }

    }
}