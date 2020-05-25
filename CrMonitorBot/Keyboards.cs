using CrMonitorBot.Logic;
using System.Collections.Generic;
using System.Net.Http;
using Telegram.Bot.Types.ReplyMarkups;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace CrMonitorBot
{
    class Keyboards
    {
        public static string path = @"C:\Users\bigbo\Desktop\bot\DB.json";
        public ReplyKeyboardMarkup MainMenu()
        {
            var keyboard = new ReplyKeyboardMarkup
            {
                Keyboard = new[] {
                                                new[] // row 1
                                                {
                                                    new KeyboardButton("/crypto")
                                                },
                                                new[] // row 2
                                                {
                                                    new KeyboardButton("/add_real"),
                                                    new KeyboardButton("/add_crypto")
                                                },
                                                new[] // row 3
                                                {
                                                    new KeyboardButton("/remove_crypto"),
                                                    new KeyboardButton("/remove_real"),
                                                    new KeyboardButton("/change_crypto")
                                                },
                                                new[] // row 4
                                                {
                                                    new KeyboardButton("/show_supported_cryptos"),
                                                    new KeyboardButton("/show_info_crypto")
                                                },
                                            },
                ResizeKeyboard = true
            };
            return keyboard;
        }
        //public ReplyKeyboardMarkup SwitchMenu()
        //{
        //    var keyboard2 = new ReplyKeyboardMarkup
        //    {
        //        Keyboard = new[] {
        //                                        new[] // row 1
        //                                        {
        //                                            new KeyboardButton("Назад"),
        //                                            new KeyboardButton("Вперед")
        //                                        },
        //                                        new[] // row ц
        //                                        {
        //                                            new KeyboardButton("Домашняя страница")
        //                                        },
        //                                    },
        //        ResizeKeyboard = true
        //    };
        //    return keyboard2;
        //}
        public InlineKeyboardMarkup SwitchMenu()
        {
            var keyboard2 = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Назад"),
                    InlineKeyboardButton.WithCallbackData("Вперед"),
                }
                
            }
            );
            return keyboard2;
        }
        
        public ReplyKeyboardMarkup CryptoChoose()
        {
            var keyboard = new ReplyKeyboardMarkup
            {
                Keyboard = new[] {
                                                new[] // row 1
                                                {
                                                    new KeyboardButton("/crypto")
                                                },
                                                new[] // row 2
                                                {
                                                    new KeyboardButton("/add_real"),
                                                    new KeyboardButton("/add_crypto")
                                                },
                                                new[] // row 3
                                                {
                                                    new KeyboardButton("/remove_crypto"),
                                                    new KeyboardButton("/remove_real"),
                                                    new KeyboardButton("/change_crypto")
                                                },
                                                new[] // row 4
                                                {
                                                    new KeyboardButton("/show_supported_cryptos"),
                                                    new KeyboardButton("/show_info_crypto")
                                                },
                                            },
                
                ResizeKeyboard = true
            };
            return keyboard;
        }
        public async Task<InlineKeyboardMarkup> DynamicButtonsForCrypto(int userid)
        {          
            using var client = new HttpClient();
            var content = await client.GetStringAsync("https://crmonapi.azurewebsites.net/crypto/user/" + userid);
            List<Tickers> show = JsonConvert.DeserializeObject<List<Tickers>>(content);
            List<List<InlineKeyboardButton>> inlineKeyboardList = new List<List<InlineKeyboardButton>>();
            foreach (var a in show)//динамичные кнопочки
            {
                    List<InlineKeyboardButton> ts = new List<InlineKeyboardButton>();
                    ts.Add(InlineKeyboardButton.WithCallbackData(a.Name, a.Name));
                    inlineKeyboardList.Add(ts);
            }
            var inline = new InlineKeyboardMarkup(inlineKeyboardList);
            return inline;
        }
        public async Task<InlineKeyboardMarkup> DynamicButtonsForReal(int userid)
        {
            DBCheck c = new DBCheck();
            using var client = new HttpClient();
            //UserList dBUserItemsDBFind = JsonConvert.DeserializeObject<UserList>(System.IO.File.ReadAllText(path));
            string money = await client.GetStringAsync("https://crmonapi.azurewebsites.net/crypto/user/realget/" + userid);

            money = JsonConvert.DeserializeObject<string>(money);
            //int ind = c.DBIndex(dBUserItemsDBFind, userid);
            //string[] cur = dBUserItemsDBFind.users[ind].Real.Split(new char[] { ',' });
            string[] cur = money.Split(new char[] { ',' });
            List<string> list = new List<string>(cur);
            for (int i = 0; i < list.Count; i++)
            {
                list[i] = list[i].ToUpper();
            }
            List<List<InlineKeyboardButton>> inlineKeyboardList = new List<List<InlineKeyboardButton>>();
            foreach (var a in list)//динамичные кнопочки
            {
                List<InlineKeyboardButton> ts = new List<InlineKeyboardButton>();
                ts.Add(InlineKeyboardButton.WithCallbackData(a, a));
                inlineKeyboardList.Add(ts);
            }
            var inline = new InlineKeyboardMarkup(inlineKeyboardList);
            return inline;
        }

    }
}
