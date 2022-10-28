using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Aspose.Pdf;
using Image = System.Drawing.Image;

namespace DB_TARpv21
{
    public partial class Form1 : Form
    {
        //SqlConnection connect = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\AppData\Tooded_DB.mdf;Integrated Security=True");
        SqlConnection connect = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\marina.oleinik\source\repos\DB_TARpv21\DB_TARpv21\AppData\Tooded_DB.mdf;Integrated Security=True");
        //
        SqlCommand cmd;
        SqlDataAdapter adapter_toode, adapter_kat;
        public Form1()
        {
            InitializeComponent();
            Naita_Andmed();
        }
        
        public void Kustuta_andmed()
        {
            Toode_txt.Text = "";
            Hind_txt.Text = "";
            Kogus_txt.Text = "";
            Kat_cbox.Items.Clear();
            
        }
        private void Lisa_Kat_btn_Click(object sender, EventArgs e)
        {
            
            cmd = new SqlCommand("INSERT INTO Kategooria (Kategooria_nimetus) VALUES (@kat)",connect);
            connect.Open();
            cmd.Parameters.AddWithValue("@kat", Kat_cbox.Text);
            cmd.ExecuteNonQuery();
            connect.Close();
            Kustuta_andmed();
            Naita_Kat();
        }

        public void Naita_Kat()
        {
            connect.Open();
            adapter_kat = new SqlDataAdapter("SELECT Kategooria_nimetus FROM Kategooria", connect);
            DataTable dt_kat = new DataTable();
            adapter_kat.Fill(dt_kat);
            foreach (DataRow nimetus in dt_kat.Rows)
            {
                Kat_cbox.Items.Add(nimetus["Kategooria_nimetus"]);
            }
            connect.Close();
        }

        private void Lisa_btn_Click(object sender, EventArgs e)
        {
            if (Toode_txt.Text.Trim() != string.Empty && Kogus_txt.Text.Trim() != string.Empty && Hind_txt.Text.Trim() != string.Empty && Kat_cbox.SelectedItem != null)
            {
                try
                {
                    cmd = new SqlCommand("INSERT INTO Toodetable (Toodenimetus,Kogus,Hind,Pilt,Kategooria_Id) VALUES (@toode,@kogus,@hind,@pilt,@kat)", connect);
                    connect.Open();
                    cmd.Parameters.AddWithValue("@toode", Toode_txt.Text);
                    cmd.Parameters.AddWithValue("@kogus", Kogus_txt.Text);
                    cmd.Parameters.AddWithValue("@hind", Hind_txt.Text);//format andmebaasis ja vormis võrdsed
                    cmd.Parameters.AddWithValue("@pilt", Toode_txt.Text + ".jpg");//format?
                    cmd.Parameters.AddWithValue("@kat", Kat_cbox.SelectedIndex + 1);//Id andmebaasist võtta
                    cmd.ExecuteNonQuery();
                    connect.Close();
                    Kustuta_andmed();
                    Naita_Andmed();
                }
                catch (Exception)
                {

                    MessageBox.Show("Amdnebaasiga viga!");
                }
            }
            else
            {
                MessageBox.Show("Sisesta andmeid!");
            }
        }
        int Id;
        private void Kustuta_btn_Click(object sender, DataGridViewCellMouseEventArgs e)
        {
            Id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            if (Id != 0)
            {
                cmd = new SqlCommand("DELETE Toodetable WHERE Id=@id", connect);
                connect.Open();
                cmd.Parameters.AddWithValue("@id", Id);
                cmd.ExecuteNonQuery();
                connect.Close();
                Naita_Andmed();
                Kustuta_andmed();
                MessageBox.Show("Andmed tabelist Tooded on kustutatud");
            }
            else
            {
                MessageBox.Show("Viga Tooded tabelist andmete kustutamisega");
            }
        }
        

