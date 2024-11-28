using BusinessLayer.Modal.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Net.payOS;
using Net.payOS.Types;


namespace VietNongAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderPayOsController : ControllerBase
    {
        private readonly PayOS _payOS;
        public OrderPayOsController(PayOS payOS)
        {
            _payOS = payOS;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePaymentLink(CreatePaymentLinkRequest body)
        {
            if (body.products == null || !body.products.Any())
            {
                return BadRequest(new Response(-1, "Product list cannot be empty", null));
            }

            try
            {
                int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
                List<ItemData> items = new List<ItemData>();

                // Duyệt qua danh sách sản phẩm và thêm vào danh sách items
                foreach (var product in body.products)
                {
                    ItemData item = new ItemData(product.productName, product.quantity, product.price);
                    items.Add(item);
                }

                // Tính tổng số tiền thanh toán
                int totalAmount = body.products.Sum(p => p.price * p.quantity);

                PaymentData paymentData = new PaymentData(orderCode, totalAmount, body.description, items, body.cancelUrl, body.returnUrl);
                CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);

                return Ok(new Response(0, "success", createPayment));
            }
            catch (System.Exception exception)
            {
                Console.WriteLine(exception);
                return StatusCode(500, new Response(-1, "fail", null));
            }
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrder([FromRoute] int orderId)
        {
            try
            {
                PaymentLinkInformation paymentLinkInformation = await _payOS.getPaymentLinkInformation(orderId);
                return Ok(new Response(0, "Ok", paymentLinkInformation));
            }
            catch (System.Exception exception)
            {

                Console.WriteLine(exception);
                return Ok(new Response(-1, "fail", null));
            }

        }
        [HttpPut("{orderId}")]
        public async Task<IActionResult> CancelOrder([FromRoute] int orderId)
        {
            try
            {
                PaymentLinkInformation paymentLinkInformation = await _payOS.cancelPaymentLink(orderId);
                return Ok(new Response(0, "Ok", paymentLinkInformation));
            }
            catch (System.Exception exception)
            {

                Console.WriteLine(exception);
                return Ok(new Response(-1, "fail", null));
            }

        }
        [HttpPost("confirm-webhook")]
        public async Task<IActionResult> ConfirmWebhook(ConfirmWebhook body)
        {
            try
            {
                await _payOS.confirmWebhook(body.webhook_url);
                return Ok(new Response(0, "Ok", null));
            }
            catch (System.Exception exception)
            {

                Console.WriteLine(exception);
                return Ok(new Response(-1, "fail", null));
            }

        }
    }
}
