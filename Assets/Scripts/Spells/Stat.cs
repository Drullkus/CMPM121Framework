namespace Spells {

	public class Stat {
		
		public string value;

		public Stat(string value) {
			this.value = value;
		}

		void ApplyModifier(Modifier modifier) {
			modifier.Modify(this);
		}

	}

}

