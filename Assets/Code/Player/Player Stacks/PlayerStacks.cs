using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStacks : MonoBehaviour
{
    private Dictionary<Stack, int> stacks = new Dictionary<Stack, int>();
    private Dictionary<TimeDepletableStack, float> stacksCooldown = new Dictionary<TimeDepletableStack, float>();
    private List<PersistentStack> persistentStacks;

    public static PlayerStacks Instance;
    public TextMeshProUGUI textField;

    [SerializeField]
    private List<Contract> testContracts;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    void Start()
    {
        EventManager.StartListening(Event.StackAdded, AddStack);
    }

    private void OnDestroy()
    {
        EventManager.StopListening(Event.StackAdded, AddStack);
        foreach(var kvp in stacks)
        {
            if(kvp.Key is EventDepletableStack)
            {
                EventDepletableStack eds = kvp.Key as EventDepletableStack;
                eds.RemoveEventListeners();
            }
        }
    }

    void AddStack(IEventPacket packet)
    {
        StackAddedPacket sap = packet as StackAddedPacket;
        if(sap != null)
        {
            sap.stackToAdd.OnAdd();
            if (stacks.ContainsKey(sap.stackToAdd))
            {
                stacks[sap.stackToAdd] += sap.stackNumber;
                if(sap.stackToAdd is TimeDepletableStack)
                    AddDepletableStack(sap.stackToAdd);

            }
            else
            {
                stacks.Add(sap.stackToAdd, sap.stackNumber);
                if (sap.stackToAdd is TimeDepletableStack)
                    AddDepletableStack(sap.stackToAdd);
            }
        }
    }

    void AddDepletableStack(Stack stack)
    {
        if (!(stack is TimeDepletableStack))
            return;
        TimeDepletableStack ds = (TimeDepletableStack)stack;
        if(stacksCooldown.ContainsKey(ds))
        {
            stacksCooldown[ds] = ds.timeToDeplete;
        }
        else
        {
            stacksCooldown.Add(ds, ds.timeToDeplete);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            foreach(var contract in testContracts)
                foreach(var stacks in contract.stacksToAdd)
                {
                    EventManager.TriggerEvent(Event.StackAdded, new StackAddedPacket()
                    {
                        stackNumber = stacks.stacksToAdd,
                        stackToAdd = stacks.stack,
                        currentRoom = LevelGeneration.Instance.roomIndex
                    });
                }
        }
        DepleteTimeStacks();
        DisplayStacks();
    }

    public bool RemoveStack(Stack stack)
    {
        if(stacks.ContainsKey(stack))
        {
            stacks[stack]--;
            if (stacks[stack] == 0)
            {
                stacks.Remove(stack);
                return true;
            }
            else
                return false;
        }
        return false;
    }

    void DepleteTimeStacks()
    {
        List<TimeDepletableStack> keys = new List<TimeDepletableStack>(stacksCooldown.Keys);

        foreach(var key in keys)
        {
            //deplete every depletable stack by a frame counter;
            stacksCooldown[key] -= Time.deltaTime;
            if (stacksCooldown[key] <= 0)
            {
                //if timer reached 0

                //lower the stack number by 1
                stacks[key]--;
                stacksCooldown[key] = key.timeToDeplete;
                //call the stack's ondeplete
                key.OnDeplete();
                //if there are no more stacks
                if (stacks[key] == 0)
                {
                    //remove them from everywhere
                    stacks.Remove(key);
                    stacksCooldown.Remove(key);
                    key.OnFinalDeplete();
                }
            }
        }
    }

    public bool ContainsStack(Stack stack)
    {
        return stacks.ContainsKey(stack);
    }
    void DisplayStacks()
    {
        string text = "";
        foreach(var kvp in stacks)
        {
            text += $"{kvp.Key.GetType()}, {kvp.Value}\n";
        }
        textField.text = text;
    }
}
