using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using S7.Net;

namespace PlcSimClient
{
    public class PlcConfig
    {
        public string IpAddress { get; set; } = "172.22.1.1"; // IP address адаптера замыкания на себя 
        public short Rack { get; set; } = 0; // Номер стойки, в контроллерах S7-300/S7-400 Rack=0       
        public short Slot { get; set; } = 2; // Номер слота, в контроллерах S7-300/S7-400 Slot=2
        public CpuType CpuType { get; set; } = CpuType.S7400; // Тип контроллера, в Numerota18 S7-400
        public int ConnectionTimeout { get; set; } = 5000; // Connection timeout in milliseconds

        public static PlcConfig ParseFromArgs(string[] args)
        {
            var config = new PlcConfig();
            foreach (var arg in args)
            {
                var parts = arg.Split('=');
                if (parts.Length != 2) continue;
                var key = parts[0].Trim().ToLower();
                var value = parts[1].Trim();
                switch (key)
                {
                    case "ip":
                        config.IpAddress = value;
                        break;
                    case "rack":
                        if (short.TryParse(value, out short rack)) config.Rack = rack;
                        break;
                    case "slot":
                        if (short.TryParse(value, out short slot)) config.Slot = slot;
                        break;
                    case "cputype":
                        if (Enum.TryParse(value, true, out CpuType cpuType)) config.CpuType = cpuType;
                        break;
                    case "timeout":
                        if (int.TryParse(value, out int timeout)) config.ConnectionTimeout = timeout;
                        break;
                }
            }
            return config;
        }

        public override string ToString()
        {
            return $"IpAddress= {IpAddress}, Rack= {Rack}, Slot= {Slot}, CpuType= {CpuType}, ConnectionTimeout= {ConnectionTimeout}ms";
        }
    }
}
