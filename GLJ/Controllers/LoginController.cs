using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GLJ.Acesso.Business.Entity;
using GLJ.Models.BPFacade;
using GLJ.Models.Login;
using GLJ.Filter.Entity;
using GLJ.Acesso.Filter.Entity;

namespace GLJ.Controllers
{
    public class LoginController : BaseController
    {
        public ActionResult Index()
        {
            base.UsuarioLogin = null;
            return View();
        }

        public ActionResult LogOn(PDLogin beUsuario)
        {
            try
            {
                //Autentico
                BEUsuario beUsrLogado = BPFUsuario.Instance.Autenticar(beUsuario.Login);

                

                if (beUsrLogado != null && beUsrLogado.IsAuthenticated)
                {
                    base.UsuarioLogin = beUsrLogado;

                    if (beUsrLogado.ExpiraSenha)
                    {
                       return RedirectToAction("AlteraSenha");
                    }
                    else
                    {
                        return RedirectToAction("UsuarioFilial");
                    }

                }
                else
                {
                    base.AdicionarMensagemErro("Usuário e/ou Senha inválidos.");
                    return View("Index");
                }
            }
            catch (Exception ex)
            {
                base.AdicionarMensagemErro("Falha ao autenticar o usuário, tentar novamente mais tarde.Mudou" + ex.Message);
                return View("Index");
            }
           
        }

        public ActionResult LogOff()
        {
            base.UsuarioLogin = null;
            return RedirectToAction("Index");
        }

        public ActionResult UsuarioFilial()
        {
            this.CarregaComboFilial();
            return View(this.ProcessData);
        }

        public ActionResult AcessaFilial(PDLogin UsuarioFilial)
        {
            if (UsuarioFilial.UsuarioLogin.CodigoFilialLoja > 0)
            {
                base.UsuarioLogin.CodigoFilialLoja = UsuarioFilial.UsuarioLogin.CodigoFilialLoja;
                return RedirectToAction(UsuarioLogin.ViewsController[0].View, UsuarioLogin.ViewsController[0].Controle);
            }
            else
            {
                base.AdicionarMensagemErro("Favor Selecionar uma filial.");
                return RedirectToAction("UsuarioFilial");
            }
            
        }

        public ActionResult AlteraSenha()
        {
            return View(this.ProcessData);
        }

        public ActionResult SalvaSenha(PDLogin Usuario)
        {
            base.UsuarioLogin.ExpiraSenha = false;
            base.UsuarioLogin.Senha = Usuario.Login.Senha;
            BPFUsuario.Instance.Salvar(base.UsuarioLogin);

            base.AdicionarMensagemSucesso("Senha Alterada Com sucesso!");

            base.UsuarioLogin = null;
            return RedirectToAction("Index");
        }


        public void CarregaComboFilial()
        {
            var filiais = from f in BPFUsuarioFilialLoja.Instance.ObterTodos(new FEUsuarioFilialLoja() { CodigoUsuario = UsuarioLogin.Codigo }).ResultList
                        select new { CodigoFilial = f.CodigoFilialLoja, f.FilialLoja.Nome};

            TempData["Filiais"] = new SelectList(filiais, "CodigoFilial", "Nome");

        }

    }
}
