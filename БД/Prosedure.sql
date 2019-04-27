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