using System.Diagnostics;

namespace AutoUpgrade
{
    public class DotNet
    {
        public void Test()
        {
            var proc = new Process
            {
                StartInfo =
                {
                    FileName = "dotnet test",
                    Arguments = "-c Release"
                }
            };
            proc.Start();
            proc.WaitForExit();
            var exitCode = proc.ExitCode;
            proc.Close();
        }
    }
}