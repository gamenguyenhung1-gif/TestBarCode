using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Extensions.Logging;

namespace TestBarCode2
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // 🔹 Cấu hình NLog từ file JSON
            try
            {
                var jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "nlog.json");

                var configRoot = new ConfigurationBuilder()
     .AddJsonFile(jsonPath, optional: false, reloadOnChange: true)
     .Build();

                LogManager.Configuration = new NLogLoggingConfiguration(configRoot.GetSection("NLog"));

                var logger = LogManager.GetCurrentClassLogger();
                logger.Info("Ứng dụng khởi động.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cấu hình NLog: " + ex.Message);
            }

            // 🔹 Chạy ứng dụng
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
