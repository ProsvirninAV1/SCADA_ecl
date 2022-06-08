using ECL.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCADA_ecl.Classes
{
    public class Device
    {
        private uint _status;
        private uint _qual = 192;

        private IntPack statusSet;
        private ChmiPostersStatuses chmiPostersStatuses = new ChmiPostersStatuses();

        private UInt16 _qfStatus;
        private bool _accident;
        private bool _malfunction;
        private uint _controlMode;
        private bool _call;
        private uint _basketStatus;

        private uint _command_error_for_Arm;
        private uint _command_for_Arm;
        private uint _command_location_for_Arm;

        public Device()
        {
            statusSet = new IntPack(ref _status);
        }

        public uint Status { get => _status;}

        private void StatusForArm()
        {
            if (_qual == 192)
            {
                statusSet.setBits(0, 2, Convert.ToUInt16(_qfStatus));
                statusSet.setBit(3, _accident);
                statusSet.setBit(4, _malfunction);
                statusSet.setBit(5, _controlMode == 2);//местное
                statusSet.setBit(6, _controlMode == 1);//дистанционное
                statusSet.setBit(7, _call);
                statusSet.setBits(8, 10, Convert.ToUInt16(_basketStatus));
                statusSet.setBit(11, chmiPostersStatuses.poster_work_line);
                statusSet.setBit(12, chmiPostersStatuses.poster_work_peoples);
                statusSet.setBit(13, chmiPostersStatuses.poster_work_under_voltage);
                statusSet.setBit(14, chmiPostersStatuses.poster_ground);
                statusSet.setBits(23, 25, _command_error_for_Arm);
                statusSet.setBits(26, 28, _command_for_Arm);
                statusSet.setBits(29, 31, _command_location_for_Arm);
                //_status = output.value;
            }

            //SetValue("status_for_Arm", new PinValue(_status, _qual, DateTime.Now));

            //blockWorkOrRepair = repairPosterStatus.statusOfPosterRepair;
        }
    }
}
