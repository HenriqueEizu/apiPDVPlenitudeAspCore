using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiPlenitude.Entities
{
    public class Cliente
    {
       public int IdCliente { get; set; }
       public int? Id_Cli { get; set; }
        public int Id_Cli_Lj { get; set; }
        public int IdPessoa { get; set; }
        public Pessoa OBJ_PESSOA { get; set; }
        public string Contato { get; set; }
        public string ProfEmpresa { get; set; }
        public string ProfCargo { get; set; }
        public string ProfProfissao { get; set; }
        public int ProfSalario { get; set; }
        public DateTime ProfDtAdmissao { get; set; }
        public string BanCodBanco { get; set; }
        public string BanNomeBanco { get; set; }
        public DateTime BanDtInicioBanco { get; set; }
        public string BanAgencia { get; set; }
        public string BanNumConta { get; set; }
        public bool BanChequeEspecial { get; set; }
        public int IdUsuario { get; set; }

    }
}
