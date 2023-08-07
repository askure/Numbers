using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(CsvImporter))]
public class CsvImpoterEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		var csvImpoter = target as CsvImporter;

		if (GUILayout.Button("敵データの作成"))
		{
			
			SetCsvDataToScriptableObject(csvImpoter);
			Debug.Log("敵データ作成完了");
		}
        if (GUILayout.Button("味方データの作成"))
        {
			
			SetCsvCardDataToScriptableObject(csvImpoter);
			Debug.Log("味方データ作成完了"); 

		}
		if (GUILayout.Button("スキルデータの作成"))
		{
			
			SetCsvSkillDataToScriptableObject(csvImpoter);
			Debug.Log("スキルデータ作成完了"); 

		}
	}

	void SetCsvDataToScriptableObject(CsvImporter csvImporter)
	{
		if (csvImporter.csvFile == null)
		{
			Debug.LogWarning(csvImporter.name + " : 読み込むCSVファイルがセットされていません。");
			return;
		}
		if (csvImporter.csvFile.name != "EnemyData")
		{
			Debug.LogWarning(csvImporter.name + " : 読み込むファイルが違います");
			return;
		}

		string csvText = csvImporter.csvFile.text;

			// 改行ごとにパース
			string[] afterParse = csvText.Split('\n');
		for (int i = 1; i < afterParse.Length; i++)
		{
			string[] parseByComma = afterParse[i].Split(',');

			int column = 0;

			// 先頭の列が空であればその行は読み込まない
			if (parseByComma[column] == "")
			{
				continue;
			}

			// 行数をIDとしてファイルを作成
			string fileName = "Enemy" + (i-1).ToString() + ".asset";
			string path = "Assets/Resources/EnemyEntityList/" + fileName;

			// EnemyDataのインスタンスをメモリ上に作成
			var enemyData = CreateInstance<EnemyEntity>();

			//ID
			enemyData.EnemyId = int.Parse(parseByComma[column]);
			// 名前
			column++;
			enemyData.name = parseByComma[column];

			// 最大HP
			column += 1;
			enemyData.Hp = int.Parse(parseByComma[column]);

			//攻撃力
			column++;
			enemyData.at = int.Parse(parseByComma[column]);

			// 防御力
			column += 1;
			enemyData.df = int.Parse(parseByComma[column]);

			// 数値
			column += 1;
			enemyData.numba = int.Parse(parseByComma[column]);

			// 経験値
			column += 1;
			enemyData._exp = int.Parse(parseByComma[column]);

			//icon
			column += 1;
			enemyData.icon = Resources.Load<Sprite>("character/" + parseByComma[column]);
			
			//スキル
			column += 1;
			string vs = "";
			for (int j = column; j < parseByComma.Length; j++)
			{
				var s = parseByComma[j].Split('"');
				foreach (string x in s)
				{
					if (x == "") continue;
					if (x == "\r") continue;
					vs += x + ",";
				}

			}
			var parse = vs.Split(',');
			List<Skill_origin> skill_s = new List<Skill_origin>();

			foreach(string s in parse)
            {
				var skill = Resources.Load<Skill_origin>("Skill_origin/" + s);
				skill_s.Add(skill);
			}
			enemyData.skilllist = skill_s;
			// インスタンス化したものをアセットとして保存
			var asset = (EnemyEntity)AssetDatabase.LoadAssetAtPath(path, typeof(EnemyEntity));
			if (asset == null)
			{
				// 指定のパスにファイルが存在しない場合は新規作成
				AssetDatabase.CreateAsset(enemyData, path);
			}
			else
			{
				// 指定のパスに既に同名のファイルが存在する場合は更新
				EditorUtility.CopySerialized(enemyData, asset);
				AssetDatabase.SaveAssets();
			}
			AssetDatabase.Refresh();
		}

	}
		
	void SetCsvCardDataToScriptableObject(CsvImporter csvImporter)
        {
			if (csvImporter.csvFile.name == null)
			{
				Debug.LogWarning(csvImporter.name + " : 読み込むCSVファイルがセットされていません。");
				return;
			}
		if (csvImporter.csvFile.name != "CardData")
		{
			Debug.LogWarning(csvImporter.name + " : 読み込むファイルが違います");
			return;
		}

		string csvText = csvImporter.csvFile.text;

			// 改行ごとにパース
			string[] afterParse = csvText.Split('\n');
			for (int i = 1; i < afterParse.Length; i++)
			{
				string[] parseByComma = afterParse[i].Split(',');

				int column = 0;

				// 先頭の列が空であればその行は読み込まない
				if (parseByComma[column] == "")
				{
						continue;
				}

				// 行数をIDとしてファイルを作成
				string fileName = "Card " + (i - 1).ToString() + ".asset";
				string path = "Assets/Resources/CardEntityList/" + fileName;

				// EnemyDataのインスタンスをメモリ上に作成
				var cardData = CreateInstance<CardEntity>();
				

				//ID
				cardData.cardID = int.Parse(parseByComma[column]);
				
				// 名前
				column++;
				cardData.name = parseByComma[column];

				//レア度
				column += 1;
				cardData.rare = parseByComma[column];

				// 数値
				column += 1;
				cardData.num = int.Parse(parseByComma[column]);

				// 最大HP
				column += 1;
				cardData.Hp = int.Parse(parseByComma[column]);

				// 攻撃力
				column += 1;
				cardData.at = int.Parse(parseByComma[column]);

				// 防御力
				column += 1;
				cardData.df = int.Parse(parseByComma[column]);

				// 経験値
				column += 1;
				cardData.firstExp = int.Parse(parseByComma[column]);

				//StageNum
				column++;
				cardData.stageNum  = int.Parse(parseByComma[column]);

				//icon
				column++;
				cardData.icon = Resources.Load<Sprite>("character/" + parseByComma[column]);

				//スキル
				column += 1;
				cardData.AutoSkill = Resources.Load<Skill_origin>("Skill_origin/" + parseByComma[column]);

				//Rスキル
				column += 1;
				var y = parseByComma[column].ToCharArray();
				string s = "";
			
				for(int j = 0; j< y.Length - 1; j++)
				{
					s += y[j];
				}

				cardData.ReaderSkill = Resources.Load<Skill_origin>("Skill_origin/" + s);
		
				// インスタンス化したものをアセットとして保存
				var asset = (CardEntity)AssetDatabase.LoadAssetAtPath(path, typeof(CardEntity));
				if (asset == null)
				{
					// 指定のパスにファイルが存在しない場合は新規作成
					AssetDatabase.CreateAsset(cardData, path);
				}
				else
				{
					// 指定のパスに既に同名のファイルが存在する場合は更新
					EditorUtility.CopySerialized(cardData, asset);
					AssetDatabase.SaveAssets();
				}
				AssetDatabase.Refresh();
			}
		}

	void SetCsvSkillDataToScriptableObject(CsvImporter csvImporter)
	{
		if (csvImporter.csvFile == null)
		{
			Debug.LogWarning(csvImporter.name + " : 読み込むCSVファイルがセットされていません。");
			return;
		}
		if(csvImporter.csvFile.name != "SkillOrigin")
        {
			Debug.LogWarning(csvImporter.name + " : 読み込むファイルが違います");
			return;
		}

		string csvText = csvImporter.csvFile.text;

		// 改行ごとにパース
		string[] afterParse = csvText.Split('\n');
		for (int i = 1; i < afterParse.Length; i++)
		{
			string[] parseByComma = afterParse[i].Split(',');

			int column = 0;

			// 先頭の列が空であればその行は読み込まない
			if (parseByComma[column] == "")
			{
				continue;
			}

			// 行数をIDとしてファイルを作成
			string fileName =  parseByComma[column] + ".asset";
			string path = "Assets/Resources/Skill_origin/" + fileName;
			 

			// EnemyDataのインスタンスをメモリ上に作成
			var enemyData = CreateInstance<Skill_origin>();
			
			//スキル名
			enemyData.skill_name = parseByComma[column];

			

			//スキル説明
			column++;
			enemyData.skill_infomatin = parseByComma[column];

			//スキル条件
			column++;
			int conditon = int.Parse(parseByComma[column]);
			column++;
			
			List<Skill_origin.magic_conditon_origin> _list = new List<Skill_origin.magic_conditon_origin>();
			if (conditon== 1)
            {
				Skill_origin.magic_conditon_origin _Origin = new Skill_origin.magic_conditon_origin();
				//スキルタイプ
				_Origin.type = IntToSkill_Type(int.Parse(parseByComma[column]));

				//スキル条件
				column++;
				int kindnum;
				if (parseByComma[column].Equals("n")) kindnum = (int)Skill_origin.Magic_condition_kind.Length - 1;
				else kindnum = int.Parse(parseByComma[column]);
				_Origin.condition_kind = IntToCondition_kind(kindnum);

				//スキル条件数
				column++;
				_Origin.condition_num = int.Parse(parseByComma[column]);

				//スキル演算
				column++;
				_Origin.magic_kind = IntTokind(int.Parse(parseByComma[column]));

				//スキル効果量
				column++;
				_Origin.effect_size = double.Parse(parseByComma[column]);

				_list.Add(_Origin);
				


			}
            else
            {
				string vs="";
				for(int j = column; j < parseByComma.Length; j++)
                {
					var s = parseByComma[j].Split('"');
					foreach(string x in s)
                    {
						if (x == "") continue;
						if (x == "\r")continue;
						vs += x + ",";
                    }

                }
				var parse = vs.Split(',');
				for(int j = 0; j < conditon; j++)
                {
					Skill_origin.magic_conditon_origin _Origin = new Skill_origin.magic_conditon_origin();
					//スキルタイプ
					_Origin.type = IntToSkill_Type(int.Parse(parse[j]));

					//スキル条件
					int kindnum;
					if (parse[j + conditon].Equals("n")) kindnum = (int)Skill_origin.Magic_condition_kind.Length - 1;
					else kindnum = int.Parse(parse[j + conditon]);
					_Origin.condition_kind = IntToCondition_kind(kindnum);
					//スキル条件数
					_Origin.condition_num = int.Parse(parse[j + conditon*2]);

					//スキル演算
					_Origin.magic_kind = IntTokind(int.Parse(parse[j + conditon*3]));

					//スキル効果量
					_Origin.effect_size = double.Parse(parse[j + conditon * 4]);

					_list.Add(_Origin);
                }
            }
			enemyData.magic_Conditon_Origins = _list;



			// インスタンス化したものをアセットとして保存
			var asset = (Skill_origin)AssetDatabase.LoadAssetAtPath(path, typeof(Skill_origin));
			if (asset == null)
			{
				// 指定のパスにファイルが存在しない場合は新規作成
				AssetDatabase.CreateAsset(enemyData, path);
			}
			else
			{
				// 指定のパスに既に同名のファイルが存在する場合は更新
				EditorUtility.CopySerialized(enemyData, asset);
				AssetDatabase.SaveAssets();
			}
			AssetDatabase.Refresh();
		}
		}

	Skill_origin.Skill_type IntToSkill_Type(int skilltypeid)
	{
		if (skilltypeid < 0|| skilltypeid >= (int)Skill_origin.Skill_type.Length){
			Debug.LogWarning("スキルタイプで誤った値があります");
			
        }
		return (Skill_origin.Skill_type)skilltypeid;
	}
	Skill_origin.Magic_condition_kind IntToCondition_kind(int conditonId)
    {	
		if(conditonId<0 || conditonId>=(int)Skill_origin.Magic_condition_kind.Length)
        {
			Debug.LogWarning("スキル条件で誤った値があります");
        }
		return (Skill_origin.Magic_condition_kind)conditonId;
    }
	Skill_origin.MagicKind IntTokind(int kindId)
	{	
		if(kindId<0 || kindId >=(int)Skill_origin.MagicKind.Length)
        {
			Debug.LogWarning("演算種類で誤った値があります");
        }
		return (Skill_origin.MagicKind)kindId;
	}

	
}

