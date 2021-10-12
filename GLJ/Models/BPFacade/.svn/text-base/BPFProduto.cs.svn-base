using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Onion.Business;
using GLJ.Business.Process;
using GLJ.Filter.Entity;
using GLJ.Business.Entity;

namespace GLJ.Models.BPFacade
{
    public class BPFProduto : Singleton<BPProduto>
    {
        internal BEProduto Salvar(BEProduto beProduto)
        {
            if (beProduto.Codigo > 0)
            {

                BEProduto beProdutoOld = Instance.ObterTodos(new FEProduto() { Codigo = beProduto.Codigo, CodigoLoja = beProduto.CodigoLoja }).ResultList.FirstOrDefault();
                beProduto = (BEProduto)beProdutoOld.AlterProperties(beProduto);
            }
            return Instance.Salvar(beProduto);
        }
    }
}