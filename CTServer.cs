using System;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Threading.Tasks;

namespace CulebraTesterAPI
{
    public class CTServer : IDisposable
    {
        private bool disposedValue;
        private Process Process { get; } = new Process();
        private bool Useable { get; set; } = false;
        private int Port { get; set; }
        public CTServer(int localport = 9987, bool logtoconsole = false)
        {
            Port = localport;
            Process.StartInfo.FileName = @"C:\Program Files\Git\bin\bash.exe";
            Process.StartInfo.Arguments = $"-c './c2 start-server --local-port {localport}'";
            Process.StartInfo.WorkingDirectory = @"G:\Source\adblibtest\bin\Debug\net8.0\lib";
            Process.StartInfo.UseShellExecute = false;
            Process.StartInfo.CreateNoWindow = true;
            Process.StartInfo.RedirectStandardInput = true;
            Process.StartInfo.RedirectStandardOutput = true;
            Process.StartInfo.RedirectStandardError = true;
            Process.OutputDataReceived += (_, e) =>
            {
                if (e?.Data?.Contains("INSTRUMENTATION_STATUS_CODE: 1") ?? false)
                {
                    Useable = true;
                }
                else if (e?.Data?.Contains("INSTRUMENTATION_CODE: 0") ?? false)
                {
                    Useable = false;
                }

                if(e?.Data?.Contains("INSTRUMENTATION_RESULT:") ?? false)
                {
                    Debug.WriteLine(e?.Data?.Replace("INSTRUMENTATION_RESULT:", "").Trim());
                }

                if (logtoconsole) Console.WriteLine(e.Data);
                Debug.WriteLine(e.Data);
            };
            Process.ErrorDataReceived += (_, e) =>
            {
                if (logtoconsole) Console.WriteLine(e.Data);
                Debug.WriteLine(e.Data);
            };

            Process.Start();
            Process.BeginOutputReadLine();
            Process.BeginErrorReadLine();

            while (true)
            {
                if (Useable)
                {
                    break;
                }

                Task.Delay(100).Wait();
            }
        }

        public static void CleanServerStart()
        {
            Console.WriteLine("Performing Clean Start");
            foreach (var process in Process.GetProcessesByName("adb"))
            {
                Debug.WriteLine($"{process.Id} :: {process.ProcessName} - K");
                process.Kill();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            void KAC(int pid)
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * From Win32_Process Where ParentProcessID=" + pid);
                ManagementObjectCollection moc = searcher.Get();
                foreach (ManagementObject mo in moc.Cast<ManagementObject>()) KAC(Convert.ToInt32(mo["ProcessID"]));

                try
                {
                    Process proc = Process.GetProcessById(pid);
                    Debug.WriteLine($"Killing process {proc.ProcessName} ({pid})");
                    proc.Kill();
                }
                catch (ArgumentException)
                {
                    // 进程已经结束
                }
            }

            if (!disposedValue)
            {
                if (disposing)
                {
                    KAC(Process.Id);
                }

                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        public CTClient GetClient() => new CTClient("http://localhost", Port);
    }
}
