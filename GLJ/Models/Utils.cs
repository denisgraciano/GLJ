using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Text.RegularExpressions;

namespace GLJ.Models
{
    public class Utils
    {
        public static String AplicationPathName
        {
            get { return "GLJ"; }
        }
        /// <summary>
        /// Propriedade que retorna o separador padrão das url's.
        /// </summary>
        public static String Slash
        {
            get
            {
                String _strUrl = string.Empty;
                Regex regex = new Regex(@"^(/){1}[\w]+$");

                if (regex.IsMatch(HttpContext.Current.Request.ApplicationPath))
                {
                    _strUrl = "/";
                }
                return _strUrl;
            }
        }

        /// <summary>
        /// Propriedade que retorna a porta utilizada pelo protocolo da aplicação.
        /// </summary>
        public static String Port
        {
            get
            {
                String _strPort = (HttpContext.Current.Request.Url.Port.ToString() == "80" ? String.Empty : (":" + HttpContext.Current.Request.Url.Port.ToString()));
                return _strPort;
            }
        }

        /// <summary>
        /// Propriedade que retorna o caminho Root (Domínio) da aplicação.
        /// </summary>
        public static String Root
        {
            get
            {
                String _strRoot = (HttpContext.Current.Request.Url.Scheme + "://") + HttpContext.Current.Request.Url.Host + Port + HttpContext.Current.Request.ApplicationPath + Slash;
                return _strRoot;
            }
        }

        public static String MapPageDictionary(String caminho)
        {

            String siteMap = ConfigurationManager.AppSettings[caminho];
            return (!string.IsNullOrEmpty(siteMap) ? siteMap : caminho);
        }

        public static List<String> Views()
        {
            int appCount = ConfigurationManager.AppSettings.Count;

            List<String> lst = new List<String>();
            for (int i = 3; i < appCount; i++)
            {
                lst.Add(ConfigurationManager.AppSettings[i].ToString());
            }

            return lst;
        }

    }
}