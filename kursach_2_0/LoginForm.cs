using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace kursach_2_0
{
    public partial class LoginForm : Form
    {
        public LoginForm()
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
                {
                    builder.Append(b.ToString("x2")); // Преобразование в шестнадцатеричный формат
                }
                return builder.ToString();
            }
        }

        private void AddRegistration_Click(object sender, EventArgs e)
        {
            string login = Login.Text.Trim();
            string password = Password.Text.Trim();

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль.");
                return;
            }

            string hashedPassword = HashPassword(password);
            DB db = new DB();
            try
            {
                db.openConnection();

                string query = @"SELECT id, role, permissions
                                 FROM user_registration
                                 WHERE login = @uL AND password_hash = @uP";

                using (MySqlCommand command = new MySqlCommand(query, db.getConnection()))
                {
                    command.Parameters.AddWithValue("@uL", login);
                    command.Parameters.AddWithValue("@uP", hashedPassword);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int userId = reader.GetInt32("id");
                            string role = reader["role"]?.ToString() ?? "user";
                            string perms = reader["permissions"]?.ToString() ?? "0000";

                            Session.UserId = userId;
                            Session.Login = login;
                            Session.Role = role;
                            Session.Permissions = perms;

                            this.Hide();
                            mainForm mf = new mainForm(login, role);

                            mf.FormClosed += (_, __) =>
                            {
                                this.Show();
                            };

                            mf.Show();
                        }
                        else
                        {
                            MessageBox.Show("Ошибка авторизации. Неверный логин или пароль.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к базе данных: {ex.Message}");
            }
            finally
            {
                db.closeConnection();
            }
        }


        private void RegisterLabel_Click(object sender, EventArgs e)
        {
            this.Hide();
            RegisterForm registerForm = new RegisterForm();
            registerForm.Show();
        }

        private void ExitLabel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
