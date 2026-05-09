using Doancuoiki;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CoffeeManager
{
    public partial class frmVoucher : Form
    {
        private int currentVoucherID = -1;

        public frmVoucher()
        {
            InitializeComponent();
        }

        private void frmVoucher_Load(object sender, EventArgs e)
        {
            LoadData();
            dtpExpired.Value = DateTime.Now.AddMonths(1);
        }

        private void LoadData()
        {
            dgvVoucher.DataSource = DataProvider.ExecuteQuery("SELECT id, code, discountPercent, maxDiscount, expiredDate, amount, status FROM Voucher");
            dgvVoucher.Columns["id"].Visible = false;
            if (dgvVoucher.Columns.Contains("status"))
                dgvVoucher.Columns["status"].HeaderText = "Hiệu lực";
        }

        private void dgvVoucher_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvVoucher.Rows[e.RowIndex];
                currentVoucherID = Convert.ToInt32(row.Cells["id"].Value);
                txtCode.Text = row.Cells["code"].Value.ToString();
                txtDiscount.Text = row.Cells["discountPercent"].Value.ToString();
                txtMaxDiscount.Text = row.Cells["maxDiscount"].Value?.ToString();
                txtAmount.Text = row.Cells["amount"].Value.ToString();
                if (row.Cells["expiredDate"].Value != DBNull.Value)
                    dtpExpired.Value = Convert.ToDateTime(row.Cells["expiredDate"].Value);
            }
        }

        private void ClearForm()
        {
            currentVoucherID = -1;
            txtCode.Clear();
            txtDiscount.Clear();
            txtMaxDiscount.Clear();
            txtAmount.Clear();
            dtpExpired.Value = DateTime.Now.AddMonths(1);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCode.Text)) return;
            DataProvider.ExecuteNonQuery(
                "INSERT INTO Voucher (code, discountPercent, maxDiscount, expiredDate, amount, status) VALUES (@c, @d, @m, @e, @a, 1)",
                new SqlParameter[] {
                    new SqlParameter("@c", txtCode.Text),
                    new SqlParameter("@d", txtDiscount.Text),
                    new SqlParameter("@m", string.IsNullOrEmpty(txtMaxDiscount.Text) ? (object)DBNull.Value : txtMaxDiscount.Text),
                    new SqlParameter("@e", dtpExpired.Value),
                    new SqlParameter("@a", txtAmount.Text)
                });
            LoadData();
            ClearForm();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (currentVoucherID == -1) return;
            DataProvider.ExecuteNonQuery(
                "UPDATE Voucher SET code=@c, discountPercent=@d, maxDiscount=@m, expiredDate=@e, amount=@a WHERE id=@id",
                new SqlParameter[] {
                    new SqlParameter("@c", txtCode.Text),
                    new SqlParameter("@d", txtDiscount.Text),
                    new SqlParameter("@m", string.IsNullOrEmpty(txtMaxDiscount.Text) ? (object)DBNull.Value : txtMaxDiscount.Text),
                    new SqlParameter("@e", dtpExpired.Value),
                    new SqlParameter("@a", txtAmount.Text),
                    new SqlParameter("@id", currentVoucherID)
                });
            LoadData();
            ClearForm();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (currentVoucherID == -1) return;
            DataProvider.ExecuteNonQuery("DELETE FROM Voucher WHERE id=@id", new SqlParameter[] { new SqlParameter("@id", currentVoucherID) });
            LoadData();
            ClearForm();
        }
    }
}