using System.Windows.Forms;

namespace kursach_2_0
{
    /// <summary>
    /// Хост-контрол для вкладки "Справка".
    /// Нужен, чтобы пункт меню мог загружаться динамически из таблицы app_menu.
    /// </summary>
    public class ReferenceControlHost : UserControl
    {
        public ReferenceControlHost()
        {
            Dock = DockStyle.Fill;

            var tabs = new TabControl
            {
                Dock = DockStyle.Fill
            };

            var about = new TabPage("О программе")
            {
                BackColor = System.Drawing.Color.DarkGray
            };

            about.Controls.Add(new Label
            {
                Dock = DockStyle.Fill,
                AutoSize = false,
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                Padding = new Padding(16),
                Text = "Программа выполнена студенткой 3-го курса АВТФ группы АП 227 Евдокимовой Анастасией."
            });

            var content = new TabPage("Содержание")
            {
                BackColor = System.Drawing.Color.DarkGray
            };
            content.Controls.Add(new Label
            {
                Dock = DockStyle.Fill,
                AutoSize = false,
                TextAlign = System.Drawing.ContentAlignment.TopLeft,
                Padding = new Padding(16),
                Text = "Содержание формируется вкладками главного меню: Заказы, Разное, Справочники, Документы, Сотрудники (для администратора)."
            });

            tabs.TabPages.Add(content);
            tabs.TabPages.Add(about);

            Controls.Add(tabs);
        }
    }
}
