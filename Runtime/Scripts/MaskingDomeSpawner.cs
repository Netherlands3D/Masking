using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.EventSystems;

namespace Netherlands3D.Masking
{
    public class MaskingDomeSpawner : MonoBehaviour
    {
        [Header("Placement actions")]
        [SerializeField] private InputActionReference clickPlacementAction;
        [SerializeField] private float maxCameraTravelToPlacement = 20.0f; 
        [SerializeField] private DisappearDome disappearEffect;
        [SerializeField] private float margin;

        [Header("Global shader settings")]
        [SerializeField] private string sphericalMaskFeatureKeyword = "_SPHERICAL_MASKING";
        [SerializeField] private string sphericalMaskPositionName = "_SphericalMaskPosition";
        [SerializeField] private string sphericalMaskRadiusName = "_SphericalMaskRadius";
        [SerializeField] private bool resetMaskOnDisable = true;
        private int positionPropertyID;
        private int radiusPropertyID;

        [SerializeField] private VisualDome domeVisualisation;

        private Camera mainCamera;
        private Vector3 cameraLookatPosition = Vector3.zero;

        private bool waitForInitialPlacement = false;

        private void Start() {
            mainCamera = Camera.main;
            
            GetPropertyIDs();
            ApplyGlobalShaderVariables();
        }

        private void OnEnable() {
            Shader.EnableKeyword(sphericalMaskFeatureKeyword);

            clickPlacementAction.action.Enable();
            clickPlacementAction.action.started += StartTap;
            clickPlacementAction.action.performed += EndTap;

            StickToPointer();
        }

        private void OnDisable()
        {
            Shader.DisableKeyword(sphericalMaskFeatureKeyword);

            // Unsubscribe and disable the click action when the script is disabled
            clickPlacementAction.action.performed -= StartTap;
            clickPlacementAction.action.Disable();

            if(resetMaskOnDisable)
            {
                ResetGlobalShaderVariables();
            }
        }

        /// <summary>
        /// Initial start will make dome follow pointer untill first click
        /// </summary>
        private void StickToPointer()
        {
            domeVisualisation.AnimateIn();
            waitForInitialPlacement = true;
        }

        public void SpawnDisappearAnimation()
        {
            var newDisappearEffect = Instantiate(disappearEffect.gameObject,this.transform.parent);
            newDisappearEffect.GetComponent<DisappearDome>().DisappearFrom(domeVisualisation.transform.position, domeVisualisation.transform.localScale);
        }

        private void StartTap(InputAction.CallbackContext context)
        {
            cameraLookatPosition = LookPosition();
        }
        private void EndTap(InputAction.CallbackContext context)
        {
            var currentCameraLookatPosition = LookPosition();
            var distanceTraveled = Vector3.Distance(cameraLookatPosition, currentCameraLookatPosition);
            if(distanceTraveled < maxCameraTravelToPlacement)
            {
                PlaceDome();
            }
        }

        private Vector3 LookPosition()
        {
            // Calculate the pointer position in world space
            Ray ray = mainCamera.ScreenPointToRay(Vector3.one*0.5f);
            Plane plane = new Plane(Vector3.up, transform.position);
            plane.Raycast(ray, out float distance);
            Vector3 pointerWorldPosition = ray.GetPoint(distance);

            return pointerWorldPosition;
        }

        private void PlaceDome()
        {
            if(!EventSystem.current.IsPointerOverGameObject()){
                Vector2 pointerPosition = Pointer.current.position.ReadValue();

                if(!waitForInitialPlacement)
                    SpawnDisappearAnimation();

                domeVisualisation.MoveToScreenPoint(pointerPosition);
                domeVisualisation.AnimateIn();
            }

            waitForInitialPlacement = false;
            domeVisualisation.AllowInteraction = true;
        }      

        void Update()
        {
            if(waitForInitialPlacement)
            {
                domeVisualisation.MoveToScreenPoint(Pointer.current.position.ReadValue());
            }

            if (domeVisualisation.transform.hasChanged)
            {
                ApplyGlobalShaderVariables();
                domeVisualisation.transform.hasChanged = false;
            }
        }

        private void GetPropertyIDs(){
            positionPropertyID = Shader.PropertyToID(sphericalMaskPositionName);
            radiusPropertyID = Shader.PropertyToID(sphericalMaskRadiusName);
        }

        private void ApplyGlobalShaderVariables()
        {
            Shader.SetGlobalVector(positionPropertyID,domeVisualisation.transform.position);
            Shader.SetGlobalFloat(radiusPropertyID,(domeVisualisation.transform.localScale.x/2.0f) + margin);
        }

        private void ResetGlobalShaderVariables()
        {
            Shader.SetGlobalVector(positionPropertyID,Vector3.zero);
            Shader.SetGlobalFloat(radiusPropertyID,0.0f);
        }
    }
}
