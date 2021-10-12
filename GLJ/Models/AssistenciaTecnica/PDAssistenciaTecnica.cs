using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GLJ.Business.Entity;
using GLJ.Filter.Entity;
using Onion.Util;

namespace GLJ.Models.AssistenciaTecnica
{
    public class PDAssistenciaTecnica : ProcessData
    {
        public BEAssistenciaTecnica AssistenciaTecnica { get; set; }
        public List<BEAssistenciaTecnica> ListAssistenciaTecnica { get; set; }
        public FEAssistenciaTecnica FEAssistenciaTecnica { get; set; }
        public FEPedido FEPedido { get; set; }
        public List<BEPedido> ListPedido { get; set; }
        public FEProduto FEProduto { get; set; }
        public List<BEAssistenciaTecnicaDetalhe> AssistenciaTecnicaDetalhe { get; set; }
        public BEAssistenciaTecnicaDetalhe BEAssistenciaTecnicaDetalhe { get; set; }
        public string Motivo { get; set; }
        public PDAssistenciaTecnica()
        {
            this.AssistenciaTecnica = new BEAssistenciaTecnica();
            this.ListAssistenciaTecnica = new List<BEAssistenciaTecnica>();
            this.FEAssistenciaTecnica = new FEAssistenciaTecnica();
            this.FEPedido = new FEPedido();
            this.ListPedido = new List<BEPedido>();
            this.FEProduto = new FEProduto();
            this.AssistenciaTecnicaDetalhe = new List<BEAssistenciaTecnicaDetalhe>();
            this.BEAssistenciaTecnicaDetalhe = new BEAssistenciaTecnicaDetalhe();
        }


        public static String GetStatusDescricaoItem(String value)
        {
            Int32 enumValue;
            if (Int32.TryParse(value, out enumValue))
            {
                return EnumHelper.GetDescription<StatusAssistencia>(enumValue);
            }
            else
            {
                return value;
            }
        }

    }

}