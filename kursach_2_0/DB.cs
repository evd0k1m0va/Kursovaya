using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.IO;
using System.Windows.Forms;

namespace kursach_2_0
{
    internal class DB
    {
        // Основное подключение к БД (из App.config)
        private readonly MySqlConnection connection;

        public DB()
        {
            string cs = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            connection = new MySqlConnection(cs);
        }

        // Открытие соединения
        public void openConnection()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
                connection.Open();
        }

        // Закрытие соединения
        public void closeConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
                connection.Close();
        }

        // Получить объект соединения
        public MySqlConnection getConnection()
        {
            return connection;
        }

        // Автоматическая установка базы данных из SQL-файла
        public void InstallDatabaseIfNotExists()
        {
            try
            {
                // Берём строку из App.config и делаем "master"
                var csb = new MySqlConnectionStringBuilder(
                    ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString
                );

                // Подключаемся к серверу без указания базы, чтобы можно было создать БД
                csb.Database = string.Empty;

                using (MySqlConnection conn = new MySqlConnection(csb.ConnectionString))
                {
                    conn.Open();

                    // Ищем SQL рядом с exe (bin\Debug/Release)
                    string sqlPath = Path.Combine(Application.StartupPath, "advertising_agency.sql");

                    if (File.Exists(sqlPath))
                    {
                        string sql = File.ReadAllText(sqlPath);
                        MySqlScript script = new MySqlScript(conn, sql);
                        script.Execute();
                    }
                    else
                    {
                        MessageBox.Show(
                            $"Файл advertising_agency.sql не найден.\nОжидался по пути:\n{sqlPath}",
                            "Ошибка",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при установке БД: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
