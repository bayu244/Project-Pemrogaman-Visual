using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Project
{
    public partial class FormProdukUser : Form
    {
        private DatabaseConnection dbConn = new DatabaseConnection();
        private MySqlConnection conn;

        public FormProdukUser()
        {
            InitializeComponent();
            conn = dbConn.GetConnection();
            LoadData();
        }

        void LoadData()
        {
            try
            {
                string query = "SELECT Id, Nama, Deskripsi, Harga, Gambar FROM produk";
                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvProduk.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat data: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvProduk_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvProduk.Rows[e.RowIndex];
                txtNama.Text = row.Cells["Nama"].Value.ToString();
                txtDeskripsi.Text = row.Cells["Deskripsi"].Value.ToString();
                txtHarga.Text = row.Cells["Harga"].Value.ToString();
                pbGambar.ImageLocation = row.Cells["Gambar"].Value.ToString();
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormLogin loginForm = new FormLogin();
            loginForm.Show();
        }
    }
}