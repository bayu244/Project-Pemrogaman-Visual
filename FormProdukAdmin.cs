using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;

namespace Project
{
    public partial class FormProdukAdmin : Form
    {
        DatabaseConnection dbConn = new DatabaseConnection();
        MySqlConnection conn;

        public FormProdukAdmin()
        {
            InitializeComponent();
            conn = dbConn.GetConnection();
        }

        private void FormProdukAdmin_Load(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                MessageBox.Show("Koneksi ke database berhasil!", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                conn.Close();

                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal terkoneksi ke database!\n\n" + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        void LoadData()
        {
            try
            {
                string query = "SELECT * FROM produk";
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

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (pbGambar.ImageLocation == null)
            {
                MessageBox.Show("Silakan pilih gambar terlebih dahulu.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string path = Path.GetFileName(pbGambar.ImageLocation);

            try
            {
                Directory.CreateDirectory("img");
                File.Copy(pbGambar.ImageLocation, Path.Combine("img", path), true);

                string query = "INSERT INTO produk (Nama, Deskripsi, Harga, Gambar, Stok) VALUES (@Nama, @Deskripsi, @Harga, @Gambar, @Stok)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nama", txtNama.Text);
                cmd.Parameters.AddWithValue("@Deskripsi", txtDeskripsi.Text);
                cmd.Parameters.AddWithValue("@Harga", txtHarga.Text);
                cmd.Parameters.AddWithValue("@Gambar", "img/" + path);
                cmd.Parameters.AddWithValue("@Stok", txtStok.Text);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menambahkan data: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "UPDATE produk SET Nama=@Nama, Deskripsi=@Deskripsi, Harga=@Harga, Stok=@Stok WHERE Id=@Id";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nama", txtNama.Text);
                cmd.Parameters.AddWithValue("@Deskripsi", txtDeskripsi.Text);
                cmd.Parameters.AddWithValue("@Harga", txtHarga.Text);
                cmd.Parameters.AddWithValue("@Stok", txtStok.Text);
                cmd.Parameters.AddWithValue("@Id", txtId.Text);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memperbarui data: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "DELETE FROM produk WHERE Id=@Id";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", txtId.Text);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menghapus data: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }
        }

        private void dgvProduk_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvProduk.Rows[e.RowIndex];
                txtId.Text = row.Cells["Id"].Value.ToString();
                txtNama.Text = row.Cells["Nama"].Value.ToString();
                txtDeskripsi.Text = row.Cells["Deskripsi"].Value.ToString();
                txtHarga.Text = row.Cells["Harga"].Value.ToString();
                txtStok.Text = row.Cells["Stok"].Value.ToString();
                pbGambar.ImageLocation = row.Cells["Gambar"].Value.ToString();
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pbGambar.ImageLocation = openFileDialog1.FileName;
            }
        }

        private void FormProdukAdmin_Load_1(object sender, EventArgs e)
        {
        }
    }
}