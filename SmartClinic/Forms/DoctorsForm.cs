using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SmartClinic
{
    public partial class DoctorsForm : Form
    {
        public DoctorsForm()
        {
            InitializeComponent();
            LoadDoctors();
        }

        private void LoadDoctors()
        {
            try
            {
                using (var con = DbHelper.GetConnection())
                using (var da = new SqlDataAdapter("SELECT * FROM Doctors", con))
                {
                    var dt = new DataTable();
                    da.Fill(dt);
                    dgvDoctors.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في تحميل الأطباء:\n" + ex.Message);
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (var con = DbHelper.GetConnection())
                using (var cmd = new SqlCommand(
                    "INSERT INTO Doctors (Name, Specialty) VALUES (@n,@s)", con))
                {
                    cmd.Parameters.AddWithValue("@n", txtName.Text.Trim());
                    cmd.Parameters.AddWithValue("@s", txtSpecialty.Text.Trim());
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                LoadDoctors();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في الإضافة:\n" + ex.Message);
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvDoctors.CurrentRow == null) return;

            int id = Convert.ToInt32(dgvDoctors.CurrentRow.Cells["Id"].Value);

            try
            {
                using (var con = DbHelper.GetConnection())
                using (var cmd = new SqlCommand(
                    "UPDATE Doctors SET Name=@n, Specialty=@s WHERE Id=@id", con))
                {
                    cmd.Parameters.AddWithValue("@n", txtName.Text.Trim());
                    cmd.Parameters.AddWithValue("@s", txtSpecialty.Text.Trim());
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                LoadDoctors();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في التعديل:\n" + ex.Message);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDoctors.CurrentRow == null) return;

            int id = Convert.ToInt32(dgvDoctors.CurrentRow.Cells["Id"].Value);

            if (MessageBox.Show("هل تريد حذف هذا الطبيب؟", "تأكيد",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            try
            {
                using (var con = DbHelper.GetConnection())
                using (var cmd = new SqlCommand("DELETE FROM Doctors WHERE Id=@id", con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                LoadDoctors();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في الحذف:\n" + ex.Message);
            }
        }

        private void DgvDoctors_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDoctors.CurrentRow == null) return;

            txtName.Text = dgvDoctors.CurrentRow.Cells["Name"].Value?.ToString();
            txtSpecialty.Text = dgvDoctors.CurrentRow.Cells["Specialty"].Value?.ToString();
        }

        private void ClearInputs()
        {
            txtName.Clear();
            txtSpecialty.Clear();
        }
    }
}