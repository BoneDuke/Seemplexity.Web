using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Seemplexity.Avalon.BusinesLogic.Model;
using Seemplexity.Avalon.BusinesLogic.Extensions;

namespace Seemplexity.Avalon.BusinesLogic.Utils
{
    public static class Converters
    {
        /// <summary>
        /// Метод получает статус квотирования по числу мест осталось, и числу мест всего
        /// </summary>
        /// <param name="context">Контекст базы данных</param>
        /// <param name="serviceClass">Класс услуги</param>
        /// <param name="quotaExistCount">Осталось мест</param>
        /// <param name="quotaAllCount">Всего мест</param>
        /// <returns></returns>
        public static QuotesStates GetQuotesStateByInt(this Avalon context, int serviceClass, int quotaExistCount, int quotaAllCount)
        {

            QuotesStates result;
            //todo: добавить настрйоку в web.config
            if (quotaExistCount <= 0)
                result = QuotesStates.Request;
            else
                result = context.IsSmallQuotaState(serviceClass, (uint)quotaExistCount, (uint)quotaAllCount) ? QuotesStates.Small : QuotesStates.Availiable;

            return result;
        }

        /// <summary>
        /// Является ли значение типом мало
        /// </summary>
        /// <param name="context">Контекст базы данных</param>
        /// <param name="serviceClass">Класс услуги</param>
        /// <param name="quotaExistCount">Число оставшихся квот</param>
        /// <param name="quotaAllCount">Число квот всего</param>
        /// <returns></returns>
        public static bool IsSmallQuotaState(this Avalon context, int serviceClass, uint quotaExistCount, uint quotaAllCount)
        {
            var smallServiceParams = ServicesExtension.GetQuotaSmallServiceParams(context);
            QuotaSmallServiceParams pars;

            if (smallServiceParams.ContainsKey((uint) serviceClass))
                pars = ServicesExtension.GetQuotaSmallServiceParams(context)[(uint) serviceClass];
            else
            {
                pars = new QuotaSmallServiceParams
                {
                    PlaceParam = 5,
                    AndParam = false,
                    PercentParam = null
                };
            }
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
