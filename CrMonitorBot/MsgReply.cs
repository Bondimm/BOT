

namespace CrMonitorBot
{
    class MsgReply
    {
        public string startReply =
@"Этот бот нужен для удобного мониторинга цены различных криптовалют
Список команд:
/start - запуск бота
/crypto - вывести отслеживаемую криптовалюту
/add_crypto - добавить криптовалюту к отслеживаемым
/add_real - добавить валюту, в которую будут переводится криптовалюты
/remove_crypto - удалить криптовалюту из отслеживаемых
/remove_real - удалить реальную валюту
/show_supported_cryptos - показать все поддерживаемые криптовалюты
/show_info_crypto - показать информацию про криптовалюту (НА АНГЛИЙСКОМ)
/change_crypto - поменять отслеживаемую криптовалюту на другую"
;
        public string Inline_Text =
@"Доступные криптовалюты:"
;
        public string CoinInfoInstruction =
@"Введите полное название криптовалют, доступных из списка /show_supported_cryptos, например 'Bitcoin', или 'XRP'."
;
        public string CoinInfoError =
@"Произошла ошибка! Проверьте правильность написания и попробуйте еще раз. /show_info_crypto"
;
        public string Success =
@"Успех!!!"
;
        public string AddCryptoFailure =
@"Произошла ошибка! Проверьте правильность написания и попробуйте еще раз. /add_crypto"
;
        public string AddCryptoFailure_AlreadyExists =
@"Произошла ошибка! Криптовалюта, которую Вы добавляете, уже в списке отслеживаемых. /add_crypto"
;
        public string AddRealInstruction =
@"Введите аббревиатуру валюты, которую вы хотите добавить. 
На данный момент доступны: USD, EUR, PLN, KRW, GBP, CAD, JPY, RUB, TRY, NZD, AUD, CHF, UAH, HKD, SGD, NGN, PHP, MXN, BRL, THB, CLP, CNY, CZK, DKK, HUF, IDR, ILS, INR, MYR, NOK, PKR, SEK, TWD, ZAR, VND, BOB, COP, PEN, ARS, ISK."
;
        public string AddRealFailure =
@"Произошла ошибка! Проверьте правильность написания и попробуйте еще раз. /add_real"
;
        public string AddRealFailure_AlreadyExists =
@"Произошла ошибка! Валюта, которую Вы добавляете, уже добавлена. /add_real"
;
        public string RemoveCryptoInstruction =
@"Выберите криптовалюту, которую хотите удалить."
;
        public string RemoveCryptoFailure =
@"Произошла ошибка! Отслеживаемых криптовалют не может быть меньше одной!"
;
        public string ChooseCommandPlease =
@"Команда не выбрана! Пожалуйста, выберите команду из списка команд."
;
        public string DeletionError =
@"Произошла ошибка! Попробуйте еще раз. /remove_crypto"
;
        public string RemoveRealInstruction =
@"Выберите валюту, которую хотите удалить."
;
        public string RemoveRealFailure =
@"Произошла ошибка! Отслеживаемых валют не может быть меньше одной!"
;
        public string ChangeCryptoInstruction =
@"Выберите криптовалюту, которую хотите заменить."
;
        public string ChangeCryptoInstruction2 =
@"Введите криптовалюту, на которую хотите заменить из списка /show_supported_cryptos, например 'Bitcoin', или 'XRP'."
;
        public string ChangeCryptoFailure =
@"Произошла ошибка! Проверьте правильность написания и попробуйте еще раз. /change_crypto"
;
        public string ChangeCryptoFailure_AlreadyExists =
@"Произошла ошибка! Криптовалюта, на которую Вы пытаетесь заменить, уже в списке отслеживаемых. /change_crypto"
;
        public string CryptoInstruction =
@"Выберите криптовалюту."
;
        public string ButtonState =
@"Пожалуйста, выберите наименование из списка выше, или выберите новую команду из списка команд."
;
        public string ButtonState_supportedcrypto =
@"Пожалуйста, нажмите кнопку 'Вперед' или 'Назад', или выберите новую комманду из списка команд."
;
        public string Error_CurrNotFound =
@"Произошла ошибка! Валюта, которую Вы пытаетесь удалить, отсутствует."
;
        public string Error_CryCurrNotFound =
@"Произошла ошибка! Криптовалюта, которую Вы пытаетесь удалить, отсутствует."
;
    }
}
