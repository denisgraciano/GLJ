using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GLJ.Models.Pedido;
using MVC.Controls;
using MVC.Controls.Grid;
using GLJ.Business.Entity;
using GLJ.Models.BPFacade;
using GLJ.Filter.Entity;
using GLJ.Acesso.Filter.Entity;
using GLJ.Acesso.Business.Entity;

namespace GLJ.Controllers
{
    public class PedidoController : PagamentoController
    {
        #region Métodos de Chamada
        [AuthorizeOnion]
        public ActionResult Index(int? id, int? idCli)
        {
            if (!id.HasValue)
            {
                Session.Remove("Pedido");
                BEVendedor beVendedor = BPFVendedor.Instance.ObterTodos(new FEVendedor { CodigoUsuario = UsuarioLogin.Codigo, CodigoLoja = UsuarioLogin.CodigoLoja }).ResultList.FirstOrDefault();
                if (beVendedor == null)
                {
                    base.AdicionarMensagemAtencao("Usuário logado não é vendedor, por favor verifique sua permissão");
                    return RedirectToAction(UsuarioLogin.ViewsController[0].View, UsuarioLogin.ViewsController[0].Controle);
                }
                else
                {
                    this.ProcessData.Pedido.CodigoVendedor = beVendedor.Codigo;
                    this.ProcessData.Pedido.CodigoLoja = beVendedor.CodigoLoja;


                    //TODO: Validar com o combo Selecionado
                    this.ProcessData.Pedido.CodigoFilialLoja = (int)UsuarioLogin.CodigoFilialLoja;
                }

                this.ProcessData.Pedido.DataCompra = DateTime.Now;
                this.ProcessData.Pedido.NumeroPedido = BPFPedido.Instance.NumPedido((int)UsuarioLogin.CodigoFilialLoja, UsuarioLogin.CodigoLoja);
                this.ProcessData.ValoTotalPago = 0;
                this.CalculaEntrega();
                this.CarregaCombo();

                if (idCli != null)
                    this.ProcessData.Pedido.CodigoCliente = (int)idCli;

            }
            else
            {
                FEPedido fePedido = new FEPedido();
                fePedido.CodigoLoja = UsuarioLogin.Loja.Codigo;
                fePedido.Codigo = id.GetValueOrDefault();
                fePedido.CodigoLojaFilial = (int)UsuarioLogin.CodigoFilialLoja;
                this.ProcessData = new BPFPedido().ObterPedido(fePedido);

                if (this.ProcessData.Pedido != null)
                {
                    FEPedidoDetalhe fePedidoDetalhe = new FEPedidoDetalhe()
                    {
                        CodigoPedido = this.ProcessData.Pedido.Codigo
                    };

                    this.ProcessData.Pedido.PedidoDetalhe = BPFPedidoDetalhe.Instance.ObterTodos(fePedidoDetalhe).ResultList;

                    this.ProcessData.TotalItens = this.ProcessData.Pedido.PedidoDetalhe.Count;
                    this.ProcessData.ValorTotal = this.ProcessData.Pedido.PedidoDetalhe.Sum(p => p.ValorTotalProduto);
                    this.ProcessData.ValorRestantePagamento = this.ProcessData.ValorTotal;

                    FEPagamentoPedido fePagamentoPedido = new FEPagamentoPedido() { CodigoPedido = this.ProcessData.Pedido.Codigo };

                    this.ProcessData.ValoTotalPago = BPFPagamentoPedido.Instance.ObterTodos(fePagamentoPedido).ResultList.Sum(s => s.ValorTotalPagamento);
                    this.ProcessData.PagamentosPedido = BPFPagamentoPedido.Instance.ObterTodos(fePagamentoPedido).ResultList;
                }

            }

            var listTipo = Onion.Util.EnumHelper.GetDescriptions<TipoPagamento>();
            TempData["TiposPagamento"] = new SelectList(listTipo, "Value", "Name");


            Session["Pedido"] = this.ProcessData;

            return View("Index", this.ProcessData);
        }



        [AuthorizeOnion]
        public ActionResult ConsultarPedido()
        {
            this.LimparMensagens();
            return View(this.ProcessData);
        }

