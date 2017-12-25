using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using CastleGo.Providers.Models;
using RestSharp;

namespace CastleGo.Providers
{
    /// <summary>Facebook api provider</summary>
    public class FacebookProvider : IFacebookProvider
    {
        private readonly string GraphUrl = "https://graph.facebook.com/v2.8/";
        private readonly IWebApiProvider _webApiHelper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="webApiProvider"></param>
        public FacebookProvider(IWebApiProvider webApiProvider)
        {
            _webApiHelper = webApiProvider;
        }

        private List<Parameter> DefaultParamater(HttpMethod method, string token)
        {
            List<Parameter> parameterList = new List<Parameter>();
            parameterList.Add(new Parameter
            {
                Name = "format",
                Value = (object)"json"
            });
            Parameter parameter1 = new Parameter();
            parameter1.Name = "method";
            string lower = method.Method.ToLower();
            parameter1.Value = (object)lower;
            parameterList.Add(parameter1);
            Parameter parameter2 = new Parameter();
            parameter2.Name = "pretty";

            parameter2.Value = true;
            parameterList.Add(parameter2);
            Parameter parameter3 = new Parameter();
            parameter3.Name = "suppress_http_code";
            parameter3.Value = 1;
            parameterList.Add(parameter3);
            parameterList.Add(new Parameter()
            {
                Name = "access_token",
                Value = (object)token
            });
            return parameterList;
        }

        public async Task<List<FacebookFriend>> Friends(string token, string nextPageToken = "")
        {
            List<FacebookFriend> rs = new List<FacebookFriend>();
            List<Parameter> para = this.DefaultParamater(HttpMethod.Get, token);
            para.Add(new Parameter()
            {
                Name = "fields",
                Value = "name"
            });
            if (!string.IsNullOrEmpty(nextPageToken))
            {
                List<Parameter> parameterList1 = para;
                Parameter parameter1 = new Parameter();
                parameter1.Name = "after";
                string str = nextPageToken;
                parameter1.Value = str;
                parameterList1.Add(parameter1);
                List<Parameter> parameterList2 = para;
                Parameter parameter2 = new Parameter();
                parameter2.Name = "limit";
                parameter2.Value = 20;
                parameterList2.Add(parameter2);
            }
            FacebookFriendData facebookFriendData = await this._webApiHelper.GetAsync<FacebookFriendData>(this.GraphUrl, "me/friends", para, "");
            FacebookFriendData friends = facebookFriendData;
            FacebookFriendData facebookFriendData1 = friends;
            if (facebookFriendData1?.Data == null)
                return rs;
            foreach (FacebookFriend facebookFriend in friends.Data)
            {
                FacebookFriend friend = facebookFriend;
                friend.Avatar = $"{GraphUrl}/{ friend.Id}/picture";
                rs.Add(friend);
            }
            FacebookPagingData paging = friends.Paging;
            string str1;
            if (paging == null)
            {
                str1 = null;
            }
            else
            {
                FacebookPagingCursorData cursors = paging.Cursors;
                str1 = cursors != null ? cursors.After : (string)null;
            }
            if (!string.IsNullOrEmpty(str1))
            {
                List<FacebookFriend> facebookFriendList1 = rs;
                List<FacebookFriend> facebookFriendList2 = await this.Friends(token, friends.Paging.Cursors.After);
                IEnumerable<FacebookFriend> collection = (IEnumerable<FacebookFriend>)facebookFriendList2;
                facebookFriendList1.AddRange(collection);
            }
            return rs;
        }

        private Task<FacebookUserAvatarData> GetUserAvatar(string id, string token)
        {
            Debug.WriteLine("Start get avatar..");
            List<Parameter> parameterList = DefaultParamater(HttpMethod.Get, token);
            Parameter parameter = new Parameter();
            parameter.Name = "redirect";
            parameter.Value = false;
            parameterList.Add(parameter);
            return Task.FromResult(new FacebookUserAvatarData() { Datta = new FacebookUserAvatarDataData() { Url = string.Format("{0}/{1}/picture", (object)this.GraphUrl, (object)id) } });
        }

        public async Task<FacebookUserData> UserInfo(string token)
        {
            FacebookUserData facebookUserData1 = await _webApiHelper.GetAsync<FacebookUserData>(GraphUrl, "me", this.DefaultParamater(HttpMethod.Get, token), "");
            FacebookUserData user = facebookUserData1;
            FacebookUserData facebookUserData = user;
            if (!string.IsNullOrEmpty(facebookUserData?.Id))
            {
                FacebookUserData facebookUserData2 = user;
                FacebookUserAvatarData facebookUserAvatarData = await GetUserAvatar(user.Id, token);
                FacebookUserAvatarData facebookUserAvatarData1 = facebookUserAvatarData;
                string str1;
                if (facebookUserAvatarData1 == null)
                {
                    str1 = null;
                }
                else
                {
                    FacebookUserAvatarDataData datta = facebookUserAvatarData1.Datta;
                    str1 = datta?.Url;
                }
                string str = str1;
                facebookUserData2.Avatar = str;
            }
            return user;
        }
    }
}
