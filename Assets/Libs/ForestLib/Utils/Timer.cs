using System;

namespace ForestLib.Utils
{
	/// <summary>
	/// Simple Timer utility class. Use me
	/// to count down passed time by Delta Time
	/// </summary>
	public class Timer
	{
		private enum State
		{
			Stopped,
			Running,
			Finished
		}

		public event Action OnRunning;
		public event Action OnFinished;
		public event Action OnStopped;
		
		/// <summary>
		/// The Time we're counting down
		/// </summary>
		private float _targetTime = 0.0f;

		/// <summary>
		/// The remaining time.
		/// </summary>
		private float _timeLeft = 0.0f;

		/// <summary>
		/// Flag to track if we're running or not
		/// </summary>
		private State _state = State.Stopped;

		/// <summary>
		/// Flag if the Timer is currently running
		/// </summary>
		public bool IsRunning => _state == State.Running;

		public float ElapsedTime => _targetTime - _timeLeft;
		
		public float TimeLeft => _timeLeft;

		/// <summary>
		/// Start the Timer, counting down from the past timed parameter
		/// </summary>
		/// <param name="time">The amount of time to count down</param>
		public void Start(float time)
		{
			_timeLeft = _targetTime = time;
			_state = State.Running;
			OnRunning?.Invoke();
		}

		public void Restart()
		{
			Start(_targetTime);
		}

		public void Stop()
		{
			_timeLeft = _targetTime;
			_state = State.Stopped;
			OnStopped?.Invoke();
		}
		
		/// <summary>
		/// Tick the timer and check if the Timer has elapsed.
		/// Will return false if the Timer isn't running.
		/// N.B Call me from mono Update
		/// </summary>
		/// <param name="dt">Delta Time</param>
		/// <returns>True if timer has completed, false if it's still running or is stopped</returns>
		public bool Elapsed (float dt)
		{
			if (_state == State.Stopped)
			{
				return false;
			}
			
			if (_state == State.Finished)
			{
				return true;
			}

			_timeLeft -= dt;
			
			if (_timeLeft <= 0)
			{
				_timeLeft = 0;
				_state = State.Finished;
				OnFinished?.Invoke();
			}

			return _state == State.Finished;
		}

		public void ClearEventListeners()
		{
			OnFinished = null;
			OnRunning = null;
			OnStopped = null;
		}
	}
}