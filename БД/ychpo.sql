SET ANSI_PADDING ON
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO 

CREATE DATABASE [ychpo]
GO

USE [ychpo]
GO

CREATE TABLE [DBO].[PO]
(
[id_PO] INT NOT NULL IDENTITY (1,1),
[naim_po] varchar(max) NOT NULL,
[kol_po] int NOT NULL,
[vers_po] varchar(max) NOT NULL,
constraint [PK_id_po] PRIMARY KEY CLUSTERED
	([id_po] ASC) on [PRIMARY],
)

CREATE TABLE [DBO].[role]
(
[id_role] INT NOT NULL IDENTITY (1,1),
[naim_role] varchar(max) NOT NULL,
[polz_role] bit NOT NULL,
[zayavka_role] bit NOT NULL,
[po_role] bit NOT NULL,
[zakaz_role] bit NOT NULL,
constraint [PK_id_role] PRIMARY KEY CLUSTERED
	([id_role] ASC) on [PRIMARY],
)

CREATE TABLE [DBO].[dolj]
(
[id_dolj] INT NOT NULL IDENTITY (1,1),
[naim_dolj] varchar(max) NOT NULL,
[role_id] int null,
constraint [PK_id_dolj] PRIMARY KEY CLUSTERED
	([id_dolj] ASC) on [PRIMARY],
	CONSTRAINT [FK_role_id] FOREIGN KEY ([role_id])
REFERENCES [DBO].[role]([id_role]),
)

CREATE TABLE [DBO].[polz]
(
[id_polz] INT NOT NULL IDENTITY (1,1),
[F_P] varchar(max) NOT NULL,
[I_P] varchar(max) NOT NULL,
[O_P] varchar(max) NOT NULL,
[email] varchar(max) NOT NULL,
[login] varchar(max) NOT NULL,
[password] varchar(16) NOT NULL,
constraint [PK_id_polz] PRIMARY KEY CLUSTERED
	([id_polz] ASC) on [PRIMARY],

)

CREATE TABLE [DBO].[sovm]
(
[id_sovm] INT NOT NULL IDENTITY (1,1),
[polzsovm_id] int Null,
[dolj_id] int null,
constraint [PK_id_sovm] PRIMARY KEY CLUSTERED
	([id_sovm] ASC) on [PRIMARY],
	CONSTRAINT [FK_polzsovm_id] FOREIGN KEY ([polzsovm_id])
REFERENCES [DBO].[polz]([id_polz]),
	CONSTRAINT [FK_dolj_id] FOREIGN KEY ([dolj_id])
REFERENCES [DBO].[dolj]([id_dolj]),
)

CREATE TABLE [DBO].[zayavka]
(
[id_zayavka] INT NOT NULL IDENTITY (1,1),
[kol_zayavka] int NOT NULL,
[naim_zayavka] varchar(max) NOT NULL,
[vers_zayavka] varchar(max) NOT NULL,
[status] varchar(max) NOT NULL,
[polz_id] int NOT NULL,
constraint [PK_id_zayavka] PRIMARY KEY CLUSTERED
	([id_zayavka] ASC) on [PRIMARY],
	CONSTRAINT [FK_polz_id] FOREIGN KEY ([polz_id])
REFERENCES [DBO].[polz]([id_polz]),
)

CREATE TABLE [DBO].[zakaz]
(
[id_zakaz] INT NOT NULL IDENTITY (1,1),
[time] varchar(5) NOT NULL,
[date] varchar(10) NOT NULL,
[Kod] varchar(max) NOT NULL,
[po_id] int NOT NULL,
[zayavka_id] int NOT NULL,
constraint [PK_id_zakaz] PRIMARY KEY CLUSTERED
	([id_zakaz] ASC) on [PRIMARY],
	CONSTRAINT [FK_po_id] FOREIGN KEY ([po_id])
REFERENCES [DBO].[po]([id_po]),
	CONSTRAINT [FK_zayavka_id] FOREIGN KEY ([zayavka_id])
REFERENCES [DBO].[zayavka]([id_zayavka]),
)
go

INSERT into dbo.po(naim_po,kol_po,vers_po)
Values ('Microsoft office','1','2016');
INSERT into dbo.po(naim_po,kol_po,vers_po)
Values ('Microsoft office','1','2013');
INSERT into dbo.po(naim_po,kol_po,vers_po)
Values ('Photoshop','23','2017');


