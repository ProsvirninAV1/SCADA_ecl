using System;
using MasterSCADA.Script.FB;
using MasterSCADA.Hlp;
using FB;
using System.Linq;
using InSAT.OPC;
using MasterSCADALib;

namespace SCADA_ecl.Classes
{
    public class BaseIm : ScriptBase
    {
        protected ScriptFB _scriptFB;

        private bool _onIsLinked;
        private bool _offIsLinked;
        private bool _accidentIsLinked;
        private bool _malfunctionIsLinked;
        private bool _localIsLinked;
        private bool _distanceIsLinked;
        
        private string _statOn;
        private string _statOff;
        private string _accident;
        private string _malfunction;
        private string _local;
        private string _distance;
        
        public bool Accident { get; private set; }
        public bool Malfunction { get; private set; }

        public IQFStatus QfStatus { get; private set; }
        public IQFStatus ModeStatus { get; private set; }
                
        public OpcQuality Quality { get; private set; }

        public IntPack1 statusSet = new IntPack1();

        public BaseIm() { }

        public BaseIm(
                        ScriptFB scriptFB,
                        string statOn = null,
                        string statOff = null,
                        string accident = null,
                        string malfunction = null,
                        string local = null,
                        string distance = null)
        {
            _scriptFB = scriptFB;
            _statOn = statOn;
            _statOff = statOff;
            _accident = accident;
            _malfunction = malfunction;
            _local = local;
            _distance = distance;
           
            _onIsLinked = IsLinked(statOn);
            _offIsLinked = IsLinked(statOff);
            _accidentIsLinked = IsLinked(accident);
            _malfunctionIsLinked = IsLinked(malfunction);
            _localIsLinked = IsLinked(local);
            _distanceIsLinked = IsLinked(distance);
            
            QfStatus = GetTypeDevice(_onIsLinked, _offIsLinked);
            ModeStatus = GetTypeDevice(_distanceIsLinked, _localIsLinked);
        }

        public PinValue GetStatusForARM()
        {
            RecalcQfStatus();

            if (Quality == OpcQuality.Good)
            {
                statusSet.setBits(0, 2, Convert.ToUInt16(QfStatus.OnStatus));
                statusSet.setBit(3, Accident);
                statusSet.setBit(4, Malfunction);
                statusSet.setBit(5, Convert.ToUInt16(ModeStatus.OnStatus) == 2);//местное
                statusSet.setBit(6, Convert.ToUInt16(ModeStatus.OnStatus) == 1);//дистанционное
            }
            return new PinValue(statusSet.Value, (uint)Quality, DateTime.Now);
        }

        public void RecalcQfStatus()
        {
            if (_onIsLinked || _offIsLinked)
                QfStatus.GetStatus(_scriptFB.GetValue(_statOn), _scriptFB.GetValue(_statOff));

            if(_localIsLinked || _distanceIsLinked)
                ModeStatus.GetStatus(_scriptFB.GetValue(_distance), _scriptFB.GetValue(_local));

            if (_accidentIsLinked)
                Accident = (bool?)_scriptFB.GetValue(_accident).Value ?? false;

            if (_malfunctionIsLinked)
                Malfunction = (bool?)_scriptFB.GetValue(_malfunction).Value ?? false;

            Quality = QfStatus.Quality;
        }

        protected bool IsLinked(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var elem = _scriptFB.InputGroup.GetPin(name).TreePinHlp;
                var connectedItems = elem.GetConnections(EConnectionTypeMask.ctGeneric);
                return connectedItems.FirstOrDefault() == null ? false : true;
            }
            else return false;
        }

        protected IQFStatus GetTypeDevice(bool one, bool two)
        {
            if (one && two) return new QFOnOff();
            else if (one && !two) return new QFOn();
            else if (!one && two) return new QFOff();
            else return new QFNull();
        }
    }
}

