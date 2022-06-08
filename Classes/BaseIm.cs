using ECL.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCADA_ecl.Classes
{
    public class BaseIm
    {
        protected uint _qual = 192;

        protected IntPack statusSet = new IntPack();

        protected UInt16 _qfStatus = 1;
        protected bool _accident;
        protected bool _malfunction;
        protected uint _controlMode;
        
        public BaseIm()
        {
           
        }

        protected uint Status { get => statusSet.Value; }

        protected void StatusForArm()
        {
            if (_qual == 192)
            {
                statusSet.setBits(0, 2, Convert.ToUInt16(_qfStatus));
                statusSet.setBit(3, _accident);
                statusSet.setBit(4, _malfunction);
                statusSet.setBit(5, _controlMode == 2);//местное
                statusSet.setBit(6, _controlMode == 1);//дистанционное
            }
        }
    }
}
