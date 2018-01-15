using GZH.CL.Common;
using GZH.CL.Common.Serialize;
using GZH.CL.SnsApi.Entity;

namespace GZH.CL.SnsApi
{
    public class UserInfo
    {
        public string requestResult { get; set; }

        public UserInfoApi Get(string access_token, string openid)
        {
            string requestUri = GZH.CL.Config.ConfigSetting.GetWeixinWeb().UserinfoUrl;
            requestUri += "?access_token=" + access_token + "&openid=" + openid;
            requestUri += "&lang=zh_CN";

            requestResult = HttpService.Get(requestUri);

            return JsonHelper.ScriptDeserialize<UserInfoApi>(requestResult);
        }
    }
}
