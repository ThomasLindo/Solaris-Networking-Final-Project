using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using Sirenix.OdinInspector;


public class Event_Manager : SerializedMonoBehaviour
{
    public static Event_Manager instance;
    private void Awake() { instance = this; }

	#region plugin
	[StructLayout(LayoutKind.Sequential)]
	public struct Vec3 { public float x, y, z; }

	[DllImport("EventManager")]
	public static extern void DestroyManager();
	[DllImport("EventManager")]
	public static extern void initAccess();    // this must be called once to initialize the event manager access pointer
	[DllImport("EventManager")]
	public static extern void createEventSet();
	[DllImport("EventManager")]
	public static extern void newEvent(int eventSetIndex);
	[DllImport("EventManager")]
	public static extern void moveOn(int eventSetIndex, int eventIndex);       // move to the next MyEvent in the hierarchy
	[DllImport("EventManager")]
	public static extern void addWaypoint(int eventSetIndex, Vec3 waypoint);   // add a waypoint to the waypoint vector
	[DllImport("EventManager")]
	public static extern void addWaypointInd(int eventSetIndex, float x, float y, float z);    // same as addWaypoint except pass individual values
	[DllImport("EventManager")]
	public static extern void addAchievement(string s);
	[DllImport("EventManager")]
	public static extern bool checkRadius(int eventSetIndex, int eventIndex, Vec3 pos, int waypointIndex, float radius);   // check the radius relative to a waypoint
	[DllImport("EventManager")]
	public static extern int genNextEventInfo(int min, int max);
	[DllImport("EventManager")]
	public static extern void resetEvent(int index);
	[DllImport("EventManager")]
	public static extern void setActiveEventSet(int index, bool status);
	[DllImport("EventManager")]
	public static extern void setEventSetName(int index, string n);
	[DllImport("EventManager")]
	public static extern void setEventName(int eventSetIndex, int eventIndex, string n);
	[DllImport("EventManager")]
	public static extern void setParent(int eventSetIndex, int eventIndex, int parentEventIndex);  // set parent to the current MyEvent
	[DllImport("EventManager")]
	public static extern void setImmediateChild(int eventSetIndex, int eventIndex, int parentEventIndex);  // set the immediate (next) MyEvent in the hierarchy
	[DllImport("EventManager")]
	public static extern void setActiveEvent(int eventSetIndex, int eventIndex, bool stat);
	[DllImport("EventManager")]
	public static extern void setIconFilepath(int eventSetIndex, int eventIndex, char f);
	[DllImport("EventManager")]
	public static extern void setLast(int eventSetIndex, int eventIndex);  // sets the MyEvent given as the last in its hierarchy
	[DllImport("EventManager")]
	public static extern void setAchievementStatusSearch(string name, bool stat);
	[DllImport("EventManager")]
	public static extern void setAchievementStatus(int index, bool stat);
	[DllImport("EventManager")]
	public static extern void setAchievementDesc(int index, string desc);
	[DllImport("EventManager")]
	public static extern void setLastRoundEventHappened(int r);
	[DllImport("EventManager")]
	public static extern bool getEventStatus(int eventSetIndex, int eventIndex);       // returns whether the MyEvent has been completed or not
	[DllImport("EventManager")]
	public static extern bool getAchievementStatus(string name);
	[DllImport("EventManager")]
	public static extern string getAchievementDesc(int index);
	[DllImport("EventManager")]
	public static extern IntPtr getAchievementName(int achievementNum);
	[DllImport("EventManager")]
	public static extern IntPtr getEventName(int eventSetIndex, int eventIndex);
	[DllImport("EventManager")]
	public static extern bool isEventActive(int eventSetIndex, int eventIndex);
	//public static extern int getActiveEventSet();
	[DllImport("EventManager")]
	public static extern IntPtr getIconFilePath(int eventSetIndex, int eventIndex);
	[DllImport("EventManager")]
	public static extern float getWaypointPos(int eventSetIndex, int eventIndex, string whichAxis);
	[DllImport("EventManager")]
	public static extern int getNumObjectives(int eventSetIndex);   //returns number of objectives in a particular event set
	[DllImport("EventManager")]
	public static extern int getLastRoundEventHappened();
	[DllImport("EventManager")]
	public static extern int getNextEvent();
	[DllImport("EventManager")]
	public static extern int getLastEvent();
	#endregion

