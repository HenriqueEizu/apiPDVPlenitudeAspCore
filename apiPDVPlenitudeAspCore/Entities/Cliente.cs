using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiPlenitude.Entities
{
    public class Cliente
    {
       public int IdCliente { get; set; }
       public int Id_Cli { get; set; }
        public int Id_Cli_Lj { get; set; }
        public int IdPessoa { get; set; }
        public Pessoa OBJ_PESSOA { get; set; }
        public int Contato { get; set; }
        public int ProfEmpresa { get; set; }
        public int ProfCargo { get; set; }
        public int ProfProfissao { get; set; }
        public int ProfSalario { get; set; }
        public int ProfDtAdmissao { get; set; }
        public int BanCodBanco { get; set; }
        public int BanNomeBanco { get; set; }
        public int BanDtInicioBanco { get; set; }
        public int BanAgencia { get; set; }
        public int BanNumConta { get; set; }
        public int BanChequeEspecial { get; set; }
        public int IdUsuario { get; set; }

    }
}
