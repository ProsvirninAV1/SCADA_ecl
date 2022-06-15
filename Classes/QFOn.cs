using InSAT.OPC;

namespace SCADA_ecl.Classes
{
	public class QFOn : IQFStatus
	{
		public OpcQuality Quality { get; private set; }

		public bool? StatOn { get; private set; }
		public bool? StatOff { get; private set; }

		public CrzaFunctions.OnStatuses OnStatus { get; private set; }

		public string StatusString { get; private set; }

		public void GetStatus(MasterSCADA.Hlp.PinValue on = null, MasterSCADA.Hlp.PinValue off = null)
		{
			if (on.GoodQuality & on != null)
			{
				Quality = OpcQuality.Good;
				if ((bool?)on.Value == true)
				{
					StatOn = true;
					StatOff = false;
					StatusString = "Вкл.";
					OnStatus = CrzaFunctions.OnStatuses.STATUSON;
				}
				else
				{
					StatOn = false;
					StatOff = true;
					StatusString = "Откл.";
					OnStatus = CrzaFunctions.OnStatuses.STATUSOFF;
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