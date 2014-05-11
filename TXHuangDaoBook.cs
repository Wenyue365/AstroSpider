using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Serialization;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;

namespace AstroSpider
{
    class _JSONTXHuangDaoDay
    {
        public string FID;
        public string F1;
        public string F2;
        public string F3;
        public string F4;
        public string F5;
        public string F6;
        public string F7;
    }

    public class TXHuangDaoDay
    {
        public string FID;
        public DateTime ShowTime;
        public string LunerDate;
        public string Astro;
        public string GoodToDo;
        public string BadToDo;
        public TXHuangDaoDay()
        {

        }

        public TXHuangDaoDay(string json)
        {
            if (json != null)
            {
                parse(json);
            }
        }

        public void parse(string json)
        {
            string js = null;
            int st = json.IndexOf('{');
            int ls = json.IndexOf('}');
            if (st > 0 && ls > 0 && ls > st)
            {
                js = json.Substring(st, ls - st + 1);

                _JSONTXHuangDaoDay jo = JsonConvert.DeserializeObject<_JSONTXHuangDaoDay>(js);

                FID = jo.FID;
                int year = int.Parse(jo.F1);
                int month = int.Parse(jo.F2);
                int day = int.Parse(jo.F3);
                ShowTime = new DateTime(year, month, day);

                LunerDate = jo.F4;
                Astro = jo.F5;
                GoodToDo = jo.F6;
                BadToDo = jo.F7;
            }
        }

        public string SerialToXML()
        {
            string xml = null;
            XmlSerializer xmlSerializer = new XmlSerializer(this.GetType());
            StringWriter textWriter = new StringWriter();
            xmlSerializer.Serialize(textWriter, this);
            xml = textWriter.ToString();

            return xml;
        }

        internal string SerialToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }

        public TXHuangDaoDay DeserializeFromXML(string xml)
        {
            var serializer = new XmlSerializer(this.GetType());
            TXHuangDaoDay hdd = null;

            using (TextReader reader = new StringReader(xml))
            {
                hdd = (TXHuangDaoDay)serializer.Deserialize(reader);
            }

            return hdd;
        }
    }

    class TXHuangDaoBook
    {
#if DEBUG
        const string _TXHUANGDAOBOOK_DIR_ = @"C:\Temp\TxHuangDaoBook";
#else
        const string _TXHUANGDAOBOOK_DIR_ = "HuangDaoBook";
#endif
        const string _OUTFILE_PREFIX_ = "TxHuangDaoBook";
        const int m_startPage = 4496;
        const int m_endPage = 4860;

        WebClient m_wclient = new WebClient();
        string m_url = "http://data.astro.qq.com/hl/";

        String m_strHtml = null;

        public TXHuangDaoBook()
        {
            if (!Directory.Exists(_TXHUANGDAOBOOK_DIR_))
            {
                Directory.CreateDirectory(_TXHUANGDAOBOOK_DIR_);
            }
        }

        private string getBookUrl(int pageIdx)
        {
            double hlJsonFID = m_startPage + pageIdx;

            string url = m_url + Math.Floor(hlJsonFID / 1000) + "/" + hlJsonFID + "/info.js";

            return url;
        }

        private void readWebData()
        {
            int page_count = m_endPage - m_startPage + 1;

            for (int i = 0; i < page_count; i++)
            {
                m_strHtml = m_wclient.DownloadString(getBookUrl(i));
                TXHuangDaoDay hdd = new TXHuangDaoDay(m_strHtml);

                saveToDb(hdd);
            }
        }

        enum FileFormat { FM_JSON, FM_XML };
        bool saveToFile(TXHuangDaoDay hdd, FileFormat iFormat)
        {
            bool result = true;

            try
            {
                string tmp_filename = hdd.FID + ".txt";

                FileStream fs = new FileStream(tmp_filename, FileMode.CreateNew);
                StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("UTF-8"));

                switch (iFormat)
                {
                    case FileFormat.FM_JSON:
                        sw.Write(hdd.SerialToJSON());
                        break;
                    case FileFormat.FM_XML:
                        sw.Write(hdd.SerialToXML());
                        break;
                }

                sw.Close();
                fs.Close();

            }
            catch (IOException ex)
            {
                result = false;
            }

            return result;
        }

        string connStringBuilder(string host, int port, string dbname, string username, string password, string charset)
        {
            string cs = string.Format("Server={0};Port={1};Database={2};Uid={3};Pwd={4};CharacterSet={5}",
                host, port, dbname, username, password, charset);
            return cs;
        }

        string m_connString = null;
        MySqlConnection m_connSql = null;

        bool initDb()
        {
            bool result = true;

            const string db_host = "admin.yun03.yhosts.com";
            const int db_port = 3306;
            const string db_name = "dbxwearon";
            const string db_user = "dbxwearon";
            const string db_pass = "dbxwearon---";
            const string db_charset = "utf8"; // this value is query from the DB

            if (m_connSql == null) // Open DB connection when it is null
            {
                try
                {
                    m_connString = connStringBuilder(db_host, db_port, db_name, db_user, db_pass, db_charset);
                    m_connSql = new MySqlConnection(m_connString);

                    m_connSql.Open();
                }
                catch(MySqlException e)
                {
                    result = false;

                    m_connSql = null;
                }
            }

            return result;
        }

        void closeDb()
        {
            if (m_connSql != null)
            {
                m_connSql.Close();
                m_connSql = null;
            }
        }
        bool saveToDb(TXHuangDaoDay hdd)
        {
            bool result = false;
            if (!initDb())
            {
                return false;
            }

            string cmdText = string.Format("INSERT INTO wy_huangli(fid, showtime, lunerdate, goodtodo, badtodo) VALUES(\'{0}\', \'{1}\', \'{2}\', \'{3}\', \'{4}\')",
                             hdd.FID, hdd.ShowTime, hdd.LunerDate, hdd.GoodToDo, hdd.BadToDo);

            MySqlCommand cmdSql = new MySqlCommand(cmdText, m_connSql);
            cmdSql.CommandType = CommandType.Text;
            

            if (cmdSql.ExecuteNonQuery() == 1)
            {
                result = true;
            }

            return result;
        }

        private string getOutputFileName(int year, int month, int day)
        {

            string filename = null;

            if (m_strHtml != null && m_strHtml != "")
            {
                filename = string.Format("{0}\\{1}.{2}-{3}-{4}.{5}", _TXHUANGDAOBOOK_DIR_, _OUTFILE_PREFIX_, year, month, day, "html");
            }

            return filename;
        }

        internal bool Go()
        {
            bool r = false;

            readWebData();

            //parseWebData();

            //saveData();

            return r;
        }
    }
}
