using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GLJ.Models.Cliente;
using GLJ.Models.BPFacade;
using GLJ.Filter.Entity;
using GLJ.Business.Entity;
using MVC.Controls;
using MVC.Controls.Grid;
using Onion.Business.Entity;
using GLJ.Business.Validation;

namespace GLJ.Controllers
{
    public class ClienteController : BaseControllerGeneric<PDCliente>
    {
        #region Métodos Chamada

        [AuthorizeOnion]
        public ActionResult Index()
        {
            this.LimparMensagens();
            return View();
        }

        [HttpGet]
        [AuthorizeOnion]
        public ActionResult CadastroCliente(int? id)
        {
            if (id.HasValue && id.GetValueOrDefault() > 0)
            {
                this.ProcessData.Cliente = BPFCliente.Instance.ObterTodos(new FECliente() { Codigo = id.GetValueOrDefault(), CodigoLoja = UsuarioLogin.CodigoLoja }).ResultList.FirstOrDefault();
                this.ProcessData.DadosCliente = BPFDadosCliente.Instance.ObterTodos(new FEDadosCliente() { CodigoCliente = this.ProcessData.Cliente.Codigo }).ResultList.FirstOrDefault();
                this.ProcessData.TelefoneCliente.CodigoCliente = this.ProcessData.Cliente.Codigo;
                Session["Cliente"] = this.ProcessData;
            }
            else
            {
                Session["Cliente"] = null;
            }
            this.CarregaCombos();
            CarregaUFCidade();
            return View(this.ProcessData);
        }

        [AuthorizeOnion]
        public ActionResult ConsultarCliente()
        {
            CarregaUFCidade();
            return View(this.ProcessData);
        }

        #endregion

        #region Metodos Especificos
        [HttpPost]
        public JsonResult Salvar(PDCliente model)
        {
            try
            {


                BVCliente cliente = new BVCliente();
                cliente.Salvar(model.Cliente);
                bool ClienteExiste = BPFCliente.Instance.VerificaCPFExistente(new FECliente() { CodigoLoja = UsuarioLogin.CodigoLoja, Cpf = model.Cliente.CPF });

                this.ProcessData.Cliente = model.Cliente;

                if (Session["Cliente"] != null)
                {
                    this.ProcessData = (PDCliente)Session["Cliente"];
                }


                if (ClienteExiste && this.ProcessData.Cliente.Codigo == 0 && this.ProcessData.Cliente.TipoCliente == TipoCliente.PessoaFisica)
                {
                    return Json(new JsonReturn
                    {
                        Success = false,
                        Mensage = "Já é um cliente cadastrado.",
                    });
                }

                model.Cliente.CodigoLoja = UsuarioLogin.CodigoLoja;

                this.ProcessData.Cliente = new BPFCliente().Salvar(model.Cliente);

                Session["Cliente"] = this.ProcessData;

                return Json(new JsonReturn
                {
                    Success = true,
                    Mensage = "Pedido aguardando complemento das informações",
                });
            }
            catch (Exception e)
            {

                return Json(new JsonReturn
                {
                    Success = false,
                    Mensage = e.Message,
                });
            }
        }

        [HttpPost]
        public JsonResult VerificaCliente(string cpf)
        {
            if (BPFCliente.Instance.VerificaCPFExistente(new FECliente() { CodigoLoja = UsuarioLogin.CodigoLoja, Cpf = cpf }))
            {
                return Json(new JsonReturn
                {
                    Success = false,
                    Mensage = "Já é um cliente cadastrado.",
                });

            }

            return Json(new JsonReturn
            {
                Success = true,
                Mensage = "Pedido aguardando complemento das informações",
            });
        }

