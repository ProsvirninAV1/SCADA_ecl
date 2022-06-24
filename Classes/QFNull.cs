using InSAT.OPC;

public class QFNull : IQFStatus
{
	public OpcQuality Quality { get; private set; }

	public bool? StatOn { get; private set; }
	public bool? StatOff { get; private set; }

	public CrzaFunctions.OnStatuses OnStatus { get; private set; }

	public string StatusString { get; private set; }

	public void GetStatus(MasterSCADA.Hlp.PinValue on = null, MasterSCADA.Hlp.PinValue off = null)
	{
		Quality = OpcQuality.Bad;
		StatOn = null;
		StatOff = null;
		StatusString = "?";
		OnStatus = CrzaFunctions.OnStatuses.STATUSERROR;
	}
}