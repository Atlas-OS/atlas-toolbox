﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtlasToolbox.Utils;
using MVVMEssentials.Commands;

namespace AtlasToolbox.Commands.ConfigurationButtonsCommand
{
    public class CurrentVBSConfigurationCommand : AsyncCommandBase
    {
        protected override async Task ExecuteAsync(object parameter)
        {
            await Task.Run(() => { ProcessHelper.StartShellExecute($@"{Environment.GetEnvironmentVariable("windir")}\AtlasModules\Toolbox\Scripts\vbsCurrentConfig.cmd"); });
        }
    }
}
