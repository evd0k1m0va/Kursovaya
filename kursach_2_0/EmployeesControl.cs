using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace kursach_2_0
{
    public partial class EmployeesControl : UserControl
    {
        private DataGridView dataGridView1;
        private CheckBox readCheckBox;
        private CheckBox addCheckBox;
        private CheckBox editCheckBox;
        private CheckBox deleteCheckBox;
        private Button saveButton;

        private DataTable _usersTable;

        public EmployeesControl()
        {
            InitializeComponent();
            HookEvents();
            LoadData();
        }

        private void HookEvents()
        {
            dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;
            dataGridView1.DataError += (s, e) =>
            {
                MessageBox.Show("Ошибка в данных: " + e.Exception.Message, "DataGrid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.ThrowException = false;
            };
        }

        /// <summary>
        /// Проверяет наличие колонки в таблице
        /// </summary>
        private bool ColumnExists(MySqlConnection conn, string tableName, string columnName)
        {
            using (var cmd = new MySqlCommand(@"
                SELECT COUNT(*)
                FROM INFORMATION_SCHEMA.COLUMNS
                WHERE TABLE_SCHEMA = DATABASE()
                  AND TABLE_NAME = @t
                  AND COLUMN_NAME = @c;", conn))
            {
                cmd.Parameters.AddWithValue("@t", tableName);
                cmd.Parameters.AddWithValue("@c", columnName);
                var res = cmd.ExecuteScalar();
                return Convert.ToInt32(res) > 0;
            }
        }

        private void LoadData()
        {
            try
            {
                DB db = new DB();
                db.openConnection();

                bool hasSurname = ColumnExists(db.getConnection(), "user_registration", "surname");
                bool hasUsername = ColumnExists(db.getConnection(), "user_registration", "username");
                bool hasNumber = ColumnExists(db.getConnection(), "user_registration", "number");

                string select = "SELECT id AS `ID`, login AS `Логин`";

                if (hasSurname) select += ", surname AS `Фамилия`";
                if (hasUsername) select += ", username AS `Имя`";

                select += ", role AS `Роль`, permissions AS `Права доступа`";

                if (hasNumber) select += ", number AS `Телефон`";

                select += ", created_at AS `Дата регистрации` FROM user_registration ORDER BY id;";

                using (var da = new MySqlDataAdapter(select, db.getConnection()))
                {
                    _usersTable = new DataTable();
                    da.Fill(_usersTable);
                    dataGridView1.DataSource = _usersTable;
                }

                db.closeConnection();

                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.MultiSelect = false;

                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[0].Selected = true;
                    SyncCheckboxesFromSelectedRow();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных (Сотрудники): {ex.Message}", "EmployeesControl", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            SyncCheckboxesFromSelectedRow();
        }

        private void SyncCheckboxesFromSelectedRow()
        {
            try
            {
                if (dataGridView1.SelectedRows.Count != 1)
                    return;

                // Проверяем, что колонка существует
                if (!dataGridView1.Columns.Contains("Права доступа"))
                    return;

                var row = dataGridView1.SelectedRows[0];


                object cellValue = row.Cells["Права доступа"].Value;
                string p = Convert.ToString(cellValue) ?? "0000";

                // Оставляем только 0/1
                p = new string(p.Where(ch => ch == '0' || ch == '1').ToArray());

                // Делаем строго 4 бита
                if (p.Length < 4) p = p.PadRight(4, '0');
                if (p.Length > 4) p = p.Substring(0, 4);

                readCheckBox.Checked = p[0] == '1';
                addCheckBox.Checked = p[1] == '1';
                editCheckBox.Checked = p[2] == '1';
                deleteCheckBox.Checked = p[3] == '1';
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка синхронизации прав: " + ex.Message);
            }
        }


        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.readCheckBox = new System.Windows.Forms.CheckBox();
            this.addCheckBox = new System.Windows.Forms.CheckBox();
            this.editCheckBox = new System.Windows.Forms.CheckBox();
            this.deleteCheckBox = new System.Windows.Forms.CheckBox();
            this.saveButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();

            // dataGridView1
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.InactiveBorder;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(50, 20);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.Size = new System.Drawing.Size(1050, 400);
            this.dataGridView1.TabIndex = 0;

            // readCheckBox
            this.readCheckBox.Location = new System.Drawing.Point(50, 450);
            this.readCheckBox.Name = "readCheckBox";
            this.readCheckBox.Size = new System.Drawing.Size(104, 24);
            this.readCheckBox.TabIndex = 1;
            this.readCheckBox.Text = "Чтение";

            // addCheckBox
            this.addCheckBox.Location = new System.Drawing.Point(170, 450);
            this.addCheckBox.Name = "addCheckBox";
            this.addCheckBox.Size = new System.Drawing.Size(130, 24);
            this.addCheckBox.TabIndex = 2;
            this.addCheckBox.Text = "Добавление";

            // editCheckBox
            this.editCheckBox.Location = new System.Drawing.Point(320, 450);
            this.editCheckBox.Name = "editCheckBox";
            this.editCheckBox.Size = new System.Drawing.Size(120, 24);
            this.editCheckBox.TabIndex = 3;
            this.editCheckBox.Text = "Изменение";

            // deleteCheckBox
            this.deleteCheckBox.Location = new System.Drawing.Point(460, 450);
            this.deleteCheckBox.Name = "deleteCheckBox";
            this.deleteCheckBox.Size = new System.Drawing.Size(110, 24);
            this.deleteCheckBox.TabIndex = 4;
            this.deleteCheckBox.Text = "Удаление";

            // saveButton
            this.saveButton.Location = new System.Drawing.Point(600, 446);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(200, 32);
            this.saveButton.TabIndex = 5;
            this.saveButton.Text = "Сохранить права";
            this.saveButton.Click += new System.EventHandler(this.SavePermissions_Click);

            // EmployeesControl
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.readCheckBox);
            this.Controls.Add(this.addCheckBox);
            this.Controls.Add(this.editCheckBox);
            this.Controls.Add(this.deleteCheckBox);
            this.Controls.Add(this.saveButton);
            this.Name = "EmployeesControl";
            this.Size = new System.Drawing.Size(1174, 524);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
        }

        private void SavePermissions_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 1)
            {
                MessageBox.Show("Выберите пользователя.", "Права", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string selectedLogin = Convert.ToString(dataGridView1.SelectedRows[0].Cells["Логин"].Value);
            if (string.IsNullOrWhiteSpace(selectedLogin))
            {
                MessageBox.Show("Не удалось определить логин пользователя.", "Права", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string permissions =
                (readCheckBox.Checked ? "1" : "0") +
                (addCheckBox.Checked ? "1" : "0") +
                (editCheckBox.Checked ? "1" : "0") +
                (deleteCheckBox.Checked ? "1" : "0");

            try
            {
                DB db = new DB();
                db.openConnection();

                using (var command = new MySqlCommand(
                    "UPDATE user_registration SET permissions = @permissions WHERE login = @login",
                    db.getConnection()))
                {
                    command.Parameters.AddWithValue("@permissions", permissions);
                    command.Parameters.AddWithValue("@login", selectedLogin);
                    command.ExecuteNonQuery();
                }

                db.closeConnection();


                dataGridView1.SelectedRows[0].Cells["Права доступа"].Value = permissions;

                MessageBox.Show("Права доступа обновлены.", "Права", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения прав: " + ex.Message, "Права", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
