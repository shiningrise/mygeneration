using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MyMeta;

namespace MyGeneration.UI.Plugins.EntityManager
{
    public partial class TableEditorControl : UserControl
    {
        public TableEditorControl()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public void BindData(IDatabases dbs)
        {
            if(dbs != null)
                this.dataGridView1.DataSource = dbs;
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }
    }
}
