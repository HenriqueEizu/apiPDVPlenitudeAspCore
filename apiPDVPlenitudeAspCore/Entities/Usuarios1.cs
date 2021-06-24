using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace apiPlenitude.Entities
{
    public class Usuarios1
    {
        [Key]
        public int Id_Usr { get; set; }
        public string Login { get; set; }
        
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public bool FlAtivo { get; set; }
        public DateTime? DtVencSenha { get; set; }

        public string? IdEMail { get; set; }

        public string Token { get; set; }
    }
}
