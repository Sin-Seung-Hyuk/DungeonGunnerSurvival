using System.Collections.Generic;
using UnityEngine;

public static class AStar
{
    /// <summary>
    /// start end ���� ��θ� �����ϰ� �߰��մϴ�.
    /// ��ȯ�� �������� �̵� �ܰ��Դϴ�. ��θ� ã�� �� ������ null�� ��ȯ�մϴ�.
    /// </summary>
    public static Stack<Vector3> BuildPath(Room room, Vector3Int start, Vector3Int end)
    {
        start -= (Vector3Int)room.lowerBounds; // ��������
        end -= (Vector3Int)room.lowerBounds;   // ��������

        List<Node> openNodeList = new List<Node>();
        HashSet<Node> closedNodeHashSet = new HashSet<Node>();

        GridNodes gridNodes = new GridNodes(room.upperBounds.x - room.lowerBounds.x + 1,
            room.upperBounds.y - room.lowerBounds.y + 1);

        Node startNode = gridNodes.GetGridNode(start.x, start.y);
        Node targetNode = gridNodes.GetGridNode(end.x, end.y);

        Node endPathNode = FindShortestPath(startNode, targetNode, gridNodes, openNodeList, closedNodeHashSet, room);

        if (endPathNode != null)
        {
            return CreatePathStack(endPathNode, room);
        }

        return null;
    }

    /// <summary>
    /// �ִ� ��� ã�� - ��ΰ� �߰ߵǸ� �� Node�� ��ȯ�ϰ� �׷��� ������ null�� ��ȯ�մϴ�.
    /// </summary>
    private static Node FindShortestPath(Node startNode, Node targetNode, GridNodes gridNodes, List<Node> openNodeList, HashSet<Node> closedNodeHashSet, Room instantiatedRoom)
    {
        openNodeList.Add(startNode);

        while (openNodeList.Count > 0)
        {
            openNodeList.Sort(); // Node �����Լ� �����Ѵ�� ���ĵ�

            Node currentNode = openNodeList[0]; // ù ��� ��������
            openNodeList.RemoveAt(0); // ù ��� �����

            if (currentNode == targetNode) return currentNode; // ����

            closedNodeHashSet.Add(currentNode); // �湮�ߴ� ���� �ݱ�

            // ���� ����� �ֺ� ��� �˻��Ͽ� �湮����Ʈ�� ����
            EvaluateCurrentNodeNeighbours(currentNode, targetNode, gridNodes, openNodeList, closedNodeHashSet, instantiatedRoom);
        }

        return null;
    }


    /// <summary>
    ///  ������ ��� ���ÿ� �׾Ƽ� ���� ��ȯ�ϱ� (���ÿ��� ���Ҹ� ������ ��ΰ� ��)
    /// </summary>
    private static Stack<Vector3> CreatePathStack(Node targetNode, Room room)
    {
        Stack<Vector3> movementPathStack = new Stack<Vector3>();

        Node nextNode = targetNode;

        // cell �� �߰���
        Vector3 cellMidPoint = room.grid.cellSize * 0.5f;
        cellMidPoint.z = 0f;

        while (nextNode != null)
        {
            // �׸��� ��ǥ�� ���� ��ǥ�� ��ȯ
            Vector3 worldPosition = room.grid.CellToWorld(
                new Vector3Int(nextNode.gridPosition.x + room.lowerBounds.x, nextNode.gridPosition.y + room.lowerBounds.y, 0));

            // ���� ��ġ�� �׸��� �� �߾����� �����մϴ�.
            worldPosition += cellMidPoint;
            movementPathStack.Push(worldPosition);
            nextNode = nextNode.parentNode;
        }

        return movementPathStack;
    }

    /// <summary>
    /// �������� �̵��� �ֺ���� Ž���Ͽ� ����Ʈ�� �ֱ�
    /// </summary>
    private static void EvaluateCurrentNodeNeighbours(Node currentNode, Node targetNode, GridNodes gridNodes, List<Node> openNodeList, HashSet<Node> closedNodeHashSet, Room instantiatedRoom)
    {
        Vector2Int currentNodeGridPos = currentNode.gridPosition; // ���� ����� ��ǥ ��������

        Node validNode; // ���� ����� �ֺ� ���

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue;

                validNode = GetValidNode(currentNodeGridPos.x + i, currentNodeGridPos.y + j,
                    gridNodes, closedNodeHashSet, instantiatedRoom);

                if (validNode != null)
                {   // �ֺ� ����� G�� ��� (��������� G�� �Ÿ� ���Ѱ�)
                    int newG = currentNode.G + GetDistance(currentNode, validNode);

                    bool isValidNodeInOpenList = openNodeList.Contains(validNode);

                    if (newG < validNode.G || !isValidNodeInOpenList)
                    {   // G�� ��, �湮���� ������ �ش� ��带 ����Ʈ�� ����
                        validNode.G = newG;
                        validNode.H = GetDistance(validNode, targetNode);
                        validNode.parentNode = currentNode;

                        if (!isValidNodeInOpenList)
                            openNodeList.Add(validNode);
                    }
                }
            }
        }
    }

    private static int GetDistance(Node n1, Node n2)
    {
        int disX = Mathf.Abs(n1.gridPosition.x - n2.gridPosition.x);
        int disY = Mathf.Abs(n1.gridPosition.y - n2.gridPosition.y);

        if (disX > disY) return 14 * disY + 10 * (disX - disY);
        return 14 * disX + 10 * (disY - disX);
    }

    private static Node GetValidNode(int xPos, int yPos, GridNodes gridNodes, HashSet<Node> closedNodeHashSet, Room room)
    {   // �ֺ� ��尡 ��ȿ���� �˻� (x,y�� �ش� �ֺ������ ��ǥ)
        if (xPos >= room.upperBounds.x - room.lowerBounds.x ||
            xPos < 0 || yPos >= room.upperBounds.y - room.lowerBounds.y || yPos < 0)
        {   // �ֺ� ����� ��ǥ ��ȿ�� �˻�
            return null;
        }

        Node newNode = gridNodes.GetGridNode(xPos, yPos);

        // �ش� ��ǥ�� G�� �˻�
        int movementPenaltyForGridSpace = room.aStarMovementPenalty[xPos, yPos];

        if (movementPenaltyForGridSpace == 0 || closedNodeHashSet.Contains(newNode)) return null;
        else return newNode;
    }
}