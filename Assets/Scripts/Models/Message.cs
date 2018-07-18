namespace Serverfull.Models {
	public class Message {
		public string Title   { get; }
		public string Content { get; }

		public Message(string title, string content) {
			Title   = title;
			Content = content;
		}
	}
}
