using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SmartClinic
{
    public partial class AppointmentsForm : Form
    {
        public AppointmentsForm()
        {
            InitializeComponent();
            LoadDoctors();
            LoadPatients();
            LoadAppointments();
        }

        private void LoadDoctors()
        {
            using (var con = DbHelper.GetConnection())
            using (var da = new SqlDataAdapter("SELECT Id, Name FROM Doctors", con))
            {
                var dt = new DataTable();
                da.Fill(dt);
                cmbDoctor.DataSource = dt;
                cmbDoctor.DisplayMember = "Name";
                cmbDoctor.ValueMember = "Id";
            }
        }

        private void LoadPatients()
        {
            using (var con = DbHelper.GetConnection())
            using (var da = new SqlDataAdapter("SELECT Id, FullName FROM Patients", con))
            {
                var dt = new DataTable();
                da.Fill(dt);
                cmbPatient.DataSource = dt;
                cmbPatient.DisplayMember = "FullName";
                cmbPatient.ValueMember = "Id";
            }
        }

        private void LoadAppointments()
        {
            try
            {
                using (var con = DbHelper.GetConnection())
                using (var da = new SqlDataAdapter(
                    @"SELECT A.Id,
                             P.FullName AS PatientName,
                             D.Name AS DoctorName,
                             A.AppointmentDate,
                             A.Notes
                      FROM Appointments A
                      JOIN Patients P ON A.PatientId = P.Id
                      JOIN Doctors D ON A.DoctorId = D.Id", con))
                {
                    var dt = new DataTable();
                    da.Fill(dt);
                    dgvAppointments.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في تحميل المواعيد:\n" + ex.Message);
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (cmbPatient.SelectedValue == null || cmbDoctor.SelectedValue == null)
            {
                MessageBox.Show("اختر المريض والطبيب", "تنبيه");
                return;
            }

            int patientId = Convert.ToInt32(cmbPatient.SelectedValue);
            int doctorId = Convert.ToInt32(cmbDoctor.SelectedValue);
            DateTime date = dtpDate.Value.Date + dtpTime.Value.TimeOfDay;

            try
            {
                using (var con = DbHelper.GetConnection())
                using (var cmd = new SqlCommand(
                    @"INSERT INTO Appointments (PatientId, DoctorId, AppointmentDate, Notes)
                      VALUES (@p,@d,@dt,@n)", con))
                {
                    cmd.Parameters.AddWithValue("@p", patientId);
                    cmd.Parameters.AddWithValue("@d", doctorId);
                    cmd.Parameters.AddWithValue("@dt", date);
                    cmd.Parameters.AddWithValue("@n", txtNotes.Text.Trim());

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                LoadAppointments();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في إضافة الموعد:\n" + ex.Message);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvAppointments.CurrentRow == null) return;

            int id = Convert.ToInt32(dgvAppointments.CurrentRow.Cells["Id"].Value);

            if (MessageBox.Show("حذف هذا الموعد؟", "تأكيد",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            try
            {
                using (var con = DbHelper.GetConnection())
                using (var cmd = new SqlCommand("DELETE FROM Appointments WHERE Id=@id", con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                LoadAppointments();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في الحذف:\n" + ex.Message);
            }
        }

        private void ClearInputs()
        {
            txtNotes.Clear();
            dtpDate.Value = DateTime.Today;
            dtpTime.Value = DateTime.Now;
        }
    }
}