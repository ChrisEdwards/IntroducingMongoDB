using System;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Linq;


namespace LinqDemo
{
	/// <summary>
	/// The program.
	/// </summary>
	internal class Program
	{
		/// <summary>
		/// The main.
		/// </summary>
		/// <param name="args">The args.</param>
		private static void Main( string[] args )
		{
			// Connect to the mongo instance.
			var mongo = new Mongo();
			mongo.Connect();

			// Use the myorders database.
			Database db = mongo.GetDatabase("myorders");

			// Get the orders collection.
			IMongoCollection orders = db.GetCollection("orders");

			DeleteAllDocumentsFromCollection(orders);
			CreateAndInsertDocumentIntoCollection(orders);

			QueryFromCollection( orders );

			Console.WriteLine("\nPress Enter to Exit.");
			Console.ReadLine();
		}


		/// <summary>
		/// Demo querying the collection with Linq.
		/// </summary>
		/// <param name="orders">The orders.</param>
		private static void QueryFromCollection(IMongoCollection orders)
		{
			Console.WriteLine("\n\n======= Query Using Linq =======");
			// Query the orders collection.
			IQueryable<Document> results =
					from doc in orders.AsQueryable()
					where doc.Key("customerName") == "Elmer Fudd"
					select doc;
			Document result = results.FirstOrDefault();

			Console.WriteLine(string.Format("Found: {0}", result));
		}


		/// <summary>
		/// Demo deleting documents from collection.
		/// </summary>
		/// <param name="orders">The orders.</param>
		private static void DeleteAllDocumentsFromCollection(IMongoCollection orders)
		{
			Console.WriteLine("\n\n======= Delete Test =======");
			Console.WriteLine(string.Format("Document Count Before Delete: [ {0} ]", orders.Count()));

			// Delete documents matching a criteria.
			orders.Delete(new Document { { "customerName", "Elmer Fudd" } });
			Console.WriteLine(string.Format("Document Count After Deleting Elmer Fudd: [ {0} ]", orders.Count()));


			// Delete all docs.
			orders.Delete(new Document());

			Console.WriteLine("Deleted all docs");
			Console.WriteLine(string.Format("Document Count After Deleting All Docs: [ {0} ]\n", orders.Count()));
		}


		/// <summary>
		/// Inserts a new document.
		/// </summary>
		/// <param name="orders">The orders.</param>
		private static void CreateAndInsertDocumentIntoCollection(IMongoCollection orders)
		{
			// Create a new order.
			var order = new Document();
			order["orderAmount"] = 57.22;
			order["customerName"] = "Elmer Fudd";

			// Add the new order to the mongo orders colleciton.
			orders.Insert(order);
			Console.WriteLine(string.Format("Inserted: {0}", order));
		}
	}
}