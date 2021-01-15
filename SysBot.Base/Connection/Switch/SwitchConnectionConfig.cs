﻿using System;
using System.Net;
using static SysBot.Base.SwitchProtocol;

namespace SysBot.Base
{
    public record SwitchConnectionConfig : ISwitchBotConfig, IWirelessBotConfig
    {
        public SwitchProtocol Protocol { get; set; }
        public string IP { get; set; } = string.Empty;
        public int Port { get; set; } = 6000;

        public bool UseCRLF => Protocol is WiFi;

        public bool IsValid() => Protocol switch
        {
            WiFi => IPAddress.TryParse(IP, out _),
            _ => false,
        };

        public override string ToString() => Protocol switch
        {
            WiFi => IP,
            _ => Port.ToString(),
        };

        public ISwitchConnectionAsync CreateAsynchronous() => Protocol switch
        {
            WiFi => new SwitchSocketAsync(this),
            USB => new SwitchUSBAsync(Port),
            _ => throw new IndexOutOfRangeException(nameof(SwitchProtocol)),
        };

        public ISwitchConnectionSync CreateSync() => Protocol switch
        {
            WiFi => new SwitchSocketSync(this),
            USB => new SwitchUSBSync(Port),
            _ => throw new IndexOutOfRangeException(nameof(SwitchProtocol)),
        };
    }

    public enum SwitchProtocol
    {
        WiFi,
        USB,
    }
}