	public Text prompt;
	public MenuManager menu;
	public GameObject firePrefab;
	public GameObject barnBarrier;
	public GameObject bunkerEntrance;
	public GameObject generatorRoom;
    public UnHide Flood;
    public UnHide generator;
	public UnHide brokenGenerator;
	public bool playerInEntranceBoundary = false;
	public bool playerInGRBoundary = false;
	public bool newGennyPlaced = false;
	public bool gennyReplaced = false;
	public int eventRoundProgress = 0;  // how many rounds passed since event started
	public bool carrying = false;
	public static int fireCount = 0;   // current fire count for the barn fire
	public static int nextEventRound = 0;  //	0 just for initialization
	public static int active = 100;   // 100 for null (essentially, not literally)
	public static bool fireFailed = false;
	private bool fireSpawned = false;
	private bool bunkerFlooded = false;
	private bool outageFailed = false;
	private bool floodedFailed = false;
	private bool hasEventSetActive = false;	//	keeps track if the event set has been set to active or not just so we're not repeatedly setting it to true
	private bool promptSpawned = false;
	private GameObject fire;
	private float KeepTime = 0.0f;
	private int fireCountMax = 10;

    [HorizontalGroup("Basic Info 1", 0.5f), LabelWidth(50)]
    public float StartIn = 1.0f;
    [HorizontalGroup("Basic Info 1", 0.5f), LabelWidth(90)]
    public float RepeatEvery = 1.0f;

    private bool Islonely = false;
    private bool IsServer = false;
    private Server_Manager server_Manager;

    // Use this for initialization
    private void Start() {
		createEvents();
		Debug.Log("Events created successfully!");
		createAchievements();
		Debug.Log("Achievements created successfully!");
		nextEventRound = 4;

        Invoke("LateStart", 0.5f);
        InvokeRepeating("UpdateControlled", StartIn, RepeatEvery);
    }
    private void LateStart() {
        if (Client_Manager.instance) { IsServer = false; }
        else if (Server_Manager.instance) { IsServer = true; Server_Manager.instance = server_Manager; }
        else { Islonely = true; }
    }

