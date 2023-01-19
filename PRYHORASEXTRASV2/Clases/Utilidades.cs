using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace CONTROLDEINGRESOS.Clases
{
    public static class Utilidades
    {
        public static List<char> charsToRemove = new List<char>() { '@', '_', ',', '.','*','-','/','&' };
        public static string FilterRegex(this string str)
        {
             
            String chars = "[" + String.Concat(charsToRemove) + "]";
            return Regex.Replace(str, chars, String.Empty);
        }



        //método para establecer solo letras con expresión regular
        public static string CleanName(this string strIn)
        {
            try
            {
                return Regex.Replace(strIn, @"[^a-zA-ZñÑ\s\u00C0-\u017F]", "");
            }

            catch (RegexMatchTimeoutException)
            {
                return String.Empty;
            }
        }

        //método para establecer solo números con expresión regular
        public static string CleanId(this string strIn)
        {
            try
            {
                return Regex.Replace(strIn, @"[^0-9]*$", "");
            }

            catch (RegexMatchTimeoutException)
            {
                return String.Empty;
            }
        }


    }






}