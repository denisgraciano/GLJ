using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GLJ.Acesso.Business.Entity;
using GLJ.Models;

namespace GLJ.Controllers
{
    public class AuthorizeOnionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string View = filterContext.ActionDescriptor.ActionName;
            string controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

            BEUsuario beUsuario = filterContext.HttpContext.Session["UsuarioLogin"] as BEUsuario;

            if (beUsuario != null)
            {
                var query = beUsuario.ViewsController.Where(v => v.View.Equals(View) && v.Controle.Equals(controller));

                if (query.Count() == 0)
                {
                    this.RedirectLogin(filterContext);
                }
                else
                {
                    base.OnActionExecuting(filterContext);
                }
            }
            else
            {
                this.RedirectLogin(filterContext);
            }
        }

        private void RedirectLogin(ActionExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Redirect(string.Format("{0}{1}", Utils.Root, "Login"));
        }
    }
}