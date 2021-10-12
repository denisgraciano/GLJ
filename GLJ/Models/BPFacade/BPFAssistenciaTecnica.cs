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
    public class BPFAssistenciaTecnica : Singleton<BPAssistenciaTecnica>
    {
        internal BEAssistenciaTecnica Salvar(BEAssistenciaTecnica beAssistenciaTecnica, List<BEAssistenciaTecnicaDetalhe> ListAssistenciaTecnicaDetalhe)
        {
            if (beAssistenciaTecnica.Codigo > 0)
            {

                BEAssistenciaTecnica beAssistenciaTecnicaOld = Instance.ObterTodos(new FEAssistenciaTecnica() { Codigo = beAssistenciaTecnica.Codigo, CodigoLoja = beAssistenciaTecnica.CodigoLoja, CodigoFilialLoja = (int)beAssistenciaTecnica.CodigoFilialLoja }).ResultList.FirstOrDefault();
                beAssistenciaTecnica = (BEAssistenciaTecnica)beAssistenciaTecnicaOld.AlterProperties(beAssistenciaTecnica);
            }
            return Instance.SalvarAssistencia(beAssistenciaTecnica, ListAssistenciaTecnicaDetalhe);
        }

        internal BEAssistenciaTecnica AlterarStatus(BEAssistenciaTecnica beAssistenciaTecnica)
        {
            StatusAssistencia status = beAssistenciaTecnica.Status;

            FEAssistenciaTecnica filtro = new FEAssistenciaTecnica()
            {
                Codigo = beAssistenciaTecnica.Codigo,
                CodigoFilialLoja = beAssistenciaTecnica.CodigoFilialLoja,
                CodigoLoja = beAssistenciaTecnica.CodigoLoja
            };

            beAssistenciaTecnica = Instance.ObterTodos(filtro).ResultList.FirstOrDefault();

            beAssistenciaTecnica.Status = status;

            return Instance.Salvar(beAssistenciaTecnica);
        }
    }
}