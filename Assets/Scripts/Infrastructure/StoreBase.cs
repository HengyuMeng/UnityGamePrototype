using System.Collections.Generic;
using Gameplay.Events;
using Infrastructure;
using UI;
using UnityEngine;

namespace Infrastructure
{
    public class StoreBase:MonoBehaviour
    {
        public Identity id;
        protected bool UIInitialized;

        protected virtual void Identify() => id = new Identity();

        /// <summary>
        /// 查找全局的Dispatcher，将自己注册到这个Dispatcher上。
        /// </summary>
        public virtual void Start()
        {
            Identify();
            Dispatcher.Instance().Register(this);
        }
        
        /// <summary>
        /// 用于加载 UI 面板。
        /// </summary>
        protected virtual void LoadUI()
        {
        }

        protected virtual void FixedUpdate()
        {
            if (!UIInitialized && UIManager.Instance() != null && UIManager.Instance().Loaded)
            {
                LoadUI();
                UIInitialized = true;
            }
        }

        /// <summary>
        /// 在Storebase中声明的action是所有实例共有的
        /// 在子类中可以对这个函数进行覆盖，分别把自己感兴趣的字符串存储到Dispatcher中。
        /// </summary>
        public virtual List<string> InputActions() => new List<string>
            {ActionID.Stage.Attack};

        /// <summary>
        /// 事件处理函数，可重写。
        /// </summary>
        /// <param name="action">事件</param>
        public virtual void Receive(IAction action)
        {
            switch (action.ActionName())
            {
                case ActionID.Stage.Attack:
                    var attackAction = (Attack) action;
                    if (attackAction.Camp == id.camp)
                    {
                        gameObject.GetComponent<Material>().color = Color.red;
                    }
                    break;
            }
        }
        
    }
}