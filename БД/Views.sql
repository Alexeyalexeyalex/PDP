create view [dbo].[polzv]
as select id_polz as '����� ������������',F_P as '������� ������������', I_P as '��� ������������',O_P as '������� ������������',email as 'Email',login as'�����', password as'������',id_dolj as '����� ���������',naim_dolj as '���������',id_role as '����� ����',naim_role as '����', polz_role as '������ � �������������',zayavka_role as '������ � �������',po_role as'������ � ��', zakaz_role as '������ � �������'
from
polz inner join
sovm on sovm.polzsovm_id=polz.id_polz inner join
dolj on dolj.id_dolj=sovm.dolj_id inner join
role on role.id_role=dolj.role_id
go

create view [dbo].[zahazi]
as select id_zayavka as '����� ������',naim_po as '�������� ��', vers_po as '������ ��',status as '������',login as '�����'
from
zayavka inner join
polz on zayavka.polz_id=polz.id_polz inner join
PO on id_PO=poz_id 
go
 
 create view [dbo].[rols]
as select id_dolj as '����� ���������',naim_dolj as '���������',id_role as '����� ����',naim_role as '����', polz_role as '������ � �������������',zayavka_role as '������ � �������',po_role as'������ � ��', zakaz_role as '������ � �������'
from
dolj  inner join
role on role.id_role=dolj.role_id 
go

create view [dbo].[statistika]
as select id_lickluch as '�����',naim_po as '�������� ��', vers_po as '������ ��',F_P as'�������',I_P as '���',O_P as '��������',time as '����� ������',date as '���� ������'  from zayavka
join polz on polz.id_polz = zayavka.polz_id
join PO on Po.id_PO = zayavka.poz_id
join lickluch on lickluch.pol_id = PO.id_PO
where lickluch.statuskluch = 1 and zayavka.status = '������'
go

create view [dbo].[izmlickluch]
as select id_lickluch as '�����',naim_po as '�������� ��', kod as '���',statuskluch as '�����' from lickluch
join PO on PO.id_PO = lickluch.pol_id
go