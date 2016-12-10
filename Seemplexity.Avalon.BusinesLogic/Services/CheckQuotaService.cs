using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seemplexity.Avalon.BusinesLogic.Model;
using Seemplexity.Avalon.BusinesLogic.Utils;
using Seemplexity.Common.Abstract;
using Seemplexity.Common.Helpers;
using Seemplexity.Common.Impl;

namespace Seemplexity.Avalon.BusinesLogic.Services
{
    public class CheckQuotaService
    {
        public ICache Cache { get; set; }

        public CheckQuotaService()
        {
            Cache = new InMemoryCache();
        }


        /// <summary>
        /// Статусы квот в порядке приоретизации для алгоритма проверки
        /// </summary>
        public static Dictionary<QuotesStates, int> OrderderQuotaStates = new Dictionary<QuotesStates, int>
            {
                {QuotesStates.None, 0},
                {QuotesStates.No, 1},
                {QuotesStates.Request, 2},
                {QuotesStates.Small, 3},
                {QuotesStates.Availiable, 4}
            };

        /// <summary>
        /// Проверяет квоту на всю услугу по параметрам
        /// </summary>
        /// <param name="serviceClass">Класс услуги</param>
        /// <param name="code">Code услуги (для авиаперелетов - Charter, для проживания - ключ отеля)</param>
        /// <param name="subCode1">SubCode1 услуги, для перелета - класс, для проживания - тип номера (Room)</param>
        /// <param name="subCode2">SubCode1 услуги, для перелета - не используется, для проживания - категория номера (RoomCategory)</param>
        /// <param name="partnerKey">Ключ партнера по услуге</param>
        /// <param name="serviceDateFrom">Дата начала услуги</param>
        /// <param name="serviceDateTo">Дата окончания услуги</param>
        /// <param name="agentKey">Ключ агентства</param>
        /// <returns></returns>
        public QuotaStatePlaces CheckServiceQuota(int serviceClass, int code,
            int subCode1, int? subCode2, int partnerKey, int? agentKey, DateTime serviceDateFrom, DateTime serviceDateTo)
        {
            int duration;
            var result = new QuotaStatePlaces() { IsCheckInQuota = false, Places = 0, QuotaState = QuotesStates.Request };
            if (serviceClass == (int)ServiceClass.Flight)
            {
                DateTime linkedDate1, linkedDate2;
                if (serviceDateFrom <= serviceDateTo)
                {
                    linkedDate1 = serviceDateFrom;
                    linkedDate2 = serviceDateTo;
                }
                else
                {
                    linkedDate1 = serviceDateTo;
                    linkedDate2 = serviceDateFrom;
                }
                duration = linkedDate2.Subtract(linkedDate1).Days + 1;
            }
            else
            {
                duration = serviceDateTo.Subtract(serviceDateFrom).Days + 1;
            }

            if (serviceClass == (int)ServiceClass.Flight)
                serviceDateTo = serviceDateFrom;

            var quotasStatusesByDays = new Dictionary<DateTime, QuotaStatePlaces>(duration);
            for (var date = serviceDateFrom; date <= serviceDateTo; date = date.AddDays(1))
            {
                quotasStatusesByDays.Add(date, CheckServiceQuotaByDay(serviceClass, code, subCode1, subCode2, partnerKey, agentKey, date, duration, date == serviceDateFrom));
            }

            if (quotasStatusesByDays.Values.Any(s => s.QuotaState == QuotesStates.No))
                result.QuotaState = QuotesStates.No;
            else if (quotasStatusesByDays.Values.Any(s => s.QuotaState == QuotesStates.Request))
                result.QuotaState = QuotesStates.Request;
            else if (quotasStatusesByDays.Values.Any(s => s.QuotaState == QuotesStates.Small))
                result.QuotaState = QuotesStates.Small;
            else if (quotasStatusesByDays.Values.Any(s => s.QuotaState == QuotesStates.Availiable))
                result.QuotaState = QuotesStates.Availiable;

            if (result.QuotaState == QuotesStates.Availiable || result.QuotaState == QuotesStates.Small)
                result.Places = quotasStatusesByDays.Values.Min(s => s.Places);

            if (quotasStatusesByDays.Values.Any(s => s.IsCheckInQuota))
            {
                var checkInState = quotasStatusesByDays.Values.Where(s => s.IsCheckInQuota).Select(s => s).SingleOrDefault();
                if (checkInState != null && OrderderQuotaStates[checkInState.QuotaState] > OrderderQuotaStates[result.QuotaState])
                    result = checkInState;
            }

            return result;
        }

