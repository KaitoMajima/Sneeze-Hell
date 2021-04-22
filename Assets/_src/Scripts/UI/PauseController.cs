using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace KaitoCo
{
    public class PauseController : MonoBehaviour
    {
        [SerializeField] private PlayerInput uiExtraInput;

        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private GameObject pauseMenu;
        private bool pauseActivated = false;
        private InputActionMap playerMap;
        private InputAction pauseAction;
        private bool canPause = true;

        private void Start()
        {
            var uiExtraMap  = uiExtraInput.actions.FindActionMap("UIExtra");
            playerMap  = playerInput.actions.FindActionMap("Player");
            pauseAction = uiExtraMap.FindAction("Pause");
            pauseAction.performed += Pause;
        }

        private void Pause(InputAction.CallbackContext context)
        {
            if(!canPause)
                return;
            pauseActivated = !pauseActivated;
            pauseMenu.SetActive(pauseActivated);

            if(pauseActivated)
                PauseStop();
            if(!pauseActivated)
                PauseRelease();
        }

        private void PauseStop()
        {
            Time.timeScale = 0;
            playerMap.Disable();
        }

        private void PauseRelease()
        {
            Time.timeScale = 1;
            playerMap.Enable();
        }
        public void RestraintPause(bool shouldPause)
        {
            canPause = shouldPause;
        }
        public void ReloadScene()
        {
            SceneManager.LoadScene(gameObject.scene.name);
        }
        private void OnDestroy()
        {
            PauseRelease();
            pauseAction.performed -= Pause;
        }
    }
}
