using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GLJ.Business.Entity;
using System.ComponentModel;
using GLJ.Filter.Entity;
using GLJ.Models.Agenda;
using GLJ.Acesso.Business.Entity;

namespace GLJ.Models.Pedido
{
    public class PDPedido : ProcessData
    {
        [DisplayName("Total de Itens :")]
        public Int32 TotalItens { get; set; }

        [DisplayName("Valor Total do Pedido :")]
        public Decimal? ValorTotal { get; set; }
        
        [DisplayName("Valor Restante para Pagamento :")]
        public Decimal? ValorRestantePagamento { get; set; }

        [DisplayName("Valor Restante para Pagamento :")]
        public Decimal? ValoTotalPago { get; set; }

        public Decimal? ValoDesconto { get; set; }

        public DateTime DataEnt {get; set;}

        public int PopUp { get; set; }

        public BEUsuario Usuario { get; set; }

        private BEPedido _Pedido;
        public BEPedido Pedido {
            get { return _Pedido; }
            set { 
                _Pedido = value;
                Agenda.Pedido = value;
            }
        }

        public BEPagamentoPedido PagamentoPedido { get; set; }
        public BEPagamentoCartao PagamentoCartao { get; set; }
        public BEPagamentoCheque PagamentoCheque { get; set; }
        public BEPagamentoDinheiro PagamentoDinheiro { get; set; }
        public BEPagamentoDebito PagamentoDebito { get; set; }
        public List<BEPagamentoPedido> PagamentosPedido { get; set; }
        public FECliente FECliente { get; set; }
        public PDAgenda Agenda { get; set; }
        public FEPedido FEPedido { get; set; }
        public List<BEPedido> ListPedido { get; set; }
        public BEPedidoObservacao PedidoObservacao { get; set; }
        public FEProduto FEProduto { get; set; }

        public PDPedido()
        {
            PagamentosPedido = new List<BEPagamentoPedido>();
            PagamentoCartao = new BEPagamentoCartao();
            PagamentoCheque = new BEPagamentoCheque();
            PagamentoDinheiro = new BEPagamentoDinheiro();
            PagamentoDebito = new BEPagamentoDebito();
            FECliente = new FECliente();
            Agenda = new PDAgenda();
            //Tem que ser intanciado depois de agenda.
            Pedido = new BEPedido();
            FEPedido = new FEPedido();
            ListPedido = new List<BEPedido>();
            PedidoObservacao = new BEPedidoObservacao();
            FEProduto = new FEProduto();
            Usuario = new BEUsuario();
        }

        #region Métodos da Process Data
        
        /// <summary>
        /// Adicionar Pagamento Cartão
        /// </summary>
        /// <param name="bePagamentoCartao"></param>
        public void AdicionarPagamentoCartao(BEPagamentoCartao bePagamentoCartao)
        {
            //Adicionar Pagamento
            AdicionarPagamento(TipoPagamento.CARTAOCREDITO);

            //Somar Pagamento
            this.PagamentosPedido.SingleOrDefault(p => p.TipoPagamento == TipoPagamento.CARTAOCREDITO).PagamentosCartao.Add(bePagamentoCartao);
            this.PagamentosPedido.SingleOrDefault(p => p.TipoPagamento == TipoPagamento.CARTAOCREDITO).ValorTotalPagamento =
                this.PagamentosPedido.SingleOrDefault(p => p.TipoPagamento == TipoPagamento.CARTAOCREDITO).PagamentosCartao.Sum(c => c.ValorPagamento);

            AtualizarValorRestantePagamento();
        }

        /// <summary>
        /// Adicionar Pagamento Cartão de Débito
        /// </summary>
        /// <param name="bePagamentoDebito"></param>
        public void AdicionarPagamentoDebito(BEPagamentoDebito bePagamentoDebito)
        {
            //Adicionar Pagamento
            AdicionarPagamento(TipoPagamento.CARTAODEBITO);

            //Somar Pagamento
            this.PagamentosPedido.SingleOrDefault(p => p.TipoPagamento == TipoPagamento.CARTAODEBITO).PagamentosDebito.Add(bePagamentoDebito);
            this.PagamentosPedido.SingleOrDefault(p => p.TipoPagamento == TipoPagamento.CARTAODEBITO).ValorTotalPagamento =
                this.PagamentosPedido.SingleOrDefault(p => p.TipoPagamento == TipoPagamento.CARTAODEBITO).PagamentosDebito.Sum(c => c.ValorPagamento);

            AtualizarValorRestantePagamento();
        }

        /// <summary>
        /// Adicionar Pagamento Cheque
        /// </summary>
        /// <param name="bePagamentoCheque"></param>
        public void AdicionarPagamentoCheque(BEPagamentoCheque bePagamentoCheque)
        {
            TipoPagamento tipo = TipoPagamento.CHEQUE;
            //Adicionar Pagamento
            AdicionarPagamento(tipo);

            //Somar Pagamento
            this.PagamentosPedido.SingleOrDefault(p => p.TipoPagamento == tipo).PagamentosCheque.Add(bePagamentoCheque);
            this.PagamentosPedido.SingleOrDefault(p => p.TipoPagamento == tipo).ValorTotalPagamento =
                this.PagamentosPedido.SingleOrDefault(p => p.TipoPagamento == tipo).PagamentosCheque.Sum(c => c.ValorPagamento);

            AtualizarValorRestantePagamento();
        }

        /// <summary>
        /// Adicionar Pagamento Dinheiro
        /// </summary>
        /// <param name="bePagamentoDinhero"></param>
        public void AdicionarPagamentoDinheiro(BEPagamentoDinheiro bePagamentoDinheiro)
        {
            TipoPagamento tipo = TipoPagamento.DINHEIRO;
            //Adicionar Pagamento
            AdicionarPagamento(tipo);

            //Somar Pagamento  
            this.PagamentosPedido.SingleOrDefault(p => p.TipoPagamento == tipo).PagamentosDinheiro = bePagamentoDinheiro;
            this.PagamentosPedido.SingleOrDefault(p => p.TipoPagamento == tipo).ValorTotalPagamento =
                this.PagamentosPedido.SingleOrDefault(p => p.TipoPagamento == tipo).ValorTotalPagamento + bePagamentoDinheiro.ValorPagamento;
                

            AtualizarValorRestantePagamento();
        }


        /// <summary>
        /// Adiconar Pagamento
        /// </summary>
        /// <param name="tipoPagamento"></param>
        private void AdicionarPagamento(TipoPagamento tipoPagamento)
        {
            if (this.PagamentosPedido.FirstOrDefault(p=>p.TipoPagamento == tipoPagamento) == null)
            {
                this.PagamentosPedido.Add(new BEPagamentoPedido
                {
                    CodigoPedido = this.Pedido.Codigo,
                    DataPagamento = DateTime.Now,
                    TipoPagamento = tipoPagamento,
                });
            }
        }

        public void RemoverPagamento(BEPagamentoPedido pagamentoPedido)
        {
            this.PagamentosPedido.Remove(pagamentoPedido);

            AtualizarValorRestantePagamento();
        }

        private void AtualizarValorRestantePagamento()
        {
            decimal? valorPago = this.PagamentosPedido.Sum(v => v.ValorTotalPagamento);
            this.ValorRestantePagamento = this.ValorTotal - valorPago;
        }

        #endregion
    }
}