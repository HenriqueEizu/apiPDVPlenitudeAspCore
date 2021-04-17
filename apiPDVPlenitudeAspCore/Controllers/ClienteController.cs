using apiPlenitude.Context;
using apiPlenitude.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
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


        [HttpGet("GetIdCliente")]
        [Route("{id}")]
        [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
        public Cliente GetIdCliente([FromQuery] string id )
        {
            List<Cliente> lstusuarios = new List<Cliente>();
            Cliente cliente = null;
            Pessoa pessoa = null;
            Endereco endereco = null;
            string strSql = null;
            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = Configuration.GetConnectionString("ConnectioString");
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;


                    strSql = " SELECT top 1 P.CNPJCPF, P.IESTRG, P.INDFISJUR , c.Id_Cli ";
                    strSql += " ,p.IdPessoa, p.Nome,p.Apelido,p.OrgEmisRg, p.EstCivil ";
                    strSql += " ,p.DtNascimento, c.ProfProfissao, p.DhIns ";
                    strSql += " ,PE.IdPessoaEndereco, E.IdEndereco ,E.CEP, E.UF, E.LOCALIDADE,E.LOGRADOURO ";
                    strSql += " ,E.NUMERO, E.COMPLEMENTO, E.BAIRRO ";
                    strSql += " ,p.EMail,c.Contato ";
                    // strSql += " ,f.CodDdd ,f.Numero, f.Ramal "
                    strSql += " FROM	CLIENTE C ";
                    strSql += " JOIN	PESSOA P ON C.IDPESSOA = P.IDPESSOA ";
                    strSql += " JOIN  PessoaEndereco PE ON PE.IdPessoa = P.IdPessoa ";
                    strSql += " JOIN  Endereco E ON E.IdEndereco = PE.IdEndereco ";
                    // strSql += " JOIN	PessoaTelefone T ON P.IdPessoa = T.IdPessoa "
                    // strSql += " JOIN	Telefone F ON F.IdTelefone = T.IdTelefone "
                    strSql += " WHERE   c.Id_Cli = " + id;

                    cmd.CommandText = strSql;

                    SqlDataReader rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        cliente = new Cliente();
                        pessoa = new Pessoa();
                        endereco = new Endereco();

                        cliente.Id_Cli = Convert.ToInt32(rd["Id_Cli"]);
                        cliente.ProfProfissao = rd["ProfProfissao"].ToString();
                        cliente.Contato = rd["ProfProfissao"].ToString();
                        pessoa.CnpjCpf = rd["CNPJCPF"].ToString();
                        pessoa.IEstRG = rd["IESTRG"].ToString();
                        pessoa.IndFisJur = rd["INDFISJUR"].ToString();
                        pessoa.IdPessoa = Convert.ToInt32(rd["IdPessoa"]);
                        pessoa.Nome = rd["Nome"].ToString();
                        pessoa.Apelido = rd["Apelido"].ToString();
                        pessoa.OrgEmisRg = rd["OrgEmisRg"].ToString();
                        pessoa.EstCivil = rd["EstCivil"].ToString();
                        pessoa.DtNascimento = Convert.ToDateTime(rd["DtNascimento"]);
                        pessoa.DhIns = Convert.ToDateTime(rd["DhIns"]);
                        pessoa.IdPessoaEndereco = Convert.ToInt32(rd["IdPessoaEndereco"]);
                        //pessoa.IdEndereco = rd["CNPJCPF"].ToString();
                        pessoa.EMail = rd["EMail"].ToString();
                        endereco.CEP = rd["CEP"].ToString();
                        endereco.UF = rd["UF"].ToString();
                        endereco.Localidade  = rd["LOCALIDADE"].ToString();
                        endereco.Logradouro = rd["LOGRADOURO"].ToString();
                        endereco.Numero = rd["NUMERO"].ToString();
                        endereco.Complemento = rd["COMPLEMENTO"].ToString();
                        endereco.Bairro = rd["BAIRRO"].ToString();
                        pessoa.OBJ_ENDERECO = endereco;
                        cliente.OBJ_PESSOA = pessoa;
                        lstusuarios.Add(cliente);
                    }
                    con.Close();
                }
            }

            return cliente;
        }


        [HttpGet("GetTelefonePorIdCliente")]
        [Route("{id}")]
        [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
        public List<Telefone> GetTelefonePorIdCliente ([FromQuery] string id)
        {
            if (id == "undefined")
                return null;

            List<Telefone> lstTelefone = new List<Telefone>();
            Telefone fone = null;
            string strSql = null;
            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = Configuration.GetConnectionString("ConnectioString");
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;


                    strSql = " SELECT  F.IdTelefone, F.CodDdd as DDD ,F.Numero, F.Ramal, F.IndTipoFone";
                    strSql += " FROM	CLIENTE C ";
                    strSql += " JOIN	PESSOA P ON C.IDPESSOA = P.IDPESSOA ";
                    strSql += " JOIN    PessoaEndereco PE ON PE.IdPessoa = P.IdPessoa ";
                    strSql += " JOIN    Endereco E ON E.IdEndereco = PE.IdEndereco ";
                    strSql += " JOIN	PessoaTelefone T ON P.IdPessoa = T.IdPessoa ";
                    strSql += " JOIN	Telefone F ON F.IdTelefone = T.IdTelefone ";
                    strSql += " WHERE   c.Id_Cli = " + id;

                    cmd.CommandText = strSql;

                    SqlDataReader rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        fone = new Telefone();

                        fone.IdTelefone = Convert.ToInt32(rd["IdTelefone"]); 
                        fone.CodDdd = (rd["DDD"]).ToString();
                        fone.Numero = (rd["Numero"]).ToString();
                        fone.Ramal = (rd["Ramal"]).ToString();
                        fone.IndTipoFone = (rd["IndTipoFone"]).ToString();

                        lstTelefone.Add(fone);
                    }
                    con.Close();
                }
            }

            return lstTelefone;
        }

        [HttpGet("ConsultaCEP")]
        [Route("{id}")]
        [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
        public Endereco ConsultaCEP([FromQuery] string id)
        {
                if (id == "undefined")
                return null;

            Endereco endereco = null;
            string strSql = null;
            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = Configuration.GetConnectionString("ConnectioString");
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;


                    strSql = "exec Sp_CepDados '" +  id + "'";

                    cmd.CommandText = strSql;

                    SqlDataReader rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        endereco = new Endereco();
                        endereco.UF = rd["UF"].ToString();
                        endereco.Localidade = rd["LOCALIDADE"].ToString();
                        endereco.Logradouro = rd["LOGRADOURO"].ToString();
                        endereco.Bairro = rd["BAIRRO"].ToString();
                    }
                    con.Close();
                }
            }

            return endereco;
        }

        [HttpGet("ValidaCnpjCpf")]
        [Route("{id}")]
        [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
        public cpf ValidaCnpjCpf([FromQuery] string id)
        {
            if (id == "undefined")
                return null;

            cpf registro = null;
            string strSql = null;
            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = Configuration.GetConnectionString("ConnectioString");
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;


                    strSql = "select dbo.Fn_ValidaCnpjCpf('" + id + "') as cpfNotValid ";

                    cmd.CommandText = strSql;

                    SqlDataReader rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        registro = new cpf();
                        registro.cpfValid = Convert.ToBoolean(rd["cpfNotValid"]);
                        registro.cpfNotValid = !Convert.ToBoolean(rd["cpfNotValid"]);
                    }
                    con.Close();
                }
            }

            return registro;
        }

        




        [HttpGet("GetAllEstados")]
        [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
        public List<Estado> GetAllEstados()
        {
            List<Estado> lstestados = new List<Estado>();
            Estado estado = null;
            string strSql = null;
            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = Configuration.GetConnectionString("ConnectioString");
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;


                    strSql = "  select UF, Estado from tab_uf";

                    cmd.CommandText = strSql;

                    SqlDataReader rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        estado = new Estado();

                        estado.uf = (rd["UF"]).ToString();
                        estado.estado = (rd["Estado"]).ToString();

                        lstestados.Add(estado);
                    }
                    con.Close();
                }
            }

            return lstestados;
        }

        [HttpPut("AlterarCliente")]
        [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
        public bool  AlterarCliente(Cliente cliente)
        {
            SqlTransaction tran = null;
            using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("ConnectioString")))
            {
                tran = con.BeginTransaction();
                using (SqlCommand cmd = new SqlCommand("Sp_ClienteGrava", con,tran))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Id_Cli", SqlDbType.Int).Value = cliente.Id_Cli;
                    cmd.Parameters.Add("@IdPessoa", SqlDbType.Int).Value = cliente.IdPessoa;
                    cmd.Parameters.Add("@IndFisJur", SqlDbType.VarChar).Value = cliente.OBJ_PESSOA.IndFisJur;
                    cmd.Parameters.Add("@Nome", SqlDbType.VarChar).Value = cliente.OBJ_PESSOA.Nome;
                    cmd.Parameters.Add("@Apelido", SqlDbType.VarChar).Value = cliente.OBJ_PESSOA.Apelido;
                    cmd.Parameters.Add("@CnpjCpf", SqlDbType.VarChar).Value = cliente.OBJ_PESSOA.CnpjCpf;
                    cmd.Parameters.Add("@IEstRG", SqlDbType.VarChar).Value = cliente.OBJ_PESSOA.IEstRG;
                    cmd.Parameters.Add("@OrgEmisRg", SqlDbType.VarChar).Value = cliente.OBJ_PESSOA.OrgEmisRg;
                    cmd.Parameters.Add("@DtEmisRg", SqlDbType.DateTime).Value = cliente.OBJ_PESSOA.DtEmisRg;
                    cmd.Parameters.Add("@DtNascimento", SqlDbType.DateTime).Value = cliente.OBJ_PESSOA.DtNascimento;
                    cmd.Parameters.Add("@EstCivil", SqlDbType.VarChar).Value = cliente.OBJ_PESSOA.EstCivil;
                    cmd.Parameters.Add("@EMail", SqlDbType.VarChar).Value = cliente.OBJ_PESSOA.EMail;
                    cmd.Parameters.Add("@HomePage", SqlDbType.VarChar).Value = cliente.OBJ_PESSOA.HomePage;
                    cmd.Parameters.Add("@Natural", SqlDbType.VarChar).Value = cliente.OBJ_PESSOA.Natural;

                    cmd.Parameters.Add("@Contato", SqlDbType.VarChar).Value = cliente.Contato;
                    cmd.Parameters.Add("@ProfEmpresa", SqlDbType.VarChar).Value = cliente.ProfEmpresa;
                    cmd.Parameters.Add("@ProfCargo", SqlDbType.VarChar).Value = cliente.ProfCargo;
                    cmd.Parameters.Add("@ProfProfissao", SqlDbType.VarChar).Value = cliente.ProfProfissao;
                    cmd.Parameters.Add("@ProfSalario", SqlDbType.Int).Value = cliente.ProfSalario;
                    cmd.Parameters.Add("@ProfDtAdmissao", SqlDbType.DateTime).Value = cliente.ProfDtAdmissao;
                    cmd.Parameters.Add("@BanCodBanco", SqlDbType.VarChar).Value = cliente.BanCodBanco;
                    cmd.Parameters.Add("@BanNomeBanco", SqlDbType.VarChar).Value = cliente.BanNomeBanco;
                    cmd.Parameters.Add("@BanDtInicioBanco", SqlDbType.DateTime).Value = cliente.BanDtInicioBanco;
                    cmd.Parameters.Add("@BanAgencia", SqlDbType.VarChar).Value = cliente.BanAgencia;
                    cmd.Parameters.Add("@BanNumConta", SqlDbType.VarChar).Value = cliente.BanNumConta;
                    cmd.Parameters.Add("@BanChequeEspecial", SqlDbType.Bit).Value = cliente.BanChequeEspecial;
                    cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = cliente.IdUsuario;


                    //Add the output parameter to the command object
                    SqlParameter Id_OkOut = new SqlParameter();
                    Id_OkOut.ParameterName = "@Ok";
                    Id_OkOut.SqlDbType = System.Data.SqlDbType.VarChar;
                    Id_OkOut.Size = 30;
                    Id_OkOut.Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters.Add(Id_OkOut);

                    SqlParameter MensErroOut = new SqlParameter();
                    MensErroOut.ParameterName = "@MensErro"; 
                    MensErroOut.SqlDbType = System.Data.SqlDbType.VarChar;
                    MensErroOut.Size = 500;
                    MensErroOut.Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters.Add(MensErroOut);

                    con.Open();

                    try
                    {
                        cmd.ExecuteNonQuery();
                        if (Convert.ToBoolean(cmd.Parameters["@Ok"].Value) == true)
                        {
                            if (AlteraEndereco(con, cliente))
                            {
                                tran.Commit();
                                return true;
                            }
                            else
                            {
                                tran.Rollback();
                                return false;
                            }
                        }
                        else
                        {
                            tran.Rollback();
                            return false;
                        }

                    }
                    catch (Exception)
                    {
                        tran.Rollback();
                        return false;
                    }

                }
            }
        }

        bool AlteraEndereco(SqlConnection con, Cliente cli)
        {
            using (SqlCommand cmd = new SqlCommand("Sp_PessoaEnderecoGrava", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@IdPessoaEndereco", SqlDbType.Int).Value = cli.OBJ_PESSOA.IdPessoaEndereco;
                cmd.Parameters.Add("@IdPessoa", SqlDbType.Int).Value = cli.OBJ_PESSOA.IdPessoa;
                cmd.Parameters.Add("@IdEndereco", SqlDbType.Int).Value = cli.OBJ_PESSOA.OBJ_ENDERECO.IdEndereco;
                cmd.Parameters.Add("@IndTipoEndereco", SqlDbType.VarChar).Value = cli.OBJ_PESSOA.OBJ_ENDERECO.IndTipoEndereco;
                cmd.Parameters.Add("@Logradouro", SqlDbType.VarChar).Value = cli.OBJ_PESSOA.OBJ_ENDERECO.Logradouro;
                cmd.Parameters.Add("@Numero", SqlDbType.VarChar).Value = cli.OBJ_PESSOA.OBJ_ENDERECO.Numero;
                cmd.Parameters.Add("@Complemento", SqlDbType.VarChar).Value = cli.OBJ_PESSOA.OBJ_ENDERECO.Complemento;
                cmd.Parameters.Add("@Bairro", SqlDbType.VarChar).Value = cli.OBJ_PESSOA.OBJ_ENDERECO.Bairro;
                cmd.Parameters.Add("@Localidade", SqlDbType.VarChar).Value = cli.OBJ_PESSOA.OBJ_ENDERECO.Localidade;
                cmd.Parameters.Add("@CodMuni", SqlDbType.VarChar).Value = cli.OBJ_PESSOA.OBJ_ENDERECO.CodMuni;
                cmd.Parameters.Add("@UF", SqlDbType.VarChar).Value = cli.OBJ_PESSOA.OBJ_ENDERECO.UF;
                cmd.Parameters.Add("@CEP", SqlDbType.VarChar).Value = cli.OBJ_PESSOA.OBJ_ENDERECO.CEP;
                cmd.Parameters.Add("@CodPais", SqlDbType.VarChar).Value = cli.OBJ_PESSOA.OBJ_ENDERECO.CodPais;
                cmd.Parameters.Add("@PontoReferencia", SqlDbType.VarChar).Value = cli.OBJ_PESSOA.OBJ_ENDERECO.PontoReferencia;

                //Add the output parameter to the command object
                SqlParameter Id_OkOut = new SqlParameter();
                Id_OkOut.ParameterName = "@Ok";
                Id_OkOut.SqlDbType = System.Data.SqlDbType.VarChar;
                Id_OkOut.Size = 30;
                Id_OkOut.Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add(Id_OkOut);

                SqlParameter MensErroOut = new SqlParameter();
                MensErroOut.ParameterName = "@MensErro";
                MensErroOut.SqlDbType = System.Data.SqlDbType.VarChar;
                MensErroOut.Size = 500;
                MensErroOut.Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add(MensErroOut);

                con.Open();

                try
                {
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }

            }
        }
    }
}
