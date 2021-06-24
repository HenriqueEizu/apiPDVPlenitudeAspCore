using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiPlenitude.Entities
{
    public class FormaPagamento
    {
        public int Id_Frp { get; set; }
        public int Id_Ped { get; set; }
        public int Id_Forma { get; set; }
        public int Id_Plano { get; set; }
        public string FormaPag { get; set; }
        public DateTime? DtLanc { get; set; }
        public string Tp_Pagto { get; set; }
        public int Tot_Parc { get; set; }
        public decimal ValDescFin { get; set; }
        public decimal Val_Fin { get; set; }
        public decimal Val_Entr { get; set; }
        public decimal Val_ARec { get; set; }
        public string Cont_Fin { get; set; }
        public DateTime? Dt_ContF { get; set; }
        public string Aut_Cart { get; set; }
        public bool Tem_Fret { get; set; }
        public DateTime? Dt_Venc { get; set; }
        public DateTime? Dt_Baixa { get; set; }
        public bool FlCancelado { get; set; }
        public DateTime? DhIns { get; set; }

        public int NumMinParcelas { get; set; }

        public int NumMaxParcelas { get; set; }


        public string UsrIns { get; set; }

        public int? id_Usr { get; set; }
    }
}
