using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kinoteatr
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

        }
        //
        private int GetCurrentUserId()
        {
            return 1; // Статический ID пользователя
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                string query = "SELECT ID, Nazvanie, Janr, DataPokaza FROM Films";
                using (var command = new OleDbCommand(query, connection))
                using (var adapter = new OleDbDataAdapter(command))
                {
                    DataTable moviesTable = new DataTable();
                    adapter.Fill(moviesTable);
                    dgvMovies.DataSource = moviesTable;
                }
            }
        }

        private void btnBookTicket_Click(object sender, EventArgs e)
        {

            if (dgvMovies.SelectedRows.Count == 0)
            {
                MessageBox.Show("Пожалуйста, выберите фильм.");
                return;
            }

            if (dgvMovies.SelectedRows.Count > 0)
            {
                int movieId = (int)dgvMovies.SelectedRows[0].Cells["ID"].Value;
                int userId = GetCurrentUserId(); // Реализуйте метод, чтобы получить ID текущего пользователя.

                using (var connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();
                    string query = "INSERT INTO Bronirovaniya (ID_User, ID_Film, DataBroni, KolvoBiletov) " +
                                   "VALUES (?, ?, ?, ?)";
                    using (var command = new OleDbCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID_User", userId);
                        command.Parameters.AddWithValue("@ID_Film", movieId);
                        command.Parameters.AddWithValue("@DataBroni", DateTime.Now.ToString("yyyy-MM-dd"));
                        command.Parameters.AddWithValue("@KolvoBiletov", 1); // Укажите логику для выбора количества билетов.

                        command.ExecuteNonQuery();
                        MessageBox.Show("Бронирование успешно!");
                    }
                }
            }
        }

        private void btnCancelBooking_Click(object sender, EventArgs e)
        {
            if (dgvMovies.SelectedRows.Count > 0)
            {
                int movieId = (int)dgvMovies.SelectedRows[0].Cells["ID"].Value;
                int userId = GetCurrentUserId();

                using (var connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();
                    string query = "DELETE FROM Bronirovaniya WHERE ID_User = ? AND ID_Film = ?";
                    using (var command = new OleDbCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID_User", userId);
                        command.Parameters.AddWithValue("@ID_Film", movieId);

                        command.ExecuteNonQuery();
                        MessageBox.Show("Бронь отменена!");
                    }
                }
            }
        }
    }
}
