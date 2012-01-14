using System;

namespace MerchantTribe.Commerce.Content
{

	public class BVModuleEventArgs : EventArgs
	{

		private string _info = string.Empty;

		public string Info {
			get { return _info; }
			set { _info = value; }
		}

		public BVModuleEventArgs()
		{

		}

		public BVModuleEventArgs(string data)
		{
			if (data != null) {
				_info = data;
			}
		}

	}

	public abstract class BVModule : BVUserControl
	{

		private string _BlockId = string.Empty;

		public event EventHandler<BVModuleEventArgs> EditingComplete;
        
		public string BlockId {
			get { return _BlockId; }
			set {
				_BlockId = value;
			}
		}

		protected BVModule()
		{

		}

		protected void NotifyFinishedEditing()
		{
			if (EditingComplete != null) {
				EditingComplete(this, new BVModuleEventArgs());
			}
		}

		protected void NotifyFinishedEditing(string info)
		{
			if (EditingComplete != null) {
				EditingComplete(this, new BVModuleEventArgs(info));
			}
		}

	}

}
