using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiPlenitude.Entities
{
    public class RetornoCalculaPedido
    {
        public string id_Ped { get; set; }
        public DateTime entrega { get; set; }
        public decimal saldo { get; set; }
    }
}
