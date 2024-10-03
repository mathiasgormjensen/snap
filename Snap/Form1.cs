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
		private readonly KeyboardHook _hook = new KeyboardHook();

		public Form1()
		{
			InitializeComponent();

			_hook.KeyPressed += new EventHandler<KeyPressedEventArgs>(hook_KeyPressed);
			_hook.RegisterHotKey(HookModifierKeys.Control | HookModifierKeys.Alt, Keys.Left);
			_hook.RegisterHotKey(HookModifierKeys.Control | HookModifierKeys.Alt, Keys.Up);
            _hook.RegisterHotKey(HookModifierKeys.Control | HookModifierKeys.Alt, Keys.Right);
            if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.NumPad1))
                _hook.RegisterHotKey(HookModifierKeys.Control | HookModifierKeys.Alt, Keys.NumPad1);
            if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.NumPad2))
                _hook.RegisterHotKey(HookModifierKeys.Control | HookModifierKeys.Alt, Keys.NumPad2);
            if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.NumPad3))
                _hook.RegisterHotKey(HookModifierKeys.Control | HookModifierKeys.Alt, Keys.NumPad3);
            if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.NumPad4))
                _hook.RegisterHotKey(HookModifierKeys.Control | HookModifierKeys.Alt, Keys.NumPad4);
            if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.NumPad5))
                _hook.RegisterHotKey(HookModifierKeys.Control | HookModifierKeys.Alt, Keys.NumPad5);
            if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.NumPad6))
                _hook.RegisterHotKey(HookModifierKeys.Control | HookModifierKeys.Alt, Keys.NumPad6);
            if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.NumPad7))
                _hook.RegisterHotKey(HookModifierKeys.Control | HookModifierKeys.Alt, Keys.NumPad7);
            if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.NumPad8))
                _hook.RegisterHotKey(HookModifierKeys.Control | HookModifierKeys.Alt, Keys.NumPad8);
            if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.NumPad9))
                _hook.RegisterHotKey(HookModifierKeys.Control | HookModifierKeys.Alt, Keys.NumPad9);
            if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.NumPad0))
                _hook.RegisterHotKey(HookModifierKeys.Control | HookModifierKeys.Alt, Keys.NumPad0);
        }

        protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if (Properties.Settings.Default.NotificationOnStartup)
				this.tsmiAbout.PerformClick();

			base.Hide();
		}

		private void hook_KeyPressed(object sender, KeyPressedEventArgs e)
		{
            try
            {
                if (e.Key == Keys.Left)
                    this.SnapLeft();
                else if (e.Key == Keys.Up)
                    this.SnapUp();
                else if (e.Key == Keys.Right)
                    this.SnapRight();
                else if (e.Key == Keys.NumPad1)
                    NumPad1();
                else if (e.Key == Keys.NumPad2)
                    NumPad2();
                else if (e.Key == Keys.NumPad3)
                    NumPad3();
                else if (e.Key == Keys.NumPad4)
                    NumPad4();
                else if (e.Key == Keys.NumPad5)
                    NumPad5();
                else if (e.Key == Keys.NumPad6)
                    NumPad6();
                else if (e.Key == Keys.NumPad7)
                    NumPad7();
                else if (e.Key == Keys.NumPad8)
                    NumPad8();
                else if (e.Key == Keys.NumPad9)
                    NumPad9();
                else if (e.Key == Keys.NumPad0)
                    NumPad0();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Snap!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); ;
            }
        }

        private void NumPad1()
        {
            System.Diagnostics.Process.Start(Properties.Settings.Default.NumPad1);
        }

        private void NumPad2()
        {
            System.Diagnostics.Process.Start(Properties.Settings.Default.NumPad2);
        }

        private void NumPad3()
        {
            System.Diagnostics.Process.Start(Properties.Settings.Default.NumPad3);
        }

        private void NumPad4()
        {
            System.Diagnostics.Process.Start(Properties.Settings.Default.NumPad4);
        }

        private void NumPad5()
        {
            System.Diagnostics.Process.Start(Properties.Settings.Default.NumPad5);
        }

        private void NumPad6()
        {
            System.Diagnostics.Process.Start(Properties.Settings.Default.NumPad6);
        }

        private void NumPad7()
        {
            System.Diagnostics.Process.Start(Properties.Settings.Default.NumPad7);
        }

        private void NumPad8()
        {
            System.Diagnostics.Process.Start(Properties.Settings.Default.NumPad8);
        }

        private void NumPad9()
        {
            System.Diagnostics.Process.Start(Properties.Settings.Default.NumPad9);
        }

        private void NumPad0()
        {
            System.Diagnostics.Process.Start(Properties.Settings.Default.NumPad0);
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
						var children = element.FindAll(System.Windows.Automation.TreeScope.Children, System.Windows.Automation.Condition.TrueCondition);
						var offset = children.Count - 6;
						var group = children[first + offset];
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