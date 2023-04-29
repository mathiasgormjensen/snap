// based on code from https://social.msdn.microsoft.com/Forums/sqlserver/en-US/c061954b-19bf-463b-a57d-b09c98a3fe7d/assign-global-hotkey-to-a-system-tray-application-in-c?forum=csharpgeneral
using System;
using System.Windows.Forms;

namespace Snap
{
	/// <summary>
	/// Event Args for the event that is fired after the hot key has been pressed.
	/// </summary>
	public class KeyPressedEventArgs : EventArgs
	{
		private HookModifierKeys _modifier;
		private Keys _key;

		internal KeyPressedEventArgs(HookModifierKeys modifier, Keys key)
		{
			_modifier = modifier;
			_key = key;
		}

		public HookModifierKeys Modifier
		{
			get { return _modifier; }
		}

		public Keys Key
		{
			get { return _key; }
		}
	}
}
