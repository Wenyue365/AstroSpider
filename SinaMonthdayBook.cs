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
    class SinaMonthdayBook
    {
#if DEBUG
        const string _MONTHDAYBOOK_DIR_ = @"C:\Temp\MonthdayBook";
#else
        const string _MONTHDAYBOOK_DIR_ = "MonthdayBook";
#endif
        const string _OUTFILE_PREFIX_ = "MonthdayBook";
        const int m_startPage = 15038;
        const int m_endPage = 15068;

        WebClient m_wclient = new WebClient();
        String m_url = "http://astro.sina.com.cn/jian/{0}.shtml";

        String m_strHtml;

        public SinaMonthdayBook()
        {
            if (!Directory.Exists(_MONTHDAYBOOK_DIR_))
            {
                Directory.CreateDirectory(_MONTHDAYBOOK_DIR_);
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

                m_strHtml = m_wclient.DownloadString(getBookUrl(i));

                string tmp_filename = getOutputFileName(i);

                FileStream fs = new FileStream(tmp_filename, FileMode.CreateNew);
                StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("UTF-8"));

                sw.Write(m_strHtml);

                sw.Close();
                fs.Close();
            }

            // m_tmp_filename = tmp_filename;
        }

        private string getOutputFileName(int pageIdx)
        {
            // return string.Format("{0}\\{1}.{2}", _BIRTHDAYBOOK_DIR_, i, Path.GetFileName(Path.GetTempFileName()));

            string filename = null;

            if (m_strHtml != null && m_strHtml != "")
            {
                ZHtmlParser pr = new ZHtmlParser(m_strHtml);
                Entry entry = new Entry(pr);
                entry.xpath = "//*[@id=\"wrap\"]/h3/text()";

                filename = string.Format("{0}\\{1}.{2}.{3}", _MONTHDAYBOOK_DIR_, _OUTFILE_PREFIX_, entry.val, "html");
            }
            else
            {
                filename = string.Format("{0}\\{1}.{2}.{3}", _MONTHDAYBOOK_DIR_, _OUTFILE_PREFIX_, pageIdx, "html");
            }

            return filename;
        }

        private string getBookUrl(int pageIdx)
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
            //string strHtml = readTmpData(m_tmp_filename);

            //if (strHtml != null && strHtml != "")
            //{
            //    ZHtmlParser htmlParser = new ZHtmlParser(strHtml);

            //    BookEntry Characters = new BookEntry();

            //}
        }

    }
}
