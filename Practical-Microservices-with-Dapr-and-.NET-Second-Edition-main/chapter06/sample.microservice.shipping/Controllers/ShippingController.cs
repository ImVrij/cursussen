namespace sample.microservice.shipping.Controllers;

[ApiController]
public class ShippingController : ControllerBase
{
    public const string StoreName = "shippingstore";
    public const string PubSub = "commonpubsub";

    /// <summary>
    /// Method for shipping order.
    /// </summary>
    /// <param name="orderShipment">Shipment info.</param>
    /// <param name="daprClient">State client to interact with Dapr runtime.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    [Topic(PubSub, Topics.OrderPreparedTopicName)]
    [HttpPost(Topics.OrderPreparedTopicName)]
    public async Task<ActionResult<Guid>> ship(Shipment orderShipment, [FromServices] DaprClient daprClient)
    {            
        var state = await daprClient.GetStateEntryAsync<ShippingState>(StoreName, orderShipment.OrderId.ToString());
        state.Value ??= new ShippingState() {OrderId = orderShipment.OrderId, ShipmentId = Guid.NewGuid() };

        await state.SaveAsync();

        // return shipment Id
        var result = state.Value.ShipmentId;

        Console.WriteLine($"Shipment of orderId {orderShipment.OrderId} completed with id {result}");

        return this.Ok();
    }
}
