using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace FC_Diseño_de_Nervios.Controles
{
    public class AdvancedRadioButton: CheckBox
    {
        public enum Level { Parent, Form };

        [Category("AdvancedRadioButton"),
        Description("Gets or sets the level that specifies which RadioButton controls are affected."),
        DefaultValue(Level.Parent)]
        public Level GroupNameLevel { get; set; }

        [Category("AdvancedRadioButton"),
        Description("Gets or sets the name that specifies which RadioButton controls are mutually exclusive.")]
        public string GroupName { get; set; }

        protected override void OnCheckedChanged(EventArgs e)
        {
            base.OnCheckedChanged(e);

            if (Checked)
            {
                var arbControls = (dynamic)null;
                switch (GroupNameLevel)
                {
                    case Level.Parent:
                        if (this.Parent != null)
                            arbControls = GetAll(this.Parent, typeof(AdvancedRadioButton));
                        break;
                    case Level.Form:
                        Form form = this.FindForm();
                        if (form != null)
                            arbControls = GetAll(this.FindForm(), typeof(AdvancedRadioButton));
                        break;
                }
                if (arbControls != null)
                    foreach (Control control in arbControls)
                        if (control != this &&
                            (control as AdvancedRadioButton).GroupName == this.GroupName)
                            (control as AdvancedRadioButton).Checked = false;
            }
        }


        private IEnumerable<Control> GetAll(Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => GetAll(ctrl, type))
                                      .Concat(controls)
                                      .Where(c => c.GetType() == type);
        }
    }
}
