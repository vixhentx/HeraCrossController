using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeraCrossController.Models
{
    public class LinearParameter
    {
        public int CmdChannel;
        public int Value;
        public LinearParameter() { }
        public LinearParameter(int cmdChannel, int value)
        {
            CmdChannel = cmdChannel;
            Value = value;
        }
    }
}
