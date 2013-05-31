using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MyGenVS2005
{
    public partial class AddInErrorForm : Form
    {
        private List<MyGeneration.IMyGenError> _errors = new List<MyGeneration.IMyGenError>();
        private int _idx = 0;

        public AddInErrorForm(params Exception[] exs)
        {
            InitializeComponent();
            foreach (Exception ex in exs)
            {
                _errors.AddRange(MyGeneration.MyGenError.CreateErrors(ex));
            }
        }

        public List<MyGeneration.IMyGenError> Errors
        {
            get { return _errors; }
            set { _errors = value; }
        }

        private void AddInErrorForm_Load(object sender, EventArgs e)
        {
            Update();
        }

        private void Update()
        {
            bool vis = (_errors.Count > 1);

            this.buttonPrevious.Enabled = vis;
            this.buttonNext.Enabled = vis;

            if (_errors.Count > 0)
            {
                this.errorDetailControl1.Update(_errors[_idx]);
            }
            else
            {
                List<MyGeneration.IMyGenError> err = MyGeneration.MyGenError.CreateErrors(new Exception("What the heck is going on! There are no errors."));
                this.errorDetailControl1.Update(err[0]); 
            }
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (_errors.Count > 0)
            {
                _idx++;
                if (_idx >= _errors.Count) _idx = 0;
                Update();
            }
        }

        private void buttonPrevious_Click(object sender, EventArgs e)
        {
            if (_errors.Count > 0)
            {
                _idx--;
                if (_idx < 0) _idx = _errors.Count - 1;
                Update();
            }
        }
    }
}