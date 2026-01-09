using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Word = Microsoft.Office.Interop.Word;

namespace kursach_2_0
{
    public partial class DocumentsControl : UserControl
    {
        // Текстовые поля для ввода данных
        private TextBox RentalText;    // ID арендатора (rental.id)
        private TextBox EmployeeText;  // ID сотрудника (employee.id)
        private TextBox AddText;       // ID доп. услуги (optional.id)
        private TextBox PayText;       // ID типа оплаты (typepay.id)

        // DataGridView для отображения записей (список контрактов)
        private DataGridView dataGridView1;

        // Кнопка "Экспорт"
        private Button buttonExport;

        private TabControl tabControl1;
        private TabPage tabContracts;
        private TabPage tabSql;
        private ComboBox cmbTemplates;
        private TextBox txtSql;
        private Button btnRunSql;
        private Button btnExportCsv;
        private DataGridView gridSql;
        private DataTable _lastSql;

        public DocumentsControl()
        {
            InitializeComponent();
            LoadData();
            InitSqlTab();
        }

        private void InitSqlTab()
        {
            cmbTemplates.Items.Clear();
            cmbTemplates.Items.Add("Договоры дороже 50000 (v_contracts_big)");
            cmbTemplates.Items.Add("Заказы с адресами щитов");
            cmbTemplates.Items.Add("Сумма заказов по договору (функция)");

            cmbTemplates.SelectedIndexChanged += (_, __) =>
            {
                var sel = cmbTemplates.SelectedItem?.ToString() ?? "";
                if (sel.StartsWith("Договоры"))
                {
                    txtSql.Text = "SELECT * FROM v_contracts_big ORDER BY id;";
                }
                else if (sel.StartsWith("Заказы"))
                {
                    txtSql.Text = @"SELECT o.id, o.dateorder, o.startdate, o.enddate, o.`count`, o.cost, (o.`count`*o.cost) AS total,\n       b.address AS billboard, s.street, s.district\nFROM orders o\nJOIN billboards b ON b.id=o.billboard_id\nJOIN street s ON s.id=o.street_id\nORDER BY o.id;";
                }
                else if (sel.StartsWith("Сумма"))
                {
                    txtSql.Text = "SELECT 1 AS contract_id, fn_contract_orders_sum(1) AS total_sum;";
                }
            };

            // Значение по умолчанию
            if (cmbTemplates.Items.Count > 0)
                cmbTemplates.SelectedIndex = 0;

            btnRunSql.Click += (_, __) => RunSql();
            btnExportCsv.Click += (_, __) => ExportSqlToCsv();
        }

        /// <summary>
        /// Загрузка данных в DataGridView.
        
        /// </summary>
        private void LoadData()
        {
            try
            {
                DB db = new DB();
                db.openConnection();

                string query = @"
                    SELECT
                        c.id        AS 'Договор',
                        c.dateorder AS 'Дата подписания',
                        c.cost      AS 'Стоимость итого',

                        r.name      AS 'Название арендатора',
                        r.status    AS 'Статус арендатора',

                        e.name      AS 'Имя сотрудника',
                        e.surname   AS 'Фамилия сотрудника',

                        t.type      AS 'Вид оплаты',
                        o.service   AS 'Доп. услуга'
                    FROM contract c
                    LEFT JOIN rental   r ON c.renter_id   = r.id
                    LEFT JOIN employee e ON c.employee_id = e.id
                    LEFT JOIN typepay  t ON c.typepay_id  = t.id
                    LEFT JOIN `optional` o ON c.optional_id = o.id
                    ORDER BY c.id DESC
                ";

                MySqlCommand command = new MySqlCommand(query, db.getConnection());
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;

                db.closeConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
            }
        }

        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabContracts = new System.Windows.Forms.TabPage();
            this.tabSql = new System.Windows.Forms.TabPage();

            this.RentalText = new System.Windows.Forms.TextBox();
            this.EmployeeText = new System.Windows.Forms.TextBox();
            this.AddText = new System.Windows.Forms.TextBox();
            this.PayText = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.buttonExport = new System.Windows.Forms.Button();

