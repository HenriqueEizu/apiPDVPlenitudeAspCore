using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiPlenitude.Entities
{
    public class Telefone
    {
        public int IdTelefone { get; set; }
        public string CodDdi { get; set; }
        public string CodDdd { get; set; }
        public string Numero { get; set; }
        public string Ramal { get; set; }
        public string IndTipoFone { get; set; }

    }
}
