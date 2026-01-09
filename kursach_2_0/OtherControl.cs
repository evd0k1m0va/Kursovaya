using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kursach_2_0
{
    public partial class OtherControl : UserControl
    {
        private Button ChangePass;
        private Button button2;

        public OtherControl()
        {
            InitializeComponent();
            
        }

        private void InitializeComponent()
        {
            this.ChangePass = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ChangePass
            // 
            this.ChangePass.BackColor = System.Drawing.Color.Chartreuse;
            this.ChangePass.Location = new System.Drawing.Point(109, 177);
            this.ChangePass.Name = "ChangePass";
            this.ChangePass.Size = new System.Drawing.Size(227, 49);
            this.ChangePass.TabIndex = 0;
            this.ChangePass.Text = "Сменить пароль";
            this.ChangePass.UseVisualStyleBackColor = false;
            this.ChangePass.Click += new System.EventHandler(this.ChangePass_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Tomato;
            this.button2.Location = new System.Drawing.Point(109, 253);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(227, 50);
            this.button2.TabIndex = 1;
            this.button2.Text = "Выйти";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // OtherControl
            // 
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Controls.Add(this.button2);
            this.Controls.Add(this.ChangePass);
            this.Name = "OtherControl";
            this.Size = new System.Drawing.Size(763, 444);
            this.ResumeLayout(false);

        }

        private void ChangePass_Click(object sender, EventArgs e)
        {
            // Смена пароля текущего пользователя
            using (change_pass changePassForm = new change_pass())
            {
                changePassForm.StartPosition = FormStartPosition.CenterScreen;
                changePassForm.ShowDialog();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Session.Clear();

            var currentForm = this.FindForm();
            if (currentForm == null)
            {
                Application.Exit();
                return;
            }


            var login = new LoginForm();
            login.StartPosition = FormStartPosition.CenterScreen;
            login.Show();
            currentForm.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
