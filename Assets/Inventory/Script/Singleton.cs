using UnityEngine;

[DefaultExecutionOrder(-101)] // ... (1)クラスの下で説明
public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
	public static T Instance = default;

	// ----- Life Cycle Methods -----

	protected virtual void Awake()
	{
		if (Instance)
		{
			Destroy(this.gameObject);
			return;
		}

		Instance = this as T;

		// 親要素があるとDontDestroyOnLoadが機能しないため、親要素を外す
		this.gameObject.transform.parent = null;
		DontDestroyOnLoad(this.gameObject);
	}

	protected virtual void OnDestroy()
	{
		if (ReferenceEquals(this, Instance))
		{
			Instance = null;
		}
	}
}

// (1)
// PlayerInputのExecutionOrderは-100である。
// PlayerInputのAwake()よりも早く実行するために、ExecutionOrderを-101に設定している。