    //Update but with controlled timing
    private void UpdateControlled() {

        // check if the current round = anticpated event round
		if (Islonely) {
            if (EnemySpawners.Interface_SpawnTable.instance.CurrentLevel == nextEventRound) {
				if (EnemySpawners.Interface_SpawnTable.instance.CurrentLevel == 4) { active = 0; }
				else {	//	ensure that an event is not done twice in a row
					active = getNextEvent();
					if (getLastEvent() == active)
					{
						if (active == 0)
						{
							active = UnityEngine.Random.Range(1, 2);
						}
						else if (active == 1)
						{
							active = UnityEngine.Random.Range(0, 2);
							if (active == 1)
							{
								active = 0;
							}
						}
						else if (active == 2)
						{
							active = UnityEngine.Random.Range(0, 1);
						}
					}
				}
                eventRoundProgress = 0;
                nextEventRound = genNextEventInfo(3, 4) + EnemySpawners.Interface_SpawnTable.instance.CurrentLevel;
                Debug.Log("Round next event will happen: " + nextEventRound);
                Debug.Log("Next event: " + getNextEvent());
            }

            if (active == 0) { FireEvent(); }
            else if (active == 1) { OutageEvent(); }
            else if (active == 2) { FloodedEvent(); }
        }
		else if (IsServer) { 
		    // check if the current round = anticpated event round
		    if (EnemySpawners.Interface_SpawnTable.instance.CurrentLevel == nextEventRound) {
		    	active = getNextEvent();
                eventRoundProgress = 0;
		    	nextEventRound = genNextEventInfo(3, 6) + EnemySpawners.Interface_SpawnTable.instance.CurrentLevel;
		    	Debug.Log("Round next event will happen: " + nextEventRound);
		    	Debug.Log("Next event: " + getNextEvent());
            }

            if (active == 0) { FireEvent_Server(); }
            else if (active == 1) { OutageEvent_Server(); }
            else if (active == 2) { FloodedEvent_Server(); }
        }
        else if (!IsServer && !Islonely) {
            if (Tutorial_Manager.instance.Stage == nextEventRound) {
                active = getNextEvent();
                eventRoundProgress = 0;
                nextEventRound = genNextEventInfo(3, 6) + Tutorial_Manager.instance.Stage;
                Debug.Log("Round next event will happen: " + nextEventRound);
                Debug.Log("Next event: " + getNextEvent());
            }

            if (active == 0) { FireEvent(); }
            else if (active == 1) { OutageEvent(); }
            else if (active == 2) { FloodedEvent(); }
        }


		if (fireFailed) {
			//	if the player failed to finish to beat the fire event fire spreads around barn, preventing the player from going inside and fires become invincible
			if (KeepTime == 0.0f) {
				prompt.color = Color.red;
				prompt.text = "EVENT FAILED!";
				KeepTime += RepeatEvery; 
				for (int i = 11; i < 14; i++) { fire = Instantiate(firePrefab, transform.GetChild(0).transform.GetChild(i).transform); }
			}
			else if (KeepTime < 10.0f) { KeepTime += RepeatEvery; }
			else if (KeepTime >= 10.0f && KeepTime < (20.0f + RepeatEvery)) {
				prompt.text = "";
				KeepTime = 20.0f + RepeatEvery;
				prompt.color = Color.white;
			}
		}
		if (outageFailed) {
			if (KeepTime == 0.0f) {
				prompt.color = Color.red;
				prompt.text = "EVENT FAILED!";
				KeepTime += RepeatEvery;
			}
			else if (KeepTime < 5.0f) { KeepTime += RepeatEvery; }
			else if (KeepTime >= 5.0f && KeepTime < (20.0f + RepeatEvery)) {
				prompt.text = "";
				KeepTime = 20.0f + RepeatEvery;
				prompt.color = Color.white;
			}
		}
		if (floodedFailed) {
			if (KeepTime == 0.0f) {
				prompt.color = Color.red;
				prompt.text = "EVENT FAILED!";
				KeepTime += RepeatEvery;
			}
			else if (KeepTime < 5.0f) { KeepTime += RepeatEvery; }
			else if (KeepTime >= 5.0f && KeepTime < (20.0f + RepeatEvery)) {
				prompt.text = "";
				KeepTime = 20.0f + RepeatEvery;
				prompt.color = Color.white;
			}
		}
		if (menu == null) { menu = GameObject.FindObjectOfType(typeof(MenuManager)) as MenuManager; }
	}
    //Fire Event but with controlled timing //	handles fire event while it is actives
    private void FireEvent() {
        // give 2 rounds to complete the event
        if (eventRoundProgress < 3)	 {
			if (!fireSpawned) {
				for (int i = 0; i < fireCountMax; i++) {
					fire = Instantiate(firePrefab, transform.GetChild(0).transform.GetChild(i).transform);
					fireCount++;
				}
				setActiveEventSet(0, true);
				fireSpawned = true;
				KeepTime = 0.0f;
			} 
			if (!getEventStatus(0, 0) && isEventActive(0, 0)) {
				if (!promptSpawned) {
					prompt.text = "The gnomes set the barn on fire! Find the fire extinguisher!";
					promptSpawned = true;
				} 
                Component[] Slots;
                if (menu != null) { 
                    // check if fire extinguisher is in any players' inventory	
                    Slots = menu.Inventory_Slot.GetComponentsInChildren<Drag_Inventory>();	
                    foreach (Drag_Inventory slots in Slots) {
                        if (slots.typeOfItem == Drag_Inventory.Slot.Extinguisher) {
							moveOn(0, 0);
							promptSpawned = false;
						}
                    }
		            // check if fire extinguisher is in any players' inventory		
		            Slots = menu.Weapon_Slot.GetComponentsInChildren<Drag_Inventory>();
		            foreach (Drag_Inventory slots in Slots) {
		                if (slots.typeOfItem == Drag_Inventory.Slot.Extinguisher) {
						    moveOn(0, 0);
						    promptSpawned = false;
					    }
					}
                }
			}
			else if (!getEventStatus(0, 1) && isEventActive(0, 1)) {
				prompt.text = "Put out all of the fires. " + fireCount + "/" + fireCountMax + " Remaining";
				promptSpawned = true; 
				// check if all of the fires have been put out
				if (fireCount < 1) {
					moveOn(0, 1);
					promptSpawned = false;
				}
			}
			else if (getEventStatus(0, 1) && fireCount == 0) {
				if (KeepTime == 0.0f) {
					prompt.text = "THE BARN IS SAFE!";
					KeepTime += RepeatEvery;
				}
				else if (KeepTime < 5.0f) { KeepTime += RepeatEvery; }
				else if (KeepTime >= 5.0f && KeepTime < (20.0f + RepeatEvery)) {
					prompt.text = "";
					KeepTime = 20.0f + RepeatEvery;
					active = 100;
					resetEvent(0);
					fireSpawned = false;
				}
			}
		}
        // event was not completed in time
        else if (eventRoundProgress >= 3 && !getEventStatus(0, 1)) {
			active = 100;
			fireFailed = true;
			fireSpawned = false;
			//barnBarrier.GetComponent<Collider>().isTrigger = false;
		}
	}
    private void FireEvent_Server() {
        // give 2 rounds to complete the event
        if (eventRoundProgress < 3)	 {
			if (!fireSpawned) {
				for (int i = 0; i < fireCountMax; i++) {
					fire = Instantiate(firePrefab, transform.GetChild(0).transform.GetChild(i).transform);
					fireCount++;
				}
                server_Manager.SendData("!(0,0,1)");
                setActiveEventSet(0, true);
				fireSpawned = true;
				KeepTime = 0.0f;
			} 
			if (!getEventStatus(0, 0) && isEventActive(0, 0)) {
				if (!promptSpawned) {
					prompt.text = "The gnomes set the barn on fire! Find the fire extinguisher!";
					promptSpawned = true;
				} 
                Component[] Slots;
                if (menu != null) { 
                    // check if fire extinguisher is in any players' inventory	
                    Slots = menu.Inventory_Slot.GetComponentsInChildren<Drag_Inventory>();	
                    foreach (Drag_Inventory slots in Slots) {
                        if (slots.typeOfItem == Drag_Inventory.Slot.Extinguisher) {
                            server_Manager.SendData("!(0,0,0)");
                            server_Manager.SendData("!(0,1,1)");
                            moveOn(0, 0);
							promptSpawned = false;
						}
                    }
		            // check if fire extinguisher is in any players' inventory		
		            Slots = menu.Weapon_Slot.GetComponentsInChildren<Drag_Inventory>();
		            foreach (Drag_Inventory slots in Slots) {
		                if (slots.typeOfItem == Drag_Inventory.Slot.Extinguisher) {
                            server_Manager.SendData("!(0,0,0)");
                            server_Manager.SendData("!(0,1,1)");
						    moveOn(0, 0);
						    promptSpawned = false;
					    }
					}
                }
			}
			else if (!getEventStatus(0, 1) && isEventActive(0, 1)) {
				prompt.text = "Put out all of the fires. " + fireCount + "/" + fireCountMax + " Remaining";
				promptSpawned = true; 
				// check if all of the fires have been put out
				if (fireCount < 1) {
                    server_Manager.SendData("!(0,1,0)");
					moveOn(0, 1);
					promptSpawned = false;
				}
			}
			else if (getEventStatus(0, 1) && fireCount == 0) {
				if (KeepTime == 0.0f) {
					prompt.text = "THE BARN IS SAFE!";
					KeepTime += RepeatEvery;
				}
				else if (KeepTime < 5.0f) { KeepTime += RepeatEvery; }
				else if (KeepTime >= 5.0f && KeepTime < (20.0f + RepeatEvery)) {
					prompt.text = "";
					KeepTime = 20.0f + RepeatEvery;
					active = 100;
					resetEvent(0);
					fireSpawned = false;
				}
			}
		}
        // event was not completed in time
        else if (eventRoundProgress >= 3 && !getEventStatus(0, 1)) {
			active = 100;
			fireFailed = true;
			fireSpawned = false;
			//barnBarrier.GetComponent<Collider>().isTrigger = false;
		}
	}

