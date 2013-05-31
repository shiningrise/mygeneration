using System;
using System.Xml;
using System.IO;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Globalization;
using System.Text;

using MyMeta;
using WeifenLuo.WinFormsUI.Docking;

namespace MyGeneration
{
    public partial class DefaultSettingsDialog : Form
    {
        private IMyGenerationMDI _mdi;

        public DefaultSettingsDialog(IMyGenerationMDI mdi)
        {
            InitializeComponent();

            this._mdi = mdi;
            this.defaultSettingsControl.ShowOLEDBDialog = new ShowOleDbDialogHandler(ShowOLEDBDialog);
            this.defaultSettingsControl.Initialize(mdi);
        }

        public string ShowOLEDBDialog(string cs)
        {
            return _mdi.PerformMdiFuntion(null, "ShowOLEDBDialog", cs) as String;
        }

        private void DefaultSettingsDialog_Load(object sender, EventArgs e)
        {
            this.defaultSettingsControl.Populate();
        }

        private void DefaultSettingsDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((e.CloseReason == CloseReason.UserClosing) ||
                (e.CloseReason == CloseReason.FormOwnerClosing))
            {
                DialogResult r = DialogResult.None;

                // Something's Changed since the load...
                if (defaultSettingsControl.SettingsModified)
                {
                    r = MessageBox.Show("Default settings have changed.\r\n Would you like to save before exiting?", "Default Settings Changed", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                }
                else if (defaultSettingsControl.ConnectionInfoModified)
                {
                    r = MessageBox.Show("The loaded connection profile has changed.\r\n Would you like to save before exiting?", "Connection Profile Changed", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                }

                if (r != DialogResult.None)
                {
                    if (r == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                    }
                    else if (r == DialogResult.Yes)
                    {
                        if (defaultSettingsControl.Save())
                        {
                            this.DialogResult = DialogResult.OK;
                            _mdi.SendAlert(null, "UpdateDefaultSettings");
                        }
                    }
                    else
                    {
                        defaultSettingsControl.Cancel();
                        this.DialogResult = DialogResult.Cancel;
                    }
                }
            }
        }
    }
}
