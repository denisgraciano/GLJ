using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GLJ.Acesso.Business.Entity;
using System.Text;
using System.Linq.Expressions;

namespace GLJ.Models.Helpers
{
    public static class OnionHelper
    {
        /// <summary>
        /// Criar o menu superior conforme itens na base de dados
        /// </summary>
        /// <param name="helper">contexto html atual</param>
        /// <param name="views">Lista de views cadastrados no banco de dados</param>
        /// <returns></returns>
        public static MvcHtmlString Menu(this HtmlHelper helper, List<BEViewControle> views)
        {
            StringBuilder menu = new StringBuilder();
            foreach (BEViewControle item in views.Where(v => !v.CodigoViewControlePai.HasValue))
            {
                menu.AppendLine("<li>");
                menu.AppendLine(string.Format("<a href=\"{0}{1}/{2}\"><span class=\"l\"></span><span class=\"r\"></span><span class=\"t\">{3}</span></a>", Utils.Root, item.Controle, item.View, item.Descricao));
                menu.AppendLine(MontarFilho(item.Codigo, views));
                menu.AppendLine("</li>");
            }

            return MvcHtmlString.Create(menu.ToString());
        }

        /// <summary>
        /// Retornar o DysplayName para html, dentro da tag placeholder(HTML 5)
        /// </summary>
        /// <typeparam name="TModel">Tipo da Model</typeparam>
        /// <typeparam name="TValue">Propriedade da Model</typeparam>
        /// <param name="html">contexto html atua</param>
        /// <param name="expression">Expression Lambda informada na tela</param>
        /// <returns>Display Name da Propriedade informada pela expression Lambda</returns>
        public static MvcHtmlString PlaceHolder<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            string labelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();

            if (String.IsNullOrEmpty(labelText))
            {
                return MvcHtmlString.Empty;
            }

            return MvcHtmlString.Create(labelText);
        }


        /// <summary>
        /// Criar Menu superior conforme itens na base de dados(criar filhos dos menus)
        /// </summary>
        /// <param name="idPai">Código do menu pai</param>
        /// <param name="views">Lista de Views Cadastrados no banco de dados</param>
        /// <param name="strClass">Classe do css para menu, default = sub_menu</param>
        /// <returns>retorna uma lista estilizada para o menu pai</returns>
        private static String MontarFilho(int idPai, List<BEViewControle> views, string strClass = "sub_menu")
        {
            StringBuilder filho = new StringBuilder();
            List<BEViewControle> query = views.Where(v => v.CodigoViewControlePai == idPai && v.VisualizaMenu == 1).OrderBy(v => v.Ordem).ToList();
            if (query.Count > 0)
            {
                filho.AppendLine(string.Format("<ul {0}>", (!string.IsNullOrEmpty(strClass) ? "class=\"sub_menu\"" : "")));
                foreach (BEViewControle subItem in query)
                {
                    filho.AppendLine("<li>");
                    filho.AppendLine(string.Format("<a href=\"{0}{1}/{2}\">{3}</a>", Utils.Root, subItem.Controle, subItem.View, subItem.Descricao));
                    filho.AppendLine(MontarFilho(subItem.Codigo, views, string.Empty));
                    filho.AppendLine("</li>");
                }
                filho.AppendLine("</ul>");
            }

            return filho.ToString();
        }
    }
}