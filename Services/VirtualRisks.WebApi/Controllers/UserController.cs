using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using CastleGo.Application.Games;
using CastleGo.Application.Users;
using CastleGo.Providers;
using CastleGo.Providers.Models;
using CastleGo.Shared;
using CastleGo.Shared.Common;
using CastleGo.Shared.Games;
using CastleGo.Shared.Users;
using Hangfire;
using Microsoft.AspNet.Identity;
using Swashbuckle.Swagger.Annotations;

namespace CastleGo.WebApi.Controllers
{
    /// <summary>users</summary>
    [RoutePrefix(Startup.ApiPrefix)]
    public class UserController : ProtectedController
    {
        private readonly IUserService _userService;
        private readonly IFacebookProvider _facebookProvider;
        private readonly INotifySenderProvider _notifySender;
        private readonly IGameService _gameService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="facebookProvider"></param>
        /// <param name="notifySender"></param>
        /// <param name="gameService"></param>
        public UserController(IUserService userService, IFacebookProvider facebookProvider, INotifySenderProvider notifySender, IGameService gameService)
        {
            _userService = userService;
            _facebookProvider = facebookProvider;
            _notifySender = notifySender;
            _gameService = gameService;
        }

        /// <summary>get users</summary>
        /// <param name="page"></param>
        /// <param name="take"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("users")]
        [SwaggerOperation("Paging")]
        public async Task<IHttpActionResult> Users(int page = 0, int take = 10, string query = "")
        {
            PagingResult<UserDto> content = await _userService.PagingAsync(new PagingModel { Page = page, Take = take, Query = query });
            return Ok(content);
        }

        /// <summary>Get user by id</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("user/{id}")]
        [SwaggerOperation("GetById")]
        public async Task<IHttpActionResult> GetById(string id)
        {
            UserDto byIdAsync = await _userService.GetByIdAsync(id);
            return Ok(byIdAsync);
        }

        /// <summary>Edit</summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("user/{id}")]
        [SwaggerOperation("Edit")]
        public async Task<IHttpActionResult> Edit(string id, UserDto model)
        {
            UserDto user = await _userService.GetByIdAsync(id);
            if (user == null)
                return BadRequest("User not found");
            model.Email = user.Email;
            await _userService.UpdateAsync(model);
            return Ok();
        }

        /// <summary>Delete user</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("user/{id}")]
        [SwaggerOperation("Delete")]
        public async Task<IHttpActionResult> Delete(string id)
        {
            UserDto user = await _userService.GetByIdAsync(id);
            if (user == null)
                return BadRequest("User not found");
            await _userService.DeleteAsync(id);
            return Ok();
        }

        /// <summary>Active user</summary>
        /// <param name="id">User id</param>
        /// <returns></returns>
        [HttpPost]
        [Route("user/{id}/active")]
        [SwaggerOperation("Active")]
        public async Task<IHttpActionResult> Active(string id)
        {
            await _userService.ActiveAsync(id);
            return Ok();
        }

        /// <summary>Active user</summary>
        /// <param name="id">User id</param>
        /// <returns></returns>
        [HttpPost]
        [Route("user/{id}/deactive")]
        [SwaggerOperation("Deactive")]
        public async Task<IHttpActionResult> Deactive(string id)
        {
            await _userService.DeactiveAsync(id);
            return Ok();
        }

        /// <summary>Active user</summary>
        /// <param name="id">User id</param>
        /// <param name="provider"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("user/{id}/SyncFriend")]
        [SwaggerOperation("SyncFriend")]
        public async Task<IHttpActionResult> SyncFriend(string id, string provider, string token)
        {
            List<FacebookFriend> facebookFriendList = await _facebookProvider.Friends(token);
            List<FacebookFriend> friends = facebookFriendList;
            if (friends == null)
                return Ok(new List<FriendModel>());
            List<FriendModel> friendsMapped = Mapper.Map<List<FriendModel>>(friends);
            await _userService.UpdateFriendsAsync(id, friendsMapped);
            return Ok(friendsMapped);
        }
        /// <summary>
        /// Friends
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="take"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("user/{id}/friends")]
        [SwaggerOperation("friends")]
        public async Task<IHttpActionResult> Friends(string id, int page, int take, string query = "")
        {
            return Ok(await _userService.PagingFriendsAsync(new PagingByIdModel
            {
                Id = id,
                Take = take,
                Page = page,
                Query = query
            }));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("user/token")]
        public async Task<IHttpActionResult> UpdateToken(UpdateTokenModel model)
        {
            string userId = User.Identity.GetUserId();
            await _userService.UpdateTokenAsync(userId, model);
            return Ok(userId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("user/notify")]
        public async Task<IHttpActionResult> SendTestNotify()
        {
            string userId = User.Identity.GetUserId();
            var user = await _userService.GetByIdAsync(userId);
            var androidToken = user?.Tokens?.FirstOrDefault();
            if (androidToken == null)
                return Ok("User has no any token");
            await _notifySender.SendNotify(MobileDevice.Android.ToString(), androidToken.Token, userId, "Test", "icon", "Test",
                "test");
            return Ok(userId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("user/hero/location")]
        public async Task<IHttpActionResult> UpdateHeroLocation(UpdateHeroLocationModel model)
        {
            // validate
            var userId = User.Identity.GetUserId();
            var user = await _userService.GetByIdAsync(userId);
            if (user == null)
                return Unauthorized();
            if (user.Heroes?.All(e => e.Id != model.HeroId) ?? true)
                return BadRequest("Hero not found");
            // update
            await _userService.UpdateHeroLocationAsync(userId, model);
            BackgroundJob.Enqueue<IGameService>(g => g.SetHeroAroundCastleAsync(new HeroAroundCastleModel()
            {
                UserId = userId,
                HeroId = model.HeroId,
                Position = model.Position
            }));
            return Ok();
        }
    }
}
