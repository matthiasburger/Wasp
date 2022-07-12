using System;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

namespace wasp.Core;

public static class DeviceInformation
{
    public static string GetComputerName()
    {
        IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
        return computerProperties.HostName;
    }

    public static OSPlatform OperatingSystem
    {
        get
        {
            if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return OSPlatform.Windows;
            if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return OSPlatform.Linux;
            if(RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return OSPlatform.OSX;
            if(RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
                return OSPlatform.FreeBSD;
            throw new NotImplementedException(nameof(OSPlatform));
        }
    }
}