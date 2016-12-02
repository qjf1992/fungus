﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Fungus
{
    public class SaveGameHelper : MonoBehaviour 
    {
        const string NewGameSavePointKey = "new_game";

        [SerializeField] protected string startScene = "";

        [SerializeField] protected bool autoStartGame = true;

        [SerializeField] protected bool restartDeletesSave = false;

        [SerializeField] protected AudioClip buttonClickClip;

        [SerializeField] protected SaveGameObjects saveGameObjects = new SaveGameObjects();

        protected AudioSource clickAudioSource;

        protected virtual void Awake()
        {
            clickAudioSource = GetComponent<AudioSource>();
        }

        protected virtual void Start()
        {
            var saveManager = FungusManager.Instance.SaveManager;

            if (autoStartGame &&
                saveManager.NumSavePoints == 0)
            {
                SavePointLoaded.NotifyEventHandlers(NewGameSavePointKey);
            }

            CheckSavePointKeys();
        }

        protected void CheckSavePointKeys()
        {
            List<string> keys = new List<string>();

            var savePoints = GameObject.FindObjectsOfType<SavePoint>();

            foreach (var savePoint in savePoints)
            {
                if (string.IsNullOrEmpty(savePoint.SavePointKey))
                {
                    continue;
                }

                if (keys.Contains(savePoint.SavePointKey))
                {
                    Debug.LogError("Save Point Key " + savePoint.SavePointKey + " is defined multiple times.");
                }
                else
                {
                    keys.Add(savePoint.SavePointKey);
                }
            }

        }

        protected void PlayClickSound()
        {
            if (clickAudioSource != null)
            {
                clickAudioSource.Play();
            }
        }

        #region Public methods

        public SaveGameObjects SaveGameObjects { get { return saveGameObjects; } }

        public virtual void Save()
        {
            PlayClickSound();

            var saveManager = FungusManager.Instance.SaveManager;
            saveManager.Save();
        }

        public virtual void Load()
        {
            PlayClickSound();

            var saveManager = FungusManager.Instance.SaveManager;
            saveManager.Load();
        }

        public virtual void Rewind()
        {
            PlayClickSound();

            var saveManager = FungusManager.Instance.SaveManager;
            saveManager.Rewind();
        }

        public virtual void Restart()
        {
            var saveManager = FungusManager.Instance.SaveManager;
            saveManager.ClearHistory();

            if (restartDeletesSave)
            {
                saveManager.Delete();
            }

            PlayClickSound();

            SceneManager.LoadScene(startScene);
        }

        public virtual void LoadScene(string sceneName)
        {
            PlayClickSound();

            SceneManager.LoadScene(sceneName);
        }

        #endregion
    }
}