        /// <summary>
        /// Проверяет квоту на конкретный день по параметрам. Для услуг с продолжительностью этот метод нужно вызвать на каждый день
        /// </summary>
        /// <param name="serviceClass">Класс услуги</param>
        /// <param name="code">Code услуги (для авиаперелетов - Charter, для проживания - ключ отеля)</param>
        /// <param name="subCode1">SubCode1 услуги, для перелета - класс, для проживания - тип номера (Room)</param>
        /// <param name="subCode2">SubCode1 услуги, для перелета - не используется, для проживания - категория номера (RoomCategory)</param>
        /// <param name="partnerKey">Ключ партнера по услуге</param>
        /// <param name="date">Дата проверки квоты</param>
        /// <param name="duration">Продолжительность услуги (нужна для подбора квот на продолжительность)</param>
        /// <param name="isTheFirstDay">Является ли проверяемая дата первым днем или нет (нужно для подбора квот на заезд)</param>
        /// <param name="agentKey">КЛюч агентства</param>
        /// <returns></returns>
        public QuotaStatePlaces CheckServiceQuotaByDay(int serviceClass, int code,
            int subCode1, int? subCode2, int partnerKey, int? agentKey, DateTime date, int duration, bool isTheFirstDay)
        {
            var result = new QuotaStatePlaces();

            List<QuotaPlain> plainQuotasByDate;
            using (var context = new Avalon())
            {
                plainQuotasByDate = GetPlainQuotasObjects(context, serviceClass, code, subCode1, subCode2, partnerKey, agentKey, date);
            }

            plainQuotasByDate =
                plainQuotasByDate.Where(q => q.Duration.Split(',').Contains(duration.ToString()) || q.Duration == String.Empty)
                    .ToList();

            if (!plainQuotasByDate.Any())
                result.QuotaState = QuotesStates.Request;

            var ssQoIds = new List<bool>();
            foreach (var plainQuota in plainQuotasByDate.OrderByDescending(p => p.SsId))
            {
                ////стоит какой-то стоп
                if (plainQuota.SsId != null)
                {
                    //стоит общий стоп
                    if (plainQuota.SsQdId == null)
                    {
                        // стоп только на Allotment, или еще и на Commitment
                        ssQoIds.Add(plainQuota.IsAllotmentAndCommitment);
                    }

                    if (result.QuotaState == QuotesStates.None)
                        result.QuotaState = QuotesStates.No;

                    continue;
                }

                //стопа на текущей строке нет, но есть общий стоп, который подходит под эту строку
                //в переменной result.QuotaState у нас и так уже QuotesStates.No, но проставлю еще раз для наглядности
                if (ssQoIds.Any(s => s || plainQuota.Type == QuotaType.Allotment))
                {
                    result.QuotaState = QuotesStates.No;
                    continue;
                }

                //наступил релиз-период
                //todo: сделать настройку в web.config
                if (plainQuota.Release != null && (plainQuota.Date.Subtract(DateTime.Now.Date).Days < plainQuota.Release))
                {
                    if (result.QuotaState == QuotesStates.None || result.QuotaState == QuotesStates.No)
                    {
                        result.QuotaState = QuotesStates.Request;
                    }
                        

                    continue;
                }

                // проверяем квоту на заезд или на период
                if (serviceClass != (int)ServiceClass.Hotel || 
                    (serviceClass == (int)ServiceClass.Hotel && ((plainQuota.CheckInPlaces ?? 0) <= 0 || plainQuota.CheckInPlacesBusy == null)))
                {
                    QuotesStates state;
                    using (var context = new Avalon())
                    {
                        state = context.GetQuotesStateByInt(serviceClass, plainQuota.Places - plainQuota.Busy, plainQuota.Places);
                    }

                    // или на эту дату не было статуса квотирования, либо мы нашли статус на текущую дату и он лучше
                    if (OrderderQuotaStates[result.QuotaState] < OrderderQuotaStates[state])
                    {
                        result.QuotaState = state;
                        result.Places += (uint)(plainQuota.Places - plainQuota.Busy);
                    }

                }
                else if (isTheFirstDay)
                {
                    // нашли квоту на заезд
                    QuotesStates checkInState;
                    using (var context = new Avalon())
                    {
                        checkInState = context.GetQuotesStateByInt(serviceClass, plainQuota.Places - plainQuota.Busy, plainQuota.Places);
                    }

                    result.Places = (uint)(plainQuota.Places - plainQuota.Busy);
                    result.QuotaState = checkInState;
                    result.IsCheckInQuota = true;
                }
            }

            return result;
        }

