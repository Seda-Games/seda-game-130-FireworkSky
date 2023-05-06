
    using UnityEngine;

    public class SingleInstance<T> : MonoBehaviour where T : SingleInstance<T>
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    //在场景中根据类型查找引用
                    instance = FindObjectOfType<T>();
                    //如果忘记挂载  T这个脚本了 
                    if (instance == null)
                    {
                        //创建脚本对象（立即执行Awake）
                        new GameObject("Singleton of" + typeof(T).Name).AddComponent<T>();
                    }
                    else
                    {
                        //初始化
                        //instance.Init();
                    }
                }
                return instance;
            }
        }
        /// <summary>
        /// 如果子类需要在Awake操作 重写这个方法
        /// </summary>
        public virtual void Init()
        {

        }
    }


