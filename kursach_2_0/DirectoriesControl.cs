using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace kursach_2_0
{
    public class TableItem
    {
        public string TableName { get; set; }
        public string DisplayName { get; set; }

        public TableItem(string tableName, string displayName)
        {
            TableName = tableName;
            DisplayName = displayName;
        }

        public override string ToString() => DisplayName;
    }

    public partial class DirectoriesControl : UserControl
    {
        private readonly string _role;
        private readonly string _login;

        private ComboBox comboBoxTableList;
        private DataGridView dataGridView1;
        private TextBox txtSearch;
        private Button btnSearch;
        private Button btnReset;
        private Button btnSaveChanges;
        private Button btnDelete;

        private Label lblAccessInfo;

        private MySqlDataAdapter adapter;
        private DataTable currentTable;

        private List<TableItem> tableMapping;

        public DirectoriesControl(string login, string role)
        {
            _role = role ?? "";
            _login = login ?? "";

            InitializeComponent();
            InitializeTableMapping();
            LoadTableList();
            CheckPermissions(); // после загрузки UI
        }

        private void InitializeComponent()
        {
            this.comboBoxTableList = new ComboBox();
            this.dataGridView1 = new DataGridView();
            this.txtSearch = new TextBox();
            this.btnSearch = new Button();
            this.btnReset = new Button();
            this.btnSaveChanges = new Button();
            this.btnDelete = new Button();
            this.lblAccessInfo = new Label();

            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();

            // comboBoxTableList
            this.comboBoxTableList.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxTableList.Location = new Point(20, 20);
            this.comboBoxTableList.Size = new Size(250, 24);
            this.comboBoxTableList.SelectedIndexChanged += new EventHandler(this.ComboBoxTableList_SelectedIndexChanged);

            // search box
            this.txtSearch.Location = new Point(620, 22);
            this.txtSearch.Size = new Size(260, 22);
            
            this.txtSearch.Text = "Поиск...";
            this.txtSearch.ForeColor = Color.Gray;
            this.txtSearch.Enter += (s, e) =>
            {
                if (txtSearch.Text == "Поиск...") { txtSearch.Text = ""; txtSearch.ForeColor = Color.Black; }
            };
            this.txtSearch.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtSearch.Text)) { txtSearch.Text = "Поиск..."; txtSearch.ForeColor = Color.Gray; }
            };

            this.btnSearch.Location = new Point(890, 18);
            this.btnSearch.Size = new Size(110, 30);
            this.btnSearch.Text = "Найти";
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);

            this.btnReset.Location = new Point(1010, 18);
            this.btnReset.Size = new Size(110, 30);
            this.btnReset.Text = "Сброс";
            this.btnReset.Click += new EventHandler(this.btnReset_Click);

            // buttons
            this.btnSaveChanges.Location = new Point(290, 18);
            this.btnSaveChanges.Size = new Size(150, 30);
            this.btnSaveChanges.Text = "Сохранить";
            this.btnSaveChanges.Click += new EventHandler(this.btnSaveChanges_Click);

            this.btnDelete.Location = new Point(450, 18);
            this.btnDelete.Size = new Size(150, 30);
            this.btnDelete.Text = "Удалить";
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);

            // dataGridView
            this.dataGridView1.Location = new Point(20, 72);
            this.dataGridView1.Size = new Size(1100, 420);
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView1.DataError += DataGridView1_DataError;

            // lblAccessInfo
            this.lblAccessInfo.Location = new Point(20, 500);
            this.lblAccessInfo.Size = new Size(1100, 24);
            this.lblAccessInfo.ForeColor = Color.DarkRed;
            this.lblAccessInfo.Text = "";
            this.lblAccessInfo.Visible = false;

            // Control
            this.Controls.Add(this.comboBoxTableList);
            this.Controls.Add(this.btnSaveChanges);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.lblAccessInfo);

            this.Size = new Size(1150, 540);

            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
        }

        private void DataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show($"Ошибка в данных: {e.Exception.Message}", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            e.ThrowException = false;
        }

        /// <summary>
        /// Проверка прав из user_registration.permissions
        /// 0000 - нет доступа
        /// 1000 - только чтение
        /// 1100 - чтение + добавление (без удаления)
        /// 1110 - чтение + добавление + изменение (без удаления)
        /// 1111 - полный доступ
        /// </summary>
        private void CheckPermissions()
        {
            string permissions = "0000";

            try
            {
                DB db = new DB();
                db.openConnection();

                using (var command = new MySqlCommand("SELECT permissions FROM user_registration WHERE login = @login", db.getConnection()))
                {
                    command.Parameters.AddWithValue("@login", _login);
                    permissions = command.ExecuteScalar()?.ToString() ?? "0000";
                }

                db.closeConnection();
            }
            catch (Exception ex)
            {
                // Если не смогли получить права — считаем, что доступ запрещён (и показываем причину)
                permissions = "0000";
                ShowAccessInfo("Не удалось получить права доступа: " + ex.Message);
            }

            ApplyPermissions(permissions);
        }

        private void ApplyPermissions(string permissions)
        {
            // По умолчанию всё выключено
            btnSaveChanges.Enabled = false;
            btnDelete.Enabled = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;

            if (permissions == "0000")
            {
                ShowAccessInfo("Доступ к справочникам запрещён (permissions = 0000).");
                comboBoxTableList.Enabled = false;
                dataGridView1.Enabled = false;
                return;
            }

            HideAccessInfo();
            comboBoxTableList.Enabled = true;
            dataGridView1.Enabled = true;

            if (permissions == "1000") // Только чтение
            {
                dataGridView1.ReadOnly = true;
            }
            else if (permissions == "1100") // Чтение + Добавление
            {
                btnSaveChanges.Enabled = true;
                dataGridView1.ReadOnly = false;
                dataGridView1.AllowUserToAddRows = true;
                dataGridView1.AllowUserToDeleteRows = false;
            }
            else if (permissions == "1110") // Чтение + Добавление + Изменение
            {
                btnSaveChanges.Enabled = true;
                dataGridView1.ReadOnly = false;
                dataGridView1.AllowUserToAddRows = true;
                dataGridView1.AllowUserToDeleteRows = false;
            }
            else if (permissions == "1111") // Полный доступ
            {
                btnSaveChanges.Enabled = true;
                btnDelete.Enabled = true;
                dataGridView1.ReadOnly = false;
                dataGridView1.AllowUserToAddRows = true;
                dataGridView1.AllowUserToDeleteRows = true;
            }
            else
            {
                ShowAccessInfo($"Неизвестный формат прав: {permissions}. Доступ ограничен.");
            }
        }

        private void ShowAccessInfo(string text)
        {
            lblAccessInfo.Text = text;
            lblAccessInfo.Visible = true;
        }

        private void HideAccessInfo()
        {
            lblAccessInfo.Visible = false;
            lblAccessInfo.Text = "";
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    if (!row.IsNewRow)
                        dataGridView1.Rows.Remove(row);
                }
            }
            else
            {
                MessageBox.Show("Выберите строку для удаления!");
            }
        }

        private void LoadTableData(string tableName)
        {
            try
            {
                string safeTableName = $"`{tableName.Replace("`", "")}`";
                string query = $"SELECT * FROM {safeTableName}";

                DB db = new DB();

                db.openConnection();
                var cmd = new MySqlCommand(query, db.getConnection());

                adapter = new MySqlDataAdapter(cmd);
                MySqlCommandBuilder builder = new MySqlCommandBuilder(adapter);

                currentTable = new DataTable();
                adapter.Fill(currentTable);

                dataGridView1.DataSource = currentTable;

                // Требование: при просмотре таблиц заменять внешние ключи на понятные названия
                ConfigureForeignKeyColumns(tableName);

                // сброс поиска при переключении таблиц
                if (txtSearch != null)
                {
                    txtSearch.Text = "Поиск...";
                    txtSearch.ForeColor = Color.Gray;
                }

                db.closeConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке таблицы '{tableName}': {ex.Message}");
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (currentTable == null) return;

            string text = (txtSearch?.Text ?? "").Trim();
            if (string.IsNullOrEmpty(text) || text == "Поиск...")
            {
                btnReset_Click(sender, e);
                return;
            }

            try
            {
                var dv = currentTable.DefaultView;
                var parts = new List<string>();

                foreach (DataColumn col in currentTable.Columns)
                {
                    string colName = col.ColumnName.Replace("]", "]]" );
                    parts.Add($"CONVERT([{colName}], 'System.String') LIKE '%{text.Replace("'", "''")}%" );
                }

                dv.RowFilter = string.Join(" OR ", parts);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка поиска: " + ex.Message);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (currentTable == null) return;
            currentTable.DefaultView.RowFilter = string.Empty;
        }

        private void ConfigureForeignKeyColumns(string tableName)
        {
            for (int i = dataGridView1.Columns.Count - 1; i >= 0; i--)
            {
                if (dataGridView1.Columns[i] is DataGridViewComboBoxColumn)
                    dataGridView1.Columns.RemoveAt(i);
            }

            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = currentTable;
            dataGridView1.Refresh();

            try
            {
                switch (tableName)
                {
                    case "employee":
                        ReplaceWithLookup("post_id", "post", "title");
                        break;
                    case "billboards":
                        ReplaceWithLookup("street_id", "street", "street");
                        break;
                    case "work_record":
                        ReplaceWithLookup("employee_id", "employee", "surname");
                        ReplaceWithLookup("post_id", "post", "title");
                        break;
                    case "contract":
                        ReplaceWithLookup("renter_id", "rental", "name");
                        ReplaceWithLookup("employee_id", "employee", "surname");
                        ReplaceWithLookup("typepay_id", "typepay", "type");
                        ReplaceWithLookup("optional_id", "optional", "service");
                        break;
                    case "orders":
                        ReplaceWithLookup("contract_id", "contract", "id");
                        ReplaceWithLookup("billboard_id", "billboards", "address");
                        ReplaceWithLookup("street_id", "street", "street");
                        break;
                    case "pricelist":
                        ReplaceWithLookup("billboard_id", "billboards", "address");
                        ReplaceWithLookup("optional_id", "optional", "service");
                        break;
                }
            }
            catch
            {
                // если lookup не удалось загрузить — просто оставляем id
            }
        }

        private void ReplaceWithLookup(string fkColumnName, string lookupTable, string displayField)
        {
            if (currentTable == null) return;
            if (!currentTable.Columns.Contains(fkColumnName)) return;
            if (!dataGridView1.Columns.Contains(fkColumnName)) return;

            // Загружаем справочник
            DataTable lookup = new DataTable();
            DB db = new DB();
            db.openConnection();
            using (var cmd = new MySqlCommand($"SELECT id, `{displayField}` AS display_value FROM `{lookupTable}` ORDER BY id", db.getConnection()))
            using (var ad = new MySqlDataAdapter(cmd))
            {
                ad.Fill(lookup);
            }
            db.closeConnection();

            int colIndex = dataGridView1.Columns[fkColumnName].Index;
            dataGridView1.Columns.Remove(fkColumnName);

            var cb = new DataGridViewComboBoxColumn
            {
                Name = fkColumnName,
                DataPropertyName = fkColumnName,
                DataSource = lookup,
                ValueMember = "id",
                DisplayMember = "display_value",
                FlatStyle = FlatStyle.Flat,
                DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton,
                HeaderText = fkColumnName
            };

            dataGridView1.Columns.Insert(colIndex, cb);
        }

        private void InitializeTableMapping()
        {
            tableMapping = new List<TableItem>
            {
                new TableItem("street", "Улицы"),
                new TableItem("area", "Районы"),
                new TableItem("billboards", "Рекламные щиты"),
                new TableItem("contract", "Договоры"),
                new TableItem("employee", "Сотрудники"),
                new TableItem("optional", "Доп. опции"),
                new TableItem("orders", "Заказы"),
                new TableItem("post", "Должности"),
                new TableItem("pricelist", "Прайс-лист"),
                new TableItem("rental", "Арендаторы"),
                new TableItem("typepay", "Тип оплаты"),
                new TableItem("user_registration", "Пользователи"),
                new TableItem("work_record", "Трудовая книжка")
            };
        }

        private void LoadTableList()
        {

            if (!string.Equals(_role, "admin", StringComparison.OrdinalIgnoreCase))
            {
                tableMapping.RemoveAll(t => t.TableName == "user_registration" || t.TableName == "employee");
            }

            comboBoxTableList.DataSource = tableMapping;
            comboBoxTableList.DisplayMember = "DisplayName";
            comboBoxTableList.ValueMember = "TableName";

            if (comboBoxTableList.Items.Count > 0)
                comboBoxTableList.SelectedIndex = 0;
        }

        private void ComboBoxTableList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxTableList.SelectedValue is string tableName && !string.IsNullOrWhiteSpace(tableName))
            {
                LoadTableData(tableName);
            }
        }

        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            if (adapter == null || currentTable == null)
            {
                MessageBox.Show("Нет данных для сохранения!");
                return;
            }

            try
            {
                int rowsUpdated = adapter.Update(currentTable);
                MessageBox.Show($"Изменения сохранены! Обновлено строк: {rowsUpdated}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}");
            }
        }
    }
}
