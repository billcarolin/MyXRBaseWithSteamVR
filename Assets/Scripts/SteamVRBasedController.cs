using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Valve.VR;

/** This class links the Valve VR Steam plugin with the OpenXR actions.  This allows the SteamVR Controller 
    to interact with OpenXR Objects in our scene */
public class SteamVRBasedController : XRBaseController
{
    // SteamVR Tracking
    [Header ("Steam VR Tracking")]
    public SteamVR_Input_Sources inputSource = SteamVR_Input_Sources.Any;
    public SteamVR_Action_Pose actionPose = null;

    [Header ("Steam VR Input")]
    public SteamVR_Action_Boolean selectAction;
    public SteamVR_Action_Boolean activateAction;
    public SteamVR_Action_Boolean interfaceAction;


    // SteamVR Input

    private void Start()
    {
        // Start SteamVR
        SteamVR.Initialize();

    }

    protected override void UpdateTrackingInput(XRControllerState controllerState)
    {
        if(controllerState != null)
        {
            // Get position from pose action
            Vector3 position = actionPose[inputSource].localPosition;
            controllerState.position = position;

            // Get rotation from position action
            Quaternion rotation = actionPose[inputSource].localRotation;
            controllerState.rotation = rotation;
        }
        
    }

    protected override void UpdateInput(XRControllerState controllerState)
    {
        if(controllerState != null) 
        {
            // Reset all of the input values
            controllerState.ResetFrameDependentStates();

            // Check select action, apply to controller
            SetInteractionState(ref controllerState.selectInteractionState, selectAction[inputSource]);

            // Check activate action, apply to controller
            SetInteractionState(ref controllerState.activateInteractionState, activateAction[inputSource]);

            // Check UI action, apply to controller
            SetInteractionState(ref controllerState.uiPressInteractionState, interfaceAction[inputSource]);
        }
        
    }

    private void SetInteractionState(ref InteractionState interactionState, SteamVR_Action_Boolean_Source action)
    {
        // Pressed this frame
        interactionState.activatedThisFrame = action.stateDown;
        // if(action.stateDown){
        //     print("Pressed Down...");
        // }

        // Released this frame
        interactionState.deactivatedThisFrame = action.stateUp;
        // if(action.state){
        //     print("...Released");
        // }

        // Is pressed
        interactionState.active = action.state;
        // if(action.state){
        //     print("Is being Pressed");
        // }
    }
}

