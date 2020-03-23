using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SignToolDll
{
    class Program
    {
        static void Main(string[] args)
        {
            string pathScan = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string pathUp1 = Path.GetFullPath(Path.Combine(pathScan, @".."));
            string pathUp2 = Path.GetFullPath(Path.Combine(pathScan, @"..\.."));
            string certificateName = @"\ArnoldVinkCertificate\ArnoldVinkCertificate.pfx";
            string pathCertificate = string.Empty;
            string signToolName = @"\ArnoldVinkCertificate\SignTool.exe";
            string pathSignTool = string.Empty;

            if (File.Exists(pathUp1 + certificateName))
            {
                pathCertificate = pathUp1 + certificateName;
            }

            if (File.Exists(pathUp1 + signToolName))
            {
                pathSignTool = pathUp1 + signToolName;
            }

            if (File.Exists(pathUp2 + certificateName))
            {
                pathCertificate = pathUp2 + certificateName;
            }

            if (File.Exists(pathUp2 + signToolName))
            {
                pathSignTool = pathUp2 + signToolName;
            }

            if (string.IsNullOrWhiteSpace(pathCertificate))
            {
                Console.WriteLine("Sign certificate could not be found: " + certificateName);
                return;
            }

            string[] skipFiles = new string[] { "SignToolExe", "SignToolDll", @"\obj\" };
            string arguments = "sign /t http://timestamp.digicert.com /f " + pathCertificate;

            if (!File.Exists(pathSignTool))
            {
                Console.WriteLine("SignTool could not be found: " + pathSignTool);
                return;
            }

            Console.WriteLine("Adding certificate: " + pathCertificate);

            string[] signFiles = Directory.GetFiles(pathScan, "*.dll", SearchOption.AllDirectories);
            foreach (string signFile in signFiles)
            {
                try
                {
                    if (skipFiles.Any(signFile.Contains)) { continue; }
                    Console.WriteLine("Signing: " + signFile);
                    Process LaunchProcess = new Process();
                    LaunchProcess.StartInfo.UseShellExecute = false;
                    LaunchProcess.StartInfo.CreateNoWindow = true;
                    LaunchProcess.StartInfo.FileName = pathSignTool;
                    LaunchProcess.StartInfo.Arguments = arguments + " " + signFile;
                    LaunchProcess.Start();
                }
                catch { }
            }
        }
    }
}