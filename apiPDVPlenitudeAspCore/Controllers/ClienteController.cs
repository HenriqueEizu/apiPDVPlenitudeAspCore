using apiPlenitude.Context;
using apiPlenitude.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using System.Xml.Linq;
using System.Xml.Serialization;

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
        [Authorize(AuthenticationSchemes = "Bearer")]
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

        [HttpGet("GetClienteCPF")]
        [Route("{id}")]
        [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
        public Cliente GetClienteCPF([FromQuery] string id)
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


                    strSql = "  SELECT  C.ID_CLI, P.NOME, P.CNPJCPF, P.IESTRG, P.INDFISJUR , PE.IdEndereco, PE.IndTipoEndereco ";
                    strSql += " ,PE.IdPessoaEndereco, E.IdEndereco ,E.CEP, E.UF, E.LOCALIDADE,E.LOGRADOURO ";
                    strSql += " ,E.NUMERO, E.COMPLEMENTO, E.BAIRRO ";
                    strSql += " FROM	CLIENTE C ";
                    strSql += " JOIN	PESSOA P ON C.IDPESSOA = P.IDPESSOA ";
                    strSql += " JOIN    PessoaEndereco PE ON PE.IdPessoa = P.IdPessoa ";
                    strSql += " JOIN  Endereco E ON E.IdEndereco = PE.IdEndereco ";
                    strSql += " WHERE   P.CNPJCPF = '" + id + "'";

                    cmd.CommandText = strSql;

                    SqlDataReader rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        cliente = new Cliente();
                        pessoa = new Pessoa();
                        endereco = new Endereco();
                        cliente.Id_Cli = Convert.ToInt32(rd["ID_CLI"]);
                        pessoa.CnpjCpf = rd["CNPJCPF"].ToString();
                        pessoa.IEstRG = rd["IESTRG"].ToString();
                        pessoa.IndFisJur = rd["INDFISJUR"].ToString();
                        pessoa.Nome = rd["NOME"].ToString();
                        pessoa.IdEndereco = Convert.ToInt32(rd["IdEndereco"]);
                        endereco.CEP = rd["CEP"].ToString();
                        endereco.UF = rd["UF"].ToString();
                        endereco.Localidade = rd["LOCALIDADE"].ToString();
                        endereco.Logradouro = rd["LOGRADOURO"].ToString();
                        endereco.Numero = rd["NUMERO"].ToString();
                        endereco.Complemento = rd["COMPLEMENTO"].ToString();
                        endereco.Bairro = rd["BAIRRO"].ToString();
                        pessoa.OBJ_ENDERECO = endereco;
                        cliente.OBJ_PESSOA = pessoa;
                    }
                    con.Close();
                }
            }

            if (cliente == null)
                cliente = new Cliente();

            return cliente;
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


                    strSql = " SELECT top 1 C.IdCliente, P.CNPJCPF, P.IESTRG, P.INDFISJUR , c.Id_Cli ";
                    strSql += " ,p.IdPessoa, p.Nome,p.Apelido,p.OrgEmisRg, p.EstCivil ";
                    strSql += " ,p.DtNascimento, c.ProfProfissao, p.DhIns ";
                    strSql += " ,PE.IdPessoaEndereco, E.IdEndereco ,E.CEP, E.UF, E.LOCALIDADE,E.LOGRADOURO ";
                    strSql += " ,E.NUMERO, E.COMPLEMENTO, E.BAIRRO ";
                    strSql += " ,p.EMail,c.Contato , p.DtEmisRg ";
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
                        cliente.IdCliente = Convert.ToInt32(rd["IdCliente"]);
                        cliente.ProfProfissao = rd["ProfProfissao"].ToString();
                        cliente.Contato = rd["Contato"].ToString();
                        pessoa.CnpjCpf = rd["CNPJCPF"].ToString();
                        pessoa.IEstRG = rd["IESTRG"].ToString();
                        pessoa.IndFisJur = rd["INDFISJUR"].ToString();
                        pessoa.IdPessoa = Convert.ToInt32(rd["IdPessoa"]);
                        pessoa.Nome = rd["Nome"].ToString();
                        pessoa.Apelido = rd["Apelido"].ToString();
                        pessoa.OrgEmisRg = rd["OrgEmisRg"].ToString();
                        pessoa.EstCivil = rd["EstCivil"].ToString();
                        pessoa.DtNascimento = Convert.ToDateTime(rd["DtNascimento"]);
                        pessoa.DtNascimento = DateTime.Now;
                        //pessoa.DtEmisRg = rd["DtEmisRg"] == "" ? DateTime.Now : Convert.ToDateTime(rd["DtEmisRg"]);
                        pessoa.DhIns = Convert.ToDateTime(rd["DhIns"]);
                        pessoa.IdPessoaEndereco = Convert.ToInt32(rd["IdPessoaEndereco"] == null ? 0 : rd["IdPessoaEndereco"]);
                        pessoa.IdEndereco = Convert.ToInt32(rd["IdEndereco"]);
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

                    strSql = " SELECT F.IdTelefone, F.CodDdd as DDD ,F.Numero, F.Ramal, F.IndUsoFone, F.CodDdi, F.Sequencia, ";
                    strSql += " CASE ";
                    strSql += "     WHEN F.IndUsoFone = 'P' THEN 'Uso Pessoal' ";
                    strSql += "     WHEN F.IndUsoFone = 'C' THEN 'Uso Comercial' ";
                    strSql += "     WHEN F.IndUsoFone = 'T' THEN 'Todos' ";
                    strSql += " END AS DescTipoFone ";
                    strSql += " FROM VW_CLIENTEFONE F ";
                    strSql += " WHERE   F.Id_Cli = " + id;
                    strSql += " ORDER BY    F.Sequencia ";

                    cmd.CommandText = strSql;

                    SqlDataReader rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        fone = new Telefone();

                        fone.IdTelefone = Convert.ToInt32(rd["IdTelefone"]); 
                        fone.codddd = (rd["DDD"]).ToString();
                        fone.numero = (rd["Numero"]).ToString();
                        fone.ramal = (rd["Ramal"]).ToString();
                        fone.indusofone = (rd["IndUsoFone"]).ToString();
                        fone.descTipoFone = (rd["DescTipoFone"]).ToString();
                        fone.codddi = (rd["CodDdi"]).ToString();
                        fone.ordenador = Convert.ToInt32(rd["Sequencia"]);

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
        public cpf ValidaCnpjCpf([FromQuery] string id, [FromQuery] string idCliente)
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

                    strSql = "  SELECT  P.CNPJCPF ";
                    strSql += " FROM	CLIENTE C ";
                    strSql += " JOIN	PESSOA P ON C.IDPESSOA = P.IDPESSOA ";
                    strSql += " WHERE   P.CNPJCPF = '" + id + "'";
                    if (idCliente != null)
                        strSql += " AND   C.Id_Cli != " + idCliente;

                    cmd.CommandText = strSql;

                    SqlDataReader rd = cmd.ExecuteReader();
                    if (rd.HasRows)
                    {
                        registro = new cpf();
                        registro.cpfValid = false;
                        registro.cpfNotValid = true;
                        return registro;
                    }

                    con.Close();

                    con.Open();

                    strSql = "select dbo.Fn_ValidaCnpjCpf('" + id + "') as cpfNotValid ";

                    cmd.CommandText = strSql;

                    SqlDataReader rd1 = cmd.ExecuteReader();
                    while (rd1.Read())
                    {
                        registro = new cpf();
                        registro.cpfValid = Convert.ToBoolean(rd1["cpfNotValid"]);
                        registro.cpfNotValid = !Convert.ToBoolean(rd1["cpfNotValid"]);
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

        private string FormatarListaTelefone(string lstTelefone)
        {
            string[] strTelefone = null;
            string strXmlFinal = null;
            Telefone fone = null;
            List<Telefone> lstTelefoneResult = new List<Telefone>();
            strTelefone = lstTelefone.Split("[");
            for (int i = 1; i <= strTelefone.Length - 1; i++ )
            {
                string[] strProp = strTelefone[i].Split(",");
                fone = new Telefone();
                fone.codddd = strProp[0];
                fone.codddi = "55";
                //fone.IdTelefone = Convert.ToInt32(strProp[2]);
                fone.numero = strProp[4];
                fone.ramal = strProp[5];
                fone.indusofone = strProp[3];
                fone.ordenador = i;
                lstTelefoneResult.Add(fone);
            }

            XmlSerializer serializer = new XmlSerializer(typeof(List<Telefone>));

            var stringwriter = new System.IO.StringWriter();
            serializer.Serialize(stringwriter, lstTelefoneResult);

            string[] strLinha = stringwriter.ToString().Split("\n");
            for (int i = 1; i <= strLinha.Length - 1; i++)
            {
                if (strLinha[i].IndexOf("<Telefone>") > -1)
                    strXmlFinal += "<row>";
                if (strLinha[i].IndexOf("</Telefone>") > -1)
                    strXmlFinal += "</row>";

                //if (i > 2 && strLinha[i].IndexOf("<IdTelefone>") == -1 && strLinha[i].IndexOf("<Telefone") == -1 && strLinha[i].IndexOf("</Telefone") == -1 && strLinha[i].IndexOf("</ArrayOfTelefone>") == -1)
                if (i > 2 && strLinha[i].IndexOf("<Telefone") == -1 && strLinha[i].IndexOf("</Telefone") == -1 && strLinha[i].IndexOf("</ArrayOfTelefone>") == -1)
                {
                    strXmlFinal += strLinha[i];
                }
            }

            return strXmlFinal.Replace(@"\r", "");

        }

        [HttpPost("InserirCliente")]
        [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
        public bool InserirCliente(Cliente cliente)
        {
            string strLista = null;
            if (cliente.OBJ_PESSOA.lstTelefone != null && cliente.OBJ_PESSOA.lstTelefone != "")
            {
                strLista = FormatarListaTelefone(cliente.OBJ_PESSOA.lstTelefone).ToString();
            }
            cliente.Id_Cli = null;
            SqlTransaction tran = null;
            using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("ConnectioString")))
            {
                con.Open();
                tran = con.BeginTransaction();
                using (SqlCommand cmd = new SqlCommand("Sp_ClienteGrava", con, tran))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add("@Id_Cli", SqlDbType.Int).Value = cliente.Id_Cli;
                    //cmd.Parameters.Add("@IdPessoa", SqlDbType.Int).Value = cliente.IdPessoa;
                    cmd.Parameters.Add("@IndFisJur", SqlDbType.VarChar).Value = cliente.OBJ_PESSOA.IndFisJur;
                    cmd.Parameters.Add("@Nome", SqlDbType.VarChar).Value = cliente.OBJ_PESSOA.Nome;
                    cmd.Parameters.Add("@Apelido", SqlDbType.VarChar).Value = cliente.OBJ_PESSOA.Apelido;
                    cmd.Parameters.Add("@CnpjCpf", SqlDbType.VarChar).Value = cliente.OBJ_PESSOA.CnpjCpf;
                    cmd.Parameters.Add("@IEstRG", SqlDbType.VarChar).Value = cliente.OBJ_PESSOA.IEstRG;
                    cmd.Parameters.Add("@OrgEmisRg", SqlDbType.VarChar).Value = cliente.OBJ_PESSOA.OrgEmisRg;
                    cmd.Parameters.Add("@DtEmisRg", SqlDbType.DateTime).Value = DateTime.Now;
                    cmd.Parameters.Add("@DtNascimento", SqlDbType.DateTime).Value = cliente.OBJ_PESSOA.DtNascimento;
                    cmd.Parameters.Add("@EstCivil", SqlDbType.VarChar).Value = cliente.OBJ_PESSOA.EstCivil;
                    cmd.Parameters.Add("@EMail", SqlDbType.VarChar).Value = cliente.OBJ_PESSOA.EMail;
                    cmd.Parameters.Add("@cXmlFones", SqlDbType.VarChar).Value = strLista;
                    cmd.Parameters.Add("@Profissao", SqlDbType.VarChar).Value = cliente.ProfProfissao == null ? "" : "";
                    //cmd.Parameters.Add("@Natural", SqlDbType.VarChar).Value = cliente.OBJ_PESSOA.Natural == null ? "" : cliente.OBJ_PESSOA.Natural;

                    cmd.Parameters.Add("@Contato", SqlDbType.VarChar).Value = cliente.Contato == null ? "" : cliente.Contato;
                    cmd.Parameters.Add("@EndLogradouro", SqlDbType.VarChar).Value = cliente.OBJ_PESSOA.OBJ_ENDERECO.Logradouro == null ? "" : cliente.OBJ_PESSOA.OBJ_ENDERECO.Logradouro;
                    cmd.Parameters.Add("@EndNumero", SqlDbType.VarChar).Value = cliente.OBJ_PESSOA.OBJ_ENDERECO.Numero == null ? "" : cliente.OBJ_PESSOA.OBJ_ENDERECO.Numero;
                    cmd.Parameters.Add("@EndComplemento", SqlDbType.VarChar).Value = cliente.OBJ_PESSOA.OBJ_ENDERECO.Complemento == null ? "" : cliente.OBJ_PESSOA.OBJ_ENDERECO.Complemento;
                    cmd.Parameters.Add("@EndBairro", SqlDbType.VarChar).Value = cliente.OBJ_PESSOA.OBJ_ENDERECO.Bairro == null ? "" : cliente.OBJ_PESSOA.OBJ_ENDERECO.Bairro;
                    cmd.Parameters.Add("@EndLocalidade", SqlDbType.VarChar).Value = cliente.OBJ_PESSOA.OBJ_ENDERECO.Localidade == null ? "" : cliente.OBJ_PESSOA.OBJ_ENDERECO.Localidade;
                    cmd.Parameters.Add("@EndCodMuni", SqlDbType.VarChar).Value = cliente.OBJ_PESSOA.OBJ_ENDERECO.CodMuni == null ? "" : cliente.OBJ_PESSOA.OBJ_ENDERECO.CodMuni;
                    cmd.Parameters.Add("@EndUF", SqlDbType.VarChar).Value = cliente.OBJ_PESSOA.OBJ_ENDERECO.UF == null ? "" : cliente.OBJ_PESSOA.OBJ_ENDERECO.UF;
                    cmd.Parameters.Add("@EndCEP", SqlDbType.VarChar).Value = cliente.OBJ_PESSOA.OBJ_ENDERECO.CEP == null ? "" : cliente.OBJ_PESSOA.OBJ_ENDERECO.CEP; 
                    cmd.Parameters.Add("@EndCodPais", SqlDbType.VarChar).Value = cliente.OBJ_PESSOA.OBJ_ENDERECO.CodPais == null ? "" : cliente.OBJ_PESSOA.OBJ_ENDERECO.CodPais;
                    cmd.Parameters.Add("@EndPontoReferencia", SqlDbType.VarChar).Value = cliente.OBJ_PESSOA.OBJ_ENDERECO.PontoReferencia == null ? "" : cliente.OBJ_PESSOA.OBJ_ENDERECO.PontoReferencia;
                    cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = cliente.IdUsuario;

                    SqlParameter Id_CliOut = new SqlParameter();
                    Id_CliOut.ParameterName = "@IdCliente";
                    Id_CliOut.SqlDbType = System.Data.SqlDbType.Int;
                    Id_CliOut.Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters.Add(Id_CliOut);

                    SqlParameter Id_PessoaOut = new SqlParameter();
                    Id_PessoaOut.ParameterName = "@IdPessoa";
                    Id_PessoaOut.SqlDbType = System.Data.SqlDbType.Int;
                    Id_PessoaOut.Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters.Add(Id_PessoaOut);

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


                    try
                    {
                        cmd.ExecuteNonQuery();
                        if (cmd.Parameters["@Ok"].Value.ToString() == "1")
                        {
                            tran.Commit();
                            con.Close();
                            return true;
                        }
                        else
                        {
                            tran.Rollback();
                            throw new InvalidOperationException(cmd.Parameters["@MensErro"].Value.ToString());
                        }

                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException(cmd.Parameters["@MensErro"].Value.ToString());
                    }

                }
            }
        }

        private int ConsultaIdPessoa()
        {
            using (SqlConnection con = new SqlConnection())
            {
                int idPessoResult = 0;
                con.ConnectionString = Configuration.GetConnectionString("ConnectioString");
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;


                    string strSql = "SELECT MAX(IdPessoa) as IdPessoa from CLIENTE ";

                    cmd.CommandText = strSql;

                    SqlDataReader rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        idPessoResult = Convert.ToInt32(rd["IdPessoa"]);
                    }
                    con.Close();
                }
                return idPessoResult;
            }
        }

        [HttpPut("AlterarCliente")]
        [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
        public bool  AlterarCliente(Cliente cliente)
        {
            string strLista = "";
            if (cliente.OBJ_PESSOA.lstTelefone != null && cliente.OBJ_PESSOA.lstTelefone != "")
            {
                strLista = FormatarListaTelefone(cliente.OBJ_PESSOA.lstTelefone).ToString();
            }
            SqlTransaction tran = null;
            using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("ConnectioString")))
            {
                con.Open();
                tran = con.BeginTransaction();
                using (SqlCommand cmd = new SqlCommand("Sp_ClienteGrava", con,tran))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add("@Id_Cli", SqlDbType.Int).Value = cliente.Id_Cli;
                    
                    //cmd.Parameters.Add("@IdPessoa", SqlDbType.Int).Value = cliente.IdPessoa;
                    cmd.Parameters.Add("@IndFisJur", SqlDbType.VarChar).Value = cliente.OBJ_PESSOA.IndFisJur;
                    cmd.Parameters.Add("@Nome", SqlDbType.VarChar).Value = cliente.OBJ_PESSOA.Nome;
                    cmd.Parameters.Add("@Apelido", SqlDbType.VarChar).Value = cliente.OBJ_PESSOA.Apelido;
                    cmd.Parameters.Add("@CnpjCpf", SqlDbType.VarChar).Value = cliente.OBJ_PESSOA.CnpjCpf;
                    cmd.Parameters.Add("@IEstRG", SqlDbType.VarChar).Value = cliente.OBJ_PESSOA.IEstRG;
                    cmd.Parameters.Add("@OrgEmisRg", SqlDbType.VarChar).Value = cliente.OBJ_PESSOA.OrgEmisRg;
                    cmd.Parameters.Add("@DtEmisRg", SqlDbType.DateTime).Value = DateTime.Now;
                    cmd.Parameters.Add("@DtNascimento", SqlDbType.DateTime).Value = cliente.OBJ_PESSOA.DtNascimento;
                    cmd.Parameters.Add("@EstCivil", SqlDbType.VarChar).Value = cliente.OBJ_PESSOA.EstCivil;
                    cmd.Parameters.Add("@EMail", SqlDbType.VarChar).Value = cliente.OBJ_PESSOA.EMail;
                    cmd.Parameters.Add("@cXmlFones", SqlDbType.VarChar).Value = strLista;
                    cmd.Parameters.Add("@Profissao", SqlDbType.VarChar).Value = cliente.ProfProfissao == null ? "" : cliente.ProfProfissao;

                    cmd.Parameters.Add("@Contato", SqlDbType.VarChar).Value = cliente.Contato == null ? "" : cliente.Contato;
                    cmd.Parameters.Add("@EndLogradouro", SqlDbType.VarChar).Value = cliente.OBJ_PESSOA.OBJ_ENDERECO.Logradouro == null ? "" : cliente.OBJ_PESSOA.OBJ_ENDERECO.Logradouro;
                    cmd.Parameters.Add("@EndNumero", SqlDbType.VarChar).Value = cliente.OBJ_PESSOA.OBJ_ENDERECO.Numero == null ? "" : cliente.OBJ_PESSOA.OBJ_ENDERECO.Numero;
                    cmd.Parameters.Add("@EndComplemento", SqlDbType.VarChar).Value = cliente.OBJ_PESSOA.OBJ_ENDERECO.Complemento == null ? "" : cliente.OBJ_PESSOA.OBJ_ENDERECO.Complemento;
                    cmd.Parameters.Add("@EndBairro", SqlDbType.VarChar).Value = cliente.OBJ_PESSOA.OBJ_ENDERECO.Bairro == null ? "" : cliente.OBJ_PESSOA.OBJ_ENDERECO.Bairro;
                    cmd.Parameters.Add("@EndLocalidade", SqlDbType.VarChar).Value = cliente.OBJ_PESSOA.OBJ_ENDERECO.Localidade == null ? "" : cliente.OBJ_PESSOA.OBJ_ENDERECO.Localidade;
                    cmd.Parameters.Add("@EndCodMuni", SqlDbType.VarChar).Value = cliente.OBJ_PESSOA.OBJ_ENDERECO.CodMuni == null ? "" : cliente.OBJ_PESSOA.OBJ_ENDERECO.CodMuni;
                    cmd.Parameters.Add("@EndUF", SqlDbType.VarChar).Value = cliente.OBJ_PESSOA.OBJ_ENDERECO.UF == null ? "" : cliente.OBJ_PESSOA.OBJ_ENDERECO.UF;
                    cmd.Parameters.Add("@EndCEP", SqlDbType.VarChar).Value = cliente.OBJ_PESSOA.OBJ_ENDERECO.CEP == null ? "" : cliente.OBJ_PESSOA.OBJ_ENDERECO.CEP;
                    cmd.Parameters.Add("@EndCodPais", SqlDbType.VarChar).Value = cliente.OBJ_PESSOA.OBJ_ENDERECO.CodPais == null ? "" : cliente.OBJ_PESSOA.OBJ_ENDERECO.CodPais;
                    cmd.Parameters.Add("@EndPontoReferencia", SqlDbType.VarChar).Value = cliente.OBJ_PESSOA.OBJ_ENDERECO.PontoReferencia == null ? "" : cliente.OBJ_PESSOA.OBJ_ENDERECO.PontoReferencia;
                    cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = cliente.IdUsuario;


                    SqlParameter Id_CliOut = new SqlParameter();
                    Id_CliOut.ParameterName = "@IdCliente";
                    Id_CliOut.Value = cliente.IdCliente;
                    Id_CliOut.SqlDbType = System.Data.SqlDbType.Int;
                    Id_CliOut.Direction = System.Data.ParameterDirection.InputOutput;
                    cmd.Parameters.Add(Id_CliOut);

                    SqlParameter Id_PessoaOut = new SqlParameter();
                    Id_PessoaOut.ParameterName = "@IdPessoa";
                    Id_PessoaOut.Value = cliente.IdPessoa;
                    Id_PessoaOut.SqlDbType = System.Data.SqlDbType.Int;
                    Id_PessoaOut.Direction = System.Data.ParameterDirection.InputOutput;
                    cmd.Parameters.Add(Id_PessoaOut);

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

                    

                    try
                    {
                        cmd.ExecuteNonQuery();
                        if (cmd.Parameters["@Ok"].Value.ToString() == "1")
                        {
                            tran.Commit();
                            con.Close();
                            if (AlteraEndereco(cliente))
                            {
                                return true;
                            }
                            else
                            {
                                throw new InvalidOperationException(cmd.Parameters["@MensErro"].Value.ToString());
                            }
                        }
                        else
                        {
                            tran.Rollback();
                            throw new InvalidOperationException(cmd.Parameters["@MensErro"].Value.ToString());
                        }

                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException(cmd.Parameters["@MensErro"].Value.ToString());
                    }

                }
            }
        }


        [HttpDelete("ExcluirCliente")]
        [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
        public bool ExcluirCliente(string idCliente)
        {
            SqlTransaction tran = null;
            using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("ConnectioString")))
            {
                con.Open();
                tran = con.BeginTransaction();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM ", con, tran))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                        if (cmd.Parameters["@Ok"].Value.ToString() == "1")
                        {
                            tran.Commit();
                            con.Close();
                            return true;
                        }
                        else
                        {
                            tran.Rollback();
                            throw new InvalidOperationException(cmd.Parameters["@MensErro"].Value.ToString());
                        }

                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException(cmd.Parameters["@MensErro"].Value.ToString());
                    }

                }
            }
        }

        bool AlteraEndereco(Cliente cli)
        {
            using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("ConnectioString")))
            {
                using (SqlCommand cmd = new SqlCommand("Sp_PessoaEnderecoGrava", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    //cmd.Parameters.Add("@IdPessoaEndereco", SqlDbType.Int).Value = cli.OBJ_PESSOA.IdPessoaEndereco;
                    cmd.Parameters.Add("@IdPessoa", SqlDbType.Int).Value = cli.OBJ_PESSOA.IdPessoa;
                    cmd.Parameters.Add("@IdEndereco", SqlDbType.Int).Value = cli.OBJ_PESSOA.IdEndereco == null ? 0 : cli.OBJ_PESSOA.IdEndereco;
                    cmd.Parameters.Add("@IndTipoEndereco", SqlDbType.VarChar).Value = cli.OBJ_PESSOA.OBJ_ENDERECO.IndTipoEndereco == null ? "CAD" : cli.OBJ_PESSOA.OBJ_ENDERECO.IndTipoEndereco;
                    cmd.Parameters.Add("@Logradouro", SqlDbType.VarChar).Value = cli.OBJ_PESSOA.OBJ_ENDERECO.Logradouro == null ? "" : cli.OBJ_PESSOA.OBJ_ENDERECO.Logradouro; 
                    cmd.Parameters.Add("@Numero", SqlDbType.VarChar).Value = cli.OBJ_PESSOA.OBJ_ENDERECO.Numero == null ? "" : cli.OBJ_PESSOA.OBJ_ENDERECO.Numero; 
                    cmd.Parameters.Add("@Complemento", SqlDbType.VarChar).Value = cli.OBJ_PESSOA.OBJ_ENDERECO.Complemento == null ? "" : cli.OBJ_PESSOA.OBJ_ENDERECO.Complemento;
                    cmd.Parameters.Add("@Bairro", SqlDbType.VarChar).Value = cli.OBJ_PESSOA.OBJ_ENDERECO.Bairro == null ? "" : cli.OBJ_PESSOA.OBJ_ENDERECO.Bairro;
                    cmd.Parameters.Add("@Localidade", SqlDbType.VarChar).Value = cli.OBJ_PESSOA.OBJ_ENDERECO.Localidade == null ? "" : cli.OBJ_PESSOA.OBJ_ENDERECO.Localidade;
                    cmd.Parameters.Add("@CodMuni", SqlDbType.VarChar).Value = cli.OBJ_PESSOA.OBJ_ENDERECO.CodMuni == null ? "" : cli.OBJ_PESSOA.OBJ_ENDERECO.CodMuni;
                    cmd.Parameters.Add("@UF", SqlDbType.VarChar).Value = cli.OBJ_PESSOA.OBJ_ENDERECO.UF == null ? "" : cli.OBJ_PESSOA.OBJ_ENDERECO.UF;
                    cmd.Parameters.Add("@CEP", SqlDbType.VarChar).Value = cli.OBJ_PESSOA.OBJ_ENDERECO.CEP == null ? "" : cli.OBJ_PESSOA.OBJ_ENDERECO.CEP;
                    cmd.Parameters.Add("@CodPais", SqlDbType.VarChar).Value = cli.OBJ_PESSOA.OBJ_ENDERECO.CodPais == null ? "" : cli.OBJ_PESSOA.OBJ_ENDERECO.CodPais;
                    cmd.Parameters.Add("@PontoReferencia", SqlDbType.VarChar).Value = cli.OBJ_PESSOA.OBJ_ENDERECO.PontoReferencia == null ? "" : cli.OBJ_PESSOA.OBJ_ENDERECO.PontoReferencia;

                    

                    SqlParameter IdPessoaEnderecoOut = new SqlParameter();
                    IdPessoaEnderecoOut.ParameterName = "@IdPessoaEndereco";
                    IdPessoaEnderecoOut.SqlDbType = System.Data.SqlDbType.Int;
                    IdPessoaEnderecoOut.Value = cli.OBJ_PESSOA.IdPessoaEndereco;
                    IdPessoaEnderecoOut.Direction = System.Data.ParameterDirection.InputOutput;
                    cmd.Parameters.Add(IdPessoaEnderecoOut);

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


                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        if (cmd.Parameters["@Ok"].Value.ToString() == "1")
                        {
                            return true;
                        }
                        else
                        {
                            throw new InvalidOperationException(cmd.Parameters["@MensErro"].Value.ToString());
                        }
                            
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }

                }
            }
        }
    }
}
