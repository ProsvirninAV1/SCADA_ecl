using System;

public class Command
{
	private CmdTimer _cmdTimer = new CmdTimer();
	private CmdTimer _errorCMDTimer = new CmdTimer();

	private bool _lastCmd = false;

	private bool _commandCompletedForError;

	public bool Cmd { get; private set; }

	public event Action SetCommand;
	public event Action ResetCommandByTimer;
	public event Action ErrorCommand;
	public event Action AfterExeuqutingCommand;

	public Command(int t)
	{
		_cmdTimer.delay = t;
	}

	public Command(int t1, int t2)
	{
		_cmdTimer.delay = t1;
		_errorCMDTimer.delay = t2;
	}

	public void CommandWithCheck(bool _command, bool _resetCommand, bool _commandCompleted)
	{
		_commandCompletedForError = _commandCompleted;

		if (_command && !_lastCmd && !_commandCompleted)
		{
			Cmd = true;

			if (SetCommand != null) SetCommand();


			_cmdTimer.start(() => { Cmd = false; if (ResetCommandByTimer != null) ResetCommandByTimer(); _cmdTimer.reset(); });
			_errorCMDTimer.start(() => { if (!_commandCompletedForError) { if (ErrorCommand != null) ErrorCommand(); _errorCMDTimer.reset(); } });
		}

		if (_commandCompleted || _resetCommand)
		{
			Cmd = false;
			if (AfterExeuqutingCommand != null) AfterExeuqutingCommand();
		}

		_lastCmd = _command;

	}

	public void CommandWithoutCheck(bool _command)
	{

		if (_command && !_lastCmd)
		{
			Cmd = true;

			if (SetCommand != null) SetCommand();

			_cmdTimer.start(() => { Cmd = false; if (ResetCommandByTimer != null) ResetCommandByTimer(); _cmdTimer.reset(); });

		}

		_lastCmd = _command;

	}
}