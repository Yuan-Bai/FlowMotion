

using UnityEngine;

public class SingletonMono<T> : MonoBehaviour where T : Component
{
    private static T _instance;
    private static bool _isQuitting = false;
    public static T Instance
    {
        get
        {
            if (_isQuitting)
            {
                Debug.LogWarning($"【单例】程序正在退出，无法获取 {typeof(T)} 实例。");
                return null;
            }
            if (_instance == null)
            {
                // 现在场景中查找
                _instance = FindAnyObjectByType<T>();

                // 没有就自己创建一个
                if (_instance == null)
                {
                    GameObject gameObject = new(typeof(T).Name);
                    _instance = gameObject.AddComponent<T>();
                }
            }

            return  _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            // 已存在其他实例，销毁当前
            Destroy(gameObject);
            return;
        }
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    protected virtual void OnApplicationQuit()
    {
        _isQuitting = true;
    }
}