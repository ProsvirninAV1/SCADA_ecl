using System;
using MasterSCADA.Script.FB;
using MasterSCADA.Hlp;
using FB;
using System.Linq;
using InSAT.OPC;
using MasterSCADALib;

public class SwitchWithCommand : BaseIm
{
    private bool _autoIsLinked;
    private bool _manualIsLinked;
    private bool _cmdOnIsLinked;
    private bool _cmdOffIsLinked;

    private string _auto;
    private string _manual;
    private string _command_from_Arm;

    private string _cmdOn;
    private string _cmdOff;

    private Command _commandOn;
    private Command _commandOff;

    private bool _command_on;
    private bool _command_off;
    private uint _command;
    private uint _command_location;
    private uint _command_for_Arm;
    private uint _command_location_for_Arm;
    private uint _command_error_for_Arm;
    private uint _commandError;

    public event Action StatusForArmEvent;

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
                                string manual = null,
                                string command_from_Arm = null,

                                string cmdOn = null,
                                string cmdOff = null,

                                int timeCmdOn = 4000,
                                int timeResetOn = 7000,
                                int timeCmdOff = 4000,
                                int timeResetOff = 7000) : base(
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
        _command_from_Arm = command_from_Arm;

        _cmdOn = cmdOn;
        _cmdOff = cmdOff;

        _autoIsLinked = Methods.IsLinked(_scriptFB, auto);
        _manualIsLinked = Methods.IsLinked(_scriptFB, manual);
        _cmdOnIsLinked = Methods.IsLinkedOut(_scriptFB, cmdOn);
        _cmdOffIsLinked = Methods.IsLinkedOut(_scriptFB, cmdOff);

        AutoManual = GetTypeDevice(_manualIsLinked, _autoIsLinked);

        _commandOn = new Command(timeCmdOn, timeResetOn);
        _commandOff = new Command(timeCmdOff, timeResetOff);

        _commandOn.SetCommand += SetCommand;
        _commandOn.ResetCommandByTimer += ResetCommandByTimer;
        _commandOn.ErrorCommand += ErrorCommand;
        _commandOn.AfterExeuqutingCommand += ChangeOutCmd;

        _commandOff.SetCommand += SetCommand;
        _commandOff.ResetCommandByTimer += ResetCommandByTimer;
        _commandOff.ErrorCommand += ErrorCommand;
        _commandOff.AfterExeuqutingCommand += ChangeOutCmd;
    }

    public new PinValue GetStatusForARM()
    {
        base.GetStatusForARM();
        RecalcQfStatus();

        if (Quality == OpcQuality.Good)
        {
            statusSet.SetBit(7, Convert.ToUInt16(AutoManual.OnStatus) == 1);//автоматическое
            statusSet.SetBit(8, Convert.ToUInt16(AutoManual.OnStatus) == 2);//ручное
            statusSet.SetBits(23, 25, _command_error_for_Arm);
            statusSet.SetBits(26, 28, _command_for_Arm);
            statusSet.SetBits(29, 31, _command_location_for_Arm);
        }
        return new PinValue(statusSet.Value, (uint)Quality, DateTime.Now);
    }

    public void CommandFromArm()
    {
        IntParser input_command_from_Arm = new IntParser(Convert.ToUInt32(_scriptFB.GetValue(_command_from_Arm).Value));
        _command_location = input_command_from_Arm.getBits(13, 15);
        _command = _command_location != 0 ? input_command_from_Arm.getBits(0, 2) : 0;
        _command = input_command_from_Arm.getBits(0, 2);
        _command_on = ((_command & 0x7) == 0x1);//команда включить с АРМ, МПУ
        _command_off = ((_command & 0x7) == 0x2);//команда отключить с АРМ, МПУ

        if (_cmdOnIsLinked)
            _commandOn.CommandWithCheck(_command_on, _command_off, QfStatus.OnStatus == CrzaFunctions.OnStatuses.STATUSON);

        if (_cmdOffIsLinked)
            _commandOff.CommandWithCheck(_command_off, _command_on, QfStatus.OnStatus == CrzaFunctions.OnStatuses.STATUSOFF);
    }

    protected new void RecalcQfStatus()
    {
        if (_manualIsLinked || _autoIsLinked)
            AutoManual.GetStatus(_scriptFB.GetValue(_auto), _scriptFB.GetValue(_manual));
    }

    protected void SetCommand()
    {
        _command_for_Arm = _command;
        _command_location_for_Arm = _command_location;
        _commandError = _command;
        _command_error_for_Arm = 0;

        if (StatusForArmEvent != null) StatusForArmEvent();
        ChangeOutCmd();
    }

    protected void ResetCommandByTimer()
    {
        _command_for_Arm = 0;
        _command_error_for_Arm = 0;
        _command_location_for_Arm = 0;

        if (StatusForArmEvent != null) StatusForArmEvent();
        ChangeOutCmd();
    }

    protected void ErrorCommand()
    {
        _command_error_for_Arm = _commandError;

        if (StatusForArmEvent != null) StatusForArmEvent();
    }

    protected void ChangeOutCmd()
    {
        _scriptFB.SetValue(_cmdOn, new PinValue(_commandOn.Cmd, (uint)Quality, DateTime.Now));
        _scriptFB.SetValue(_cmdOff, new PinValue(_commandOff.Cmd, (uint)Quality, DateTime.Now));
    }
}