using HuangDao;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AstroSpider
{
    public class LaoHLEntryEx : Entry
    {
        char[] m_sepCharset = { ' ', '、' };
        string m_entryName = null;
        int m_idxOfVal = -1;
        string m_value = null;
        string[] m_values = null;

        public LaoHLEntryEx()
        { }

        public LaoHLEntryEx(string en, string xpath, int iVal = -1)
        {
            m_entryName = en;
            m_idxOfVal = iVal;
            base.xpath = xpath;
        }

        public string Value
        {
            get { return m_value; }
            set { m_value = value; }
        }

        /// <summary>
        /// 重载基类 Entry 的同名函数，对val 值作进一步处理
        /// </summary>
        /// <returns>经过处理后的 m_value 值 </returns>
        override protected string parseValue()
        {
            base.val = base.parseValue(); // 使用基类的函数提取 val 值
            if (m_idxOfVal >= 0)
            {
                if (base.val != null)         // 提取给定序号的子值，并保存于 m_value
                {
                    m_values = base.val.Split(m_sepCharset);
                    if (m_values.Length > m_idxOfVal)
                    {
                        m_value = m_values[m_idxOfVal];
                    }
                }
            }
            else
            {
                m_value = base.val;
            }
            return m_value;
        }
    }

    class LaoHuangDaoBook
    {
#if DEBUG
        const string _LAOHUANGDAOBOOK_DIR_ = @"C:\Temp\LaoHuangDaoBook";
#else
        const string _LAOHUANGDAOBOOK_DIR_ = "HuangDaoBook";
#endif
        const string _OUTFILE_PREFIX_ = "LaoHuangDaoBook";
        const int m_startPage = 0; // Huangdao book of 2014 year
        const int m_endPage = 365;

        WebClient m_wclient = new WebClient();
        String m_url = "http://astro.sina.com.cn/jian/hdrl/{0:D4}-{1:D2}-{2:D2}.shtml";

        String m_strHtml;

        public LaoHuangDaoBook()
        {
            if (!Directory.Exists(_LAOHUANGDAOBOOK_DIR_))
            {
                Directory.CreateDirectory(_LAOHUANGDAOBOOK_DIR_);
            }
        }

        private string getBookUrl(int pageIdx)
        {
            DateTime dt = new DateTime(2014, 1, 1);
            dt = dt.AddDays(pageIdx);
            string url = string.Format(m_url, dt.Year, dt.Month, dt.Day);
            return url;
        }

        private void readWebData()
        {
            int page_count = m_endPage - m_startPage + 1;

            for (int i = 0; i < page_count; i++)
            {
                string targetUrl = getBookUrl(i);
                Debug.WriteLine(targetUrl);

                try
                {
                    m_strHtml = m_wclient.DownloadString(targetUrl);
                    string tmp_filename = getOutputFileName(i);

                    FileStream fs = new FileStream(tmp_filename, FileMode.CreateNew);
                    StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("UTF-8"));

                    sw.Write(m_strHtml);

                    sw.Close();
                    fs.Close();
                }
                catch (WebException we)
                {
                    // Do nothing, try next url
                }
                catch (IOException ie)
                {
                    // Do nothing, try next url
                }

            }
        }

        private string getOutputFileName(int pageIdx)
        {
            string filename = null;

            if (m_strHtml != null && m_strHtml != "")
            {
                ZHtmlParser pr = new ZHtmlParser(m_strHtml);
                Entry entry = new Entry(pr);
                entry.xpath = "//*[@id=\"con01-0\"]/div[2]/div[2]/p/text()";

                string[] values = entry.val.Split(' ');
                string fex = values[0];

                filename = string.Format("{0}\\{1}.{2}.{3}", _LAOHUANGDAOBOOK_DIR_, _OUTFILE_PREFIX_, fex, "html");
            }
            else
            {
                filename = string.Format("{0}\\{1}.{2}.{3}", _LAOHUANGDAOBOOK_DIR_, _OUTFILE_PREFIX_, pageIdx, "html");
            }

            return filename;
        }


        class LaoHLEntryEx : Entry
        {
            char[] m_sepCharset = { ' ', '、' };
            string m_entryName = null;
            int m_idxOfVal = -1;
            string m_value = null;
            string[] m_values = null;
            public LaoHLEntryEx(string en, string xpath, int iVal = -1)
            {
                m_entryName = en;
                m_idxOfVal = iVal;
                base.xpath = xpath;
            }

            public string Value
            {
                get { return m_value; }
                set { m_value = value; }
            }

            override protected string parseValue()
            {
                base.val = base.parseValue(); // 使用基类的函数提取 val 值
                if (m_idxOfVal >= 0)
                {
                    if (base.val != null)         // 提取给定序号的子值，并保存于 m_value
                    {
                        m_values = base.val.Split(m_sepCharset);
                        if (m_values.Length > m_idxOfVal)
                        {
                            m_value = m_values[m_idxOfVal];
                        }
                    }
                }
                else
                {
                    m_value = base.val;
                }
                return m_value;
            }
        }

        class LaoHLDayEx
        {
            public LaoHLEntryEx m_date;          // 日期 
            public LaoHLEntryEx m_solarDate;     // 公历
            public LaoHLEntryEx m_lunarDate;     // 农历
            public LaoHLEntryEx m_yearOrder;     // 岁次
            public LaoHLEntryEx m_zodiac;        // 生肖
            public LaoHLEntryEx m_monthOrder;    // 月次
            public LaoHLEntryEx m_dayOrder;      // 日次
            public LaoHLEntryEx m_birthGod;      // 日胎神占方
            public LaoHLEntryEx m_fiveElem;      // 五行
            public LaoHLEntryEx m_collide;       // 冲
            public LaoHLEntryEx m_pengAvoid;     // 彭祖百忌
            public LaoHLEntryEx m_goodAngelYi;   // 吉神宜趋
            public LaoHLEntryEx m_evilAngelJi;   // 凶神宜忌
            public LaoHLEntryEx m_Yi;            // 宜
            public LaoHLEntryEx m_Ji;            // 忌

            public LaoHLDayEx(ZHtmlParser pr)
            {
                m_date = new LaoHLEntryEx("公历", "//*[@id=\"con01-0\"]/div[2]/div[2]/p/text()", 0);
                m_solarDate = new LaoHLEntryEx("公历", "//*[@id=\"con01-0\"]/div[2]/div[2]/p/text()", 0);
                m_lunarDate = new LaoHLEntryEx("农历", "//*[@id=\"con01-0\"]/div[2]/div[2]/p/text()", 1);
                m_yearOrder = new LaoHLEntryEx("岁次", "//*[@id=\"con01-0\"]/div[2]/div[2]/table/tr[1]/td[2]/text()", 0); // 注意：xpath 里的 tbody 须忽略！！！
                m_zodiac = new LaoHLEntryEx("岁次", "//*[@id=\"con01-0\"]/div[2]/div[2]/table/tr[1]/td[2]/text()", 1);
                m_monthOrder = new LaoHLEntryEx("岁次", "//*[@id=\"con01-0\"]/div[2]/div[2]/table/tr[1]/td[2]/text()", 2);
                m_dayOrder = new LaoHLEntryEx("岁次", "//*[@id=\"con01-0\"]/div[2]/div[2]/table/tr[1]/td[2]/text()", 3);
                m_birthGod = new LaoHLEntryEx("日胎神占方", "//*[@id=\"con01-0\"]/div[2]/div[2]/table/tr[2]/td[2]/text()");
                m_fiveElem = new LaoHLEntryEx("五行", "//*[@id=\"con01-0\"]/div[2]/div[2]/table/tr[3]/td[2]/text()");
                m_collide = new LaoHLEntryEx("冲", "//*[@id=\"con01-0\"]/div[2]/div[2]/table/tr[4]/td[2]/text()");
                m_pengAvoid = new LaoHLEntryEx("彭祖百忌", "//*[@id=\"con01-0\"]/div[2]/div[2]/table/tr[5]/td[2]/text()");
                m_goodAngelYi = new LaoHLEntryEx("吉神宜趋", "//*[@id=\"con01-0\"]/div[2]/div[2]/table/tr[6]/td[2]/text()");
                m_evilAngelJi = new LaoHLEntryEx("凶神宜忌", "//*[@id=\"con01-0\"]/div[2]/div[2]/table/tr[8]/td[2]/text()");
                m_Yi = new LaoHLEntryEx("宜", "//*[@id=\"con01-0\"]/div[2]/div[2]/table/tr[7]/td[2]/text()");
                m_Ji = new LaoHLEntryEx("忌", "//*[@id=\"con01-0\"]/div[2]/div[2]/table/tr[9]/td[2]/text()");

                // 为Entry 实例的 Parser 对象赋值，将会同时完成相应数值的解析
                m_date.Parser = pr;
                m_solarDate.Parser = pr;
                m_lunarDate.Parser = pr;
                m_yearOrder.Parser = pr;
                m_zodiac.Parser = pr;
                m_monthOrder.Parser = pr;
                m_dayOrder.Parser = pr;
                m_birthGod.Parser = pr;
                m_fiveElem.Parser = pr;
                m_collide.Parser = pr;
                m_pengAvoid.Parser = pr;
                m_goodAngelYi.Parser = pr;
                m_evilAngelJi.Parser = pr;
                m_Yi.Parser = pr;
                m_Ji.Parser = pr;

            }
        }
        
        public int parseHLDayFiles(string dirpath)
        {
            int nResult = 0;

            HdDBHelper db = new HdDBHelper();

            string[] filenames = Directory.GetFiles(dirpath);
            foreach (string fn in filenames)
            {
                FileStream fs = new FileStream(fn, FileMode.Open);
                if (fs != null)
                {
                    TextReader tr = new StreamReader(fs);
                    string strHtml = tr.ReadToEnd();
                    ZHtmlParser htmlParser = new ZHtmlParser(strHtml);
                    LaoHLDayEx hlday = new LaoHLDayEx(htmlParser);

                    /******
                    db.saveToDb(hlday);
                    *********/

                    tr.Close();
                }
                fs.Close();
            }

            return nResult;
        }

        internal bool Go()
        {
            bool r = false;

            // readWebData();

            parseHLDayFiles(_LAOHUANGDAOBOOK_DIR_);

            //saveData();

            return r;
        }
    }
}
