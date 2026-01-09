using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kursach_2_0
{
    public partial class MiscControl : UserControl
    {
        public MiscControl()
        {
            InitializeComponent();
            Label label = new Label
            {
                Text = "Разное",
                Dock = DockStyle.Top,
                AutoSize = true
            };
            this.Controls.Add(label);
        }

        private void InitializeComponent()
        {
            throw new NotImplementedException();
        }
    }
}
