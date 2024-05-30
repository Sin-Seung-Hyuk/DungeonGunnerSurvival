using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingCtrl : MonoBehaviour
{
    private Volume postProcessing;
    private Vignette vignette;
    private Player player;
    private bool isVignetteRoutine;
    private Coroutine vignetteRoutineInstance;

    private float targetValue = 0.5f; 
    private bool increasing = true;
    float waitTime = 0.5f; // 0.3f에서 0.4f로 변화하는 데 걸리는 시간

    private void Awake()
    {
        postProcessing = GetComponent<Volume>(); // 포스트프로세싱 볼륨 얻기

        // profile.TryGet<T>(out t) -> 포스트프로세싱 속성 얻어오기
        postProcessing.profile.TryGet<Vignette>(out vignette);

        isVignetteRoutine = false;
    }

    private void Start()
    {
        player = GameManager.Instance.GetPlayer();

        vignette.active = false;
    }

    private void Update()
    {
        // 플레이어 체력이 30% 이하 && 코루틴 한번만 실행
        if (player.health.GetCurrentHealthRatio() < 0.3f && !isVignetteRoutine)
        {
            isVignetteRoutine = true;
            vignette.active = true;
            vignetteRoutineInstance = StartCoroutine(vignetteRoutine());
        }

        else if (player.health.GetCurrentHealthRatio() >= 0.3f)
        {
            if (vignetteRoutineInstance != null)
            {
                StopCoroutine(vignetteRoutineInstance);
                vignetteRoutineInstance = null;
            }
            vignette.active = false;
            isVignetteRoutine = false;
        }
    }

    private IEnumerator vignetteRoutine()
    {
        float elapsedTime = 0.0f;
        float startValue = 0.4f;
        targetValue = 0.5f;
        increasing = true;

        while (true)
        {
            // vignette 크기의 시작값 (0.3 or 0.4)
            startValue = vignette.intensity.value;

            while (elapsedTime < waitTime)
            {
                // vignette의 크기 값을 Lerp로 보간 (시작값, 도착값, 누적시간 / 목표시간)
                vignette.intensity.value = Mathf.Lerp(startValue, targetValue, elapsedTime / waitTime);
                elapsedTime += Time.deltaTime;

                yield return null;
            }

            // 목표 값을 전환 (0.3~0.4 or 0.4~0.3)
            increasing = !increasing;
            targetValue = increasing ? 0.4f : 0.3f;
            elapsedTime = 0.0f;
        }
    }
}

//private IEnumerator vignetteRoutine()
//{
//    vignette.active = true;
//    vignette.intensity.value = 0.3f;

//    while (lightFlickerTimer < 0.2f)
//    {
//        lightFlickerTimer += Time.deltaTime;

//        vignette.intensity.value = Mathf.Lerp(0.3f, 0.0f, lightFlickerTimer / 0.2f);

//        yield return null;
//    }

//    vignette.intensity.value = 0f;
//    lightFlickerTimer = 0f;
//    vignette.active = false;
//}
