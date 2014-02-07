using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Hlt.IO
{
	/// <summary>
	/// 此類別可用來執行檔案。
	/// </summary>
	public class FileRunner : IDisposable
	{
		private ProcessStartInfo _startInfo;
		private Process _process;
		private bool _needWait;		// 是否等待應用程式執行結束
		private int _waitTime;			// 要等多久（seconds），0 表示採用內定值（30分鐘）

		private string _errMsg;
		private StringBuilder _stdError;
		private StringBuilder _stdOutput;
		private event DataReceivedEventHandler _stdOutputReceivedEvent;
		private event EventHandler _exitedEvent;

		public FileRunner()
		{
			_stdError = new StringBuilder();
			_stdOutput = new StringBuilder();

			_startInfo = new ProcessStartInfo();

			_needWait = true;
			_waitTime = 0;
			_startInfo.CreateNoWindow = false;
			_startInfo.UseShellExecute = false;
			_startInfo.WorkingDirectory = "";
			_startInfo.WindowStyle = ProcessWindowStyle.Normal;
			_startInfo.RedirectStandardError = false;
			_startInfo.RedirectStandardOutput = false;
		}

		public void Dispose()
		{
			if (_process != null) 
			{
				KillProcess();
			}
		}

		/// <summary>
		/// 執行檔案。
		/// </summary>
		/// <param name="filename">檔案名稱。</param>
		/// <param name="argument">檔案執行時帶的參數，可傳空字串。</param>
		/// <returns>若執行錯誤，則設定錯誤訊息並傳回 false。</returns>
		public bool Run(string filename, string argument)
		{
			if (Running)
			{
				_errMsg = "先前執行的程式尚未結束!";
				return false;
			}

			_errMsg = "";
			_stdError.Length = 0;
			_stdOutput.Length = 0;

			if (String.IsNullOrEmpty(filename))
			{
				throw new ArgumentException("未指定欲執行的檔案名稱!");
			}

			// 不要檢查檔案是否存在，因為有些是在 PATH 路徑下的檔案
			/*
			if (!File.Exists(filename))
			{
				m_ErrMsg = "指定的檔案不存在: " + filename;
				return false;
			}*/

			if (!String.IsNullOrEmpty(WorkingDirectory))
			{
				if (!Directory.Exists(WorkingDirectory))
				{
					throw new InvalidOperationException("指定的工作路徑不存在: " + WorkingDirectory);
				}
			}

			_startInfo.FileName = filename;
			_startInfo.Arguments = argument;

			_process = new Process();
			_process.StartInfo = _startInfo;

			// 訂閱 process 事件
			_process.Exited += new EventHandler(Process_Exited); 
			_process.OutputDataReceived += new DataReceivedEventHandler(Process_OutputDataReceived);
			_process.ErrorDataReceived += new DataReceivedEventHandler(Process_ErrorDataReceived);

			try
			{				
				if (_process.Start())
				{
					if (RedirectStandardOutput)	// 需要重新導向 console 輸出
					{
						_process.BeginOutputReadLine();
					}
					// 不重新導向 console 輸出，那麼等待程式執行完畢的機制必須另尋他法
					if (_needWait)
					{
						WaitToKill();
						KillProcess();						
					}
					return true;
				}
				else
				{
					_errMsg = "沒有啟動新的處理序（可能重複使用既有的處理序）。";
					return false;
				}
			}
			catch (Exception ex)
			{
				_process.Close();
				_process.Dispose();
				_process = null;
				throw ex;
			}			
		}

		void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
		{
			lock (this)
			{
				_stdOutput.Append(e.Data);
				_stdOutput.Append(System.Environment.NewLine);
			}
			OnStdOutputReceived(e);
		}

		void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
		{
			// Standard error 一樣也輸出到 standard output device.
			Process_OutputDataReceived(sender, e);
		}


		private void Process_Exited(object sender, EventArgs e)
		{
			OnProcessExited(e);
		}

		private void OnProcessExited(EventArgs e)
		{
			if (_exitedEvent != null)
			{
				_exitedEvent(this, e);
			}
		}

		private void WaitToKill()
		{
			int waitTimeToKill;

			if (_waitTime <= 0)	// 如果沒有指定等待時間
			{
				waitTimeToKill = 1000 * 60 * 30;	// 則預設等待 30 分鐘
			}
			else
			{
				waitTimeToKill = 1000 * _waitTime;
			}
			_process.WaitForExit(waitTimeToKill);
		}

		private void OnStdOutputReceived(DataReceivedEventArgs args)
		{
			if (_stdOutputReceivedEvent != null)
			{				
				// 觸發用戶端事件（Note: 用戶端若要在此事件中更新 UI，務必 marshal 回主執行緒!）
				_stdOutputReceivedEvent(this, args);

				// 注意：這裡一定要延遲一下，拖延此事件所屬的 worker thread 的速度，
				// 因為 UpdateUI 會導致控制項更新，這更新要花些時間，如果 worker thread 速度很快地
				// 一直塞資料進來，user 又用滑鼠在視窗上東點西點的，會導致整個 main form 訊息佇列
				// 來不及處理，而讓程式看起來像當掉一樣。註: 用 Application.DoEvents() 沒有幫助。

				System.Threading.Thread.Sleep(10);	
			}
		}

		/// <summary>
		/// 強制終止執行的程式。
		/// </summary>
		public void KillProcess()
		{
			if (_process == null)
				return;

			if (_process.HasExited == false)
			{
				_process.Kill();
				_errMsg = "執行的應用程式超過指定等待時間而被強制終止!";
			}

			// 解除訂閱事件
			_process.Exited -= this.Process_Exited;
			_process.OutputDataReceived -= this.Process_OutputDataReceived;
			_process.ErrorDataReceived -= this.Process_ErrorDataReceived;

			_process.Close();
			_process.Dispose();
			_process = null;
		}

		#region 屬性------------------------------

		public bool Running
		{
			get 
			{ 
				if (_process == null)
					return false;
				return !_process.HasExited;
			}
		}

		public string WorkingDirectory
		{
			get { return _startInfo.WorkingDirectory; }
			set { _startInfo.WorkingDirectory = value; }
		}

		public bool UseShellExecute
		{
			get { return _startInfo.UseShellExecute; }
			set 
			{
				if (value && RedirectStandardOutput)
				{
					throw new InvalidOperationException("UseShellExecute 和 RedirectStandardOutput 不可同時為 true!");
				}
				_startInfo.UseShellExecute = value; 
			}
		}

		public bool ShowWindow
		{
			get { return !_startInfo.CreateNoWindow; }
			set { _startInfo.CreateNoWindow = !value; }
		}

		public bool RedirectStandardOutput
		{
			get { return _startInfo.RedirectStandardOutput; }
			set
			{
				if (value && UseShellExecute)
				{
					throw new InvalidOperationException("UseShellExecute 和 RedirectStandardOutput 不可同時為 true!");
				}

				_startInfo.RedirectStandardOutput = value;
				_startInfo.RedirectStandardError = value;
			}
		}

		public bool NeedWait
		{
			get { return _needWait; }
			set { _needWait = value; }
		}

		/// <summary>
		/// 要等多久（seconds），0 表示採用內定值（30分鐘）。超過此時間 process 將被強制終止。
		/// </summary>
		public int WaitTime
		{
			get { return _waitTime; }
			set
			{
				if (value < 0 || value > (24 * 60 * 60))
				{
					throw new Exception("WaitTime 不可超過 24 小時!");
				}
				_waitTime = value;
			}
		}

		public string ErrorMsg
		{
			get { return _errMsg; }
		}

		public string StdOutputMsg
		{
			get
			{
				return _stdOutput.ToString();
			}
		}

		public ProcessWindowStyle WindowStyle
		{
			get { return _startInfo.WindowStyle; }
			set { _startInfo.WindowStyle = value; }
		}

		public string Verb
		{
			get { return _startInfo.Verb; }
			set { _startInfo.Verb = value; }
		}

		public bool ErrorDialog
		{
			get { return _startInfo.ErrorDialog; }
			set { _startInfo.ErrorDialog = value; }
		}

		public int ExitCode 
		{
			get 
			{ 
				if (_process == null)
					throw new Exception("沒有執行程式或程式已完全釋放，故無法取得 ExitCode。");
				return _process.ExitCode;
			}
		}

		public DateTime ExitTime
		{
			get
			{
				if (_process == null)
					throw new Exception("沒有執行程式或程式已完全釋放，故無法取得 ExitTime。");
				return _process.ExitTime;
			}
		}

		#endregion

		#region 事件--------------------------------

		/// <summary>
		/// 執行的程式「正常」結束時會觸發此事件。若程式被元件強制終止（直接 Kill），則不會觸發此事件。
		/// 注意：不要在此事件中使用類似 MessageBox.Show 的函式，程式可能會出現怪問題。
		/// </summary>
		public event EventHandler ProcessExited 
		{
			add
			{
				_exitedEvent += value;
			}
			remove
			{
				_exitedEvent -= value;
			}
		}

		public event DataReceivedEventHandler StdOutputReceived
		{
			add
			{
				_stdOutputReceivedEvent += value;
			}
			remove
			{
				_stdOutputReceivedEvent -= value;
			}
		}

		#endregion
	}
}
