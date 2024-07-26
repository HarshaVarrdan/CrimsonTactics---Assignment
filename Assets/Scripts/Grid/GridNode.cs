using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNode : MonoBehaviour
{
    public Vector3 Position { get; }
    public GridNode(Vector3 position)
    {
        Position = position;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        GridNode other = (GridNode)obj;
        return Position.Equals(other.Position);
    }

    public override int GetHashCode()
    {
        return Position.GetHashCode();
    }

    public static bool operator ==(GridNode node1, GridNode node2)
    {
        if (object.ReferenceEquals(node1, null))
            return object.ReferenceEquals(node2, null);

        return node1.Equals(node2);
    }

    public static bool operator !=(GridNode node1, GridNode node2)
    {
        return !(node1 == node2);
    }
}
