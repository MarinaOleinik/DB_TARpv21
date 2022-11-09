using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_TARpv21
{
    public class Toode
    {
        public int Id { get; set; }
        public string Toodenimetus { get; set; }
        public int Kogus { get; set; }
        public float Hind { get; set; }
        public string Pilt { get; set; }
        //public int Kategooria_Id { get; set; }
        //public string Kategoorianimetus { get; set; }
        public IEnumerable<Kategooria> Kategooriad { get; set; }

    }
    public class Kategooria
    {
        public int Id { get; set; }
        public string Kategooria_nimetus { get; set; }
        public string Kirjeldus { get; set; }
    }
}
