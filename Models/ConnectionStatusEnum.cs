﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeraCrossController.Models
{
    public enum ConnectionStatusEnum
    {
        Disconnected,
        Connected,
        Connecting,
        Discovering,
        Disconnecting,
        Sending,
        Error
    }
}
