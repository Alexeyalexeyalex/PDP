INSERT into dbo.po(naim_po,kol_po,vers_po)
Values ('Microsoft office','1','2016');
INSERT into dbo.po(naim_po,kol_po,vers_po)
Values ('Microsoft office','1','2013');
INSERT into dbo.po(naim_po,kol_po,vers_po)
Values ('Photoshop','23','2017');


INSERT into dbo.polz(F_P,I_P,O_P,email,login,password)
Values ('Ланистер','Джейми','Тайвинович','i_a.k.silaenkov@mpt.ru','Jeimi','dolgi123');
INSERT into dbo.polz(F_P,I_P,O_P,email,login,password)
Values ('Ланистер','Серсея','Тайвиновна','silaenckov2014@yandex.ru','Sersea','OgonIb');

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