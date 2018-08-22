using System;
using UniRx;
using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using UnityEngine;

public class MarbleCtrl : MonoBehaviour, I_StrikeDetectable, I_Flickable
{
    private Rigidbody2D _rigidbody2D;
    [SerializeField] private float _flickPower;
    [Range(0.0f, 1.0f)] [SerializeField] private float _fliction;

    private FlickListener _flickListener;
    [SerializeField] private GameObject flickArrowRoot;
    [SerializeField] private GameObject flickArrow;
    [SerializeField] private float flickScalingMaxBounds;
    [SerializeField] private float flickMinSize;
    [SerializeField] private float flickMaxSize;
    private bool _isFlickEnd;

    [SerializeField] private GameObject hitWallVFX;
    [SerializeField] private AudioClip hitWallSE;

    public Subject<MarbleCtrl> OnStopFlickMove = new Subject<MarbleCtrl>();

    public bool IsFlickable { get; private set; }
    public bool IsDetectable
    {
        get { return !alreadyStrikeSmash; }
    }

    [SerializeField] private float stopThreshold;
    public bool IsStopMove => _rigidbody2D?.velocity.sqrMagnitude <= stopThreshold;

    [SerializeField] private GameObject strikeShotVfx;
    [SerializeField] private AudioClip strikeShotSE;
    private bool alreadyStrikeSmash;

    public void Init()
    {
        flickArrowRoot.gameObject.SetActive(value: false);
        _rigidbody2D = GetComponent<Rigidbody2D>();

        IsFlickable = false;
        _isFlickEnd = false;
        alreadyStrikeSmash = false;

        _flickListener = gameObject.AddComponent<FlickListener>();
        _flickListener.Init();
        _flickListener.OnStartFlick += OnStartFlick;
        _flickListener.OnEndFlick += OnEndFlick;
        _flickListener.OnFlicking += OnFlicking;

        Deactivate();
    }

    public void Move()
    {
        if (IsFlickable)
        {
            _flickListener.Move();
        }

        _rigidbody2D.velocity = _rigidbody2D.velocity * (1.0f - _fliction);
        if (_isFlickEnd && IsStopMove)
        {
            _isFlickEnd = false;
            OnStopFlickMove?.OnNext(this);
        }
    }

    public void TurnInit()
    {
        alreadyStrikeSmash = false;
    }

    public void Activate()
    {
        IsFlickable = true;
        GetComponent<SpriteRenderer>().color = Color.green;
    }

    public void Deactivate()
    {
        IsFlickable = false;
        GetComponent<SpriteRenderer>().color = Color.blue;
    }

    public void SetVelocity(Vector3 newVelocity)
    {
        _rigidbody2D.velocity = newVelocity;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        
        MarbleCtrl hitMarbleCtrl = other.gameObject.GetComponent<MarbleCtrl>();
        /** 反射 **/
        Vector3 normal = other.contacts[0].normal;
        if (other.contacts.Length > 0 && hitMarbleCtrl == null)
        {
            _rigidbody2D.velocity = Vector3.Reflect(_rigidbody2D.velocity, normal);
            GameObject hitWallVfxObj = Instantiate(hitWallVFX, transform.position, Quaternion.identity);
            Quaternion refrectRot = Quaternion.LookRotation(Vector3.forward, normal);
            hitWallVfxObj.transform.rotation = refrectRot;
            SoundCtrl.PlayOneShot(hitWallSE);
        }

        if (hitMarbleCtrl != null && _isFlickEnd)
        {
            hitMarbleCtrl.SetVelocity(normal * 0.4f);
        }
        
        /** ストライク時 **/
        I_StrikeDetectable strikeDetectable = other.gameObject.GetComponent<I_StrikeDetectable>();
        if (strikeDetectable != null)
        {
            if (strikeDetectable.IsDetectable == false)
                return;
            strikeDetectable.StrikeDetect();
        }

        I_Damageable damageable = other.gameObject.GetComponent<I_Damageable>();
        if (damageable != null)
        {
            if (damageable.IsDamageable)
            {
                DamageInfo damageInfo = new DamageInfo(damageValue: 10.0f, attacker: this.gameObject);
                damageable.ApplyDamage(damageInfo: damageInfo);
            }
        }
    }

    public void StrikeDetect()
    {
        if (alreadyStrikeSmash)
        {
            return;
        }
        GameObject strikeShotVfxObj = Instantiate(strikeShotVfx, transform.position, Quaternion.identity);
        SoundCtrl.PlayOneShot(strikeShotSE);
        alreadyStrikeSmash = true;
    }

    public void OnStartFlick(Vector3 startPos)
    {
        flickArrowRoot.gameObject.SetActive(value: true);
        _isFlickEnd = false;
    }

    public void OnFlicking(Vector3 flickingPos)
    {
        /** フリックアローのスケーリング **/
        Vector3 diff = flickingPos - transform.position;
        float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        angle += 90.0f;
        flickArrowRoot.transform.rotation = Quaternion.Euler(euler: new Vector3(0.0f, 0.0f, angle));

        float distToMouse = (flickingPos - transform.position).sqrMagnitude;
        distToMouse = distToMouse - 100.0f;
        float distToMouseRatio = distToMouse / flickScalingMaxBounds;

        float scalingSize = Mathf.Lerp(flickMinSize, flickMaxSize, distToMouseRatio);
        Vector3 scale = Vector3.one;
        scale.y = scalingSize;
        flickArrow.transform.localScale = scale;
    }

    public void OnEndFlick(FlickData flickData)
    {
        /** フリック処理の後片付け **/
        _rigidbody2D.velocity = flickData.FlickDirection * -_flickPower;
        flickArrow.transform.localScale = Vector3.one;
        flickArrowRoot.gameObject.SetActive(value: false);
        _isFlickEnd = true;
        IsFlickable = false;
    }
}