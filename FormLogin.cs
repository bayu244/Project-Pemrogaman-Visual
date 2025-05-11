using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Project
{
    public partial class FormLogin : Form
    {
        private DatabaseConnection dbConn = new DatabaseConnection();
        private MySqlConnection conn;

        public FormLogin()
        {
            InitializeComponent();
            conn = dbConn.GetConnection();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                string query = "SELECT * FROM users WHERE Username=@Username AND Password=@Password";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", txtUsername.Text);
                cmd.Parameters.AddWithValue("@Password", txtPassword.Text);

                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string role = reader["Role"].ToString();
                    this.Hide();

                    if (role == "admin")
                    {
                        FormProdukAdmin adminForm = new FormProdukAdmin();
                        adminForm.Show();
                    }
                    else
                    {
                        FormProdukUser userForm = new FormProdukUser();
                        userForm.Show();
                    }
                }
                else
                {
                    MessageBox.Show("Username atau password salah!", "Login Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormRegistrasi registerForm = new FormRegistrasi();
            registerForm.Show();
        }
    }
}