        public ActionResult DetalhePedido(int? id)
        {
            if (id == 0)
                return RedirectToAction("ConsultarPedido");

            this.LimparMensagens();

            this.ProcessData.Pedido = BPFPedido.Instance.ObterTodos(new FEPedido() { Codigo = id, CodigoLojaFilial = (int)UsuarioLogin.CodigoFilialLoja, CodigoLoja = UsuarioLogin.CodigoLoja }).ResultList.FirstOrDefault();

            return View(this.ProcessData);
        }


        //[AuthorizeOnion]
        public ActionResult ImprimirPedido()
        {
            this.LimparMensagens();
            return View(this.ProcessData);
        }

        #endregion

        #region Metodos Especificos
        [HttpGet]
        public JsonResult BuscarProduto(SearchModel searchModel)
        {
            IQueryable<BEPedidoDetalhe> model = ((PDPedido)Session["Pedido"]).Pedido.PedidoDetalhe.AsQueryable();
            GridData gridData = model.ToGridData(searchModel, GridPedidoModel.BEPedidoDetalheColumns);
            return Json(gridData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Pagamentos(SearchModel searchModel)
        {
            IQueryable<BEPagamentoPedido> model = ((PDPedido)Session["Pedido"]).PagamentosPedido.AsQueryable();

            GridData gridData = model.ToGridData(searchModel, GridPedidoModel.BEPagamentoPedidoColumns);
            return Json(gridData, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult AdicionarProduto(Int32 Codigo)
        {
            this.ProcessData = (PDPedido)Session["Pedido"];
            if (this.ProcessData.Pedido.PedidoDetalhe.FirstOrDefault(prop => prop.CodigoProduto == Codigo) == null)
            {
                BEProduto produto = BPFProduto.Instance.ObterTodos(new FEProduto() { Codigo = Codigo, CodigoLoja = UsuarioLogin.CodigoLoja, LoadType = Onion.Business.Entity.LoadType.Medium }).ResultList.SingleOrDefault();
                this.ProcessData.Pedido.PedidoDetalhe.Add(new BEPedidoDetalhe()
                {
                    Produto = produto,
                    ValorTotalProduto = Convert.ToDecimal(produto.Preco.PrecoVenda),
                    QuantidadeProduto = 1,
                });
                return Json(new JsonReturn(true));
            }
            else
            {
                return Json(new JsonReturn(false, "Já existe este item na lista"));
            }
        }

        [HttpPost]
        public JsonResult EditarQuantidade(Int32 CodigoProduto, Int32 QuantidadeProduto, string oper)
        {
            this.ProcessData = (PDPedido)Session["Pedido"];
            BEPedidoDetalhe pedidoDetalhe = this.ProcessData.Pedido.PedidoDetalhe.FirstOrDefault(prop => prop.CodigoProduto == CodigoProduto);
            pedidoDetalhe.QuantidadeProduto = QuantidadeProduto;
            pedidoDetalhe.ValorTotalProduto = Convert.ToDecimal(pedidoDetalhe.Produto.Preco.PrecoVenda) * QuantidadeProduto;
            return Json(true);
        }

        [HttpPost]
        public JsonResult ObterValores(decimal? desconto)
        {
            this.ProcessData = (PDPedido)Session["Pedido"];
            decimal? valorTotal = this.ProcessData.Pedido.PedidoDetalhe.Sum(p => p.ValorTotalProduto);

            return Json(new JsonReturn(true, "", new { SubTotal = valorTotal, TotalPedido = valorTotal, totalItens = this.ProcessData.TotalItens }));
        }

        [HttpPost]
        public JsonResult ObterValoresPagamento()
        {
            return Json(new JsonReturn(true, "", new { totalItens = this.ProcessData.TotalItens, totalRestante = this.ProcessData.ValorRestantePagamento, valorTotal = this.ProcessData.ValorTotal }));
        }

        /// <summary>
        /// Salvar Rascunho pedido ABA 1 (Adiconar Cliente )
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SalvarRascunhoPedido(PDPedido model)
        {
            this.ProcessData = (PDPedido)Session["Pedido"];
            try
            {
                if (model.Pedido.CodigoCliente == 0)
                {
                    return Json(new JsonReturn(false, "Selecione um cliente."));
                }

                if (this.ProcessData.Pedido.PedidoDetalhe.Count == 0)
                {
                    return Json(new JsonReturn(false, "Selecione um produto para continuar."));
                }

                if (!ValidaFimDeSemanada(model.Pedido.DataEntrega))
                {
                    return Json(new JsonReturn(false, "Data Escolhida não é possível entrega."));
                }

                if (this.ProcessData.Pedido.Desconto == null)
                {
                    this.ProcessData.ValorTotal = this.ProcessData.Pedido.PedidoDetalhe.Sum(p => p.ValorTotalProduto);
                }

                this.ProcessData.Pedido.CodigoCliente = model.Pedido.CodigoCliente;
                this.ProcessData.TotalItens = this.ProcessData.Pedido.PedidoDetalhe.Count;
                this.ProcessData.ValorRestantePagamento = this.ProcessData.ValorTotal;
                this.ProcessData.Pedido.DataEntrega = model.Pedido.DataEntrega;


                if (this.ProcessData.PopUp == 1)
                {
                    return Json(new JsonReturn
                    {
                        Success = true,
                        Mensage = "",
                        Data = new
                        {
                            Popup = this.ProcessData.PopUp
                        }
                    });
                }
                else
                {
                    return Json(new JsonReturn
                    {
                        Success = true,
                        Mensage = "Pedido aguardando complemento das informações",
                        Data = new
                            {
                                TotalItens = this.ProcessData.TotalItens,
                                ValorTotal = this.ProcessData.ValorTotal,
                                ValorRestantePagamento = this.ProcessData.ValorRestantePagamento - this.ProcessData.ValoTotalPago,
                                Popup = this.ProcessData.PopUp
                            }
                    });
                }

            }
            catch (Exception ex)
            {
                return Json(new JsonReturn(false, ex.Message));
            }
        }



        [HttpPost]
        public JsonResult SalvarAprovacao()
        {
            try
            {
                this.ProcessData = (PDPedido)Session["Pedido"];
                this.ProcessData.Pedido.StatusPedido = StatusPedido.APROVACAO;
                Session["Pedido"] = new BPFPedido().SalvarPedido(this.ProcessData);
                this.ProcessData = (PDPedido)Session["Pedido"];

                SalvarDataEntrega();


                return Json(new JsonReturn
                {
                    Success = true,
                    Mensage = "Confirme os dados do Pedido.",
                    Data = new
                    {
                        Cliente = this.ProcessData.Pedido.Cliente,
                        ValorTotal = this.ProcessData.ValorTotal,
                        ValorRestantePagamento = this.ProcessData.ValorRestantePagamento - this.ProcessData.ValoTotalPago,

                    }
                });

            }
            catch (Exception ex)
            {
                return Json(new JsonReturn(false, ex.Message));
            }
        }


        public void SalvarDataEntrega()
        {
            try
            {
                this.ProcessData = (PDPedido)Session["Pedido"];


                this.ProcessData.Agenda.AgendaEntrega.StatusAgenda = StatusAgendaEntrega.APROVADO;
                this.ProcessData.Agenda.AgendaEntrega.CodigoPedido = this.ProcessData.Pedido.Codigo;
                this.ProcessData.Agenda.AgendaEntrega.DataInicio = this.ProcessData.Pedido.DataEntrega;
                this.ProcessData.Agenda.AgendaEntrega.DataFim = this.ProcessData.Pedido.DataEntrega;
                this.ProcessData.Agenda.AgendaEntrega.CodigoLoja = UsuarioLogin.CodigoLoja;
                this.ProcessData.Agenda.AgendaEntrega.CodigoFilialLoja = (int)UsuarioLogin.CodigoFilialLoja;

                BECliente cliente = BPFCliente.Instance.ObterTodos(new FECliente { Codigo = this.ProcessData.Pedido.CodigoCliente, CodigoLoja = this.ProcessData.Pedido.CodigoLoja }).ResultList.FirstOrDefault();
                BEDadosCliente DadosCliente = BPFDadosCliente.Instance.ObterTodos(new FEDadosCliente() { CodigoCliente = cliente.Codigo }).ResultList.FirstOrDefault();

                this.ProcessData.Agenda.AgendaEntrega.Descricao = string.Format("Pedido: {0} - Cliente : {1} - CEP: {2}", this.ProcessData.Pedido.Codigo, cliente.Nome, DadosCliente.CEP);

                this.ProcessData.Agenda.AgendaEntrega = new BPFAgendaEntrega().Salvar(this.ProcessData.Agenda.AgendaEntrega);
            }
            catch (Exception ex)
            {
                base.AdicionarMensagemErro(ex.Message);
            }

        }


        [HttpPost]
        public JsonResult Finalizar()
        {
            try
            {
                this.ProcessData = (PDPedido)Session["Pedido"];
                this.ProcessData.Pedido.StatusPedido = StatusPedido.APROVACAO;
                return Json(new JsonReturn(true, "Verifique as informações do pedido", this.ProcessData));
            }
            catch (Exception ex)
            {
                return Json(new JsonReturn(false, ex.Message));
            }
        }

        #region Busca Pedido
        public JsonResult BuscarPedido(FEPedido fePedido)
        {
            fePedido.CodigoLoja = UsuarioLogin.CodigoLoja;
            fePedido.CodigoLojaFilial = (int)UsuarioLogin.CodigoFilialLoja;

            Session["FEPedido"] = fePedido;
            return Json(null);
        }

        [HttpGet]
        public JsonResult Buscar(SearchModel searchModel)
        {

            if (Session["FEPedido"] == null) Session["FEPedido"] = new FEPedido() { CodigoLoja = UsuarioLogin.CodigoLoja, CodigoLojaFilial = (int)UsuarioLogin.CodigoFilialLoja };

            this.ProcessData.ListPedido = BPFPedido.Instance.ObterTodos(Session["FEPedido"] as FEPedido).ResultList;
            Session["listItem"] = new List<BEPedido>();


            ((List<BEPedido>)Session["listItem"]).AddRange(this.ProcessData.ListPedido);

            IQueryable<BEPedido> model = ((List<BEPedido>)Session["listItem"]).AsQueryable();

            GridData gridData = model.ToGridData(searchModel, GridPedidoModel.BEPedidoColumns);
            return Json(gridData, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region Pedido Observação
        [HttpGet]
        public JsonResult ObterPedidoObservacao(int codigoPedido, SearchModel searchModel)
        {
            IQueryable<BEPedidoObservacao> model = BPFPedidoObservacao.Instance.ObterTodos(new FEPedidoObservacao() { CodigoPedido = codigoPedido }).ResultList.AsQueryable();
            GridData gridData = model.ToGridData(searchModel, GridPedidoModel.BEPedidoObservacaoColumns);

            return Json(gridData, JsonRequestBehavior.AllowGet);
        }


        public ActionResult SalvarObservacao(PDPedido model)
        {
            model.PedidoObservacao.Status = StatusPedidoObservacao.Novo;
            model.PedidoObservacao.CodigoPedido = model.Pedido.Codigo;
            model.PedidoObservacao.CodigoUsuario = UsuarioLogin.Codigo;

            this.ProcessData.PedidoObservacao = new BPFPedidoObservacao().SalvarPedidoObservacao(model.PedidoObservacao);

            BEAgendaEntrega entidade = BPFAgendaEntrega.Instance.ObterTodos(new FEAgendaEntrega() { CodigoPedido = this.ProcessData.PedidoObservacao.CodigoPedido }).ResultList.FirstOrDefault();
            entidade.StatusAgenda = StatusAgendaEntrega.OBSERVACAO;

            BPFAgendaEntrega.Instance.Salvar(entidade);

            base.AdicionarMensagemSucesso("Obeservação Adicionada com sucesso");
            if (Request.IsAjaxRequest())
                return Json(null);

            return RedirectToAction("ConsultarPedido");
        }

        public ActionResult CarregarStatusItem()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            var listMotivo = Onion.Util.EnumHelper.GetDescriptions<StatusPedidoObservacao>();

            foreach (var m in listMotivo)
                sb.Append(m.Value.ToString() + ":" + m.Name + ";");

            return Json(sb.ToString(), JsonRequestBehavior.AllowGet);
        }

        #endregion

        [HttpPost]
        public JsonResult CalculaValorParcela(decimal? VlrPagamento, int? NroParc)
        {
            decimal? valorParcela = 0;

            if (NroParc == 0)
                return Json(new JsonReturn(true, "", new { VlrParc = valorParcela.ToString() }));

            valorParcela = VlrPagamento / NroParc;

            return Json(new JsonReturn(true, "", new { VlrParc = valorParcela.ToString() }));
        }

        [HttpPost]
        public JsonResult CalculaDesconto(decimal? VlrDesc)
        {
            this.ProcessData = (PDPedido)Session["Pedido"];
                       
            decimal? VlrSubTotal = this.ProcessData.Pedido.PedidoDetalhe.Sum(p => p.ValorTotalProduto);

            decimal vlrCalc = (decimal)VlrSubTotal;

            decimal? valorDescontado = ((VlrDesc * 100) / vlrCalc);

            int? mrgVendendor = BPFVendedor.Instance.ObterTodos(new FEVendedor() { CodigoLoja = UsuarioLogin.CodigoLoja, CodigoUsuario = UsuarioLogin.Codigo }).ResultList.FirstOrDefault().MargemVenda;

            if (valorDescontado > mrgVendendor)
            {
                this.ProcessData.PopUp = 1;
            }
            else
            {
                this.ProcessData.PopUp = 0;
            }


            decimal? valorFinal = vlrCalc - VlrDesc;
            if (valorFinal.HasValue)
            {
                this.ProcessData.ValorTotal = valorFinal;
                this.ProcessData.Pedido.ValorTotal = valorFinal;
                this.ProcessData.Pedido.Desconto = 1;
                return Json(new JsonReturn(true, "", new { SubTotal = VlrSubTotal, TotalPedido = valorFinal }));
            }
            else
            {
                this.ProcessData.Pedido.Desconto = 0;
                this.ProcessData.ValorTotal = Convert.ToDecimal(VlrSubTotal);
                return Json(new JsonReturn(true, "", new { SubTotal = this.ProcessData.ValorTotal, TotalPedido = this.ProcessData.ValorTotal }));
            }


        }

        [HttpPost]
        public JsonResult ValidaGerente(string Login, string Senha)
        {
            this.ProcessData = (PDPedido)Session["Pedido"];

            BEUsuario entidade = new BEUsuario()
            {
                CodigoLoja = UsuarioLogin.CodigoLoja,
                Login = Login,
                Senha = Senha
            };

            bool IsManager = BPFUsuario.Instance.ValidaGerente(entidade);


            if (IsManager)
            {
                return Json(new JsonReturn
                {
                    Success = true,
                    Mensage = "Pedido aguardando complemento das informações",
                    Data = new
                    {
                        TotalItens = this.ProcessData.TotalItens,
                        ValorTotal = this.ProcessData.ValorTotal,
                        ValorRestantePagamento = this.ProcessData.ValorRestantePagamento - this.ProcessData.ValoTotalPago,
                    }
                });
            }
            else
            {
                return Json(new JsonReturn
                {
                    Success = false,
                    Mensage = "Usuário não pode Autorizar Desconto!",
                });
            }
        }

        private void CalculaEntrega()
        {

            BEConfiguracaoLoja entidade = BPFConfiguracaoLoja.Instance.ObterTodos(new FEConfiguracaoLoja() { CodigoLoja = UsuarioLogin.CodigoLoja }).ResultList.FirstOrDefault();

            if (entidade.EntregaMontagem == true)
            {

                DateTime dataPedido = RetornarDataSemFimDeSemana(Convert.ToDateTime(DateTime.Now.AddDays((double)entidade.QuantidadeDiaEntrega).ToString("dd/MM/yyyy")));

                DateTime data = DiaEntrega(dataPedido, (int)entidade.QuantidadeEntrega);

                this.ProcessData.Pedido.DataEntrega = data;
                this.ProcessData.Pedido.DataMontagemPedido = data;
            }
            else
            {
                this.ProcessData.Pedido.DataEntrega = DateTime.Now.AddDays((double)entidade.QuantidadeDiaEntrega);
                //TODO: Colocar campo na tabela para a montagem
                //                this.ProcessData.Pedido.DataMontagemPedido = DateTime.Now.AddDays((double)entidade.qu);
            }

        }

        private DateTime DiaEntrega(DateTime dataParaEntrega, int QuantidadeEntrega)
        {
            bool diaLiberado = false;
            int count = 1;

            while (diaLiberado == false)
            {
                int quantidade = BPFAgendaEntrega.Instance.ObterTodos(new FEAgendaEntrega() { DataInicio = dataParaEntrega, DataFim = Convert.ToDateTime(dataParaEntrega.ToString("yyyy-MM-dd 23:59:59"))  }).ResultCount;

                if (quantidade < QuantidadeEntrega)
                {
                    diaLiberado = true;
                    return RetornarDataSemFimDeSemana(dataParaEntrega);
                }
                else
                {
                    dataParaEntrega = dataParaEntrega.AddDays(count);
                    count++;
                }

            }

            return dataParaEntrega;
        }

        private DateTime RetornarDataSemFimDeSemana(DateTime dataEntrega)
        {
            DayOfWeek diasDaSemana = dataEntrega.DayOfWeek;

            if (diasDaSemana == DayOfWeek.Sunday)
            {
                dataEntrega = dataEntrega.AddDays(1);
            }
            else if (diasDaSemana == DayOfWeek.Saturday)
            {
                dataEntrega = dataEntrega.AddDays(2);
            }

            return dataEntrega;
        }

        private bool ValidaFimDeSemanada(DateTime dataEntrega)
        {
            DayOfWeek diasDaSemana = dataEntrega.DayOfWeek;

            if (diasDaSemana == DayOfWeek.Sunday || diasDaSemana == DayOfWeek.Saturday)
            {
                return false;
            }

            return true;
        }

        public JsonResult CalculaPeriodoEntrega()
        {
            BEConfiguracaoLoja entidade = BPFConfiguracaoLoja.Instance.ObterTodos(new FEConfiguracaoLoja() { CodigoLoja = UsuarioLogin.CodigoLoja }).ResultList.FirstOrDefault();

            DateTime dataPedido =  RetornarDataSemFimDeSemana(Convert.ToDateTime(DateTime.Now.AddDays((double)entidade.QuantidadeDiaEntrega).ToString("dd/MM/yyyy")));

            DateTime diaPedido = DiaEntrega(dataPedido, (int)entidade.QuantidadeEntrega);

            return Json(new JsonReturn
            {
                Data = new
                {
                    DtEnt = diaPedido.ToString(),
                    Periodo = entidade.QuantidadeDiaEntrega.ToString()
                }
            });

        }


        public ActionResult ImpressaoPedido(PDPedido model)
        {
            try
            {
                this.ProcessData = (PDPedido)Session["Pedido"];

                //this.ProcessData.Pedido.InformacaoAdicional = model.Pedido.InformacaoAdicional;

                //Session["Pedido"] = new BPFPedido().SalvarPedido(this.ProcessData);

                FEPedido fePedido = new FEPedido()
                {
                    Codigo = this.ProcessData.Pedido.Codigo,
                    CodigoLoja = UsuarioLogin.CodigoLoja,
                    CodigoLojaFilial = (int)UsuarioLogin.CodigoFilialLoja
                };

                BEPedido entidade = BPFPedido.Instance.ObterTodos(fePedido).ResultList.FirstOrDefault();
                entidade.InformacaoAdicional = model.Pedido.InformacaoAdicional;

                BPFPedido.Instance.SalvarInformacaoAdicional(entidade);

                return RedirectToAction("Index", "Relatorio", new { id = this.ProcessData.Pedido.Codigo });

            }
            catch (Exception ex)
            {
                return Json(new JsonReturn(true, ex.Message));

            }

        }

        private void CarregaCombo()
        {
            var cartaoCredito = from f in BPFCartaoCredito.Instance.ObterTodos(new FECartaoCredito() { CodigoLoja = UsuarioLogin.CodigoLoja }).ResultList
                                select new { Codigo = f.Codigo, f.Nome };

            TempData["CartaoCredito"] = new SelectList(cartaoCredito, "Codigo", "Nome");
        }

        [HttpPost]
        public ActionResult DeletarProdutoPedido(BEPedidoDetalhe pedidoDetalhe)
        {
            this.ProcessData = (PDPedido)Session["Pedido"];

            this.ProcessData.Pedido.PedidoDetalhe.RemoveAll(p => p.CodigoProduto == pedidoDetalhe.CodigoProduto);

            this.ObterValores(0);

            return Json(GridResponse.CreateSuccess(), JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}