INSERT into dbo.polz(F_P,I_P,O_P,email,login,password)
Values ('Ланистер','Джейми','Тайвинович','dkmfo@klcmk.ru','Jeimi','dolgi123');
INSERT into dbo.polz(F_P,I_P,O_P,email,login,password)
Values ('Ланистер','Серсея','Тайвиновна','kndnnu@mail.com','Sersea','OgonIb');

INSERT into dbo.zayavka(naim_zayavka,kol_zayavka,vers_zayavka,status,polz_id)
Values ('Microsoft office','1','2016','В процессе','2');

INSERT into dbo.role(naim_role,polz_role,zayavka_role,po_role,zakaz_role)
Values ('Admin','1','1','1','1');
INSERT into dbo.role(naim_role,polz_role,zayavka_role,po_role,zakaz_role)
Values ('Сотрудник','0','1','1','1');
INSERT into dbo.role(naim_role,polz_role,zayavka_role,po_role,zakaz_role)
Values ('Пользователь','0','1','1','0');

INSERT into dbo.dolj(naim_dolj,role_id)
Values ('Admin','1');
INSERT into dbo.dolj(naim_dolj,role_id)
Values ('Сотрудник','2');
INSERT into dbo.dolj(naim_dolj,role_id)
Values ('Преподаватель','3');

INSERT into dbo.sovm(polzsovm_id,dolj_id)
Values ('1','1');
INSERT into dbo.sovm(polzsovm_id,dolj_id)
Values ('2','3');

INSERT into dbo.zakaz(time,date,kod,po_id,zayavka_id)
Values ('16:44','26.09.2018','kjwe-23ml-234d-kke1','1','1');
go

CREATE PROCEDURE [DBO].[polz_add]
(
@F_P varchar(max),
@I_P varchar(max),
@O_P varchar(max),
@email varchar(max),
@login varchar(max),
@password varchar(16)
)
AS
	insert into [dbo].[polz]([F_P],[I_P],[O_P],[email],[login],[password]) values((@F_P),(@I_P),(@O_P),(@email),(@login),(@password));
go
CREATE PROCEDURE [DBO].[polz_edit]
@id_polz int,
@F_P varchar(max),
@I_P varchar(max),
@O_P varchar(max),
@email varchar(max),
@login varchar(max),
@password varchar(16)
AS
	update [dbo].polz
	set
	F_P=@F_P,
	I_P=@I_P,
	O_P=@O_P,
	email=@email,
	login=@login,
	password=@password
	where id_polz=@id_polz;
go
CREATE PROCEDURE [DBO].[polz_delete]
@id_polz int
AS
	DELETE from dbo.polz
	where id_polz=@id_polz;
go

CREATE PROCEDURE [DBO].[role_add]
(
@naim_role varchar(max),
@polz_role bit,
@zayavka_role bit,
@po_role bit,
@zakaz_role bit
)
AS
	insert into [dbo].[role]([naim_role],[polz_role],[zayavka_role],[po_role],[zakaz_role]) values((@naim_role),(@polz_role),(@zayavka_role),(@po_role),(@zakaz_role));
go
CREATE PROCEDURE [DBO].[role_edit]
@id_role int,
@naim_role varchar(max),
@polz_role bit,
@zayavka_role bit,
@po_role bit,
@zakaz_role bit
AS
	update [dbo].role
	set
	naim_role=@naim_role,
	polz_role=@polz_role,
	zayavka_role=@zayavka_role,
	po_role=@po_role,
	zakaz_role=@zakaz_role
	where id_role=@id_role;
go
CREATE PROCEDURE [DBO].[role_delete]
@id_role int
AS
	DELETE from dbo.role
	where id_role=@id_role;
go

CREATE PROCEDURE [DBO].[dolj_add]
(
@naim_dolj varchar(max),
@role_id int
)
AS
	insert into [dbo].[dolj]([naim_dolj],[role_id]) values((@naim_dolj),(@role_id));
go
CREATE PROCEDURE [DBO].[dolj_edit]
@id_dolj int,
@naim_dolj varchar(max)
AS
	update [dbo].dolj
	set
	naim_dolj=@naim_dolj
	where id_dolj=@id_dolj;
go
CREATE PROCEDURE [DBO].[dolj_delete]
@id_dolj int
AS
	DELETE from dbo.dolj
	where id_dolj=@id_dolj;
go

