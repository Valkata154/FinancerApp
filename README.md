# FinancerApp

WPF Application, which gathers real-time information on stocks from the YahooFinanceAPI. Stocks can be added to a Watchlist. Stock alarms can be created to send notifications on Whatsapp. The application is connected to a DB, which saves all the tickers and their alarms.

![image](https://user-images.githubusercontent.com/45520042/101022096-97dba000-3579-11eb-85c5-48cd9ddb706e.png)
### How to run
- Create a Twillio account and add your SID and TOKEN to the AlarmWindow.xaml.cs
- Add your phone number to to the AlarmWindow.xaml.cs
- For more information on using Twillio: https://www.twilio.com/docs/usage/api

### Watchlist

The watchlist shows the Stock Symbol, the current price, open price and the volume. Updates every 1 second (this can be modified in code).
- If the ticker symbol is red, that means that the Current Price is lower than the Open Price. If it is green the Current Price is higher than the Open Price
- When the market opens the Current Price changes from red to green and green to red every second based on the previous current price. This indicates if the price is going low or high.
- Tickers are added by clicking the "+" symbol and entering the name of the ticker. (example: "ACB")
- Deleting a ticker can be done by double-clicking on the ListView item.

![image](https://user-images.githubusercontent.com/45520042/101021653-da50ad00-3578-11eb-9655-f8fcd749e6e7.png)

### Alarms
- Multiple alarms can be added to various tickers.
- A whatsapp message is sent to the number when the ticker goes above or bellow the given target price alarm.
- The check method runs every 2 secs by retrieving the live data and sending notifications if alarm reached. (seconds can be modified in the code)
- Alarms are added by clicking on the "+" symbol and entering the Name and Alarm Price for a ticker.
- Alarms can be deleted by double clicking on the ListView item.
- WhatsApp message format is: TickerName CurrentPrice is ABOVE/BELLOW TargetPrice alarm.

![image](https://user-images.githubusercontent.com/45520042/101022005-7084d300-3579-11eb-814a-4ef3ce91db0f.png)
![image](https://user-images.githubusercontent.com/45520042/101022798-8a72e580-357a-11eb-9876-b5fd30626c10.png)


![image](https://user-images.githubusercontent.com/45520042/101022507-25b78b00-357a-11eb-8f52-a3bee5a6970b.png)
