using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FC_Diseño_de_Nervios.Controles
{
    public class TextBoxCustom:TextBox
    {
        public bool TextChangedProperty { get; set; } = true;

        protected override void OnTextChanged(EventArgs e)
        {
            if (TextChangedProperty)
                base.OnTextChanged(e);
        }
    }
}
