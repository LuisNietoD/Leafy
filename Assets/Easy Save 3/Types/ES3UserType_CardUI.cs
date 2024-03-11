using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("card")]
	public class ES3UserType_CardUI : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_CardUI() : base(typeof(Leafy.Objects.CardUI)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (Leafy.Objects.CardUI)obj;
			
			writer.WriteProperty("card", instance.card, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(Leafy.Data.Card)));
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (Leafy.Objects.CardUI)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "card":
						instance.card = reader.Read<Leafy.Data.Card>();
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_CardUIArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_CardUIArray() : base(typeof(Leafy.Objects.CardUI[]), ES3UserType_CardUI.Instance)
		{
			Instance = this;
		}
	}
}