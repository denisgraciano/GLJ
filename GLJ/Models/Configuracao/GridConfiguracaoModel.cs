using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVC.Controls.Grid;
using GLJ.Business.Entity;

namespace GLJ.Models.Configuracao
{
    public class GridConfiguracaoModel
    {
        public static GridColumnModelList<BEConfiguracaoLoja> BEConfiguracaoColumns
        {
            get
            {
                return CreateMotivoColumns();
            }
        }
        private static GridColumnModelList<BEConfiguracaoLoja> CreateMotivoColumns()
        {
            GridColumnModelList<BEConfiguracaoLoja> cn = new GridColumnModelList<BEConfiguracaoLoja>();
            cn.Add(p => p.Codigo).SetAsPrimaryKey().SetHidden(true);
            cn.Add(p => p.Loja.Nome);
            cn.Add(p => p.QuantidadeDiaEntrega);
            cn.Add(p => p.QuantidadeEntrega);
            cn.Add(p => p.EntregaMontagem).SetFormatter("'checkbox'");
            cn.Add(p => p.TrackingState).SetHidden(true);
            return cn;
        }
    }
}