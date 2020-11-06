Доброго времени суток.

В данном репозитории представлено само выполнененное задание и небольшие тесты к CRUD методам аккаунта.

Для удобства создан запрос на создание базы данных и таблиц (https://github.com/Heavy32/TadosCatFeeding/blob/master/TableCreation.sql). Для того, чтобы протестировать самому, нужно в appsettings.json указать сервер, на котором вы работаете. 
Все можно протестировать через Postman (я так тестировал).

Задания:

1)  "Одноранговая ролевая модель, но нужно учесть, что в будущем возможно разграничение прав по разным уровням учеток — админ, пользователь, et cetera;"
У каждого пользователя есть роль, в дальнейшем можно сделать ограничения к action по атрибуту {Authorize("Roles = "exampleRole")]
https://github.com/Heavy32/TadosCatFeeding/blob/master/TadosCatFeeding/Models/Account.cs

2) "CRUD для учетных записей"
https://github.com/Heavy32/TadosCatFeeding/blob/master/TadosCatFeeding/CRUDoperations/AccountCRUD.cs

3) Добавление питомцев
https://github.com/Heavy32/TadosCatFeeding/blob/master/TadosCatFeeding/CRUDoperations/PetCRUD.cs

4) Добавление приёмов пищи
https://github.com/Heavy32/TadosCatFeeding/blob/master/TadosCatFeeding/CRUDoperations/PetFeedingCRUD.cs

Где throw new NotImplementedException() сделано специально, так как по ТЗ надо будет иметь возможность допиливать CRUD

5) Авторизация по логину и паролю. Результат — token, с которым можно проводить дальнейшие операции. Функция LogIn() (строчка 25)
https://github.com/Heavy32/TadosCatFeeding/blob/master/TadosCatFeeding/Controllers/AccountController.cs

6) Добавить прием пищи. Метод FeedPet (строчка 19)
https://github.com/Heavy32/TadosCatFeeding/blob/master/TadosCatFeeding/Controllers/PetFeedingController.cs

7) Вернуть все приемы пищи за указанный диапазон дат. функция  GetFeedingForPeriod() (строчка 53)
https://github.com/Heavy32/TadosCatFeeding/blob/master/TadosCatFeeding/Controllers/StatisticsController.cs

8) "Нужно сделать метод, который возвращает число, насколько раз в среднем котов чаще кормят в выходные, чем в будни. Например, если кота в среднем в будни кормят 3 раза, а в выходные 5, то результат — 2. Результат может быть отрицательным. Округлять нужно до первого знака после запятой в большую сторону."
https://github.com/Heavy32/TadosCatFeeding/blob/master/TadosCatFeeding/Controllers/StatisticsController.cs
Функция GetDifferenceBetweenWeekendsAndWeekDays() (строчка 22)

9) "Метод для расшаривания питомца с другой учётной записью. В таком случае пользователь получает доступ к истории приёмов пищи, получает возможность работать с приёмами пищи для данного питомца. Расшарить может только владелец питомца." 
Сделано это с помощью добавления в таблицу-связь UsersPets.
https://github.com/Heavy32/TadosCatFeeding/blob/master/TadosCatFeeding/Controllers/PetController.cs
Функция SharePetWith (строчка 32)

Есть над чем работать ещё:
1) написать более внятные http.response
2) переделать тестирование, замокать объект работы с бд
3) сделать DI в контроллерах

Можно ещё найти минусы, но они уже локальные
