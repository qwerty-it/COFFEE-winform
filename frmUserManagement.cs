using CoffeeManager;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Doancuoiki
{
    public partial class frmUserManagement : Form
    {
        public frmUserManagement()
        {
            InitializeComponent();
        }

        private void frmUserManagement_Load(object sender, EventArgs e)
        {
            txtDisplayName.Text = UserSession.CurrentDisplayName;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string displayName = txtDisplayName.Text.Trim();
            string oldPass = txtOldPassword.Text;
            string newPass = txtNewPassword.Text;
            string confirmPass = txtConfirmPassword.Text;

            if (string.IsNullOrEmpty(displayName))
            {
                MessageBox.Show("Tên hiển thị không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!string.IsNullOrEmpty(newPass) && newPass != confirmPass)
            {
                MessageBox.Show("Mật khẩu mới không khớp!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
            new SqlParameter("@userName", UserSession.CurrentUserName),
            new SqlParameter("@displayName", displayName),
            new SqlParameter("@password", oldPass),
            new SqlParameter("@newPassword", newPass)
                };
                DataProvider.ExecuteNonQuery("USP_UpdateAccount", parameters, CommandType.StoredProcedure);

                UserSession.CurrentDisplayName = displayName;
                MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}