namespace DcxStudioNet
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;
    using System.Xml.Serialization;
    using System.IO;

    public partial class DcxStudio : Form
    {
        public SaveFileDialog saveFile;
        public OpenFileDialog openFile;

        public DcxStudio()
        {
            InitializeComponent();
        }

        #region Auto generated MDI code
        /*//private void ShowNewForm(object sender, EventArgs e)
        //{
        //    // Create a new instance of the child form.
        //    Form childForm = new Form();
        //    // Make it a child of this MDI form before showing it.
        //    childForm.MdiParent = this;
        //    childForm.Text = "Window " + childFormNumber++;
        //    childForm.Show();
        //}

        //private void OpenFile(object sender, EventArgs e)
        //{
        //    OpenFileDialog openFileDialog = new OpenFileDialog();
        //    openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        //    openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
        //    if (openFileDialog.ShowDialog(this) == DialogResult.OK)
        //    {
        //        string FileName = openFileDialog.FileName;
        //        // TODO: Add code here to open the file.
        //    }
        //}*/

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                // TODO: Add code here to save the current contents of the form to a file.
                string fileName = saveFileDialog.FileName;
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
/*
        ////private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        ////{
        ////    // TODO: Use System.Windows.Forms.Clipboard to insert the selected text or images into the clipboard
        ////}

        //private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    // TODO: Use System.Windows.Forms.Clipboard to insert the selected text or images into the clipboard
        //}

        //private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    // TODO: Use System.Windows.Forms.Clipboard.GetText() or System.Windows.Forms.GetData to retrieve information from the clipboard.
        //}*/

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tbMain.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusbar.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }
        #endregion

        private void DcxStudio_Load(object sender, EventArgs e)
        {
            DcxsC.studio = this;
            DcxsC.xdialog = new XDialog();
            DcxsC.dialog = new FormTest(DcxsC.xdialog);
            DcxsC.studio.selectControl(DcxsC.xdialog);

            DcxsC.dialog.MdiParent = this;
            DcxsC.dialog.Show();
            DcxsC.dialog.BringToFront();

            this.Text += DcxsC.version;

            this.saveFile = new SaveFileDialog();
            this.openFile = new OpenFileDialog();

            this.saveFile.FileName = DcxsC.defaultProjectName;
            this.saveFile.DefaultExt = "dcxs";

            ////openFile.FileName = lastOpenedProject;
            this.openFile.DefaultExt = "dcxs";
        }

        public void selectControl(Object ctrl)
        {
            this.controlProperties.SelectedObject = ctrl;
        }

        #region Control listview events
        private void lvControls_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.lvControls.SelectedItems.Count < 1) 
            {
                return;
            }
            
            ListViewItem item = this.lvControls.SelectedItems[0];
            DcxsC.dialog.setAddType(item.Text.Trim().ToLower());
        }
        #endregion

        #region Public access to statusbar
        public String StatusText
        {
            get { return this.statusLabel.Text; }
            set { this.statusLabel.Text = value; }
        }

        public int StatusProgress
        {
            get { return this.statusPbar.Value; }
            set { this.statusPbar.Value = value; }
        }
        #endregion

        #region Main toolbar/dropdown menu events
        private void tbbNew_Click(object sender, EventArgs e)
        {
            DcxsC.xdialog.clearDialog();
        }

        private void tbbGenerate_Click(object sender, EventArgs e)
        {
            if (DcxsC.generate != null)
            {
                DcxsC.generate.Activate();
                return;
            }

            DcxsC.generate = new FormGenerate();
            DcxsC.generate.Show(DcxsC.studio);
        }

        private void tbbSave_Click(object sender, EventArgs e)
        {
            // user clicked ok
            if (DialogResult.OK != saveFile.ShowDialog(DcxsC.studio))
            {
                return;
            }

            try
            {
                // Serialization
                XmlSerializer s = new XmlSerializer(typeof(XDialog));
                TextWriter w = new StreamWriter(saveFile.FileName);
                s.Serialize(w, DcxsC.xdialog);
                w.Close();
            }
            catch (InvalidOperationException ioe)
            {
                MessageBox.Show(ioe.Message);
            }
        }

        private void tbbOpen_ButtonClick(object sender, EventArgs e)
        {
            OpenProject();
        }

        private void importMircScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Make import work");
        }

        private void tbbHelp_ButtonClick(object sender, EventArgs e)
        {
            MessageBox.Show("about dcxs");
        }
        #endregion

        private void OpenProject()
        {
            MessageBox.Show("Make open projct work");
        }
    }
}
