using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiPlenitude.Entities
{
    public class ItensEstoque
    {
        public int Id_Alp { get; set; }
        public string Produto { get; set; }
      
        public string Quantidade { get; set; }
        public decimal Preco { get; set; }
        public bool Livre { get; set; }
        public int Qtd { get; set; }
        public int QtdPedido { get; set; }
        public int Id_Ped { get; set; }
    }
}
