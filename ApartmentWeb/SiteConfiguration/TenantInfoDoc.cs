using System;

namespace ApartmentWeb.SiteConfiguration
{
    [Serializable]
    public class TenantInfoDoc
    {
        public string DisplayName { get; set; }
        public string FileName { get; set; }
    }
}