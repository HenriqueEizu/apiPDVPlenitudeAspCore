using apiPlenitude.Context;
using apiPlenitude.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;

namespace apiPlenitude.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("origins: http://localhost:4200", headers: "*", methods: "*")]
    public class ClienteController : Controller
    {
        private readonly AppDbContext context;

        public IConfiguration Configuration { get; }

        public ClienteController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [HttpGet]
        public IEnumerable<Cliente> Get()
        {
            //var users = context.Usuario;
            //var users = GetUsuarios();
            return null;
        }
       
        [HttpGet("GetAllClientes")]
        [Route("{campo}/{criterio}")]
        public List<Cliente> GetAllClientes([FromQuery] string campo, [FromQuery] string criterio)
        {
            List<Cliente> lstusuarios = new List<Cliente>();
            Cliente cliente = null;
            Pessoa pessoa = null;
            string strSql = null;
            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = Configuration.GetConnectionString("ConnectioString");
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;


                    strSql = "  SELECT  C.ID_CLI, P.NOME, P.CNPJCPF, P.IESTRG, P.INDFISJUR ";
                    strSql += " FROM	CLIENTE C ";
                    strSql += " JOIN	PESSOA P ON C.IDPESSOA = P.IDPESSOA ";
                    if (campo != "" && campo != null)
                    {
                        if (campo == "C.ID_CLI")
                            strSql += " WHERE   " + campo + " = " + criterio;
                        else
                            strSql += " WHERE   " + campo + " like  '%" + criterio + "%'";
                    }
                    else
                    {
                        strSql += " WHERE   1 = 2 ";
                    }

                    cmd.CommandText = strSql;

                    SqlDataReader rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        cliente = new Cliente();
                        pessoa = new Pessoa();

                        cliente.Id_Cli = Convert.ToInt32(rd["ID_CLI"]);
                        pessoa.CnpjCpf = rd["CNPJCPF"].ToString();
                        pessoa.Nome = rd["NOME"].ToString();
                        pessoa.IEstRG = rd["IESTRG"].ToString();
                        pessoa.IndFisJur = rd["INDFISJUR"].ToString();
                        cliente.OBJ_PESSOA = pessoa;
                        lstusuarios.Add(cliente);
                    }
                    con.Close();
                }
            }

            return lstusuarios;
        }
    }
}