    //	handles outage event while it is active
    private void OutageEvent() {
		if (eventRoundProgress < 3) {
			if (!hasEventSetActive) {
				PowerScript.isActive = false;
				setActiveEventSet(1, true);
				hasEventSetActive = true;
				promptSpawned = false;
				KeepTime = 0.0f;
			}

			if (!getEventStatus(1, 0) && isEventActive(1, 0)) {
				// check if a player is in the bunker space
				if (!promptSpawned) {
					prompt.text = "The gnomes shut the power off! Get to the bunker!";
					promptSpawned = true;
				}
				//Debug.Log("player is in boundary " + playerInEntranceBoundary); 
				if (playerInEntranceBoundary) {
					moveOn(1, 0);
					setActiveEvent(1, 1, true);
					promptSpawned = false;
				}
			}
			else if (!getEventStatus(1, 1) && isEventActive(1, 1)) {
				// check if a player has reached the generator room
				if (!promptSpawned) {
					prompt.text = "Find the power room";
					promptSpawned = true;
				} 
				if (playerInGRBoundary) {
					moveOn(1, 1);
					promptSpawned = false;
				}
			}
			else if (!getEventStatus(1, 2) && isEventActive(1, 2)) {
				// check the generator status
				if (!promptSpawned) {
					prompt.text = "Activate the power switch";
					promptSpawned = true;
				} 
				if (PowerScript.isActive) {
					moveOn(1, 2);
					promptSpawned = false;
				}
			}
			else if (getEventStatus(1, 2) && PowerScript.isActive) {
				if (KeepTime == 0.0f) {
					prompt.text = "Power has returned! You can access RainForest again";
					KeepTime += RepeatEvery;
				}
				else if (KeepTime < 5.0f) { KeepTime += RepeatEvery; }
				else if (KeepTime >= 5.0f && KeepTime < (6.0f + RepeatEvery)) {
					prompt.text = "";
					KeepTime = 6.0f + RepeatEvery;
					active = 100;
					resetEvent(1);
					hasEventSetActive = false;
				}
			}
		}
		else if (eventRoundProgress >= 3 && !getEventStatus(1, 2)) {
			active = 100;
			outageFailed = true;
			hasEventSetActive = false;
		}
	}
    private void OutageEvent_Server() {
		if (eventRoundProgress < 3) {
			if (!hasEventSetActive) {
				PowerScript.isActive = false;
                server_Manager.SendData("!(1,0,1)");
                setActiveEventSet(1, true);
				hasEventSetActive = true;
				promptSpawned = false;
				KeepTime = 0.0f;
			}

			if (!getEventStatus(1, 0) && isEventActive(1, 0)) {
				// check if a player is in the bunker space
				if (!promptSpawned) {
					prompt.text = "The gnomes shut the power off! Get to the bunker!";
					promptSpawned = true;
				}
				//Debug.Log("player is in boundary " + playerInEntranceBoundary); 
				if (playerInEntranceBoundary) {
                    server_Manager.SendData("!(1,0,0)");
                    server_Manager.SendData("!(1,1,1)");
                    moveOn(1, 0);
					setActiveEvent(1, 1, true);
					promptSpawned = false;
				}
			}
			else if (!getEventStatus(1, 1) && isEventActive(1, 1)) {
				// check if a player has reached the generator room
				if (!promptSpawned) {
					prompt.text = "Find the power room";
					promptSpawned = true;
				} 
				if (playerInGRBoundary) {
                    server_Manager.SendData("!(1,1,0)");
                    server_Manager.SendData("!(1,2,1)");
					moveOn(1, 1);
					promptSpawned = false;
				}
			}
			else if (!getEventStatus(1, 2) && isEventActive(1, 2)) {
				// check the generator status
				if (!promptSpawned) {
					prompt.text = "Activate the power switch";
					promptSpawned = true;
				} 
				if (PowerScript.isActive) {
                    server_Manager.SendData("!(1,2,0)");
					moveOn(1, 2);
					promptSpawned = false;
				}
			}
			else if (getEventStatus(1, 2) && PowerScript.isActive) {
				if (KeepTime == 0.0f) {
					prompt.text = "Power has returned! You can access RainForest again";
					KeepTime += RepeatEvery;
				}
				else if (KeepTime < 5.0f) { KeepTime += RepeatEvery; }
				else if (KeepTime >= 5.0f && KeepTime < (6.0f + RepeatEvery)) {
					prompt.text = "";
					KeepTime = 6.0f + RepeatEvery;
					active = 100;
					resetEvent(1);
					hasEventSetActive = false;
				}
			}
		}
		else if (eventRoundProgress >= 3 && !getEventStatus(1, 2)) {
			active = 100;
			outageFailed = true;
			hasEventSetActive = false;
		}
	}

