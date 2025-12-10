using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SmartClinic
{
    public partial class PatientsForm : Form
    {
        public PatientsForm()
        {
            InitializeComponent();
            LoadPatients();
        }

        private void LoadPatients()
        {
            try
            {
                using (var con = DbHelper.GetConnection())
                using (var da = new SqlDataAdapter("SELECT * FROM Patients", con))
                {
                    var dt = new DataTable();
                    da.Fill(dt);
                    dgvPatients.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في تحميل المرضى:\n" + ex.Message);
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (var con = DbHelper.GetConnection())
                using (var cmd = new SqlCommand(
                    "INSERT INTO Patients (FullName, Phone, Address, Notes) VALUES (@n,@p,@a,@no)", con))
                {
                    cmd.Parameters.AddWithValue("@n", txtName.Text.Trim());
                    cmd.Parameters.AddWithValue("@p", txtPhone.Text.Trim());
                    cmd.Parameters.AddWithValue("@a", txtAddress.Text.Trim());
                    cmd.Parameters.AddWithValue("@no", txtNotes.Text.Trim());

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                LoadPatients();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في الإضافة:\n" + ex.Message);
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvPatients.CurrentRow == null) return;

            int id = Convert.ToInt32(dgvPatients.CurrentRow.Cells["Id"].Value);

            try
            {
                using (var con = DbHelper.GetConnection())
                using (var cmd = new SqlCommand(
                    "UPDATE Patients SET FullName=@n, Phone=@p, Address=@a, Notes=@no WHERE Id=@id", con))
                {
                    cmd.Parameters.AddWithValue("@n", txtName.Text.Trim());
                    cmd.Parameters.AddWithValue("@p", txtPhone.Text.Trim());
                    cmd.Parameters.AddWithValue("@a", txtAddress.Text.Trim());
                    cmd.Parameters.AddWithValue("@no", txtNotes.Text.Trim());
                    cmd.Parameters.AddWithValue("@id", id);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                LoadPatients();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في التعديل:\n" + ex.Message);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvPatients.CurrentRow == null) return;

            int id = Convert.ToInt32(dgvPatients.CurrentRow.Cells["Id"].Value);

            if (MessageBox.Show("هل تريد حذف هذا المريض؟", "تأكيد",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            try
            {
                using (var con = DbHelper.GetConnection())
                using (var cmd = new SqlCommand("DELETE FROM Patients WHERE Id=@id", con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                LoadPatients();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في الحذف:\n" + ex.Message);
            }
        }

        private void DgvPatients_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPatients.CurrentRow == null) return;

            txtName.Text = dgvPatients.CurrentRow.Cells["FullName"].Value?.ToString();
            txtPhone.Text = dgvPatients.CurrentRow.Cells["Phone"].Value?.ToString();
            txtAddress.Text = dgvPatients.CurrentRow.Cells["Address"].Value?.ToString();
            txtNotes.Text = dgvPatients.CurrentRow.Cells["Notes"].Value?.ToString();
        }

        private void ClearInputs()
        {
            txtName.Clear();
            txtPhone.Clear();
            txtAddress.Clear();
            txtNotes.Clear();
        }
    }
}