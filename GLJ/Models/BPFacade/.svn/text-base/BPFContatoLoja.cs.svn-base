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
    public class BPFContatoLoja : Singleton<BPContatoLoja>
    {
        internal BEContatoLoja SalvarItemProduto(BEContatoLoja beContatoLoja)
        {
            if (beContatoLoja.Codigo > 0)
            {
                BEContatoLoja beProdutoItemOld = Instance.ObterTodos(new FEContatoLoja() { Codigo = beContatoLoja.Codigo }).ResultList.FirstOrDefault();
                beContatoLoja = (BEContatoLoja)beProdutoItemOld.AlterProperties(beContatoLoja);
            }
            return Instance.Salvar(beContatoLoja);
        }
    }
}