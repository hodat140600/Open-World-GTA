# How to create a Mission
I write this for the backup purposes and to keep me not forgetting what I have done, or in case someone might need it in the future.

### 1. Create a Mission ScriptableObject
1. Go to _NEWGAME/_Setting/Mission
2. Left click
3. Click on Create/Mission

### 2. General information
- This include Mission order and Cash reward, just follow the GDD for it

### 3. Submissions
- This is the main part of the mission settings. You can think of it as an Event (or it actually is) that the monitor will have to execute in order.
- The "THEN" button indicates whether the monitor must wait until it is "done" before going on to the next submission; when disabled (made grey), the next mission will be executed immediately following the current mission.
> Read the code for more information about how a mission is considered "done".
- List of available submissions, order by its importance:
  - SpawnTommyAt: usually at the first index to spawn Tommy corresponding to the GDD initial location
  - GoTo: usually at the second index to indicate where Tommy has to go to to start the mission. "THEN" should be enabled.
  - Kill: also in the GDD, the mission input said it all. "THEN" should be enabled.
  - SpawnNpcGroup: assign a NpcGroup to a position.
  - ShowDescription: the name said it all.
  - Delay: just a few seconds without doing anything, "THEN" must be enabled in order for this to work.

### 4. Waypoint name
- Some subMissions include the waypoint name attribute, which allows you to identify the position without having to provide the actual coordinates but just its defined name.
- To determine which name should be used, go to Scene "Game", enable the MapEditor, find the position assigned with a name, that is the one you are looking for
- When you successfully input-ted it, the readonly position attribute that is beneath the Waypoint name will be altered to something other than (0,0,0)

### 5. Save
- In case your mission settings does not change, i.e you did not see any change in Github Desktop after creating the mission, press the blue SAVE button.

### 6. If you are extending this Domain
> It is recommended to add new scripts to this Domain rather than changing the existing code as this is almost in the production phase, even if it is a change to the old requirement. 

Deep dive:
- Before extending this, consider reading about the Visitor pattern and the Double dispatch principle, which are both components of the entire subMission code:
  - [Visitor Pattern](https://refactoring.guru/design-patterns/visitor)
  - [Double Dispatch Principle](https://refactoring.guru/design-patterns/visitor-double-dispatch)
- When you want to create a new subMission (or again, Event), create an inheritance to SubMission, refer to the /SubMission folder.
- The Override to Accept method tells you to call an action from the MissionMonitor. Therefore, you will want to create a function in the Monitor to accept this Event as well.
  - The SubMission simply provides information and functions as an immutable object. 
  - The one that actually tells how that subMission should work is the MissionMonitor.
  - You may occasionally want to mutate the SubMission. In those cases, use the ResetMissionProgress method in those circumstances in MissionMonitor.
  > This Reset feature is rarely needed, thus, it is located in the Monitor instead of the SubMission itself. If you find these are needed more often, put them there instead.
- After all the monitoring in the MissionMonitor, it is recommended to call MarkDone on the subMission to declare it as finished. This is for situations where "THEN" was *accidentally* enabled in the editor.
- A ListenTo function was developed in order to, obviously, listen to events from MessageBroker in order to work with actual Observer outside of this domain. Instead of using the Receive function from MessageBroker, you should utilize this as it abstracted all the Subscription work. (MissionMonitor, line 165)
- If it comes from no event, an alternative is Observable, but keep in mind to call .AddTo(MissionEventSubscriptions) to prevent system from unnecessarily subscribing to those events. (MissionMonitor, line 127)
