using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Linq;


namespace SupportedMongoDbCSharpDriverDemo
{
	/// <summary>
	/// The program.
	/// </summary>
	internal class Program
	{
		/// <summary>
		/// The main.
		/// </summary>
		/// <param name="args">
		/// The args.
		/// </param>
		static void Main( string[] args )
		{
			MongoServer server = MongoServer.Create();
			MongoDatabase db = server.GetDatabase( "test" );

			var collection = db.GetCollection<Order>( "Orders" );

			DeleteAllDocumentsFromCollection(collection);
			CreateAndInsertSingleDocumentIntoCollection(collection);
			CreateAndInsertMultipleDocumentIntoCollection(collection);
			UpdateDocument(collection);
			UpdateUsingAtomicIncrement(collection);
			QueryFromCollection(collection);

			Console.WriteLine("\n Press Enter to Exit.");
			Console.ReadLine();
		}


		/// <summary>
		/// Demo deleting documents from collection.
		/// </summary>
		/// <param name="orders">The orders.</param>
		static void DeleteAllDocumentsFromCollection( MongoCollection< Order > collection )
		{
			Console.WriteLine( "\n\n======= Delete Documents =======" );
			Console.WriteLine( string.Format( "Document Count Before Delete: [ {0} ]", collection.Count() ) );

			// Delete documents matching a criteria.
			collection.Remove( new {CustomerName = "Elmer Fudd"} );
			Console.WriteLine( string.Format( "Document Count After Deleting Elmer Fudd: [ {0} ]", collection.Count() ) );


			// Delete all docs.
			collection.Remove( new {} );

			Console.WriteLine( "Deleted all docs" );
			Console.WriteLine( string.Format( "Document Count After Deleting All Docs: [ {0} ]", collection.Count() ) );
		}


		/// <summary>
		/// Inserts a new document.
		/// </summary>
		/// <param name="collection">The orders.</param>
		private static void CreateAndInsertSingleDocumentIntoCollection(MongoCollection<Order> collection)
		{
			Console.WriteLine("\n\n======= Insert Multiple Documents =======");
			Console.WriteLine(string.Format("Document Count Before Insert: [ {0} ]", collection.Count()));

			// Create a new order.
			var order = new Order()
			{
				OrderAmount = 57.22,
				CustomerName = "Elmer Fudd"
			};

			// Add the new order to the mongo orders colleciton.
			collection.Insert(order);
			Console.WriteLine(string.Format("Inserted: {0}", order));

			Console.WriteLine(string.Format("Document Count After Insert: [ {0} ]", collection.Count()));
		}


		/// <summary>
		/// Demo inserting multiple document into collection.
		/// </summary>
		/// <param name="orders">The orders.</param>
		private static void CreateAndInsertMultipleDocumentIntoCollection(MongoCollection<Order> orders)
		{
			Console.WriteLine("\n\n======= Insert Multiple Documents =======");
			Console.WriteLine(string.Format("Document Count Before Insert: [ {0} ]", orders.Count()));

			// Create new orders.
			var order1 = new Order
			             	{
			             			OrderAmount = 100.23,
			             			CustomerName = "Bugs Bunny"
			             	};
			var order2 = new Order
			             	{
			             			OrderAmount = 0.01,
			             			CustomerName = "Daffy Duck"
			             	};

			IEnumerable<Order> orderList = new List<Order> { order1, order2 };

			// Insert an IEnumerable.
			orders.InsertBatch(orderList);

			Console.WriteLine(string.Format("Inserted: {0}", order1));
			Console.WriteLine(string.Format("Inserted: {0}", order2));

			Console.WriteLine(string.Format("Document Count After Insert: [ {0} ]", orders.Count()));
		}


		/// <summary>
		/// Demo of Updating a document.
		/// </summary>
		/// <param name="orders">The orders.</param>
		private static void UpdateDocument(MongoCollection<Order> orders)
		{
			Console.WriteLine("\n\n======= Update Documents =======");
			var query = Query.EQ( "CustomerName", "Daffy Duck" );
			var orderToUpdate = orders.FindOne( query );

			Console.WriteLine("Before Update: " + orderToUpdate);

			// I'm in the money!
			orderToUpdate.OrderAmount = 1000000.00;

			// Update Daffy's account before Hasaan finds him.
			orders.Save(orderToUpdate);

			Console.WriteLine("After Update: " + orders.FindOne(query));
		}


		/// <summary>
		/// Demoes Updates a single field using an atomic Increment ($inc) operator.
		/// </summary>
		/// <param name="orders">The orders.</param>
		private static void UpdateUsingAtomicIncrement(MongoCollection<Order> orders)
		{
			Console.WriteLine("\n\n======= Update Document using Increment =======");

			var query = Query.EQ("CustomerName", "Daffy Duck");
			Console.WriteLine("Before Update: " + orders.FindOne(query));

			// Add 2000 to order amount on document matching selector.
			orders.Update(query, Update.Inc( "OrderAmount", 2000 ));

			Console.WriteLine("After Update: " + orders.FindOne(query));
		}


		/// <summary>
		/// Demos querying the collection.
		/// </summary>
		/// <param name="orders">The orders.</param>
		private static void QueryFromCollection(MongoCollection<Order> orders)
		{
			Console.WriteLine("\n\n======= Query One Document =======");

			// Create a specification to query the orders collection.
			var query = Query.GT( "OrderAmount", 0 );

			// Run the query.
			IEnumerable<Order> result = orders.Find(query);

			// Print results
			string[] resultsAsString = result.Select( x => x.ToString() ).ToArray();
			Console.WriteLine(string.Format("Found: {0}", String.Join("\n",resultsAsString)));
		}
	}
}