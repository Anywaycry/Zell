using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Zell
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public void f (string sql)
        {
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection("Data Source = ROMAN; Initial Catalog = Zell; Integrated Security = True");
            con.Open();

            SqlDataAdapter adapter_movie = new SqlDataAdapter(sql, con);
            SqlCommandBuilder cb_adapter_movie = new SqlCommandBuilder(adapter_movie);

            adapter_movie.Fill(ds, "Movies"); // прочитать books

            con.Close();

            dataGridView1.DataSource = ds.Tables["Movies"];//выводим books в datagrid
            dataGridView1.Columns["Name"].HeaderText = "Название";
            dataGridView1.Columns["Name"].Width = 200;
            dataGridView1.Columns["Year"].HeaderText = "Год";
            dataGridView1.Columns["Country"].HeaderText = "Страна";
            dataGridView1.Columns["Country"].Width = 150;
            dataGridView1.Columns["Genre"].HeaderText = "Жанр";
            dataGridView1.Columns["Genre"].Width = 150;
            dataGridView1.Columns["Time"].HeaderText = "Время";
            dataGridView1.Columns["Rate"].HeaderText = "Рейтинг";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string sql = "SELECT Name, Year, Country, Genre, Time, Rate FROM MOVIES ORDER BY Name";
            // TODO: данная строка кода позволяет загрузить данные в таблицу "zellDataSet.Movies". При необходимости она может быть перемещена или удалена.
            this.moviesTableAdapter.Fill(this.zellDataSet.Movies);
            f(sql);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Add_Zell addZell = new Add_Zell();
            addZell.Owner = this;
            addZell.StartPosition = FormStartPosition.CenterScreen;
            addZell.ShowDialog();
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            int i = 0;
            int ind2 = dataGridView1.CurrentCell.RowIndex;
            SqlConnection con = new SqlConnection("Data Source = ROMAN; Initial Catalog = Zell; Integrated Security = True");
            con.Open();
            string sql = "SELECT Poster FROM MOVIES WHERE Name = '"+dataGridView1.Rows[ind2].Cells["Name"].Value.ToString()+"'";
            SqlCommand sqlCommand = new SqlCommand(sql, con);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            if (sqlDataReader.HasRows)
            {
                i++;
                MemoryStream memoryStream = new MemoryStream();
                foreach (DbDataRecord record in sqlDataReader)
                    memoryStream.Write((byte[])record["Poster"], 0, ((byte[])record["Poster"]).Length);
                Image image = Image.FromStream(memoryStream);
                image.Save("C:\\Users\\Роман\\Documents\\Visual Studio 2015\\Projects\\Zell\\Zell\\bin\\Debug\\1.BMP");
                memoryStream.Dispose();
                image.Dispose();
            }
            else
            {
                MessageBox.Show("Пустая выборка!");
                con.Close();
            }
            pictureBox1.ImageLocation="C:\\Users\\Роман\\Documents\\Visual Studio 2015\\Projects\\Zell\\Zell\\bin\\Debug\\1.BMP";
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            label5.Text = dataGridView1.Rows[ind2].Cells["Name"].Value.ToString();
            label6.Text = dataGridView1.Rows[ind2].Cells["Year"].Value.ToString();
            label7.Text = dataGridView1.Rows[ind2].Cells["Country"].Value.ToString();
            label8.Text = dataGridView1.Rows[ind2].Cells["Genre"].Value.ToString();
            comboBox1.Text = dataGridView1.Rows[ind2].Cells["Rate"].Value.ToString();
            con.Close();
            sql = "SELECT Discription, Comments FROM MOVIES WHERE Name = '" + dataGridView1.Rows[ind2].Cells["Name"].Value.ToString() + "'";
            SqlCommand sqlCommand1 = new SqlCommand(sql, con);
            con.Open();
            SqlDataReader DataReader = sqlCommand1.ExecuteReader();
            while (DataReader.Read())
            {
                textBox1.Text = DataReader["Discription"] + "";
                textBox2.Text = DataReader["Comments"] + "";
            }
            DataReader.Close();
            con.Close();
        }
    }
}
