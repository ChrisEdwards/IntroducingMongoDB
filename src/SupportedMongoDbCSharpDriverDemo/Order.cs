using MongoDB.Bson;


namespace SupportedMongoDbCSharpDriverDemo
{
	/// <summary>
	/// The order.
	/// </summary>
	public class Order
	{
		public ObjectId Id { get; set; }
		public double OrderAmount { get; set; }
		public string CustomerName { get; set; }

		public override string ToString()
		{
			return string.Format("{{ Order: Id={0}; OrderAmount={1}; CustomerName={2} }}", Id, OrderAmount, CustomerName);
		}
	}
}