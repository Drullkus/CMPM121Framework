using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[JsonConverter(typeof(StringEnumConverter))]
public enum ClassSpecialValue {
	[EnumMember(Value = "none")]
	NONE,
	[EnumMember(Value = "teleport")]
	TELEPORT
};

public class ClassSpecials {

	public static Dictionary<ClassSpecialValue, Action<GameObject>> specialLookup = new(){
		[ ClassSpecialValue.NONE] = Nop,
		[ ClassSpecialValue.TELEPORT ] = Teleport,
	};

	public static void Nop(GameObject _) { }

	public static void Teleport(GameObject target) {
		int startingChunk = UnityEngine.Random.Range(0, 99);
		
		for(int i = 0; i < 100; i++) {
			float angle = (float)((startingChunk + i) % 99) * 2.0f * Mathf.PI / 99.0f;
			Vector2 direction = new(Mathf.Cos(angle), Mathf.Sin(angle));

			List<RaycastHit2D> hits = new List<RaycastHit2D>();
			ContactFilter2D filter = new();
			filter.useTriggers = false;

			int collisionCount = target.GetComponent<Rigidbody2D>().Cast(direction, filter, hits, 3.0f);

			if(collisionCount < 1) {
				_Teleport(direction * 3.0f); 
				break;
			}
		}

		return;

		void _Teleport(Vector2 displacement) {
			target.transform.Translate(displacement);
		}
	}

}