        public ActionResult AdicionarObservacao(PDCliente model)
        {
            try
            {
                this.ProcessData = (PDCliente)Session["Cliente"];

                model.Cliente.CodigoLoja = UsuarioLogin.CodigoLoja;
                model.Cliente.Codigo = this.ProcessData.Cliente.Codigo;

                if (model.Cliente.Codigo > 0)
                {
                    string obs = model.Cliente.Observacao;
                    this.ProcessData.Cliente = BPFCliente.Instance.ObterTodos(new FECliente() { Codigo = model.Cliente.Codigo, CodigoLoja = model.Cliente.CodigoLoja }).ResultList.FirstOrDefault();
                    this.ProcessData.Cliente.Observacao = obs;
                    this.ProcessData.Cliente = new BPFCliente().Salvar(this.ProcessData.Cliente);
                }
                else
                {
                    throw new Exception("Salve os dados do cliente antes de adicionar uma obsevação.");
                }

                return RedirectToAction("Index", "Pedido", new { idCli = this.ProcessData.Cliente.Codigo });

            }
            catch (Exception ex)
            {
                return Json(new JsonReturn(true, ex.Message));

            }

        }

        [HttpPost]
        public JsonResult BuscarCliente(PDCliente model)
        {
            model.FECliente.CodigoLoja = UsuarioLogin.CodigoLoja;

            Session["FECliente"] = model.FECliente;
            return Json(null);
        }

