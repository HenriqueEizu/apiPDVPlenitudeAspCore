using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiPlenitude.Entities
{
    public class Estoque
    {
        public int Id_Alm { get; set; }
        public int Id_Loja { get; set; }
        public string Descr { get; set; }
        public bool Padrao { get; set; }
        public bool Web_Ve { get; set; }
        public bool Ver_Est { get; set; }
        public bool Bloq_Est { get; set; }
        public bool Empenha { get; set; }
        public bool GeraOp { get; set; }
        public bool LimiProd { get; set; }
        public int PrazoMinimo { get; set; }
    }
}
