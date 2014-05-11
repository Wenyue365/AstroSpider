using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace AstroSpider
{
    class SinaTemperament
    {
        WebClient m_wclient = new WebClient();
        string m_targetUrl = "";
        
        public void Start()
        {
            string strHtml = m_wclient.DownloadString(m_targetUrl);

            if (parserTemperament(strHtml))
            {

            }
            else
            {

            }

        }

        private bool parserTemperament(string strHtml)
        {
            throw new NotImplementedException();
        }
    }
}