    //	handles flooded event while it is active
    private void FloodedEvent() {
		if(eventRoundProgress < 3) {
			if (!hasEventSetActive) {
				setActiveEventSet(2, true);
				hasEventSetActive = true;
				GeneratorScript.eventIsActive = true;
				KeepTime = 0.0f;
			}

			if(!getEventStatus(2,0) && isEventActive(2, 0)) {
				if (!promptSpawned) {
					prompt.text = "The bunker generator was broken! Find the replacement!";
					promptSpawned = true;
					GeneratorScript.isActive = false;
				}

				if (carrying) {
					generator.Hide();
					moveOn(2, 0);
					setActiveEvent(2, 1, true);
					promptSpawned = false;
				}
			}
			else if(!getEventStatus(2,1) && isEventActive(2, 1)) {
				if (!promptSpawned) {
					prompt.text = "Find the old generator in the bunker";
					promptSpawned = true;
				}

				if (newGennyPlaced)
				{
					moveOn(2, 1);
					promptSpawned = false;
				}
			}
			else if(!getEventStatus(2,2) && isEventActive(2, 2)) {
				if (!promptSpawned) {
					prompt.text = "Stand by the generator to replace it";
					promptSpawned = true;
					GeneratorScript.eventIsActive = true;
				}
				Debug.Log(gennyReplaced);
				if(gennyReplaced)
				{
					generator.View();
					ButtonPrompt.promptActive = false;
					promptSpawned = false;
					moveOn(2, 2);
				}
			}
			else if(getEventStatus(2,2) && !isEventActive(2, 2))
			{
				if (KeepTime == 0.0f)
				{
					prompt.text = "You Stopped The Bunker From Flooding!";
					KeepTime += RepeatEvery;
				}
				else if (KeepTime < 5.0f) { KeepTime += RepeatEvery; }
				else if (KeepTime >= 5.0f && KeepTime < (6.0f + RepeatEvery))
				{
					prompt.text = "";
					KeepTime = 6.0f + RepeatEvery;
					active = 100;
					resetEvent(2);
					hasEventSetActive = false;
					gennyReplaced = false;
					newGennyPlaced = false;
                    GeneratorScript.isActive = true;
				}
			}
		}
		else if (eventRoundProgress >= 3 && !getEventStatus(2, 2)) {
			active = 100;
			floodedFailed = true;
			resetEvent(2);
			hasEventSetActive = false;
			gennyReplaced = false;
			newGennyPlaced = false;
            Flood.View();

        }
	}
    private void FloodedEvent_Server() {
        //Needs to be updated with 

        //server_Manager.SendData("!(2,0,0)");
        //server_Manager.SendData("!(2,1,1)");

    }

