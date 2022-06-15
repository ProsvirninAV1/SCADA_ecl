using InSAT.OPC;

public interface IQFStatus
{
    OpcQuality Quality { get; }
    bool? StatOn { get; }
    bool? StatOff { get; }
    CrzaFunctions.OnStatuses OnStatus { get; }
    string StatusString { get; }
    void GetStatus(MasterSCADA.Hlp.PinValue on = null, MasterSCADA.Hlp.PinValue off = null);
}