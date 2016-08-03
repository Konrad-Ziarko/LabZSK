using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabZKT
{
    interface IPMView
    {
        void LoadMicroOperations(ref List<MicroOperation> List_MicroOps);
    }
}
