using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GLJ.Business.Process;
using Onion.Business;
using GLJ.Business.Entity;
using GLJ.Models.Pedido;
using GLJ.Filter.Entity;

namespace GLJ.Models.BPFacade
{
    public class BPFPedido : Singleton<BPPedido>
    {
        internal PDPedido ObterPedido(FEPedido fePedido)
        {
            PDPedido pedido = new PDPedido();
            pedido.Pedido = Instance.ObterTodos(fePedido).ResultList.SingleOrDefault();
            return pedido;
        }

        internal PDPedido SalvarPedido(PDPedido pedido)
        {
            //Alterar Pedido
            if (pedido.Pedido.Codigo > 0)
            {
                BEPedido bePedidoOld = Instance.ObterTodos(new FEPedido() { Codigo = pedido.Pedido.Codigo, CodigoCliente = pedido.Pedido.CodigoCliente, CodigoLoja = pedido.Pedido.CodigoLoja, CodigoLojaFilial = pedido.Pedido.CodigoFilialLoja }).ResultList.FirstOrDefault();
                pedido.Pedido = (BEPedido)bePedidoOld.AlterProperties(pedido.Pedido);
            }

            pedido.Pedido = Instance.Salvar(pedido.Pedido, pedido.PagamentosPedido);

            return pedido;
        }
    }
}