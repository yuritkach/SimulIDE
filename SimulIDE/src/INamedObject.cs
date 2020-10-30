using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulIDE.src
{
    public interface INamedObject
    {
        void SetObjectName(string name);
        string GetObjectName();
    }
}
