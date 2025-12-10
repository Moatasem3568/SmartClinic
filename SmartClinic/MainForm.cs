using System;
using System.Windows.Forms;

namespace SmartClinic
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void BtnPatients_Click(object sender, EventArgs e)
        {
            using (var f = new PatientsForm())
                f.ShowDialog();
        }

        private void BtnDoctors_Click(object sender, EventArgs e)
        {
            using (var f = new DoctorsForm())
                f.ShowDialog();
        }

        private void BtnAppointments_Click(object sender, EventArgs e)
        {
            using (var f = new AppointmentsForm())
                f.ShowDialog();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}