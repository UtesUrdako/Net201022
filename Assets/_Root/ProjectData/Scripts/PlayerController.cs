using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private GameObject _cameraGameObject;
    [SerializeField] private Canvas _playerHPCanvas;
    [SerializeField] private TMP_Text _playerHPText;

    private CanvasHPController _canvasHPController;

    private float _rotateSpeed = 2;
    private float _moveSpeed = 15;

    private float angleX = 0;
    private float angleY = 0;

    private int HP = 0;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(HP);
        }
        else
        {
            HP = (int)stream.ReceiveNext();
            _playerHPText.text = HP.ToString();
        }
    }

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();

        _cameraGameObject.SetActive(photonView.IsMine);

        _playerHPCanvas.worldCamera = Camera.main;

        _canvasHPController = FindObjectOfType<CanvasHPController>();

        if (photonView.IsMine)
            _canvasHPController.ActionOnHPUpdate += OnHpUpdate;
    }

    private void OnDestroy()
    {
        _canvasHPController.ActionOnHPUpdate -= OnHpUpdate;
    }

    private void OnHpUpdate(int HPvalue)
    {
        HP = HPvalue;
        _playerHPText.text = HP.ToString();
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            UpdateController();
        }
    }

    private void UpdateController()
    {
        angleX += Input.GetAxis("Mouse X") * _rotateSpeed;
        angleY -= Input.GetAxis("Mouse Y") * _rotateSpeed;

        transform.rotation = Quaternion.Euler(0, angleX, 0);
        _cameraTransform.localRotation = Quaternion.Euler(angleY, 0, 0);

        //transform.Rotate(0, Input.GetAxis("Horizontal") * _rotateSpeed, 0);

        float moveX = Input.GetAxis("Horizontal") * _moveSpeed * Time.deltaTime;
        float moveY = Input.GetAxis("Vertical") * _moveSpeed * Time.deltaTime;

        _characterController.Move(transform.forward * moveY + transform.right * moveX);
    }
}
