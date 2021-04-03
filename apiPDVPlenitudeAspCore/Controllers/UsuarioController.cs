using apiPlenitude.Context;
using apiPlenitude.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web.Http.Cors;

namespace apiPlenitude.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("origins: http://localhost:4200", headers: "*", methods: "*")]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext context;

        public IConfiguration Configuration { get; }
        //public UsuarioController(AppDbContext context)
        //{
        //    this.context = context;
        //}

        public UsuarioController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [HttpGet]
        public IEnumerable<Usuarios1> Get()
        {
            //var users = context.Usuario;
            var users = GetUsuarios();
            return users.ToList();
        }

        [HttpPost]
        [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
         public Usuarios1 Post(Usuarios1 userParam)
        {
            var user = AutenticaLogin(userParam.Login, userParam.Senha);
            return user;
            //return null;
        }

        [HttpGet("{id}")]
        public Usuarios1 Get(int id)
        {
            
            var usuario = context.Usuario.FirstOrDefault(p => p.Id_Usr == id);
            return usuario;
        }

        public Usuarios1 AutenticaLogin( string strLogin,  string strSenha)
        {
            Usuarios1 userResult = null;
            using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("ConnectioString")))
            {
                using (SqlCommand cmd = new SqlCommand("Sp_ValidaUsuario", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Login", SqlDbType.VarChar).Value = strLogin;
                    cmd.Parameters.Add("@Senha", SqlDbType.VarChar).Value = strSenha;

                    //Add the output parameter to the command object
                    SqlParameter Id_UsrOut = new SqlParameter();
                    Id_UsrOut.ParameterName = "@Id_Usr";
                    Id_UsrOut.SqlDbType = System.Data.SqlDbType.Int;
                    Id_UsrOut.Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters.Add(Id_UsrOut);

                    SqlParameter UsuarioOut = new SqlParameter();
                    UsuarioOut.ParameterName = "@Usuario";
                    UsuarioOut.SqlDbType = System.Data.SqlDbType.VarChar;
                    UsuarioOut.Size = 30;
                    UsuarioOut.Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters.Add(UsuarioOut);

                    SqlParameter OkOut = new SqlParameter();
                    OkOut.ParameterName = "@Ok";
                    OkOut.SqlDbType = System.Data.SqlDbType.Bit;
                    OkOut.Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters.Add(OkOut);

                    SqlParameter MensErroOut = new SqlParameter();
                    MensErroOut.ParameterName = "@MensErro";
                    MensErroOut.SqlDbType = System.Data.SqlDbType.VarChar;
                    MensErroOut.Size = 200;
                    MensErroOut.Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters.Add(MensErroOut);


                    con.Open();

                    cmd.ExecuteNonQuery();

                    if (Convert.ToBoolean(cmd.Parameters["@Ok"].Value) == true)
                    {
                        userResult = new Usuarios1();
                        userResult.Id_Usr = Convert.ToInt32(cmd.Parameters["@Id_Usr"].Value);
                        userResult.Login = strLogin;
                        userResult.Usuario = cmd.Parameters["@Usuario"].Value.ToString();
                        userResult.FlAtivo = true;
                    }
                    else
                    {
                        userResult = new Usuarios1();
                        userResult.Usuario = cmd.Parameters["@MensErro"].Value.ToString();
                        return userResult;
                    }

                }
            }
            return userResult;
        }

        public List< Usuarios1>GetUsuarios() 
        {
            List<Usuarios1> lstusuarios = new List<Usuarios1>();
            Usuarios1 user1 = null;
            using(SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = Configuration.GetConnectionString("ConnectioString");
                con.Open();
                using(SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "SELECT * FROM USUARIO ";
                    SqlDataReader rd = cmd.ExecuteReader();
                    while(rd.Read())
                    {
                        user1 = new Usuarios1();
                        user1.Id_Usr = Convert.ToInt32(rd["Id_Usr"]);
                        user1.Login = rd["Login"].ToString();
                        user1.Usuario = rd["Usuario"].ToString();
                        user1.Senha = rd["Senha"].ToString();
                        user1.FlAtivo = Convert.ToBoolean(rd["FlAtivo"]);
                        //user1.DtVencSenha = rd["DtVencSenha"] != null ? Convert.ToDateTime(rd["DtVencSenha"]) : null;
                        //user1.IdEMail = rd["IdEMail"] != null ? rd["IdEMail"].ToString() : null;
                        lstusuarios.Add(user1);
                    }
                    con.Close();
                }
            }

            return lstusuarios;
        }

    }


}
