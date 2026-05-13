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

		public void Modify(ref string stat) {
			stat.value = format.Replace("{value}", stat.value);
		}

	}

}

