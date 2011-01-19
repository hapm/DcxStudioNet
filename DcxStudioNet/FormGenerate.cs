namespace DcxStudioNet
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;

    public partial class FormGenerate : Form
    {
        ////private string template;

        public FormGenerate()
        {
            ////template = System.IO.File.ReadAllText("template.txt");
            InitializeComponent();
        }

        private void FormGenerate_FormClosing(object sender, FormClosingEventArgs e)
        {
            DcxsC.generate = null;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormGenerate_Load(object sender, EventArgs e)
        {
            List<string> scriptLines = new List<string>();
            StringBuilder str = new StringBuilder();

            foreach (DcxControl ctrl in DcxsC.xdialog.getAllControls())
            {
                generateScripts(ctrl, scriptLines);
            }

            foreach (string line in scriptLines) {
                str.Append(line);
                str.Append("\r\n");
            }

            this.txtScript.Text = str.ToString();
        }

        private void generateScripts(DcxControl ctrl, List<string> writeTo)
        {
            int counter = 0;

            if (writeTo.Count > 0)
                writeTo.Add(string.Empty);

            writeTo.Add(string.Format(";// Creating {0} (ID: {1})", ctrl.ControlType, ctrl.ControlID));
            writeTo.Add(ctrl.generateConstructorScript(counter));
            ctrl.generateControlScript(writeTo);

            if (ctrl is DcxContainer)
            {
                foreach (DcxControl child in ((DcxContainer)ctrl).getChildren())
                {
                    writeTo.Add(string.Empty);
                    generateScripts(child, writeTo);
                }
            }
        }
    }
}