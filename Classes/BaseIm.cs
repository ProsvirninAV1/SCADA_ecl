using System;
using MasterSCADA.Script.FB;
using MasterSCADA.Hlp;
using FB;
using FB.FBAttributes;
using System.Linq;
using System.Timers;
using System.Threading.Tasks;
using MasterSCADA.Hlp.Pins;
using InSAT.OPC;
using FB.VisualFB;
using MasterSCADALib;
using System.Globalization;

namespace SCADA_ecl.Classes
{
    public class BaseIm //: ScriptBase
    {
        private FBGroup _fBGroup;

        protected uint _qual = 192;

        protected IntPack statusSet = new IntPack();

        protected UInt16 _qfStatus = 1;
        protected bool _accident;
        protected bool _malfunction;
        protected uint _controlMode;

        public bool _onIsLinked;
        public bool _offIsLinked;
        protected bool _localIsLinked;
        protected bool _disanceIsLinked;

        public BaseIm()
        {

        }

        public BaseIm(FBGroup fBGroup, string on, string off)
        {
            _fBGroup = fBGroup;
            _onIsLinked = IsLinked(on);
            _offIsLinked = IsLinked(off);
        }

        public uint Status
        {
            get
            {
                return statusSet.Value;
            }
        }

        protected bool IsLinked(string name)
        {
            var elem = _fBGroup.GetPin(name).TreePinHlp;
            var connectedItems = elem.GetConnections(EConnectionTypeMask.ctGeneric);
            return connectedItems.FirstOrDefault() == null ? false : true;
        }

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
