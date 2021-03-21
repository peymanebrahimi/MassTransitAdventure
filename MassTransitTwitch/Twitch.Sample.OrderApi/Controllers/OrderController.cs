using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Twitch.Sample.Contracts;

namespace Twitch.Sample.OrderApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IRequestClient<SubmitOrder> _submitOrderRequestClient;
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly IRequestClient<CheckOrder> _checkOrderClient;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrderController(ILogger<OrderController> logger,
            IRequestClient<SubmitOrder> submitOrderRequestClient,
            ISendEndpointProvider sendEndpointProvider,
            IRequestClient<CheckOrder> checkOrderClient,
            IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _submitOrderRequestClient = submitOrderRequestClient;
            _sendEndpointProvider = sendEndpointProvider;
            _checkOrderClient = checkOrderClient;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid id)
        {
            var (status, notFound) = await _checkOrderClient
                .GetResponse<OrderStatus, OrderNotFound>(new
                {
                    OrderId = id
                });

            if (status.IsCompletedSuccessfully)
            {
                var response = await status;
                return Ok(response.Message);
            }
            else
            {
                var response = await notFound;
                return NotFound(response.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(Guid id, string customerNumber)
        {
            var (accepted, rejected) =
                await _submitOrderRequestClient.GetResponse<OrderSubmissionAccepted, OrderSubmissionRejected>(
                    new
                    {

                        OrderId = id,
                        Timestamp = InVar.Timestamp,
                        CustomerNumber = customerNumber,
                        //PaymentCardNumber = default(string)
                    });

            if (accepted.IsCompletedSuccessfully)
            {
                var response = await accepted;

                return Accepted(response);
            }
            else
            {
                var response = await rejected;

                return BadRequest(response.Message);
            }
        }

        [HttpPatch]
        public async Task<IActionResult> Patch(Guid id)
        {
            await _publishEndpoint.Publish<OrderAccepted>(new
            {
                OrderId = id,
                InVar.Timestamp,
            });

            return Accepted();
        }
        
        [HttpPut]
        public async Task<IActionResult> Put(Guid id, string customerNumber)
        {
            var endPoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("exchange:submit-order")); // SubmitOrder

            await endPoint.Send<SubmitOrder>(new
            {
                OrderId = id,
                Timestamp = InVar.Timestamp,
                CustomerNumber = customerNumber,
                //PaymentCardNumber = default(string)
            });

            return Accepted();
        }


    }
}
