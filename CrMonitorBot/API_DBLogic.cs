using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CrMonitorBot.Logic;
using Newtonsoft.Json;

namespace CrMonitorBot
{
    class API_DBLogic
    {
        public static async void AddUser(string id)
        {
            UserId userId = new UserId();
            userId.Id = id;
            var json = JsonConvert.SerializeObject(userId);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            using var client = new HttpClient();
            var content = await client.PostAsync("https://crmonapi.azurewebsites.net/crypto/adduser/", data);
        }
        public async Task<List<string>> Forward(int index)
        {
            List<string> cryptos = new List<string>();
            using var client = new HttpClient();
            string content = await client.GetStringAsync("https://crmonapi.azurewebsites.net/crypto/coins");
            List<string> json = JsonConvert.DeserializeObject<List<string>>(content);
            if(json == null)
            {
                return null;
            }
            int index15 = index + 15;
            for(int i = index; i < index15; i++)
            {
                cryptos.Add(json[i]);
            }
            return cryptos;
        }
        // не активно по многим причинам
        //public async Task<int> MaxValueCrypto()
        //{
        //    List<string> cryptos = new List<string>();
        //    using var client = new HttpClient();
        //    string content = await client.GetStringAsync("https://localhost:44305/crypto/coins");
        //    List<string> json = JsonConvert.DeserializeObject<List<string>>(content);
        //    int x = json.Count;
        //    return x;
        //}
        public async Task<string> GetInfo(string cryptoname)
        {
            cryptoname = cryptoname.ToLower();
            using var client = new HttpClient();
            string content = await client.GetStringAsync("https://crmonapi.azurewebsites.net/crypto/coinslist/");
            Dictionary<string, string> list = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
            string id = list.FirstOrDefault(x => x.Value == cryptoname).Key;
            string info_ = await client.GetStringAsync("https://crmonapi.azurewebsites.net/crypto/coins/info/" + id);
            CoinInformation info = JsonConvert.DeserializeObject<CoinInformation>(info_);
            string isnew;
            string isactive;
            if(info.Is_new == false)
            {
                isnew = "Нет";
            }
            else
            {
                isnew = "Да";
            }
            if (info.Is_active == false)
            {
                isactive = "Нет";
            }
            else
            {
                isactive = "Да";
            }

            string CoinInformation =
@$"Название: {info.Name}
Новая? - {isnew}
Активная? - {isactive}
Ранг: {info.Rank}
Описание: {info.Description}"
;
            return CoinInformation;
    }
        public async Task<System.Net.HttpStatusCode> AddCrypto(string cryptoname, int userid)
        {
            UserCurrency curr = new UserCurrency();
            curr.crypto = cryptoname;
            var json = JsonConvert.SerializeObject(curr);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            using var client = new HttpClient();
            var content = await client.PostAsync("https://crmonapi.azurewebsites.net/crypto/useraddcrypto/" + userid, data);
            System.Net.HttpStatusCode state = content.StatusCode;
            return state;
        }
        public async Task<System.Net.HttpStatusCode> AddReal(string realname, int userid)
        {
            UserCurrencyToChange curr = new UserCurrencyToChange();
            curr.real = realname;
            var json = JsonConvert.SerializeObject(curr);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            using var client = new HttpClient();
            var content = await client.PutAsync("https://crmonapi.azurewebsites.net/crypto/useredit/" + userid, data);
            System.Net.HttpStatusCode state = content.StatusCode;
            return state;
        }
        public async Task<System.Net.HttpStatusCode> RemoveCrypto(string cryptoname, int userid)
        {
            using var client = new HttpClient();
            var content = await client.DeleteAsync("https://crmonapi.azurewebsites.net/crypto/useredit/" + userid+"/deletecrypto/"+cryptoname);
            System.Net.HttpStatusCode state = content.StatusCode;
            return state;
        }
        public async Task<System.Net.HttpStatusCode> RemoveReal(string realname, int userid)
        {
            using var client = new HttpClient();
            realname = realname.ToLower();
            var content = await client.DeleteAsync("https://crmonapi.azurewebsites.net/crypto/useredit/" + userid + "/deletereal/" + realname);
            System.Net.HttpStatusCode state = content.StatusCode;
            return state;
        }
        public bool ChechCommand(string tocheck)
        {
            Regex regex = new Regex(@"^/");
            bool match = regex.IsMatch(tocheck);
            return match;
        }
        public async Task<System.Net.HttpStatusCode> ChangeCrypto(string cryptoname, string cryptoname_tochange, int userid)
        {
            UserCurrencyToChange curr = new UserCurrencyToChange();
            curr.crypto = cryptoname;
            curr.crypto_to_change = cryptoname_tochange;
            var json = JsonConvert.SerializeObject(curr);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            using var client = new HttpClient();
            var content = await client.PutAsync("https://crmonapi.azurewebsites.net/crypto/useredit/" + userid, data);
            System.Net.HttpStatusCode state = content.StatusCode;
            return state;
        }
        public async Task<string> Crypto(string cryptoname, int userid)
        {
            using var client = new HttpClient();
            string content = await client.GetStringAsync("https://crmonapi.azurewebsites.net/crypto/user/" + userid+"/"+cryptoname.ToLower());
            Tickers ticker = JsonConvert.DeserializeObject<Tickers>(content);
            string max_supply;
            if(ticker.MaxSupply == 0)
            {
                max_supply = "Неограничено";
            }
            else
            {
                max_supply = ticker.MaxSupply.ToString();
            }
            string CoinInformation =
@$"Название: {ticker.Name}
Символ: {ticker.Symbol}
Ранг: {ticker.Rank}
Текущее количество: {ticker.TotalSupply}
Максимальное количество: {max_supply}"
;
            List<string> q = new List<string>();
            foreach (var c in ticker.Quotes)
            {
                string quotes =
@$"Цена в {c.Key}:
Цена: {c.Value.Price}
Процентное изменение за 1 час: {c.Value.PercentChange1H}
Процентное изменение за 12 часов: {c.Value.PercentChange12H}
Процентное изменение за 24 часа: {c.Value.PercentChange24H}"
;
                q.Add(quotes);
            }
            string joined = string.Join("\n", q.ToArray());
            CoinInformation = $"{CoinInformation}\n{string.Join("\n", q.ToArray())}";
            return CoinInformation;
        }

    }
}
