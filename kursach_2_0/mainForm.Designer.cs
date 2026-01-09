using System.Data;
using System.Windows.Forms;

namespace kursach_2_0
{
    public partial class mainForm : Form
    {
        private void InitializeComponent()
        {
            this.documentsControl = new kursach_2_0.DocumentsControl();
            this.Rental = new System.Windows.Forms.TabPage();
            this.Reference = new System.Windows.Forms.TabPage();
            this.ReferenceControl = new System.Windows.Forms.TabControl();
            this.Content = new System.Windows.Forms.TabPage();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.Others_button = new System.Windows.Forms.Button();
            this.Orders_button = new System.Windows.Forms.Button();
            this.AboutProgram = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.DirectoriesPage = new System.Windows.Forms.TabPage();
            this.OtherPage = new System.Windows.Forms.TabPage();
            this.otherControl = new kursach_2_0.OtherControl();
            this.tabPageOrders = new System.Windows.Forms.TabPage();
            this.ordersControl = new kursach_2_0.OrdersControl();
            this.Fullcontrol = new System.Windows.Forms.TabControl();
            this.Rental.SuspendLayout();
            this.Reference.SuspendLayout();
            this.ReferenceControl.SuspendLayout();
            this.Content.SuspendLayout();
            this.AboutProgram.SuspendLayout();
            this.OtherPage.SuspendLayout();
            this.tabPageOrders.SuspendLayout();
            this.Fullcontrol.SuspendLayout();
            this.SuspendLayout();
            // 
            // documentsControl
            // 
            this.documentsControl.BackColor = System.Drawing.SystemColors.ControlDark;
            this.documentsControl.Location = new System.Drawing.Point(-4, 0);
            this.documentsControl.Name = "documentsControl";
            this.documentsControl.Size = new System.Drawing.Size(1178, 524);
            this.documentsControl.TabIndex = 0;
            // 
            // Rental
            // 
            this.Rental.BackColor = System.Drawing.Color.DarkGray;
            this.Rental.Controls.Add(this.documentsControl);
            this.Rental.Location = new System.Drawing.Point(4, 25);
            this.Rental.Name = "Rental";
            this.Rental.Padding = new System.Windows.Forms.Padding(3);
            this.Rental.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Rental.Size = new System.Drawing.Size(1174, 524);
            this.Rental.TabIndex = 5;
            this.Rental.Text = "Документы";
            
            // 
            // Reference
            // 
            this.Reference.BackColor = System.Drawing.Color.Transparent;
            this.Reference.Controls.Add(this.ReferenceControl);
            this.Reference.Location = new System.Drawing.Point(4, 25);
            this.Reference.Name = "Reference";
            this.Reference.Padding = new System.Windows.Forms.Padding(3);
            this.Reference.Size = new System.Drawing.Size(1174, 524);
            this.Reference.TabIndex = 4;
            this.Reference.Text = "Справка";
            // 
            // ReferenceControl
            // 
            this.ReferenceControl.Controls.Add(this.Content);
            this.ReferenceControl.Controls.Add(this.AboutProgram);
            this.ReferenceControl.Location = new System.Drawing.Point(0, 0);
            this.ReferenceControl.Name = "ReferenceControl";
            this.ReferenceControl.SelectedIndex = 0;
            this.ReferenceControl.Size = new System.Drawing.Size(1178, 524);
            this.ReferenceControl.TabIndex = 0;
            // 
            // Content
            // 
            this.Content.BackColor = System.Drawing.Color.DarkGray;
            this.Content.Controls.Add(this.button4);
            this.Content.Controls.Add(this.button3);
            this.Content.Controls.Add(this.Others_button);
            this.Content.Controls.Add(this.Orders_button);
            this.Content.Location = new System.Drawing.Point(4, 25);
            this.Content.Name = "Content";
            this.Content.Padding = new System.Windows.Forms.Padding(3);
            this.Content.Size = new System.Drawing.Size(1170, 495);
            this.Content.TabIndex = 0;
            this.Content.Text = "Содержание";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(77, 169);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(155, 35);
            this.button4.TabIndex = 3;
            this.button4.Text = "Документы";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(77, 128);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(155, 35);
            this.button3.TabIndex = 2;
            this.button3.Text = "Справочники";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // Others_button
            // 
            this.Others_button.Location = new System.Drawing.Point(77, 87);
            this.Others_button.Name = "Others_button";
            this.Others_button.Size = new System.Drawing.Size(155, 35);
            this.Others_button.TabIndex = 1;
            this.Others_button.Text = "Разное";
            this.Others_button.UseVisualStyleBackColor = true;
            // 
            // Orders_button
            // 
            this.Orders_button.Location = new System.Drawing.Point(77, 46);
            this.Orders_button.Name = "Orders_button";
            this.Orders_button.Size = new System.Drawing.Size(155, 35);
            this.Orders_button.TabIndex = 0;
            this.Orders_button.Text = "Заказы";
            this.Orders_button.UseVisualStyleBackColor = true;
            // 
            // AboutProgram
            // 
            this.AboutProgram.BackColor = System.Drawing.Color.DarkGray;
            this.AboutProgram.Controls.Add(this.label1);
            this.AboutProgram.Location = new System.Drawing.Point(4, 25);
            this.AboutProgram.Name = "AboutProgram";
            this.AboutProgram.Padding = new System.Windows.Forms.Padding(3);
            this.AboutProgram.Size = new System.Drawing.Size(1170, 495);
            this.AboutProgram.TabIndex = 1;
            this.AboutProgram.Text = "О программе";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(617, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Программа выполнена студенткой 3-го курса АВТФ группы АП 227 Евдокимовой Анастаси" +
    "ей";
            // 
            // DirectoriesPage
            // 
            //this.DirectoriesPage.Controls.Add(this.directoriesControl);
            this.DirectoriesPage.Location = new System.Drawing.Point(4, 25);
            this.DirectoriesPage.Name = "DirectoriesPage";
            this.DirectoriesPage.Size = new System.Drawing.Size(1174, 524);
            this.DirectoriesPage.TabIndex = 3;
            this.DirectoriesPage.Text = "Справочники";
            // 
            // OtherPage
            // 
            this.OtherPage.Controls.Add(this.otherControl);
            this.OtherPage.Location = new System.Drawing.Point(4, 25);
            this.OtherPage.Name = "OtherPage";
            this.OtherPage.Size = new System.Drawing.Size(1174, 524);
            this.OtherPage.TabIndex = 1;
            this.OtherPage.Text = "Разное";
            // 
            // otherControl
            // 
            this.otherControl.BackColor = System.Drawing.SystemColors.ControlDark;
            this.otherControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.otherControl.Location = new System.Drawing.Point(0, 0);
            this.otherControl.Name = "otherControl";
            this.otherControl.Size = new System.Drawing.Size(1174, 524);
            this.otherControl.TabIndex = 0;
            // 
            // tabPageOrders
            // 
            this.tabPageOrders.Controls.Add(this.ordersControl);
            this.tabPageOrders.Location = new System.Drawing.Point(4, 25);
            this.tabPageOrders.Name = "tabPageOrders";
            this.tabPageOrders.Size = new System.Drawing.Size(1174, 524);
            this.tabPageOrders.TabIndex = 0;
            this.tabPageOrders.Text = "Заказы";
            // 
            // ordersControl
            // 
            this.ordersControl.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ordersControl.Cursor = System.Windows.Forms.Cursors.Default;
            this.ordersControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ordersControl.Location = new System.Drawing.Point(0, 0);
            this.ordersControl.Name = "ordersControl";
            this.ordersControl.Size = new System.Drawing.Size(1174, 524);
            this.ordersControl.TabIndex = 0;
            // 
            // Fullcontrol
            // 
            this.Fullcontrol.Controls.Add(this.tabPageOrders);
            this.Fullcontrol.Controls.Add(this.OtherPage);
            this.Fullcontrol.Controls.Add(this.DirectoriesPage);
            this.Fullcontrol.Controls.Add(this.Reference);
            this.Fullcontrol.Controls.Add(this.Rental);
            this.Fullcontrol.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Fullcontrol.Location = new System.Drawing.Point(0, 0);
            this.Fullcontrol.Name = "Fullcontrol";
            this.Fullcontrol.SelectedIndex = 0;
            this.Fullcontrol.Size = new System.Drawing.Size(1182, 553);
            this.Fullcontrol.TabIndex = 0;
            // 
            // mainForm
            // 
            this.ClientSize = new System.Drawing.Size(1182, 553);
            this.Controls.Add(this.Fullcontrol);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "mainForm";
            this.Text = "Рекламное агентство";
            this.Rental.ResumeLayout(false);
            this.Reference.ResumeLayout(false);
            this.ReferenceControl.ResumeLayout(false);
            this.Content.ResumeLayout(false);
            this.AboutProgram.ResumeLayout(false);
            this.AboutProgram.PerformLayout();
            this.OtherPage.ResumeLayout(false);
            this.tabPageOrders.ResumeLayout(false);
            this.Fullcontrol.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private DocumentsControl documentsControl;

        private TabPage Rental;
        private TabPage Reference;
        private TabControl ReferenceControl;
        private TabPage Content;
        private Button button4;
        private Button button3;
        private Button Others_button;
        private Button Orders_button;
        private TabPage AboutProgram;
        private Label label1;
        private TabPage DirectoriesPage;
        private TabPage OtherPage;
        private OtherControl otherControl;
        private TabPage tabPageOrders;
        private OrdersControl ordersControl;
        private TabControl Fullcontrol;
        


    }
}
