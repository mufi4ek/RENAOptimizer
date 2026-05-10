using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;

namespace RENAOptimizer
{
    public static class OptimizerLogic
    {
        public static void ApplyTweaks()
        {
            // 1. Игровой режим и приоритет GPU
            SetRegistry(@"HKEY_CURRENT_USER\Software\Microsoft\GameBar", "AutoGameModeEnabled", 1);
            SetRegistry(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\PolicyManager\default\ApplicationManagement\AllowGameDVR", "value", 0);

            // 2. Оптимизация Сети (Снижение пинга / Disable Nagle's Algorithm)
            SetRegistry(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters\Interfaces", "TcpAckFrequency", 1);
            SetRegistry(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters\Interfaces", "TCPNoDelay", 1);

            // 3. Отключение лимита пропускной способности сети
            SetRegistry(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Psched", "NonBestEffortLimit", 0);

            // 4. Оптимизация визуальных эффектов Windows (на производительность)
            SetRegistry(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects", "VisualFXSetting", 2);

            // 5. Отключение телеметрии и фоновых служб
            SetRegistry(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "AllowTelemetry", 0);

            // 6. Очистка кэша (Shader Cache, Temp, Logs)
            CleanSystem();

            // 7. План электропитания
            RunCommand("powercfg -duplicatescheme e9a42b02-d5df-448d-aa00-03f14749eb61");
            RunCommand("powercfg /setactive e9a42b02-d5df-448d-aa00-03f14749eb61");
        }

        private static void CleanSystem()
        {
            string[] paths = {
                Path.GetTempPath(),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Temp"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "D3DSCache"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "NVIDIA\\GLCache")
            };

            foreach (var path in paths)
            {
                if (Directory.Exists(path))
                {
                    RunCommand($"del /q /f /s \"{path}\\*\"");
                }
            }
        }

        private static void SetRegistry(string keyPath, string valueName, object value)
        {
            try { Registry.SetValue(keyPath, valueName, value); }
            catch { /* Игнорируем ошибки доступа */ }
        }

        private static void RunCommand(string command)
        {
            ProcessStartInfo psi = new ProcessStartInfo("cmd.exe", "/c " + command)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden
            };
            Process.Start(psi);
        }
    }
}
