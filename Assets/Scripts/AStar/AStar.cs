using System.Collections.Generic;
using UnityEngine;

public static class AStar
{
    /// <summary>
    /// start end 방의 경로를 구축하고 추가합니다.
    /// 반환된 스택으로 이동 단계입니다. 경로를 찾을 수 없으면 null을 반환합니다.
    /// </summary>
    public static Stack<Vector3> BuildPath(Room room, Vector3Int start, Vector3Int end)
    {
        start -= (Vector3Int)room.lowerBounds; // 시작지점
        end -= (Vector3Int)room.lowerBounds;   // 도착지점

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
    /// 최단 경로 찾기 - 경로가 발견되면 끝 Node를 반환하고 그렇지 않으면 null을 반환합니다.
    /// </summary>
    private static Node FindShortestPath(Node startNode, Node targetNode, GridNodes gridNodes, List<Node> openNodeList, HashSet<Node> closedNodeHashSet, Room instantiatedRoom)
    {
        openNodeList.Add(startNode);

        while (openNodeList.Count > 0)
        {
            openNodeList.Sort(); // Node 정렬함수 구현한대로 정렬됨

            Node currentNode = openNodeList[0]; // 첫 노드 가져오기
            openNodeList.RemoveAt(0); // 첫 노드 지우기

            if (currentNode == targetNode) return currentNode; // 도착

            closedNodeHashSet.Add(currentNode); // 방문했던 지역 닫기

            // 현재 노드의 주변 노드 검사하여 방문리스트에 삽입
            EvaluateCurrentNodeNeighbours(currentNode, targetNode, gridNodes, openNodeList, closedNodeHashSet, instantiatedRoom);
        }

        return null;
    }


    /// <summary>
    ///  지나온 경로 스택에 쌓아서 스택 반환하기 (스택에서 원소를 꺼내면 경로가 됨)
    /// </summary>
    private static Stack<Vector3> CreatePathStack(Node targetNode, Room room)
    {
        Stack<Vector3> movementPathStack = new Stack<Vector3>();

        Node nextNode = targetNode;

        // cell 의 중간점
        Vector3 cellMidPoint = room.grid.cellSize * 0.5f;
        cellMidPoint.z = 0f;

        while (nextNode != null)
        {
            // 그리드 좌표를 월드 좌표로 변환
            Vector3 worldPosition = room.grid.CellToWorld(
                new Vector3Int(nextNode.gridPosition.x + room.lowerBounds.x, nextNode.gridPosition.y + room.lowerBounds.y, 0));

            // 월드 위치를 그리드 셀 중앙으로 설정합니다.
            worldPosition += cellMidPoint;
            movementPathStack.Push(worldPosition);
            nextNode = nextNode.parentNode;
        }

        return movementPathStack;
    }

    /// <summary>
    /// 다음으로 이동할 주변노드 탐색하여 리스트에 넣기
    /// </summary>
    private static void EvaluateCurrentNodeNeighbours(Node currentNode, Node targetNode, GridNodes gridNodes, List<Node> openNodeList, HashSet<Node> closedNodeHashSet, Room instantiatedRoom)
    {
        Vector2Int currentNodeGridPos = currentNode.gridPosition; // 현재 노드의 좌표 가져오기

        Node validNode; // 현재 노드의 주변 노드

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue;

                validNode = GetValidNode(currentNodeGridPos.x + i, currentNodeGridPos.y + j,
                    gridNodes, closedNodeHashSet, instantiatedRoom);

                if (validNode != null)
                {   // 주변 노드의 G값 계산 (이전노드의 G에 거리 더한값)
                    int newG = currentNode.G + GetDistance(currentNode, validNode);

                    bool isValidNodeInOpenList = openNodeList.Contains(validNode);

                    if (newG < validNode.G || !isValidNodeInOpenList)
                    {   // G값 비교, 방문한적 없으면 해당 노드를 리스트에 삽입
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
    {   // 주변 노드가 유효한지 검사 (x,y는 해당 주변노드의 좌표)
        if (xPos >= room.upperBounds.x - room.lowerBounds.x ||
            xPos < 0 || yPos >= room.upperBounds.y - room.lowerBounds.y || yPos < 0)
        {   // 주변 노드의 좌표 유효성 검사
            return null;
        }

        Node newNode = gridNodes.GetGridNode(xPos, yPos);

        // 해당 좌표의 G값 검사
        int movementPenaltyForGridSpace = room.aStarMovementPenalty[xPos, yPos];

        if (movementPenaltyForGridSpace == 0 || closedNodeHashSet.Contains(newNode)) return null;
        else return newNode;
    }
}