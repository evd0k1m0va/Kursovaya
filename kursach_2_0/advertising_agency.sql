DROP DATABASE IF EXISTS advertising_agency;
CREATE DATABASE advertising_agency
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_unicode_ci;

USE advertising_agency;

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET FOREIGN_KEY_CHECKS = 0;


CREATE TABLE area (
  id INT NOT NULL AUTO_INCREMENT,
  name VARCHAR(100) NOT NULL,
  description TEXT,
  PRIMARY KEY (id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE street (
  id INT NOT NULL AUTO_INCREMENT,
  street VARCHAR(100) NOT NULL,
  district VARCHAR(100) NOT NULL,
  PRIMARY KEY (id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE post (
  id INT NOT NULL AUTO_INCREMENT,
  title VARCHAR(100) NOT NULL,
  salary DECIMAL(10,2) NOT NULL DEFAULT 0,
  PRIMARY KEY (id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE typepay (
  id INT NOT NULL AUTO_INCREMENT,
  type VARCHAR(50) NOT NULL,
  PRIMARY KEY (id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;


CREATE TABLE rental (
  id INT NOT NULL AUTO_INCREMENT,
  name VARCHAR(100) NOT NULL,
  status VARCHAR(50) NOT NULL,
  office VARCHAR(100) NOT NULL,
  fio VARCHAR(100) NOT NULL,
  phone_number1 VARCHAR(15) NOT NULL,
  responsible VARCHAR(100) NOT NULL,
  phone_number2 VARCHAR(15) NOT NULL,
  bank VARCHAR(100) NOT NULL,
  bank_account VARCHAR(20) NOT NULL,
  inn VARCHAR(13) NOT NULL,
  PRIMARY KEY (id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `optional` (
  id INT NOT NULL AUTO_INCREMENT,
  service VARCHAR(100) NOT NULL,
  cost INT NOT NULL,
  term DATE NOT NULL,
  count INT NOT NULL,
  PRIMARY KEY (id),
  CONSTRAINT chk_optional_cost CHECK (cost >= 0),
  CONSTRAINT chk_optional_count CHECK (`count` > 0)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE employee (
  id INT NOT NULL AUTO_INCREMENT,
  name VARCHAR(50) NOT NULL,
  surname VARCHAR(50) NOT NULL,
  residential_address VARCHAR(100) NOT NULL,
  datebirth DATE NOT NULL,
  post_id INT NOT NULL,
  salary INT NOT NULL,
  PRIMARY KEY (id),
  CONSTRAINT fk_employee_post FOREIGN KEY (post_id) REFERENCES post(id)
    ON UPDATE RESTRICT ON DELETE RESTRICT,
  CONSTRAINT chk_employee_salary CHECK (salary >= 0)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE work_record (
  id INT NOT NULL AUTO_INCREMENT,
  employee_id INT NOT NULL,
  post_id INT NOT NULL,
  start_date DATE NOT NULL,
  end_date DATE,
  PRIMARY KEY (id),
  CONSTRAINT fk_wr_employee FOREIGN KEY (employee_id) REFERENCES employee(id)
    ON UPDATE CASCADE ON DELETE CASCADE,
  CONSTRAINT fk_wr_post FOREIGN KEY (post_id) REFERENCES post(id)
    ON UPDATE RESTRICT ON DELETE RESTRICT,
  CONSTRAINT chk_work_dates CHECK (end_date IS NULL OR end_date >= start_date)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE billboards (
  id INT NOT NULL AUTO_INCREMENT,
  address VARCHAR(100) NOT NULL,
  location_description VARCHAR(100) NOT NULL,
  usable_area FLOAT NOT NULL,
  street_id INT NOT NULL,
  PRIMARY KEY (id),
  CONSTRAINT fk_billboards_street FOREIGN KEY (street_id) REFERENCES street(id)
    ON UPDATE RESTRICT ON DELETE RESTRICT,
  CONSTRAINT chk_billboards_area CHECK (usable_area > 0)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE contract (
  id INT NOT NULL AUTO_INCREMENT,
  dateorder DATE NOT NULL,
  cost DECIMAL(10,2) NOT NULL DEFAULT 0,
  renter_id INT NOT NULL,
  employee_id INT NOT NULL,
  typepay_id INT NOT NULL,
  optional_id INT,
  PRIMARY KEY (id),
  CONSTRAINT fk_contract_renter FOREIGN KEY (renter_id) REFERENCES rental(id)
    ON UPDATE RESTRICT ON DELETE RESTRICT,
  CONSTRAINT fk_contract_employee FOREIGN KEY (employee_id) REFERENCES employee(id)
    ON UPDATE RESTRICT ON DELETE RESTRICT,
  CONSTRAINT fk_contract_typepay FOREIGN KEY (typepay_id) REFERENCES typepay(id)
    ON UPDATE RESTRICT ON DELETE RESTRICT,
  CONSTRAINT fk_contract_optional FOREIGN KEY (optional_id) REFERENCES `optional`(id)
    ON UPDATE SET NULL ON DELETE SET NULL,
  CONSTRAINT chk_contract_cost CHECK (cost >= 0)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE orders (
  id INT NOT NULL AUTO_INCREMENT,
  contract_id INT NOT NULL,
  billboard_id INT NOT NULL,
  count INT NOT NULL,
  cost INT NOT NULL,
  street_id INT NOT NULL,
  dateorder TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  startdate DATE NOT NULL,
  enddate DATE NOT NULL,
  pictures VARCHAR(250) NOT NULL,
  PRIMARY KEY (id),
  CONSTRAINT fk_orders_contract FOREIGN KEY (contract_id) REFERENCES contract(id)
    ON UPDATE CASCADE ON DELETE CASCADE,
  CONSTRAINT fk_orders_billboard FOREIGN KEY (billboard_id) REFERENCES billboards(id)
    ON UPDATE RESTRICT ON DELETE RESTRICT,
  CONSTRAINT fk_orders_street FOREIGN KEY (street_id) REFERENCES street(id)
    ON UPDATE RESTRICT ON DELETE RESTRICT,
  CONSTRAINT chk_orders_qty CHECK (`count` > 0),
  CONSTRAINT chk_orders_cost CHECK (cost >= 0),
  CONSTRAINT chk_orders_dates CHECK (enddate >= startdate)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE pricelist (
  id INT NOT NULL AUTO_INCREMENT,
  billboard_id INT NOT NULL,
  cost INT NOT NULL,
  optional_id INT NOT NULL,
  PRIMARY KEY (id),
  CONSTRAINT fk_pricelist_billboard FOREIGN KEY (billboard_id) REFERENCES billboards(id)
    ON UPDATE RESTRICT ON DELETE RESTRICT,
  CONSTRAINT fk_pricelist_optional FOREIGN KEY (optional_id) REFERENCES `optional`(id)
    ON UPDATE RESTRICT ON DELETE RESTRICT,
  CONSTRAINT chk_pricelist_cost CHECK (cost >= 0)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------
-- Users
-- --------------------------------------------------------

CREATE TABLE IF NOT EXISTS user_registration (
  id INT NOT NULL AUTO_INCREMENT,
  login VARCHAR(50) NOT NULL,
  password_hash VARCHAR(64) NOT NULL,
  role VARCHAR(20) NOT NULL DEFAULT 'user',
  permissions VARCHAR(10) DEFAULT '0000',

  surname VARCHAR(50) NULL,
  username VARCHAR(50) NULL,
  number VARCHAR(20) NULL,
  created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  date_of_birth DATE NULL,

  PRIMARY KEY (id),
  UNIQUE KEY uq_user_login (login)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE app_menu (
  id INT NOT NULL AUTO_INCREMENT,
  title VARCHAR(100) NOT NULL,
  role_scope ENUM('all','admin','user') DEFAULT 'all',
  control_type VARCHAR(255) NOT NULL,
  sort_order INT DEFAULT 0,
  is_enabled TINYINT(1) DEFAULT 1,
  PRIMARY KEY (id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;


ALTER TABLE rental
  ADD CONSTRAINT uq_rental_inn UNIQUE (inn);

ALTER TABLE post
  ADD CONSTRAINT uq_post_title UNIQUE (title);

ALTER TABLE typepay
  ADD CONSTRAINT uq_typepay_type UNIQUE (type);

ALTER TABLE street
  ADD CONSTRAINT uq_street_district UNIQUE (street, district);

ALTER TABLE billboards
  ADD CONSTRAINT uq_billboards_address UNIQUE (address);

SET FOREIGN_KEY_CHECKS = 1;

START TRANSACTION;

INSERT INTO user_registration (login, password_hash, role, permissions, surname, username)
SELECT 'admin',
       '240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9',
       'admin','1111','Администратор','Системы'
WHERE NOT EXISTS (SELECT 1 FROM user_registration WHERE login='admin');

INSERT INTO user_registration (login, password_hash, role, permissions, surname, username)
SELECT 'viewer',
       '240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9',
       'user','1000','Иванов','Иван'
WHERE NOT EXISTS (SELECT 1 FROM user_registration WHERE login='viewer');

INSERT INTO user_registration (login, password_hash, role, permissions, surname, username)
SELECT 'editor_add',
       '240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9',
       'user','1100','Петров','Пётр'
WHERE NOT EXISTS (SELECT 1 FROM user_registration WHERE login='editor_add');

INSERT INTO user_registration (login, password_hash, role, permissions, surname, username)
SELECT 'editor_full',
       '240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9',
       'user','1110','Смирнова','Анна'
WHERE NOT EXISTS (SELECT 1 FROM user_registration WHERE login='editor_full');

INSERT INTO user_registration (login, password_hash, role, permissions, surname, username)
SELECT 'blocked',
       '240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9',
       'user','0000','Заблокирован','Пользователь'
WHERE NOT EXISTS (SELECT 1 FROM user_registration WHERE login='blocked');


INSERT INTO app_menu (title, role_scope, control_type, sort_order) VALUES
('Заказы','all','kursach_2_0.OrdersControl',10),
('Разное','all','kursach_2_0.OtherControl',20),
('Справочники','all','kursach_2_0.DirectoriesControl',30),
('Документы','all','kursach_2_0.DocumentsControl',40),
('Сотрудники','admin','kursach_2_0.EmployeesControl',50),
('Справка','all','kursach_2_0.ReferenceControlHost',60);


INSERT INTO street (street,district) VALUES
('Ленина','Центр'),('Мира','Север'),('Победы','Юг');

INSERT INTO post (title,salary) VALUES
('Менеджер',60000),('Оператор',45000),('Директор',90000);

INSERT INTO typepay (type) VALUES
('Полная'),('Еженедельная'),('Ежемесячная');

INSERT INTO `optional` (service,cost,term,count) VALUES
('Печать',5000,'2025-12-31',1),
('Монтаж',3000,'2025-12-31',1);

INSERT INTO employee (name,surname,residential_address,datebirth,post_id,salary) VALUES
('Иван','Иванов','ул. Ленина','1990-05-10',1,60000),
('Петр','Петров','ул. Мира','1988-03-22',2,45000);

INSERT INTO rental (name,status,office,fio,phone_number1,responsible,phone_number2,bank,bank_account,inn) VALUES
('ООО Реклама+','ООО','Офис','Иванов И.И.','111','Иванов','222','Сбербанк','40802','7701234567890');

INSERT INTO billboards (address,location_description,usable_area,street_id) VALUES
('Ленина,10','Перекресток',12.5,1);

INSERT INTO contract (dateorder,cost,renter_id,employee_id,typepay_id,optional_id)
VALUES (CURDATE(),0,1,1,1,1);

INSERT INTO orders (contract_id,billboard_id,count,cost,street_id,startdate,enddate,pictures)
VALUES (1,1,10,1000,1,'2025-01-01','2025-01-10','banner.jpg');


INSERT INTO area (name, description) VALUES
('Центр', 'Центральные районы, высокий трафик'),
('Север', 'Северная часть города'),
('Юг', 'Южная часть города'),
('Восток', 'Восточная часть города'),
('Запад', 'Западная часть города'),
('Промзона', 'Промышленные кварталы'),
('Спальный район', 'Жилые микрорайоны'),
('ТЦ зона', 'Рядом с торговыми центрами'),
('Вокзал', 'Ж/д вокзал и прилегающие улицы'),
('Аэропорт', 'Территория рядом с аэропортом');

INSERT INTO street (street, district) VALUES
('Советская','Центр'),
('Кирова','Центр'),
('Гагарина','Север'),
('Титова','Юг'),
('Станционная','Юг'),
('Гоголя','Центр'),
('Дуси Ковальчук','Север'),
('Блюхера','Юг'),
('Русская','Восток'),
('Зорге','Запад');

INSERT INTO post (title, salary) VALUES
('Бухгалтер',70000),
('Маркетолог',75000),
('Администратор',50000),
('Дизайнер',80000),
('Монтажник',55000),
('Юрист',90000),
('Логист',65000);

INSERT INTO typepay (type) VALUES
('Разовая'),
('Поэтапная'),
('По факту'),
('Безналичная'),
('Наличная'),
('Смешанная'),
('Предоплата');

INSERT INTO `optional` (service, cost, term, count) VALUES
('Дизайн макета',12000,'2025-12-31',1),
('Поклейка',8000,'2025-12-31',1),
('Доставка материалов',3000,'2025-12-31',1),
('Ламинация',6000,'2025-12-31',1),
('Срочное изготовление',9000,'2025-12-31',1),
('Фотосъёмка щита',5000,'2025-12-31',1),
('Корректировка макета',4000,'2025-12-31',1),
('Демонтаж',3000,'2025-12-31',1);

INSERT INTO employee (name, surname, residential_address, datebirth, post_id, salary) VALUES
('Анна','Смирнова','ул. Советская, 10','1994-08-30',3,70000),
('Дмитрий','Кузнецов','ул. Кирова, 11','1993-12-05',1,60000),
('Ольга','Соколова','ул. Гагарина, 5','1995-04-12',4,75000),
('Никита','Егоров','ул. Титова, 15','1992-03-19',5,50000),
('Людмила','Яковлева','ул. Станционная, 7','1991-07-07',6,80000),
('Павел','Климов','ул. Гоголя, 25','1990-05-10',7,55000),
('Мария','Фролова','ул. Дуси Ковальчук, 2','1998-11-03',8,90000),
('Алексей','Орлов','ул. Блюхера, 9','1988-03-22',9,65000);

INSERT INTO work_record (employee_id, post_id, start_date, end_date) VALUES
(1,1,'2022-01-10',NULL),
(2,2,'2022-02-01',NULL),
(3,3,'2022-03-15',NULL),
(4,1,'2021-11-01','2024-01-01'),
(4,4,'2024-01-02',NULL),
(5,5,'2023-05-10',NULL),
(6,6,'2023-06-01',NULL),
(7,7,'2023-07-20',NULL),
(8,8,'2023-08-01',NULL),
(9,9,'2023-09-01',NULL);

INSERT INTO rental (name,status,office,fio,phone_number1,responsible,phone_number2,bank,bank_account,inn) VALUES
('ЗАО Бета-Маркет','ЗАО','Офис 2','Петров П.П.','333','Сидоров','444','ВТБ','40803','7701234567891'),
('ИП Гамма','ИП','Офис 3','Смирнов С.С.','555','Смирнов','666','Альфа-Банк','40804','7701234567892'),
('ООО Дельта','ООО','Офис 4','Кузнецов К.К.','777','Кузнецов','888','Газпромбанк','40805','7701234567893'),
('ТОО Эпсилон','ТОО','Офис 5','Орлов О.О.','999','Орлов','000','Райффайзен','40806','7701234567894'),
('ООО Зета','ООО','Офис 6','Федоров Ф.Ф.','101','Федоров','202','Открытие','40807','7701234567895'),
('ЗАО Эта','ЗАО','Офис 7','Егоров Е.Е.','303','Егоров','404','Тинькофф','40808','7701234567896'),
('ИЧП Тета','ИЧП','Офис 8','Титов Т.Т.','505','Титов','606','ПСБ','40809','7701234567897'),
('ООО Йота','ООО','Офис 9','Яковлев Я.Я.','707','Яковлев','808','Сбербанк','40810','7701234567898'),
('ООО Каппа','ООО','Офис 10','Крылов К.К.','909','Крылов','010','ВТБ','40811','7701234567899');

INSERT INTO billboards (address, location_description, usable_area, street_id) VALUES
('Мира, 5','Остановка',14.2,2),
('Победы, 12','У ТЦ',18.0,3),
('Советская, 8','Центр, перекрёсток',16.5,4),
('Кирова, 30','У метро',19.0,5),
('Гагарина, 7','Въезд в район',21.3,6),
('Титова, 55','Спальный район',13.8,7),
('Станционная, 1','Промзона',22.1,8),
('Гоголя, 25','Пешеходный поток',17.6,9),
('Дуси Ковальчук, 40','Развязка',23.4,10);

INSERT INTO contract (dateorder, cost, renter_id, employee_id, typepay_id, optional_id) VALUES
('2025-01-10',65000,2,2,2,2),
('2025-01-12',48000,3,3,3,3),
('2025-01-15',70000,4,4,4,4),
('2025-01-18',45000,5,5,5,5),
('2025-01-20',62000,6,6,6,6),
('2025-01-25',52000,7,7,7,7),
('2025-02-01',40000,8,8,8,8),
('2025-02-05',43000,9,9,9,9),
('2025-02-10',75000,10,10,10,10);

INSERT INTO orders (contract_id, billboard_id, count, cost, street_id, startdate, enddate, pictures) VALUES
(2,2,5,6500,2,'2025-01-12','2025-01-20','b2.jpg'),
(3,3,7,7000,3,'2025-01-15','2025-01-30','b3.jpg'),
(4,4,10,9000,4,'2025-01-20','2025-02-05','b4.jpg'),
(5,5,3,3500,5,'2025-01-25','2025-02-10','b5.jpg'),
(6,6,8,8000,6,'2025-02-01','2025-02-20','b6.jpg'),
(7,7,6,6000,7,'2025-02-05','2025-02-25','b7.jpg'),
(8,8,4,4200,8,'2025-02-10','2025-03-01','b8.jpg'),
(9,9,2,2500,9,'2025-02-12','2025-03-05','b9.jpg'),
(10,10,9,9500,10,'2025-02-15','2025-03-15','b10.jpg');

INSERT INTO pricelist (billboard_id, cost, optional_id) VALUES
(1,50000,1),
(2,65000,2),
(3,48000,3),
(4,70000,4),
(5,45000,5),
(6,62000,6),
(7,52000,7),
(8,40000,8),
(9,43000,9),
(10,75000,10);

COMMIT;

DROP VIEW IF EXISTS v_contracts_big;

CREATE VIEW v_contracts_big AS
SELECT
  id, dateorder, cost, renter_id, employee_id, typepay_id, optional_id
FROM contract
WHERE cost >= 50000
WITH CASCADED CHECK OPTION;

-- Просмотр содержимого VIEW
SELECT 'VIEW v_contracts_big:' AS marker;
SELECT * FROM v_contracts_big ORDER BY id;

-- Вставка строки, удовлетворяющей условию VIEW 
INSERT INTO v_contracts_big (dateorder, cost, renter_id, employee_id, typepay_id, optional_id)
VALUES ('2025-03-01', 55000, 1, 1, 1, 1);

SELECT 'VIEW after valid insert:' AS marker;
SELECT * FROM v_contracts_big ORDER BY id;

DROP TABLE IF EXISTS mv_contracts_big;

CREATE TABLE mv_contracts_big
AS
SELECT
  c.id, c.dateorder, c.cost,
  r.name AS renter_name,
  CONCAT(e.surname,' ',e.name) AS employee_fio,
  tp.type AS pay_type
FROM contract c
JOIN rental r ON r.id = c.renter_id
JOIN employee e ON e.id = c.employee_id
JOIN typepay tp ON tp.id = c.typepay_id
WHERE c.cost >= 50000;

ALTER TABLE mv_contracts_big
  ADD PRIMARY KEY (id);

DELIMITER $$

DROP PROCEDURE IF EXISTS refresh_mv_contracts_big $$
CREATE PROCEDURE refresh_mv_contracts_big()
BEGIN
  TRUNCATE TABLE mv_contracts_big;

  INSERT INTO mv_contracts_big (id, dateorder, cost, renter_name, employee_fio, pay_type)
  SELECT
    c.id, c.dateorder, c.cost,
    r.name AS renter_name,
    CONCAT(e.surname,' ',e.name) AS employee_fio,
    tp.type AS pay_type
  FROM contract c
  JOIN rental r ON r.id = c.renter_id
  JOIN employee e ON e.id = c.employee_id
  JOIN typepay tp ON tp.id = c.typepay_id
  WHERE c.cost >= 50000;
END $$

DELIMITER ;

-- Проверка MV (таблица-снимок)
SELECT 'MV mv_contracts_big initial:' AS marker;
SELECT * FROM mv_contracts_big ORDER BY id;

-- Добавляем новую подходящую строку в BASE TABLE contract
INSERT INTO contract (dateorder, cost, renter_id, employee_id, typepay_id, optional_id)
VALUES ('2025-03-03', 80000, 2, 2, 2, 2);

-- В MV её НЕ будет (пока не refresh)
SELECT 'MV before refresh (new row NOT visible):' AS marker;
SELECT * FROM mv_contracts_big ORDER BY id;

-- Обновление "материализованного представления"
CALL refresh_mv_contracts_big();

SELECT 'MV after refresh (new row visible):' AS marker;
SELECT * FROM mv_contracts_big ORDER BY id;

DELIMITER $$

-- Сумма по заказам в рамках договора (contract_id)
DROP FUNCTION IF EXISTS fn_contract_orders_sum $$
CREATE FUNCTION fn_contract_orders_sum(p_contract_id INT)
RETURNS DECIMAL(12,2)
DETERMINISTIC
READS SQL DATA
BEGIN
  DECLARE v_sum DECIMAL(12,2);
  SELECT COALESCE(SUM(o.cost * o.count), 0)
    INTO v_sum
  FROM orders o
  WHERE o.contract_id = p_contract_id;
  RETURN v_sum;
END $$

-- Количество договоров арендатора (renter_id)
DROP FUNCTION IF EXISTS fn_renter_contracts_count $$
CREATE FUNCTION fn_renter_contracts_count(p_renter_id INT)
RETURNS INT
DETERMINISTIC
READS SQL DATA
BEGIN
  DECLARE v_cnt INT;
  SELECT COUNT(*)
    INTO v_cnt
  FROM contract c
  WHERE c.renter_id = p_renter_id;
  RETURN v_cnt;
END $$

DELIMITER ;

-- Примеры вызова функций
SELECT 'Functions demo:' AS marker;
SELECT
  1 AS contract_id,
  fn_contract_orders_sum(1) AS orders_sum_for_contract_1;

SELECT
  1 AS renter_id,
  fn_renter_contracts_count(1) AS contracts_count_for_renter_1;

DELIMITER $$

DROP PROCEDURE IF EXISTS export_mv_contracts_big_to_csv $$
CREATE PROCEDURE export_mv_contracts_big_to_csv()
BEGIN
  DECLARE v_dir TEXT;
  DECLARE v_path TEXT;

  SELECT @@secure_file_priv INTO v_dir;

  -- Если secure_file_priv пустой, сервер запрещает OUTFILE
  IF v_dir IS NULL OR v_dir = '' THEN
    SIGNAL SQLSTATE '45000'
      SET MESSAGE_TEXT = 'secure_file_priv is empty. OUTFILE запрещён на сервере. Включи secure_file_priv или экспортируй через клиент.';
  END IF;

  -- ✅ Уникальное имя файла: mv_contracts_big_<uuid>.csv
  SET v_path = CONCAT(v_dir, 'mv_contracts_big_', REPLACE(UUID(),'-',''), '.csv');

  SET @sql = CONCAT(
    "SELECT id, dateorder, cost, renter_name, employee_fio, pay_type ",
    "FROM mv_contracts_big ORDER BY id ",
    "INTO OUTFILE '", REPLACE(v_path, "\\", "\\\\"), "' ",
    "FIELDS TERMINATED BY ';' ENCLOSED BY '", '"', "' ",
    "LINES TERMINATED BY '\n'"
  );

  PREPARE stmt FROM @sql;
  EXECUTE stmt;
  DEALLOCATE PREPARE stmt;

  -- показываем путь, куда выгрузили
  SELECT v_path AS exported_to;
END $$

DELIMITER ;

-- Вызов экспорта (создаст НОВЫЙ файл каждый раз)
CALL export_mv_contracts_big_to_csv();

USE advertising_agency;


DROP USER IF EXISTS 'viewer_dcl'@'localhost';
DROP USER IF EXISTS 'editor_dcl'@'localhost';
DROP USER IF EXISTS 'admin_dcl'@'localhost';

-- Создание пользователей
CREATE USER 'viewer_dcl'@'localhost' IDENTIFIED BY 'viewer_pass';
CREATE USER 'editor_dcl'@'localhost' IDENTIFIED BY 'editor_pass';
CREATE USER 'admin_dcl'@'localhost' IDENTIFIED BY 'admin_pass';

-- Назначение прав:
-- viewer_dcl: только чтение
GRANT SELECT ON advertising_agency.* TO 'viewer_dcl'@'localhost';

-- editor_dcl: чтение + добавление + изменение (без удаления)
GRANT SELECT, INSERT, UPDATE ON advertising_agency.* TO 'editor_dcl'@'localhost';

-- admin_dcl: полный доступ
GRANT ALL PRIVILEGES ON advertising_agency.* TO 'admin_dcl'@'localhost';

FLUSH PRIVILEGES;

-- Проверка назначенных прав:
SHOW GRANTS FOR 'viewer_dcl'@'localhost';
SHOW GRANTS FOR 'editor_dcl'@'localhost';
SHOW GRANTS FOR 'admin_dcl'@'localhost';

DELIMITER $$

DROP TRIGGER IF EXISTS trg_after_order_insert $$
DROP TRIGGER IF EXISTS trg_after_order_delete $$

-- AFTER INSERT: увеличиваем стоимость договора
CREATE TRIGGER trg_after_order_insert
AFTER INSERT ON orders
FOR EACH ROW
BEGIN
  UPDATE contract
  SET cost = cost + (NEW.cost * NEW.count)
  WHERE id = NEW.contract_id;
END $$

-- AFTER DELETE: уменьшаем стоимость договора
CREATE TRIGGER trg_after_order_delete
AFTER DELETE ON orders
FOR EACH ROW
BEGIN
  UPDATE contract
  SET cost = cost - (OLD.cost * OLD.count)
  WHERE id = OLD.contract_id;
END $$

DELIMITER ;

-- ===== Демонстрация триггеров =====
-- До
SELECT 'Contract cost BEFORE trigger test' AS marker;
SELECT id, cost FROM contract WHERE id = 1;

-- Добавляем заказ (сработает INSERT trigger)
INSERT INTO orders (contract_id, billboard_id, count, cost, street_id, startdate, enddate, pictures)
VALUES (1, 1, 2, 3000, 1, '2025-04-01', '2025-04-10', 'trigger_test.jpg');

-- После вставки
SELECT 'Contract cost AFTER INSERT trigger' AS marker;
SELECT id, cost FROM contract WHERE id = 1;

-- Удаляем заказ (сработает DELETE trigger)
DELETE FROM orders WHERE pictures = 'trigger_test.jpg';

-- После удаления
SELECT 'Contract cost AFTER DELETE trigger' AS marker;
SELECT id, cost FROM contract WHERE id = 1;


SELECT 'Salaries BEFORE COMMIT test' AS marker;
SELECT id, surname, salary FROM employee WHERE post_id = 1 ORDER BY id;

START TRANSACTION;

UPDATE employee
SET salary = salary + 5000
WHERE post_id = 1;

-- Проверка ДО фиксации
SELECT 'Salaries INSIDE transaction (before COMMIT)' AS marker;
SELECT id, surname, salary FROM employee WHERE post_id = 1 ORDER BY id;

COMMIT;

-- Проверка ПОСЛЕ фиксации
SELECT 'Salaries AFTER COMMIT' AS marker;
SELECT id, surname, salary FROM employee WHERE post_id = 1 ORDER BY id;

SELECT 'Employee #1 BEFORE ROLLBACK test' AS marker;
SELECT id, surname, salary FROM employee WHERE id = 1;

START TRANSACTION;

UPDATE employee
SET salary = salary - 10000
WHERE id = 1;

-- Проверка ДО отката
SELECT 'Employee #1 INSIDE transaction (before ROLLBACK)' AS marker;
SELECT id, surname, salary FROM employee WHERE id = 1;

ROLLBACK;

-- Проверка ПОСЛЕ отката
SELECT 'Employee #1 AFTER ROLLBACK' AS marker;
SELECT id, surname, salary FROM employee WHERE id = 1;