using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace kursach_2_0
{
    public partial class OrdersControl : UserControl
    {
        // Поля формы
        private Button button_word;
        private Button change;
        private Button deleteButton;
        private DataGridView dataGridView1;
        private Button add_button;

        // TextBox для INSERT/UPDATE:
        private TextBox id_cont;  // contract_id
        private TextBox id_bill;  // billboard_id
        private TextBox id_str;   // street_id
        private TextBox cost;         // cost
        private TextBox start;        // startdate
        private TextBox end;          // enddate
        private TextBox pic;          // pictures

        public OrdersControl()
        {
            InitializeComponent();
            LoadData();
        }
        private void InitializeComponent()
        {
            this.button_word = new System.Windows.Forms.Button();
            this.change = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.add_button = new System.Windows.Forms.Button();
            this.id_cont = new System.Windows.Forms.TextBox();
            this.id_bill = new System.Windows.Forms.TextBox();
            this.id_str = new System.Windows.Forms.TextBox();
            this.cost = new System.Windows.Forms.TextBox();
            this.start = new System.Windows.Forms.TextBox();
            this.end = new System.Windows.Forms.TextBox();
            this.pic = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // button_word
            // 
            this.button_word.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.button_word.Location = new System.Drawing.Point(920, 420);
            this.button_word.Name = "button_word";
            this.button_word.Size = new System.Drawing.Size(130, 40);
            this.button_word.TabIndex = 9;
            this.button_word.Text = "Экспорт";
            this.button_word.UseVisualStyleBackColor = true;
            this.button_word.Click += new System.EventHandler(this.button_word_Click);
            // 
            // change
            // 
            this.change.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.change.Location = new System.Drawing.Point(770, 420);
            this.change.Name = "change";
            this.change.Size = new System.Drawing.Size(130, 40);
            this.change.TabIndex = 8;
            this.change.Text = "Изменить";
            this.change.UseVisualStyleBackColor = true;
            this.change.Click += new System.EventHandler(this.change_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.deleteButton.Location = new System.Drawing.Point(620, 420);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(130, 40);
            this.deleteButton.TabIndex = 7;
            this.deleteButton.Text = "Удалить";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(370, 80);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.Size = new System.Drawing.Size(796, 320);
            this.dataGridView1.TabIndex = 6;
            // 
            // add_button
            // 
            this.add_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.add_button.Location = new System.Drawing.Point(470, 420);
            this.add_button.Name = "add_button";
            this.add_button.Size = new System.Drawing.Size(130, 40);
            this.add_button.TabIndex = 5;
            this.add_button.Text = "Добавить";
            this.add_button.UseVisualStyleBackColor = true;
            this.add_button.Click += new System.EventHandler(this.add_button_Click);
            // 
            // id_cont
            // 
            this.id_cont.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.id_cont.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.id_cont.Location = new System.Drawing.Point(60, 80);
            this.id_cont.Name = "id_cont";
            this.id_cont.Size = new System.Drawing.Size(270, 38);
            this.id_cont.TabIndex = 0;
            this.id_cont.Text = "Номер договора";
            this.id_cont.Enter += new System.EventHandler(this.id_cont_Enter);
            this.id_cont.Leave += new System.EventHandler(this.id_cont_Leave);
            // 
            // id_bill
            // 
            this.id_bill.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.id_bill.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.id_bill.Location = new System.Drawing.Point(60, 125);
            this.id_bill.Name = "id_bill";
            this.id_bill.Size = new System.Drawing.Size(270, 38);
            this.id_bill.TabIndex = 1;
            this.id_bill.Text = "Номер щита";
            this.id_bill.Enter += new System.EventHandler(this.id_bill_Enter);
            this.id_bill.Leave += new System.EventHandler(this.id_bill_Leave);
            // 
            // id_str
            // 
            this.id_str.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.id_str.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.id_str.Location = new System.Drawing.Point(60, 170);
            this.id_str.Name = "id_str";
            this.id_str.Size = new System.Drawing.Size(270, 38);
            this.id_str.TabIndex = 2;
            this.id_str.Text = "Улица (ID)";
            this.id_str.Enter += new System.EventHandler(this.id_str_Enter);
            this.id_str.Leave += new System.EventHandler(this.id_str_Leave);
            // 
            // cost
            // 
            this.cost.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.cost.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.cost.Location = new System.Drawing.Point(60, 214);
            this.cost.Name = "cost";
            this.cost.Size = new System.Drawing.Size(270, 38);
            this.cost.TabIndex = 4;
            this.cost.Text = "Стоимость";
            this.cost.Enter += new System.EventHandler(this.cost_Enter);
            this.cost.Leave += new System.EventHandler(this.cost_Leave);
            // 
            // start
            // 
            this.start.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.start.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.start.Location = new System.Drawing.Point(60, 258);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(270, 38);
            this.start.TabIndex = 10;
            this.start.Text = "Начало аренды";
            this.start.Enter += new System.EventHandler(this.start_Enter);
            this.start.Leave += new System.EventHandler(this.start_Leave);
            // 
            // end
            // 
            this.end.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.end.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.end.Location = new System.Drawing.Point(60, 302);
            this.end.Name = "end";
            this.end.Size = new System.Drawing.Size(270, 38);
            this.end.TabIndex = 11;
            this.end.Text = "Конец аренды";
            this.end.Enter += new System.EventHandler(this.end_Enter);
            this.end.Leave += new System.EventHandler(this.end_Leave);
            // 
            // pic
            // 
            this.pic.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.pic.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.pic.Location = new System.Drawing.Point(60, 346);
            this.pic.Name = "pic";
            this.pic.Size = new System.Drawing.Size(270, 38);
            this.pic.TabIndex = 12;
            this.pic.Text = "Дополнительно";
            this.pic.Enter += new System.EventHandler(this.pic_Enter);
            this.pic.Leave += new System.EventHandler(this.pic_Leave);
            // 
            // OrdersControl
            // 
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Controls.Add(this.pic);
            this.Controls.Add(this.end);
            this.Controls.Add(this.start);
            this.Controls.Add(this.button_word);
            this.Controls.Add(this.change);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.add_button);
            this.Controls.Add(this.cost);
            this.Controls.Add(this.id_str);
            this.Controls.Add(this.id_bill);
            this.Controls.Add(this.id_cont);
            this.Name = "OrdersControl";
            this.Size = new System.Drawing.Size(1200, 600);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        /// <summary>
        /// Загрузка данных из таблицы `orders` в DataGridView.
        /// </summary>
        private void LoadData()
        {
            try
            {
                DB db = new DB();
                db.openConnection();

                string query = @"
                    SELECT
                        o.id                 AS 'ID',
                        c.id                 AS 'Договор',        -- contract_id
                        b.id                 AS 'Рекламный щит',  -- billboard_id
                        s.street             AS 'Улица',          -- street_id
                        o.count              AS 'Количество (дней)',
                        o.cost               AS 'Стоимость',
                        o.startdate          AS 'Начало аренды',
                        o.enddate            AS 'Конец аренды',
                        o.pictures           AS 'Дополнительно'
                    FROM `orders` AS o
                    LEFT JOIN `contract`   AS c ON o.contract_id   = c.id
                    LEFT JOIN `billboards` AS b ON o.billboard_id  = b.id
                    LEFT JOIN `street`     AS s ON o.street_id     = s.id
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

        
        private bool Checker()
        {
            if (id_cont.Text == "Номер договора")
            {
                MessageBox.Show("Введите номер договора (contract_id)!");
                return false;
            }
            if (id_bill.Text == "Номер щита")
            {
                MessageBox.Show("Введите номер щита (billboard_id)!");
                return false;
            }
            if (id_str.Text == "Улица (ID)")
            {
                MessageBox.Show("Введите street_id!");
                return false;
            }
            

            if (cost.Text == "Стоимость")
            {
                MessageBox.Show("Введите стоимость!");
                return false;
            }
            if (start.Text == "Начало аренды")
            {
                MessageBox.Show("Введите дату начала аренды (startdate)!");
                return false;
            }
            if (end.Text == "Конец аренды")
            {
                MessageBox.Show("Введите дату окончания аренды (enddate)!");
                return false;
            }
            if (pic.Text == "Дополнительно")
            {
                MessageBox.Show("Введите дополнительную информацию (pictures) или оставьте пустым!");
                return false;
            }

            // Проверки на int
            if (!int.TryParse(id_cont.Text, out _))
            {
                MessageBox.Show("contract_id должно быть числом!");
                return false;
            }
            if (!int.TryParse(id_bill.Text, out _))
            {
                MessageBox.Show("billboard_id должно быть числом!");
                return false;
            }
            if (!int.TryParse(id_str.Text, out _))
            {
                MessageBox.Show("street_id должно быть числом!");
                return false;
            }
            if (!int.TryParse(cost.Text, out _))
            {
                MessageBox.Show("Стоимость (cost) должна быть числом!");
                return false;
            }

            // Проверки на дату
            if (!DateTime.TryParse(start.Text, out _))
            {
                MessageBox.Show("Неверная дата для «Начало аренды»!");
                return false;
            }
            if (!DateTime.TryParse(end.Text, out _))
            {
                MessageBox.Show("Неверная дата для «Конец аренды»!");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Кнопка "Добавить" → INSERT.
        /// Количество (count) вычисляется автоматически как (endDate - startDate).Days.
        /// Если billboard занят – выводим ошибку. Если разница дат < 1 – тоже ошибка.
        /// </summary>
        private void add_button_Click(object sender, EventArgs e)
        {
            if (!Checker())
                return;

            int billboardId = Convert.ToInt32(id_bill.Text);
            DateTime startDate = DateTime.Parse(start.Text);
            DateTime endDate = DateTime.Parse(end.Text);

            // Проверяем, не занят ли щит
            if (CheckBillboardBusy(billboardId, startDate, endDate))
            {
                MessageBox.Show("Рекламный щит занят!");
                return;
            }

            // Вычисляем кол-во дней
            int dayCount = (endDate - startDate).Days;
            if (dayCount < 1)
            {
                MessageBox.Show("Некорректный период аренды (должен быть минимум 1 день)!");
                return;
            }

            DB db = new DB();
            MySqlCommand cmd = new MySqlCommand(@"
                INSERT INTO `orders`
                (`contract_id`,`billboard_id`,`street_id`,`count`,`cost`,`startdate`,`enddate`,`pictures`)
                VALUES
                (@cont_id,@bill_id,@str_id,@dayCount,@cost,@start,@end,@pic)
            ", db.getConnection());

            cmd.Parameters.AddWithValue("@cont_id", Convert.ToInt32(id_cont.Text));
            cmd.Parameters.AddWithValue("@bill_id", billboardId);
            cmd.Parameters.AddWithValue("@str_id", Convert.ToInt32(id_str.Text));
            cmd.Parameters.AddWithValue("@dayCount", dayCount);
            cmd.Parameters.AddWithValue("@cost", Convert.ToInt32(cost.Text));
            cmd.Parameters.AddWithValue("@start", startDate);
            cmd.Parameters.AddWithValue("@end", endDate);
            cmd.Parameters.AddWithValue("@pic", pic.Text);

            try
            {
                db.openConnection();
                if (cmd.ExecuteNonQuery() == 1)
                    MessageBox.Show("Запись успешно добавлена!");
                else
                    MessageBox.Show("Ошибка при добавлении записи!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении: " + ex.Message);
            }
            finally
            {
                db.closeConnection();
            }

            LoadData();
            ResetPlaceholders();
        }

        
        private bool CheckBillboardBusy(int billboardId, DateTime start, DateTime end)
        {
            bool isBusy = false;
            try
            {
                DB db = new DB();
                db.openConnection();

                string query = @"
                    SELECT COUNT(*)
                    FROM `orders`
                    WHERE billboard_id = @bid
                      AND (@start <= enddate)
                      AND (@end >= startdate)
                ";

                MySqlCommand cmd = new MySqlCommand(query, db.getConnection());
                cmd.Parameters.AddWithValue("@bid", billboardId);
                cmd.Parameters.AddWithValue("@start", start);
                cmd.Parameters.AddWithValue("@end", end);

                long count = (long)cmd.ExecuteScalar();
                isBusy = (count > 0);

                db.closeConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при проверке занятости: " + ex.Message);
            }
            return isBusy;
        }

        /// <summary>
        /// Кнопка "Удалить".
        /// </summary>
        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 1)
            {
                MessageBox.Show("Выберите одну строку для удаления!");
                return;
            }
            DialogResult dr = MessageBox.Show("Вы действительно хотите удалить запись?",
                                              "Внимание",
                                              MessageBoxButtons.YesNo,
                                              MessageBoxIcon.Warning);
            if (dr == DialogResult.No) return;

            int orderId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ID"].Value);

            DB db = new DB();
            string query = "DELETE FROM `orders` WHERE `id` = @oid";
            MySqlCommand cmd = new MySqlCommand(query, db.getConnection());
            cmd.Parameters.AddWithValue("@oid", orderId);

            try
            {
                db.openConnection();
                int res = cmd.ExecuteNonQuery();
                if (res == 1)
                {
                    MessageBox.Show("Запись успешно удалена!");
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Ошибка при удалении!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении: " + ex.Message);
            }
            finally
            {
                db.closeConnection();
            }
        }

        /// <summary>
        /// Кнопка "Изменить" (UPDATE).
        /// Теперь также пересчитываем кол-во дней при изменении.
        /// </summary>
        private void change_Click(object sender, EventArgs e)
        {
            if (!Checker())
                return;

            if (dataGridView1.SelectedRows.Count != 1)
            {
                MessageBox.Show("Выделите одну строку для изменения!");
                return;
            }

            int orderId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ID"].Value);

            DateTime startDate = DateTime.Parse(start.Text);
            DateTime endDate = DateTime.Parse(end.Text);
            int billboardId = Convert.ToInt32(id_bill.Text);

            // Проверяем занятость
            if (CheckBillboardBusy(billboardId, startDate, endDate))
            {
                MessageBox.Show("Рекламный щит занят!");
                return;
            }

            // Вычисляем кол-во дней
            int dayCount = (endDate - startDate).Days;
            if (dayCount < 1)
            {
                MessageBox.Show("Некорректный период аренды (должен быть минимум 1 день)!");
                return;
            }

            DB db = new DB();
            string query = @"
                UPDATE `orders`
                SET 
                    `contract_id`  = @cont_id,
                    `billboard_id` = @bill_id,
                    `street_id`    = @str_id,
                    `count`        = @dayCount,
                    `cost`         = @cost,
                    `startdate`    = @start,
                    `enddate`      = @end,
                    `pictures`     = @pic
                WHERE `id`       = @oid
            ";

            MySqlCommand cmd = new MySqlCommand(query, db.getConnection());
            cmd.Parameters.AddWithValue("@cont_id", Convert.ToInt32(id_cont.Text));
            cmd.Parameters.AddWithValue("@bill_id", billboardId);
            cmd.Parameters.AddWithValue("@str_id", Convert.ToInt32(id_str.Text));
            cmd.Parameters.AddWithValue("@dayCount", dayCount);
            cmd.Parameters.AddWithValue("@cost", Convert.ToInt32(cost.Text));
            cmd.Parameters.AddWithValue("@start", startDate);
            cmd.Parameters.AddWithValue("@end", endDate);
            cmd.Parameters.AddWithValue("@pic", pic.Text);
            cmd.Parameters.AddWithValue("@oid", orderId);

            try
            {
                db.openConnection();
                if (cmd.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Запись успешно изменена!");
                    LoadData();
                    ResetPlaceholders();
                }
                else
                {
                    MessageBox.Show("Ошибка при изменении записи!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при изменении: " + ex.Message);
            }
            finally
            {
                db.closeConnection();
            }
        }

        /// <summary>
        /// Кнопка "Экспорт" (TXT).
        /// </summary>
        private void button_word_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Текстовый файл (*.txt)|*.txt|Все файлы (*.*)|*.*";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(sfd.FileName, false, Encoding.UTF8))
                    {
                        // Заголовки
                        for (int c = 0; c < dataGridView1.ColumnCount; c++)
                        {
                            sw.Write(dataGridView1.Columns[c].HeaderText);
                            if (c < dataGridView1.ColumnCount - 1) sw.Write("\t");
                        }
                        sw.WriteLine();

                        // Строки
                        for (int r = 0; r < dataGridView1.RowCount; r++)
                        {
                           
                            if (dataGridView1.AllowUserToAddRows && r == dataGridView1.RowCount - 1)
                                break;

                            for (int c = 0; c < dataGridView1.ColumnCount; c++)
                            {
                                var val = dataGridView1[c, r].Value;
                                sw.Write(val == null ? "" : val.ToString());
                                if (c < dataGridView1.ColumnCount - 1) sw.Write("\t");
                            }
                            sw.WriteLine();
                        }
                    }
                    MessageBox.Show("Экспорт завершён!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при экспорте: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Сброс текстовых полей к плейсхолдерам.
        /// </summary>
        private void ResetPlaceholders()
        {
            id_cont.Text = "Номер договора";
            id_cont.ForeColor = Color.Gray;

            id_bill.Text = "Номер щита";
            id_bill.ForeColor = Color.Gray;

            id_str.Text = "Улица (ID)";
            id_str.ForeColor = Color.Gray;

            cost.Text = "Стоимость";
            cost.ForeColor = Color.Gray;

            start.Text = "Начало аренды";
            start.ForeColor = Color.Gray;

            end.Text = "Конец аренды";
            end.ForeColor = Color.Gray;

            pic.Text = "Дополнительно";
            pic.ForeColor = Color.Gray;
        }

        
        private void id_cont_Enter(object sender, EventArgs e)
        {
            if (id_cont.Text == "Номер договора")
            {
                id_cont.Text = "";
                id_cont.ForeColor = Color.Black;
            }
        }
        private void id_cont_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(id_cont.Text))
            {
                id_cont.Text = "Номер договора";
                id_cont.ForeColor = Color.Gray;
            }
        }

        private void id_bill_Enter(object sender, EventArgs e)
        {
            if (id_bill.Text == "Номер щита")
            {
                id_bill.Text = "";
                id_bill.ForeColor = Color.Black;
            }
        }
        private void id_bill_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(id_bill.Text))
            {
                id_bill.Text = "Номер щита";
                id_bill.ForeColor = Color.Gray;
            }
        }

        private void id_str_Enter(object sender, EventArgs e)
        {
            if (id_str.Text == "Улица (ID)")
            {
                id_str.Text = "";
                id_str.ForeColor = Color.Black;
            }
        }
        private void id_str_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(id_str.Text))
            {
                id_str.Text = "Улица (ID)";
                id_str.ForeColor = Color.Gray;
            }
        }

        private void cost_Enter(object sender, EventArgs e)
        {
            if (cost.Text == "Стоимость")
            {
                cost.Text = "";
                cost.ForeColor = Color.Black;
            }
        }
        private void cost_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cost.Text))
            {
                cost.Text = "Стоимость";
                cost.ForeColor = Color.Gray;
            }
        }

        private void start_Enter(object sender, EventArgs e)
        {
            if (start.Text == "Начало аренды")
            {
                start.Text = "";
                start.ForeColor = Color.Black;
            }
        }
        private void start_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(start.Text))
            {
                start.Text = "Начало аренды";
                start.ForeColor = Color.Gray;
            }
        }

        private void end_Enter(object sender, EventArgs e)
        {
            if (end.Text == "Конец аренды")
            {
                end.Text = "";
                end.ForeColor = Color.Black;
            }
        }
        private void end_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(end.Text))
            {
                end.Text = "Конец аренды";
                end.ForeColor = Color.Gray;
            }
        }

        private void pic_Enter(object sender, EventArgs e)
        {
            if (pic.Text == "Дополнительно")
            {
                pic.Text = "";
                pic.ForeColor = Color.Black;
            }
        }
        private void pic_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(pic.Text))
            {
                pic.Text = "Дополнительно";
                pic.ForeColor = Color.Gray;
            }
        }
    }
}
