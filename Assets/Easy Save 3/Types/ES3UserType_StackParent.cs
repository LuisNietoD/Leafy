using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("firstSpawn")]
	public class ES3UserType_StackParent : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_StackParent() : base(typeof(StackParent)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (StackParent)obj;
			
			writer.WriteProperty("firstSpawn", instance.firstSpawn, ES3Type_bool.Instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (StackParent)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "firstSpawn":
						instance.firstSpawn = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_StackParentArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_StackParentArray() : base(typeof(StackParent[]), ES3UserType_StackParent.Instance)
		{
			Instance = this;
		}
	}
}