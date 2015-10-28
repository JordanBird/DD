using UnityEngine;
using System.Collections;

public class NotificationCenter : MonoBehaviour
{
    // We need a static method for objects to be able to obtain the default notification center.
    // This default center is what all objects will use for most notifications.  We can of course create our own separate instances of NotificationCenter, but this is the static one used by all.
    private static NotificationCenter defaultCenter;

    public static NotificationCenter DefaultCenter()
    {
        // If the defaultCenter doesn't already exist, we need to create it
        if (!defaultCenter)
        {
            // Because the NotificationCenter is a component, we have to create a GameObject to attach it to.
            GameObject notificationObject = new GameObject("Default Notification Center");
            // Add the NotificationCenter component, and set it as the defaultCenter
            defaultCenter = notificationObject.AddComponent<NotificationCenter>();
        }

        return defaultCenter;
    }

    // Our hashtable containing all the notifications.  Each notification in the hash table is an ArrayList that contains all the observers for that notification.
    Hashtable notifications = new Hashtable();

    // AddObserver includes a version where the observer can request to only receive notifications from a specific object.  We haven't implemented that yet, so the sender value is ignored for now.
    public void AddObserver(GameObject observer, string name) { AddObserver(observer, name, null); }
    public void AddObserver(GameObject observer, string name, GameObject sender)
    {
        // If the name isn't good, then throw an error and return.
        if (name == null || name == "") { Debug.Log("Null name specified for notification in AddObserver."); return; }
        // If this specific notification doens't exist yet, then create it.
        if (!notifications.ContainsKey(name))
        {
            notifications[name] = new ArrayList();
        }

        ArrayList notifyList = (ArrayList)notifications[name];

        // If the list of observers doesn't already contains the one that's registering, then add it.
        if (!notifyList.Contains(observer)) { notifyList.Add(observer); }
    }

    // RemoveObserver removes the observer from the notification list for the specified notification type
    public void RemoveObserver(GameObject observer, string name)
    {
        ArrayList notifyList = (ArrayList)notifications[name];

        // Assuming that this is a valid notification type, remove the observer from the list.
        // If the list of observers is now empty, then remove that notification type from the notifications hash.  This is for housekeeping purposes.
        if (notifyList.Count == 0)
        {
            if (notifyList.Contains(observer)) { notifyList.Remove(observer); }
            if (notifyList.Count == 0) { notifications.Remove(name); }
        }
    }

    // PostNotification sends a notification object to all objects that have requested to receive this type of notification.
    // A notification can either be posted with a notification object or by just sending the individual components.
    public void PostNotification(GameObject aSender, string aName) { PostNotification(aSender, aName, null); }
    public void PostNotification(GameObject aSender, string aName, string aData) { PostNotification(new Notification(aSender, aName, aData)); }
    public void PostNotification(Notification aNotification)
    {
        // First make sure that the name of the notification is valid.
        if (aNotification.name == null || aNotification.name == "") { Debug.Log("Null name sent to PostNotification."); return; }
        // Obtain the notification list, and make sure that it is valid as well
        ArrayList notifyList = (ArrayList)notifications[aNotification.name];
        if (notifyList == null) { Debug.Log("Notify list not found in PostNotification."); return; }

        // Create an array to keep track of invalid observers that we need to remove
        ArrayList observersToRemove = new ArrayList();
 
        // Itterate through all the objects that have signed up to be notified by this type of notification.
        foreach (GameObject observer in notifyList)
        {
            // If the observer isn't valid, then keep track of it so we can remove it later.
            // We can't remove it right now, or it will mess the for loop up.
            if (!observer)
            {
                observersToRemove.Add(observer);
            }
            else
            {
                // If the observer is valid, then send it the notification.  The message that's sent is the name of the notification.
                observer.SendMessage(aNotification.name, aNotification, SendMessageOptions.DontRequireReceiver);
            }
        }

        // Remove all the invalid observers
        foreach (GameObject observer in observersToRemove)
        {
            notifyList.Remove(observer);
        }
    }

    // The Notification class is the object that is send to receiving objects of a notification type.
    // This class contains the sending GameObject, the name of the notification, and optionally a hashtable containing data.
    public class Notification
    {
        public GameObject sender;
        public string name;
        public string data;

        public Notification(GameObject aSender, string aName) { sender = aSender; name = aName; data = null; }
        public Notification(GameObject aSender, string aName, string aData) { sender = aSender; name = aName; data = aData; }
    }
}
