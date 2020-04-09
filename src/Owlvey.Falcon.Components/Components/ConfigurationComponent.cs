﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Components
{
    public class ConfigurationComponent
    {
        public int DefaultRetryAttempts { get; set; } = 3;
        public TimeSpan DefaultPauseBetweenFails { get; set; } = TimeSpan.FromSeconds(2);
    }
}
