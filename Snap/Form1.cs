using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms;
using WindowsInput.Native;

namespace Snap
{
	public partial class Form1 : Form
	{
		KeyboardHook hook = new KeyboardHook();

		public Form1()
		{
			InitializeComponent();

			hook.KeyPressed += new EventHandler<KeyPressedEventArgs>(hook_KeyPressed);
			hook.RegisterHotKey(HookModifierKeys.Control | HookModifierKeys.Alt, Keys.Left);
			hook.RegisterHotKey(HookModifierKeys.Control | HookModifierKeys.Alt, Keys.Up);
			hook.RegisterHotKey(HookModifierKeys.Control | HookModifierKeys.Alt, Keys.Right);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if (Properties.Settings.Default.NotificationOnStartup)
				this.tsmiAbout.PerformClick();
		}

		private void hook_KeyPressed(object sender, KeyPressedEventArgs e)
		{
			if (e.Key == Keys.Left)
				this.SnapLeft();
			else if (e.Key == Keys.Up)
				this.SnapUp();
			else if (e.Key == Keys.Right)
				this.SnapRight();
		}

		private void SnapLeft()
		{
			this.Snap(Properties.Settings.Default.LeftGroupIndex, Properties.Settings.Default.LeftItemIndex);
		}

		private void SnapUp()
		{
			this.Snap(Properties.Settings.Default.UpGroupIndex, Properties.Settings.Default.UpItemIndex);
		}

		private void SnapRight()
		{
			this.Snap(Properties.Settings.Default.RightGroupIndex, Properties.Settings.Default.RightItemIndex);
		}

		private void Snap(int first, int second)
		{
			var element = System.Windows.Automation.AutomationElement.FocusedElement;
			if (Properties.Settings.Default.DetectAndIgnoreeWindowsSnapWindow)
			{
				try
				{
					if (element.Current.FrameworkId == "XAML" && element.Current.ClassName == "ListViewItem")
					{
						Serilog.Log.Information($"Current element is part of Windows' snap-feature and is therefore ignored");
						return;
					}
				}
				catch (Exception ex)
				{
					Serilog.Log.Warning($"Failed to detect whether current element is related to Windows' snap-feature: {ex}");
				}
			}

			try
			{
				Serilog.Log.Information($"Current element: {element?.Current.Name}");
			}
			catch (Exception ex)
			{
				Serilog.Log.Warning($"Failed to log information about current element: {ex}");
			}

			Serilog.Log.Information("Releasing pressed keys");

			WindowsInput.InputSimulator simulator = new WindowsInput.InputSimulator();
			if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
			{
				simulator.Keyboard.KeyUp(VirtualKeyCode.CONTROL);
				simulator.Keyboard.KeyUp(VirtualKeyCode.LCONTROL);
				simulator.Keyboard.KeyUp(VirtualKeyCode.LCONTROL);
			}

			if ((Control.ModifierKeys & Keys.Alt) == Keys.Alt)
			{
				simulator.Keyboard.KeyUp(VirtualKeyCode.LMENU);
				simulator.Keyboard.KeyUp(VirtualKeyCode.MENU);
				simulator.Keyboard.KeyUp(VirtualKeyCode.RMENU);
			}

			Serilog.Log.Information("Pressing WIN+Z");
			System.Threading.Thread.Sleep(Properties.Settings.Default.SleepBeforeWinZ);
			simulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.RWIN, VirtualKeyCode.VK_Z);
			System.Threading.Thread.Sleep(Properties.Settings.Default.SleepAfterWinZ);

			try
			{
				for (int i = 0; i < 10; i++)
				{
					Serilog.Log.Information($"Locating flyout (attempt {(i + 1)}...)");
					element = System.Windows.Automation.AutomationElement.FocusedElement;
					Serilog.Log.Information($"Current element: {element?.Current.Name}");
					var current = element?.Current;
					Serilog.Log.Information(current?.Name);
					if (current?.AutomationId == "SnapFlyoutControl")
					{
						var group = element.FindAll(System.Windows.Automation.TreeScope.Children, System.Windows.Automation.Condition.TrueCondition)[first];
						Serilog.Log.Information($"Group #{first}: {group.Current.Name}");
						var item = group.FindAll(TreeScope.Children, System.Windows.Automation.Condition.TrueCondition)[second];
						Serilog.Log.Information($"Item #{first}: {item.Current.Name}");
						var invokePattern = item.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
						Serilog.Log.Information($"Clicking {item.Current.Name}");
						invokePattern.Invoke();
						return;
					}
					System.Threading.Thread.Sleep(Properties.Settings.Default.SleepBetweenAttempts);
				}

				Serilog.Log.Warning($"Snap flyout not detected");
				this.notifyIcon.ShowBalloonTip(0, "Snap!", "Snap flyout not detected", ToolTipIcon.Warning);
			}
			catch (Exception ex)
			{
				Serilog.Log.Warning(ex.ToString());
				this.notifyIcon.ShowBalloonTip(0, "Snap!", ex.Message, ToolTipIcon.Error);
			}
		}

		private void closeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}


		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.notifyIcon.ShowBalloonTip(0, "Snap!", @"Snap left: CTRL+ALT+LEFT
Snap middle: CTRL+ALT+UP
Snap right: CTRL+ALT+RIGHT", ToolTipIcon.Info);
		}
	}
}