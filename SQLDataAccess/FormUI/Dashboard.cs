using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

namespace FormUI
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection(@"Data Source=ASUS\SQL;Initial Catalog=TestForApplication;Integrated Security=True");
        public int StudentID;

        private void Dashboard_Load(object sender, EventArgs e)
        {
            GetStudentsRecord();
        }

        private void GetStudentsRecord()
        {
            
            SqlCommand cmd = new SqlCommand("Select * from StudentsTb", con);
            DataTable dt = new DataTable();

            con.Open();

            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();

            StudentRecordDataGridView.DataSource = dt;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (isValid())
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO StudentsTb VALUES (@name, @FatherName, @Roll, @Address, @Mobile)", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@name", txtStudentName.Text);
                cmd.Parameters.AddWithValue("@FatherName", txtFatherName.Text);
                cmd.Parameters.AddWithValue("@Roll", txtRollNumber.Text);
                cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                cmd.Parameters.AddWithValue("@Mobile", txtMobile.Text);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("New student is successfully saved in the database", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

                GetStudentsRecord();
                ResetFormControls();
            }
        }

        private bool isValid()
        {
            if(txtStudentName.Text == string.Empty)
            {
                MessageBox.Show("Student Name is Required", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            ResetFormControls();
        }

        private void ResetFormControls()
        {
            StudentID = 0;
            txtStudentName.Clear();
            txtFatherName.Clear();
            txtRollNumber.Clear();
            txtAddress.Clear();
            txtMobile.Clear();

            txtStudentName.Focus();
        }

        private void StudentRecordDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            StudentID = Convert.ToInt32(StudentRecordDataGridView.SelectedRows[0].Cells[0].Value);
            txtStudentName.Text = StudentRecordDataGridView.SelectedRows[0].Cells[1].Value.ToString();
            txtFatherName.Text = StudentRecordDataGridView.SelectedRows[0].Cells[2].Value.ToString();
            txtRollNumber.Text = StudentRecordDataGridView.SelectedRows[0].Cells[3].Value.ToString();
            txtAddress.Text = StudentRecordDataGridView.SelectedRows[0].Cells[4].Value.ToString();
            txtMobile.Text = StudentRecordDataGridView.SelectedRows[0].Cells[5].Value.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (StudentID > 0)
            {
                SqlCommand cmd = new SqlCommand("UPDATE StudentsTb SET Name = @name, FatherName = @FatherName, RollNumber = @Roll, Address = @Address, Mobile = @Mobile WHERE StudentID = @ID", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@name", txtStudentName.Text);
                cmd.Parameters.AddWithValue("@FatherName", txtFatherName.Text);
                cmd.Parameters.AddWithValue("@Roll", txtRollNumber.Text);
                cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                cmd.Parameters.AddWithValue("@Mobile", txtMobile.Text);
                cmd.Parameters.AddWithValue("@ID", this.StudentID);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Student information is updated successfully", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);

                GetStudentsRecord();
                ResetFormControls();
            }
            else
            {
                MessageBox.Show("Please select a student to update his/her information", "Select?", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(StudentID > 0)
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM StudentsTb WHERE StudentID = @ID", con);
                cmd.CommandType = CommandType.Text;
                
                cmd.Parameters.AddWithValue("@ID", this.StudentID);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Student is deleted from the system", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);

                GetStudentsRecord();
                ResetFormControls();
            }
            else
            {
                MessageBox.Show("Please select a student to delete", "Select?", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
    }
}
