using System;


namespace SupportedMongoDbCSharpDriverDemo
{
	public class Post
	{
		public string Title { get; set; }
		public string Author { get; set; }
		public string Body { get; set; }
		public string[] Tags { get; set; }

		public override string ToString()
		{
			return string.Format( "Post: [ Title={0}; Author={1}; Body={2}; Tags={3}", Title, Author, Body, String.Join( ", ", Tags ) );

		}
	}
}