        /// <summary>
        /// Возвращает список квот по заданным параметрам
        /// </summary>
        /// <param name="context">Контекст базы данных</param>
        /// <param name="serviceClass">Класс услуги</param>
        /// <param name="code">Code услуги (для авиаперелетов - Charter, для проживания - ключ отеля)</param>
        /// <param name="subCode1">SubCode1 услуги, для перелета - класс, для проживания - тип номера (Room)</param>
        /// <param name="subCode2">SubCode1 услуги, для перелета - не используется, для проживания - категория номера (RoomCategory)</param>
        /// <param name="partnerKey">Ключ партнера по услуге</param>
        /// <param name="date">Дата проверки квоты</param>
        /// <param name="agentKey">Ключ агентства</param>
        /// <returns></returns>
        private List<QuotaPlain> GetPlainQuotasObjects(Avalon context, int serviceClass, int code,
            int subCode1, int? subCode2, int partnerKey, int? agentKey, DateTime date)
        {
            var plainQuotasObjects = Cache.Get(GetPlainQuotasObjects, context, serviceClass, code);

            var result = (from pq in plainQuotasObjects
                          where (pq.PartnerKey <= 0 || pq.PartnerKey == partnerKey)
                      && ((agentKey != null && (pq.AgentKey == agentKey.Value || pq.AgentKey == 0)) || (agentKey == null && pq.AgentKey == 0))
                      && (pq.SubCode1 <= 0 || pq.SubCode1 == subCode1)
                      && (subCode2 == null || pq.SubCode2 <= 0 || pq.SubCode2 == subCode2.Value)
                      && (pq.Date == date)
                select pq)
                .ToList();

            return result;
        }

