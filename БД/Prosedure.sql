CREATE PROCEDURE [DBO].[polz_add]
(
@F_P varchar(max),
@I_P varchar(max),
@O_P varchar(max),
@email varchar(max),
@login varchar(max),
@password varchar(max),
@dostup bit
)
AS
	insert into [dbo].[polz]([F_P],[I_P],[O_P],[email],[login],[password],[dostup]) values((@F_P),(@I_P),(@O_P),(@email),(@login),(@password),(@dostup));
go
CREATE PROCEDURE [DBO].[polz_edit]
@id_polz int,
@F_P varchar(max),
@I_P varchar(max),
@O_P varchar(max),
@email varchar(max),
@login varchar(max)

AS
	update [dbo].polz
	set
	F_P=@F_P,
	I_P=@I_P,
	O_P=@O_P,
	email=@email,
	login=@login

	where id_polz=@id_polz;
go

CREATE PROCEDURE [DBO].[fullpolz_edit]
@id_polz int,
@F_P varchar(max),
@I_P varchar(max),
@O_P varchar(max),
@email varchar(max),
@login varchar(max),
@password varchar(max)

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

CREATE PROCEDURE [DBO].[polzpass_edit]
@id_polz int,
@password varchar(max)
AS
	update [dbo].polz
	set
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
@status varchar(max),
@poz_id int,
@polz_id int
)
AS
	insert into [dbo].[zayavka]([poz_id],[status],[polz_id]) values((@poz_id),(@status),(@polz_id));
go
CREATE PROCEDURE [DBO].[zayavka_edit]
@id_zayavka int,
@naim_zayavka varchar(max),
@vers_zayavka varchar(max),
@status varchar(max),
@polz_id int
AS
	update [dbo].zayavka
	set
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


CREATE PROCEDURE [DBO].[lickluchtd_edit]
(
@id_lickluch int,
@time varchar(5),
@date varchar(10),
@statuskluch bit
)
AS
	update [dbo].lickluch
	set
	time=@time,
	date=@date,
	statuskluch=@statuskluch
	where id_lickluch=@id_lickluch;
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
CREATE PROCEDURE [DBO].[pokol_update]
(
@id_PO int,
@kol_po int
)
AS
	update [dbo].po
	set
	kol_po=@kol_po
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


CREATE PROCEDURE [DBO].[kluch_add]
(
@kod varchar(max),
@statuskluch bit,
@pol_id int
)
AS
	insert into [dbo].[lickluch]([kod],[statuskluch],[pol_id]) values((@kod),(@statuskluch),(@pol_id));
go



CREATE PROCEDURE [DBO].[error_add]
(
@naim_error varchar(max),
@opisanie varchar(max),
@statusError bit,
@sposobYstranenia varchar(max)
)
AS
	insert into [dbo].[error]([naim_error],[opisanie],[statusError],[sposobYstranenia]) values((@naim_error),(@opisanie),(@statusError),(@sposobYstranenia));
go

CREATE PROCEDURE [DBO].[error_update]
(
@id_error int,
@naim_error varchar(max),
@opisanie varchar(max),
@statusError bit,
@sposobYstranenia varchar(max)
)
AS
	update [dbo].error
	set
	naim_error=@naim_error,
	opisanie=@opisanie,
	statusError=@statusError,
	sposobYstranenia=@sposobYstranenia
	where @id_error = id_error
go

CREATE PROCEDURE [DBO].[error_delete]
@id_error int
AS
	DELETE from dbo.error
	where 
	 id_error=@id_error;
go

CREATE PROCEDURE [DBO].[kluch_edit]
@id_lickluch int,
@kod varchar(max)

AS
	update [dbo].lickluch
	set
	kod=@kod

	where id_lickluch=@id_lickluch;
go