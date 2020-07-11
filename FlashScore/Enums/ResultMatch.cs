using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashScore.Enums
{
    /// <summary>
    /// Результат матча
    /// </summary>
    public enum ResultMatch
    {
        /// <summary>
        /// Завершен
        /// </summary>
        Completed,
        /// <summary>
        /// Перенесен
        /// </summary>
        Moved,
        /// <summary>
        /// Нет информации
        /// </summary>
        None,
        /// <summary>
        /// Отменен
        /// </summary>
        Calceled,
        /// <summary>
        /// После серии пинальте
        /// </summary>
        SeriesOfPinal,
        /// <summary>
        /// Неявка
        /// </summary>
        Absence,
        /// <summary>
        /// Тех. поражение
        /// </summary>
        TechDefeat,
        /// <summary>
        /// После дополнительного времени
        /// </summary>
        AfterExtraTime,
    }
}
