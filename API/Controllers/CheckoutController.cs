//using API.Models;
//using Microsoft.AspNetCore.Mvc;
//using PaypalServerSdk.Standard;
//using PaypalServerSdk.Standard.Authentication;
//using PaypalServerSdk.Standard.Controllers;
//using PaypalServerSdk.Standard.Http.Response;
//using PaypalServerSdk.Standard.Models;
//using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

//namespace API.Controllers
//{
//    [ApiController]
//    public class CheckoutController : Controller
//    {
//        private readonly OrdersController _ordersController;
//        private readonly PaypalServerSdk.Standard.Controllers.PaymentsController _paymentsController;
//        private readonly PromoCodeController _promoCodeController;
//        private readonly Dictionary<string, CheckoutPaymentIntent> _paymentIntentMap;
//        private readonly Dictionary<string, CardVerificationMethod> _cardVerificationMethodMap;
//        private IConfiguration _configuration { get; }
//        private readonly string _paypalClientId;
//        private readonly string _paypalClientSecret;

//        private readonly ILogger<CheckoutController> _logger;

//        public CheckoutController(IConfiguration configuration, ILogger<CheckoutController> logger, PromoCodeController promoCodeController)
//        {
//            _configuration = configuration;
//            _promoCodeController = promoCodeController;
//            _logger = logger;
//            _paymentIntentMap = new Dictionary<string, CheckoutPaymentIntent>
//        {
//            { "CAPTURE", CheckoutPaymentIntent.Capture },
//            { "AUTHORIZE", CheckoutPaymentIntent.Authorize }
//        };
//            _cardVerificationMethodMap = new Dictionary<string, CardVerificationMethod>
//        {
//            { "SCA_ALWAYS", CardVerificationMethod.ScaAlways },
//            { "SCA_WHEN_REQUIRED", CardVerificationMethod.ScaWhenRequired }
//        };
//            _paypalClientId = configuration["PAYPAL_CLIENT_ID"];
//            _paypalClientSecret = configuration["PAYPAL_CLIENT_SECRET"];

//            // Initialize the PayPal SDK client
//            PaypalServerSdkClient client = new PaypalServerSdkClient.Builder()
//              .Environment(PaypalServerSdk.Standard.Environment.Sandbox)
//              .ClientCredentialsAuth(
//                new ClientCredentialsAuthModel.Builder(_paypalClientId, _paypalClientSecret).Build()
//              )
//              .LoggingConfig(config =>
//                config
//                .LogLevel(LogLevel.Information)
//                .RequestConfig(reqConfig => reqConfig.Body(true))
//                .ResponseConfig(respConfig => respConfig.Headers(true))
//              )
//              .Build();

//            _ordersController = client.OrdersController;
//            _paymentsController = client.PaymentsController;
//        }


//        [HttpPost("api/orders")]
//        public async Task<IActionResult> CreateOrder([FromBody] OrderRequestCart cart)
//        {
//            try
//            {
//                // Validation du code promo s'il est fourni
//                if (!string.IsNullOrEmpty(cart.PromoCode))
//                {
//                    var promoValidationResult = _promoCodeController.ValidatePromoCode(cart.PromoCode);
//                    if (promoValidationResult == null)
//                    {
//                        return BadRequest(new { error = "Invalid promo code." });
//                    }
//                    // Si le code promo est valide, tu peux éventuellement ajuster le prix ici.
//                    // Par exemple, appliquer une réduction à `cart.Price`
//                    // cart.Price = cart.Price * promoValidationResult.DiscountPercentage;
//                }
//                var result = await _CreateOrder(cart);
//                return StatusCode((int)result.StatusCode, result.Data);
//            }
//            catch (Exception ex)
//            {
//                Console.Error.WriteLine("Failed to create order:", ex);
//                return StatusCode(500, new
//                {
//                    error = "Failed to create order."
//                });
//            }
//        }

//        private async Task<dynamic> _CreateOrder(OrderRequestCart cart)
//        {
//            //var plan = cart as SubscriptionPlan;
//            CreateOrderInput createOrderInput = new CreateOrderInput
//            {
//                Body = new OrderRequest
//                {
//                    Intent = _paymentIntentMap["CAPTURE"],
//                    PurchaseUnits = new List<PurchaseUnitRequest> {
//            new PurchaseUnitRequest {
//                Amount = new AmountWithBreakdown {
//                    CurrencyCode = cart.CurrencyCode,
//                    MValue = "100",
//                },
//            }
//        },

//                },
//            };

//            ApiResponse<Order> result = await _ordersController.CreateOrderAsync(createOrderInput);
//            return result;
//        }


//        [HttpPost("api/orders/{orderID}/capture")]
//        public async Task<IActionResult> CaptureOrder(string orderID)
//        {
//            try
//            {
//                var result = await _CaptureOrder(orderID);
//                return StatusCode((int)result.StatusCode, result.Data);
//            }
//            catch (Exception ex)
//            {
//                Console.Error.WriteLine("Failed to capture order:", ex);
//                return StatusCode(500, new
//                {
//                    error = "Failed to capture order."
//                });
//            }
//        }

//        private async Task<dynamic> _CaptureOrder(string orderID)
//        {
//            CaptureOrderInput captureOrderInput = new CaptureOrderInput
//            {
//                Id = orderID,
//            };

//            ApiResponse<Order> result = await _ordersController.CaptureOrderAsync(captureOrderInput);

//            return result;
//        }

//        [HttpPost("api/orders/{orderID}/authorize")]
//        public async Task<IActionResult> AuthorizeOrder(string orderID)
//        {
//            try
//            {
//                var result = await _AuthorizeOrder(orderID);
//                return StatusCode((int)result.StatusCode, result.Data);
//            }
//            catch (Exception ex)
//            {
//                Console.Error.WriteLine("Failed to authorize order:", ex);
//                return StatusCode(500, new
//                {
//                    error = "Failed to authorize order."
//                });
//            }
//        }

//        [HttpPost("api/orders/{authorizationID}/captureAuthorize")]
//        public async Task<IActionResult> CaptureAuthorizeOrder(string authorizationID)
//        {
//            try
//            {
//                var result = await _CaptureAuthorizeOrder(authorizationID);
//                return StatusCode((int)result.StatusCode, result.Data);
//            }
//            catch (Exception ex)
//            {
//                Console.Error.WriteLine("Failed to authorize order:", ex);
//                return StatusCode(500, new
//                {
//                    error = "Failed to authorize order."
//                });
//            }
//        }

//        private async Task<dynamic> _AuthorizeOrder(string orderID)
//        {
//            AuthorizeOrderInput authorizeOrderInput = new AuthorizeOrderInput
//            {
//                Id = orderID,
//            };

//            ApiResponse<OrderAuthorizeResponse> result = await _ordersController.AuthorizeOrderAsync(
//              authorizeOrderInput
//            );

//            return result;
//        }

//        private async Task<dynamic> _CaptureAuthorizeOrder(string authorizationID)
//        {
//            CaptureAuthorizedPaymentInput captureAuthorizedPaymentInput = new CaptureAuthorizedPaymentInput
//            {
//                AuthorizationId = authorizationID,
//            };

//            ApiResponse<CapturedPayment> result = await _paymentsController.CaptureAuthorizedPaymentAsync(
//              captureAuthorizedPaymentInput
//            );

//            return result;
//        }
//    }

//    internal class CardVerificationMethod
//    {
//        internal static readonly CardVerificationMethod ScaAlways;
//        internal static readonly CardVerificationMethod ScaWhenRequired;
//    }
//}