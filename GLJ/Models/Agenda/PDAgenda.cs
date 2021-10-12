using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GLJ.Business.Entity;

namespace GLJ.Models.Agenda
{
    public class PDAgenda : ProcessData
    {
        public int codigoStatus { get; set; }
        public BEPedido Pedido { get; set; }
        public BEAgendaEntrega AgendaEntrega { get; set; }
        public BEAgendaMontagem AgendaMontagem { get; set; }
        public List<BEPedidoDetalhe> PedidoDetalhe { get; set; }
        public List<BEPedidoObservacao> PedidoObservacao { get; set; }

        public PDAgenda()
        {
            AgendaEntrega = new BEAgendaEntrega();
            AgendaMontagem = new BEAgendaMontagem();
            Pedido = new BEPedido();
            PedidoDetalhe = new List<BEPedidoDetalhe>();
            PedidoObservacao = new List<BEPedidoObservacao>();
        }
    }
}