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

		if (GUILayout.Button("�G�f�[�^�̍쐬"))
		{
			
			SetCsvDataToScriptableObject(csvImpoter);
			Debug.Log("�G�f�[�^�쐬����");
		}
        if (GUILayout.Button("�����f�[�^�̍쐬"))
        {
			
			SetCsvCardDataToScriptableObject(csvImpoter);
			Debug.Log("�����f�[�^�쐬����"); 

		}
		if (GUILayout.Button("�X�L���f�[�^�̍쐬"))
		{
			
			SetCsvSkillDataToScriptableObject(csvImpoter);
			Debug.Log("�X�L���f�[�^�쐬����"); 

		}
	}

	void SetCsvDataToScriptableObject(CsvImporter csvImporter)
	{
		if (csvImporter.csvFile == null)
		{
			Debug.LogWarning(csvImporter.name + " : �ǂݍ���CSV�t�@�C�����Z�b�g����Ă��܂���B");
			return;
		}
		if (csvImporter.csvFile.name != "EnemyData")
		{
			Debug.LogWarning(csvImporter.name + " : �ǂݍ��ރt�@�C�����Ⴂ�܂�");
			return;
		}

		string csvText = csvImporter.csvFile.text;

			// ���s���ƂɃp�[�X
			string[] afterParse = csvText.Split('\n');
		for (int i = 1; i < afterParse.Length; i++)
		{
			string[] parseByComma = afterParse[i].Split(',');

			int column = 0;

			// �擪�̗񂪋�ł���΂��̍s�͓ǂݍ��܂Ȃ�
			if (parseByComma[column] == "")
			{
				continue;
			}

			// �s����ID�Ƃ��ăt�@�C�����쐬
			string fileName = "Enemy" + (i-1).ToString() + ".asset";
			string path = "Assets/Resources/EnemyEntityList/" + fileName;

			// EnemyData�̃C���X�^���X����������ɍ쐬
			var enemyData = CreateInstance<EnemyEntity>();

			//ID
			enemyData.EnemyId = int.Parse(parseByComma[column]);
			// ���O
			column++;
			enemyData.name = parseByComma[column];

			// �ő�HP
			column += 1;
			enemyData.Hp = int.Parse(parseByComma[column]);

			//�U����
			column++;
			enemyData.at = int.Parse(parseByComma[column]);

			// �h���
			column += 1;
			enemyData.df = int.Parse(parseByComma[column]);

			// ���l
			column += 1;
			enemyData.numba = int.Parse(parseByComma[column]);

			// �o���l
			column += 1;
			enemyData._exp = int.Parse(parseByComma[column]);

			//icon
			column += 1;
			enemyData.icon = Resources.Load<Sprite>("character/" + parseByComma[column]);
			
			//�X�L��
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
			// �C���X�^���X���������̂��A�Z�b�g�Ƃ��ĕۑ�
			var asset = (EnemyEntity)AssetDatabase.LoadAssetAtPath(path, typeof(EnemyEntity));
			if (asset == null)
			{
				// �w��̃p�X�Ƀt�@�C�������݂��Ȃ��ꍇ�͐V�K�쐬
				AssetDatabase.CreateAsset(enemyData, path);
			}
			else
			{
				// �w��̃p�X�Ɋ��ɓ����̃t�@�C�������݂���ꍇ�͍X�V
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
				Debug.LogWarning(csvImporter.name + " : �ǂݍ���CSV�t�@�C�����Z�b�g����Ă��܂���B");
				return;
			}
		if (csvImporter.csvFile.name != "CardData")
		{
			Debug.LogWarning(csvImporter.name + " : �ǂݍ��ރt�@�C�����Ⴂ�܂�");
			return;
		}

		string csvText = csvImporter.csvFile.text;

			// ���s���ƂɃp�[�X
			string[] afterParse = csvText.Split('\n');
			for (int i = 1; i < afterParse.Length; i++)
			{
				string[] parseByComma = afterParse[i].Split(',');

				int column = 0;

				// �擪�̗񂪋�ł���΂��̍s�͓ǂݍ��܂Ȃ�
				if (parseByComma[column] == "")
				{
						continue;
				}

				// �s����ID�Ƃ��ăt�@�C�����쐬
				string fileName = "Card " + (i - 1).ToString() + ".asset";
				string path = "Assets/Resources/CardEntityList/" + fileName;

				// EnemyData�̃C���X�^���X����������ɍ쐬
				var cardData = CreateInstance<CardEntity>();
				

				//ID
				cardData.cardID = int.Parse(parseByComma[column]);
				
				// ���O
				column++;
				cardData.name = parseByComma[column];

				//���A�x
				column += 1;
				cardData.rare = parseByComma[column];

				// ���l
				column += 1;
				cardData.num = int.Parse(parseByComma[column]);

				// �ő�HP
				column += 1;
				cardData.Hp = int.Parse(parseByComma[column]);

				// �U����
				column += 1;
				cardData.at = int.Parse(parseByComma[column]);

				// �h���
				column += 1;
				cardData.df = int.Parse(parseByComma[column]);

				// �o���l
				column += 1;
				cardData.firstExp = int.Parse(parseByComma[column]);

				//StageNum
				column++;
				cardData.stageNum  = int.Parse(parseByComma[column]);

				//icon
				column++;
				cardData.icon = Resources.Load<Sprite>("character/" + parseByComma[column]);

				//�X�L��
				column += 1;
				cardData.AutoSkill = Resources.Load<Skill_origin>("Skill_origin/" + parseByComma[column]);

				//R�X�L��
				column += 1;
				var y = parseByComma[column].ToCharArray();
				string s = "";
			
				for(int j = 0; j< y.Length - 1; j++)
				{
					s += y[j];
				}

				cardData.ReaderSkill = Resources.Load<Skill_origin>("Skill_origin/" + s);
		
				// �C���X�^���X���������̂��A�Z�b�g�Ƃ��ĕۑ�
				var asset = (CardEntity)AssetDatabase.LoadAssetAtPath(path, typeof(CardEntity));
				if (asset == null)
				{
					// �w��̃p�X�Ƀt�@�C�������݂��Ȃ��ꍇ�͐V�K�쐬
					AssetDatabase.CreateAsset(cardData, path);
				}
				else
				{
					// �w��̃p�X�Ɋ��ɓ����̃t�@�C�������݂���ꍇ�͍X�V
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
			Debug.LogWarning(csvImporter.name + " : �ǂݍ���CSV�t�@�C�����Z�b�g����Ă��܂���B");
			return;
		}
		if(csvImporter.csvFile.name != "SkillOrigin")
        {
			Debug.LogWarning(csvImporter.name + " : �ǂݍ��ރt�@�C�����Ⴂ�܂�");
			return;
		}

		string csvText = csvImporter.csvFile.text;

		// ���s���ƂɃp�[�X
		string[] afterParse = csvText.Split('\n');
		for (int i = 1; i < afterParse.Length; i++)
		{
			string[] parseByComma = afterParse[i].Split(',');

			int column = 0;

			// �擪�̗񂪋�ł���΂��̍s�͓ǂݍ��܂Ȃ�
			if (parseByComma[column] == "")
			{
				continue;
			}

			// �s����ID�Ƃ��ăt�@�C�����쐬
			string fileName =  parseByComma[column] + ".asset";
			string path = "Assets/Resources/Skill_origin/" + fileName;
			 

			// EnemyData�̃C���X�^���X����������ɍ쐬
			var enemyData = CreateInstance<Skill_origin>();
			
			//�X�L����
			enemyData.skill_name = parseByComma[column];

			

			//�X�L������
			column++;
			enemyData.skill_infomatin = parseByComma[column];

			//�X�L������
			column++;
			int conditon = int.Parse(parseByComma[column]);
			column++;
			
			List<Skill_origin.magic_conditon_origin> _list = new List<Skill_origin.magic_conditon_origin>();
			if (conditon== 1)
            {
				Skill_origin.magic_conditon_origin _Origin = new Skill_origin.magic_conditon_origin();
				//�X�L���^�C�v
				_Origin.type = IntToSkill_Type(int.Parse(parseByComma[column]));

				//�X�L������
				column++;
				int kindnum;
				if (parseByComma[column].Equals("n")) kindnum = (int)Skill_origin.Magic_condition_kind.Length - 1;
				else kindnum = int.Parse(parseByComma[column]);
				_Origin.condition_kind = IntToCondition_kind(kindnum);

				//�X�L��������
				column++;
				_Origin.condition_num = int.Parse(parseByComma[column]);

				//�X�L�����Z
				column++;
				_Origin.magic_kind = IntTokind(int.Parse(parseByComma[column]));

				//�X�L�����ʗ�
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
					//�X�L���^�C�v
					_Origin.type = IntToSkill_Type(int.Parse(parse[j]));

					//�X�L������
					int kindnum;
					if (parse[j + conditon].Equals("n")) kindnum = (int)Skill_origin.Magic_condition_kind.Length - 1;
					else kindnum = int.Parse(parse[j + conditon]);
					_Origin.condition_kind = IntToCondition_kind(kindnum);
					//�X�L��������
					_Origin.condition_num = int.Parse(parse[j + conditon*2]);

					//�X�L�����Z
					_Origin.magic_kind = IntTokind(int.Parse(parse[j + conditon*3]));

					//�X�L�����ʗ�
					_Origin.effect_size = double.Parse(parse[j + conditon * 4]);

					_list.Add(_Origin);
                }
            }
			enemyData.magic_Conditon_Origins = _list;



			// �C���X�^���X���������̂��A�Z�b�g�Ƃ��ĕۑ�
			var asset = (Skill_origin)AssetDatabase.LoadAssetAtPath(path, typeof(Skill_origin));
			if (asset == null)
			{
				// �w��̃p�X�Ƀt�@�C�������݂��Ȃ��ꍇ�͐V�K�쐬
				AssetDatabase.CreateAsset(enemyData, path);
			}
			else
			{
				// �w��̃p�X�Ɋ��ɓ����̃t�@�C�������݂���ꍇ�͍X�V
				EditorUtility.CopySerialized(enemyData, asset);
				AssetDatabase.SaveAssets();
			}
			AssetDatabase.Refresh();
		}
		}

	Skill_origin.Skill_type IntToSkill_Type(int skilltypeid)
	{
		if (skilltypeid < 0|| skilltypeid >= (int)Skill_origin.Skill_type.Length){
			Debug.LogWarning("�X�L���^�C�v�Ō�����l������܂�");
			
        }
		return (Skill_origin.Skill_type)skilltypeid;
	}
	Skill_origin.Magic_condition_kind IntToCondition_kind(int conditonId)
    {	
		if(conditonId<0 || conditonId>=(int)Skill_origin.Magic_condition_kind.Length)
        {
			Debug.LogWarning("�X�L�������Ō�����l������܂�");
        }
		return (Skill_origin.Magic_condition_kind)conditonId;
    }
	Skill_origin.MagicKind IntTokind(int kindId)
	{	
		if(kindId<0 || kindId >=(int)Skill_origin.MagicKind.Length)
        {
			Debug.LogWarning("���Z��ނŌ�����l������܂�");
        }
		return (Skill_origin.MagicKind)kindId;
	}

	
}

