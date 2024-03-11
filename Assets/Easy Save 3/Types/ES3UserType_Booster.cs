using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("numberOfPositions", "radius", "cardsPrefab", "index", "positions", "cards", "fixedList", "fixedCard", "anim", "isOpen")]
	public class ES3UserType_Booster : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_Booster() : base(typeof(Booster)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (Booster)obj;
			
			writer.WriteProperty("numberOfPositions", instance.numberOfPositions, ES3Type_int.Instance);
			writer.WriteProperty("radius", instance.radius, ES3Type_float.Instance);
			writer.WritePropertyByRef("cardsPrefab", instance.cardsPrefab);
			writer.WritePrivateField("index", instance);
			writer.WritePrivateField("positions", instance);
			writer.WriteProperty("cards", instance.cards, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(RNGNeeds.ProbabilityList<ScriptableCard>)));
			writer.WriteProperty("fixedList", instance.fixedList, ES3Type_bool.Instance);
			writer.WriteProperty("fixedCard", instance.fixedCard, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<ScriptableCard>)));
			writer.WritePropertyByRef("anim", instance.anim);
			writer.WriteProperty("isOpen", instance.isOpen, ES3Type_bool.Instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (Booster)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "numberOfPositions":
						instance.numberOfPositions = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "radius":
						instance.radius = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "cardsPrefab":
						instance.cardsPrefab = reader.Read<UnityEngine.GameObject>(ES3Type_GameObject.Instance);
						break;
					case "index":
					instance = (Booster)reader.SetPrivateField("index", reader.Read<System.Int32>(), instance);
					break;
					case "positions":
					instance = (Booster)reader.SetPrivateField("positions", reader.Read<System.Collections.Generic.List<UnityEngine.Vector3>>(), instance);
					break;
					case "cards":
						instance.cards = reader.Read<RNGNeeds.ProbabilityList<ScriptableCard>>();
						break;
					case "fixedList":
						instance.fixedList = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "fixedCard":
						instance.fixedCard = reader.Read<System.Collections.Generic.List<ScriptableCard>>();
						break;
					case "anim":
						instance.anim = reader.Read<UnityEngine.Animator>();
						break;
					case "isOpen":
						instance.isOpen = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_BoosterArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_BoosterArray() : base(typeof(Booster[]), ES3UserType_Booster.Instance)
		{
			Instance = this;
		}
	}
}