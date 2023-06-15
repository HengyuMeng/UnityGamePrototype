using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    /// <summary>
    /// <c>UIManager</c> 用于管理UI组件。
    /// 自动加载UI场景，并提供获取指定UI组件控制器的方法。
    /// </summary>
    public class UIManager : StoreBase
    {
        private static UIManager _instance;

        // 单例
        public static UIManager Instance()
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UIManager>();
            }

            return _instance;
        }

        private ChatPanel _chatPanel;
        
        private Scene _uiScene;
        private AsyncOperation _loadScene;
        public bool Loaded => _loadScene.isDone;
        
        /// <summary>
        /// 自动加载UI场景。
        /// </summary>
        protected void Awake()
        {
            _loadScene = SceneManager.LoadSceneAsync("UI",LoadSceneMode.Additive);
            _uiScene = SceneManager.GetSceneByName("UI");
        }

        public override void Start()
        {
            base.Start();
            SetCursorLock(true);
        }
        
        /// <summary>
        /// 设置鼠标的解锁状态
        /// </summary>
        /// <param name="isLock">需要设置的鼠标解锁状态</param>
        public static void SetCursorLock(bool isLock)
        {
            Cursor.lockState = isLock ? CursorLockMode.Locked : CursorLockMode.None;
        }
        
        /// <summary>
        /// 获取指定类型UI组件控制器。
        /// </summary>
        /// <typeparam name="T">UI组件控制器类型</typeparam>
        /// <returns>UI组件控制器</returns>
        public T UI<T>()
        {
            if (!Loaded)
            {
                throw new Exception("Try to get UI element before ui scene loaded.");
            }

            //在场景中找到第一个具有符合要求的GameObject后，获取到他相应的面板组件
            return (_uiScene.GetRootGameObjects().First(o => o.GetComponent<T>() != null)).GetComponent<T>();
        }

        protected override void LoadUI()
        {
            base.LoadUI();
            _chatPanel = UI<ChatPanel>();
        }
        
        public override List<string> InputActions()
        {
            return base.InputActions().Concat(new List<string>
            {
               
            }).ToList();
        }

        /// <summary>
        /// 处理输入事件。
        /// </summary>
        /// <param name="action"></param>
        public override void Receive(IAction action)
        {
            base.Receive(action);
            switch (action.ActionName())
            {
                case ActionID.Stage.Attack:
                    break;
            }
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }
    }
}