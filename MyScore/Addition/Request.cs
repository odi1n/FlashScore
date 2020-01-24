using Leaf.xNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyScoreMatch.Action
{
    class Request
    {
        /// <summary>
        /// Данные для запроса
        /// </summary>
        /// <returns></returns>
        public HttpRequest httpRequest()
        {
            HttpRequest request = new HttpRequest();
            request.UserAgentRandomize();
            request.Reconnect = true;
            request.ReconnectLimit = 3;
            request.KeepAlive = true;
            return request;
        }

        /// <summary>
        /// Сделать Get запросс
        /// </summary>
        /// <param name="url">ссылка</param>
        /// <param name="param">параметры</param>
        /// <returns></returns>
        public HttpResponse GetResponse(string url, RequestParams param)
        {
            var response = httpRequest().Get(url, param);
            return response;
        }
    }
}
