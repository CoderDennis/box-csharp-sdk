namespace BoxSync.Core
{
	class AsyncCallState<TCallbackDelegateType>
	{
		public object UserState
		{
			get; 
			set;
		}

		public TCallbackDelegateType CallbackDelegate
		{
			get; 
			set;
		}
	}
}
