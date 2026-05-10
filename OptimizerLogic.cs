using Microsoft.Win32;
using System;
using System.Diagnostics;

namespace RENAOptimizer
{
    public static class OptimizerLogic
    {
        public static void ApplyTweaks()
        {
            try
            {
                // 1. Включение игрового режима
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\GameBar", "AutoGameModeEnabled", 1);

                // 2. Отключение телеметрии (слежки)
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "AllowTelemetry", 0);

                // 3. Оптимизация задержки мыши
                Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseSpeed", "0");

                // 4. Очистка временных файлов через CMD
                RunCommand("del /q /f /s %temp%\\*");
                
                // 5. Импорт максимальной производительности (Power Plan)
                RunCommand("powercfg -duplicatescheme e9a42b02-d5df-448d-aa00-03f14749eb61");
                RunCommand("powercfg /setactive e9a42b02-d5df-448d-aa00-03f14749eb61");
            }
            catch (Exception ex)
            {
                // Ошибка прав доступа (нужен запуск от админа)
            }
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