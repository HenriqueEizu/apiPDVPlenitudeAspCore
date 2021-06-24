using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiPlenitude.Entities
{
    public class Vendedor
    {
        public int IdVendedor { get; set; }
        public string Nome { get; set; }

        public string Fantasia { get; set; }

        public string SenhaLoja { get; set; }
    }
}