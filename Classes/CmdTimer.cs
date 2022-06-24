using FB.FBAttributes;
using System.Timers;

public partial class CmdTimer
{
	private Timer timer = new Timer(4000) { AutoReset = false };

	public CmdTimer()
	{
		timer.Elapsed += timerElapsed;
	}

	public double delay
	{
		get
		{
			return timer.Interval;
		}
		set
		{
			timer.Interval = value;
		}
	}

	public delegate void TimerDelegate();

	private TimerDelegate elapseProc;

	public void start(TimerDelegate onElapsed)
	{
		elapseProc = onElapsed;
		timer.Stop();
		timer.Start();
	}

	public void reset()
	{
		timer.Stop();
	}

	private void timerElapsed(object source, ElapsedEventArgs e)
	{
		if (elapseProc != null)
		{
			elapseProc();
		}
	}
}