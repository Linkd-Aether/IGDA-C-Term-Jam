using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPath : MonoBehaviour
{
    private Transform[] nodes;
    private int currentNode = 0;

    private void Start() {
        nodes = new Transform[transform.childCount];
        
        int childNum = 0;
        foreach (Transform child in transform) {
            nodes[childNum] = child;
            childNum++;
        }
    }

    private void OnDrawGizmos() {
        Start();
        for (int i = 0; i < nodes.Length; i++) {
            int next = (i + 1) % nodes.Length;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(nodes[next].position, nodes[i].position);
            Gizmos.color = Color.gray;
            Gizmos.DrawSphere(nodes[i].position, .2f);
        }
    }

    public Transform nextNode() {
        currentNode = (currentNode + 1) % nodes.Length;
        return nodes[currentNode];
    }
}