	private void OnDestroy() { DestroyManager(); }

	private string ansiIt(IntPtr str) { return Marshal.PtrToStringAnsi(str); }

	private void createEvents() {
		initAccess();

		// barn fire event
		createEventSet();
		setEventSetName(0, "Barn Fire");
		newEvent(0);
		setEventName(0, 0, "The Gnomes set the Barn on Fire! Find the fire extinguisher!");
		setParent(0, 0, -1);
		newEvent(0);
		setEventName(0, 1, "Put out all the fires");
		setImmediateChild(0, 1, 0);
		setLast(0, 1);

		// outage event
		createEventSet();
		setEventSetName(1, "Outage");
		newEvent(1);
		setEventName(1, 0, "The Gnomes Shut the Power Off! Get to the bunker!");
		setParent(1, 0, -1);
		newEvent(1);
		setEventName(1, 1, "Find the generator");
		setImmediateChild(1, 1, 0);
		newEvent(1);
		setEventName(1, 2, "Turn on the generator");
		setImmediateChild(1, 2, 1);
		setLast(1, 2);

		// flooded event
		createEventSet();
		setEventSetName(2, "Flooded");
		newEvent(2);
		setEventName(2, 0, "The Gnomes Broke The Pump Generator In The Bunker! Find The Replacement!");
		setParent(2, 0, -1);
		newEvent(2);
		setEventName(2, 1, "Find the Old Generator in the Bunker");
		setImmediateChild(2, 1, 0);
		newEvent(2);
		setEventName(2, 2, "Stand by the Generator to Replace it");
		setImmediateChild(2, 2, 1);
		setLast(2, 2);
	}

	private void createAchievements() {
		addAchievement("Up and Coming");
		setAchievementDesc(0, "Complete the Tutorial");

		//if (!checkForAchieveList()) {
		//	for (int i = 0; i < getNumAchievements(); i++)
		//	{
		//		setAchievementStatus(i, false);
		//	}
		//}
	}

    public void SetEvent(Vector3 evt) {
        if (evt.z == 0) { setActiveEvent((int)evt.x, (int)evt.y, false); }
        else if (evt.z == 1) { setActiveEvent((int)evt.x, (int)evt.y, true); }
    }
}