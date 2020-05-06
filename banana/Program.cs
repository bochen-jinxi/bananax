using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using EIIP.Helper;
using Newtonsoft.Json;
using System.IO.Compression;
using System.Collections.Generic; 
using System.Text;
namespace Stock季报
{
    public class RequiredData
    {
        public List<string> Params { get; set; }
    }


 

    internal class Program
    {
        private static readonly StringBuilder sb = new StringBuilder(500);

        private static readonly StringBuilder sb2 = new StringBuilder(500);
        private static readonly string url = "http://android.fuliapps.com/search?page=1&wd={0}&apiversion=28";
        private static int count2;
        
 private static List<string> arrayStock = new List<string>
        {
 "新垣结衣",
            "深田恭子",
                "比嘉爱未",
            "北川景子",
            "立花美凉",
            "仁科百华",
            "佐佐木希",
            "本田翼",
            "有村架纯",
            "小嶋阳菜",
            "天使萌",
            "七泽米亚",
            "星奈爱",
            "美谷朱里",
            "户田真琴",
            "樱井莉亚",
            "藤泽安奈",
            "真白希实",
            "松岛枫",
            "大桥未久",
            "秋野千寻",
            "日向优梨",
            "上原保奈美",
            "立川理惠",
            "上原志织",
            "白石优",
"名空",
"百多惠美里 ",
"河合乃乃香 ",
"桥本有菜   ",
"三上悠亚   ",
"吉高宁宁   ",
"坂道美琉   ",

"明日花绮罗 ",

"香坂紗梨   ",
"北川礼子   ",

"彩乃奈奈   ",
"香椎梨亚   ",

"佐佐木明希 ",
"相泽南     ",


"樱木凛     ",
"宇都宮紫苑 ",

"苍井空     ",

"水卜樱     ",
"市來美保   ",

"天海翼     ",
"波多野结衣 ",
"江波凉     ",

"高桥圣子   ",
"立花瑠莉   ",
"上原亚衣   ",
"园田美樱   ",
"明里紬     ",
"前田香织",
"吉沢明歩   ",
"葵千恵     ",
"葵司       ",
"九重环奈   ",





        };
        private static void Main(string[] args)
        {
            Console.WriteLine("先输入start运行，直到看到over后，输入continue继续，直到看到over输入end结束！");
            Console.ReadLine();

            foreach (var el in arrayStock)
            {
                SetpTwo(el.Trim().ToString());
           
           }
          
           return;
            
        }


        private static void SetpTwo(string code)
        {


            string responsedata = Reader(string.Format(url, code),false);
            var obj = JsonConvert.DeserializeObject<ResponseData>(responsedata);


            var i = 0;
            foreach (var el in obj.data.vodrows)
            {
                var tempurl=String.Format("http://android.fuliapps.com{0}",el.play_url);
                sb.Append(tempurl).Append("\r\n");
                i++;
                SetpThree(tempurl,code,i);
            }
                count2++;

                if (count2 > 0)
                {
                    FileHelper.WriteLine(@"c:/repalyall" + count2 + ".sql", sb.ToString());
                    sb.Clear();
                    
                }
            


            Console.WriteLine("所有上榜 \n" + code);
        }
        private static void SetpThree(string url,string code,int i)
        {

            //http://android.fuliapps.com/vod/reqplay/4695
            try
            {


                string responsedata = Reader(url, true);
                var obj = JsonConvert.DeserializeObject<ResponseData2>(responsedata);





                sb2.Append(code).Append(i).Append(",").Append(string.IsNullOrEmpty(obj.data.httpurl) ? obj.data.httpurl_preview : obj.data.httpurl);







                if (sb2.Length > 0)
                {
                   // FileHelper.WriteLine(@"c:/replay99" + code + count2 + ".txt", sb2.ToString());
                    FileHelper.WriteLine(@"c:/replay99.txt", sb2.ToString());

                    sb2.Clear();

                }

            }
            catch (Exception)
            {

                return;
            }
            finally {
                Console.WriteLine("所有上榜 \n" + url);
            }
             
        }
        private static string Reader(string url, bool isNeedCookie)
        {
            while (true)
            {
                Thread.Sleep(500);
                var request = (HttpWebRequest) WebRequest.Create(url);
                request.ContentType = "application/json; charset=UTF-8";
                request.Method = "get";
                request.Headers.Set("Accept-Language", "zh-CN");
                if (isNeedCookie)
                { 
                var cc= new CookieContainer();
                string cookie = "xxx_api_auth=3832323264383563303339333739333763346336653561326132663061613035";
               cc.SetCookies(new Uri("http://android.fuliapps.com"), cookie);
                request.CookieContainer = cc;
                }
                // request.Credentials = CredentialCache.DefaultCredentials;
                // request.Timeout = 1000 * 1;
                // request.ServicePoint.ConnectionLimit = 1000;
             
                try
                {
                    var httpResponse = (HttpWebResponse) request.GetResponse();


                    if (httpResponse.ContentEncoding.ToLower().Contains("gzip"))
                {
                    using (GZipStream stream = new GZipStream(httpResponse.GetResponseStream(), CompressionMode.Decompress))
                    {
                        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
 

                    using (
                        var streamReader = new StreamReader(httpResponse.GetResponseStream(),
                            Encoding.GetEncoding("utf-8")))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                    Thread.Sleep(1000*10);
                }
            }
        }





    }



    public class ResponseData2
    {
        public ResultSets2 data { get; set; }
        public int retcode { get; set; }
        public string errmsg { get; set; }
    }
    public class ResultSets2
    {
        public string httpurl { get; set; }

        public string httpurl_preview { get; set; }
         

    }

    public class ResponseData
    {
        public ResultSets data { get; set; }
        public int retcode { get; set; }
        public string errmsg { get; set; }
    }

    public class ResultSets
    {
      public List<ColDes> vodrows { get; set; }
    
    }

    public class ColDes
    {
        public string down_url { get; set; }
        public string play_url { get; set; }
    }

}
