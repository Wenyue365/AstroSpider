using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace AstroSpider
{
   
    public class ZHtmlParser
    {
        // htmlDcoument 对象用来访问 Html文档s
        HtmlAgilityPack.HtmlDocument m_htmlDoc = new HtmlAgilityPack.HtmlDocument();

        public ZHtmlParser(string strHtml)
        {
            load(strHtml);
        }
        public void load(string strHtml)
        {
            // 加载Html文档
            m_htmlDoc.LoadHtml(strHtml);
        }

        static int ierr = 0;

        public string getValue(string xpath)
        {
            string str = null;
            if (xpath != null && xpath != "")
            {
                HtmlNode node = m_htmlDoc.DocumentNode.SelectSingleNode(xpath);
                if (node != null) { 
                    // str = node.OuterHtml;
                    str = node.InnerText;
                    str = str.Trim();
                }
                else
                {
                    str = string.Format("error_{0}", ierr++);
                }
            }

            return str;
        }

        private string removeScript(string strHtml)
        {
            const string script_start = "<script";
            const string script_end = "</script>";

            int start = 0;
            int end = 0;

            do{
                start = strHtml.IndexOf(script_start);
                if (start > 0)
                {
                    end = strHtml.IndexOf(script_end, start);

                    if (end > start)
                    {
                        strHtml = strHtml.Remove(start, end - start + script_end.Length);
                    }
                }
            }while(start > 0 && end  > start);

            return strHtml;
        }
    }

    public class Entry
    {
        public Entry(ZHtmlParser pr)
        {
            _parser = pr;
        }

        public Entry()
        {

        }

        private ZHtmlParser _parser = null;

        virtual protected string parseValue()
        {
            return _parser.getValue(_xpath);
        }

        public ZHtmlParser Parser
        {
            get { return _parser; }
            set { _parser = value;
                if (_parser != null) // 设置 ZHtmlParser 对象的时候，重新提取 val  
                {
                    val = parseValue();
                }
            }
        }

        public string val;
        private string _xpath;

        public string xpath
        {
            get { return _xpath; }
            set
            {
                _xpath = value;
                if (_parser != null) // 设置 xpath 的时候，重新提取 val  
                {
                    val = parseValue();
                }
            }
        }
    }

}
