using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCADA_ecl.Classes
{
	public class QFOnOff : IQFStatus
	{
		public OpcQuality Quality { get; private set; }

		public bool? StatOn { get; private set; }
		public bool? StatOff { get; private set; }

		public CrzaFunctions.OnStatuses OnStatus { get; private set; }

		public string StatusString { get; private set; }

		public void GetStatus(MasterSCADA.Hlp.PinValue on = null, MasterSCADA.Hlp.PinValue off = null)
		{
			if (on.GoodQuality & off.GoodQuality & on != null & off != null)
			{
				Quality = OpcQuality.Good;
				if ((bool?)on.Value == true && (bool?)off.Value == false)
				{
					StatOn = true;
					StatOff = false;
					StatusString = "Вкл.";
					OnStatus = CrzaFunctions.OnStatuses.STATUSON;
				}
				else if ((bool?)off.Value == true && (bool?)on.Value == false)
				{
					StatOn = false;
					StatOff = true;
					StatusString = "Откл.";
					OnStatus = CrzaFunctions.OnStatuses.STATUSOFF;
				}
				else if (!(bool?)off.Value == true && (bool?)on.Value == false)
				{
					StatOn = null;
					StatOff = null;
					StatusString = "?";
					OnStatus = CrzaFunctions.OnStatuses.STATUSDIFFERENT;
				}
				else
				{
					StatOn = null;
					StatOff = null;
					StatusString = "?";
					OnStatus = CrzaFunctions.OnStatuses.STATUSERROR;
				}
			}
			else
			{
				Quality = OpcQuality.Bad;
				StatOn = null;
				StatOff = null;
				StatusString = "?";
				OnStatus = CrzaFunctions.OnStatuses.STATUSERROR;
			}
		}
	}
}
