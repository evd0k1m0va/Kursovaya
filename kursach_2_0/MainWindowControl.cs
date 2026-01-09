using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace kursach_2_0
{
    public partial class MainWindowControl : UserControl
    {
        public MainWindowControl()
        {
            InitializeComponent();
            Label label = new Label
            {
                Text = "Главное окно",
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
