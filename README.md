# MyScore (библиотека помогает получить матчи с сервиса)

Бибилотека предназначена для получения матчей с сервиса и получением статистики о них.

Подключить 
```C#
using MyScoreApi;
```

Для получения всех матчей на следующий день
```C#
MyScore myScore = new MyScore();
var ss = await myScore.GetMatches();//Получить все матчи на текущий день
var infos = await ss.GetInfo();//Получить всю информацию о матче
```
Для получение матчей за определенную промежуток времени на текущий день
```C#
MyScore myScore = new MyScore();
var matches = await myScore.GetMatches();//Получить на следующий день
var info = await matches.GetNearest(DateTime.Now.AddHours(4)).GetInfo();//Получить на 4 часа от текущего времени, информацию об этих матчах
```

Можем получить информацию  на перед в нескольи вариантах
```C#
public static List<MatchModels> GetNearest(this List<MatchModels> MatchesToday, DateTime end);//На сколько часов вперед
public static List<MatchModels> GetNearest(this List<MatchModels> MatchesToday, DateTime start, DateTime end);//Со скольки и до
public static List<MatchModels> GetNearest(this List<MatchModels> MatchesToday, NearestMatchesModels nearestMatche);//Модель в которой указываем по желанию часы/минуты
public static List<MatchModels> GetNearest(this List<MatchModels> MatchesToday, int minutes = 60);//На сколько минут вперед
```

Пример готового примера
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
Все методы асинхронные. Время получения матчей и информации о всех матчах зависит от количества матчей в день.

Используются библиотеки:
1. Flurl, Flurl.http
2. Anglesharp
