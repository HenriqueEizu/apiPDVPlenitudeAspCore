using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiPlenitude.Entities
{
    public class ItensPedido
    {
        public int Id_IPv { get; set; }
        public string Produto { get; set; }
        public int Quantid { get; set; }
        public decimal Valuni { get; set; }
        public decimal Valtot { get; set; }
        public string Saida { get; set; }
        public string TipoFrete { get; set; }
    }
}
