using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("actualPrice", "priceText")]
	public class ES3UserType_BuyZone : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_BuyZone() : base(typeof(BuyZone)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (BuyZone)obj;
			
			writer.WriteProperty("actualPrice", instance.actualPrice, ES3Type_int.Instance);
			writer.WritePropertyByRef("priceText", instance.priceText);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (BuyZone)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "actualPrice":
						instance.actualPrice = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "priceText":
						instance.priceText = reader.Read<TMPro.TextMeshPro>();
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_BuyZoneArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_BuyZoneArray() : base(typeof(BuyZone[]), ES3UserType_BuyZone.Instance)
		{
			Instance = this;
		}
	}
}