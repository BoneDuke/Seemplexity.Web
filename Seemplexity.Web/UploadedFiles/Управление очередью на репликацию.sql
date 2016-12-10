--Для просмотра текущего состояния очереди необходимо выполнить следующий запрос:

	select mwReplQueue.*, TO_NAME, TO_PriceCount from mwReplQueue left join TP_TOURS on to_key = rq_tokey
	order by rq_id
	
--Для списка выполненных запросов

	select mwReplQueue.*, TO_NAME, TO_PriceCount from mwReplQueue left join TP_TOURS on to_key = rq_tokey
	where rq_startdate is not null
	order by rq_startdate desc
	
-- Для просмотра списка команд на выполнение

	select mwReplQueue.*, TO_NAME, TO_PriceCount from mwReplQueue left join TP_TOURS on to_key = rq_tokey
	where rq_state in (1,3)
	order by rq_priority desc, rq_id

--Описание результатов запроса:
--Rq_id – порядковый номер команды в очереди
--Rq_crdate – дата-время, когда запись попала в очередь (была передана с основного сервера)
--Rq_mode – тип операции (1- выставление тура, 2- выставление тура с дозаписью, 4- удаление прайс-листа )
--Rq_tokey – ключ прайс-листа, над которым будут производиться действия
--Rq_state – состояние записи в очереди (1- ожидает, 3- обрабатывается, 4- выполнение завершено с ошибкой, 5- выполнение успешно завершено )
--Rq_priority – приоритет выполнения команды в очереди ( чем выше, тем раньше команда будет выполнена)
--Rq_remark – не используется
--Rq_startdate – дата-время начала обработки команды
--Rq_enddate – дата-время окончания обработки команды

--Приоритет можно повысить запросом следующего вида:
update mwReplQueue set rq_priority = 600 where rq_tokey in (select TO_Key from tp_tours where to_key = 12345) and rq_state = 1

--Историю выполнения команды можно посмотреть следующей командой:
select * from mwReplQueueHistory where rqh_rqid = 6507
--, где 6507 – порядковый номер записи в очереди

--Также данным запросом можно посмотреть ошибку, с которой было завершено выполнение команды (если ее статус Rq_state = 4).
