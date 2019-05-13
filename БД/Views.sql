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
as select id_zakaz as 'Номер заказа',naim_po as 'Название ПО', vers_po as 'Версия ПО',F_P as'Фамилия',I_P as 'Имя',O_P as 'Отчество',time as 'Время заказа',date as 'Дата заказа' 
from
zakaz inner join
lickluch on id_zakaz=zak_id inner join
PO on id_PO=pol_id inner join
zayavka on id_PO=poz_id inner join
polz on id_polz=polz_id
go