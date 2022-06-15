using InSAT.OPC;

public class QFOff : IQFStatus
{
	public OpcQuality Quality { get; private set; }

	public bool? StatOn { get; private set; }
	public bool? StatOff { get; private set; }

	public CrzaFunctions.OnStatuses OnStatus { get; private set; }

	public string StatusString { get; private set; }

	public void GetStatus(MasterSCADA.Hlp.PinValue on = null, MasterSCADA.Hlp.PinValue off = null)
	{
		if (off.GoodQuality & off != null)
		{
			Quality = OpcQuality.Good;
			if ((bool?)off.Value == true)
			{
				StatOn = false;
				StatOff = true;
				StatusString = "Откл.";
				OnStatus = CrzaFunctions.OnStatuses.STATUSOFF;
			}
			else
			{
				StatOn = true;
				StatOff = false;
				StatusString = "Вкл.";
				OnStatus = CrzaFunctions.OnStatuses.STATUSON;
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
