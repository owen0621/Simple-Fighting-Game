using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;

public class BodySourceView : MonoBehaviour 
{
    public Material BoneMaterial;
    public GameObject BodySourceManager;
    public int user1;
    public int user2;
    
    private Dictionary<ulong, GameObject> _Bodies = new Dictionary<ulong, GameObject>();
    private BodySourceManager _BodyManager;
    
    private Dictionary<Kinect.JointType, Kinect.JointType> _BoneMap = new Dictionary<Kinect.JointType, Kinect.JointType>()
    {
        { Kinect.JointType.FootLeft, Kinect.JointType.AnkleLeft },
        { Kinect.JointType.AnkleLeft, Kinect.JointType.KneeLeft },
        { Kinect.JointType.KneeLeft, Kinect.JointType.HipLeft },
        { Kinect.JointType.HipLeft, Kinect.JointType.SpineBase },
        
        { Kinect.JointType.FootRight, Kinect.JointType.AnkleRight },
        { Kinect.JointType.AnkleRight, Kinect.JointType.KneeRight },
        { Kinect.JointType.KneeRight, Kinect.JointType.HipRight },
        { Kinect.JointType.HipRight, Kinect.JointType.SpineBase },
        
        { Kinect.JointType.HandTipLeft, Kinect.JointType.HandLeft },
        { Kinect.JointType.ThumbLeft, Kinect.JointType.HandLeft },
        { Kinect.JointType.HandLeft, Kinect.JointType.WristLeft },
        { Kinect.JointType.WristLeft, Kinect.JointType.ElbowLeft },
        { Kinect.JointType.ElbowLeft, Kinect.JointType.ShoulderLeft },
        { Kinect.JointType.ShoulderLeft, Kinect.JointType.SpineShoulder },
        
        { Kinect.JointType.HandTipRight, Kinect.JointType.HandRight },
        { Kinect.JointType.ThumbRight, Kinect.JointType.HandRight },
        { Kinect.JointType.HandRight, Kinect.JointType.WristRight },
        { Kinect.JointType.WristRight, Kinect.JointType.ElbowRight },
        { Kinect.JointType.ElbowRight, Kinect.JointType.ShoulderRight },
        { Kinect.JointType.ShoulderRight, Kinect.JointType.SpineShoulder },
        
        { Kinect.JointType.SpineBase, Kinect.JointType.SpineMid },
        { Kinect.JointType.SpineMid, Kinect.JointType.SpineShoulder },
        { Kinect.JointType.SpineShoulder, Kinect.JointType.Neck },
        { Kinect.JointType.Neck, Kinect.JointType.Head },
    };
    
    void Update () 
    {
        if (BodySourceManager == null)
        {
            return;
        }
        
        _BodyManager = BodySourceManager.GetComponent<BodySourceManager>();
        if (_BodyManager == null)
        {
            return;
        }
        
        Kinect.Body[] data = _BodyManager.GetData(); //the bodies
        if (data == null)
        {
            return;
        }

        
        
        List<ulong> trackedIds = new List<ulong>();
        foreach(var body in data)
        {
            if (body == null)
            {         
                continue;
            }
                
            if(body.IsTracked)
            {
                trackedIds.Add (body.TrackingId);
            }
        }

        // to check whether the player is enough or not

        // print("it has "+trackedIds.Count+" body");
        if(trackedIds.Count==0){
            // print("Nobody");
            user1=0;
            user2=0;
            // return;
        }else if(trackedIds.Count==1){
            user2=0;
            // print("Only one player");
            // return; 
        }
        
        List<ulong> knownIds = new List<ulong>(_Bodies.Keys);
        // print("list.count=="+knownIds.Count);
        
        // First delete untracked bodies
        foreach(ulong trackingId in knownIds)
        {
            // print("list.count=="+knownIds.Count);
            if(knownIds.Count==1){
                // print(knownIds[0]);
            }else if(knownIds.Count==2){
                // print(knownIds[0]);
                // print(knownIds[1]);
            }
            
            
            // print(knownIds[2]);
            // print("trackedIds="+trackingId+"\n");
            if(!trackedIds.Contains(trackingId))
            {
                Destroy(_Bodies[trackingId]);
                _Bodies.Remove(trackingId);
            }
            
        }

        int body_number=0;

        // print(knownIds);
        // print("data[0]="+data[0]);
        // Kinect.Joint source=data[0].Joints[Kinect.JointType.SpineBase];
        // print("point for :"+source.ToString());
        // Transform jointObj = _Bodies[data[0].TrackingId].transform.Find(Kinect.JointType.SpineBase.ToString());
        // // jointObj.localPosition = GetVector3FromJoint(source);
        // print("data:"+data[0].Joints[Kinect.JointType.SpineBase]);
        


        foreach(var body in data)
        {
            if (body == null)
            {
                continue;
            }
            
            if(body.IsTracked)
            {
                print("body_number="+body_number);
                if(!_Bodies.ContainsKey(body.TrackingId))
                {
                    _Bodies[body.TrackingId] = CreateBodyObject(body.TrackingId);
                }
                
                RefreshBodyObject(body, _Bodies[body.TrackingId],body_number);
                body_number++;
            }
        }
    }
    
