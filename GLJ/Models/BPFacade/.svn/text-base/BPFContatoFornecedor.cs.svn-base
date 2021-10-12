using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Onion.Business;
using GLJ.Business.Process;
using GLJ.Business.Entity;
using GLJ.Filter.Entity;

namespace GLJ.Models.BPFacade
{
    public class BPFContatoFornecedor : Singleton<BPContatoFornecedor>
    {
        internal BEContatoFornecedor SalvarContatoFornecedor(BEContatoFornecedor beContatoFornecedor)
        {
            if (beContatoFornecedor.Codigo > 0)
            {
                BEContatoFornecedor beFornecedorOld = Instance.ObterTodos(new FEContatoFornecedor() { Codigo = beContatoFornecedor.Codigo }).ResultList.FirstOrDefault();
                beContatoFornecedor = (BEContatoFornecedor)beFornecedorOld.AlterProperties(beContatoFornecedor);
            }
            return Instance.Salvar(beContatoFornecedor);
        }
    }
}