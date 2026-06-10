using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[JsonConverter(typeof(StringEnumConverter))]
public enum ClassSpecialValue {
	[EnumMember(Value = "teleport")]
	TELEPORT
};

public class ClassSpecials {

	public static Dictionary<ClassSpecialValue, Action<GameObject>> specialLookup = new(){
		[ ClassSpecialValue.TELEPORT ] = null,
	};

}