        /// <summary>
        /// ПОлучения плоского списка квот из БД по параметрам. Загружается сразу на все даты для уменьшения числа обращений к БД
        /// </summary>
        /// <param name="context">Контекст БД</param>
        /// <param name="serviceClass">Класс услуги</param>
        /// <param name="code">Code услуги (для перелета - ключ чартера, для отеля - ключ отеля)</param>
        /// <returns></returns>
        private IEnumerable<QuotaPlain> GetPlainQuotasObjects(DbContext context, int serviceClass, int code)
        {
            var result = new List<QuotaPlain>();

            var commandBuilder = new StringBuilder();
            commandBuilder.AppendLine("select SS_QDID, QD_Type, SS_AllotmentAndCommitment, QP_Busy, QP_CheckInPlacesBusy, QP_Places, QP_CheckInPlaces, QP_Durations, QO_ID, SS_ID, QD_ID, ISNULL(QT_PRKey, 0) as QT_PRKey, ISNULL(QP_AgentKey, 0) as QP_AgentKey, ISNULL(QO_SubCode1, 0) as QO_SubCode1, ISNULL(QO_SubCode2, 0) as QO_SubCode2, QD_Date, QD_Release ");
            commandBuilder.AppendLine("from Quotas ");
            commandBuilder.AppendLine("join QuotaObjects on QO_QTID = QT_ID ");
            commandBuilder.AppendLine("join QuotaDetails on QD_QTID = QT_ID ");
            commandBuilder.AppendLine("join QuotaParts on QP_QDID = QD_ID ");
            commandBuilder.AppendLine("left join StopSales on SS_QDID = QD_ID and (SS_IsDeleted is null or SS_IsDeleted = 0) ");
            commandBuilder.AppendLine($"where QO_SVKey = {serviceClass} ");
            commandBuilder.AppendLine($"and QO_Code = {code} ");
            commandBuilder.AppendLine("and (SS_Date is null or SS_Date = QD_Date) ");
            commandBuilder.AppendLine("and (QP_IsDeleted is null or QP_IsDeleted = 0) ");
            commandBuilder.AppendLine("and (QD_IsDeleted is null or QD_IsDeleted = 0) ");
            commandBuilder.AppendLine("and (QP_IsNotCheckin is null or QP_IsNotCheckin = 0) ");
            commandBuilder.AppendLine($"and QD_Date >= '{DateTime.Now.Date:yyyy-MM-dd}'");
            commandBuilder.AppendLine("union ");
            commandBuilder.AppendLine("select null as SS_QDID, null as QD_Type, SS_AllotmentAndCommitment, 0 as QP_Busy, 0 as QP_CheckInPlacesBusy, 0 as QP_Places, 0 as QP_CheckInPlaces, '' as QP_Durations, QO_ID, SS_ID, 0 as QD_ID, SS_PRKey as QT_PRKey, 0 as QP_AgentKey, ISNULL(QO_SubCode1, 0) as QO_SubCode1, ISNULL(QO_SubCode2, 0) as QO_SubCode2, SS_Date as QD_Date, null as QD_Release ");
            commandBuilder.AppendLine("from QuotaObjects ");
            commandBuilder.AppendLine("join StopSales on SS_QOID = QO_ID ");
            commandBuilder.AppendLine("where QO_QTID is null ");
            commandBuilder.AppendLine("and (SS_IsDeleted is null or SS_IsDeleted = 0) ");
            commandBuilder.AppendLine($"and QO_SVKey = {serviceClass} ");
            commandBuilder.AppendLine($"and QO_Code = {code} ");
            commandBuilder.AppendLine($"and SS_Date >= '{DateTime.Now.Date:yyyy-MM-dd}'");

            using (var command = context.Database.Connection.CreateCommand())
            {
                command.CommandText = commandBuilder.ToString();

                context.Database.Connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new QuotaPlain()
                        {
                            SsQdId = reader.GetInt32OrNull("SS_QDID"),
                            Type = reader.GetInt16OrNull("QD_Type") == null || reader.GetInt16OrNull("QD_Type") == 1 ? QuotaType.Allotment : QuotaType.Commitment,
                            IsAllotmentAndCommitment = reader.GetBooleanOrNull("SS_AllotmentAndCommitment") != null && reader.GetBooleanOrNull("SS_AllotmentAndCommitment") != false,
                            Busy = reader.GetInt32("QP_Busy"),
                            CheckInPlacesBusy = reader.GetInt32OrNull("QP_CheckInPlacesBusy"),
                            Places = reader.GetInt32("QP_Places"),
                            CheckInPlaces = reader.GetInt32OrNull("QP_CheckInPlaces"),
                            Duration = reader.GetString("QP_Durations"),
                            QoId = reader.GetInt32("QO_ID"),
                            SsId = reader.GetInt32OrNull("SS_ID"),
                            QdId = reader.GetInt32("QD_ID"),
                            PartnerKey = reader.GetInt32("QT_PRKey"),
                            AgentKey = reader.GetInt32("QP_AgentKey"),
                            SubCode1 = reader.GetInt32("QO_SubCode1"),
                            SubCode2 = reader.GetInt32("QO_SubCode2"),
                            Date = reader.GetDateTime("QD_Date"),
                            Release = reader.GetInt16OrNull("QD_Release")
                        });
                    }
                }
                context.Database.Connection.Close();
            }

            return result;
        }
    }
}
