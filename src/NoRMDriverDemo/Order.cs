using Norm;


namespace NoRMDriverDemo
{
	/// <summary>
	/// The order.
	/// </summary>
	public class Order
	{
		public ObjectId ID { get; set; }
		public double OrderAmount { get; set; }
		public string CustomerName { get; set; }

		public override string ToString()
		{
			return string.Format( "{{ Order: ID={0}; OrderAmount={1}; CustomerName={2} }}", ID, OrderAmount, CustomerName );
		}
	}
}