using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Onion.Business;
using GLJ.Acesso.Business.Process;
using GLJ.Acesso.Business.Entity;
using GLJ.Acesso.Filter.Entity;

namespace GLJ.Models.BPFacade
{
    public class BPFGrupoPermissaoViewControle : Singleton<BPGrupoPermissaoViewControle>
    {
        public BEGrupoPermissaoViewControle SalvarGrupoPermissaoViewControle(BEGrupoPermissaoViewControle beGrupoPermissaoViewControle)
        {
            if (beGrupoPermissaoViewControle.Codigo > 0)
            {
                BEGrupoPermissaoViewControle beGrupoPermissaoViewControleOld = Instance.ObterTodos(new FEGrupoPermissaoViewControle() { Codigo = beGrupoPermissaoViewControle.Codigo }).ResultList.FirstOrDefault();
                beGrupoPermissaoViewControle = (BEGrupoPermissaoViewControle)beGrupoPermissaoViewControleOld.AlterProperties(beGrupoPermissaoViewControle);
            }
            return Instance.Salvar(beGrupoPermissaoViewControle);
        }
    }
}