    private GameObject CreateBodyObject(ulong id)
    {
        GameObject body = new GameObject("Body:" + id);
        
        for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        {
            GameObject jointObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            
            LineRenderer lr = jointObj.AddComponent<LineRenderer>();
            lr.SetVertexCount(2);
            lr.material = BoneMaterial;
            lr.SetWidth(0.05f, 0.05f);
            
            jointObj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            jointObj.name = jt.ToString();
            jointObj.transform.parent = body.transform;
        }
        
        return body;
    }
    
    private void RefreshBodyObject(Kinect.Body body, GameObject bodyObject,int body_number)
    {
        
        for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        {
            Kinect.Joint sourceJoint = body.Joints[jt];
            Kinect.Joint? targetJoint = null;
            
            if(_BoneMap.ContainsKey(jt))
            {
                targetJoint = body.Joints[_BoneMap[jt]];
            }
            
            Transform jointObj = bodyObject.transform.Find(jt.ToString());
            jointObj.localPosition = GetVector3FromJoint(sourceJoint);


            // for distance determin, maybe use the public variable

            // if(jt.ToString()=="SpineMid"){
            //     print("body number : "+body_number);
            //     if(jointObj.localPosition.z>=12&&jointObj.localPosition.z<22){
            //         print("right, "+jointObj.localPosition.z);
            //     }else if(jointObj.localPosition.z>=22&&jointObj.localPosition.z<35){
            //         print("left, "+jointObj.localPosition.z);
            //     }else if(jointObj.localPosition.z<12){
            //         print("too short, "+jointObj.localPosition.z);
            //     }else{
            //         print("too long, "+jointObj.localPosition.z);
            //     }
            // }

            if(body_number==0){
                if(jt.ToString()=="SpineMid"){
                    // print("body number : "+body_number);
                    if(jointObj.localPosition.z>=10&&jointObj.localPosition.z<15){
                        // print("close, "+jointObj.localPosition.z);
                        user1=2;
                    }else if(jointObj.localPosition.z>=15&&jointObj.localPosition.z<25){
                        // print("far, "+jointObj.localPosition.z);
                        user1=3;
                    }else if(jointObj.localPosition.z<10){
                        user1=1;
                        // print("too short, "+jointObj.localPosition.z);
                    }else{
                        user1=4;
                        // print("too long, "+jointObj.localPosition.z);
                    }
                }
            }

            if(body_number==1){
                if(jt.ToString()=="SpineMid"){
                    // print("body number : "+body_number);
                    if(jointObj.localPosition.z>=10&&jointObj.localPosition.z<15){
                        // print("close, "+jointObj.localPosition.z);
                        user2=2;
                    }else if(jointObj.localPosition.z>=15&&jointObj.localPosition.z<25){
                        // print("far, "+jointObj.localPosition.z);
                        user2=3;
                    }else if(jointObj.localPosition.z<10){
                        user2=1;
                        // print("too short, "+jointObj.localPosition.z);
                    }else{
                        user2=4;
                        // print("too long, "+jointObj.localPosition.z);
                    }
                }
            }
            

            
            // LineRenderer lr = jointObj.GetComponent<LineRenderer>();
            // if(targetJoint.HasValue)
            // {
            //     lr.SetPosition(0, jointObj.localPosition);
            //     lr.SetPosition(1, GetVector3FromJoint(targetJoint.Value));
            //     lr.SetColors(GetColorForState (sourceJoint.TrackingState), GetColorForState(targetJoint.Value.TrackingState));
            // }
            // else
            // {
            //     lr.enabled = false;
            // }
        }
    }
    
    private static Color GetColorForState(Kinect.TrackingState state)
    {
        switch (state)
        {
        case Kinect.TrackingState.Tracked:
            return Color.green;

        case Kinect.TrackingState.Inferred:
            return Color.red;

        default:
            return Color.black;
        }
    }
    
    private static Vector3 GetVector3FromJoint(Kinect.Joint joint)
    {
        return new Vector3(joint.Position.X * 10, joint.Position.Y * 10, joint.Position.Z * 10);
    }
}
