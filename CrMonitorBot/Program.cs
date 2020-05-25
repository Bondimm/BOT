using System;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using Newtonsoft.Json;

namespace CrMonitorBot
{
    class Program
    {
        static TelegramBotClient Bot;
        private static System.Timers.Timer aTimer;
        public static int index_of_cryptos = 0;
        //public static Dictionary<int, string> user_status = new Dictionary<int, string>(); JsonConvert.DeserializeObject<UserList>(System.IO.File.ReadAllText(path))

        //public static Dictionary<int, string> user_status2 = JsonConvert.DeserializeObject<Dictionary<int, string>>(System.IO.File.ReadAllText("userstatus.json"));
        //public static Dictionary<int, string> user_status = Program.trydictionary(user_status2);
        public static Dictionary<int, string> user_status = new Dictionary<int, string>();
        public static Dictionary<int, string> currency_to_change_from_button = new Dictionary<int, string>();
        public static Dictionary<int, UserCryptoCount> message_to_edit = new Dictionary<int, UserCryptoCount>();
        public static Dictionary<int, string> trydictionary(Dictionary<int, string> user_status4)
        {
            Dictionary<int, string> user_status3;
            if (user_status4 != null)
            {
                user_status3 = user_status4;
            }
            else
            {
                user_status3 = new Dictionary<int, string>();
            }
            return user_status3;
        }
        static void Main()
        {
            Bot = new TelegramBotClient("1150211203:AAGRIGVvt3iK3-vbkGzeTvDgV1kDJxtLDeI");
            Bot.OnMessage += Bot_OnMessage;
            Bot.OnCallbackQuery += Bot_OnCallbackQuery;
            var me = Bot.GetMeAsync().Result;
            Bot.StartReceiving();
            Console.WriteLine(me.FirstName);
            aTimer = new System.Timers.Timer(60 * 60 * 1000); //one hour in milliseconds
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Start();
            Console.ReadLine();
            Bot.StopReceiving();
        }
        private static async void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            await System.IO.File.WriteAllTextAsync("userstatus.json", JsonConvert.SerializeObject(user_status, Formatting.Indented));
            System.Diagnostics.Process.Start("CrMonitorBot");
            Environment.Exit(0);

        }
        private static async void Bot_OnCallbackQuery(object sender, Telegram.Bot.Args.CallbackQueryEventArgs e)
        {
            Keyboards keyboards = new Keyboards();
            InlineKeyboardMarkup keyboard_inline = keyboards.SwitchMenu();
            API_DBLogic DBLogic = new API_DBLogic();
            MsgReply Reply = new MsgReply();
            string buttonText = e.CallbackQuery.Data;
            string name = $"{e.CallbackQuery.From.FirstName} {e.CallbackQuery.From.LastName}";
            Console.WriteLine($"{name} with id {e.CallbackQuery.From.Id} clicked button: '{buttonText}'");
            //int maxvalue = await DBLogic.MaxValueCrypto();
            int maxvalue = 2115; //поставить выше или максимальное значение - тихий омут, теневой и криминальный сброд, неактивные криптовалюты, аут оф рейндж
            bool keyExists = user_status.ContainsKey(e.CallbackQuery.From.Id);
            if (keyExists == false)
            {
                await Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, Reply.ChooseCommandPlease);
                return;
            }
            if (user_status[e.CallbackQuery.From.Id] == "change_crypto_mode")
            {
                await Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, Reply.ChangeCryptoInstruction2);
                bool ex = currency_to_change_from_button.ContainsKey(e.CallbackQuery.From.Id);
                if (ex == false)
                {
                    currency_to_change_from_button.Add(e.CallbackQuery.From.Id, buttonText.ToLower());
                }
                else
                {
                    currency_to_change_from_button[e.CallbackQuery.From.Id] = buttonText.ToLower();
                }
                user_status[e.CallbackQuery.From.Id] = "change_crypto_mode_part2";
            }
            if (user_status[e.CallbackQuery.From.Id] == "remove_crypto_mode")
            {
                Thread.Sleep(900);
                var x = await DBLogic.RemoveCrypto(buttonText, e.CallbackQuery.From.Id);
                if (x == System.Net.HttpStatusCode.NoContent)
                {
                    await Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, Reply.Success);
                    //user_status[e.CallbackQuery.From.Id] = "normal";
                    return;
                }
                if (x == System.Net.HttpStatusCode.MethodNotAllowed)
                {
                    await Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, Reply.RemoveCryptoFailure);
                    //user_status[e.CallbackQuery.From.Id] = "normal";
                    return;
                }
                if (x == System.Net.HttpStatusCode.NotFound)
                {
                    await Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, Reply.Error_CryCurrNotFound);
                    //user_status[e.CallbackQuery.From.Id] = "normal";
                    return;
                }
            }
            if (user_status[e.CallbackQuery.From.Id] == "crypto_mode")
            {
                Thread.Sleep(755);
                string x = await DBLogic.Crypto(buttonText, e.CallbackQuery.From.Id);
                await Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, x);
                //user_status[e.CallbackQuery.From.Id] = "normal";
                return;

            }
            if (user_status[e.CallbackQuery.From.Id] == "remove_real_mode")
            {
                Thread.Sleep(751);
                var x = await DBLogic.RemoveReal(buttonText, e.CallbackQuery.From.Id);
                if (x == System.Net.HttpStatusCode.NoContent)
                {
                    await Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, Reply.Success);
                    //user_status[e.CallbackQuery.From.Id] = "normal";
                    return;
                }
                if (x == System.Net.HttpStatusCode.MethodNotAllowed)
                {
                    await Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, Reply.RemoveRealFailure);
                    //user_status[e.CallbackQuery.From.Id] = "normal";
                    return;
                }
                if (x == System.Net.HttpStatusCode.NotFound)
                {
                    await Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, Reply.Error_CurrNotFound);
                    //user_status[e.CallbackQuery.From.Id] = "normal";
                    return;
                }
            }
            if (user_status[e.CallbackQuery.From.Id] == "show_supported_cryptos_mode")
            {
                bool command = DBLogic.ChechCommand(e.CallbackQuery.Message.Text);
                if (command != true)
                {
                    if (buttonText == "Вперед")
                    {
                        if (message_to_edit == null)
                        {
                            return;
                        }
                        else
                        {
                            message_to_edit[e.CallbackQuery.From.Id].Count += 15;
                            if (message_to_edit[e.CallbackQuery.From.Id].Count > maxvalue)
                            {
                                message_to_edit[e.CallbackQuery.From.Id].Count = 0;
                            }
                            List<string> cryptos = await DBLogic.Forward(message_to_edit[e.CallbackQuery.From.Id].Count);
                            string combindedString = string.Join("; ", cryptos.ToArray());
                            try { await Bot.EditMessageTextAsync(e.CallbackQuery.From.Id, message_to_edit[e.CallbackQuery.From.Id].Message.MessageId, combindedString, replyMarkup: keyboard_inline); }
                            catch { }
                        }
                    }
                    if (buttonText == "Назад")
                    {
                        if (message_to_edit == null)
                        {
                            return;
                        }
                        else
                        {
                            message_to_edit[e.CallbackQuery.From.Id].Count -= 15;
                            if (message_to_edit[e.CallbackQuery.From.Id].Count < 0)
                            {
                                message_to_edit[e.CallbackQuery.From.Id].Count = maxvalue;
                            }
                            List<string> cryptos = await DBLogic.Forward(message_to_edit[e.CallbackQuery.From.Id].Count);
                            string combindedString = string.Join("; ", cryptos.ToArray());
                            try { await Bot.EditMessageTextAsync(e.CallbackQuery.From.Id, message_to_edit[e.CallbackQuery.From.Id].Message.MessageId, combindedString, replyMarkup: keyboard_inline); }
                            catch { }
                        }
                    }
                }
                else
                {
                    user_status[e.CallbackQuery.From.Id] = "normal";
                }
            }
        }
        private static async void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            API_DBLogic DBLogic = new API_DBLogic();
            MsgReply Reply = new MsgReply();
            Keyboards keyboards = new Keyboards();
            ReplyKeyboardMarkup keyboard_reply;
            InlineKeyboardMarkup keyboard_inline;
            var message = e.Message;
            index_of_cryptos = 0;
            List<string> cryptos = await DBLogic.Forward(index_of_cryptos);
            if(cryptos == null)
            {
                return;
            }
            bool exist = false;
            foreach (int c in user_status.Keys)
            {
                if (c == message.Chat.Id)
                {
                    exist = true;
                    break;
                }
            }
            if (exist == false)
            {
                try { user_status.Add(message.From.Id, "normal"); }
                catch { return; }//????????
            }
            //button handler
            if (user_status[message.From.Id] == "remove_crypto_mode" || user_status[message.From.Id] == "show_supported_cryptos_mode" || user_status[message.From.Id] == "remove_real_mode" || user_status[message.From.Id] == "crypto_mode" || user_status[message.From.Id] == "change_crypto_mode")
            {
                bool command = DBLogic.ChechCommand(message.Text);
                if (command != true)
                {
                    if(user_status[message.From.Id] == "show_supported_cryptos_mode")
                    {
                        await Bot.SendTextMessageAsync(message.From.Id, Reply.ButtonState_supportedcrypto);
                        return;
                    }
                    await Bot.SendTextMessageAsync(message.From.Id, Reply.ButtonState);
                    return;
                }
                else
                {
                    user_status[message.From.Id] = "normal";
                }
            }
            if (user_status[message.From.Id] == "show_info_crypto_mode")
            {
                bool command = DBLogic.ChechCommand(message.Text);
                if(command != true)
                {
                    try
                    {
                        message.Text = message.Text.ToLower();
                        string info = await DBLogic.GetInfo(message.Text);
                        await Bot.SendTextMessageAsync(message.From.Id, info);
                        user_status[message.From.Id] = "normal";
                        return;
                    }
                    catch
                    {
                        await Bot.SendTextMessageAsync(message.From.Id, Reply.CoinInfoError);
                        user_status[message.From.Id] = "normal";
                        return;
                    }
                }
                else
                {
                    user_status[message.From.Id] = "normal";
                }
                //if (message.Text == "/start" || message.Text == "/crypto" || message.Text == "/add_crypto" || message.Text == "/show_info_crypto" || message.Text == "/add_real" || message.Text == "/remove_crypto" || message.Text == "/remove_real" || message.Text == "/show_supported_cryptos" || message.Text == "/show_info_crypto" || message.Text == "/change_crypto")
                //{
                //    user_status[message.From.Id] = "normal";
                //}
                //if (user_status[message.From.Id] != "normal")
                //{
                //    try
                //    {
                //        message.Text = message.Text.ToLower();
                //        string info = await DBLogic.GetInfo(message.Text);
                //        await Bot.SendTextMessageAsync(message.From.Id, info);
                //        user_status[message.From.Id] = "normal";
                //    }
                //    catch
                //    {
                //        await Bot.SendTextMessageAsync(message.From.Id, Reply.CoinInfoError);
                //        user_status[message.From.Id] = "normal";
                //    }
                //}
            }
            if (user_status[message.From.Id] == "add_crypto_mode")
            {
                bool command = DBLogic.ChechCommand(message.Text);
                if (command != true)
                {
                    try
                    {
                        var reqest = await DBLogic.AddCrypto(message.Text, message.From.Id);
                        if (reqest == System.Net.HttpStatusCode.BadRequest)
                        {
                            await Bot.SendTextMessageAsync(message.From.Id, Reply.AddCryptoFailure);
                            user_status[message.From.Id] = "normal";
                            return;
                        }
                        if (reqest == System.Net.HttpStatusCode.NoContent)
                        {
                            await Bot.SendTextMessageAsync(message.From.Id, Reply.Success);
                            user_status[message.From.Id] = "normal";
                            return;
                        }
                        if (reqest == System.Net.HttpStatusCode.Conflict)
                        {
                            await Bot.SendTextMessageAsync(message.From.Id, Reply.AddCryptoFailure_AlreadyExists);
                            user_status[message.From.Id] = "normal";
                            return;
                        }
                    }
                    catch
                    {
                        await Bot.SendTextMessageAsync(message.From.Id, Reply.AddCryptoFailure);
                        user_status[message.From.Id] = "normal";
                        return;
                    }
                }
                else
                {
                    user_status[message.From.Id] = "normal";
                }
            }
            if (user_status[message.From.Id] == "change_crypto_mode_part2")
            {
                bool command = DBLogic.ChechCommand(message.Text);
                if (command != true)
                {
                    try
                    {
                        var reqest = await DBLogic.ChangeCrypto(message.Text.ToLower(), currency_to_change_from_button[message.From.Id], message.From.Id);
                        if (reqest == System.Net.HttpStatusCode.NotFound)
                        {
                            await Bot.SendTextMessageAsync(message.From.Id, Reply.ChangeCryptoFailure);
                            user_status[message.From.Id] = "normal";
                            return;
                        }
                        if (reqest == System.Net.HttpStatusCode.NoContent)
                        {
                            await Bot.SendTextMessageAsync(message.From.Id, Reply.Success);
                            user_status[message.From.Id] = "normal";
                            return;
                        }
                        if (reqest == System.Net.HttpStatusCode.Conflict)
                        {
                            await Bot.SendTextMessageAsync(message.From.Id, Reply.ChangeCryptoFailure_AlreadyExists);
                            user_status[message.From.Id] = "normal";
                            return;
                        }
                    }
                    catch
                    {
                        await Bot.SendTextMessageAsync(message.From.Id, Reply.AddRealFailure);
                        user_status[message.From.Id] = "normal";
                        return;
                    }
                }
                else
                {
                    user_status[message.From.Id] = "normal";
                }
            }
            if (user_status[message.From.Id] == "add_real_mode")
            {
                bool command = DBLogic.ChechCommand(message.Text);
                if (command != true)
                {
                    try
                    {
                        var reqest = await DBLogic.AddReal(message.Text, message.From.Id);
                        if (reqest == System.Net.HttpStatusCode.BadRequest)
                        {
                            await Bot.SendTextMessageAsync(message.From.Id, Reply.AddRealFailure);
                            user_status[message.From.Id] = "normal";
                            return;
                        }
                        if (reqest == System.Net.HttpStatusCode.NoContent)
                        {
                            await Bot.SendTextMessageAsync(message.From.Id, Reply.Success);
                            user_status[message.From.Id] = "normal";
                            return;
                        }
                        if (reqest == System.Net.HttpStatusCode.Conflict)
                        {
                            await Bot.SendTextMessageAsync(message.From.Id, Reply.AddRealFailure_AlreadyExists);
                            user_status[message.From.Id] = "normal";
                            return;
                        }
                    }
                    catch
                    {
                        await Bot.SendTextMessageAsync(message.From.Id, Reply.AddRealFailure);
                        user_status[message.From.Id] = "normal";
                        return;
                    }
                }
                else
                {
                    user_status[message.From.Id] = "normal";
                }
            }
            string combindedString = string.Join("; ", cryptos.ToArray());

            string id = message.From.Id.ToString();

            if (message == null || message.Type != MessageType.Text)
            {
                return;
            }
            string name = $"{message.From.FirstName} {message.From.LastName}";
            Console.WriteLine($"{name} with id {message.Chat.Id} sent: '{message.Text}'");
            try
            {
                switch (message.Text)
                {
                    case "/start":
                        API_DBLogic.AddUser(id);
                        keyboard_reply = keyboards.MainMenu();
                        await Bot.SendTextMessageAsync(message.From.Id, Reply.startReply, replyMarkup: keyboard_reply);
                        break;
                    case "/show_supported_cryptos":
                        //Тут бага есть :)
                        keyboard_inline = keyboards.SwitchMenu();
                        await Bot.SendTextMessageAsync(message.From.Id, Reply.Inline_Text);
                        UserCryptoCount c = new UserCryptoCount
                        {
                            Count = 0,
                            Message = await Bot.SendTextMessageAsync(message.From.Id, combindedString, replyMarkup: keyboard_inline)
                        };
                        bool ex = message_to_edit.ContainsKey(message.From.Id);
                        if (ex == false)
                        {
                            message_to_edit.Add(message.From.Id, c);
                        }
                        else
                        {
                            message_to_edit[message.From.Id] = c;
                        }
                        user_status[message.From.Id] = "show_supported_cryptos_mode";
                        break;
                    case "/show_info_crypto":
                        await Bot.SendTextMessageAsync(message.From.Id, Reply.CoinInfoInstruction);
                        user_status[message.From.Id] = "show_info_crypto_mode";
                        break;
                    case "/add_crypto":
                        await Bot.SendTextMessageAsync(message.From.Id, Reply.CoinInfoInstruction);
                        user_status[message.From.Id] = "add_crypto_mode";
                        break;
                    case "/add_real":
                        await Bot.SendTextMessageAsync(message.From.Id, Reply.AddRealInstruction);
                        user_status[message.From.Id] = "add_real_mode";
                        break;
                    case "/remove_crypto":
                        keyboard_inline = await keyboards.DynamicButtonsForCrypto(message.From.Id);
                        await Bot.SendTextMessageAsync(message.From.Id, Reply.RemoveCryptoInstruction, replyMarkup: keyboard_inline);
                        user_status[message.From.Id] = "remove_crypto_mode";
                        break;
                    case "/remove_real":
                        keyboard_inline = await keyboards.DynamicButtonsForReal(message.From.Id);
                        await Bot.SendTextMessageAsync(message.From.Id, Reply.RemoveCryptoInstruction, replyMarkup: keyboard_inline);
                        user_status[message.From.Id] = "remove_real_mode";
                        break;
                    case "/change_crypto":
                        keyboard_inline = await keyboards.DynamicButtonsForCrypto(message.From.Id);
                        await Bot.SendTextMessageAsync(message.From.Id, Reply.ChangeCryptoInstruction, replyMarkup: keyboard_inline);
                        user_status[message.From.Id] = "change_crypto_mode";
                        break;
                    case "/crypto":
                        keyboard_inline = await keyboards.DynamicButtonsForCrypto(message.From.Id);
                        await Bot.SendTextMessageAsync(message.From.Id, Reply.CryptoInstruction, replyMarkup: keyboard_inline);
                        user_status[message.From.Id] = "crypto_mode";
                        break;
                    default:
                        await Bot.SendTextMessageAsync(message.From.Id, Reply.ChooseCommandPlease);
                        break;
                }
            }
            catch { return; }
        }
    }
}