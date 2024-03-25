using System;

namespace ApartmentWeb.Models.Site
{
    [Serializable]
    public class TenantInfoDoc
    {
        public string DisplayName { get; set; }
        public string FileName { get; set; }
    }
}