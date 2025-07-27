﻿namespace AtlasToolbox.Utils
{
    public static class ComputerStateHelper
    {
        public static void LogOffComputer()
        {
            CommandPromptHelper.RunCommand("logoff");
        }

        public static void RestartComputer()
        {
            CommandPromptHelper.RunCommand("shutdown /r /t 000");
        }

        public static void RestartApp()
        {
            App.RestartApp();
        }
    }
}