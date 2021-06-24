using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiPlenitude.Entities
{
    public class Pedido
    {

        public int IdPedido	{ get; set; }
        public int Id_Ped { get; set; }
        public int Id_Cli { get; set; }
        public Cliente cliente { get; set; }

        public Vendedor vendedor { get; set; }

        public int Id_Loja { get; set; }
        public Loja loja  { get; set; }
        public int IdMidia { get; set; }
        public string DescMidia { get; set; }

        public Midia midia  { get; set; }

        public bool FlMesmoEndEntrega { get; set; }
          
        public int IdEnderecoEntrega { get; set; }

        public Endereco endereco	 { get; set; }
    
        public int IdFoneEntrega { get; set; }
         
        public Telefone telefone { get; set; }

        public int Situacao { get; set; }

        public string DescrSituacao { get; set; }

        public string Tipo { get; set; }

        public string DescrTipo { get; set; }
        public DateTime DtPed { get; set; }
        public DateTime Entrega { get; set; }
        public DateTime DtReceb { get; set; }
        public string Per_Ent { get; set; }

        public int TotProd { get; set; }
        public decimal Desconto { get; set; }
        public int Desc_Por { get; set; }
        public decimal TotPed { get; set; }
        public decimal VlFrete { get; set; }
        public decimal Val_Afin { get; set; }
        public bool Entregue { get; set; }
        public string Cup_Fisc { get; set; }
        public bool Tem_Frt { get; set; }

        public bool Encerrou { get; set; }

        public bool Enviado { get; set; }
        public string Nome_Cli { get; set; }
        public int Versao { get; set; }
        public string Codpdv { get; set; }
        public bool Impresso { get; set; }

        public bool Env_Mail { get; set; }
        public int Id_PdOri { get; set; }
        public bool Bloq_Est { get; set; }

        public int Prazo_Mp { get; set; }
        public int Desc_Max { get; set; }
        public bool Nf_Pauli { get; set; }
        public bool TemCupom { get; set; }
        public string NumCupom { get; set; }
        public bool EnvCupom { get; set; }

        public string ObsMidia { get; set; }
        public string Observ { get; set; }
        public string Obs_Fin { get; set; }

    }
}
