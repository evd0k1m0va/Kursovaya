using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kursach_2_0
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();

            Username.Text = "Имя";
            Username.ForeColor = Color.Gray;
            UserSurname.Text = "Фамилия";
            UserSurname.ForeColor = Color.Gray;
            Login.Text = "Логин";
            Login.ForeColor = Color.Gray;
            Password.Text = "Придумайте пароль";
            Password.ForeColor = Color.Gray;
            PhoneNumber.Text = "Номер телефона";
            PhoneNumber.ForeColor = Color.Gray;
            DateOfBirth.Text = "Дата рождения";
            DateOfBirth.ForeColor = Color.Gray;

        }
        
        static string HashPassword(string Password)
        {
            // Используем SHA256 для хэширования
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(Password));

                // Конвертируем байты в строку
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2")); // Формат в 16-ричном виде
                }

                return builder.ToString();
            }
        }
        private void Username_TextChanged(object sender, EventArgs e)
        {

        }

        private void Login_Enter(object sender, EventArgs e)
        {
            if (Login.Text == "Логин")
            {
                Login.Text = "";
                Login.ForeColor = Color.Black;
            }
        }

        private void Login_Leave(object sender, EventArgs e)
        {
            if (Login.Text == "")
            {
                Login.Text = "Логин";
                Login.ForeColor = Color.Gray;
            }

        }

        private void Password_Enter(object sender, EventArgs e)
        {
            if (Password.Text == "Придумайте пароль")
            {
                Password.Text = "";
                Password.ForeColor = Color.Black;
            }

        }

        private void Password_Leave(object sender, EventArgs e)
        {
            if (Password.Text == "")
            {
                Password.Text = "Придумайте пароль";
                Password.ForeColor = Color.Gray;
            }
        }

        private void PhoneNumber_Enter(object sender, EventArgs e)
        {
            if (PhoneNumber.Text == "Номер телефона")
            {
                PhoneNumber.Text = "";
                PhoneNumber.ForeColor = Color.Black;
            }

        }

        private void PhoneNumber_Leave(object sender, EventArgs e)
        {
            if (PhoneNumber.Text == "")
            {
                PhoneNumber.Text = "Номер телефона";
                PhoneNumber.ForeColor = Color.Gray;
            }

        }

        private void Username_Enter(object sender, EventArgs e)
        {
            if (Username.Text == "Имя")
            {
                Username.Text = "";
                Username.ForeColor = Color.Black;
            }

        }

        private void Username_Leave(object sender, EventArgs e)
        {
            if (Username.Text == "")
            {
                Username.Text = "Имя";
                Username.ForeColor = Color.Gray;
            }
        }

        private void UserSurname_Enter(object sender, EventArgs e)
        {
            if (UserSurname.Text == "Фамилия")
            {
                UserSurname.Text = "";
                UserSurname.ForeColor = Color.Black;
            }
        }

        private void UserSurname_Leave(object sender, EventArgs e)
        {
            if (UserSurname.Text == "")
            {
                UserSurname.Text = "Фамилия";
                UserSurname.ForeColor = Color.Gray;
            }
        }
        private void DateOfBirth_Leave(object sender, EventArgs e)
        {
            if (DateOfBirth.Text == "")
            {
                DateOfBirth.Text = "Дата рождения";
                DateOfBirth.ForeColor = Color.Gray;
            }

        }

        private void DateOfBirth_Enter(object sender, EventArgs e)
        {
            if (DateOfBirth.Text == "Дата рождения")
            {
                DateOfBirth.Text = "";
                DateOfBirth.ForeColor = Color.Black;
            }
        }
        private bool Check() 
        {

            foreach (char num in Username.Text)
            {
                if (char.IsLetter(num) == false)
                {
                    MessageBox.Show("Имя должно состоять из букв");
                    return false;
                }
            }
            foreach (char num in UserSurname.Text)
            {
                if (char.IsLetter(num) == false)
                {
                    MessageBox.Show("Фамилия должна состоять из букв");
                    return false;
                }
            }
            foreach (char num in PhoneNumber.Text)
            {
                if (char.IsDigit(num) == false)
                {
                    MessageBox.Show("Номер должен состоять из цифр");
                    return false;
                }
            }
            if (DateTime.TryParse(DateOfBirth.Text, out DateTime birthDate))
            {
                if (birthDate >= DateTime.Today)
                {
                    MessageBox.Show("Дата рождения не может быть позже сегодняшней даты.");
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Неверный формат даты.");
                return false;
            }


            if (PhoneNumber.Text.Length != 11)
            {
                MessageBox.Show("Введите номер корректно.");
                return false;
            }

            else return true;
        }
        private void ButtonRegistration_Click(object sender, EventArgs e)
        {
            bool check = Check();
            if (check == true)
            {
                string hashedPassword = HashPassword(Password.Text);
                DB db = new DB();
                MySqlCommand command = new MySqlCommand("INSERT INTO `user_registration` (`username`,`surname`,`login`, `password_hash`, `number`, `date_of_birth`) VALUES (@username, @surname, @login, @password_hash, @number, @dob)", db.getConnection());
                
                command.Parameters.Add("@username", MySqlDbType.VarChar).Value = Username.Text;
                command.Parameters.Add("@surname", MySqlDbType.VarChar).Value = UserSurname.Text;
                command.Parameters.Add("@login", MySqlDbType.VarChar).Value = Login.Text;
                command.Parameters.Add("@password_hash", MySqlDbType.VarChar).Value = hashedPassword;
                command.Parameters.Add("@number", MySqlDbType.Int64).Value = PhoneNumber.Text;
                command.Parameters.Add("@dob", MySqlDbType.Date).Value = DateTime.Parse(DateOfBirth.Text);

                
                db.openConnection();

                if (command.ExecuteNonQuery() == 1)
                    MessageBox.Show("Аккаунт успешно создан");
                else
                    MessageBox.Show("Не удалось создать аккаунт");



                db.closeConnection();
                
                ResetFormFields();
                this.Hide();
                LoginForm loginForm = new LoginForm();
                loginForm.Show();
            }
        }
        private void ResetFormFields()
        {
            Username.Text = "Имя";
            Username.ForeColor = Color.Gray;
            UserSurname.Text = "Фамилия";
            UserSurname.ForeColor = Color.Gray;
            Login.Text = "Логин";
            Login.ForeColor = Color.Gray;
            Password.Text = "Придумайте пароль";
            Password.ForeColor = Color.Gray;
            PhoneNumber.Text = "Номер телефона";
            PhoneNumber.ForeColor = Color.Gray;
            DateOfBirth.Text = "Дата рождения";
            DateOfBirth.ForeColor = Color.Gray;
        }

        public Boolean CheckUser()
        {
            DB DB = new DB();

            DataTable table = new DataTable();
            DataTable table1 = new DataTable();
          
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlDataAdapter adapter1 = new MySqlDataAdapter();
           

            MySqlCommand command = new MySqlCommand("SELECT * FROM `user_registration` WHERE `login` = @uL ", DB.getConnection());
            MySqlCommand comman = new MySqlCommand("SELECT * FROM `user_registration` WHERE `number` = @num ", DB.getConnection());
            command.Parameters.Add("@uL", MySqlDbType.VarChar).Value = Login.Text;
            comman.Parameters.Add("@num", MySqlDbType.VarChar).Value = PhoneNumber.Text;
            adapter.SelectCommand = command;
            adapter1.SelectCommand = comman;
            adapter1.Fill(table1);
            adapter.Fill(table);
            if (table.Rows.Count > 0)
            {
                MessageBox.Show("Такой логин уже есть, введите другой");
                return true;
            }
           
             if (table1.Rows.Count > 0)
            {
                MessageBox.Show("Номер уже зарегистрирован.");
                return true;
            }
            else
                return false;

        }

        private void Labelfinish_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm loginForm = new LoginForm(); 
            loginForm.Show();
        }

        private void LabelExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }



    }
}
