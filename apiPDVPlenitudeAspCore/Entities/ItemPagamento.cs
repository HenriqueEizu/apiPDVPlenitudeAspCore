using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiPlenitude.Entities
{
    public class ItemPagamento
    {
        public int Id_Item { get; set; }
        public int Id_Frp { get; set; }
        public decimal Num_Parc { get; set; }
        public string FormaPag { get; set; }
        public string Document { get; set; }
        public string Tp_Parc { get; set; }
        public decimal Valor { get; set; }
        public string Plan_Fin { get; set; }
        public decimal Val_Fin { get; set; }
        public string Banco { get; set; }
        public string Agencia { get; set; }
        public string Praca { get; set; }
        public string Num_Cheq { get; set; }
        public decimal Val_Cheq { get; set; }
        public DateTime Dt_Venc { get; set; }
        public decimal Val_ARec { get; set; }
        public decimal ValFrete { get; set; }
    }
}
