using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace kursach_2_0
{
    public partial class change_pass : Form
    {
        private TextBox old_pass;
        private TextBox new_pass;
        private TextBox confirm_pass;
        private Button ChangeButton;
        private Button backbutton;

        public change_pass()
        {
            InitializeComponent();
        }

        private static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                    builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }

        private void InitializeComponent()
        {
            this.old_pass = new TextBox();
            this.new_pass = new TextBox();
            this.confirm_pass = new TextBox();
            this.backbutton = new Button();
            this.ChangeButton = new Button();
            this.SuspendLayout();

            // old_pass
            this.old_pass.Location = new Point(190, 90);
            this.old_pass.Size = new Size(200, 22);
            this.old_pass.Text = "Текущий пароль";
            this.old_pass.ForeColor = SystemColors.ScrollBar;
            this.old_pass.UseSystemPasswordChar = false;
            this.old_pass.Enter += (_, __) => PlaceholderEnter(old_pass, "Текущий пароль");
            this.old_pass.Leave += (_, __) => PlaceholderLeave(old_pass, "Текущий пароль");

            // new_pass
            this.new_pass.Location = new Point(190, 130);
            this.new_pass.Size = new Size(200, 22);
            this.new_pass.Text = "Новый пароль";
            this.new_pass.ForeColor = SystemColors.ScrollBar;
            this.new_pass.UseSystemPasswordChar = false;
            this.new_pass.Enter += (_, __) => PlaceholderEnter(new_pass, "Новый пароль");
            this.new_pass.Leave += (_, __) => PlaceholderLeave(new_pass, "Новый пароль");

            // confirm_pass
            this.confirm_pass.Location = new Point(190, 170);
            this.confirm_pass.Size = new Size(200, 22);
            this.confirm_pass.Text = "Повторите новый пароль";
            this.confirm_pass.ForeColor = SystemColors.ScrollBar;
            this.confirm_pass.UseSystemPasswordChar = false;
            this.confirm_pass.Enter += (_, __) => PlaceholderEnter(confirm_pass, "Повторите новый пароль");
            this.confirm_pass.Leave += (_, __) => PlaceholderLeave(confirm_pass, "Повторите новый пароль");

            // backbutton
            this.backbutton.Location = new Point(12, 314);
            this.backbutton.Size = new Size(75, 23);
            this.backbutton.Text = "Назад";
            this.backbutton.Click += backbutton_Click;

            // ChangeButton
            this.ChangeButton.Location = new Point(465, 314);
            this.ChangeButton.Size = new Size(92, 23);
            this.ChangeButton.Text = "Изменить";
            this.ChangeButton.Click += ChangeButton_Click;

            // form
            this.BackColor = SystemColors.ControlDark;
            this.ClientSize = new Size(579, 365);
            this.Controls.Add(this.old_pass);
            this.Controls.Add(this.new_pass);
            this.Controls.Add(this.confirm_pass);
            this.Controls.Add(this.ChangeButton);
            this.Controls.Add(this.backbutton);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Смена пароля";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private static void PlaceholderEnter(TextBox tb, string placeholder)
        {
            if (tb.Text == placeholder)
            {
                tb.Text = "";
                tb.ForeColor = Color.Black;
                tb.UseSystemPasswordChar = true;
            }
        }

        private static void PlaceholderLeave(TextBox tb, string placeholder)
        {
            if (string.IsNullOrWhiteSpace(tb.Text))
            {
                tb.UseSystemPasswordChar = false;
                tb.Text = placeholder;
                tb.ForeColor = Color.Gray;
            }
        }

        private void ChangeButton_Click(object sender, EventArgs e)
        {
            if (Session.UserId <= 0)
            {
                MessageBox.Show("Не определён текущий пользователь. Перезайдите в систему.");
                return;
            }

            if (old_pass.Text == "Текущий пароль" || string.IsNullOrWhiteSpace(old_pass.Text) ||
                new_pass.Text == "Новый пароль" || string.IsNullOrWhiteSpace(new_pass.Text) ||
                confirm_pass.Text == "Повторите новый пароль" || string.IsNullOrWhiteSpace(confirm_pass.Text))
            {
                MessageBox.Show("Заполните все поля.");
                return;
            }

            if (new_pass.Text != confirm_pass.Text)
            {
                MessageBox.Show("Новый пароль и его подтверждение не совпадают.");
                return;
            }

            if (new_pass.Text.Length < 6)
            {
                MessageBox.Show("Пароль должен быть не короче 6 символов.");
                return;
            }

            string oldHash = HashPassword(old_pass.Text);
            string newHash = HashPassword(new_pass.Text);

            DB db = new DB();
            try
            {
                db.openConnection();

                // Проверяем текущий пароль
                using (var checkCmd = new MySqlCommand(
                           "SELECT 1 FROM user_registration WHERE id=@id AND password_hash=@ph", db.getConnection()))
                {
                    checkCmd.Parameters.AddWithValue("@id", Session.UserId);
                    checkCmd.Parameters.AddWithValue("@ph", oldHash);

                    var ok = checkCmd.ExecuteScalar();
                    if (ok == null)
                    {
                        MessageBox.Show("Текущий пароль введён неверно.");
                        return;
                    }
                }

                // Обновляем пароль
                using (var updCmd = new MySqlCommand(
                           "UPDATE user_registration SET password_hash=@ph WHERE id=@id", db.getConnection()))
                {
                    updCmd.Parameters.AddWithValue("@ph", newHash);
                    updCmd.Parameters.AddWithValue("@id", Session.UserId);

                    int res = updCmd.ExecuteNonQuery();
                    if (res == 1)
                    {
                        MessageBox.Show("Пароль успешно изменён.");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Не удалось изменить пароль.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
            finally
            {
                db.closeConnection();
            }
        }

        private void backbutton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
