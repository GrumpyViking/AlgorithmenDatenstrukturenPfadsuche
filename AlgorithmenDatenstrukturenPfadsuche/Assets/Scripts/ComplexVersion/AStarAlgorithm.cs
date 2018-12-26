using System.Collections.Generic;
using UnityEngine;

public class AStarAlgorithm : MonoBehaviour
{
    private Grid GridReference; 
    public Transform StartPosition; 
    public Transform TargetPosition; 
    public List<Node> OpenList = new List<Node>(); 
    public HashSet<Node> ClosedList = new HashSet<Node>();
    private void Awake() 
    {
        GridReference = GetComponent<Grid>(); 
    }

    void Update()
    {
        FindPath(StartPosition.position, TargetPosition.position);
    }

    private void FindPath(Vector3 a_StartPos, Vector3 a_TargetPos)
    {
        var StartNode = GridReference.NodeFromWorldPoint(a_StartPos); 
        var TargetNode = GridReference.NodeFromWorldPoint(a_TargetPos); 

         

        OpenList.Add(StartNode); 

        while (OpenList.Count > 0) 
        {
            var CurrentNode = OpenList[0]; 
            for (var i = 1; i < OpenList.Count; i++) 
                if (OpenList[i].fCost < CurrentNode.fCost ||
                    OpenList[i].fCost == CurrentNode.fCost && OpenList[i].hCost < CurrentNode.hCost
                ) 
                    CurrentNode = OpenList[i]; 
            OpenList.Remove(CurrentNode); 
            ClosedList.Add(CurrentNode);

            if (CurrentNode == TargetNode)
            {
                GetFinalPath(StartNode, TargetNode);
                break;
            }

            foreach (var NeighborNode in GridReference.GetNeighboringNodes(CurrentNode)){
                if (!NeighborNode.traversable || ClosedList.Contains(NeighborNode))
                {
                    continue;
                } 
                    
                
                var MoveCost = CurrentNode.gCost + GetManhattenDistance(CurrentNode, NeighborNode); 

                if (MoveCost < NeighborNode.gCost || !OpenList.Contains(NeighborNode)
                ) 
                {
                    NeighborNode.gCost = MoveCost; 
                    NeighborNode.hCost = GetManhattenDistance(NeighborNode, TargetNode); 
                    NeighborNode.parent = CurrentNode; 

                    if (!OpenList.Contains(NeighborNode)) 
                        OpenList.Add(NeighborNode); 
                }
            }
        }
        
    }


    private void GetFinalPath(Node a_StartingNode, Node a_EndNode)
    {
        var FinalPath = new List<Node>();
        var CurrentNode = a_EndNode; 

        while (CurrentNode != a_StartingNode
        ) 
        {
            FinalPath.Add(CurrentNode); 
            CurrentNode = CurrentNode.parent; 
        }

        FinalPath.Reverse(); 

        GridReference.path = FinalPath; 
    }

    private int GetManhattenDistance(Node a_nodeA, Node a_nodeB)
    {
        var ix = Mathf.Abs(a_nodeA.cordX - a_nodeB.cordX); 
        var iy = Mathf.Abs(a_nodeA.cordY - a_nodeB.cordY); 

        return ix + iy; 
    }
}