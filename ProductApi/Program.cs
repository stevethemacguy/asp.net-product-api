using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace ProductApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                //Optional. Use this VM's IP instead of localhost. Use this to connect to the API from your mac:
                //For example: 192.168.173.201:5000/api/cities. Also, when debugging, you should select ProductApi instead of IISExpress.
                //WARNING: UseIISIntegration() will override UseUrls. If you want to test on your mac, you must comment out the UseIISIntegration Line!
                .UseUrls("http://192.168.173.201:5000/")  
                //.UseIISIntegration()
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .Build();

            host.Run();
        }
    }
}
