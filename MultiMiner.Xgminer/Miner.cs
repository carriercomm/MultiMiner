﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MultiMiner.Xgminer
{
    public class Miner : IDisposable
    {
        private Process minerProcess;
        private readonly MinerConfiguration minerConfig;

        public Miner(MinerConfiguration minerConfig)
        {
            this.minerConfig = minerConfig;
        }

        public List<Device> GetDevices()
        {
            List<Device> result = new List<Device>();

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = minerConfig.ExecutablePath;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.CreateNoWindow = true;
            startInfo.Arguments = MinerParameter.EnumerateDevices;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            minerProcess = Process.Start(startInfo);


            List<string> output = new List<string>();

            while (!minerProcess.StandardOutput.EndOfStream)
            {
                string line = minerProcess.StandardOutput.ReadLine();
                output.Add(line);
            }

            DeviceParser.ParseOutputForDevices(output, result);

            return result;
        }

        public void Dispose()
        {
            if (minerProcess != null)
            {
                minerProcess.Dispose();
                minerProcess = null;
            }
        }
    }
}
