using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DB_TARpv21
{
    public class Kassa : Form
    {
        SqlConnection connect = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\marina.oleinik\source\repos\DB_TARpv21\DB_TARpv21\AppData\Tooded_DB.mdf;Integrated Security=True");
        //
        SqlCommand cmd;
        SqlDataAdapter adapter_toode, adapter_kat;
        TabControl kategooriad;
        public Kassa()
        {
            this.Size = new System.Drawing.Size(600, 300);
            Kategooriad();
            


        }
        public void Kategooriad()
        {
            kategooriad = new TabControl();
            kategooriad.Name = "Kategooriad";
            kategooriad.Dock = DockStyle.Left;
            kategooriad.Width = this.Width;
            kategooriad.Height = this.Height;

            connect.Open();
            adapter_kat = new SqlDataAdapter("SELECT Kategooria_nimetus FROM Kategooria", connect);
            DataTable dt_kat = new DataTable();
            adapter_kat.Fill(dt_kat);
            ImageList iconsList = new ImageList();
            iconsList.ColorDepth = ColorDepth.Depth32Bit;
            iconsList.ImageSize = new Size(25, 25);

            int i = 0;
            foreach (DataRow nimetus in dt_kat.Rows)
            {
                kategooriad.TabPages.Add((string)nimetus["Kategooria_nimetus"]);
                iconsList.Images.Add(Image.FromFile(@"..\..\Kat_pildid\" + (string)nimetus["Kategooria_nimetus"] + ".jpg"));
                kategooriad.TabPages[i].ImageIndex = i;
                i++;
            }
            kategooriad.ImageList = iconsList;
            connect.Close();
            this.Controls.Add(kategooriad);
        }
        
    }
}
