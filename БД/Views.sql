create view [dbo].[polzv]
as select id_polz as 'Номер пользователя',F_P as 'Фамилия пользователя', I_P as 'Имя пользователя',O_P as 'Очество пользователя',email as 'Email',login as'Логин', password as'Пароль',id_dolj as 'Номер должности',naim_dolj as 'Должность',id_role as 'Номер роли',naim_role as 'роль', polz_role as 'Доступ к пользователям',zayavka_role as 'Доступ к заявкам',po_role as'Доступ к ПО', zakaz_role as 'Доступ к заказам'
from
polz inner join
sovm on sovm.polzsovm_id=polz.id_polz inner join
dolj on dolj.id_dolj=sovm.dolj_id inner join
role on role.id_role=dolj.role_id
go

create view [dbo].[zahazi]
as select id_zayavka as 'Номер заявки',naim_po as 'Название ПО', vers_po as 'Версия ПО',status as 'Статус',login as 'Логин'
from
zayavka inner join
polz on zayavka.polz_id=polz.id_polz inner join
PO on id_PO=poz_id 
go
 
 create view [dbo].[rols]
as select id_dolj as 'Номер должности',naim_dolj as 'Должность',id_role as 'Номер роли',naim_role as 'роль', polz_role as 'Доступ к пользователям',zayavka_role as 'Доступ к заявкам',po_role as'Доступ к ПО', zakaz_role as 'Доступ к заказам'
from
dolj  inner join
role on role.id_role=dolj.role_id 
go

create view [dbo].[statistika]
as select id_lickluch as 'Номер',naim_po as 'Название ПО', vers_po as 'Версия ПО',F_P as'Фамилия',I_P as 'Имя',O_P as 'Отчество',time as 'Время заказа',date as 'Дата заказа'  from zayavka
join polz on polz.id_polz = zayavka.polz_id
join PO on Po.id_PO = zayavka.poz_id
join lickluch on lickluch.pol_id = PO.id_PO
where lickluch.statuskluch = 1 and zayavka.status = 'Готово'
go

create view [dbo].[izmlickluch]
as select id_lickluch as 'Номер',naim_po as 'Название ПО', kod as 'Код',statuskluch as 'Выдан' from lickluch
join PO on PO.id_PO = lickluch.pol_id
go