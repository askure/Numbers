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
    private static bool isLoad = false;// ���g�����łɃ��[�h����Ă��邩�𔻒肷��t���O
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
        { // ���łɃ��[�h����Ă�����
            Destroy(this.gameObject); // �������g��j�����ďI��
            return;
        }
        isLoad = true; // ���[�h����Ă��Ȃ�������A�t���O�����[�h�ς݂ɐݒ肷��
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

            Debug.Log("BGM���ݒ肳��ĂȂ���");
            return;
        }
        if (_introAudioSource != null && _loopAudioSource != null)
            if ((AudioClipIntro == _introAudioSource.clip) && (AudioClipLoop == _loopAudioSource.clip)) return;
        // AudioSource �����g�ɒǉ�
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
        // �N���b�v���ݒ肳��Ă��Ȃ��ꍇ�͉������Ȃ�
        if (_introAudioSource == null || _loopAudioSource == null) return;

        // Pause ���� isPlaying �� false
        // �W���@�\�����ł͈ꎞ��~�������ʕs�\
        if (_isPause)
        {
            _introAudioSource.UnPause();
            if (_introAudioSource.isPlaying)
            {
                // �C���g�����Ȃ烋�[�v�J�n���Ԃ��c�莞�ԂōĐݒ�
                _loopAudioSource.Stop();
                _loopAudioSource.PlayScheduled(AudioSettings.dspTime + AudioClipIntro.length - _introAudioSource.time);
            }
            else
            {
                   // WebGL �ȊO�� UnPause ���邾��
                    _loopAudioSource.UnPause();
     
            }
        }
        else if (IsPlaying == false)
        {
            // �ŏ�����Đ�
            Stop();
            _introAudioSource.Play();

            // �C���g���̎��Ԃ��o�߂�����ɍĐ��ł���悤�ɂ���
            // �ݒ肷�鎞�Ԃ̓Q�[���Y�����Ԃł̐ݒ�ƂȂ�
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

    /// <summary>BGM ���~���܂��B</summary>
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
