using CrMonitorBot.Logic;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;

namespace CrMonitorBot
{
    class DBCheck
    {
        public int DBIndex(UserList user, int id)
        {
            int Index = 0;
            string x = id.ToString();
            for (int i = 0; i < user.users.Count; i++)
            {
                if (user.users[i].Id == x)
                {
                    Index = i;
                }
            }
            return Index;
        }
        public async Task<HttpStatusCode> RealCheck(string r)
        {
            try
            {
                using var client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("https://api.coinpaprika.com/v1/tickers/btc-bitcoin?quotes=" + r);
                response.EnsureSuccessStatusCode();

            }
            catch (HttpRequestException)
            {
                return HttpStatusCode.BadRequest;
            }
            return HttpStatusCode.OK;
        }
        public async Task<HttpStatusCode> CryptoCheck(string r)
        {
            try
            {
                r = r.ToLower();
                using var client = new HttpClient();
                string response0 = await client.GetStringAsync("https://crmonapi.azurewebsites.net/crypto/coinslist");
                Dictionary<string, string> d = JsonConvert.DeserializeObject<Dictionary<string, string>>(response0);
                r = d.FirstOrDefault(x => x.Value == r).Key;
                HttpResponseMessage response = await client.GetAsync("https://crmonapi.azurewebsites.net/crypto/coins/info/"+r);
                response.EnsureSuccessStatusCode();

            }
            catch (HttpRequestException)
            {
                return HttpStatusCode.BadRequest;
            }
            return HttpStatusCode.OK;
        }
        public async Task<bool> CryptoExist(string r, string userid)
        {
            bool ex = false;
            using var client = new HttpClient();
            var content = await client.GetStringAsync("https://crmonapi.azurewebsites.net/crypto/user/" + userid);
            List<Tickers> show = JsonConvert.DeserializeObject<List<Tickers>>(content);
            foreach (var a in show)
            {
                if(a.Name == r)
                {
                    ex = true;
                }
            }
            return ex;
        }
    }
}
