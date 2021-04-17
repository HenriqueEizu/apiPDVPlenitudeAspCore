using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiPlenitude.Entities
{
    public class Pessoa
    {
        public int IdPessoa { get; set; }
        public string lstTelefone { get; set; }
        public int IdPessoaEndereco { get; set; }
        public Endereco OBJ_ENDERECO { get; set; }
        public string IndFisJur { get; set; }
        public string Nome { get; set; }
        public string Apelido { get; set; }
        public string CnpjCpf { get; set; }
        public string IEstRG { get; set; }
        public string OrgEmisRg { get; set; }
        public DateTime DtEmisRg { get; set; }
        public DateTime DtNascimento { get; set; }
        public string EstCivil { get; set; }
        public string EMail { get; set; }
        public string HomePage { get; set; }
        public string Natural { get; set; }
        public string UsrIns { get; set; }
        public DateTime DhIns { get; set; }
        public string UsrUpd { get; set; }
        public DateTime DhUpd { get; set; }
    }
  
}
