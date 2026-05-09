using Doancuoiki;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CoffeeManager
{
    public partial class frmTableArea : Form
    {
        private int currentAreaID = -1;

        public frmTableArea()
        {
            InitializeComponent();
        }

        private void frmTableArea_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            dgvArea.DataSource = DataProvider.ExecuteQuery("SELECT * FROM TableArea");
            dgvArea.Columns["id"].Visible = false;
        }

        private void dgvArea_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvArea.Rows[e.RowIndex];
                currentAreaID = Convert.ToInt32(row.Cells["id"].Value);
                txtName.Text = row.Cells["name"].Value.ToString();
                txtDescription.Text = row.Cells["description"].Value?.ToString();
            }
        }

        private void ClearForm()
        {
            currentAreaID = -1;
            txtName.Clear();
            txtDescription.Clear();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text)) return;
            DataProvider.ExecuteNonQuery("INSERT INTO TableArea (name, description) VALUES (@name, @desc)",
                new SqlParameter[] { new SqlParameter("@name", txtName.Text), new SqlParameter("@desc", txtDescription.Text) });
            LoadData();
            ClearForm();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (currentAreaID == -1) return;
            DataProvider.ExecuteNonQuery("UPDATE TableArea SET name=@name, description=@desc WHERE id=@id",
                new SqlParameter[] { new SqlParameter("@name", txtName.Text), new SqlParameter("@desc", txtDescription.Text), new SqlParameter("@id", currentAreaID) });
            LoadData();
            ClearForm();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (currentAreaID == -1) return;
            DataProvider.ExecuteNonQuery("DELETE FROM TableArea WHERE id=@id", new SqlParameter[] { new SqlParameter("@id", currentAreaID) });
            LoadData();
            ClearForm();
        }
    }
}