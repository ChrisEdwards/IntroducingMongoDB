using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Linq;


namespace MongoCSharpDriverDemo
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
			Console.WriteLine("MongoDB Demo,");

			// Connect to the mongo instance.
			var mongo = new Mongo();
			mongo.Connect();

			// Use the myorders database.
			Database db = mongo.GetDatabase( "myorders" );

			// Get the orders collection.
			IMongoCollection orders = db.GetCollection( "orders" );


			DeleteAllDocumentsFromCollection( orders );

			CreateAndInsertSingleDocumentIntoCollection( orders );
			CreateAndInsertMultipleDocumentIntoCollection( orders );

			UpdateDocument( orders );
			UpdateUsingAtomicIncrement( orders );

			QueryFromCollection( orders );
			QueryUsingLinq( orders );

			Console.WriteLine( "\nPress Enter to Exit." );
			Console.ReadLine();
		}


		/// <summary>
		/// Demo deleting documents from collection.
		/// </summary>
		/// <param name="orders">The orders.</param>
		private static void DeleteAllDocumentsFromCollection( IMongoCollection orders )
		{
			Console.WriteLine( "\n\n======= Delete Documents =======" );
			Console.WriteLine( string.Format( "Document Count Before Delete: [ {0} ]", orders.Count() ) );

			// Delete documents matching a criteria.
			orders.Delete( new Document {{"CustomerName", "Elmer Fudd"}} );
			Console.WriteLine( string.Format( "Document Count After Deleting Elmer Fudd: [ {0} ]", orders.Count() ) );


			// Delete all docs.
			orders.Delete( new Document() );

			Console.WriteLine( "Deleted all docs" );
			Console.WriteLine( string.Format( "Document Count After Deleting All Docs: [ {0} ]", orders.Count() ) );
		}


		/// <summary>
		/// Inserts a new document.
		/// </summary>
		/// <param name="orders">The orders.</param>
		private static void CreateAndInsertSingleDocumentIntoCollection( IMongoCollection orders )
		{
			Console.WriteLine( "\n\n======= Insert Multiple Documents =======" );
			Console.WriteLine( string.Format( "Document Count Before Insert: [ {0} ]", orders.Count() ) );

			// Create a new order.
			var order = new Document();
			order["OrderAmount"] = 57.22;
			order["CustomerName"] = "Elmer Fudd";

			// Add the new order to the mongo orders colleciton.
			orders.Insert( order );
			Console.WriteLine( string.Format( "Inserted: {0}", order ) );

			Console.WriteLine( string.Format( "Document Count After Insert: [ {0} ]", orders.Count() ) );
		}


		/// <summary>
		/// Demo inserting multiple document into collection.
		/// </summary>
		/// <param name="orders">The orders.</param>
		private static void CreateAndInsertMultipleDocumentIntoCollection( IMongoCollection orders )
		{
			Console.WriteLine( "\n\n======= Insert Multiple Documents =======" );
			Console.WriteLine( string.Format( "Document Count Before Insert: [ {0} ]", orders.Count() ) );
			// Create new orders.
			var order1 = new Document();
			order1["OrderAmount"] = 100.23;
			order1["CustomerName"] = "Bugs Bunny";

			var order2 = new Document();
			order2["OrderAmount"] = 0.01;
			order2["CustomerName"] = "Daffy Duck";

			IEnumerable< Document > orderList = new List< Document > {order1, order2};

			// Insert an IEnumerable.
			orders.Insert( orderList );

			Console.WriteLine( string.Format( "Inserted: {0}", order1 ) );
			Console.WriteLine( string.Format( "Inserted: {0}", order2 ) );

			Console.WriteLine( string.Format( "Document Count After Insert: [ {0} ]", orders.Count() ) );
		}


		/// <summary>
		/// Demo of Updating a document.
		/// </summary>
		/// <param name="orders">The orders.</param>
		private static void UpdateDocument( IMongoCollection orders )
		{
			Console.WriteLine( "\n\n======= Update Documents =======" );
			var selector = new Document {{"CustomerName", "Daffy Duck"}};
			Document docToUpdate = orders.FindOne( selector );

			Console.WriteLine( "Before Update: " + docToUpdate );

			// I'm in the money!
			docToUpdate["OrderAmount"] = 1000000.00;

			// Update Daffy's account before Hasaan finds him.
			orders.Update( docToUpdate );

			Console.WriteLine( "After Update: " + orders.FindOne( selector ) );
		}


		/// <summary>
		/// Demoes Updates a single field using an atomic Increment ($inc) operator.
		/// </summary>
		/// <param name="orders">The orders.</param>
		private static void UpdateUsingAtomicIncrement(IMongoCollection orders)
		{
			Console.WriteLine("\n\n======= Update Document using Increment =======");

			var selector = new Document { { "CustomerName", "Daffy Duck" } };
			Console.WriteLine("Before Update: " + orders.FindOne(selector));

			// Add 2000 to order amount on document matching selector.
			orders.Update( new Document {{"$inc", new Document {{"OrderAmount", 2000}} }}, selector );

			Console.WriteLine("After Update: " + orders.FindOne(selector));
		}


		/// <summary>
		/// Demos querying the collection.
		/// </summary>
		/// <param name="orders">The orders.</param>
		private static void QueryFromCollection( IMongoCollection orders )
		{
			Console.WriteLine( "\n\n======= Query One Document =======" );
			// Create a specification to query the orders collection.
			var spec = new Document();
			spec["CustomerName"] = "Elmer Fudd";

			// Run the query.
			Document result = orders.FindOne( spec );
			Console.WriteLine( string.Format( "Found: {0}", result ) );
		}


		/// <summary>
		/// Demo querying the collection with Linq.
		/// </summary>
		/// <param name="orders">The orders.</param>
		private static void QueryUsingLinq(IMongoCollection orders)
		{
			Console.WriteLine("\n\n======= Query Using Linq =======");
			// Query the orders collection.
			IQueryable<Document> results =
					from doc in orders.AsQueryable()
					where doc.Key("CustomerName") == "Elmer Fudd"
					select doc;
			Document result = results.FirstOrDefault();

			Console.WriteLine(string.Format("Found: {0}", result));
		}
	}
}