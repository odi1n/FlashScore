# MyScore - получение матче, БМ, 1х2, H2H

Бибилотека предназначена для получения матчей с сервиса MyScore и получением статистики о них.

Получить можно следующую информацию:
1. H2H
2. Коэфициенты:
    - 1х2
    - БМ

### Подключение
```C#
using MyScore;
```

### Получить все Матчи
```C#
MyScoreApi myScore = new MyScoreApi();
var matches = await myScore.GetAllMatchesAsync(); //Получить матчи на текущий день
var matches = await myScore.GetAllMatchesAsync(true); //Получить матчи на следующий день
```

### Получение информации о всех матчах
Мы можем получить информацию о всех матчах одним методом
```C#
 public static async Task<List<MatchModels>> GetInfoAsync(this List<MatchModels> MatchesToday, bool info = true, bool fds = true, bool bm = true,bool h2h = false)
```

По умолчанию мы получаем информацию:1х2, БМ. Так же можем указать нужные нам параметр
```C#
var infoMatch = matches.GetInfoAsync(info: false, fds: false, bm: true, h2h: true);
```

Имеются так же следующие методы которыми можем получить информацию об определенных параметрах
```c#
var infoMatch = matches.GetH2H();//Получаем h2h
var infoMatch = matches.GetCoefficient(true, true);//Получаем 1x2, БМ
```

### Получение об одном матче
Все так же как и со всей информацией
```C#
public async Task<MatchModels> GetAllInfoAsync(bool info = true, bool fds = true, bool bm = true, bool h2h = false)
```

По умолчанию получаем все те же параметры
```C#
var match = matches[0].GetAllInfoAsync(info: false, fds: false, bm: true, h2h: true);
```

Имеются методы для получения информации отдельно.
```c#
var match = matches[0].GetMatchInfoAsync();
var match = matches[0].GetPageCoefficient(true,true);
var match = matches[0].GetH2HAsync();
```

#### Пример
```C#
foreach(var match in matches )
{
	await match.GetAllInfoAsync(true, fds:true, bm:false, h2h:true);
}
```

### Матчи можем быстро отсортировать по дате исопльзуя следующие методы
```C#
public static List<MatchModels> GetNearest(DateTime end);//На сколько часов вперед
public static List<MatchModels> GetNearest(DateTime start, DateTime end);//Со скольки и до
public static List<MatchModels> GetNearest(NearestMatchesModels nearestMatche);//Модель в которой указываем по желанию часы/минуты
public static List<MatchModels> GetNearest( int minutes = 60);//На сколько минут вперед
```

### Пример. Вывод матчей в консоль
```C#
async static void  Test()
        {
            MyScoreApi myScore = new MyScoreApi();
            var matches = await myScore.GetAllMatchesAsync();
            var info = await  matches.GetInfoAsync(h2h: true);

            foreach ( var match in info )
            {
                string test = "";

                test += "name: " + match.Match.Name + "\n";
                test += "time: " + match.Match.DateStart + "\n";
                test += "liga: " + match.Match.Liga + "\n";
                test += "link: " + match.Link + "\n";

                foreach ( var matchTotal in match.Coefficient.BM )
                {
                    test += "key:" + matchTotal.Total + "\n";
                    test += "bk:" + matchTotal.BkName + "\n";
                    test += "more:" + matchTotal.More + "\n";
                    test += "less:" + matchTotal.More + "\n";
                }
                Console.WriteLine(test + "\n");
            }
        }
```

Все методы асинхронные. Время получения матчей и информации о всех матчах зависит от количества матчей в день.

### Используются библиотеки:
1. Flurl, Flurl.http - запросы
2. Anglesharp - парсинг
