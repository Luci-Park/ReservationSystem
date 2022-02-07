using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RestaurantReservation
{
    public partial class ReservationEditor : Form
    {
        int currentIdx = -1;
        public ReservationEditor()
        {
            InitializeComponent();
            addBtn.Text = "추가";
            datePicker.Value = DateTime.Now;
            timePicker.Value = DateTime.Now;
        }
        public ReservationEditor(DataRow row, int index)
        {
            InitializeComponent();
            currentIdx = index;
            string dtstring = row[(int)MainProgram.DataIdx.date].ToString() +" "+ row[(int)MainProgram.DataIdx.time].ToString();
            DateTime dt = DateTime.Parse(dtstring);
            MessageBox.Show(dtstring + "\n" + dt.ToString());
            datePicker.Value = dt;
            timePicker.Value = dt;
            nameBox.Text = row[(int)MainProgram.DataIdx.name].ToString();
            extraBox.Text = row[(int)MainProgram.DataIdx.extra].ToString();
            numericUpDown1.Value = Convert.ToInt32(row[(int)MainProgram.DataIdx.num]);

            addBtn.Text = "수정";
        }

        private void ReservationEditor_Load(object sender, EventArgs e)
        {

        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            if (!IsReservationValid())
            {
                MessageBox.Show("이름이 비어있습니다.");
                return;
            }
            DataRow dr = MainProgram.reservations.NewRow();
            dr[(int)MainProgram.DataIdx.entered] = false;
            dr[(int)MainProgram.DataIdx.date] = datePicker.Value.ToString("yyyy/MM/dd");
            dr[(int)MainProgram.DataIdx.time] = timePicker.Value.ToShortTimeString();
            dr[(int)MainProgram.DataIdx.name] = nameBox.Text;
            dr[(int)MainProgram.DataIdx.num] = numericUpDown1.Value;
            if (string.IsNullOrEmpty(extraBox.Text))
            {
                extraBox.Text = "-";
            }
            dr[(int)MainProgram.DataIdx.extra] = extraBox.Text;
            
            if (currentIdx > -1)
            {
                MainProgram.main.EditReservation(dr, currentIdx);
            }
            else
            {
                MainProgram.main.AddReservation(dr);
            }
            
            //MainProgram.SortReservation();
            currentIdx = -1;
            Close();
        }

        bool IsReservationValid()
        {
            if (string.IsNullOrEmpty(nameBox.Text))
            {
                return false;
            }
            return true;
        }
    }
}
