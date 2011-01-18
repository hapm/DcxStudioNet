using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DcxStudio2
{
    public partial class FormGenerate : Form
    {
        //private String template;

        public FormGenerate()
        {
            //template = System.IO.File.ReadAllText("template.txt");
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
            List<String> scriptLines = new List<String>();
            StringBuilder str = new StringBuilder();

            foreach (DcxControl ctrl in DcxsC.xdialog.getAllControls())
            {
                generateScripts(ctrl, scriptLines);
            }

            foreach (String line in scriptLines){
                str.Append(line);
                str.Append("\r\n");
            }

            txtScript.Text = str.ToString();
        }

        private void generateScripts(DcxControl ctrl, List<String> writeTo)
        {
            int counter = 0;

            if (writeTo.Count > 0)
                writeTo.Add("");

            writeTo.Add(String.Format(";// Creating {0} (ID: {1})", ctrl.ControlType, ctrl.ControlID));
            writeTo.Add(ctrl.generateConstructorScript(counter));
            ctrl.generateControlScript(writeTo);

            if (ctrl is DcxContainer)
            {
                foreach (DcxControl child in ((DcxContainer)ctrl).getChildren())
                {
                    writeTo.Add("");
                    generateScripts(child, writeTo);
                }
            }
        }
    }
}