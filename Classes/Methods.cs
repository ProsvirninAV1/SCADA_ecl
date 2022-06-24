using MasterSCADA.Script.FB;
using System.Linq;
using MasterSCADALib;

public class Methods : ScriptBase
{
    public static bool IsLinked(ScriptFB scriptFB, string name)
    {
        if (!string.IsNullOrEmpty(name))
        {
            var elem = scriptFB.InputGroup.GetPin(name).TreePinHlp;
            var connectedItems = elem.GetConnections(EConnectionTypeMask.ctGeneric);
            return connectedItems.FirstOrDefault() == null ? false : true;
        }
        else return false;
    }

    public static bool IsLinkedOut(ScriptFB scriptFB, string name)
    {
        if (!string.IsNullOrEmpty(name))
        {
            var elem = scriptFB.OutputGroup.GetPin(name).TreePinHlp;
            var connectedItems = elem.GetConnections(EConnectionTypeMask.ctGeneric);
            return connectedItems.FirstOrDefault() == null ? false : true;
        }
        else return false;
    }
}
