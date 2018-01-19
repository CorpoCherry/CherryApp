using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Cherry.Web
{
    public static class Parameters
    {
        public static string DBtype
        {
            get
            {
            #if DEVELOPMENT
                 return "_local";
            #else
                 return "_online";
            #endif
            }
        }
        public static bool DBtypebool
        {
            get
            {
            #if DEVELOPMENT
                            return true;
            #else
                             return false;
            #endif
            }
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
