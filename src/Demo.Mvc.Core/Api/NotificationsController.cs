﻿using System.Threading.Tasks;
using Demo.Business;
using Demo.Business.Command;
using Demo.Business.Command.News.Models;
using Demo.Business.Command.Notifications;
using Demo.Common.Command;
using Demo.Mvc.Core.Api.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Mvc.Core.Api
{
    public class NotificationsController : ApiControllerBase
    {

        public NotificationsController(BusinessFactory business)
            : base(business)
        {
        }

        [HttpGet]
        [Route("api/notifications/item/get/{siteId}/{moduleId}")]
        public async Task<CommandResult> GetItem([FromServices]GetNotificationsItemCommand _getNotificationsItemCommand, string siteId, string moduleId)
        {
            var userInput = new UserInput<GetModuleInput>
            {
                UserId = User.Identity == null ? string.Empty : User.GetUserId(),
                Data = new GetModuleInput {ModuleId = moduleId, SiteId = siteId}
            };

            var result =
                await Business
                    .InvokeAsync<GetNotificationsItemCommand, UserInput<GetModuleInput>,
                        CommandResult<GetNewsItemResult>>(_getNotificationsItemCommand, userInput);
            return result;
        }


        [HttpGet]
        [Route("api/notifications/{siteId}")]
        public async Task<CommandResult<GetNewsResult>> GetNotifications([FromServices]GetNotificationsCommand getNotificationsCommand, string siteId)
        {
            return await GetNotifications(getNotificationsCommand, siteId, null);
        }


        [HttpGet]
        [Route("api/notifications/{siteId}/{index}")]
        public async Task<CommandResult<GetNewsResult>> GetNotifications([FromServices]GetNotificationsCommand _getNotificationsCommand, string siteId, int? index)
        {
            var result =
                await Business.InvokeAsync<GetNotificationsCommand, GetNewsInput, CommandResult<GetNewsResult>>(
                    _getNotificationsCommand, new GetNewsInput
                    {
                        ModuleId = null,
                        SiteId = siteId,
                        FilterIndex = index
                    });
            return result;
        }

        [Authorize]
        [HttpPost]
        [Route("api/notifications/item/send")]
        public async Task<CommandResult> Send([FromServices]SendNotificationCommand sendNotificationCommand, [FromBody] SendNotificationInput saveUserInput)
        {
            var userInput = new UserInput<SendNotificationInput>
            {
                UserId = User.GetUserId(),
                Data = saveUserInput
            };

            var result = await
                Business.InvokeAsync<SendNotificationCommand, UserInput<SendNotificationInput>, CommandResult>(
                    sendNotificationCommand, userInput);

            return result;
        }

        [Authorize]
        [HttpPost]
        [Route("api/notifications/item/save")]
        public async Task<CommandResult> Save([FromServices]SaveNotificationItemCommand _saveNotificationItemCommand, [FromBody] SaveNewsItemInput saveUserInput)
        {
            var userInput = new UserInput<SaveNewsItemInput>
            {
                UserId = User.GetUserId(),
                Data = saveUserInput
            };

            var result = await
                Business.InvokeAsync<SaveNotificationItemCommand, UserInput<SaveNewsItemInput>, CommandResult<dynamic>>(
                    _saveNotificationItemCommand, userInput);

            return result;
        }
    }
}