Доброго времени суток.

В данном репозитории представлено само выполнененное задание и небольшие тесты.

Для удобства создан запрос на создание базы данных и таблиц (https://github.com/Heavy32/TadosCatFeeding/blob/master/TableCreation.sql).
А так же диаграма таблиц (https://github.com/Heavy32/TadosCatFeeding/blob/master/Database.png)
Для того, чтобы протестировать самому, нужно в appsettings.json указать сервер, на котором вы работаете. 
Все можно протестировать через Postman (я так тестировал).

Задания:

1)  "Одноранговая ролевая модель, но нужно учесть, что в будущем возможно разграничение прав по разным уровням учеток — админ, пользователь, et cetera;"
У каждого пользователя есть роль, это выраженно в виде enum, в дальнейшем можно сделать ограничения к action по атрибуту {Authorize("Roles = "exampleRole")]
https://github.com/Heavy32/TadosCatFeeding/blob/master/Services/UserManagement/UserModels.cs/UserServiceModel.cs

2) "CRUD для учетных записей"
Реализацию на уровне базы данных можно найти на уровне DataBaseManagement
https://github.com/Heavy32/TadosCatFeeding/tree/master/DataBaseManagement

3) Добавление питомцев
https://github.com/Heavy32/TadosCatFeeding/blob/master/DataBaseManagement/CatManagement/CatRepository.cs

4) Добавление приёмов пищи
https://github.com/Heavy32/TadosCatFeeding/blob/master/DataBaseManagement/CatFeedingManagement/CatFeedingRepository.cs

5) Авторизация по логину и паролю. Результат — token, с которым можно проводить дальнейшие операции. Функция LogInAsync()
https://github.com/Heavy32/TadosCatFeeding/blob/master/Services/UserManagement/UserEntranceProvider.cs

6) Добавить прием пищи. Метод Feed
https://github.com/Heavy32/TadosCatFeeding/blob/master/Services/CatFeedingManagement/CatFeedingService.cs

7) Вернуть все приемы пищи за указанный диапазон дат. функция  GetFeedingForPeriodAsync
https://github.com/Heavy32/TadosCatFeeding/blob/master/Services/CatFeedingManagement/CatFeedingService.cs

8) "Нужно сделать метод, который возвращает число, насколько раз в среднем котов чаще кормят в выходные, чем в будни. Например, если кота в среднем в будни кормят 3 раза, а в выходные 5, то результат — 2. Результат может быть отрицательным. Округлять нужно до первого знака после запятой в большую сторону."
Создана отдельная таблица для таких запросов, таблица и запрос находятся в файле https://github.com/Heavy32/TadosCatFeeding/blob/master/TableCreation.sql
Чтобы вызвать данный запрос нужно вызвать метод Execute с айди запроса.
Файл: https://github.com/Heavy32/TadosCatFeeding/blob/master/Presentation/CatFeedingManagement/%D0%A1atFeedingController.cs
запрос: GET cats/feedings/statistics/{statisticId}

9) "Метод для расшаривания питомца с другой учётной записью. В таком случае пользователь получает доступ к истории приёмов пищи, получает возможность работать с приёмами пищи для данного питомца. Расшарить может только владелец питомца." 
Сделано это с помощью добавления в таблицу-связь UsersPets.
https://github.com/Heavy32/TadosCatFeeding/blob/master/Services/CatSharingManagement/CatSharingService.cs
Функция Share 
