﻿using System;

namespace Rental.Domain.Models;

[Serializable]
public class MailSettings
{
    public string SMTPServer { get; set; }

    public string SMTPUsername { get; set; }

    public string SMTPPw { get; set; }

    public int SMTPPort { get; set; }

    public string SMTPTo { get; set; }
}
