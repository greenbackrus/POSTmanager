using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSTmanager.helpers
{
    internal static class GeneralHelper
    {
        public static void Wait(int MSeconds) 
        {
            //TODO: refactor
            Task.Delay(MSeconds).GetAwaiter().GetResult();
        }
    }
}