        private void Kus_Kat_btn_Click(object sender, EventArgs e)
        {

            cmd= new SqlCommand("SELECT Id FROM Kategooria WHERE Kategooria_nimetus=@kat",connect);
            connect.Open();
            cmd.Parameters.AddWithValue("@kat", Kat_cbox.Text);
            cmd.ExecuteNonQuery();
            Id = Convert.ToInt32(cmd.ExecuteScalar());
            connect.Close();
            if (Id != 0)
            {
                cmd = new SqlCommand("DELETE FROM Kategooria WHERE Id=@id", connect);
                connect.Open();
                cmd.Parameters.AddWithValue("@id", Id);
                cmd.ExecuteNonQuery();
                connect.Close();
                Kustuta_andmed();
                Naita_Kat();
                MessageBox.Show("Andmed tabelist Kategooria on kustutatud");
            }
            else
            {
                MessageBox.Show("Viga kustutamisega");
            }
            connect.Close();
        }
        SaveFileDialog save;
        OpenFileDialog open;
        private void Otsi_btn_Click(object sender, EventArgs e)
        {
            open = new OpenFileDialog();
            
            open.InitialDirectory = @"C:\Users\marina.oleinik\Pildid";
            open.Filter = "Image Files(*.jpeg;*.bmp;*.png;*.jpg)|*.jpeg;*.bmp;*.png;*.jpg";
            FileInfo open_info = new FileInfo(@"C:\Users\marina.oleinik\Pildid\"+open.FileName);
            if (open.ShowDialog() == DialogResult.OK && Toode_txt!=null)
            {
                    save = new SaveFileDialog();
                    save.InitialDirectory = Path.GetFullPath(@"..\..\Images");
                    save.FileName = Toode_txt.Text + Path.GetExtension(open.FileName); //".jpg";            
                    save.Filter = "Image Files"+ Path.GetExtension(open.FileName)+"|" + Path.GetExtension(open.FileName);
                    

                if (save.ShowDialog() == DialogResult.OK)
                    {
                    File.Copy(open.FileName, save.FileName);
                    save.RestoreDirectory = true;
                    Toode_pbox.Image = Image.FromFile(save.FileName);
                    }
            }
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Id = (int)dataGridView1.Rows[e.RowIndex].Cells[0].Value;//kui andmed puubivad reas
            Toode_txt.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            Kogus_txt.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            Hind_txt.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            try
            {
                Toode_pbox.Image = Image.FromFile(@"..\..\Images\" + dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString());
            }
            catch (Exception)
            {
                Toode_pbox.Image = Image.FromFile(@"..\..\Images\about.png");
                MessageBox.Show("Fail puudub");
            }
            string v = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
            Kat_cbox.SelectedIndex = Int32.Parse(v) - 1;
        }

        private void Uuenda_btn_Click(object sender, EventArgs e)
        {
            if (Toode_txt.Text != "" && Kogus_txt.Text != "" && Hind_txt.Text != "" && Toode_pbox.Image != null)
            {
                cmd = new SqlCommand("UPDATE Toodetable  SET Toodenimetus=@toode,Kogus=@kogus,Hind=@hind, Pilt=@pilt WHERE Id=@id", connect);
                connect.Open();
                cmd.Parameters.AddWithValue("@id", Id);
                cmd.Parameters.AddWithValue("@toode", Toode_txt.Text);
                cmd.Parameters.AddWithValue("@kogus", Kogus_txt.Text);
                cmd.Parameters.AddWithValue("@hind", Hind_txt.Text.Replace(",", "."));
                string file_pilt = Toode_txt.Text + ".jpg";//kontroll
                cmd.Parameters.AddWithValue("@pilt", file_pilt);
                cmd.ExecuteNonQuery();
                connect.Close();
                Naita_Andmed();
                Kustuta_andmed();
                MessageBox.Show("Andmed uuendatud");
            }
            else
            {
                MessageBox.Show("Viga");
            }
        }
        List<string> Tooded_list = new List<string>();
        private void Arve_btn_Click(object sender, EventArgs e)
        {
            Tooded_list.Add("Toode  Hind  Kogus Summa");
            Tooded_list.Add((Toode_txt.Text+"  "+Hind_txt.Text+"  " +Kogus_txt.Text+"  "+(Convert.ToInt32(Kogus_txt.Text.ToString())* Convert.ToInt32(Hind_txt.Text.ToString()))).ToString());

        }
        Document document;
        private void Ost_btn_Click(object sender, EventArgs e)
        {
            document = new Document();//using Aspose.Pdf
            var page = document.Pages.Add();
            foreach (var toode in Tooded_list)
            {
                page.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment(toode));
            }

            //using (var stream = new MemoryStream())
            //{
            document.Save(@"..\..\Arved\Arve_.pdf");
            document.Dispose();
            //}
        }

        public void N_Arve_btn_Click(object sender, EventArgs e)
        {
            
            System.Diagnostics.Process.Start(@"..\..\Arved\Arve_.pdf");
        }

        public void Naita_Andmed()
        {
            connect.Open();
            DataTable dt_toode=new DataTable();
            adapter_toode=new SqlDataAdapter("SELECT * FROM Toodetable",connect);
            adapter_toode.Fill(dt_toode);
            dataGridView1.DataSource = dt_toode;
           
            Toode_pbox.Image = Image.FromFile("../../Images/about.png");
            connect.Close();
            Naita_Kat();

            
        }

        
    }
}
