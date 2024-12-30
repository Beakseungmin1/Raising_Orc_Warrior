using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class SoundManager : SingletonDontDestroy<SoundManager>
{
    public AudioSource sfxSource;       //sfx전용 오디오 변수
    public AudioSource bgmSource;       //bgm전용 오디오 변수


    //소리 저장해놓는 Dictionary
    private Dictionary<Enum, AudioClip> soundDictionary = new Dictionary<Enum, AudioClip>();

    /// <summary>
    /// SFX 재생 함수
    /// </summary>
    public void PlaySFX(SFXType type)
    {
        //한번만 재생.(다른 음악이 실행중이여도 실행 가능)
        //play = 다른 음악이 실행중이면 종료 후 실행

        //loop가 false일 경우
        //play는 기존에 음악이 재생중이면 해당 음악을 끄고 진행
        //playoneshot는 기존에 음악이 재생중이여도 겹쳐서 진행
        
        //loop가 true일 경우
        //play는 음악을 끄고 재생 후 음악이 끝나면 다시 실행
        //playoneshot는 마찬가지로 한번만 실행하는데
        //재생중인 음악이 있어도 실행
        sfxSource.PlayOneShot(soundDictionary[type]);
    }

    /// <summary>
    /// BGM 재생 함수
    /// </summary>
    public void PlayBGM(BGMType type)
    {
        //현재 bgmSource에 설정된 배경 음악이 재생하려는 배경 음악과 같은지 확인하는 코드
        //이미 같은 음악이 재생 중이라면 중복해서 재생하지 않도록 방지
        if (bgmSource.clip == soundDictionary[type])
            return;
        //dictionary에 있는 bgmtype이랑 bgmsource.clip 이름이 같다면
        bgmSource.clip = soundDictionary[type];

        //계속 재생해라.
        bgmSource.Play();

        //이 코드의 목적은 배경 음악의 중복 재생을 방지하고,
        //요청된 배경 음악이 다를 경우에만 새로운 배경 음악을 설정하고 재생
    }

    public void MuteBGM()
    {
        if (bgmSource.mute)
        {
            bgmSource.mute = false;
        }
        else
        {
            bgmSource.mute = true;
        }
    }

    public void MuteSFX()
    {
        if (sfxSource.mute)
        {
            sfxSource.mute = false;
        }
        else
        {
            sfxSource.mute = true;
        }
    }







    public void SetVolume(string name, float volume)
    {
        if (name == "BGM")
        {
            bgmSource.volume = volume;
            PlayerPrefs.SetFloat("BGM", volume);
        }
        else if (name == "SFX")
        {
            sfxSource.volume = volume;
            PlayerPrefs.SetFloat("SFX", volume);
        }
    }

    /// <summary>
    /// Scene 로드 시 자동으로 BGM 호출해주는 함수
    /// </summary>
    public void OnLoadCompleted(Scene scene, LoadSceneMode loadSceneMode)
    {
        switch (scene.name)
        {
            case "Title":
                //bgmSource가 비어있거나 bgmsource가 enum BGMType의 Title과 다르다면(다른 노래가 재생되고 있다면)
                if (bgmSource.clip == null || bgmSource.clip != soundDictionary[BGMType.Title])
                {
                    //Title enumBGMType로 바꿔라
                    bgmSource.clip = soundDictionary[BGMType.Title];
                    //반복재생
                    bgmSource.Play();
                }
                break;


            case "Town":
                //위와 동일
                if (bgmSource.clip == null || bgmSource.clip != soundDictionary[BGMType.Town])
                {
                    bgmSource.clip = soundDictionary[BGMType.Town];
                    bgmSource.Play();
                }
                break;


            case "Stage1":
                //위와 동일
                if (bgmSource.clip == null || bgmSource.clip != soundDictionary[BGMType.stageTo2])
                {
                    bgmSource.clip = soundDictionary[BGMType.stageTo2];
                    bgmSource.Play();
                }
                break;


            case "Stage2":
                //위와 동일
                if (bgmSource.clip == null || bgmSource.clip != soundDictionary[BGMType.stageTo2])
                {
                    bgmSource.clip = soundDictionary[BGMType.stageTo2];
                    bgmSource.Play();
                }
                break;

                
            case "Stage3":
                //위와 동일
                if (bgmSource.clip == null || bgmSource.clip != soundDictionary[BGMType.stage3])
                {
                    bgmSource.clip = soundDictionary[BGMType.stage3];
                    bgmSource.Play();
                }
                break;
        }
    }
    /// <summary>
    /// SoundManager 생성 함수
    /// </summary>
    public void Init()
    {
        //새로운 빈 게임오브젝트 2개를 SFX와 BGM이라는 이름으로 생성.
        GameObject sfxGameObject = new GameObject("SFX");
        GameObject bgmGameObject = new GameObject("BGM");

        //두 개의 새로운 게임 오브젝트(sfxGameObject와 bgmGameObject)를 생성한 후,
        //이 두 오브젝트의 부모를 현재 SoundManager 객체로 설정
        sfxGameObject.transform.SetParent(gameObject.transform);
        bgmGameObject.transform.SetParent(gameObject.transform);

        //각 게임 오브젝트들에 AudioSource 컴포넌트를 달아줌.
        sfxSource = sfxGameObject.AddComponent<AudioSource>();
        bgmSource = bgmGameObject.AddComponent<AudioSource>();

        //Resources 폴더 안의 Sounds폴더의 SFX AudioClip을 모두 로드.
        var sfxClipArr = Resources.LoadAll<AudioClip>("Sounds/SFX");
        ClipLoader<SFXType>(sfxClipArr);

        //Resources 폴더 안의 Sounds폴더의 BGM AudioClip을 모두 로드.
        var bgmClipArr = Resources.LoadAll<AudioClip>("Sounds/BGM");
        ClipLoader<BGMType>(bgmClipArr);


        //게임 오브젝트가 활성화될 때 오디오가 자동으로 재생되지 않게함(False)
        bgmSource.playOnAwake = false;
        //반복 재생할건지 여부(기본은 false)
        //loop가 false일 경우 play()를 실행하면 한번만 재생.
        //true일 경우 반복 재생.
        bgmSource.loop = true;
        //시작 BGM 볼륨 설정
        bgmSource.volume = 0.3f;

        //게임 오브젝트가 활성화될 때 오디오가 자동으로 재생되지 않게함(False)
        sfxSource.playOnAwake = false;
        //시작 SFX 볼륨 설정
        sfxSource.volume = 1f;

        //BGM과 SFX의 소리를 최대 1로 설정.
        bgmSource.volume = PlayerPrefs.GetFloat("BGM", 1);
        sfxSource.volume = PlayerPrefs.GetFloat("SFX", 1);

        //Scene이 전환되고 load가 끝난 후 타이틀의 이름에 따라 다른 노래를 실행.
        //SceneManagerEx.OnLoadCompleted(OnLoadCompleted);
    }

    /// <summary>
    /// 클립 읽어주는 함수
    /// </summary>
    public void ClipLoader<T>(AudioClip[] arr) where T : Enum
    {
        for (int i = 0; i < arr.Length; i++)
        {
            //Enum.Parse 메서드가 내부적으로 예외를 발생시키는 메서드이기
            //때문에 만약 변환이 실패하면 FormatException 또는 ArgumentException을
            //던지기 때문에, 이를 처리하기 위해 try-catch를 사용
            try
            {
                T type = (T)Enum.Parse(typeof(T), arr[i].name);
                soundDictionary.Add(type, arr[i]);
            }
            //Enum.Parse가 실패할 경우(즉, arr[i].name이 Enum에 없는 값일 경우), 경고 로그를 출력
            catch
            {
                Debug.LogWarning("Need Enum : " + arr[i].name);
            }
        }
    }
}
