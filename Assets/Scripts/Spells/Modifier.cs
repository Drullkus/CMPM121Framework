namespace Spells {
	
	public class Modifier {
		
		public readonly string name;
		public readonly string description;

		private string _target;
		private string _format;

		public Modifier(string name, string description, string target, string format) {
			this.name = name;
			this.description = description;

			_target = target;
			_format = format;
		}

		public void Modify(Stat stat) {
			stat.value = _format.Replace("{value}", stat.value);
		}

	}

}

