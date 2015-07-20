using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;

namespace MainInterface
{
    public partial class MainForm : Form
    {
		[Serializable]
		private class MySettings
		{
		}

		private FileVersionInfo assemblyInfo;

		private bool intentionalClose;

		private MySettings settings;
		
        public MainForm()
        {
            InitializeComponent();

			intentionalClose = false;

			assemblyInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);

			LoadSettings();
        }

		private void LoadSettings()
		{
			settings = (MySettings)CommonTypes.SettingsManager.LoadSettings(assemblyInfo.CompanyName, assemblyInfo.ProductName) ?? new MySettings();
		}

		private void StoreSettings()
		{
			CommonTypes.SettingsManager.StoreSettings(assemblyInfo.CompanyName, assemblyInfo.ProductName, settings);
		}


		private void HideMe()
		{
			Hide();
			WindowState = FormWindowState.Minimized;
		}

		private void ShowMe()
		{
			WindowState = FormWindowState.Normal;
			Show();
			Activate();
		}

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
			if (!intentionalClose)
			{
				bool cancel = false;

				switch (e.CloseReason)
				{
					case CloseReason.UserClosing: cancel = true; break;
					default: cancel = false; break;
				}
				if (cancel)
				{
					e.Cancel = true;
					HideMe();
				}
			}
        }

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ShowMe();
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			intentionalClose = true;
			Close();
		}

		private void notifyIcon1_DoubleClick(object sender, EventArgs e)
		{
			ShowMe();
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == Platform.PlatformApi.WM_SHOWME)
				ShowMe();
			base.WndProc(ref m);
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (AboutBox1 form = new AboutBox1())
				form.ShowDialog();
		}

		private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			StoreSettings();
		}
    }
}