using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSTmanager.handlers
{
    internal interface IApplicationStartable
    {
        void Start();
        void BeforeStart();
        void AfterStart();
    }
}
