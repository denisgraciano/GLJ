using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GLJ.Business.Process;
using Onion.Business;
using GLJ.Business.Entity;
using GLJ.Filter.Entity;

namespace GLJ.Models.BPFacade
{
    public class BPFPedidoObservacao : Singleton<BPPedidoObservacao>
    {
        internal BEPedidoObservacao SalvarPedidoObservacao(BEPedidoObservacao bePedidoObservacao)
        {
            if (bePedidoObservacao.Codigo > 0)
            {
                BEPedidoObservacao beFornecedorOld = Instance.ObterTodos(new FEPedidoObservacao() { Codigo = bePedidoObservacao.Codigo }).ResultList.FirstOrDefault();
                bePedidoObservacao = (BEPedidoObservacao)beFornecedorOld.AlterProperties(bePedidoObservacao);
            }
            return Instance.Salvar(bePedidoObservacao);
        }

    }
}