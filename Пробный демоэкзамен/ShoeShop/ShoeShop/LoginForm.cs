using System;
using System.Windows.Forms;

namespace ShoeShop
{
    public partial class LoginForm : Form
    {
        private LoginDB dbHelper;

        public LoginForm()
        {
            InitializeComponent();
            dbHelper = new LoginDB();
        }

        private void LoginForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string login = textBox1.Text.Trim();
            string password = textBox2.Text;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var authResult = dbHelper.AuthenticateUser(login, password);

            if (authResult.Success)
            {
                MessageBox.Show($"Добро пожаловать!", "Успешная авторизация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Открываем соответствующую форму в зависимости от роли
                OpenRoleBasedForm(authResult.Role);
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль", "Ошибка авторизации",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenRoleBasedForm(string role)
        {
            Form formToOpen = null;

            switch (role.ToLower())
            {
                case "администратор":
                case "administrator":
                    formToOpen = new AdminForm();
                    break;
                case "менеджер":
                case "manager":
                    formToOpen = new ManagerForm();
                    break;
                case "авторизированный клиент":
                case "user":
                    formToOpen = new UserForm();
                    break;
                default:
                    MessageBox.Show($"Роль '{role}' не распознана. Обратитесь к администратору.",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
            }

            if (formToOpen != null)
            {
                this.Hide();
                formToOpen.Show();
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                textBox2.Focus();
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                button1_Click(sender, e);
                e.Handled = true;
            }
        }
    }
}
