using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable] // 데이터를 직렬화/역직렬화로 저장하기 위한 클래스
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
	[SerializeField]
	private List<TKey> keys = new List<TKey>();

	[SerializeField]
	private List<TValue> values = new List<TValue>();

	// 직렬화를 이용한 저장
	public void OnBeforeSerialize()
	{
		keys.Clear();
		values.Clear();
		foreach (KeyValuePair<TKey, TValue> pair in this)
		{
			keys.Add(pair.Key);
			values.Add(pair.Value);
		}
	}

	// 역직렬화를 이용한 로드
	public void OnAfterDeserialize()
	{
		this.Clear();

		if (keys.Count != values.Count)
			throw new System.Exception(string.Format("there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable."));

		for (int i = 0; i < keys.Count; i++)
			this.Add(keys[i], values[i]);
	}

}