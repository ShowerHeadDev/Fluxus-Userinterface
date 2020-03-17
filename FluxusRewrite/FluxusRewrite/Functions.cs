using System;
using System.Threading;
using System.Windows.Forms;

namespace FluxusRewrite
{
    class Functions
    {
        public static string exploitdllname = "FluxusTeamAPI.dll";
        public static void Inject()
        {
            if (NamedPipes.NamedPipeExist(NamedPipes.luapipename))
            {
                MessageBox.Show("Already injected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            else if (!NamedPipes.NamedPipeExist(NamedPipes.luapipename))
            {
                switch (Injector.DllInjector.GetInstance.Inject("RobloxPlayerBeta", AppDomain.CurrentDomain.BaseDirectory + exploitdllname))
                {
                    case Injector.DllInjectionResult.DllNotFound:
                        MessageBox.Show("Couldn't find " + exploitdllname, "Dll was not found!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    case Injector.DllInjectionResult.GameProcessNotFound:
                        MessageBox.Show("Couldn't find RobloxPlayerBeta.exe!", "Target process was not found!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    case Injector.DllInjectionResult.InjectionFailed:
                        MessageBox.Show("Injection Failed!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                }
                Thread.Sleep(3000);
                if (!NamedPipes.NamedPipeExist(NamedPipes.luapipename))
                {
                
                }
            }
        }
    }
}