CREATE PROCEDURE [DBO].[sovm_add]
(
@polzsovm_id int,
@dolj_id int
)
AS
	insert into [dbo].[sovm]([polzsovm_id],[dolj_id]) values((@polzsovm_id),(@dolj_id));
go

CREATE PROCEDURE [DBO].[zayavka_add]
(
@kol_zayavka int,
@naim_zayavka varchar(max),
@vers_zayavka varchar(max),
@status varchar(max),
@polz_id int
)
AS
	insert into [dbo].[zayavka]([kol_zayavka],[naim_zayavka],[vers_zayavka],[status],[polz_id]) values((@kol_zayavka),(@naim_zayavka),(@vers_zayavka),(@status),(@polz_id));
go
CREATE PROCEDURE [DBO].[zayavka_edit]
@id_zayavka int,
@kol_zayavka int,
@naim_zayavka varchar(max),
@vers_zayavka varchar(max),
@status varchar(max),
@polz_id int
AS
	update [dbo].zayavka
	set
	kol_zayavka=@kol_zayavka,
	naim_zayavka=@naim_zayavka,
	vers_zayavka=@vers_zayavka,
	status=@status,
	polz_id=@polz_id
	where id_zayavka=@id_zayavka;
go
CREATE PROCEDURE [DBO].[zayavkast_edit]
@id_zayavkast int,
@status varchar(max)
AS
	update [dbo].zayavka
	set
	status=@status
	where id_zayavka=@id_zayavkast;
go
CREATE PROCEDURE [DBO].[zayavka_delete]
@id_zayavka int
AS
	DELETE from dbo.zayavka
	where id_zayavka=@id_zayavka;
go


CREATE PROCEDURE [DBO].[zakaz_add]
(
@time varchar(5),
@date varchar(10),
@kod varchar(max),
@po_id int,
@zayavka_id int
)
AS
	insert into [dbo].[zakaz]([time],[date],[kod],[po_id],[zayavka_id]) values((@time),(@date),(@kod),(@po_id),(@zayavka_id));
go

CREATE PROCEDURE [DBO].[po_add]
(
@naim_po varchar(max),
@kol_po int,
@vers_po varchar(max)
)
AS
	insert into [dbo].[po]([naim_po],[kol_po],[vers_po]) values((@naim_po),(@kol_po),(@vers_po));
go
CREATE PROCEDURE [DBO].[po_update]
(
@id_PO int,
@naim_po varchar(max),
@kol_po int,
@vers_po varchar(max)
)
AS
	update [dbo].po
	set
	naim_po=@naim_po,
	kol_po=@kol_po,
	vers_po=@vers_po
	where @id_PO = id_PO
go
CREATE PROCEDURE [DBO].[po_delete]
@id_po int
AS
	DELETE from dbo.po
	where id_po=@id_po;
go

CREATE PROCEDURE [DBO].[sovm_delete]
@id_polzov int
AS
	DELETE from dbo.sovm
	where 
	 polzsovm_id=@id_polzov;
	
go

create view [dbo].[polzv]
as select id_polz as 'Номер пользователя',F_P as 'Фамилия пользователя', I_P as 'Имя пользователя',O_P as 'Очество пользователя',email as 'Email',login as'Логин', password as'Пароль',id_dolj as 'Номер должности',naim_dolj as 'Должность',id_role as 'Номер роли',naim_role as 'роль', polz_role as 'Доступ к пользователям',zayavka_role as 'Доступ к заявкам',po_role as'Доступ к ПО', zakaz_role as 'Доступ к заказам'
from
polz inner join
sovm on sovm.polzsovm_id=polz.id_polz inner join
dolj on dolj.id_dolj=sovm.dolj_id inner join
role on role.id_role=dolj.role_id
go

create view [dbo].[zahazi]
as select id_zayavka as 'Номер заявки',naim_zayavka as 'Название ПО', kol_zayavka as 'Количество копий',vers_zayavka as 'Версия ПО',status as 'Статус',login as 'Логин'
from
zayavka inner join
polz on zayavka.polz_id=polz.id_polz
go
 
 create view [dbo].[rols]
as select id_dolj as 'Номер должности',naim_dolj as 'Должность',id_role as 'Номер роли',naim_role as 'роль', polz_role as 'Доступ к пользователям',zayavka_role as 'Доступ к заявкам',po_role as'Доступ к ПО', zakaz_role as 'Доступ к заказам'
from
dolj  inner join
role on role.id_role=dolj.role_id
go