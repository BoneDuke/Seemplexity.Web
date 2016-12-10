using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Seemplexity.Avalon.BusinesLogic.Model;

namespace Seemplexity.Avalon.BusinesLogic.Extensions
{
    public static class ServicesExtension
    {
        /// <summary>
        /// Получение списка параметров для квоты "мало" по классам услуг
        /// </summary>
        /// <param name="context">Контекст базы данных</param>
        /// <returns></returns>
        public static Dictionary<uint, QuotaSmallServiceParams> GetQuotaSmallServiceParams(Avalon context)
        {
            var serviceSmallParams = (from s in context.Services
                                  where (s.SV_KEY == 1 || s.SV_KEY == 3)
                                  select s
                )
                .ToDictionary(s => (uint)s.SV_KEY, s => new QuotaSmallServiceParams
                {
                    AndParam = s.SV_LittleAnd.HasValue && s.SV_LittleAnd.Value,
                    PercentParam = (double?)s.SV_LittlePercent,
                    PlaceParam = (uint?)s.SV_LittlePlace
                });

            return serviceSmallParams;
        }

        /// <summary>
        /// Является ли значение типом мало
        /// </summary>
        /// <param name="context">Контекст базы данных</param>
        /// <param name="serviceClass">Класс услуги</param>
        /// <param name="quotaExistCount">Число оставшихся квот</param>
        /// <param name="quotaAllCount">Число квот всего</param>
        /// <returns></returns>
        public static bool IsSmallQuotaState(Avalon context, ServiceClass serviceClass, uint quotaExistCount, uint quotaAllCount)
        {

            var pars = GetQuotaSmallServiceParams(context)[(uint)serviceClass];
            if (pars == null)
                throw new ApplicationException("Ошибка метода: " + MethodBase.GetCurrentMethod().Name + "_" + serviceClass);

            bool res;
            bool? placeCondition = null;
            bool? percentCondition = null;
            if (pars.PlaceParam.HasValue)
                placeCondition = quotaExistCount <= pars.PlaceParam.Value;
            if (pars.PercentParam.HasValue)
                percentCondition = (quotaExistCount / (double)quotaAllCount) * 100 <= pars.PercentParam.Value;

            if (pars.AndParam)
            {
                if (placeCondition.HasValue && placeCondition.Value == false ||
                    percentCondition.HasValue && percentCondition.Value == false)
                    res = false;
                else
                    res = true;
            }
            else
            {
                if (placeCondition.HasValue && placeCondition.Value ||
                    percentCondition.HasValue && percentCondition.Value)
                    res = true;
                else
                    res = false;
            }

            return res;
        }
    }
}
