--��� ��������� �������� ��������� ������� ���������� ��������� ��������� ������:

	select mwReplQueue.*, TO_NAME, TO_PriceCount from mwReplQueue left join TP_TOURS on to_key = rq_tokey
	order by rq_id
	
--��� ������ ����������� ��������

	select mwReplQueue.*, TO_NAME, TO_PriceCount from mwReplQueue left join TP_TOURS on to_key = rq_tokey
	where rq_startdate is not null
	order by rq_startdate desc
	
-- ��� ��������� ������ ������ �� ����������

	select mwReplQueue.*, TO_NAME, TO_PriceCount from mwReplQueue left join TP_TOURS on to_key = rq_tokey
	where rq_state in (1,3)
	order by rq_priority desc, rq_id

--�������� ����������� �������:
--Rq_id � ���������� ����� ������� � �������
--Rq_crdate � ����-�����, ����� ������ ������ � ������� (���� �������� � ��������� �������)
--Rq_mode � ��� �������� (1- ����������� ����, 2- ����������� ���� � ���������, 4- �������� �����-����� )
--Rq_tokey � ���� �����-�����, ��� ������� ����� ������������� ��������
--Rq_state � ��������� ������ � ������� (1- �������, 3- ��������������, 4- ���������� ��������� � �������, 5- ���������� ������� ��������� )
--Rq_priority � ��������� ���������� ������� � ������� ( ��� ����, ��� ������ ������� ����� ���������)
--Rq_remark � �� ������������
--Rq_startdate � ����-����� ������ ��������� �������
--Rq_enddate � ����-����� ��������� ��������� �������

--��������� ����� �������� �������� ���������� ����:
update mwReplQueue set rq_priority = 600 where rq_tokey in (select TO_Key from tp_tours where to_key = 12345) and rq_state = 1

--������� ���������� ������� ����� ���������� ��������� ��������:
select * from mwReplQueueHistory where rqh_rqid = 6507
--, ��� 6507 � ���������� ����� ������ � �������

--����� ������ �������� ����� ���������� ������, � ������� ���� ��������� ���������� ������� (���� �� ������ Rq_state = 4).
