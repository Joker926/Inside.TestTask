# Inside.TestTask
Для выполнения требования: «После остановки контейнеров с микросервисами и окружением база данных должна быть доступна для просмотра средствами СУБД.» База данных должна быть развернута отдельно от окружения и микро-сервисов находящихся внутри docker-compose.
1.	В докере создается сетка  
docker network create test-insider --subnet=172.21.0.0/16
2.	Поднимается образ MariaDB 
docker run --net test-insider --ip 172.21.0.2  -p 3306:3306 --name some-mariadb  -e MARIADB_ROOT_PASSWORD=my-secret-pw -d mariadb
3.	Подключившись любым инструментом к БД выполнить скрипт из файла DB_create_scrypt
4.	Клонировать репозиторий
5.	Запустить команду docker compose build
6.	Запустить команду docker compose up
(пункты 6 и 7 выполняются в директории в которой расположены файлы docker-compose.yml и docker-compose.override.yml)
Для решения доступны сваггеры по адресу http://localhost:5001/swagger/index.html
