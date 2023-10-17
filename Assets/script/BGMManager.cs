using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    // Start is called before the first frame update

    private AudioClip AudioClipIntro;
    [SerializeField] private float volume;
    private   AudioSource _introAudioSource;
    private  AudioSource _loopAudioSource;
    private AudioSource _introAudioSourcetmp;
    private AudioSource _loopAudioSourcetmp;
    private bool _isPause;
    private static bool _isFadeIn;
    private  static bool _isFadeOut;
    private static bool _isChange;
    private static bool isLoad = false;// 自身がすでにロードされているかを判定するフラグ
    private double FadeInSecond;
    double FadeDeltaTime = 0;

    private bool IsPlaying
   => (_introAudioSource.isPlaying || _introAudioSource.time > 0)
     || (_loopAudioSource.isPlaying || _loopAudioSource.time > 0);

    public float time
  => _introAudioSource == null ? 0
    : _introAudioSource.time > 0 ? _introAudioSource.time
    : _loopAudioSource.time > 0 ? AudioClipIntro.length + _loopAudioSource.time
    : 0;

   
    private void Awake()
    {
        if (isLoad)
        { // すでにロードされていたら
            Destroy(this.gameObject); // 自分自身を破棄して終了
            return;
        }
        isLoad = true; // ロードされていなかったら、フラグをロード済みに設定する
        DontDestroyOnLoad(this.gameObject);
    }

    public bool CheckIntroNull()
    {
        return _introAudioSource == null;
    }

    public bool CheckLoopNull()
    {
        return _loopAudioSource == null;
    }
    public void SetBGM(AudioClip AudioClipIntro, AudioClip AudioClipLoop,float volume)
   {
        if(AudioClipIntro == null || AudioClipLoop == null)
        {

            Debug.Log("BGMが設定されてないよ");
            return;
        }
        if (_introAudioSource != null && _loopAudioSource != null)
            if ((AudioClipIntro == _introAudioSource.clip) && (AudioClipLoop == _loopAudioSource.clip)) return;
        // AudioSource を自身に追加
        if (_introAudioSource != null && _loopAudioSource != null)
        {
            if (_introAudioSource.isPlaying || _loopAudioSource.isPlaying)
            {
                Stop();
                Destroy(GetComponent<AudioSource>());
                Destroy(GetComponent<AudioSource>());
            }
        }
        
        _introAudioSource = gameObject.AddComponent<AudioSource>();
        _loopAudioSource = gameObject.AddComponent<AudioSource>();
      
        _introAudioSource.clip = AudioClipIntro;
        _introAudioSource.loop = false;
        _introAudioSource.playOnAwake = false;
        _loopAudioSource.clip = AudioClipLoop;
        _loopAudioSource.loop = true;
        _loopAudioSource.playOnAwake = false;
        _introAudioSource.volume = 0;
        _loopAudioSource.volume = 0;
        this.volume = volume;
        this.AudioClipIntro = AudioClipIntro;
        FadeInSecond = 1.0;
        Play();



   }


    public void ChangeBGM(AudioClip AudioClipIntro, AudioClip AudioClipLoop, float volume)
    {
        _introAudioSourcetmp = gameObject.AddComponent<AudioSource>();
        _loopAudioSourcetmp = gameObject.AddComponent<AudioSource>();
        _introAudioSourcetmp.clip = AudioClipIntro;
        _introAudioSourcetmp.loop = false;
        _introAudioSourcetmp.playOnAwake = false;
        _loopAudioSourcetmp.clip = AudioClipLoop;
        _loopAudioSourcetmp.loop = true;
        _loopAudioSourcetmp.playOnAwake = false;
        _introAudioSourcetmp.volume = 0;
        _loopAudioSourcetmp.volume = 0;
        this.volume = volume;
        this.AudioClipIntro = AudioClipIntro;
        FadeInSecond = 1.0;
        _isChange = true;
    }
    // Update is called once per frame
    void Update()
    {
        
        if (CheckIntroNull() || CheckLoopNull()) return;
       
        if (_isFadeIn)
        {
            FadeDeltaTime += Time.deltaTime;
            if(FadeDeltaTime >= FadeInSecond)
            {
                FadeDeltaTime = FadeInSecond;
                _isFadeIn = false;
            }
            _introAudioSource.volume = (float)(FadeDeltaTime / FadeInSecond) * volume;
            _loopAudioSource.volume = (float)(FadeDeltaTime / FadeInSecond) * volume;
        }
        else if (_isFadeOut)
        {
            
            FadeDeltaTime += Time.deltaTime;
            if (FadeDeltaTime >= FadeInSecond)
            {
                FadeDeltaTime = FadeInSecond;
                _isFadeOut = false;
            }
            _introAudioSource.volume = (1f- (float)(FadeDeltaTime / FadeInSecond))  *volume;
            _loopAudioSource.volume = (1f-(float)(FadeDeltaTime / FadeInSecond))*volume;
            if(_isChange && FadeDeltaTime == FadeInSecond)
            {
                _introAudioSource = _introAudioSourcetmp;
                _loopAudioSource  = _loopAudioSourcetmp;
                _isFadeIn = true;
            }
        }

     
    }

    public void ChangeVolume(float v)
    {
        if (CheckIntroNull() || CheckLoopNull()) return;
        _introAudioSource.volume =  v;
        _loopAudioSource.volume =  v;
        this.volume = v;
        
    }
    public void Play()
    {
        // クリップが設定されていない場合は何もしない
        if (_introAudioSource == null || _loopAudioSource == null) return;

        // Pause 中は isPlaying は false
        // 標準機能だけでは一時停止中か判別不可能
        if (_isPause)
        {
            _introAudioSource.UnPause();
            if (_introAudioSource.isPlaying)
            {
                // イントロ中ならループ開始時間を残り時間で再設定
                _loopAudioSource.Stop();
                _loopAudioSource.PlayScheduled(AudioSettings.dspTime + AudioClipIntro.length - _introAudioSource.time);
            }
            else
            {
                   // WebGL 以外は UnPause するだけ
                    _loopAudioSource.UnPause();
     
            }
        }
        else if (IsPlaying == false)
        {
            // 最初から再生
            Stop();
            _introAudioSource.Play();

            // イントロの時間が経過した後に再生できるようにする
            // 設定する時間はゲーム刑か時間での設定となる
            _loopAudioSource.PlayScheduled(AudioSettings.dspTime + AudioClipIntro.length);
        }
        _isFadeIn = true;
        _isPause = false;
        FadeDeltaTime = 0;
    }

    public void Pause()
    {
        if (_introAudioSource == null || _loopAudioSource == null) return;

        _introAudioSource.Pause();
        _loopAudioSource.Pause();    
        _isPause = true;
    }

    /// <summary>BGM を停止します。</summary>
    public void Stop()
    {
        if (_introAudioSource == null || _loopAudioSource == null) return;

        _introAudioSource.Stop();
        _loopAudioSource.Stop();

        _isPause = false;
    }

    public void FadeOut()
    {
        _isFadeOut = true;
        _isFadeIn = false;
        FadeDeltaTime = 0;
    }

}
