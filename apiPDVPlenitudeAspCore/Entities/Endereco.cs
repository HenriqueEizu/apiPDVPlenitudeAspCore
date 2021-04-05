using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiPlenitude.Entities
{
    public class Endereco
    {
        public int IdEndereco { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Localidade { get; set; }
        public string CodMuni { get; set; }
        public string CodPais { get; set; }
        public string UF { get; set; }
        public string CEP { get; set; }
        public string PontoReferencia { get; set; }
    }
}