            this.cmbTemplates = new System.Windows.Forms.ComboBox();
            this.txtSql = new System.Windows.Forms.TextBox();
            this.btnRunSql = new System.Windows.Forms.Button();
            this.btnExportCsv = new System.Windows.Forms.Button();
            this.gridSql = new System.Windows.Forms.DataGridView();

            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSql)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabContracts.SuspendLayout();
            this.tabSql.SuspendLayout();
            this.SuspendLayout();

            // 
            // tabControl1
            // 
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Controls.Add(this.tabContracts);
            this.tabControl1.Controls.Add(this.tabSql);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1174, 524);
            this.tabControl1.TabIndex = 0;

            // 
            // tabContracts
            // 
            this.tabContracts.Text = "Документ (чек)";
            this.tabContracts.UseVisualStyleBackColor = true;

            // 
            // RentalText
            // 
            this.RentalText.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.RentalText.Location = new System.Drawing.Point(49, 90);
            this.RentalText.Name = "RentalText";
            this.RentalText.Size = new System.Drawing.Size(150, 22);
            this.RentalText.TabIndex = 0;
            this.RentalText.Text = "Арендатор";
            this.RentalText.Enter += new System.EventHandler(this.RentalText_Enter);
            this.RentalText.Leave += new System.EventHandler(this.RentalText_Leave);

            // 
            // EmployeeText
            // 
            this.EmployeeText.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.EmployeeText.Location = new System.Drawing.Point(49, 132);
            this.EmployeeText.Name = "EmployeeText";
            this.EmployeeText.Size = new System.Drawing.Size(150, 22);
            this.EmployeeText.TabIndex = 1;
            this.EmployeeText.Text = "Сотрудник";
            this.EmployeeText.Enter += new System.EventHandler(this.EmployeeText_Enter);
            this.EmployeeText.Leave += new System.EventHandler(this.EmployeeText_Leave);

            // 
            // AddText
            // 
            this.AddText.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.AddText.Location = new System.Drawing.Point(49, 173);
            this.AddText.Name = "AddText";
            this.AddText.Size = new System.Drawing.Size(150, 22);
            this.AddText.TabIndex = 2;
            this.AddText.Text = "Дополнительно";
            this.AddText.Enter += new System.EventHandler(this.AddText_Enter);
            this.AddText.Leave += new System.EventHandler(this.AddText_Leave);

            // 
            // PayText
            // 
            this.PayText.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.PayText.Location = new System.Drawing.Point(49, 217);
            this.PayText.Name = "PayText";
            this.PayText.Size = new System.Drawing.Size(150, 22);
            this.PayText.TabIndex = 3;
            this.PayText.Text = "Вид оплаты";
            this.PayText.Enter += new System.EventHandler(this.PayText_Enter);
            this.PayText.Leave += new System.EventHandler(this.PayText_Leave);

            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView1.ColumnHeadersHeight = 29;
            this.dataGridView1.Location = new System.Drawing.Point(241, 44);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.Size = new System.Drawing.Size(893, 390);
            this.dataGridView1.TabIndex = 4;

            // 
            // buttonExport
            // 
            this.buttonExport.Location = new System.Drawing.Point(49, 265);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(142, 37);
            this.buttonExport.TabIndex = 5;
            this.buttonExport.Text = "Экспорт (Word)";
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);

            this.tabContracts.Controls.Add(this.buttonExport);
            this.tabContracts.Controls.Add(this.dataGridView1);
            this.tabContracts.Controls.Add(this.PayText);
            this.tabContracts.Controls.Add(this.AddText);
            this.tabContracts.Controls.Add(this.EmployeeText);
            this.tabContracts.Controls.Add(this.RentalText);

            // 
            // tabSql
            // 
            this.tabSql.Text = "SQL-запросы";
            this.tabSql.UseVisualStyleBackColor = true;

            // 
            // cmbTemplates
            // 
            this.cmbTemplates.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTemplates.Location = new System.Drawing.Point(16, 14);
            this.cmbTemplates.Name = "cmbTemplates";
            this.cmbTemplates.Size = new System.Drawing.Size(530, 24);
            this.cmbTemplates.TabIndex = 0;

            // 
            // btnRunSql
            // 
            this.btnRunSql.Location = new System.Drawing.Point(560, 12);
            this.btnRunSql.Name = "btnRunSql";
            this.btnRunSql.Size = new System.Drawing.Size(140, 28);
            this.btnRunSql.TabIndex = 1;
            this.btnRunSql.Text = "Выполнить";
            this.btnRunSql.UseVisualStyleBackColor = true;

            // 
            // btnExportCsv
            // 
            this.btnExportCsv.Location = new System.Drawing.Point(708, 12);
            this.btnExportCsv.Name = "btnExportCsv";
            this.btnExportCsv.Size = new System.Drawing.Size(170, 28);
            this.btnExportCsv.TabIndex = 2;
            this.btnExportCsv.Text = "Экспорт CSV";
            this.btnExportCsv.UseVisualStyleBackColor = true;

            // 
            // txtSql
            // 
            this.txtSql.Location = new System.Drawing.Point(16, 50);
            this.txtSql.Multiline = true;
            this.txtSql.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSql.WordWrap = false;
            this.txtSql.Name = "txtSql";
            this.txtSql.Size = new System.Drawing.Size(1130, 130);
            this.txtSql.TabIndex = 3;
            this.txtSql.Font = new System.Drawing.Font("Consolas", 10F);

            // 
            // gridSql
            // 
            this.gridSql.AllowUserToOrderColumns = true;
            this.gridSql.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridSql.ColumnHeadersHeight = 29;
            this.gridSql.Location = new System.Drawing.Point(16, 190);
            this.gridSql.Name = "gridSql";
            this.gridSql.RowHeadersWidth = 51;
            this.gridSql.Size = new System.Drawing.Size(1130, 290);
            this.gridSql.TabIndex = 4;

            this.tabSql.Controls.Add(this.gridSql);
            this.tabSql.Controls.Add(this.txtSql);
            this.tabSql.Controls.Add(this.btnExportCsv);
            this.tabSql.Controls.Add(this.btnRunSql);
            this.tabSql.Controls.Add(this.cmbTemplates);

            // 
            // DocumentsControl
            // 
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Controls.Add(this.tabControl1);
            this.Name = "DocumentsControl";
            this.Size = new System.Drawing.Size(1174, 524);

            this.tabControl1.ResumeLayout(false);
            this.tabContracts.ResumeLayout(false);
            this.tabContracts.PerformLayout();
            this.tabSql.ResumeLayout(false);
            this.tabSql.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSql)).EndInit();
            this.ResumeLayout(false);
        }

        // Плейсхолдеры
        private void RentalText_Enter(object sender, EventArgs e)
        {
            if (RentalText.Text == "Арендатор")
            {
                RentalText.Text = "";
                RentalText.ForeColor = Color.Black;
            }
        }
        private void RentalText_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(RentalText.Text))
            {
                RentalText.Text = "Арендатор";
                RentalText.ForeColor = Color.Gray;
            }
        }

        private void EmployeeText_Enter(object sender, EventArgs e)
        {
            if (EmployeeText.Text == "Сотрудник")
            {
                EmployeeText.Text = "";
                EmployeeText.ForeColor = Color.Black;
            }
        }
        private void EmployeeText_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EmployeeText.Text))
            {
                EmployeeText.Text = "Сотрудник";
                EmployeeText.ForeColor = Color.Gray;
            }
        }

        private void AddText_Enter(object sender, EventArgs e)
        {
            if (AddText.Text == "Дополнительно")
            {
                AddText.Text = "";
                AddText.ForeColor = Color.Black;
            }
        }
        private void AddText_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AddText.Text))
            {
                AddText.Text = "Дополнительно";
                AddText.ForeColor = Color.Gray;
            }
        }

        private void PayText_Enter(object sender, EventArgs e)
        {
            if (PayText.Text == "Вид оплаты")
            {
                PayText.Text = "";
                PayText.ForeColor = Color.Black;
            }
        }
        private void PayText_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PayText.Text))
            {
                PayText.Text = "Вид оплаты";
                PayText.ForeColor = Color.Gray;
            }
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            // Проверка, что введены числовые ID
            if (!int.TryParse(RentalText.Text, out int renterId) ||
                !int.TryParse(EmployeeText.Text, out int employeeId) ||
                !int.TryParse(AddText.Text, out int optionalId) ||
                !int.TryParse(PayText.Text, out int payTypeId))
            {
                MessageBox.Show("Введите числовые ID: Арендатор, Сотрудник, Дополнительно, Вид оплаты.");
                return;
            }

            // 1) Вставляем контракт (cost=0)
            int contractId = InsertContract(renterId, employeeId, optionalId, payTypeId);
            if (contractId == -1)
            {
                MessageBox.Show("Ошибка при добавлении записи в таблицу contract.");
                return;
            }

            // 2) Суммируем стоимость заказов + доп. услуга
            int totalOrderCost = GetOrderCost(contractId);
            int optionalCost = GetOptionalServiceCost(optionalId);
            int totalCost = totalOrderCost + optionalCost;

            // 3) Обновляем cost в contract
            if (!UpdateContractCost(contractId, totalCost))
            {
                MessageBox.Show("Ошибка при обновлении стоимости в таблице contract.");
                return;
            }

            // 4) Формируем и экспортируем чек в Word
            string orderDetails = GetOrderInfoForContract(contractId);
            ExportReceiptToWord(contractId, renterId, employeeId, payTypeId, optionalId, orderDetails, totalCost);

            LoadData();
        }

        /// <summary>
        /// Вставляем запись в contract (дата подписания = сегодня, cost=0) и возвращаем новый id.
        /// </summary>
        private int InsertContract(int renterId, int employeeId, int optionalId, int payTypeId)
        {
            int newId = -1;
            try
            {
                DB db = new DB();
                db.openConnection();

                string query = @"
                    INSERT INTO contract
                      (renter_id, dateorder, employee_id, optional_id, cost, typepay_id)
                    VALUES
                      (@renter, @now, @emp, @opt, 0, @pay);
                    SELECT LAST_INSERT_ID();
                ";

                MySqlCommand cmd = new MySqlCommand(query, db.getConnection());
                cmd.Parameters.AddWithValue("@renter", renterId);
                cmd.Parameters.AddWithValue("@now", DateTime.Now);
                cmd.Parameters.AddWithValue("@emp", employeeId);
                cmd.Parameters.AddWithValue("@opt", optionalId);
                cmd.Parameters.AddWithValue("@pay", payTypeId);

                object result = cmd.ExecuteScalar();
                if (result != null)
                    newId = Convert.ToInt32(result);

                db.closeConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при вставке в contract: " + ex.Message);
            }
            return newId;
        }

        /// <summary>
        /// Суммарная стоимость заказов для контракта: SUM(count * cost) из таблицы orders.
        /// </summary>
        private int GetOrderCost(int contractId)
        {
            int total = 0;
            try
            {
                DB db = new DB();
                db.openConnection();

                string query = @"
                    SELECT `count`, `cost`
                    FROM orders
                    WHERE contract_id = @cid
                ";
                MySqlCommand cmd = new MySqlCommand(query, db.getConnection());
                cmd.Parameters.AddWithValue("@cid", contractId);

                using (MySqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        int cnt = Convert.ToInt32(rdr["count"]);
                        int cst = Convert.ToInt32(rdr["cost"]);
                        total += (cnt * cst);
                    }
                }

                db.closeConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при получении стоимости заказов: " + ex.Message);
            }
            return total;
        }

        private string GetRenterName(int renterId)
        {
            try
            {
                DB db = new DB();
                db.openConnection();

                string query = "SELECT name FROM rental WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(query, db.getConnection());
                cmd.Parameters.AddWithValue("@id", renterId);

                object result = cmd.ExecuteScalar();
                db.closeConnection();

                return result?.ToString() ?? "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при получении названия арендатора: " + ex.Message);
                return "";
            }
        }

        private string GetEmployeeName(int employeeId)
        {
            try
            {
                DB db = new DB();
                db.openConnection();

                string query = "SELECT CONCAT(name, ' ', surname) FROM employee WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(query, db.getConnection());
                cmd.Parameters.AddWithValue("@id", employeeId);

                object result = cmd.ExecuteScalar();
                db.closeConnection();

                return result?.ToString() ?? "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при получении имени сотрудника: " + ex.Message);
                return "";
            }
        }

        private string GetPayTypeName(int payTypeId)
        {
            try
            {
                DB db = new DB();
                db.openConnection();

                string query = "SELECT type FROM typepay WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(query, db.getConnection());
                cmd.Parameters.AddWithValue("@id", payTypeId);

                object result = cmd.ExecuteScalar();
                db.closeConnection();

                return result?.ToString() ?? "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при получении типа оплаты: " + ex.Message);
                return "";
            }
        }

        private string GetOptionalServiceName(int optionalId)
        {
            try
            {
                DB db = new DB();
                db.openConnection();

                string query = "SELECT service FROM `optional` WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(query, db.getConnection());
                cmd.Parameters.AddWithValue("@id", optionalId);

                object result = cmd.ExecuteScalar();
                db.closeConnection();

                return result?.ToString() ?? "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при получении дополнительной услуги: " + ex.Message);
                return "";
            }
        }

        private int GetOptionalServiceCost(int optId)
        {
            try
            {
                DB db = new DB();
                db.openConnection();

                string query = "SELECT cost FROM `optional` WHERE id = @oid";
                MySqlCommand cmd = new MySqlCommand(query, db.getConnection());
                cmd.Parameters.AddWithValue("@oid", optId);

                object res = cmd.ExecuteScalar();
                db.closeConnection();

                return res != null ? Convert.ToInt32(res) : 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при получении стоимости доп. услуги: " + ex.Message);
                return 0;
            }
        }

        private bool UpdateContractCost(int contractId, int totalCost)
        {
            try
            {
                DB db = new DB();
                db.openConnection();

                string query = "UPDATE contract SET cost = @cost WHERE id = @cid";
                MySqlCommand cmd = new MySqlCommand(query, db.getConnection());
                cmd.Parameters.AddWithValue("@cost", totalCost);
                cmd.Parameters.AddWithValue("@cid", contractId);

                int rows = cmd.ExecuteNonQuery();
                db.closeConnection();

                return rows == 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при обновлении поля cost: " + ex.Message);
                return false;
            }
        }

        private string GetOrderInfoForContract(int contractId)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                DB db = new DB();
                db.openConnection();

                string query = @"
                    SELECT billboard_id, startdate, enddate, `count`, cost, pictures
                    FROM orders
                    WHERE contract_id = @cid
                ";

                MySqlCommand cmd = new MySqlCommand(query, db.getConnection());
                cmd.Parameters.AddWithValue("@cid", contractId);

                using (MySqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        int billboard = Convert.ToInt32(rdr["billboard_id"]);
                        DateTime st = Convert.ToDateTime(rdr["startdate"]);
                        DateTime en = Convert.ToDateTime(rdr["enddate"]);
                        int cnt = Convert.ToInt32(rdr["count"]);
                        int cst = Convert.ToInt32(rdr["cost"]);
                        string pics = rdr["pictures"]?.ToString() ?? "";

                        int totalOrder = cnt * cst;
                        sb.AppendLine($"Рекламный щит: {billboard}");
                        sb.AppendLine($"Начало аренды: {st:dd-MM-yyyy}");
                        sb.AppendLine($"Конец аренды: {en:dd-MM-yyyy}");
                        sb.AppendLine($"Количество (дней): {cnt}");
                        sb.AppendLine($"Цена за единицу: {cst}");
                        sb.AppendLine($"Итоговая стоимость заказа: {totalOrder}");
                        sb.AppendLine($"Дополнительно: {pics}");
                        sb.AppendLine("----------------------------");
                    }
                }

                db.closeConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при получении заказов: " + ex.Message);
            }
            return sb.ToString();
        }

        private void ExportReceiptToWord(
            int contractId,
            int renterId,
            int employeeId,
            int payTypeId,
            int optionalId,
            string orderDetails,
            int totalContractCost)
        {
            try
            {
                string renterName = GetRenterName(renterId);
                string employeeName = GetEmployeeName(employeeId);
                string payTypeName = GetPayTypeName(payTypeId);
                string optionalService = GetOptionalServiceName(optionalId);

                string todayDate = DateTime.Now.ToString("dd-MM-yyyy");

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("ЧЕК ОБ ОПЛАТЕ");
                sb.AppendLine("----------------------------");
                sb.AppendLine($"Номер контракта: {contractId}");
                sb.AppendLine($"Дата подписания: {todayDate}");
                sb.AppendLine($"Арендатор: {renterName}");
                sb.AppendLine($"Сотрудник: {employeeName}");
                sb.AppendLine($"Вид оплаты: {payTypeName}");
                sb.AppendLine($"Дополнительно: {optionalService}");
                sb.AppendLine("----------------------------");
                sb.AppendLine("Информация по заказам:");
                sb.AppendLine(orderDetails);
                sb.AppendLine("----------------------------");
                sb.AppendLine($"Итоговая стоимость по договору: {totalContractCost}");
                sb.AppendLine("----------------------------");
                sb.AppendLine("Спасибо за оплату!");

                Word.Application wordApp = new Word.Application();
                wordApp.Visible = false;
                Word.Document doc = wordApp.Documents.Add();

                Word.Paragraph para = doc.Content.Paragraphs.Add();
                para.Range.Text = sb.ToString();
                para.Range.Font.Name = "Arial";
                para.Range.Font.Size = 12;
                para.Range.InsertParagraphAfter();

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Word Document (*.docx)|*.docx";
                sfd.FileName = "Чек.docx";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    doc.SaveAs2(sfd.FileName);
                    MessageBox.Show("Документ успешно создан!");
                }
                else
                {
                    MessageBox.Show("Сохранение документа отменено.");
                }

                doc.Close();
                wordApp.Quit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при экспорте в Word: " + ex.Message);
            }
        }

        private void RunSql()
        {
            try
            {
                string sql = (txtSql.Text ?? string.Empty).Trim();
                if (string.IsNullOrWhiteSpace(sql))
                {
                    MessageBox.Show("Введите SQL-запрос.");
                    return;
                }

                string firstToken = sql.Split(new[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                        .FirstOrDefault()?.ToUpperInvariant() ?? "";

                if (!(firstToken == "SELECT" || firstToken == "WITH"))
                {
                    MessageBox.Show("Разрешены только запросы SELECT (и WITH ... SELECT).\nИзменение данных выполняйте в других разделах приложения.");
                    return;
                }

                DB db = new DB();
                db.openConnection();
                using (MySqlCommand cmd = new MySqlCommand(sql, db.getConnection()))
                {
                    using (MySqlDataAdapter ad = new MySqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        ad.Fill(dt);
                        _lastSql = dt;
                        gridSql.DataSource = dt;
                    }
                }
                db.closeConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка выполнения SQL: " + ex.Message);
            }
        }

        private void ExportSqlToCsv()
        {
            try
            {
                if (_lastSql == null || _lastSql.Columns.Count == 0)
                {
                    MessageBox.Show("Нет данных для экспорта. Сначала выполните запрос.");
                    return;
                }

                SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = "CSV (*.csv)|*.csv",
                    FileName = "document_export.csv"
                };

                if (sfd.ShowDialog() != DialogResult.OK)
                    return;

                using (var sw = new StreamWriter(sfd.FileName, false, new UTF8Encoding(true)))
                {
                    // Заголовки
                    sw.WriteLine(string.Join(";", _lastSql.Columns.Cast<DataColumn>().Select(c => EscapeCsv(c.ColumnName))));

                    // Строки
                    foreach (DataRow row in _lastSql.Rows)
                    {
                        var cells = _lastSql.Columns.Cast<DataColumn>()
                            .Select(c => EscapeCsv(row[c]?.ToString() ?? string.Empty));
                        sw.WriteLine(string.Join(";", cells));
                    }
                }

                MessageBox.Show("CSV сохранён успешно.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка экспорта CSV: " + ex.Message);
            }
        }

        private static string EscapeCsv(string value)
        {
            if (value == null) return "";
            bool needQuotes = value.Contains(";") || value.Contains('"') || value.Contains("\n") || value.Contains("\r");
            value = value.Replace("\"", "\"\"");
            return needQuotes ? $"\"{value}\"" : value;
        }
    }
}
