using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using MyGeneration;
using Zeus;

namespace MyGeneration.Forms
{
    public partial class ApplicationReleasesControl : UserControl
    {
        private static List<IAppRelease> releases;
        private delegate void AddGridDataCallback();
        private IAsyncResult asyncres;
        private int index = -1;

        public ApplicationReleasesControl()
        {
            InitializeComponent();
        }

        private void ApplicationReleasesControl_Load(object sender, EventArgs e)
        {
            SetupAsync();
        }

        private void dataGridViewUpdates_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == this.ColumnDownload.Index)
                {
                    Uri u = this.dataGridViewUpdates.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag as Uri;
                    WindowsTools.LaunchBrowser(u.AbsoluteUri, System.Diagnostics.ProcessWindowStyle.Normal, true);
                }
                else if (e.ColumnIndex == this.ColumnReleaseNotes.Index)
                {
                    Uri u = this.dataGridViewUpdates.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag as Uri;
                    WindowsTools.LaunchBrowser(u.AbsoluteUri, System.Diagnostics.ProcessWindowStyle.Normal, true);
                }
            }
        }

        private void timerImgAnimate_Tick(object sender, EventArgs e)
        {
            index = (index >= 3) ? 0 : (index + 1);
            switch (index)
            {
                case 0:
                    this.pictureBoxAnimation.Image = MyGeneration.Properties.Resources.Refresh16x16_1;
                    break;
                case 1:
                    this.pictureBoxAnimation.Image = MyGeneration.Properties.Resources.Refresh16x16_2;
                    break;
                case 2:
                    this.pictureBoxAnimation.Image = MyGeneration.Properties.Resources.Refresh16x16_3;
                    break;
                case 3:
                    this.pictureBoxAnimation.Image = MyGeneration.Properties.Resources.Refresh16x16_4;
                    break;
            }
            pictureBoxAnimation.Invalidate();
        }

        private bool IsBusy
        {
            get
            {
                return ((asyncres != null) && !asyncres.IsCompleted);
            }
        }

        public void SetupAsync()
        {
            if (IsBusy) return;

            this.dataGridViewUpdates.Rows.Clear();
            releases = null;
            timerImgAnimate.Start();

            this.Invalidate();
            this.Refresh();

            ThreadStart ts = new ThreadStart(SetupAndBuildReleaseList);
            asyncres = ts.BeginInvoke(new AsyncCallback(SetupAsyncCompleted), null);
        }

        private static void SetupAndBuildReleaseList()
        {
            if (releases == null)
            {
                try
                {
                    releases = ZeusController.Instance.ReleaseList;
                }
                catch (Exception ex)
                {
                    releases = null;
                }
            }
        }

        private void SetupAsyncCompleted(IAsyncResult ar)
        {
            this.AddToGridThreadSafe();
        }

        private void AddToGridThreadSafe()
        {
            if (this.dataGridViewUpdates.InvokeRequired)
            {
                AddGridDataCallback d = new AddGridDataCallback(AddToGridThreadSafe);
                this.dataGridViewUpdates.Invoke(d, new object[] { });
            }
            else
            {
                timerImgAnimate.Stop();

                try
                {
                    if (releases != null)
                    {
                        int newVersionCount = 0;
                        Version currentVersion = new Version(Application.ProductVersion);
                        foreach (IAppRelease rel in releases)
                        {
                            int i = this.dataGridViewUpdates.Rows.Add();

                            //this.dataGridViewUpdates.Rows[i].Cells[this.ColumnDate.Index].Value = rel.Date;
                            this.dataGridViewUpdates.Rows[i].Cells[this.ColumnTitle.Index].Value = rel.Title;
                            this.dataGridViewUpdates.Rows[i].Cells[this.ColumnDownload.Index].Tag = rel.DownloadLink;
                            this.dataGridViewUpdates.Rows[i].Cells[this.ColumnReleaseNotes.Index].Tag = rel.ReleaseNotesLink;

                            if (rel.AppVersion == currentVersion)
                            {
                                this.dataGridViewUpdates.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                            }
                            else if (rel.AppVersion < currentVersion)
                            {
                                this.dataGridViewUpdates.Rows[i].DefaultCellStyle.ForeColor = Color.Gray;
                            }
                            else
                            {
                                newVersionCount++;
                            }

                        }
                        if (newVersionCount == 0)
                        {
                            this.labelApplication.Text = "MyGeneration Releases on SourceForge";
                            this.labelApplication.ForeColor = Color.Gray;
                        }
                        else
                        {
                            this.labelApplication.Text = "New MyGeneration Release Available on SourceForge";
                            this.labelApplication.ForeColor = Color.Black;
                        }
                    }
                    else
                    {
                        // There is nothing to bind to the grid due to the connection error
                        this.labelApplication.Text = "Release information unavailable at this time.";
                        this.labelApplication.ForeColor = Color.Red;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private void pictureBoxAnimation_Click(object sender, EventArgs e)
        {
            SetupAsync();
        }
    }
}