        [HttpGet]
        public JsonResult Buscar(SearchModel searchModel)
        {
            Session["FECliente"] = Session["FECliente"] ?? new FECliente() { CodigoLoja = UsuarioLogin.CodigoLoja };
            List<BECliente> lst = BPFCliente.Instance.ObterTodos(Session["FECliente"] as FECliente).ResultList;

            GridData gridData = lst.AsQueryable().ToGridData(searchModel, GridClienteModel.BEClienteColumns);
            return Json(gridData, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult BuscarClientePopUp(string Nome, string Cpf, string RG)
        {
            FECliente feCliente = new FECliente { CodigoLoja = UsuarioLogin.CodigoLoja, Nome = Nome, Cpf = Cpf, RG = RG };
            Session["FECliente"] = feCliente;
            return Json(null);
        }

        private void CarregaCombos()
        {
            var listTipo = Onion.Util.EnumHelper.GetDescriptions<TipoDadoCliente>();
            TempData["Tipos"] = new SelectList(listTipo, "Value", "Name");

            var listTipoTelefone = Onion.Util.EnumHelper.GetDescriptions<TipoTelefone>();
            TempData["TiposTelefone"] = new SelectList(listTipoTelefone, "Value", "Name");

            var listTipoPessoa = Onion.Util.EnumHelper.GetDescriptions<TipoCliente>();

            TempData["TipoPessoa"] = new SelectList(listTipoPessoa, "Value", "Name");
        }

        [HttpGet]
        public JsonResult AutoCompleteCliente(string term)
        {
            FECliente feCliente = new FECliente
            {
                CodigoLoja = UsuarioLogin.CodigoLoja,
                Nome = term
            };

            var clientes = from c in BPFCliente.Instance.ObterTodos(feCliente).ResultList
                           select new
                           {
                               label = c.Nome,
                               value = c.Nome,
                               c.Codigo,
                               c.CPF,
                               c.Observacao
                           };


            return Json(clientes, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObterDetalheCliente(int? codigoCliente)
        {
            try
            {
                FEDadosCliente feDadosCliente = new FEDadosCliente
                {
                    CodigoCliente = codigoCliente.GetValueOrDefault(),
                };

                var DetalheCliente = from d in BPFDadosCliente.Instance.ObterTodos(feDadosCliente).ResultList
                                     select new
                                     {
                                         Endereco = d.Endereco + "," + d.Numero + " - " + d.Bairro + ", " + d.Cidade.NomeCidade,
                                         Nome = d.Cliente.Nome,
                                         CPF = d.Cliente.CPF,
                                         CEP = d.CEP,
                                         REF = d.Referencia
                                     };
                if (DetalheCliente.Count() > 0)
                {
                    return Json(new JsonReturn(true, "", DetalheCliente.FirstOrDefault()), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new JsonReturn(false, "Dados do cliente selecionado está incompleto."), JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {
                return Json(new JsonReturn(false, ex.Message), JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public JsonResult ObterDadosCliente(int? codigoCliente, SearchModel searchModel)
        {
            this.ProcessData = (PDCliente)Session["Cliente"];
            FEDadosCliente feDadosCliente = new FEDadosCliente() { CodigoCliente = this.ProcessData.Cliente.Codigo };
            IQueryable<BEDadosCliente> model = new BPFCliente().ObterDadosCliente(feDadosCliente).AsQueryable();

            GridData gridData = model.ToGridData(searchModel, GridClienteModel.BEDadosClienteColumns);
            return Json(gridData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObterTelefoneCliente(int? codigoCliente, SearchModel searchModel)
        {
            this.ProcessData = (PDCliente)Session["Cliente"];
            FETelefoneCliente feTelefoneCliente = new FETelefoneCliente() { CodigoCliente = this.ProcessData.Cliente.Codigo };
            IQueryable<BETelefoneCliente> model = new BPFCliente().ObterTelefonesCliente(feTelefoneCliente).AsQueryable();

            GridData gridData = model.ToGridData(searchModel, GridClienteModel.BETelefoneCienteColumns);
            return Json(gridData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SalvarDados(PDCliente model)
        {
            try
            {
                this.ProcessData = (PDCliente)Session["Cliente"];

                model.DadosCliente.CodigoCliente = this.ProcessData.Cliente.Codigo;
                model.DadosCliente.CodigoTipoDadoCliente = TipoDadoCliente.ENTREGA;

                this.ProcessData.DadosCliente = new BPFCliente().SalvarDados(model.DadosCliente);

                Session["Cliente"] = this.ProcessData;

                return Json(new JsonReturn
                {
                    Success = true,
                    Mensage = "Informe os telefones",
                });



                //return Json(new JsonReturn(true, "Endereço adicionado com sucesso!"));
            }
            catch (Exception ex)
            {
                return Json(new JsonReturn(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult SalvarTelefone(PDCliente model)
        {
            try
            {

                this.ProcessData = (PDCliente)Session["Cliente"];

                model.TelefoneCliente.CodigoCliente = this.ProcessData.Cliente.Codigo;
                this.ProcessData.TelefoneCliente = new BPFCliente().SalvarTelefone(model.TelefoneCliente);
                this.ProcessData.TelefoneCliente = model.TelefoneCliente;


                Session["Cliente"] = this.ProcessData;

                return Json(new JsonReturn
                {
                    Success = true,
                    Mensage = "Informe uma Obeservação",
                });



                //return Json(new JsonReturn(true, "Telefone adicionado com sucesso!"));
            }
            catch (Exception ex)
            {
                return Json(new JsonReturn(false, ex.Message));
            }
        }

        private void CarregaUFCidade()
        {
            if (!string.IsNullOrEmpty(this.ProcessData.DadosCliente.UF))
            {
                base.CarregaListaUF(this.ProcessData.DadosCliente.UF);

                base.CarregarCidade(this.ProcessData.DadosCliente.UF, this.ProcessData.DadosCliente.CodigoCidade);
            }
            else
            {
                base.CarregaListaUF();
            }
        }

        public ActionResult CarregarTipoEndereco()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            var listMotivo = Onion.Util.EnumHelper.GetDescriptions<TipoDadoCliente>();

            foreach (var m in listMotivo)
                sb.Append(m.Value.ToString() + ":" + m.Name + ";");

            return Json(sb.ToString(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CarregarTipoTelefone()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            var list = Onion.Util.EnumHelper.GetDescriptions<TipoTelefone>();

            foreach (var m in list)
                sb.Append(m.Value.ToString() + ":" + m.Name + ";");

            return Json(sb.ToString(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeletarTelefone(BETelefoneCliente telefoneCliente)
        {
            this.ProcessData = (PDCliente)Session["Cliente"];

            telefoneCliente.CodigoCliente = this.ProcessData.Cliente.Codigo;
            BPFTelefoneCliene.Instance.Excluir(telefoneCliente);

            
            return Json(this.ProcessData, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
