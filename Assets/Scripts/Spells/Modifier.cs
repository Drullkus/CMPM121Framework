namespace Spells {
	
	public class Modifier {
		
		public readonly string name;
		public readonly string description;

		private string _format;

		public Modifier(string name, string description, string format) {
			this.name = name;
			this.description = description;

			_format = format;
		}

		// TODO
		void Modify(ref Stat stat) {
			
		}

	}

}

