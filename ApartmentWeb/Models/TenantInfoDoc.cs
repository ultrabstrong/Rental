using System;

namespace ApartmentWeb.Models
{
    [Serializable]
    public class TenantInfoDoc
    {
        public string DisplayName { get; set; }
        public string FileName { get; set; }
    }
}