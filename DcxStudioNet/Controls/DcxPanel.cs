using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace DcxStudio2
{
    public class DcxPanel : DcxContainer
    {
        private Panel control;

        public DcxPanel()
            : base()
        {
        }

        public DcxPanel(DcxContainer container, int id)
            : base(container, id)
        {
            control = new Panel();
            control.Text = "Box " + id;
            control.Name = id.ToString();
            //control.BorderStyle = BorderStyle.FixedSingle;

            this.controlType = ControlType.Panel;
            this.control.Paint += new PaintEventHandler(control_Paint);

            setupControlActionListeners(this.control);
        }

        #region PropertyGrid Properties to expose
        //[Category(DcxsC.CATEGORY_CHECK), XmlAttribute()]
        //public bool Checked
        //{
        //    get { return this.control.Checked; }
        //    set { this.control.Checked = value; }
        //}
        #endregion

        #region Script generation
        public override void generateControlScript(List<String> writeTo)
        {
            writeTo.Add(String.Format("xdid -l $dname {0} {1}", this.ControlID, "FAKE CLA SHIT"));
        }

        public override string generateChildScript(int index, DcxControl ctrl)
        {
            // xdid -c [DNAME] [ID] [CID] [CONTROL] [X] [Y] [W] [H] (OPTIONS)

            return String.Format("xdid -c $dname {0} {1} {2} {3} {4} {5} {6}",
                this.ControlID, // 0
                ctrl.ControlID, // 1
                ctrl.ControlType.ToString().ToLower(), // 2
                ctrl.getControl().Left, // 3
                ctrl.getControl().Top, // 4
                ctrl.getControl().Width, // 5
                ctrl.getControl().Height); // 6
        }
        #endregion

        public void control_Paint(Object sender, PaintEventArgs e)
        {
            if (!this.mouseOver && !this.mouseMoving)
            {
                Rectangle rect = ctrl.ClientRectangle;

                rect.Width--;
                rect.Height--;

                Pen pen = new Pen(Brushes.Black);
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

                e.Graphics.DrawRectangle(pen, rect);
            }
        }
    }
}
