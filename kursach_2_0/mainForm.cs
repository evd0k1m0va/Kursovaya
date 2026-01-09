using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace kursach_2_0
{
    public partial class mainForm : Form
    {
        private readonly string _login;
        private readonly string _role;

        public mainForm(string login, string role)
        {
            _login = login;
            _role = role;

            InitializeComponent();
            BuildMenuFromDatabase();
        }

        private void BuildMenuFromDatabase()
        {
            Fullcontrol.TabPages.Clear();

            string roleScope = string.Equals(_role, "admin", StringComparison.OrdinalIgnoreCase)
                ? "admin"
                : "user";

            DB db = new DB();

            try
            {
                db.openConnection();

                string sql = @"
                    SELECT title, control_type
                    FROM app_menu
                    WHERE is_enabled = 1
                      AND (role_scope = 'all' OR role_scope = @role)
                    ORDER BY sort_order, id;";

                using (MySqlCommand cmd = new MySqlCommand(sql, db.getConnection()))
                {
                    cmd.Parameters.AddWithValue("@role", roleScope);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string title = reader.GetString("title");
                            string controlTypeName = reader.GetString("control_type");

                            var page = new TabPage(title)
                            {
                                BackColor = System.Drawing.Color.DarkGray
                            };

                            Control ui = CreateControlByTypeName(controlTypeName);
                            if (ui != null)
                            {
                                ui.Dock = DockStyle.Fill;
                                page.Controls.Add(ui);
                            }
                            else
                            {
                                page.Controls.Add(new Label
                                {
                                    Dock = DockStyle.Fill,
                                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                                    AutoSize = false,
                                    Text = $"Не удалось загрузить модуль: {controlTypeName}"
                                });
                            }

                            Fullcontrol.TabPages.Add(page);
                        }
                    }
                }

                if (Fullcontrol.TabPages.Count == 0)
                {
                    Fullcontrol.TabPages.Add(new TabPage("Меню")
                    {
                        Controls =
                        {
                            new Label
                            {
                                Dock = DockStyle.Fill,
                                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                                AutoSize = false,
                                Text = "Пункты меню не найдены в БД (таблица app_menu)."
                            }
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Fullcontrol.TabPages.Clear();
                Fullcontrol.TabPages.Add(new TabPage("Ошибка")
                {
                    Controls =
                    {
                        new Label
                        {
                            Dock = DockStyle.Fill,
                            TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                            AutoSize = false,
                            Text = "Ошибка при формировании меню из БД:\n" + ex.Message
                        }
                    }
                });
            }
            finally
            {
                db.closeConnection();
            }
        }

        private Control CreateControlByTypeName(string controlTypeName)
        {
            if (string.IsNullOrWhiteSpace(controlTypeName))
                return null;

            var asm = typeof(mainForm).Assembly;
            Type t = asm.GetType(controlTypeName, throwOnError: false, ignoreCase: false);

            if (t == null)
                return null;

            try
            {
                if (t == typeof(DirectoriesControl))
                    return (Control)Activator.CreateInstance(t, _login, _role);

                return (Control)Activator.CreateInstance(t);
            }
            catch
            {
                return null;
            }
        }
    }
}
