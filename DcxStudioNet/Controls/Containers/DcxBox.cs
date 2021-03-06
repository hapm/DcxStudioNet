namespace DcxStudioNet
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Forms;
    using System.Collections;
    
    public class DcxBox : DcxContainer
    {
        private GroupBox control;

        public DcxBox()
            : base()
        {
        }

        public DcxBox(DcxContainer container, int id)
            : base(container, id)
        {
            this.control = new GroupBox();
            this.control.Text = "Box " + id;
            this.control.Name = id.ToString();

            this.controlType = ControlType.Box;

            setupControlActionListeners(this.control);
        }

        /*#region PropertyGrid Properties to expose
        //[Category(DcxsC.CATEGORY_CHECK), XmlAttribute()]
        //public bool Checked
        //{
        //    get { return this.control.Checked; }
        //    set { this.control.Checked = value; }
        //}
        #endregion*/

        #region Script generation
        public override void generateControlScript(List<string> writeTo)
        {
            writeTo.Add(string.Format("xdid -t $dname {0} {1}", this.ControlID, ctrl.Text));
        }

        public override string generateChildScript(int index, DcxControl ctrl)
        {
            // xdid -c [DNAME] [ID] [CID] [CONTROL] [X] [Y] [W] [H] (OPTIONS)
            return string.Format(
                "xdid -c $dname {0} {1} {2} {3} {4} {5} {6}",
                this.ControlID, // 0
                ctrl.ControlID, // 1
                ctrl.ControlType.ToString().ToLower(), // 2
                ctrl.getControl().Left, // 3
                ctrl.getControl().Top, // 4
                ctrl.getControl().Width, // 5
                ctrl.getControl().Height); // 6
        }
        #endregion
    }
}
