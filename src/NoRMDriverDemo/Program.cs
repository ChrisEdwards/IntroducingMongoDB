using System;
using System.Collections.Generic;
using System.Linq;
using Norm;
using Norm.Collections;
using Norm.Linq;


namespace NoRMDriverDemo
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
		private static void Main( string[] args )
		{
			var mongo = new Mongo( "myorders", "localhost", "27017", string.Empty );
			// For Linq to work, collection name must match Class name.
			MongoCollection< Order > collection = mongo.GetCollection< Order >( "Order" );


			var provider = new MongoQueryProvider(mongo);
			var orderQueryable = new MongoQuery< Order >( provider );


			DeleteAllDocumentsFromCollection( collection );
			CreateAndInsertSingleDocumentIntoCollection( collection );
			CreateAndInsertMultipleDocumentIntoCollection( collection );
			UpdateDocument( collection );
			UpdateUsingAtomicIncrement(collection);
			QueryFromCollection( collection );
			QueryUsingLinq( orderQueryable );
			QueryAllDocuments(orderQueryable);


			Console.WriteLine( "\n Press Enter to Exit." );
			Console.ReadLine();
		}


		/// <summary>
		/// Demo deleting documents from collection.
		/// </summary>
		/// <param name="orders">The orders.</param>
		private static void DeleteAllDocumentsFromCollection(MongoCollection<Order> collection)
		{
			Console.WriteLine("\n\n======= Delete Documents =======");
			Console.WriteLine(string.Format("Document Count Before Delete: [ {0} ]", collection.Count()));

			// Delete documents matching a criteria.
			collection.Delete( new {CustomerName = "Elmer Fudd"} );
			Console.WriteLine(string.Format("Document Count After Deleting Elmer Fudd: [ {0} ]", collection.Count()));


			// Delete all docs.
			collection.Delete( new {} );

			Console.WriteLine("Deleted all docs");
			Console.WriteLine(string.Format("Document Count After Deleting All Docs: [ {0} ]", collection.Count()));
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
			orders.Insert(orderList);

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
			var selector = new {CustomerName = "Daffy Duck"};
			var orderToUpdate = orders.FindOne(selector);

			Console.WriteLine("Before Update: " + orderToUpdate);

			// I'm in the money!
			orderToUpdate.OrderAmount = 1000000.00;

			// Update Daffy's account before Hasaan finds him.
			orders.Save( orderToUpdate );

			Console.WriteLine("After Update: " + orders.FindOne(selector));
		}


		/// <summary>
		/// Demoes Updates a single field using an atomic Increment ($inc) operator.
		/// </summary>
		/// <param name="orders">The orders.</param>
		private static void UpdateUsingAtomicIncrement( MongoCollection< Order > orders )
		{
			Console.WriteLine("\n\n======= Update Document using Increment =======");
			
			var selector = new { CustomerName = "Daffy Duck" };
			Console.WriteLine("Before Update: " + orders.FindOne(selector));

			// Add 2000 to order amount on document matching selector.
			orders.Update( selector, x=>x.Increment( o=>o.OrderAmount, 2000 ) );

			Console.WriteLine("After Update: " + orders.FindOne(selector));
		}


		/// <summary>
		/// Demos querying the collection.
		/// </summary>
		/// <param name="orders">The orders.</param>
		private static void QueryFromCollection(MongoCollection<Order> orders)
		{
			Console.WriteLine("\n\n======= Query One Document =======");

			// Create a specification to query the orders collection.
			var spec = new {CustomerName = "Elmer Fudd"};

			// Run the query.
			Console.WriteLine(string.Format("Found: {0}", orders.FindOne(spec)));
		}


		/// <summary>
		/// Demo querying the collection with Linq.
		/// </summary>
		/// <param name="orders">The orders.</param>
		private static void QueryUsingLinq(MongoQuery<Order> orders)
		{
			Console.WriteLine("\n\n======= Query Using Linq =======");

			// Query the orders collection.
			IQueryable<Order> results =
					from order in orders
					where order.CustomerName == "Elmer Fudd"
					select order;
			var result = results.FirstOrDefault();

			Console.WriteLine(string.Format("Found: {0}", result));
		}


		/// <summary>
		/// Demo of Querying the collection.
		/// </summary>
		/// <param name="collection">The collection.</param>
		private static void QueryAllDocuments(MongoQuery<Order> orders)
		{
			Console.WriteLine( "\n\n======= Query All Documents =======" );
			foreach ( Order order in orders )
				Console.WriteLine( string.Format( "Found: {0}", order ) );
		}
	}
}