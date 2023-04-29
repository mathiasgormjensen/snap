// based on code from https://social.msdn.microsoft.com/Forums/sqlserver/en-US/c061954b-19bf-463b-a57d-b09c98a3fe7d/assign-global-hotkey-to-a-system-tray-application-in-c?forum=csharpgeneral
using System;

namespace Snap
{
	/// <summary>
	/// The enumeration of possible modifiers.
	/// </summary>
	[Flags]
	public enum HookModifierKeys : uint
	{
		Alt = 1,
		Control = 2,
		Shift = 4,
		Win = 8
	}
}
