using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabZKT
{
    //C# does not allow to pass bools by ref so I was forced to create my own 'boolean'; which I can pass by refrence
    public class RefBool
    {
        private bool Value;
        public RefBool(bool value)
        {
            this.Value = value;
        }

        public static implicit operator RefBool(bool val)
        {
            return new RefBool(val);
        }
    }
}
