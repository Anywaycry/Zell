using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;


namespace Zell
{
    public partial class Add_Zell : Form
    {
        public Add_Zell()
        {
            InitializeComponent();
        }

        public string fileName;
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog OPF = new OpenFileDialog();
            //OPF.Title = "Выберите файл";
            //OPF.Filter = "Изображения|*.jpg, *.bmp, *.png";

            if (OPF.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.ImageLocation = OPF.FileName;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                fileName = OPF.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection("Data Source = ROMAN; Initial Catalog = Zell; Integrated Security = True");
            con.Open();

            SqlCommand command = new SqlCommand("INSERT INTO MOVIES (Name, Year, Country, Genre, Discription, Comments, Time, Rate, Poster, Path) VALUES (@Name, @Year, @Country, @Genre, @Discription, @Comments, @Time, @Rate, @Poster, @Path)", con);
            command.Parameters.Add("@Name", textBox1.Text);
            command.Parameters.Add("@Year", Convert.ToInt32(textBox2.Text));
            command.Parameters.Add("@Country", textBox3.Text);
            command.Parameters.Add("@Genre", comboBox1.Text);
            command.Parameters.Add("@Discription", textBox4.Text);
            command.Parameters.Add("@Comments", textBox5.Text);
            command.Parameters.Add("@Time", "00:00:00");
            command.Parameters.Add("@Rate", Convert.ToInt32(comboBox2.Text));
            command.Parameters.Add("@Path", textBox6.Text);
            SqlParameter sqlParameter = new SqlParameter("Poster", SqlDbType.VarBinary);
            Image image = Image.FromFile(fileName);
            MemoryStream memoryStream = new MemoryStream();
            image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Bmp);
            sqlParameter.Value = memoryStream.ToArray();
            command.Parameters.Add(sqlParameter);
            command.ExecuteNonQuery();
            con.Close();
        }

        private void Add_Zell_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1 form1 = (Form1)this.Owner;
            form1.f("SELECT Name, Year, Country, Genre, Time, Rate FROM MOVIES ORDER BY Name");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog OPF = new OpenFileDialog();

            if (OPF.ShowDialog() == DialogResult.OK)
            {
                textBox6.Text = OPF.FileName;
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            string s = textBox4.Text;
            int sum = 269;
            sum -= s.Length;
            label9.Text = Convert.ToString(sum);
            if (sum == 0)
            {
                MessageBox.Show("Нельзя вводить более 270 символов!");
            }
        }
    }
}
