
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMovementAI : MonoBehaviour
{
    private Enemy enemy;
    private Stack<Vector3> movementSteps = new Stack<Vector3>(); // 이동 경로(A*에서 받아옴)
    private Vector3 playerRefPos; // 플레이어 포지션 (타겟지점)
    private Coroutine moveEnemyRoutine;
    private float currentEnemyPathRebuildCooldown; // 경로 재설정 쿨타임
    private WaitForFixedUpdate waitForFixedUpdate;
    private List<Vector2Int> surroundPosList = new List<Vector2Int>();

    [HideInInspector] public float moveSpeed; //movementDetail에서 받아온 이속
    [HideInInspector] public float chaseDistance; //movementDetail에서 받아온 플레이어와의 간격 (플레이어를 어디까지 쫓아올지)

    [HideInInspector] public int updateFrameNumber = 1; // 업데이트 프레임
    [HideInInspector] public int targetFrameRateToSpreadPathFindingOver = 1; // 업데이트 프레임

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }
    private void Start()
    {
        waitForFixedUpdate = new WaitForFixedUpdate();

        // 각 적마다 랜덤으로 경로 재구축 텀 설정. 랜덤으로 결정된 이 수치의 프레임마다 경로재구축 실행
        targetFrameRateToSpreadPathFindingOver = Random.Range(40, 80);

        // 플레이어 포지션 받아오기
        playerRefPos = GameManager.Instance.GetPlayerPosition();
    }
    private void Update()
    {
        MovementEnemy();
    }

    private void MovementEnemy()
    {
        // 이동 쿨다운 타이머
        currentEnemyPathRebuildCooldown -= Time.deltaTime;

        // 부하를 분산시키기 위해 특정 프레임에서만 경로를 재구축
        if (Time.frameCount % targetFrameRateToSpreadPathFindingOver != updateFrameNumber) return;

        if (currentEnemyPathRebuildCooldown <= 0f || (Vector3.Distance(playerRefPos, GameManager.Instance.GetPlayerPosition()) >
            Settings.playerMoveDistanceToRebuildPath))
        {   // 경로재탐색 타이머,플레이어와의 거리 모두 충족하면 경로탐색 시작
            currentEnemyPathRebuildCooldown = Settings.enemyPathRebuildCooldown;
            playerRefPos = GameManager.Instance.GetPlayerPosition();

            CreatePath(); // A* 이용하여 경로 탐색

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

            while (Vector3.Distance(nextPos, transform.position) > chaseDistance) // chaseDistance까지만 쫓아옴
            {
                // 1.도착위치 2.현재위치 3.속도 4.이동방향벡터
                enemy.movementToPositionEvent.CallMovementToPositionEvent(
                    nextPos, transform.position, moveSpeed, (nextPos - transform.position).normalized);

                yield return waitForFixedUpdate;
            }

            yield return waitForFixedUpdate; // 물리연산이므로 fixedUpdate 마다 수행
        }

        enemy.idleEvent.CallIdle();
    }

    private void CreatePath() // A* 사용하여 경로 만들기
    {
        Room currentRoom = GameManager.Instance.GetCurrentRoom(); // 현재 생성되어 있는 방 가져오기
        Grid grid = currentRoom.grid; // 현재 방의 Grid 컴포넌트 가져오기 

        // 그리드에서 플레이어 포지션 얻기 (플레이어 위치에 적합한 곳 찾기)
        Vector3Int playerGridPos = GetNearPlayerPos(currentRoom);

        // 그리드에서 적 포지션 얻기
        Vector3Int enemyGridPos = grid.WorldToCell(transform.position);

        // enemyGridPos : 시작지점, playerGridPos : 도착지점
        movementSteps = AStar.BuildPath(currentRoom, enemyGridPos, playerGridPos);

        if (movementSteps != null) movementSteps.Pop();
        else enemy.idleEvent.CallIdle();
    }

    private Vector3Int GetNearPlayerPos(Room currentRoom)
    {   
        // 플레이어가 장애물쪽에 있으면 그 근처의 위치로 이동해야함
        Vector3Int playerCellPos = GameManager.Instance.GetPlayerCellPosition();

        // 현재 맵을 기준으로 플레이어 포지션 구하기 (단순 셀포지션이 아닌 맵 위의 좌표)
        Vector2Int adjustPlayerCellPos = new Vector2Int(playerCellPos.x - currentRoom.lowerBounds.x,
            playerCellPos.y - currentRoom.lowerBounds.y);

        // 맵에서 플레이어 위치의 G값 구하기 (플레이어가 장애물 근처에 있는지 확인)
        int obstacle = currentRoom.aStarMovementPenalty[adjustPlayerCellPos.x, adjustPlayerCellPos.y];

        if (obstacle != 0) return playerCellPos; // 현재 플레이어가 장애물에 위치하지 않음
        else {
            surroundPosList.Clear(); // 장애물에 위치한 플레이어의 근처 좌표

            for (int i = -1; i <= 1; i++) {
                for (int j = -1; j <= 1; j++) {
                    if (j == 0 && i == 0) continue;

                    surroundPosList.Add(new Vector2Int(i, j)); // 8방향 검사하기
                }
            }

            for (int i = 0; i < 8; i++) {
                obstacle = currentRoom.aStarMovementPenalty[adjustPlayerCellPos.x +
                    surroundPosList[i].x, adjustPlayerCellPos.y + surroundPosList[i].y];

                if (obstacle != 0) { // 장애물이 아닌 좌표 발견
                    return new Vector3Int(playerCellPos.x + surroundPosList[i].x, playerCellPos.y + surroundPosList[i].y, 0);
                }

                surroundPosList.RemoveAt(i);
            }

            // 발견하지 못했다면 랜덤으로 이동
            return (Vector3Int)currentRoom.spawnPositionArray[Random.Range(0, currentRoom.spawnPositionArray.Count)];
        }
    }
}
