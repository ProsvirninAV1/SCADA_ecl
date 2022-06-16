using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCADA_ecl.Classes
{
    public class SwitchWithCommand : BaseIm
    {
        private bool _autoIsLinked;
        private bool _manualIsLinked;

        private string _auto;
        private string _manual;

        public IQFStatus AutoManual { get; private set; }

        public SwitchWithCommand(
                                    ScriptFB scriptFB,
                                    string statOn = null,
                                    string statOff = null,
                                    string accident = null,
                                    string malfunction = null,
                                    string local = null,
                                    string distance = null,
                                    string auto = null,
                                    string manual = null) : base(
                                                                    scriptFB,
                                                                    statOn,
                                                                    statOff,
                                                                    accident,
                                                                    malfunction,
                                                                    local,
                                                                    distance)
        {
            _auto = auto;
            _manual = manual;

            _autoIsLinked = IsLinked(auto);
            _manualIsLinked = IsLinked(manual);

            AutoManual = GetTypeDevice(_manualIsLinked, _autoIsLinked);
        }

        public new PinValue GetStatusForARM()
        {
            RecalcQfStatus();
            base.GetStatusForARM();

            if (Quality == OpcQuality.Good)
            {
                statusSet.setBit(7, Convert.ToUInt16(AutoManual.OnStatus) == 1);//автоматическое
                statusSet.setBit(8, Convert.ToUInt16(AutoManual.OnStatus) == 2);//ручное
            }
            return new PinValue(statusSet.Value, (uint)Quality, DateTime.Now);
        }

        public new void RecalcQfStatus()
        {
            if (_manualIsLinked || _autoIsLinked)
                AutoManual.GetStatus(_scriptFB.GetValue(_auto), _scriptFB.GetValue(_manual));
        }


    }
}
