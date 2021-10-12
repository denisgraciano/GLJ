using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Relatorio.Filter.Entity;

namespace GLJ.Models.Relatorio
{
    public class PDRelatorio : ProcessData
    {
        public FEEspelhoPedido FEEspelhoPedido { get; set; }
        public FEDadosLoja FEDadosLoja { get; set; }
        public FEEspelhoPedidoDetalhe FEEspelhoPedidoDetalhe { get; set; }
        public FEEspelhoPedidoPagamento FEEspelhoPedidoPagamento { get; set; }
        public PDRelatorio()
        {
            this.FEEspelhoPedido = new FEEspelhoPedido();
            this.FEDadosLoja = new FEDadosLoja();
            this.FEEspelhoPedidoDetalhe = new FEEspelhoPedidoDetalhe();
            this.FEEspelhoPedidoPagamento = new FEEspelhoPedidoPagamento();
        }
    }
}