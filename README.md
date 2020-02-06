# MyScore (библиотека помогает получить матчи с сервиса)

Бибилотека предназначена для получения матчей с сервиса и получением статистики о них.

Подключить 
```C#
using MyScoreApi;
```

Для получения всех матчей на следующий день
```C#
MyScore myScore = new MyScore();
var ss = await myScore.GetMatches(true);//Получить все матчи на следующий день
var infos = await ss.GetInfo();//Получить всю информацию о матче
```
Для получение матчей за определенную промежуток времени на текущий день
```C#
MyScore myScore = new MyScore();
var matches = await myScore.GetMatches();//Получить на текущий день
var info = await matches.GetNearest(DateTime.Now.AddHours(4)).GetInfo();//Получить на 4 часа от текущего времени, информацию об этих матчах
```

Можем получить информацию  на перед в нескольи вариантах
```C#
public static List<MatchModels> GetNearest(this List<MatchModels> MatchesToday, DateTime end);//На сколько часов вперед
public static List<MatchModels> GetNearest(this List<MatchModels> MatchesToday, DateTime start, DateTime end);//Со скольки и до
public static List<MatchModels> GetNearest(this List<MatchModels> MatchesToday, NearestMatchesModels nearestMatche);//Модель в которой указываем по желанию часы/минуты
public static List<MatchModels> GetNearest(this List<MatchModels> MatchesToday, int minutes = 60);//На сколько минут вперед
```

Готовый пример
```C#
private async static void  Test()
{
    MyScore myScore = new MyScore();
    var matches = await myScore.GetMatches(true);
    var info = await matches.GetNearest(DateTime.Now.AddHours(4)).GetInfo();

    Console.Clear();

    foreach ( var match in info )
    {
        string test = "";

        test += ("name: " + match.Name + "\n");
        test += ("time: " + match.DateStart + "\n");
        test += ("liga: " + match.Liga + "\n");
        test += ("link: " + match.Link + "\n");

        foreach ( var matchTotal in match.Bookmaker )
        {
            test += ("key:" + matchTotal.Coef + "\n");
            foreach ( var val in matchTotal.Total )
            {
                test += ("info: " + val.BkName + " | " + val.Less + " | " + val.More + "\n");
            }
        }
        Console.WriteLine(test + "\n");
    }
}
```

Что получилось в итоге
```name: Параду - Бискра
time: 2/6/2020 7:58:51 PM
liga: Первый дивизион - Тур 16
link: https://...
key:0.5
info: 1xBet | 7.5 | 1.05
info: bet365 | 7.5 | 1.07
info: Winline | 6.51 | 1.05
info: Betfair | 7 | 1.07
key:1
info: 1xBet | 6.15 | 1.08
info: Parimatch | 6 | 1.11
key:1.5
info: 1xBet | 2.74 | 1.38
info: bet365 | 2.75 | 1.4
info: Winline | 2.63 | 1.4
info: Betfair | 2.75 | 1.4
info: Parimatch | 2.7 | 1.44
key:1.75
info: Winline | 2.32 | 1.51
key:2
info: 1xBet | 2.07 | 1.68
info: Winline | 2.04 | 1.7
info: Parimatch | 2 | 1.75
key:2.25
info: Winline | 1.74 | 1.98
info: Parimatch | 1.72 | 2.05
key:2.5
info: 1xBet | 1.6 | 2.34
info: bet365 | 1.61 | 2.25
info: Winline | 1.52 | 2.3
info: Betfair | 1.53 | 2.45
info: Parimatch | 1.57 | 2.35
key:2.75
info: Winline | 1.39 | 2.67
key:3
info: 1xBet | 1.27 | 3.38
info: Parimatch | 1.27 | 3.6
key:3.5
info: 1xBet | 1.18 | 4.12
info: bet365 | 1.2 | 4.33
info: Winline | 1.15 | 4.19
info: Betfair | 1.17 | 4.5
info: Parimatch | 1.18 | 4.3
key:4
info: 1xBet | 1.05 | 7.5
key:4.5
info: 1xBet | 1.03 | 9
info: bet365 | 1.07 | 7.5
info: Winline | 1.04 | 7.67
info: Betfair | 1.04 | 9.5
key:5.5
info: 1xBet | 1.02 | 15
info: bet365 | 1.01 | 12
info: Winline | 1.01 | 11
info: Betfair | 1.01 | 15
```
Все методы асинхронные. Время получения матчей и информации о всех матчах зависит от количества матчей в день.

### Используются библиотеки:
1. Flurl, Flurl.http - запросы
2. Anglesharp - парсинг
