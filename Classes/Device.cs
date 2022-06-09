using ECL.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCADA_ecl.Classes
{
    public class Device: BaseIm
    {
        private ChmiPostersStatuses chmiPostersStatuses = new ChmiPostersStatuses();

        private bool _call;
        private uint _basketStatus = 1;

        private uint _command_error_for_Arm;
        private uint _command_for_Arm;
        private uint _command_location_for_Arm;

        

        public void StatusForArm()
        {
            if (_qual == 192)
            {
                base.StatusForArm();
                statusSet.setBit(7, _call);
                statusSet.setBits(8, 10, Convert.ToUInt16(_basketStatus));
                statusSet.setBit(11, chmiPostersStatuses.poster_work_line);
                statusSet.setBit(12, chmiPostersStatuses.poster_work_peoples);
                statusSet.setBit(13, chmiPostersStatuses.poster_work_under_voltage);
                statusSet.setBit(14, chmiPostersStatuses.poster_ground);
                statusSet.setBits(23, 25, _command_error_for_Arm);
                statusSet.setBits(26, 28, _command_for_Arm);
                statusSet.setBits(29, 31, _command_location_for_Arm);
            }
        }
    }
}
