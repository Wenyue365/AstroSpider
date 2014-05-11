using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.XPath;
using System.Xml;
using System.Diagnostics;
using HtmlAgilityPack;

namespace AstroSpider
{
       
    class BookEntry
    {
        public BookEntry()
        {

        }

        public BookEntry(ZHtmlParser pr)
        {
            _parser = pr;
        }

        public Entry name = new Entry();
        public Entry summary = new Entry();
        public Entry description = new Entry();

        private ZHtmlParser _parser = null;
        public ZHtmlParser Parser
        {
            get { return _parser; }
            set
            {
                _parser = value;
                name.Parser = _parser;
                summary.Parser = _parser;
                description.Parser = _parser;
            }
        }
    }
    class SinaBirthdayBook
    {
#if DEBUG
        const string _BIRTHDAYBOOK_DIR_ = @"C:\Temp\BirthdayBook";
#else
        const string _BIRTHDAYBOOK_DIR_ = "BirthdayBook";
#endif

        const int m_startPage = 3083;
        const int m_endPage = 3448;

        WebClient m_wclient = new WebClient();
        String m_url = "http://astro.sina.com.cn/jian/{0}.shtml";
    
        String m_strHtml;
        String m_tmp_filename;

        public SinaBirthdayBook()
        {
            if (!Directory.Exists(_BIRTHDAYBOOK_DIR_))
            {
                Directory.CreateDirectory(_BIRTHDAYBOOK_DIR_);
            }
        }

        public String Url
        {
            get { return m_url; }
        }

        public bool Go()
        {
            bool r = false;

            readWebData();

            //parseWebData();

            //saveData();

            return r;
        }

        private void saveData()
        {
        }

        private void readWebData()
        {
            int page_count = m_endPage - m_startPage + 1;

            for (int i = 0; i < page_count; i++)
            {
                
                m_strHtml = m_wclient.DownloadString(getBirthdaybookUrl(i));

                string tmp_filename = getBBFileName(i);

                FileStream fs = new FileStream(tmp_filename, FileMode.CreateNew);
                StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("UTF-8"));

                sw.Write(m_strHtml);

                sw.Close();
                fs.Close();
            }

            // m_tmp_filename = tmp_filename;
        }

        private string getBBFileName(int pageIdx)
        {
            // return string.Format("{0}\\{1}.{2}", _BIRTHDAYBOOK_DIR_, i, Path.GetFileName(Path.GetTempFileName()));

            string filename = null;

            if (m_strHtml != null && m_strHtml != "")
            {
                ZHtmlParser pr = new ZHtmlParser(m_strHtml);
                Entry entry = new Entry(pr);
                entry.xpath = "//*[@id=\"wrap\"]/h3/text()";

                filename = string.Format("{0}\\Bibok_{1}.{2}", _BIRTHDAYBOOK_DIR_, entry.val, "html");
            }
            else
            {
                filename = string.Format("{0}\\Bibok_{1}.{2}", _BIRTHDAYBOOK_DIR_, pageIdx, "html");
            }

            return filename;
        }

        private string getBirthdaybookUrl(int pageIdx)
        {
            string url = string.Format(m_url, m_startPage + pageIdx);
            return url;
        }

        private string readTmpData(string tmpFilename)
        {
            string strData = "";

            FileStream fs = new FileStream(tmpFilename, FileMode.Open);
            if (fs != null)
            {
                StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("UTF-8"));
                if (sr != null)
                {
                    strData = sr.ReadToEnd();
                }
            }

            return strData;
        }

        private void parseWebData()
        {
            string strHtml = readTmpData(m_tmp_filename);

            if (strHtml != null && strHtml != "")
            {
                ZHtmlParser htmlParser = new ZHtmlParser(strHtml);

                BookEntry Characters = new BookEntry();
                Characters.Parser = htmlParser;
                Characters.name.xpath = "//*[@id=\"wrap\"]/p[1]/text()";
                Characters.summary.xpath = "//*[@id=\"wrap\"]/p[2]/text()";
                Characters.description.xpath = "//*[@id=\"wrap\"]/p[3]/text()";

                BookEntry Affection = new BookEntry();
                Affection.Parser = htmlParser;
                Affection.name.xpath = "//*[@id=\"wrap\"]/p[5]/text()";
                Affection.summary.xpath = "//*[@id=\"wrap\"]/p[6]/text()";
                Affection.description.xpath = "//*[@id=\"wrap\"]/p[7]/text()";

                BookEntry MoneyLuck = new BookEntry();
                MoneyLuck.Parser = htmlParser;
                MoneyLuck.name.xpath = "//*[@id=\"wrap\"]/p[8]/text()";
                MoneyLuck.summary.xpath = "//*[@id=\"wrap\"]/p[9]/text()";
                MoneyLuck.description.xpath = "//*[@id=\"wrap\"]/p[10]/text()";

                BookEntry CareerLuck = new BookEntry();
                CareerLuck.Parser = htmlParser;
                CareerLuck.name.xpath = "//*[@id=\"wrap\"]/p[11]/text()";
                CareerLuck.summary.xpath = "//*[@id=\"wrap\"]/p[12]/text()";
                CareerLuck.description.xpath = "//*[@id=\"wrap\"]/p[13]/text()";

                BookEntry HealthLuck = new BookEntry();
                HealthLuck.Parser = htmlParser;
                HealthLuck.name.xpath = "//*[@id=\"wrap\"]/p[14]/text()";
                HealthLuck.summary.xpath = "//*[@id=\"wrap\"]/p[15]/text()";
                HealthLuck.description.xpath = "//*[@id=\"wrap\"]/p[16]/text()";

                BookEntry LuckyNumber = new BookEntry();
                LuckyNumber.Parser = htmlParser;
                LuckyNumber.name.xpath = "//*[@id=\"wrap\"]/p[17]/text()";
                LuckyNumber.summary.xpath = "//*[@id=\"wrap\"]/p[18]/text()";
                LuckyNumber.description.xpath = "//*[@id=\"wrap\"]/p[19]/text()";

                BookEntry MatchLover = new BookEntry();
                MatchLover.Parser = htmlParser;
                MatchLover.name.xpath = "//*[@id=\"wrap\"]/p[20]/text()";
                MatchLover.summary.xpath = "";
                MatchLover.description.xpath = "//*[@id=\"wrap\"]/p[22]/text()";

                BookEntry MatchFriend = new BookEntry();
                MatchFriend.Parser = htmlParser;
                MatchFriend.name.xpath = "//*[@id=\"wrap\"]/p[22]/text()";
                MatchFriend.summary.xpath = "";
                MatchFriend.description.xpath = "//*[@id=\"wrap\"]/p[23]/text()";
            }
        }

    }
}
