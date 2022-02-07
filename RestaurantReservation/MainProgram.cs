using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RestaurantReservation
{
    public partial class MainProgram : Form
    {
        public static DataTable reservations;
        public static MainProgram main;
        public enum DataIdx { entered, date, time, name, num, extra, numOfData};

        //different because of datatable issues;

        ReservationEditor editor;

        public MainProgram()
        {
            InitializeComponent();
            if(main == null)
            {
                main = this;
            }
            AutoSize = true;
            SetDataTable();
            ShowDataTable();
            SortReservation();
            FilterByDate(monthCalendar1.SelectionStart.Date);
        }

        #region dataTableSet
        void SetDataTable()
        {
            reservations = new DataTable();
            //prepare columns
            reservations.Columns.Add("방문여부", typeof(bool));
            reservations.Columns.Add("날짜", typeof(string));
            reservations.Columns.Add("시간", typeof(string));
            reservations.Columns.Add("이름", typeof(string));
            reservations.Columns.Add("인원 수", typeof(int));
            reservations.Columns.Add("비고", typeof(string));
  

            reservations = CSVUtlity.ConvertCSVtoDataTable(reservations);

        }

        void ShowDataTable()
        {
            dataGridView.AutoSize = true;
            GridRefresh();
            for (int i = 1; i<dataGridView.Columns.Count; i++)
            {
                dataGridView.Columns[i].ReadOnly = true;
            }

            DataGridViewButtonColumn edtbuttonColumn = new DataGridViewButtonColumn();
            edtbuttonColumn.Text = "수정";
            edtbuttonColumn.Name = "수정";
            edtbuttonColumn.HeaderText = "";
            edtbuttonColumn.UseColumnTextForButtonValue = true;
            dataGridView.Columns.Add(edtbuttonColumn);

            DataGridViewButtonColumn delbuttonColumn = new DataGridViewButtonColumn();
            delbuttonColumn.Text = "삭제";
            delbuttonColumn.Name = "삭제";
            delbuttonColumn.HeaderText = "";
            delbuttonColumn.UseColumnTextForButtonValue = true;
            dataGridView.Columns.Add(delbuttonColumn);

        }
        #endregion

        #region ReservationOperation

        public void SortReservation()
        {
            reservations.DefaultView.Sort = "날짜 ASC, 시간 ASC";
            reservations = reservations.DefaultView.ToTable();
            GridRefresh();
        }

        void DeleteReservation(int idx)
        {
            reservations.Rows.RemoveAt(idx);
            SortReservation();
            GridRefresh();
        }

        public void AddReservation(DataRow data)
        {
            reservations.Rows.Add(data);
            SortReservation();
            GridRefresh();
        }

        void FilterByDate(DateTime date)
        {
            
            reservations.DefaultView.RowFilter = "날짜 = #" + date.ToString("yyyy/MM/dd") + "#";
            GridRefresh();
        }

        void SaveReservation()
        {
            CSVUtlity.ToCSV(reservations);
        }

        public void EditReservation(DataRow dr, int idx)
        {
            for(int i = 0; i< reservations.Columns.Count; i++)
            {
                reservations.Rows[idx][i] = dr[i];
            }
        }
        #endregion


        void OnGridButtonClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if(e.ColumnIndex == dataGridView.Columns["삭제"].Index)
            {
                DeleteReservation(e.RowIndex);
            }else if(e.ColumnIndex == dataGridView.Columns["수정"].Index)
            {
                if (editor != null)
                {
                    editor.Close();
                }
                editor = new ReservationEditor(reservations.Rows[e.RowIndex], e.RowIndex);
                editor.Show();
            }
            else
            {
                return;
            }

        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            FilterByDate(e.Start.Date);
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            if(editor != null)
            {
                editor.Close();
            }
            editor = new ReservationEditor();
            editor.Show();
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            SaveReservation();
        }

        void GridRefresh()
        {
            dataGridView.DataSource = reservations;
        }
    }
}
