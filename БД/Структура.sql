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
[O_P] varchar(max) NULL,
[email] varchar(max) NOT NULL,
[login] varchar(max) NOT NULL,
[password] varchar(max) NOT NULL,
[dostup] bit  NULL,
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

CREATE TABLE [DBO].[error]
(
[id_error] INT NOT NULL IDENTITY (1,1),
[naim_error] varchar(max) NOT NULL,
[opisanie] varchar(max) NOT NULL,
[statusError] bit NOT NULL,
[sposobYstranenia] varchar(max) NOT NULL,
constraint [PK_id_error] PRIMARY KEY CLUSTERED
	([id_error] ASC) on [PRIMARY],
)

CREATE TABLE [DBO].[sovmosh]
(
[id_sovmosh] INT NOT NULL IDENTITY (1,1),
[polzsovmosh_id] int Null,
[error_id] int null,
constraint [PK_id_sovmosh] PRIMARY KEY CLUSTERED
	([id_sovmosh] ASC) on [PRIMARY],
	CONSTRAINT [FK_polzsovmosh_id] FOREIGN KEY ([polzsovmosh_id])
REFERENCES [DBO].[polz]([id_polz]),
	CONSTRAINT [FK_error_id] FOREIGN KEY ([error_id])
REFERENCES [DBO].[error]([id_error]),
)
go

