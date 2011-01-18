namespace DcxStudioNet
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;
    
    public partial class FormTest : Form
    {
        private ControlType addNewType;
        private int idCounter;

		  // http://www.gamedev.net/community/forums/topic.asp?topic_id=216090
		 //[DllImport("User32.dll")]
		 //private static extern short GetAsyncKeyState(int keyCode);

        public FormTest(XDialog xdialog)
            : base()
        {
            InitializeComponent();

            idCounter = 1;
            this.Tag = xdialog;
        }

        private void FormTest_Click(object sender, EventArgs e)
        {
            // select the xdialog component
            if (this.addNewType == ControlType.UNKNOWN)
            {
                DcxsC.studio.selectControl(this.Tag);
                return;
            }

            // create a new control directly onto the dialog.
            MouseEventArgs mea = (MouseEventArgs) e;
            DcxControl ctrl = ControlFactory.CreateControl(null, mea.X, mea.Y);

            if (ctrl != null)
            {
                this.Controls.Add(ctrl.getControl());
                DcxsC.xdialog.getAllControls().Add(ctrl);

					 //if (!(GetAsyncKeyState(Keys.RShiftKey) || GetAsyncKeyState(Keys.LShiftKey)))
						this.addNewType = ControlType.UNKNOWN;
            }
        }

        /// <summary>
        /// Retrieves the type of control which is about to be added.
        /// </summary>
        /// <returns></returns>
        public ControlType getAddType()
        {
            return addNewType;
        }

        public void setAddType(String newType)
        {
            ControlType type = ControlFactory.getTypeFromString(newType);

            if (addNewType != type)
            {
                addNewType = type;
                DcxsC.studio.StatusText = "Adding " + type.ToString();
            }
            else
            {
                addNewType = ControlType.UNKNOWN;
                DcxsC.studio.StatusText = "Ready";
            }
        }

        /// <summary>
        /// Retrieves the next ID for the control, and increments afterwards.
        /// </summary>
        /// <returns>The next control ID.</returns>
        public int NextID
        {
            get { return idCounter++; }
        }

        public void resetIDCounter()
        {
            this.idCounter = 1;
        }

        public bool IsAdding
        {
            get { return (this.addNewType != ControlType.UNKNOWN); }
        }

        private void FormTest_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                DcxsC.xdialog.clearDialog();
                e.Cancel = true;
            }
        }
    }
}