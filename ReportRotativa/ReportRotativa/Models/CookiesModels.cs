/* 
 * Classe para Gravar e Ler  Cookies
 * 
 * Autor : Retirada da Internet.
 * 
 * Visite nossa página http://www.codigoexpresso.com.br
 *     
 * Chamadas da Classe 
 * 
 *     Gravar um Cookie    => Set(key, Valor)
 *     Recuperar um Cookie => Get(key)
 *               Retorna uma string com o Cookie gravado com a chave indicada.
 *     Verificar se um Cookie existe => Exists(key)
 *               Retorna True se existe e false não existe.
 *     Apagar Cookie       => Delete(key)
 *                Apaga o Cookie com a chave indicada.
 *                         => DeleteAll()
 *                Apaga todos.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

public static class CookiesModels
{
    private const string CookieSetting = "Cookie.Duracao";
    private const string CookieIsHttp = "Cookie.Tipo_V_F";
    private static HttpContext _context { get { return HttpContext.Current; } }
    private static decimal _cookieDuration { get; set; }
    private static bool _cookieIsHttp { get; set; }

    static CookiesModels()
    {
        _cookieDuration = GetCookieDuration();
        _cookieIsHttp = GetCookieType();
    }

    public static void Set(string key, string value)
    {
        var c = new HttpCookie(key)
        {
            Value = value,
            Expires = DateTime.Now.AddMinutes(Convert.ToDouble(_cookieDuration)),
            HttpOnly = _cookieIsHttp
        };
        _context.Response.Cookies.Add(c);
    }

    public static string Get(string key)
    {
        var value = string.Empty;

        var c = _context.Request.Cookies[key];
        return c != null
                ? _context.Server.HtmlEncode(c.Value).Trim()
                : value;
    }

    public static bool Exists(string key)
    {
        return _context.Request.Cookies[key] != null;
    }

    public static void Delete(string key, string value)
    {
        var c = new HttpCookie(key)
        {
            Value = value,
            Expires = DateTime.Now.AddMilliseconds(0.01),
            HttpOnly = _cookieIsHttp
        };
        _context.Response.Cookies.Add(c);

    }

    public static void Atualiza(string key, string value)
    {
        var c = new HttpCookie(key)
        {
            Value = value,
            Expires = DateTime.Now.AddMinutes(20),
            HttpOnly = _cookieIsHttp
        };
        _context.Response.Cookies.Add(c);
    }

    private static decimal GetCookieDuration()
    {
        //default
        decimal duration = 0;
        decimal duration2 = 0;
        var setting = ConfigurationManager.AppSettings[CookieSetting];
        duration = Convert.ToDecimal(setting);

        if (!string.IsNullOrEmpty(setting))
        {
            duration2 = duration;
        }

        return duration2;
    }
    private static bool GetCookieType()
    {
        //default
        var isHttp = true;
        var setting = ConfigurationManager.AppSettings[CookieIsHttp];

        if (!string.IsNullOrEmpty(setting))
            bool.TryParse(setting, out isHttp);

        return isHttp;
    }
}