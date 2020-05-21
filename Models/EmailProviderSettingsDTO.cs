using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs
{
    public class EmailProviderSettingsDTO
    {
        public string Host { get; set; }
        public string ApiKey { get; set; }
        public string ApiKeySecret { get; set; }
        public int Port { get; set; }
        public string SenderEmail { get; set; }
    }
}
