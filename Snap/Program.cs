using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snap
{
	internal class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			Serilog.Log.Logger = new LoggerConfiguration()
				// .WriteTo.Console()
				.WriteTo.File("log-.txt", rollingInterval: RollingInterval.Day, shared: true)
				.CreateLogger();
			Log.Information("Starting application");

			Mutex mutex = new Mutex(true, "WindowSnapper", out bool createdNew);
			if (!createdNew)
			{
				Log.Information("Application is already running");
				return;
			}

			try
			{
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new Form1());
			}
			finally
			{
				mutex.Dispose();
			}
		}
	}
}
