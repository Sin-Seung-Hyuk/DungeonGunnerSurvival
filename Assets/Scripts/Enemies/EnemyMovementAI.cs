
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMovementAI : MonoBehaviour
{
    private Enemy enemy;
    private Stack<Vector3> movementSteps = new Stack<Vector3>(); // �̵� ���(A*���� �޾ƿ�)
    private Vector3 playerRefPos; // �÷��̾� ������ (Ÿ������)
    private Coroutine moveEnemyRoutine;
    private float currentEnemyPathRebuildCooldown; // ��� �缳�� ��Ÿ��
    private WaitForFixedUpdate waitForFixedUpdate;
    private List<Vector2Int> surroundPosList = new List<Vector2Int>();

    [HideInInspector] public float moveSpeed; //movementDetail���� �޾ƿ� �̼�
    [HideInInspector] public float chaseDistance; //movementDetail���� �޾ƿ� �÷��̾���� ���� (�÷��̾ ������ �Ѿƿ���)

    [HideInInspector] public int updateFrameNumber = 1; // ������Ʈ ������
    [HideInInspector] public int targetFrameRateToSpreadPathFindingOver = 1; // ������Ʈ ������

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }
    private void Start()
    {
        waitForFixedUpdate = new WaitForFixedUpdate();

        // �� ������ �������� ��� �籸�� �� ����. �������� ������ �� ��ġ�� �����Ӹ��� ����籸�� ����
        targetFrameRateToSpreadPathFindingOver = Random.Range(40, 80);

        // �÷��̾� ������ �޾ƿ���
        playerRefPos = GameManager.Instance.GetPlayerPosition();
    }
    private void Update()
    {
        MovementEnemy();
    }

    private void MovementEnemy()
    {
        // �̵� ��ٿ� Ÿ�̸�
        currentEnemyPathRebuildCooldown -= Time.deltaTime;

        // ���ϸ� �л��Ű�� ���� Ư�� �����ӿ����� ��θ� �籸��
        if (Time.frameCount % targetFrameRateToSpreadPathFindingOver != updateFrameNumber) return;

        if (currentEnemyPathRebuildCooldown <= 0f || (Vector3.Distance(playerRefPos, GameManager.Instance.GetPlayerPosition()) >
            Settings.playerMoveDistanceToRebuildPath))
        {   // �����Ž�� Ÿ�̸�,�÷��̾���� �Ÿ� ��� �����ϸ� ���Ž�� ����
            currentEnemyPathRebuildCooldown = Settings.enemyPathRebuildCooldown;
            playerRefPos = GameManager.Instance.GetPlayerPosition();

            CreatePath(); // A* �̿��Ͽ� ��� Ž��

            if (movementSteps != null)
            {
                if (moveEnemyRoutine != null)
                {
                    enemy.idleEvent.CallIdle();
                    StopCoroutine(moveEnemyRoutine);
                }

                moveEnemyRoutine = StartCoroutine(MoveEnemyRoutine(movementSteps));
            }
        }
    }

    public void SetUpdateFrameNumber(int updateFrameNumber)
    {
        this.updateFrameNumber = updateFrameNumber;
    }

    private IEnumerator MoveEnemyRoutine(Stack<Vector3> movementSteps)
    {
        while (movementSteps.Count > 0)
        {
            Vector3 nextPos = movementSteps.Pop();

            while (Vector3.Distance(nextPos, transform.position) > chaseDistance) // chaseDistance������ �Ѿƿ�
            {
                // 1.������ġ 2.������ġ 3.�ӵ� 4.�̵����⺤��
                enemy.movementToPositionEvent.CallMovementToPositionEvent(
                    nextPos, transform.position, moveSpeed, (nextPos - transform.position).normalized);

                yield return waitForFixedUpdate;
            }

            yield return waitForFixedUpdate; // ���������̹Ƿ� fixedUpdate ���� ����
        }

        enemy.idleEvent.CallIdle();
    }

    private void CreatePath() // A* ����Ͽ� ��� �����
    {
        Room currentRoom = GameManager.Instance.GetCurrentRoom(); // ���� �����Ǿ� �ִ� �� ��������
        Grid grid = currentRoom.grid; // ���� ���� Grid ������Ʈ �������� 

        // �׸��忡�� �÷��̾� ������ ��� (�÷��̾� ��ġ�� ������ �� ã��)
        Vector3Int playerGridPos = GetNearPlayerPos(currentRoom);

        // �׸��忡�� �� ������ ���
        Vector3Int enemyGridPos = grid.WorldToCell(transform.position);

        // enemyGridPos : ��������, playerGridPos : ��������
        movementSteps = AStar.BuildPath(currentRoom, enemyGridPos, playerGridPos);

        if (movementSteps != null) movementSteps.Pop();
        else enemy.idleEvent.CallIdle();
    }

    private Vector3Int GetNearPlayerPos(Room currentRoom)
    {   
        // �÷��̾ ��ֹ��ʿ� ������ �� ��ó�� ��ġ�� �̵��ؾ���
        Vector3Int playerCellPos = GameManager.Instance.GetPlayerCellPosition();

        // ���� ���� �������� �÷��̾� ������ ���ϱ� (�ܼ� ���������� �ƴ� �� ���� ��ǥ)
        Vector2Int adjustPlayerCellPos = new Vector2Int(playerCellPos.x - currentRoom.lowerBounds.x,
            playerCellPos.y - currentRoom.lowerBounds.y);

        // �ʿ��� �÷��̾� ��ġ�� G�� ���ϱ� (�÷��̾ ��ֹ� ��ó�� �ִ��� Ȯ��)
        int obstacle = currentRoom.aStarMovementPenalty[adjustPlayerCellPos.x, adjustPlayerCellPos.y];

        if (obstacle != 0) return playerCellPos; // ���� �÷��̾ ��ֹ��� ��ġ���� ����
        else {
            surroundPosList.Clear(); // ��ֹ��� ��ġ�� �÷��̾��� ��ó ��ǥ

            for (int i = -1; i <= 1; i++) {
                for (int j = -1; j <= 1; j++) {
                    if (j == 0 && i == 0) continue;

                    surroundPosList.Add(new Vector2Int(i, j)); // 8���� �˻��ϱ�
                }
            }

            for (int i = 0; i < 8; i++) {
                obstacle = currentRoom.aStarMovementPenalty[adjustPlayerCellPos.x +
                    surroundPosList[i].x, adjustPlayerCellPos.y + surroundPosList[i].y];

                if (obstacle != 0) { // ��ֹ��� �ƴ� ��ǥ �߰�
                    return new Vector3Int(playerCellPos.x + surroundPosList[i].x, playerCellPos.y + surroundPosList[i].y, 0);
                }

                surroundPosList.RemoveAt(i);
            }

            // �߰����� ���ߴٸ� �������� �̵�
            return (Vector3Int)currentRoom.spawnPositionArray[Random.Range(0, currentRoom.spawnPositionArray.Count)];
        }
    }
}
