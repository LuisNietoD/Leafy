using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("QuestCategories", "questTitle", "questDesc", "questPanel", "actualIndex", "actualCategories")]
	public class ES3UserType_QuestManager : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_QuestManager() : base(typeof(QuestManager)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (QuestManager)obj;
			
			writer.WriteProperty("QuestCategories", instance.QuestCategories, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<QuestCategory>)));
			writer.WritePropertyByRef("questTitle", instance.questTitle);
			writer.WritePropertyByRef("questDesc", instance.questDesc);
			writer.WritePropertyByRef("questPanel", instance.questPanel);
			writer.WriteProperty("actualIndex", instance.actualIndex, ES3Type_int.Instance);
			writer.WriteProperty("actualCategories", instance.actualCategories, ES3Type_int.Instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (QuestManager)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "QuestCategories":
						instance.QuestCategories = reader.Read<System.Collections.Generic.List<QuestCategory>>();
						break;
					case "questTitle":
						instance.questTitle = reader.Read<TMPro.TextMeshProUGUI>();
						break;
					case "questDesc":
						instance.questDesc = reader.Read<TMPro.TextMeshProUGUI>();
						break;
					case "questPanel":
						instance.questPanel = reader.Read<UnityEngine.Animator>();
						break;
					case "actualIndex":
						instance.actualIndex = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "actualCategories":
						instance.actualCategories = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_QuestManagerArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_QuestManagerArray() : base(typeof(QuestManager[]), ES3UserType_QuestManager.Instance)
		{
			Instance = this;
		}
	}
}