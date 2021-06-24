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
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace apiPlenitude.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("origins: http://localhost:4200", headers: "*", methods: "*")]
    public class PedidoController : Controller
    {
        public IConfiguration Configuration { get; }

        public PedidoController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [HttpGet("GetAllPedidos")]
        [Route("{campo}/{criterio}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public List<Pedido> GetAllPedidos([FromQuery] string campo, [FromQuery] string criterio)
        {
            List<Pedido> lstPedidos = new List<Pedido>();
            Pedido pedido = null;
            Cliente cli = null;
            Pessoa pess = null;
            string strSql = null;

            if (campo == null)
                return lstPedidos;

            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = Configuration.GetConnectionString("ConnectioString");
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;

                    strSql = "SELECT  DISTINCT PD.Id_Ped, P.Nome,PD.DtPed, PD.Entrega, PD.Situacao, PD.DescrSituacao, PD.Tipo, PD.DescrTipo ";
                    strSql += " FROM    CapaVend PD ";
                    strSql += " JOIN    CLIENTE C ON C.Id_Cli = PD.Id_Cli ";
                    strSql += " JOIN	PESSOA P ON C.IDPESSOA = P.IDPESSOA ";
                    if (campo != "")
                    {
                        if (campo == "PD.Id_Ped")
                            strSql += " WHERE   " + campo + " = " + criterio;
                        else
                            strSql += " WHERE   " + campo + " like  '%" + criterio + "%'";
                    }

                    else
                    {
                        strSql += " WHERE   1 = 2 ";
                    }

                    cmd.CommandText = strSql;

                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        pedido = new Pedido();
                        cli = new Cliente();
                        pess = new Pessoa();
                        pess.Nome = dr["Nome"].ToString();
                        cli.OBJ_PESSOA = pess;
                        pedido.cliente = cli;
                        pedido.Id_Ped = Convert.ToInt32(dr["Id_Ped"]);
                        pedido.DtPed = Convert.ToDateTime(dr["DtPed"]);
                        pedido.Entrega = Convert.ToDateTime(dr["Entrega"]);
                        pedido.Situacao = Convert.ToInt32(dr["Situacao"].ToString());
                        pedido.DescrSituacao = dr["DescrSituacao"].ToString();
                        pedido.Tipo = dr["Tipo"].ToString();
                        pedido.DescrTipo = dr["DescrTipo"].ToString();
                        lstPedidos.Add(pedido);
                    }

                    con.Close();
                }
            }

            return lstPedidos;
        }

        [HttpGet("GetAllVendedores")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public List<Vendedor> GetAllVendedores()
        {
            List<Vendedor> lstVendedores = new List<Vendedor>();
            Vendedor vendedor = null;
            string strSql = null;

            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = Configuration.GetConnectionString("ConnectioString");
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;

                    strSql = "SELECT DISTINCT IDVENDEDOR, NOME , FANTASIA, SENHALOJA FROM Vendedor ORDER BY NOME  ";

                    cmd.CommandText = strSql;

                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        vendedor = new Vendedor();
                        vendedor.IdVendedor = Convert.ToInt32(dr["IDVENDEDOR"]);
                        vendedor.Nome = dr["NOME"].ToString();
                        vendedor.Fantasia = dr["FANTASIA"].ToString();
                        vendedor.SenhaLoja = dr["SENHALOJA"].ToString();
                        lstVendedores.Add(vendedor);
                    }

                    con.Close();
                }
            }

            return lstVendedores;
        }

        [HttpGet("GetAllMidia")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public List<Midia> GetAllMidia()
        {
            List<Midia> lstMidia = new List<Midia>();
            Midia midia = null;
            string strSql = null;

            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = Configuration.GetConnectionString("ConnectioString");
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;

                    strSql = "  SELECT  M.IdMidia, M.Descr ";
                    strSql += " FROM    Midia M ";

                    cmd.CommandText = strSql;

                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        midia = new Midia();
                        midia.IdMidia = Convert.ToInt32(dr["IdMidia"]);
                        midia.DescMidia = dr["Descr"].ToString();
                        lstMidia.Add(midia);
                    }

                    con.Close();
                }
            }

            return lstMidia;
        }


        [HttpGet("GetAllFormaPagamentoDominio")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public List<FormaPagamento> GetAllFormaPagamentoDominio()
        {
            List<FormaPagamento> lstFormaPagto = new List<FormaPagamento>();
            FormaPagamento FormaPagto = null;
            string strSql = null;

            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = Configuration.GetConnectionString("ConnectioString");
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;

                    strSql = "  SELECT  Id_forma, Descr, Tipo, NumMinParcelas, NumMaxParcelas ";
                    strSql += " FROM    fn_formasPagvenda() ";
                    strSql += " WHERE   tipo <> 'F' ";

                    cmd.CommandText = strSql;

                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        FormaPagto = new FormaPagamento();
                        FormaPagto.Id_Forma = Convert.ToInt32(dr["Id_forma"]);
                        FormaPagto.FormaPag = dr["Descr"].ToString();
                        FormaPagto.Tp_Pagto = dr["Tipo"].ToString();
                        FormaPagto.NumMinParcelas = Convert.ToInt32(dr["NumMinParcelas"]);
                        FormaPagto.NumMaxParcelas = Convert.ToInt32(dr["NumMaxParcelas"]);
                        lstFormaPagto.Add(FormaPagto);
                    }

                    con.Close();
                }
            }

            return lstFormaPagto;
        }


        [HttpGet("GetAllEstoques")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public List<Estoque> GetAllEstoques()
        {
            List<Estoque> lstEstoque = new List<Estoque>();
            Estoque estoque = null;
            string strSql = null;

            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = Configuration.GetConnectionString("ConnectioString");
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;

                    strSql = "  SELECT  E.Id_Alm, E.Descr ";
                    strSql += " FROM    Almoxar E ";
                    strSql += " WHERE   GeraOp = 0 order by Id_Alm ";

                    cmd.CommandText = strSql;

                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        estoque = new Estoque();
                        estoque.Id_Alm = Convert.ToInt32(dr["Id_Alm"]);
                        estoque.Descr = dr["Descr"].ToString();
                        lstEstoque.Add(estoque);
                    }

                    con.Close();
                }
            }

            return lstEstoque;
        }


        [HttpGet("GetItensEstoque")]
        [Route("{estoque}/{palavraChave}/{nPedido}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public List<ItensEstoque> GetItensEstoque([FromQuery] string estoque, [FromQuery] string palavraChave, [FromQuery] string nPedido)
        {
            ItensEstoque Iestoque = new ItensEstoque();
            List<ItensEstoque> lstItensEstoque = new List<ItensEstoque>();
            if (estoque == null)
                return lstItensEstoque;

            string strSql = null;
            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = Configuration.GetConnectionString("ConnectioString");
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;


                    strSql = "  Sp_LojasPesquisaEstoque  " + estoque + ", '" + palavraChave + "' ";

                    cmd.CommandText = strSql;

                    SqlDataReader rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        Iestoque = new ItensEstoque();
                        Iestoque.Id_Alp = Convert.ToInt32(rd["Id_Alp"]);
                        Iestoque.Produto = rd["Produto"].ToString();
                        Iestoque.Quantidade = rd["Quantidade"].ToString();
                        Iestoque.Preco = Convert.ToDecimal(rd["Preco"]);
                        Iestoque.Livre = true;
                        Iestoque.Qtd = Convert.ToInt32(rd["Qtd"]);
                        Iestoque.QtdPedido = 0;
                        lstItensEstoque.Add(Iestoque);
                    }
                    con.Close();
                }
            }

            return lstItensEstoque;
        }

        [HttpGet("GetItensPedido")]
        [Route("{idpedido}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public List<ItensPedido> GetItensPedido([FromQuery] string idpedido)
        {
            ItensPedido IPedido= new ItensPedido();
            List<ItensPedido> lstItensPedido = new List<ItensPedido>()
;            if (idpedido == null)
                return lstItensPedido;

            string strSql = null;
            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = Configuration.GetConnectionString("ConnectioString");
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;


                    strSql = "  Sp_GridPedidoItens  " + idpedido;

                    cmd.CommandText = strSql;

                    SqlDataReader rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        IPedido = new ItensPedido();
                        IPedido.Id_IPv = Convert.ToInt32(rd["Id_IPv"]);
                        IPedido.Produto = rd["Produto"].ToString();
                        IPedido.Quantid = Convert.ToInt32(rd["Quantid"]);
                        IPedido.Valuni = Convert.ToDecimal(rd["Valuni"]);
                        IPedido.Valtot = Convert.ToDecimal(rd["Valtot"]);
                        IPedido.Saida = rd["Saida"].ToString();
                        IPedido.TipoFrete = rd["TipoFrete"].ToString();
                        lstItensPedido.Add(IPedido);
                    }
                    con.Close();
                }
            }

            if (lstItensPedido.Count == 0)
                lstItensPedido.Add(IPedido);

            return lstItensPedido;
        }

        [HttpGet("GetIdPedido")]
        [Route("{idPedido}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public Pedido GetIdPedido([FromQuery] string idPedido)
        {
            Pedido pedido = new Pedido();
            Cliente cli = null;
            Pessoa pess = null;
            string strSql = null;

            if (idPedido == null)
                return pedido;

            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = Configuration.GetConnectionString("ConnectioString");
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;

                    strSql = " SELECT  top 1 PD.*, ";
                    // strSql += " PD.Situacao, PD.DescrSituacao, PD.Entrega, PD.Per_Ent, "
                    strSql += " P.Nome, M.Descr, VP.IdVendedor, VP.Nome as NomeVendedor, ";
                    // strSql += " IV.Produto, IV.Quantid, IV.Valuni, IV.Tp_Frete, "
                    // strSql += " FP.FormaPag, FP.Tot_Parc, FP.Val_Fin, "
                    // strSql += " IP.Num_Parc, IP.Document, IP.Dt_Venc, IP.Agencia, IP.Praca, IP.Agencia, IP.Num_Cheq, IP.Val_ARec, "
                    strSql += " F.CodDdd, F.Numero, F.Ramal, ";
                    strSql += " E.CEP, E.UF, E.Localidade, E.Logradouro, E.Numero as numeroCasa, E.Complemento,E.Bairro, E.PontoReferencia ";

                    strSql += " FROM    CapaVend PD ";
                    // strSql += " LEFT JOIN    ItemVend IV ON PD.Id_Ped = IV.Id_Ped "
                    // strSql += " LEFT JOIN    Frm_Ped FP ON FP.Id_Ped = PD.Id_Ped "
                    // strSql += " LEFT JOIN    ItemPag IP ON IP.Id_Frp = FP.Id_Frp "
                    strSql += " LEFT JOIN    CLIENTE C ON C.Id_Cli = PD.Id_Cli ";
                    strSql += " LEFT JOIN	  PESSOA P ON C.IDPESSOA = P.IDPESSOA ";
                    strSql += " LEFT JOIN    PessoaEndereco PE ON PE.IdPessoa = P.IdPessoa ";
                    strSql += " LEFT JOIN    Endereco E ON E.IdEndereco = PE.IdEndereco ";
                    strSql += " LEFT JOIN	  (SELECT TOP 1 * FROM PessoaTelefone) T ON P.IdPessoa = T.IdPessoa ";
                    strSql += " LEFT JOIN	  Telefone F ON F.IdTelefone = T.IdTelefone ";
                    strSql += " LEFT JOIN    Midia M ON M.IdMidia = PD.IdMidia ";
                    strSql += " LEFT JOIN    Vend_Ped VP ON VP.Id_Ped = PD.Id_Ped ";
                    strSql += " WHERE   PD.Id_Ped =  " + idPedido;

                    cmd.CommandText = strSql;

                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        pedido = new Pedido();
                        Vendedor vend = new Vendedor();
                        vend.IdVendedor = Convert.ToInt32(dr["IdVendedor"]);
                        vend.Nome = dr["NomeVendedor"].ToString();

                        Endereco end = new Endereco();

                        end.CEP = dr["CEP"].ToString();
                        end.UF = dr["UF"].ToString();
                        end.Localidade = dr["Localidade"].ToString();
                        end.Logradouro = dr["Logradouro"].ToString();
                        end.Numero = dr["numeroCasa"].ToString();
                        end.Complemento = dr["Complemento"].ToString();
                        end.Bairro = dr["Bairro"].ToString();
                        end.PontoReferencia = dr["PontoReferencia"].ToString();

                        Telefone fone = new Telefone();

                        fone.CodDdd = dr["CodDdd"].ToString();
                        fone.Numero = dr["Numero"].ToString();
                        fone.Ramal = dr["Ramal"].ToString();

                        pedido.IdPedido = Convert.ToInt32(dr["IdPedido"].ToString() == "" ? 0 : dr["IdPedido"]);
                        pedido.Id_Ped = Convert.ToInt32(dr["Id_Ped"].ToString() == "" ? 0 : dr["Id_Ped"]);
                        pedido.Id_Cli = Convert.ToInt32(dr["Id_Cli"].ToString() == "" ? 0 : dr["Id_Cli"]); 
                        pedido.vendedor = vend;
                        pedido.Id_Loja = Convert.ToInt32(dr["Id_Loja"].ToString() == "" ? 0 : dr["Id_Loja"]);
                        pedido.IdMidia = Convert.ToInt32(dr["IdMidia"].ToString() == "" ? 0 : dr["IdMidia"]);
                        pedido.DescMidia = dr["Descr"].ToString();
                        pedido.FlMesmoEndEntrega = Convert.ToBoolean(dr["FlMesmoEndEntrega"]);
                        pedido.IdEnderecoEntrega = Convert.ToInt32(dr["IdEnderecoEntrega"].ToString() == "" ? 0 : dr["IdEnderecoEntrega"]);
                        pedido.endereco = end;
                        pedido.IdFoneEntrega = Convert.ToInt32(dr["IdFoneEntrega"].ToString() != "" ? dr["IdFoneEntrega"] : 0);
                        pedido.telefone = fone;
                        pedido.Situacao = Convert.ToInt32(dr["Situacao"].ToString() == "" ? 0 : dr["Situacao"]);
                        pedido.DescrSituacao = dr["DescrSituacao"].ToString();
                        pedido.Tipo = dr["Tipo"].ToString();
                        pedido.DescrTipo = dr["DescrTipo"].ToString();
                        pedido.DtPed = dr["DtPed"].ToString() == "" ? new DateTime(1900,1,1) : Convert.ToDateTime(dr["DtPed"]);
                        pedido.Entrega = dr["Entrega"].ToString() == "" ? new DateTime(1900,1,1) : Convert.ToDateTime(dr["Entrega"]);
                        pedido.DtReceb = dr["DtReceb"].ToString() == "" ? new DateTime(1900,1,1) : Convert.ToDateTime(dr["DtReceb"]);
                        pedido.Per_Ent = dr["Per_Ent"].ToString();
                        pedido.TotProd = Convert.ToInt32(dr["TotProd"].ToString() == "" ? 0 : dr["TotProd"]);
                        pedido.Desconto = Convert.ToDecimal(dr["Desconto"].ToString() == "" ? 0 : dr["Desconto"]);
                        pedido.Desc_Por = Convert.ToInt32(dr["Desc_Por"].ToString() == "" ? 0 : dr["Desc_Por"]);
                        pedido.TotPed = Convert.ToDecimal(dr["TotPed"].ToString() == "" ? 0 : dr["Id_Cli"]);
                        pedido.VlFrete = Convert.ToDecimal(dr["VlFrete"].ToString() == "" ? 0 : dr["VlFrete"]);
                        pedido.Val_Afin = Convert.ToDecimal(dr["Val_Afin"].ToString() == "" ? 0 : dr["Val_Afin"]);
                        pedido.Entregue = Convert.ToBoolean(dr["Entregue"]);
                        pedido.Cup_Fisc = dr["Cup_Fisc"].ToString();
                        pedido.Tem_Frt = Convert.ToBoolean(dr["Tem_Frt"]);
                        pedido.Encerrou = Convert.ToBoolean(dr["Encerrou"]);
                        pedido.Enviado = Convert.ToBoolean(dr["Enviado"]);
                        pedido.Nome_Cli = dr["Nome_Cli"].ToString();
                        pedido.Versao = Convert.ToInt32(dr["Versao"].ToString() == "" ? 0 : dr["Versao"]);
                        pedido.Codpdv = dr["Codpdv"].ToString();
                        pedido.Impresso = Convert.ToBoolean(dr["Impresso"]);
                        pedido.Env_Mail = Convert.ToBoolean(dr["Env_Mail"]);
                        pedido.Id_PdOri = Convert.ToInt32(dr["Id_PdOri"].ToString() == "" ? 0 : dr["Id_PdOri"]);
                        pedido.Bloq_Est = Convert.ToBoolean(dr["Bloq_Est"]);
                        pedido.Prazo_Mp = Convert.ToInt32(dr["Prazo_Mp"].ToString() == "" ? 0 : dr["Prazo_Mp"]);
                        pedido.Desc_Max = Convert.ToInt32(dr["Desc_Max"].ToString() == "" ? 0 : dr["Desc_Max"]);
                        pedido.Nf_Pauli = Convert.ToBoolean(dr["Nf_Pauli"]);
                        pedido.TemCupom = Convert.ToBoolean(dr["TemCupom"]);
                        pedido.NumCupom = dr["NumCupom"].ToString();
                        pedido.EnvCupom = Convert.ToBoolean(dr["EnvCupom"]);
                        pedido.ObsMidia = dr["ObsMidia"].ToString();
                        pedido.Observ = dr["Observ"].ToString();
                        pedido.Obs_Fin = dr["Obs_Fin"].ToString();
                    }

                    con.Close();
                }
            }

            return pedido;
        }

        [HttpPut("AlterarPedido")]
        [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public RetornoPedido AlterarPedido(Pedido pedido)
        {
            RetornoPedido retped = new RetornoPedido();
            SqlTransaction tran = null;
            using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("ConnectioString")))
            {
                con.Open();
                tran = con.BeginTransaction();
                using (SqlCommand cmd = new SqlCommand("Sp_PedidoCapaGrava", con, tran))
                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    //.output('Id_Ped', sql.Int)
                    cmd.Parameters.Add("@Id_Cli", SqlDbType.Int).Value = pedido.Id_Cli;
                    cmd.Parameters.Add("@IdMidia", SqlDbType.Int).Value = pedido.IdMidia;
                    cmd.Parameters.Add("@IdEnderecoEntrega", SqlDbType.Int).Value = pedido.IdEnderecoEntrega;
                    cmd.Parameters.Add("@FlMesmoEndEntrega", SqlDbType.Bit).Value = pedido.FlMesmoEndEntrega;
                    cmd.Parameters.Add("@Tipo", SqlDbType.VarChar).Value = pedido.Tipo;
                    cmd.Parameters.Add("@Entrega", SqlDbType.DateTime).Value = pedido.Entrega;
                    cmd.Parameters.Add("@Per_Ent", SqlDbType.VarChar).Value = pedido.Per_Ent;
                    cmd.Parameters.Add("@ListaVends", SqlDbType.VarChar).Value = pedido.vendedor == null ? 0 : pedido.vendedor.IdVendedor;
                    cmd.Parameters.Add("@VlFrete", SqlDbType.Decimal).Value = pedido.VlFrete;
                    cmd.Parameters.Add("@Desconto", SqlDbType.Decimal).Value = pedido.Desconto;
                    cmd.Parameters.Add("@ObsMidia", SqlDbType.VarChar).Value = pedido.ObsMidia ;
                    cmd.Parameters.Add("@Observ", SqlDbType.VarChar).Value = pedido.Observ;
                    cmd.Parameters.Add("@Obs_Fin", SqlDbType.VarChar).Value = pedido.Obs_Fin;
                    cmd.Parameters.Add("@LogradouroEnt", SqlDbType.VarChar).Value = null; //pedido.endereco.Logradouro;
                    cmd.Parameters.Add("@NumeroEnt", SqlDbType.VarChar).Value = null; //pedido.endereco.Numero;
                    cmd.Parameters.Add("@ComplementoEnt", SqlDbType.VarChar).Value = null; //pedido.endereco.Complemento;
                    cmd.Parameters.Add("@BairroEnt", SqlDbType.VarChar).Value = null; //pedido.endereco.Bairro;
                    cmd.Parameters.Add("@LocalidadeEnt", SqlDbType.VarChar).Value = null; //pedido.endereco.Localidade;
                    cmd.Parameters.Add("@CodMuniEnt", SqlDbType.VarChar).Value = null; //pedido.endereco.CodMuni;
                    cmd.Parameters.Add("@UFEnt", SqlDbType.VarChar).Value = null; //pedido.endereco.UF;
                    cmd.Parameters.Add("@CEPEnt", SqlDbType.VarChar).Value = null; //pedido.endereco.CEP;
                    cmd.Parameters.Add("@CodPaisEnt", SqlDbType.VarChar).Value = null; //pedido.endereco.CodPais;
                    cmd.Parameters.Add("@PontoRefEnt", SqlDbType.VarChar).Value = null; //pedido.endereco.PontoReferencia;
                    cmd.Parameters.Add("@IdFoneEntrega", SqlDbType.Int).Value = pedido.IdFoneEntrega;
                    cmd.Parameters.Add("@DDDEntrega", SqlDbType.VarChar).Value = ""; //pedido.telefone.CodDdd;
                    cmd.Parameters.Add("@FoneEntrega", SqlDbType.VarChar).Value = ""; //pedido.telefone.Numero;
                    cmd.Parameters.Add("@RamalEntrega", SqlDbType.VarChar).Value = ""; //pedido.telefone.Ramal;


                    //Add the output parameter to the command object
                    SqlParameter Id_PedOut = new SqlParameter();
                    Id_PedOut.ParameterName = "@Id_Ped";
                    Id_PedOut.SqlDbType = System.Data.SqlDbType.Int;
                    Id_PedOut.Value = pedido.Id_Ped;
                    Id_PedOut.Direction = System.Data.ParameterDirection.InputOutput;
                    cmd.Parameters.Add(Id_PedOut);


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
                            retped.Id_Ped = cmd.Parameters["@Id_Ped"].Value.ToString();
                            retped.Ok = cmd.Parameters["@Ok"].Value.ToString();
                            retped.MensErro = cmd.Parameters["@MensErro"].Value.ToString();
                        }
                        else
                        {
                            tran.Rollback();
                            retped.Id_Ped = cmd.Parameters["@Id_Ped"].Value.ToString();
                            retped.Ok = cmd.Parameters["@Ok"].Value.ToString();
                            retped.MensErro = cmd.Parameters["@MensErro"].Value.ToString();
                        }
                        return retped;
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException(cmd.Parameters["@MensErro"].Value.ToString());
                    }

                }
            }
        }

        [HttpDelete("ExcluirItemPedido")]
        [Route("{idItemPedido}")]
        [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public RetornoPedido ExcluirItemPedido([FromQuery] string idItemPedido)
        {
            RetornoPedido retped = new RetornoPedido();
            SqlTransaction tran = null;
            using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("ConnectioString")))
            {
                con.Open();
                tran = con.BeginTransaction();
                using (SqlCommand cmd = new SqlCommand("Sp_PedidoItemApaga", con, tran))
                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    //.output('Id_Ped', sql.Int)
                    cmd.Parameters.Add("@Id_Ipv", SqlDbType.Int).Value = idItemPedido;

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
                            retped.Id_Ped = cmd.Parameters["@Id_Ipv"].Value.ToString();
                            retped.Ok = cmd.Parameters["@Ok"].Value.ToString();
                            retped.MensErro = cmd.Parameters["@MensErro"].Value.ToString();
                        }
                        else
                        {
                            tran.Rollback();
                            retped.Id_Ped = cmd.Parameters["@Id_Ipv"].Value.ToString();
                            retped.Ok = cmd.Parameters["@Ok"].Value.ToString();
                            retped.MensErro = cmd.Parameters["@MensErro"].Value.ToString();
                        }
                        return retped;
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException(cmd.Parameters["@MensErro"].Value.ToString());
                    }

                }
            }
        }

        [HttpPost("InserirPedido")]
        [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public RetornoPedido InserirPedido(Pedido pedido)
        {
            RetornoPedido retped = new RetornoPedido();
            SqlTransaction tran = null;

            using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("ConnectioString")))
            {
                con.Open();
                tran = con.BeginTransaction();
                using (SqlCommand cmd = new SqlCommand("Sp_PedidoCapaGrava", con, tran))
                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    //.output('Id_Ped', sql.Int)
                    cmd.Parameters.Add("@Id_Cli", SqlDbType.Int).Value = pedido.Id_Cli;
                    cmd.Parameters.Add("@IdMidia", SqlDbType.Int).Value = pedido.IdMidia;
                    cmd.Parameters.Add("@IdEnderecoEntrega", SqlDbType.Int).Value = pedido.IdEnderecoEntrega;
                    cmd.Parameters.Add("@FlMesmoEndEntrega", SqlDbType.Bit).Value = pedido.FlMesmoEndEntrega;
                    cmd.Parameters.Add("@Tipo", SqlDbType.VarChar).Value = pedido.Tipo;
                    cmd.Parameters.Add("@Entrega", SqlDbType.DateTime).Value = pedido.Entrega;
                    cmd.Parameters.Add("@Per_Ent", SqlDbType.VarChar).Value = pedido.Per_Ent;
                    cmd.Parameters.Add("@ListaVends", SqlDbType.VarChar).Value = pedido.vendedor == null ? 0 : pedido.vendedor.IdVendedor;
                    cmd.Parameters.Add("@VlFrete", SqlDbType.Decimal).Value = pedido.VlFrete;
                    cmd.Parameters.Add("@Desconto", SqlDbType.Decimal).Value = pedido.Desconto;
                    cmd.Parameters.Add("@ObsMidia", SqlDbType.VarChar).Value = pedido.ObsMidia == null ? "" : pedido.ObsMidia;
                    cmd.Parameters.Add("@Observ", SqlDbType.VarChar).Value = pedido.Observ == null ? "" : pedido.Observ;
                    cmd.Parameters.Add("@Obs_Fin", SqlDbType.VarChar).Value = pedido.Obs_Fin == null ? "" : pedido.Obs_Fin;
                    cmd.Parameters.Add("@LogradouroEnt", SqlDbType.VarChar).Value = null; //pedido.endereco.Logradouro;
                    cmd.Parameters.Add("@NumeroEnt", SqlDbType.VarChar).Value = null; //pedido.endereco.Numero;
                    cmd.Parameters.Add("@ComplementoEnt", SqlDbType.VarChar).Value = null; //pedido.endereco.Complemento;
                    cmd.Parameters.Add("@BairroEnt", SqlDbType.VarChar).Value = null; //pedido.endereco.Bairro;
                    cmd.Parameters.Add("@LocalidadeEnt", SqlDbType.VarChar).Value = null; //pedido.endereco.Localidade;
                    cmd.Parameters.Add("@CodMuniEnt", SqlDbType.VarChar).Value = null; //pedido.endereco.CodMuni;
                    cmd.Parameters.Add("@UFEnt", SqlDbType.VarChar).Value = null; //pedido.endereco.UF;
                    cmd.Parameters.Add("@CEPEnt", SqlDbType.VarChar).Value = null; //pedido.endereco.CEP;
                    cmd.Parameters.Add("@CodPaisEnt", SqlDbType.VarChar).Value = null; //pedido.endereco.CodPais;
                    cmd.Parameters.Add("@PontoRefEnt", SqlDbType.VarChar).Value = null; //pedido.endereco.PontoReferencia;
                    cmd.Parameters.Add("@IdFoneEntrega", SqlDbType.Int).Value = pedido.IdFoneEntrega;
                    cmd.Parameters.Add("@DDDEntrega", SqlDbType.VarChar).Value = ""; //pedido.telefone.CodDdd;
                    cmd.Parameters.Add("@FoneEntrega", SqlDbType.VarChar).Value = ""; //pedido.telefone.Numero;
                    cmd.Parameters.Add("@RamalEntrega", SqlDbType.VarChar).Value = ""; //pedido.telefone.Ramal;


                    //Add the output parameter to the command object
                    SqlParameter Id_PedOut = new SqlParameter();
                    Id_PedOut.ParameterName = "@Id_Ped";
                    Id_PedOut.SqlDbType = System.Data.SqlDbType.Int;
                    Id_PedOut.Size = 30;
                    Id_PedOut.Value = null;
                    Id_PedOut.Direction = System.Data.ParameterDirection.InputOutput;
                    cmd.Parameters.Add(Id_PedOut);


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
                            retped.Id_Ped = cmd.Parameters["@Id_Ped"].Value.ToString();
                            retped.Ok = cmd.Parameters["@Ok"].Value.ToString();
                            retped.MensErro = cmd.Parameters["@MensErro"].Value.ToString();
                        }
                        else
                        {
                            tran.Rollback();
                            retped.Id_Ped = cmd.Parameters["@Id_Ped"].Value.ToString();
                            retped.Ok = cmd.Parameters["@Ok"].Value.ToString();
                            retped.MensErro = cmd.Parameters["@MensErro"].Value.ToString();
                        }
                        return retped;
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw new InvalidOperationException(cmd.Parameters["@MensErro"].Value.ToString());
                    }

                }
            }
        }

        
        [HttpPost("IncluirFormaPagamento")]
        [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public RetornoPedido IncluirFormaPagamento([FromBody] JObject data)
        {
            RetornoPedido retped = new RetornoPedido();
            SqlTransaction tran = null;

            FormaPagamento formPagto = data["params"]["formaPagto"].ToObject<FormaPagamento>();
            ItemPagamento itempagto = data["params"]["itempagto"].ToObject<ItemPagamento>();


            using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("ConnectioString")))
            {
                con.Open();
                tran = con.BeginTransaction();
                using (SqlCommand cmd = new SqlCommand("Sp_PedidoPlanoInsere", con, tran))
                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Id_Ped", SqlDbType.Int).Value = formPagto.Id_Ped; 
                    cmd.Parameters.Add("@Id_Forma", SqlDbType.Int).Value = formPagto.Id_Forma;  
                    cmd.Parameters.Add("@Id_Plano", SqlDbType.Int).Value = formPagto.Id_Plano;
                    cmd.Parameters.Add("@IdFormaEntrada", SqlDbType.Int).Value = 1; // ***************REVISAR
                    cmd.Parameters.Add("@IdTefOperacao", SqlDbType.Int).Value = 1; // ***************REVISAR
                    cmd.Parameters.Add("@Tot_Parc", SqlDbType.Int).Value = itempagto.Num_Parc;  
                    cmd.Parameters.Add("@Val_Fin", SqlDbType.Decimal).Value = formPagto.Val_Fin;
                    cmd.Parameters.Add("@ValorParcela", SqlDbType.Decimal).Value = itempagto.Valor;
                    cmd.Parameters.Add("@Cont_Fin", SqlDbType.VarChar).Value = formPagto.Cont_Fin; 
                    cmd.Parameters.Add("@Dt_ContF", SqlDbType.DateTime).Value = formPagto.Dt_ContF;
                    cmd.Parameters.Add("@Aut_Cart", SqlDbType.VarChar).Value = formPagto.Aut_Cart; 
                    cmd.Parameters.Add("@Tem_Fret", SqlDbType.Bit).Value = formPagto.Tem_Fret;
                    cmd.Parameters.Add("@NumCheque", SqlDbType.VarChar).Value = itempagto.Num_Cheq;
                    cmd.Parameters.Add("@Banco", SqlDbType.VarChar).Value = itempagto.Banco;
                    cmd.Parameters.Add("@Agencia", SqlDbType.VarChar).Value = itempagto.Agencia;
                    cmd.Parameters.Add("@Praca", SqlDbType.VarChar).Value = itempagto.Praca;
                    cmd.Parameters.Add("@SoValida", SqlDbType.Bit).Value = false; 
                    cmd.Parameters.Add("@Id_Usr", SqlDbType.Int).Value = formPagto.id_Usr; 

                    //Add the output parameter to the command object
                    SqlParameter @Id_FrpOut = new SqlParameter();
                    @Id_FrpOut.ParameterName = "@Id_Frp";
                    @Id_FrpOut.SqlDbType = System.Data.SqlDbType.Int;
                    @Id_FrpOut.Size = 30;
                    @Id_FrpOut.Value = null; // formPagto.Id_Frp;
                    @Id_FrpOut.Direction = System.Data.ParameterDirection.InputOutput;
                    cmd.Parameters.Add(@Id_FrpOut);


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
                            retped.Id_Ped = cmd.Parameters["@Id_Ped"].Value.ToString();
                            retped.Ok = cmd.Parameters["@Ok"].Value.ToString();
                            retped.MensErro = cmd.Parameters["@MensErro"].Value.ToString();
                        }
                        else
                        {
                            tran.Rollback();
                            retped.Id_Ped = cmd.Parameters["@Id_Ped"].Value.ToString();
                            retped.Ok = cmd.Parameters["@Ok"].Value.ToString();
                            retped.MensErro = cmd.Parameters["@MensErro"].Value.ToString();
                        }
                        return retped;
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw new InvalidOperationException(cmd.Parameters["@MensErro"].Value.ToString());
                    }

                }
            }
        }

        [HttpPost("IncluirItemEstoquePedido")]
        [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public bool IncluirItemEstoquePedido(List<ItensEstoque> ItensEstoque)
        {
            RetornoPedido retped = new RetornoPedido();
            SqlTransaction tran = null;

            using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("ConnectioString")))
            {
                con.Open();
                tran = con.BeginTransaction();

                foreach(ItensEstoque ie in ItensEstoque)
                {
                    using (SqlCommand cmd = new SqlCommand("Sp_PedidoItemEstqInsere", con, tran))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@Id_Ped", SqlDbType.Int).Value = ie.Id_Ped;
                        cmd.Parameters.Add("@Id_Alp", SqlDbType.Int).Value = ie.Id_Alp;
                        cmd.Parameters.Add("@Quantid", SqlDbType.Int).Value = ie.QtdPedido;


                        //Add the output parameter to the command object
                        //SqlParameter Id_PedOut = new SqlParameter();
                        //Id_PedOut.ParameterName = "@Id_Ped";
                        //Id_PedOut.SqlDbType = System.Data.SqlDbType.Int;
                        //Id_PedOut.Size = 30;
                        //Id_PedOut.Value = ie.Id_Ped;
                        //Id_PedOut.Direction = System.Data.ParameterDirection.InputOutput;
                        //cmd.Parameters.Add(Id_PedOut);


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
                            if (cmd.Parameters["@Ok"].Value.ToString() == "0")
                            {
                                tran.Rollback();
                                throw new InvalidOperationException("Erro ao inserir item no pedido");
                            }
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            throw new InvalidOperationException(cmd.Parameters["@MensErro"].Value.ToString());
                        }
                    }
                }

                tran.Commit();
                return true;
            }
        }


        [HttpGet("GetFormaPagamento")]
        [Route("{idpedido}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public List<FormaPagamento> GetFormaPagamento([FromQuery] string idpedido)
        {
            FormaPagamento frmPagto = new FormaPagamento();
            List<FormaPagamento> lstfrmPagto = new List<FormaPagamento>();
            if (idpedido == null)
                return lstfrmPagto;

            string strSql = null;
            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = Configuration.GetConnectionString("ConnectioString");
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;


                    strSql = " SELECT Id_Frp, Id_Ped, Id_Forma, Id_Plano, FormaPag, DtLanc, Tp_Pagto, Tot_Parc, ";
                    strSql += " ValDescFin, Val_Fin, Val_Entr, Val_ARec, Cont_Fin, Dt_ContF,Aut_Cart, Tem_Fret, Dt_Venc, Dt_Baixa, FlCancelado, DhIns, UsrIns ";
                    strSql += " FROM FRM_PED WHERE ID_PED = " + idpedido;

                    //Exec Sp_GridPedidoFrp @Id_Ped = 1951212

                    cmd.CommandText = strSql;

                    SqlDataReader rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        frmPagto = new FormaPagamento();
                        frmPagto.Id_Frp = Convert.ToInt32(rd["Id_Frp"]);
                        frmPagto.Id_Ped = Convert.ToInt32(rd["Id_Ped"]);
                        frmPagto.Id_Forma = Convert.ToInt32(rd["Id_Forma"]);
                        frmPagto.Id_Plano = Convert.ToInt32(rd["Id_Plano"]);
                        frmPagto.FormaPag = rd["FormaPag"].ToString();
                        frmPagto.DtLanc = rd["DtLanc"].ToString() != "" ? Convert.ToDateTime(rd["DtLanc"]) : null;
                        frmPagto.Tp_Pagto = rd["Tp_Pagto"].ToString();
                        frmPagto.Tot_Parc = Convert.ToInt32(rd["Tot_Parc"]);
                        frmPagto.ValDescFin = Convert.ToDecimal(rd["ValDescFin"]);
                        frmPagto.Val_Fin = Convert.ToDecimal(rd["Val_Fin"]);
                        frmPagto.Val_Entr = Convert.ToDecimal(rd["Val_Entr"]);
                        frmPagto.Val_ARec = Convert.ToDecimal(rd["Val_ARec"]);
                        frmPagto.Cont_Fin = rd["Cont_Fin"].ToString();
                        frmPagto.Dt_ContF = rd["Dt_ContF"].ToString() != "" ? Convert.ToDateTime(rd["Dt_ContF"]) : null;
                        frmPagto.Aut_Cart = rd["Aut_Cart"].ToString();
                        frmPagto.Tem_Fret = Convert.ToBoolean(rd["Tem_Fret"]);
                        frmPagto.Dt_Venc = rd["Dt_Venc"].ToString() != "" ? Convert.ToDateTime(rd["Dt_Venc"]) : null;
                        frmPagto.Dt_Baixa = rd["Dt_Baixa"].ToString() != "" ? Convert.ToDateTime(rd["Dt_Baixa"]) : null;
                        frmPagto.FlCancelado = Convert.ToBoolean(rd["FlCancelado"]);
                        frmPagto.DhIns = rd["DhIns"].ToString() != "" ? Convert.ToDateTime(rd["DhIns"]) : null;
                        frmPagto.UsrIns = rd["UsrIns"].ToString();
                        lstfrmPagto.Add(frmPagto);
                    }
                    con.Close();
                }
            }

            if (lstfrmPagto.Count == 0)
                lstfrmPagto.Add(frmPagto);

            return lstfrmPagto;
        }


        [HttpGet("GetItensPagamento")]
        [Route("{idFormaPagamento}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public List<ItemPagamento> GetItensPagamento([FromQuery] string idFormaPagamento)
        {
            ItemPagamento itensPagto = new ItemPagamento();
            List<ItemPagamento> lstitensPagto = new List<ItemPagamento>();
            if (idFormaPagamento == null)
                return lstitensPagto;

            string strSql = null;
            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = Configuration.GetConnectionString("ConnectioString");
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;


                    strSql = " Exec Sp_GridPedidoItp @Id_Frp = " + idFormaPagamento; 

                    cmd.CommandText = strSql;

                    SqlDataReader rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        itensPagto = new ItemPagamento();
                        itensPagto.Id_Item = Convert.ToInt32(rd["Id_Item"]);
                        itensPagto.Id_Frp = Convert.ToInt32(idFormaPagamento);
                        itensPagto.Num_Parc = Convert.ToInt32(rd["Num_Parc"]);
                        //itensPagto.FormaPag = rd["FormaPag"].ToString();
                        itensPagto.Document = rd["Document"].ToString();
                        //itensPagto.Tp_Parc = rd["Tp_Parc"].ToString();
                        itensPagto.Valor = Convert.ToDecimal(rd["Valor"]);
                        //itensPagto.Plan_Fin = rd["Plan_Fin"].ToString();
                        //itensPagto.Val_Fin = Convert.ToDecimal(rd["Val_Fin"]);
                        itensPagto.Banco = rd["Banco"].ToString();
                        itensPagto.Agencia = rd["Agencia"].ToString();
                        itensPagto.Praca = rd["Praca"].ToString();
                        itensPagto.Num_Cheq = rd["Num_Cheq"].ToString();
                        //itensPagto.Val_Cheq = Convert.ToDecimal(rd["Val_Cheq"]);
                        itensPagto.Dt_Venc = Convert.ToDateTime(rd["Dt_Venc"]);
                        //itensPagto.Val_ARec = Convert.ToDecimal(rd["Val_ARec"]);
                        itensPagto.ValFrete = Convert.ToDecimal(rd["ValFrete"]);
                        lstitensPagto.Add(itensPagto);
                    }
                    con.Close();
                }
            }

            if (lstitensPagto.Count == 0)
                lstitensPagto.Add(itensPagto);

            return lstitensPagto;
        }


        [HttpGet("ExcluirFormaPagamento")]
        [Route("{idFormaPagamento}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public bool ExcluirFormaPagamento([FromQuery]  string idFormaPagamento)
        {
            SqlTransaction tran = null;
            string strSql = null;

            using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("ConnectioString")))
            {
                con.Open();
                tran = con.BeginTransaction();

                strSql = "DELETE FROM ItemPag where Id_Frp = " + idFormaPagamento;

                using (SqlCommand cmd = new SqlCommand(strSql, con, tran))
                 {
                    try
                    {
                        cmd.ExecuteNonQuery();
                        strSql = "DELETE FROM Frm_Ped where Id_Frp = " + idFormaPagamento;
                        using (SqlCommand cmd1 = new SqlCommand(strSql, con, tran))
                        {
                            try
                            {
                                cmd1.ExecuteNonQuery();
                            }
                            catch (Exception)
                            {
                                tran.Rollback();
                                throw new InvalidOperationException("Erro ao excluir forma de pagamento");
                            }
                        }
                       
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw new InvalidOperationException(cmd.Parameters["@MensErro"].Value.ToString());
                    }
                }
                tran.Commit();
                return true;
            }
        }


        [HttpPut("AlterarPedido")]
        [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public RetornoPedido EncerraPedido(Pedido pedido)
        {
            RetornoPedido retped = new RetornoPedido();
            SqlTransaction tran = null;
            using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("ConnectioString")))
            {
                con.Open();
                tran = con.BeginTransaction();
                using (SqlCommand cmd = new SqlCommand("Sp_PedidoEncerra", con, tran))
                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    //.output('Id_Ped', sql.Int)
                    cmd.Parameters.Add("@Id_Ped", SqlDbType.Int).Value = pedido.Id_Cli;

                    SqlParameter mensagemOut = new SqlParameter();
                    mensagemOut.ParameterName = "@Mensagem";
                    mensagemOut.SqlDbType = System.Data.SqlDbType.VarChar;
                    mensagemOut.Size = 1000;
                    mensagemOut.Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters.Add(mensagemOut);

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
                            retped.Id_Ped = cmd.Parameters["@Id_Ped"].Value.ToString();
                            retped.Ok = cmd.Parameters["@Ok"].Value.ToString();
                            retped.MensErro = cmd.Parameters["@MensErro"].Value.ToString();
                        }
                        else
                        {
                            tran.Rollback();
                            retped.Id_Ped = cmd.Parameters["@Id_Ped"].Value.ToString();
                            retped.Ok = cmd.Parameters["@Ok"].Value.ToString();
                            retped.MensErro = cmd.Parameters["@MensErro"].Value.ToString();
                        }
                        return retped;
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException(cmd.Parameters["@MensErro"].Value.ToString());
                    }

                }
            }
        }
    }
}
